using CalculadoraNotas.Data;
using CalculadoraNotas.Models;

namespace CalculadoraNotas.Forms
{
    public class FormCursos : Form
    {
        private TextBox txtNombre = new();
        private NumericUpDown numUnidades = new();
        private NumericUpDown numObjetivo = new();
        private Button btnAgregar = new();
        private Button btnEliminar = new();
        private DataGridView grid = new();

        public FormCursos()
        {
            Text = "Cursos";
            Size = new Size(700, 470);
            StartPosition = FormStartPosition.CenterParent;
            Estilo.Formulario(this);

            // Fila superior: los controles se acomodan solos (no se tapan)
            var fila = new FlowLayoutPanel
            {
                Location = new Point(5, 10),
                Size = new Size(660, 45),
                WrapContents = false
            };

            txtNombre.Width = 150;

            numUnidades.Width = 50;
            numUnidades.Minimum = 1;
            numUnidades.Maximum = 6;
            numUnidades.Value = 2;

            numObjetivo.Width = 55;
            numObjetivo.Minimum = 0;
            numObjetivo.Maximum = 20;
            numObjetivo.DecimalPlaces = 0;
            numObjetivo.Value = 11;

            btnAgregar.Text = "Agregar";
            btnAgregar.Size = new Size(85, 29);
            btnAgregar.Margin = new Padding(12, 3, 3, 3);
            Estilo.Boton(btnAgregar, Estilo.Principal);
            btnAgregar.Click += (s, e) => Agregar();

            fila.Controls.Add(Estilo.Etiqueta("Curso:"));
            fila.Controls.Add(txtNombre);
            fila.Controls.Add(Estilo.Etiqueta("Unidades:"));
            fila.Controls.Add(numUnidades);
            fila.Controls.Add(Estilo.Etiqueta("Objetivo:"));
            fila.Controls.Add(numObjetivo);
            fila.Controls.Add(btnAgregar);
            Controls.Add(fila);

            grid.Location = new Point(15, 65);
            grid.Size = new Size(650, 285);
            Estilo.Tabla(grid);
            Controls.Add(grid);

            btnEliminar.Text = "Eliminar seleccionado";
            btnEliminar.Location = new Point(15, 365);
            btnEliminar.Size = new Size(180, 32);
            Estilo.Boton(btnEliminar, Estilo.Peligro);
            btnEliminar.Click += (s, e) => Eliminar();
            Controls.Add(btnEliminar);

            Cargar();
        }

        private void Cargar()
        {
            grid.DataSource = Repositorio.ListarCursos();
            foreach (DataGridViewColumn c in grid.Columns)
            {
                if (c.Name == "Id") c.Visible = false;
                if (c.Name == "NotaObjetivo") c.HeaderText = "Nota objetivo";
            }
        }

        private void Agregar()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Escribe el nombre del curso.");
                return;
            }
            Repositorio.AgregarCurso(txtNombre.Text.Trim(),
                (int)numUnidades.Value, (double)numObjetivo.Value);
            txtNombre.Clear();
            Cargar();
        }

        private void Eliminar()
        {
            if (grid.CurrentRow?.DataBoundItem is not Curso curso)
            {
                MessageBox.Show("Selecciona un curso.");
                return;
            }
            var r = MessageBox.Show(
                $"¿Eliminar '{curso.Nombre}' y todas sus notas?",
                "Confirmar", MessageBoxButtons.YesNo);
            if (r != DialogResult.Yes) return;

            Repositorio.EliminarCurso(curso.Id);
            Cargar();
        }
    }
}