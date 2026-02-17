using Carga;
using Domini;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;



namespace Ejemplo1
{
    public partial class AltaPokemon : Form
    {
        //creamos un pokemon nulo //para que no se carge los datos cuando ponemos agregar
       private Pokemon pokemon = null; //esto seria un atributo tambien 

        //creamos un atributo para la carga de archivos
        private OpenFileDialog archivo = null;

        //sobrecarga de constructor
        public AltaPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;
            Text = "Modificar Pokemon";
        }
        public AltaPokemon()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
           // Pokemon poke = new Pokemon();
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                if (pokemon == null) pokemon = new Pokemon();
                pokemon.Numero = Convert.ToInt32(txtNumero.Text);
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                pokemon.UrlImagen = txtImagen.Text;
                pokemon.elemento = (Elementos)cbTipo.SelectedItem;
                pokemon.Debilidad = (Elementos)cbDebilidad.SelectedItem;

                if (pokemon.Id != 0)
                {
                    negocio.Modificar(pokemon);
                    MessageBox.Show("Modificado exitosamente");

                }
                else
                {
                    
                   

                    negocio.Agregar(pokemon);
                    MessageBox.Show("Agregado exitosamente");
                }

                try
                {
                    if (archivo != null && !(txtImagen.Text.ToUpper().Contains("HTTP")))
                    {
                        File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void AltaPokemon_Load(object sender, EventArgs e)
        {
            //cargar los datos correspondientes en los comboBox
            ElementosNegocio elemen = new ElementosNegocio();

            try
            {
                //agrego los values y display para poder configurar los desplegables mejor.
                cbTipo.DataSource = elemen.listar();
                cbTipo.ValueMember = "Id";
                cbTipo.DisplayMember = "Descripcion";
                cbDebilidad.DataSource = elemen.listar();
                cbDebilidad.ValueMember = "Id";
                cbDebilidad.DisplayMember = "Descripcion";

                if (pokemon != null)
                {
                    txtNumero.Text = Convert.ToString(pokemon.Numero);
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtImagen.Text = pokemon.UrlImagen;
                    CargarImagen(pokemon.UrlImagen);
                    //los desplegables tienen un manejor particular
                    cbTipo.SelectedValue = pokemon.elemento.Id;
                    cbDebilidad.SelectedValue = pokemon.Debilidad.Id;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            

          
        }

        private void CargarImagen(string imagen)
        {
            try
            {

                pbImagen.Load(imagen);
            }
            catch (Exception ex)
            {
                pbImagen.Load("https://static.vecteezy.com/system/resources/previews/016/916/479/original/placeholder-icon-design-free-vector.jpg");
            }
        }
        private void txtImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtImagen.Text);
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            //creamos un objeto OpenFileDialog
             archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|Png|*.Png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagen.Text = archivo.FileName;
                CargarImagen(archivo.FileName);

                //guardo la imagen en una carpeta
                // File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                //la quiero guardar cuando cargo el pokemon
            }
        }

        private void txtImagen_Leave_1(object sender, EventArgs e)
        {

        }
    }
}
