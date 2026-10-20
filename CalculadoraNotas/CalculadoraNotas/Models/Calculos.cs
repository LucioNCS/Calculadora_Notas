namespace CalculadoraNotas.Models
{
    public static class Calculos
    {
        public const int NotaAprobatoria = 11;

        // Redondeo: 12.5 -> 13, 13.5 -> 14
        public static int Redondear(double x)
            => (int)Math.Round(x + 1e-9, MidpointRounding.AwayFromZero);

        // Suma de pesos registrados en una unidad (%)
        public static double PesoUnidad(List<Nota> notas, int unidad)
            => notas.Where(n => n.Unidad == unidad).Sum(n => n.Peso);

        // Puntos que llevas en la unidad (sobre 20, sin redondear)
        public static double PuntosUnidad(List<Nota> notas, int unidad)
            => notas.Where(n => n.Unidad == unidad).Sum(n => n.Valor * n.Peso / 100.0);

        // La unidad está completa cuando sus pesos suman 100%
        public static bool UnidadCompleta(List<Nota> notas, int unidad)
            => Math.Abs(PesoUnidad(notas, unidad) - 100.0) < 0.001;

        // Nota de la unidad ya redondeada
        public static int NotaUnidad(List<Nota> notas, int unidad)
            => Redondear(PuntosUnidad(notas, unidad));

        // Promedio final: promedio de las notas de unidad (ya redondeadas), redondeado.
        // Devuelve null si alguna unidad aún no está completa.
        public static int? PromedioFinal(List<Nota> notas, int unidades)
        {
            int suma = 0;
            for (int u = 1; u <= unidades; u++)
            {
                if (!UnidadCompleta(notas, u)) return null;
                suma += NotaUnidad(notas, u);
            }
            return Redondear((double)suma / unidades);
        }

        // Nota que necesitas en la siguiente evaluación de 'unidad' para llegar
        // al promedio final objetivo. Las otras unidades incompletas se asumen
        // en el objetivo.
        public static double NotaNecesaria(List<Nota> notas, int unidades, int unidad,
                                           double objetivo, double pesoSiguiente)
        {
            // El final redondeado llega al objetivo si el promedio >= objetivo - 0.5
            double sumaRequerida = unidades * (objetivo - 0.5);

            double otras = 0;
            for (int u = 1; u <= unidades; u++)
            {
                if (u == unidad) continue;
                otras += UnidadCompleta(notas, u) ? NotaUnidad(notas, u) : objetivo;
            }

            // Nota entera que debe tener esta unidad
            double notaUnidadNecesaria = Math.Ceiling(sumaRequerida - otras - 1e-9);

            // Para que se redondee a esa nota, los puntos deben ser >= nota - 0.5
            double puntosNecesarios = notaUnidadNecesaria - 0.5;

            return (puntosNecesarios - PuntosUnidad(notas, unidad)) * 100.0 / pesoSiguiente;
        }
    }
}