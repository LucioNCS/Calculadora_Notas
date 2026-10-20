using Microsoft.Data.Sqlite;

namespace CalculadoraNotas.Data
{
    public static class Conexion
    {
        // La base se guarda en Documentos\CalculadoraNotas\notas.db
        public static string Carpeta = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "CalculadoraNotas");

        public static string Ruta = Path.Combine(Carpeta, "notas.db");

        public static SqliteConnection Crear()
        {
            return new SqliteConnection($"Data Source={Ruta}");
        }

        public static bool Probar()
        {
            try
            {
                using var con = Crear();
                con.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Crea la carpeta y las tablas si no existen (se llama al iniciar la app)
        public static void Inicializar()
        {
            Directory.CreateDirectory(Carpeta);

            using var con = Crear();
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Cursos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT NOT NULL,
                    Unidades INTEGER NOT NULL DEFAULT 2,
                    NotaObjetivo REAL NOT NULL DEFAULT 11
                );
                CREATE TABLE IF NOT EXISTS Notas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CursoId INTEGER NOT NULL,
                    Unidad INTEGER NOT NULL DEFAULT 1,
                    Nombre TEXT NOT NULL,
                    Valor REAL NOT NULL,
                    Peso REAL NOT NULL,
                    FOREIGN KEY (CursoId) REFERENCES Cursos(Id) ON DELETE CASCADE
                );";
            cmd.ExecuteNonQuery();
        }
    }
}