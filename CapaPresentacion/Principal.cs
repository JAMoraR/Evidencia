using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                 "¿Desea salir de la aplicación?",
                 "Confirmar salida",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question,
                 MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            Formulario form = new Formulario();
            form.Show();
            this.Hide();
        }

        private void frmCondominio_Load(object sender, EventArgs e)
        {

        }
    }
}
