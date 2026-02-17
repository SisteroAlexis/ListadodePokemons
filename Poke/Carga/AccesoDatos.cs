using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Carga
{
    public class AccesoDatos
    {
        //Atributos

        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        //propiedad
        public SqlDataReader Lector 
        {
            get { return lector; }
        

        }

        //Constructor

        public AccesoDatos ()
        {
            try
            {
                conexion = new SqlConnection("server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security = true");
                comando = new SqlCommand();
            }
            catch (Exception)
            {

                throw;
            }
            

        }

        //metodo para setear consulta

        public void Consulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;

        }

        public void ejecutarLectura ()//EjecutarConsulta -- el otro es  EjecutarAccion
        {

            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Insertar()//EjecutarAccion -- el otro es EjecutarConsulta
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                //lo de abajo ejecuta un insert
                comando.ExecuteNonQuery(); 


            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SetearParametro(string nombre,object valor)
        {
            comando.Parameters.AddWithValue(nombre,valor);

        }

        public void CerrarConeccion ()
        { 
            if (lector != null)
                lector.Close();
            conexion.Close(); 
        }

    }
}
