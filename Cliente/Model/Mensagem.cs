using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Model
{
    public class Mensagem
    {
        public int codigoOperacao { get; set; }
        public string nomeVendedor { get; set; }
        public int codigoLoja { get; set; }
        public DateTime dataVenda { get; set; }
        public double valorVendido { get; set; }
        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }
    }
}
