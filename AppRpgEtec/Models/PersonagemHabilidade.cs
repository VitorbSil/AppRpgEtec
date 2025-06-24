using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Models
{
    public class PersonagemHabilidade
    {
        public int PersonagemId { get; set; }
        public Personagem Personagem { get; set; }

        public int HabilidadeId { get; set; }
        public Habilidade Habilidade { get; set; }

        public string HabilidadeNome
        {
            get { return Habilidade.Nome; }
        }




    }
}
