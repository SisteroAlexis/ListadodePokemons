using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domini;
using System.Data.SqlClient;
using System.ComponentModel;

namespace Carga
{
    public class ElementosNegocio
    {
        public List <Elementos> listar()
        {
            List<Elementos> lista = new List<Elementos>();
            AccesoDatos datos = new AccesoDatos();
            try
            {

                datos.Consulta("Select Id,Descripcion from ELEMENTOS;");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Elementos aux = new Elementos();
                    aux.Id = datos.Lector.GetInt32(0);
                    aux.Descripcion = datos.Lector.GetString(1);
                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            { 
                datos.CerrarConeccion();
            
            
            }



           
        }
    }
}
