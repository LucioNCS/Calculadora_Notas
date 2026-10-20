using CalculadoraNotas.Models;

namespace CalculadoraNotas.Data
{
    public static class Repositorio
    {
        private static void Ejecutar(string sql, params (string nombre, object valor)[] ps)
        {
            using var con = Conexion.Crear();
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = sql;
            foreach (var (n, v) in ps)
                cmd.Parameters.AddWithValue(n, v);
            cmd.ExecuteNonQuery();
        }

        // ---------- CURSOS ----------
        public static List<Curso> ListarCursos()
        {
            var lista = new List<Curso>();
            using var con = Conexion.Crear();
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Unidades, NotaObjetivo FROM Cursos ORDER BY Nombre";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Curso
                {
                    Id = r.GetInt32(0),
                    Nombre = r.GetString(1),
                    Unidades = r.GetInt32(2),
                    NotaObjetivo = r.GetDouble(3)
                });
            }
            return lista;
        }

        public static void AgregarCurso(string nombre, int unidades, double notaObjetivo)
        {
            Ejecutar(
                "INSERT INTO Cursos (Nombre, Unidades, NotaObjetivo) VALUES ($n, $u, $o)",
                ("$n", nombre), ("$u", unidades), ("$o", notaObjetivo));
        }

        public static void EliminarCurso(int id)
        {
            Ejecutar("DELETE FROM Notas WHERE CursoId = $id", ("$id", id));
            Ejecutar("DELETE FROM Cursos WHERE Id = $id", ("$id", id));
        }

        // ---------- NOTAS ----------
        // Devuelve las notas de TODAS las unidades del curso
        public static List<Nota> ListarNotas(int cursoId)
        {
            var lista = new List<Nota>();
            using var con = Conexion.Crear();
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText =
                "SELECT Id, CursoId, Unidad, Nombre, Valor, Peso FROM Notas " +
                "WHERE CursoId = $c ORDER BY Unidad, Id";
            cmd.Parameters.AddWithValue("$c", cursoId);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Nota
                {
                    Id = r.GetInt32(0),
                    CursoId = r.GetInt32(1),
                    Unidad = r.GetInt32(2),
                    Nombre = r.GetString(3),
                    Valor = r.GetDouble(4),
                    Peso = r.GetDouble(5)
                });
            }
            return lista;
        }

        public static void AgregarNota(int cursoId, int unidad, string nombre, double valor, double peso)
        {
            Ejecutar(
                "INSERT INTO Notas (CursoId, Unidad, Nombre, Valor, Peso) " +
                "VALUES ($c, $u, $n, $v, $p)",
                ("$c", cursoId), ("$u", unidad), ("$n", nombre), ("$v", valor), ("$p", peso));
        }

        public static void EliminarNota(int id)
        {
            Ejecutar("DELETE FROM Notas WHERE Id = $id", ("$id", id));
        }
    }
}