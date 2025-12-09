using System;
using System.Data;
using CapaDatos;
using System.Text.RegularExpressions; // Necesario para limpiar el teléfono

namespace CapaLogica
{
    // Esta clase realiza las validaciones y coordina las operaciones
    public class CapaNegocio
    {
        // Instancia de la Capa de Datos (DAL)
        private CapaDatos.CapaDatos _datos = new CapaDatos.CapaDatos();

        // [MÉTODOS CRUD OMITIDOS POR BREVEDAD, SON CORRECTOS]
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
        // [FIN MÉTODOS CRUD OMITIDOS]

        // 5. Validación centralizada
        private string ValidarDatos(Propietario prop)
        {
            // 1. Validaciones de campos de texto no vacío [cite: 81, 82, 83, 84]
            if (string.IsNullOrWhiteSpace(prop.Nombre)) return "El Nombre no puede estar vacío. [cite: 81]";
            if (string.IsNullOrWhiteSpace(prop.Apellido)) return "El Apellido no puede estar vacío. [cite: 82]";
            if (string.IsNullOrWhiteSpace(prop.Torre) || prop.Torre == "Seleccione") return "Debe seleccionar una Torre. [cite: 83]";
            if (string.IsNullOrWhiteSpace(prop.NumeroDepartamento)) return "El Número de departamento no puede estar vacío. [cite: 84]";

            // --- SECCIÓN DE VALIDACIÓN Y LIMPIEZA DE TELÉFONO ---

            // 1. Limpiar el teléfono: Eliminar espacios y símbolos (guiones, etc.) para asegurar que solo queden dígitos.
            string telefonoLimpio = Regex.Replace(prop.Telefono.Trim(), @"[^\d]", "");

            // 2. Validación de Teléfono NO VACÍO después de la limpieza.
            // Si el usuario ingresó solo caracteres no numéricos (ej. "---"), telefonoLimpio será vacío.
            if (string.IsNullOrWhiteSpace(telefonoLimpio))
            {
                return "El Teléfono no puede estar vacío.";
            }

            // 3. Verificar longitud mínima (Requerimiento 85)
            if (telefonoLimpio.Length < 8) return "El Teléfono debe tener al menos 8 dígitos. [cite: 85]";

            // 4. Intentar parsear a long para verificar si es numérico
            // Esta verificación es redundante si solo hay dígitos, pero añade robustez contra valores extremos.
            if (!long.TryParse(telefonoLimpio, out long telefonoNumerico))
            {
                return "El Teléfono debe ser un valor numérico válido.";
            }

            // 5. Reasignar el valor limpio al objeto para que sea guardado en la DB
            prop.Telefono = telefonoLimpio;

            return string.Empty;
        }
    }
}