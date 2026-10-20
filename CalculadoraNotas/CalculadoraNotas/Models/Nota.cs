namespace CalculadoraNotas.Models
{
    public class Nota
    {
        public int Id { get; set; }
        public int CursoId { get; set; }
        public int Unidad { get; set; }      // 1, 2, 3...
        public string Nombre { get; set; } = "";
        public double Valor { get; set; }    // nota de 0 a 20
        public double Peso { get; set; }     // porcentaje dentro de la unidad (0 a 100)
    }
}