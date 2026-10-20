namespace CalculadoraNotas.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public int Unidades { get; set; } = 2;
        public double NotaObjetivo { get; set; } = 11;
    }
}