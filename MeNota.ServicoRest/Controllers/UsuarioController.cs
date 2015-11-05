using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MeNota.ServicoRest.Controllers
{
    public class UsuarioController : ApiController
    {

        // GET api/usuario
        public IEnumerable<Models.Usuario> Get(int? id = null, string nome = null, int? grupo = null)
        {
            using (var c = new Models.DbMeNotaDataContext())
            {

                var r = from u in c.Usuarios
                        select u;

                if (id.HasValue)
                {
                    r = r.Where(u => u.Id == id);
                }
                else if (!string.IsNullOrEmpty(nome))
                {
                    var strNome = nome.Trim().ToLower();
                    r = r.Where(u => u.Nome.ToLower().Contains(strNome));
                }
                else if (grupo.HasValue)
                {
                    r = r.Where(u=>u.GrupoUsuarios.SingleOrDefault(g=>g.IdGrupo == grupo) != null);
                }

                return r.ToList();
            }
        }

        // POST api/usuario
        public void Post([FromBody] string value)
        {
            List<Models.Usuario> lst = JsonConvert.DeserializeObject<List<Models.Usuario>>(value);
            using (var c = new Models.DbMeNotaDataContext())
            {
                c.Usuarios.InsertAllOnSubmit(lst);
                c.SubmitChanges();
            }
        }

        // PUT api/usuario/5
        public void Put(int id, [FromBody] string value)
        {
            Models.Usuario x = JsonConvert.DeserializeObject<Models.Usuario>(value);
            using (var c = new Models.DbMeNotaDataContext())
            {
                Models.Usuario usr = c.Usuarios.SingleOrDefault(u=>u.Id == id);
                if (usr != null)
                {
                    usr.Nome = x.Nome;
                    usr.Url = x.Url;
                    c.SubmitChanges();
                }                
            }
        }

        // DELETE api/usuario/5
        public void Delete(int id)
        {
            using (var c = new Models.DbMeNotaDataContext())
            {
                Models.Usuario usr = c.Usuarios.SingleOrDefault(u => u.Id == id);
                c.GrupoUsuarios.DeleteAllOnSubmit(usr.GrupoUsuarios);
                c.Grupos.DeleteAllOnSubmit(usr.Grupos);
                c.Usuarios.DeleteOnSubmit(usr);
                c.SubmitChanges();
            }
        }
    }
}
