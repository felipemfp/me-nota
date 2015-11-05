using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeNota.Aplicativo.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int? IdAdm { get; set; }
    }
}
