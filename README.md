# Calculadora_Notas
Una app de escritorio para Windows (C#, Windows Forms) donde registras tus cursos y notas por unidad. Calcula tus promedios con las reglas de tu facultad y te dice cuánto necesitas en la siguiente evaluación para llegar a tu nota objetivo. Los datos se guardan en SQLite y no se pierden al cerrar.
CalculadoraNotas/
├── Data/
│   ├── Conexion.cs       → ruta, conexión y creación de tablas
│   └── Repositorio.cs    → consultas (agregar, listar, eliminar)
├── Models/
│   ├── Curso.cs
│   ├── Nota.cs
│   └── Calculos.cs       → redondeo, promedios y nota necesaria
├── Forms/
│   ├── Estilo.cs         → colores, botones y tablas
│   ├── FormCursos.cs
│   ├── FormNotas.cs
│   └── FormResultados.cs
├── Form1.cs              → menú principal
└── Program.cs
