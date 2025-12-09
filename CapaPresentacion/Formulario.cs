using System;
using System.Data;
using System.Windows.Forms;
using CapaLogica; // ¡CORRECCIÓN! Usar el namespace CapaLogica
// using CapaDatos; // No es necesario incluir CapaDatos en la UI si se usa la arquitectura por capas estricta

namespace CapaPresentacion
{
    public partial class Formulario : Form
    {
        // Instancia de la Capa Lógica/Negocio (Asegúrate que CapaNegocio existe en CapaLogica)
        private CapaNegocio _negocio = new CapaNegocio();

        // Variable para almacenar el ID del registro seleccionado
        private int _idPropietarioSeleccionado = 0;

        public Formulario()
        {
            InitializeComponent();
            CargarTorres();
            CargarPropietarios();
        }

        // --- MÉTODOS DE SOPORTE ---

        private void CargarTorres()
        {
            // Asumiendo el control ComboBox como 'cmbTorre' (Requerimiento 51)
            if (this.Controls.Find("cmbTorre", true).Length > 0 && this.Controls.Find("cmbTorre", true)[0] is ComboBox cmbTorre)
            {
                cmbTorre.Items.Clear();
                cmbTorre.Items.AddRange(new string[] { "Seleccione", "A", "B", "C", "D" });
                cmbTorre.SelectedIndex = 0;
            }
        }

        private void CargarPropietarios() // Requerimiento 64
        {
            try
            {
                // El método de la BLL llama al Listar de la DAL
                var dt = _negocio.MostrarPropietarios();

                // Asumiendo que el DataGridView se llama dataGridView1
                if (dt != null)
                {
                    dataGridView1.DataSource = dt;

                    if (dataGridView1.Columns.Contains("IdPropietario"))
                    {
                        dataGridView1.Columns["IdPropietario"].Visible = false;
                    }
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Diseño legible
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista: " + ex.Message, "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos() // Requerimiento 58
        {
            _idPropietarioSeleccionado = 0;
            textBox1.Text = string.Empty; // Nombre
            textBox2.Text = string.Empty; // Apellido

            // Lógica para ComboBox/TextBox Torre
            if (this.Controls.Find("cmbTorre", true).Length > 0 && this.Controls.Find("cmbTorre", true)[0] is ComboBox cmbTorre)
            {
                cmbTorre.SelectedIndex = 0;
            }
            else
            {
                textBox3.Text = string.Empty; // Torre (si usas TextBox3)
            }

            textBox4.Text = string.Empty; // Número Departamento
            textBox5.Text = string.Empty; // Teléfono
        }

        // Mapea los datos del formulario a la Entidad Propietario (Asegúrate que se llama Propietario en CapaLogica)
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

            // Usamos la clase Propietario que debe estar en CapaLogica
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

        // --- EVENTOS DE CONTROLES ---

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Usar CellClick en lugar de CellContentClick para la selección de fila
        }

        // Botón Agregar (Requerimiento 62)
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Propietario nuevoPropietario = ObtenerDatosFormulario();
            string resultado = _negocio.InsertarPropietario(nuevoPropietario); // Incluye Validación (Requerimiento 79)

            if (resultado.Contains("exitosamente"))
            {
                MessageBox.Show(resultado, "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarPropietarios(); // Refrescar DataGridView (Requerimiento 63)
            }
            else
            {
                MessageBox.Show(resultado, "Error / Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        // Botón Volver al menú principal (Requerimiento 59)
        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Asumiendo que tu menú principal se llama Principal (clase)
            Principal principal = new Principal();
            principal.Show();
            this.Close();
        }

        private void Formulario_Load(object sender, EventArgs e)
        {
            // Código de inicialización de carga
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Asegúrate de que el clic es en una fila válida y no en el encabezado
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                // 1. Cargar el ID (clave para Actualizar/Eliminar)
                // Se valida que la celda no sea nula antes de intentar convertir
                if (fila.Cells["IdPropietario"].Value != DBNull.Value)
                {
                    _idPropietarioSeleccionado = Convert.ToInt32(fila.Cells["IdPropietario"].Value);
                }

                // 2. Cargar los datos en los campos de texto
                textBox1.Text = fila.Cells["Nombre"].Value.ToString();
                textBox2.Text = fila.Cells["Apellido"].Value.ToString();
                textBox4.Text = fila.Cells["NumeroDepartamento"].Value.ToString();
                textBox5.Text = fila.Cells["Telefono"].Value.ToString(); // Teléfono

                // 3. Cargar el valor de Torre en el ComboBox (Requerimiento 67)
                string torre = fila.Cells["Torre"].Value.ToString();

                // Busca el control cmbTorre (asumiendo que es un ComboBox)
                if (this.Controls.Find("cmbTorre", true).Length > 0 && this.Controls.Find("cmbTorre", true)[0] is ComboBox cmbTorre)
                {
                    cmbTorre.SelectedItem = torre;
                }
                else
                {
                    // Si usa TextBox3 para la Torre
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