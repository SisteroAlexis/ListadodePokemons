using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using Domini;
using System.Security.Cryptography.X509Certificates;

namespace Carga
{
    public class PokemonNegocio
    {

        AccesoDatos dat = new AccesoDatos();

        //metodos
        //lee reguistros de la bd
        public List<Pokemon> listar() //lectura de pokemons
        {
            List <Pokemon> lista = new List <Pokemon>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;  //no tiene constructor y no es necesario hacer new...

            try
            {
                //cadena de coneccion
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security = true"; //puedo usar . en lugar de el nombre de la compu
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select numero,nombre,p.Descripcion,UrlImagen,e.Descripcion as 'Elemento',d.Descripcion as 'Debilidad',e.Id,d.Id,p.Id from POKEMONS p\r\nInner Join ELEMENTOS e on p.IdTipo = e.Id\r\nInner Join ELEmentos d on p.IdDebilidad = d.Id Where p.Activo = 1";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Numero = lector.GetInt32(0);       //2 formas de trabajar
                    aux.Nombre = (string)lector["Nombre"]; //esta no tengo que poner int32 peor es mas larga
                    aux.Descripcion = lector.GetString(2);


                    //if (!(lector.IsDBNull(lector.GetOrdinal("UrlImagen"))))  //una forma de hacerlo

                    if (!(lector["UrlImagen"] is DBNull))
                    { aux.UrlImagen = lector.GetString(3); }



                    //esto es un objeto por asociacion por lo que tenemos que instanciarlo
                    aux.elemento = new Elementos();
                    aux.elemento.Descripcion = lector.GetString(4);
                    aux.Debilidad = new Elementos();
                    aux.Debilidad.Descripcion = lector.GetString(5);
                    aux.elemento.Id = lector.GetInt32(6);
                    aux.Debilidad.Id = lector.GetInt32(7);
                    aux.Id = lector.GetInt32(8);
                    lista.Add(aux);
                }
                conexion.Close(); //hay un lugar mejor donde ponerlo
                return lista;
            }
            catch (Exception ex)
            {

              

                throw ex;
            }  

           
        }


        public void Agregar(Pokemon nuevo)
        {

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.Consulta("Insert Into Pokemons (Numero,Nombre,Descripcion,UrlImagen,IdTipo,IdDebilidad,Activo)  Values (" + nuevo.Numero + ",'" + nuevo.Nombre + "','" + nuevo.Descripcion + "','" + nuevo.UrlImagen + "',@idelemento,@idDebilidad,1)");
                datos.SetearParametro("@idelemento", nuevo.elemento.Id);
                datos.SetearParametro("@idDebilidad", nuevo.Debilidad.Id);

                datos.Insertar();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public  void Modificar (Pokemon poke)
            {
                AccesoDatos datos = new AccesoDatos ();

                try
                {
                datos.Consulta("update POKEMONS set Numero = @numero,Nombre = @nombre,Descripcion=@descripcion,UrlImagen=@imagen,IdTipo= @elemento,IdDebilidad= @debilidad\r\nWhere Id =@id;");
                datos.SetearParametro("@numero", poke.Numero);
                datos.SetearParametro("@nombre", poke.Nombre);
                datos.SetearParametro("@descripcion", poke.Descripcion);
                datos.SetearParametro("@imagen", poke.UrlImagen);
                datos.SetearParametro("@elemento", poke.elemento.Id);
                datos.SetearParametro("@debilidad", poke.Debilidad.Id);
                datos.SetearParametro("@id", poke.Id);
                datos.ejecutarLectura ();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    datos.CerrarConeccion();
                }
            }

        public void EliminarFisico(Pokemon poke)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.Consulta("Delete from POKEMONS Where Id = @id;");
                datos.SetearParametro("@id",poke.Id);
                datos.Insertar(); //EjecutarAccion -- el otro es EjecutarConsulta

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConeccion ();
            }
        }

        public void EliminarLogico(Pokemon poke)
        {

            try
            {
                dat.Consulta("update POKEMONS set Activo = 0 where Id = @id;");
                dat.SetearParametro("@id",poke.Id);
                
                dat.Insertar();


            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { dat.CerrarConeccion(); }
        }
        
        public List<Pokemon> Filtrar (string campo,string criterio,string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>();


            try
            {

                string consul = ("Select numero,nombre,p.Descripcion,UrlImagen,e.Descripcion as 'Elemento',d.Descripcion as 'Debilidad',e.Id,d.Id,p.Id from POKEMONS p\r\nInner Join ELEMENTOS e on p.IdTipo = e.Id\r\nInner Join ELEmentos d on p.IdDebilidad = d.Id Where p.Activo = 1");

                if (campo == "Numero")
                {
                    switch (criterio)
                    {
                        case "Mayor a: ":
                            consul += " and Numero >" + filtro;
                            break;
                        case "Menor a: ":
                            consul += " and Numero <" + filtro;
                            break;
                        case "Igual a: ":
                            consul += " and Numero =" + filtro;
                            break;


                        default:
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con:  ":
                            consul += " and Nombre like '" + filtro + "%'";
                            break;
                        case "Termina con:  ":
                            consul += " and Nombre like '%" + filtro + "'";
                            break;
                        case "Contiene: ":
                            consul += " and Nombre like '%" + filtro + "%'";
                            break;


                        default:
                            break;

                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con:  ":
                            consul += " and Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con:  ":
                            consul += " and Descripcion like '%" + filtro +"'";
                            break;
                        case "Contiene: ":
                            consul += " and Descripcion like '%" + filtro + "%'";
                            break;


                        default:
                            break;

                    }
                }

                dat.Consulta(consul);
                dat.ejecutarLectura();

                while (dat.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Numero = dat.Lector.GetInt32(0);       //2 formas de trabajar
                    aux.Nombre = (string)dat.Lector["Nombre"]; //esta no tengo que poner int32 peor es mas larga
                    aux.Descripcion = dat.Lector.GetString(2);


                    //if (!(lector.IsDBNull(lector.GetOrdinal("UrlImagen"))))  //una forma de hacerlo

                    if (!(dat.Lector["UrlImagen"] is DBNull))
                    { aux.UrlImagen = dat.Lector.GetString(3); }



                    //esto es un objeto por asociacion por lo que tenemos que instanciarlo
                    aux.elemento = new Elementos();
                    aux.elemento.Descripcion = dat.Lector.GetString(4);
                    aux.Debilidad = new Elementos();
                    aux.Debilidad.Descripcion = dat.Lector.GetString(5);
                    aux.elemento.Id = dat.Lector.GetInt32(6);
                    aux.Debilidad.Id = dat.Lector.GetInt32(7);
                    aux.Id = dat.Lector.GetInt32(8);
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
                dat.CerrarConeccion();
            }

        }
    }
}
