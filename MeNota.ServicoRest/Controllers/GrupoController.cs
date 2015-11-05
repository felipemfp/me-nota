using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MeNota.ServicoRest.Controllers
{
    public class GrupoController : ApiController
    {
        private Models.DbMeNotaDataContext c
        {
            get
            {
                return new Models.DbMeNotaDataContext();
            }
        }

        // GET api/grupo
        public IEnumerable<Models.Grupo> Get(int? id = null, int? admin = null, int? usuario = null)
        {
            using (var c = new Models.DbMeNotaDataContext())
            {
                var r = from g in c.Grupos
                        select g;

                if (id.HasValue)
                {
                    r = r.Where(g => g.Id == id);
                }
                else if (admin.HasValue)
                {
                    r = r.Where(g => g.IdAdm == admin);
                }
                else if (usuario.HasValue) {
                    r = r.Where(g => g.GrupoUsuarios.SingleOrDefault(gu => gu.IdUsuario == usuario) != null);
                }

                return r.ToList();
            }
        }

        // POST api/grupo
        public void Post([FromBody] string value)
        {
            List<Models.Grupo> lst = JsonConvert.DeserializeObject<List<Models.Grupo>>(value);
            using (var c = new Models.DbMeNotaDataContext())
            {
                c.Grupos.InsertAllOnSubmit(lst);
                c.SubmitChanges();
                for (int i = 0, length = lst.Count; i < length; i++)
                {
                    c.GrupoUsuarios.InsertOnSubmit(new Models.GrupoUsuario { IdGrupo = lst[i].Id, IdUsuario = lst[i].IdAdm });
                }
                c.SubmitChanges();
            }
        }

        // PUT api/grupo/5
        public void Put(int id, [FromBody] string value)
        {
            Models.Grupo x = JsonConvert.DeserializeObject<Models.Grupo>(value);
            using (var c = new Models.DbMeNotaDataContext())
            {
                Models.Grupo grp = c.Grupos.SingleOrDefault(g => g.Id == id);
                grp.Descricao = x.Descricao;
                c.SubmitChanges();
            }
        }

        // DELETE api/grupo/5
        public void Delete(int id)
        {
            using (var c = new Models.DbMeNotaDataContext())
            {
                Models.Grupo grp = c.Grupos.SingleOrDefault(g => g.Id == id);
                c.GrupoUsuarios.DeleteAllOnSubmit(grp.GrupoUsuarios);
                c.Grupos.DeleteOnSubmit(grp);
                c.SubmitChanges();
            }
        }
    }
}
