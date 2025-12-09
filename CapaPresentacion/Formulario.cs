using System;
using System.Data;
using System.Windows.Forms;
using CapaLogica;

namespace CapaPresentacion
{
    public partial class Formulario : Form
    {
        private CapaNegocio _negocio = new CapaNegocio();

        private int _idPropietarioSeleccionado = 0;

        public Formulario()
        {
            InitializeComponent();
            CargarTorres();
            CargarPropietarios();
        }

        private void CargarTorres()
        {
            if (this.Controls.Find("cmbTorre", true).Length > 0 && this.Controls.Find("cmbTorre", true)[0] is ComboBox cmbTorre)
            {
                cmbTorre.Items.Clear();
                cmbTorre.Items.AddRange(new string[] { "Seleccione", "A", "B", "C", "D" });
                cmbTorre.SelectedIndex = 0;
            }
        }

        private void CargarPropietarios()
        {
            try
            {
                var dt = _negocio.MostrarPropietarios();

                if (dt != null)
                {
                    dataGridView1.DataSource = dt;

                    if (dataGridView1.Columns.Contains("IdPropietario"))
                    {
                        dataGridView1.Columns["IdPropietario"].Visible = false;
                    }
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista: " + ex.Message, "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            _idPropietarioSeleccionado = 0;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;

            if (this.Controls.Find("cmbTorre", true).Length > 0 && this.Controls.Find("cmbTorre", true)[0] is ComboBox cmbTorre)
            {
                cmbTorre.SelectedIndex = 0;
            }
            else
            {
                textBox3.Text = string.Empty;
            }

            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
        }

        private Propietario ObtenerDatosFormulario()
        {
            string torreValor = "";
            if (this.Controls.Find("cmbTorre", true).Length > 0 && this.Controls.Find("cmbTorre", true)[0] is ComboBox cmbTorre)
            {
                torreValor = cmbTorre.SelectedItem != null ? cmbTorre.SelectedItem.ToString() : string.Empty;
            }
            else
            {
                torreValor = textBox3.Text;
            }

            return new Propietario
            {
                IdPropietario = _idPropietarioSeleccionado,
                Nombre = textBox1.Text.Trim(),
                Apellido = textBox2.Text.Trim(),
                Torre = torreValor,
                NumeroDepartamento = textBox4.Text.Trim(),
                Telefono = textBox5.Text.Trim()
            };
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Propietario nuevoPropietario = ObtenerDatosFormulario();
            string resultado = _negocio.InsertarPropietario(nuevoPropietario);

            if (resultado.Contains("exitosamente"))
            {
                MessageBox.Show(resultado, "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarPropietarios();
            }
            else
            {
                MessageBox.Show(resultado, "Error / Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnVolver_Click(object sender, EventArgs e)
        {
            Principal principal = new Principal();
            principal.Show();
            this.Close();
        }

        private void Formulario_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                if (fila.Cells["IdPropietario"].Value != DBNull.Value)
                {
                    _idPropietarioSeleccionado = Convert.ToInt32(fila.Cells["IdPropietario"].Value);
                }

                textBox1.Text = fila.Cells["Nombre"].Value.ToString();
                textBox2.Text = fila.Cells["Apellido"].Value.ToString();
                textBox4.Text = fila.Cells["NumeroDepartamento"].Value.ToString();
                textBox5.Text = fila.Cells["Telefono"].Value.ToString();

                string torre = fila.Cells["Torre"].Value.ToString();

                if (this.Controls.Find("cmbTorre", true).Length > 0 && this.Controls.Find("cmbTorre", true)[0] is ComboBox cmbTorre)
                {
                    cmbTorre.SelectedItem = torre;
                }
                else
                {
                    textBox3.Text = torre;
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idPropietarioSeleccionado == 0)
            {
                MessageBox.Show("Debe seleccionar un propietario para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Propietario propietarioActualizado = ObtenerDatosFormulario();
            string resultado = _negocio.ActualizarPropietario(propietarioActualizado);

            if (resultado.Contains("exitosamente"))
            {
                MessageBox.Show(resultado, "Actualización Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarPropietarios();
            }
            else
            {
                MessageBox.Show(resultado, "Error / Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (_idPropietarioSeleccionado == 0)
            {
                MessageBox.Show("Debe seleccionar un propietario para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Confirma la eliminación permanente del registro?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string resultado = _negocio.EliminarPropietario(_idPropietarioSeleccionado);

                if (resultado.Contains("exitosamente"))
                {
                    MessageBox.Show(resultado, "Eliminación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarPropietarios();
                }
                else
                {
                    MessageBox.Show(resultado, "Error de Operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnVaciar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}