# Calculadora_Notas 📊

Una aplicación de escritorio para Windows que te ayuda a gestionar tus cursos, registrar tus calificaciones por unidad y proyectar las notas necesarias para alcanzar tus objetivos académicos.

## 🚀 Características Principales

- **Registro Académico:** Administra tus cursos y notas detalladas por unidad.
- **Proyecciones Inteligentes:** Calcula tus promedios aplicando las reglas de tu facultad y te indica exactamente cuánto necesitas en la siguiente evaluación para aprobar o alcanzar tu nota objetivo.
- **Persistencia Local:** Los datos se guardan de forma segura en una base de datos **SQLite**, por lo que no perderás tu información al cerrar la aplicación.
- **Interfaz Intuitiva:** Diseñada en Windows Forms con estilos limpios y componentes personalizados.

---

## 🛠️ Tecnologías Utilizadas

- **Lenguaje:** C# (.NET)
- **Interfaz Gráfica:** Windows Forms (WinForms)
- **Base de Datos:** SQLite
- **Mapeador / Acceso a Datos:** ADO.NET / SQLite Core

---

## 📂 Estructura del Proyecto

El código está organizado siguiendo una arquitectura limpia y separada por responsabilidades:

```text
CalculadoraNotas/
├── Data/
│   ├── Conexion.cs       → Configuración de ruta, conexión y creación automática de tablas.
│   └── Repositorio.cs    → Operaciones CRUD de la base de datos (agregar, listar, eliminar).
├── Models/
│   ├── Curso.cs          → Modelo de datos para las asignaturas.
│   ├── Nota.cs           → Modelo de datos para las calificaciones.
│   └── Calculos.cs       → Lógica de negocio (redondeo, promedios y nota necesaria).
├── Forms/
│   ├── Estilo.cs         → Configuración visual (colores, diseño de botones y tablas).
│   ├── FormCursos.cs     → Ventana para la gestión de cursos.
│   ├── FormNotas.cs      → Ventana para el registro de calificaciones.
│   └── FormResultados.cs → Ventana de reportes y proyecciones.
├── Form1.cs              → Menú principal y contenedor de la aplicación.
└── Program.cs            → Punto de entrada del programa.
```

---

## 💻 Instalación y Configuración

Para clonar y ejecutar este proyecto localmente, asegúrate de tener instalado **Visual Studio** (con la carga de trabajo de desarrollo de escritorio de .NET).

1. **Clona el repositorio:**
   ```bash
   git clone https://github.com
   ```

2. **Abre el proyecto:**
   - Abre Visual Studio.
   - Selecciona **Abrir un proyecto o una solución**.
   - Busca y selecciona el archivo `.sln` en la carpeta raíz del proyecto.

3. **Restaura los paquetes NuGet:**
   - Ve a `Herramientas` > `Administrador de paquetes NuGet` > `Restaurar paquetes NuGet` (esto descargará automáticamente las dependencias de SQLite).

4. **Ejecuta la aplicación:**
   - Presiona `F5` o el botón **Iniciar** en Visual Studio.

---

## 📝 Licencia

Este proyecto está bajo la Licencia MIT. Consúltala para más detalles.
