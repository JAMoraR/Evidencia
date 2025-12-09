namespace CapaLogica
{
    // Clase que representa la estructura de la tabla Propietarios (Requerimiento 22)
    public class Propietario
    {
        public int IdPropietario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Torre { get; set; }
        public string NumeroDepartamento { get; set; }
        public string Telefono { get; set; }
    }
}