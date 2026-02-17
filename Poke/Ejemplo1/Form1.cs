using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domini;
using Carga;

namespace Ejemplo1
{
    public partial class Form1 : Form
    {
        //creamos otra variable--recoordar que esto es un atributo
        private List<Pokemon> listaPokemon;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cargarr();
            cboCampo.Items.Add("Numero");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descrpcion");

        }

        private void Cargarr()
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                listaPokemon = negocio.listar();
                dgv1.DataSource = listaPokemon;
                OcultarColumnas();
                CargarImagen(listaPokemon[0].UrlImagen);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void OcultarColumnas()
        {
            dgv1.Columns[0].Visible = false;
            dgv1.Columns[4].Visible = false;
        }

        private void dgv1_SelectionChanged(object sender, EventArgs e)
        {

            if (dgv1.CurrentRow != null)
            {

            //la fila actual //Da el objeto enlasado
            Pokemon seleccionado = (Pokemon)dgv1.CurrentRow.DataBoundItem;
            //arriba genere un casteo explicito de pokemons y lo guarde en una nueva variable pokemons
            CargarImagen(seleccionado.UrlImagen);
             }
        }

        //aca creamos un metodo privado por si no existe la imagen que buscamos.
        //esto es para que no tire un error en ejecucion.
        private void CargarImagen(string imagen)
        {
            try
            {

                pbPokemon.Load(imagen);
            }
            catch (Exception ex)
            {
                pbPokemon.Load("https://static.vecteezy.com/system/resources/previews/016/916/479/original/placeholder-icon-design-free-vector.jpg");
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Form2 ventana = new Form2();
            ventana.Show();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AltaPokemon ventana = new AltaPokemon();
            ventana.ShowDialog();
            Cargarr();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
                           //caste explicito de pokemon
            seleccionado = (Pokemon)dgv1.CurrentRow.DataBoundItem; //current.Data... es la fila seleccionada.
            AltaPokemon ventana = new AltaPokemon(seleccionado);
            ventana.ShowDialog();
            Cargarr();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgv1.CurrentRow.DataBoundItem;
            try
            {

                DialogResult respuesta = MessageBox.Show("¿De verdad queres eliminar este pokemon?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
             if (respuesta == DialogResult.Yes)   { 
                negocio.EliminarFisico(seleccionado);
                Cargarr();
                }
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        private void btnEliminacionLogica_Click(object sender, EventArgs e)
        {
            eliminar(true);


        }

        private void eliminar (bool logico= false)
        {
            Pokemon poke;
            PokemonNegocio Negocio = new PokemonNegocio();

            try
            {
                poke = (Pokemon)dgv1.CurrentRow.DataBoundItem;


                DialogResult respuesta = MessageBox.Show("¿De verdad queres eliminar este pokemon?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {

                    if (logico)
                    {
                        Negocio.EliminarLogico(poke);

                    }
                    else { Negocio.EliminarFisico(poke); }
                        Cargarr();
                }
            }
            catch (Exception ex )
            {

                MessageBox.Show(ex.Message);
            }
        }


        private bool ValidarFiltro()
        {

            if (cboCampo.SelectedIndex == -1  ) 
            {
                MessageBox.Show("Ingrese porfavor el Campo");
                return true; }
            if (cboCriterio.SelectedIndex == -1)
            {
                MessageBox.Show("Ingrese porfavor el Criterio");
                return true;

            }
            if(string.IsNullOrEmpty(txtFiltroAvanzado.Text))
            {
                MessageBox.Show("Debes cargar el filtro para realizar una busqueda");
                return true;
            }
            if (cboCampo.Text == "Numero")
            {
                return SoloNumeros(txtFiltroAvanzado.Text);
            }
            

                return false;
        }

        private bool SoloNumeros (string cadena)
        {
            int aux;
            if (int.TryParse(cadena,out aux ))
            {

                return false;
            }else {
                MessageBox.Show("Ingrese solo numeros");
                return true; }

            
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
               if( ValidarFiltro()) { return ; }

            string campo = cboCampo.Text;
            string criterio = cboCriterio.Text;
            string filtro = txtFiltroAvanzado.Text;

                if (SoloNumeros(filtro)) { return; }
                
             dgv1.DataSource = negocio.Filtrar(campo,criterio,filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message) ;
            }

           
        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
       
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> ListaFiltrada;
            string filtro = txtFiltro.Text;



            if (filtro != "")
            {
                //filtro findAll busca y encuentra todos los objetos en la lista segun unos parametros.
                ListaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(txtFiltro.Text.ToUpper()) || x.elemento.Descripcion.ToUpper().Contains(txtFiltro.Text.ToUpper())); //requiere una exprecion landam
            }
            else { ListaFiltrada = listaPokemon; }

            dgv1.DataSource = null;
            dgv1.DataSource = ListaFiltrada;
            OcultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opc = cboCampo.SelectedItem.ToString();
            if (opc =="Numero" )
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a: ");
                cboCriterio.Items.Add("Menor a: ");
                cboCriterio.Items.Add("Igual a: ");

            }else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con:  ");
                cboCriterio.Items.Add("Termina con:  ");
                cboCriterio.Items.Add("Contiene: ");
            }
            
        }
    }
}
