using Domini;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Carga;


namespace Ejemplo1
{
    public partial class Form2 : Form
    {
        List<Elementos> listaElementos;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ElementosNegocio carga = new ElementosNegocio();
            listaElementos = carga.listar();
            dataGridView1.DataSource = listaElementos;
        }
    }
}
