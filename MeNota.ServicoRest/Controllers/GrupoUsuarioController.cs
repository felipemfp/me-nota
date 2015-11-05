using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MeNota.ServicoRest.Controllers
{
    public class GrupousuarioController : ApiController
    {
        // GET api/grupousuario
        public IEnumerable<Models.GrupoUsuario> Get(int? grupo = null, int? usuario = null)
        {
            using (var c = new Models.DbMeNotaDataContext())
            {
                var r = from u in c.GrupoUsuarios
                        select u;

                if (grupo.HasValue)
                {
                    r = r.Where(g => g.IdGrupo == grupo);
                }

                if (usuario.HasValue)
                {

                    r = r.Where(g => g.IdUsuario == usuario);
                }

                return r.ToList();
            }
        }

        // POST api/grupousuario
        public void Post([FromBody] string value)
        {
            List<Models.GrupoUsuario> lst = JsonConvert.DeserializeObject<List<Models.GrupoUsuario>>(value);
            using (var c = new Models.DbMeNotaDataContext())
            {
                c.GrupoUsuarios.InsertAllOnSubmit(lst);
                c.SubmitChanges();                
            }
        }

        // DELETE api/grupousuario?grupo=5&usuario=2
        public void Delete(int grupo, int usuario)
        {
            using (var c = new Models.DbMeNotaDataContext())
            {
                Models.GrupoUsuario grp = c.GrupoUsuarios.SingleOrDefault(g => g.IdGrupo == grupo && g.IdUsuario == usuario);
                if (grp != null)
                {
                    c.GrupoUsuarios.DeleteOnSubmit(grp);
                    c.SubmitChanges();
                }
            }
        }
    }
}
