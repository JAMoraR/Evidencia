using System;
using System.Data;
using CapaDatos;
using System.Text.RegularExpressions;

namespace CapaLogica
{
    public class CapaNegocio
    {
        private CapaDatos.CapaDatos _datos = new CapaDatos.CapaDatos();

        public DataTable MostrarPropietarios() { return _datos.ListarPropietarios(); }
        public string InsertarPropietario(Propietario prop)
        {
            string validacion = ValidarDatos(prop);
            if (!string.IsNullOrEmpty(validacion)) return validacion;
            try { _datos.InsertarPropietario(prop.Nombre, prop.Apellido, prop.Torre, prop.NumeroDepartamento, prop.Telefono); return "Propietario agregado exitosamente."; }
            catch (Exception ex) { return "Error en el negocio al insertar: " + ex.Message; }
        }
        public string ActualizarPropietario(Propietario prop)
        {
            string validacion = ValidarDatos(prop);
            if (!string.IsNullOrEmpty(validacion)) return validacion;
            if (prop.IdPropietario == 0) return "Debe seleccionar un propietario para actualizar.";
            try { _datos.ActualizarPropietario(prop.IdPropietario, prop.Nombre, prop.Apellido, prop.Torre, prop.NumeroDepartamento, prop.Telefono); return "Propietario actualizado exitosamente."; }
            catch (Exception ex) { return "Error en el negocio al actualizar: " + ex.Message; }
        }
        public string EliminarPropietario(int idPropietario)
        {
            if (idPropietario == 0) return "Debe seleccionar un propietario para eliminar.";
            try { _datos.EliminarPropietario(idPropietario); return "Propietario eliminado exitosamente."; }
            catch (Exception ex) { return "Error en el negocio al eliminar: " + ex.Message; }
        }

        private string ValidarDatos(Propietario prop)
        {
            if (string.IsNullOrWhiteSpace(prop.Nombre)) return "El Nombre no puede estar vacío. [cite: 81]";
            if (string.IsNullOrWhiteSpace(prop.Apellido)) return "El Apellido no puede estar vacío. [cite: 82]";
            if (string.IsNullOrWhiteSpace(prop.Torre) || prop.Torre == "Seleccione") return "Debe seleccionar una Torre. [cite: 83]";
            if (string.IsNullOrWhiteSpace(prop.NumeroDepartamento)) return "El Número de departamento no puede estar vacío. [cite: 84]";


            string telefonoLimpio = Regex.Replace(prop.Telefono.Trim(), @"[^\d]", "");

            if (string.IsNullOrWhiteSpace(telefonoLimpio))
            {
                return "El Teléfono no puede estar vacío.";
            }

            if (telefonoLimpio.Length < 8) return "El Teléfono debe tener al menos 8 dígitos. [cite: 85]";

            if (!long.TryParse(telefonoLimpio, out long telefonoNumerico))
            {
                return "El Teléfono debe ser un valor numérico válido.";
            }

            prop.Telefono = telefonoLimpio;

            return string.Empty;
        }
    }
}