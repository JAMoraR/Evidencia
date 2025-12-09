using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CapaDatos
    {
        private SqlConnection Conexion = new SqlConnection(
            "Server=JAMORAR\\SQLJAMORAR;" + // Cambiar servidor
            "Database=BD_Condominio;" + // Cambiar BD
            "User Id=JAMORAR;" + // Cambar usuario
            "Password=abc;" // Cambiar contraseña
            );

        public DataTable ListarPropietarios()
        {
            DataTable tabla = new DataTable();
            SqlCommand comando = new SqlCommand("SELECT IdPropietario, Nombre, Apellido, Torre, NumeroDepartamento, Telefono FROM Propietarios", Conexion);
            comando.CommandType = CommandType.Text;

            try
            {
                Conexion.Open();
                SqlDataReader leer = comando.ExecuteReader();
                tabla.Load(leer);
                Conexion.Close();
            }
            catch (Exception ex)
            {
                if (Conexion.State == ConnectionState.Open) Conexion.Close();
                throw new Exception("Error DAL al listar: " + ex.Message);
            }
            return tabla;
        }

        public void InsertarPropietario(string nombre, string apellido, string torre, string numDepto, string telefono)
        {
            SqlCommand comando = new SqlCommand("INSERT INTO Propietarios (Nombre, Apellido, Torre, NumeroDepartamento, Telefono) VALUES (@nombre, @apellido, @torre, @numDepto, @telefono)", Conexion);
            comando.CommandType = CommandType.Text;

            try
            {
                comando.Parameters.AddWithValue("@nombre", nombre);
                comando.Parameters.AddWithValue("@apellido", apellido);
                comando.Parameters.AddWithValue("@torre", torre);
                comando.Parameters.AddWithValue("@numDepto", numDepto);
                comando.Parameters.AddWithValue("@telefono", telefono);

                Conexion.Open();
                comando.ExecuteNonQuery();
                Conexion.Close();
            }
            catch (Exception ex)
            {
                if (Conexion.State == ConnectionState.Open) Conexion.Close();
                throw new Exception("Error DAL al insertar: " + ex.Message);
            }
        }

        public void ActualizarPropietario(int id, string nombre, string apellido, string torre, string numDepto, string telefono)
        {
            SqlCommand comando = new SqlCommand("UPDATE Propietarios SET Nombre = @nombre, Apellido = @apellido, Torre = @torre, NumeroDepartamento = @numDepto, Telefono = @telefono WHERE IdPropietario = @id", Conexion);
            comando.CommandType = CommandType.Text;

            try
            {
                comando.Parameters.AddWithValue("@id", id);
                comando.Parameters.AddWithValue("@nombre", nombre);
                comando.Parameters.AddWithValue("@apellido", apellido);
                comando.Parameters.AddWithValue("@torre", torre);
                comando.Parameters.AddWithValue("@numDepto", numDepto);
                comando.Parameters.AddWithValue("@telefono", telefono);

                Conexion.Open();
                comando.ExecuteNonQuery();
                Conexion.Close();
            }
            catch (Exception ex)
            {
                if (Conexion.State == ConnectionState.Open) Conexion.Close();
                throw new Exception("Error DAL al actualizar: " + ex.Message);
            }
        }

        public void EliminarPropietario(int id)
        {
            SqlCommand comando = new SqlCommand("DELETE FROM Propietarios WHERE IdPropietario = @id", Conexion);
            comando.CommandType = CommandType.Text;

            try
            {
                comando.Parameters.AddWithValue("@id", id);

                Conexion.Open();
                comando.ExecuteNonQuery();
                Conexion.Close();
            }
            catch (Exception ex)
            {
                if (Conexion.State == ConnectionState.Open) Conexion.Close();
                throw new Exception("Error DAL al eliminar: " + ex.Message);
            }
        }
    }
}