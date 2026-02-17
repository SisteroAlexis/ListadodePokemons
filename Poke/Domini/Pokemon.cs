using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domini
{
    public class Pokemon
    {
        //Atributos-Prop
        public int Id { get; set; }
        [DisplayName("Número")]
        public int Numero { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public string UrlImagen { get; set; }

        [DisplayName("Tipo")]
        public Elementos elemento { get; set; } 
        public Elementos Debilidad {  get; set; }
    }
}
