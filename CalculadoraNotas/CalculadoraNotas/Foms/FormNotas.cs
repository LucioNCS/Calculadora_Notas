using CalculadoraNotas.Data;
using CalculadoraNotas.Models;

namespace CalculadoraNotas.Forms
{
    public class FormNotas : Form
    {
        private ComboBox cmbCurso = new();
        private ComboBox cmbUnidad = new();
        private TextBox txtNombre = new();
        private NumericUpDown numValor = new();
        private NumericUpDown numPeso = new();
        private Button btnAgregar = new();
        private Button btnEliminar = new();
        private DataGridView grid = new();
        private Label lblInfo = new();

        public FormNotas()
        {
            Text = "Notas";
            Size = new Size(700, 510);
            StartPosition = FormStartPosition.CenterParent;
            Estilo.Formulario(this);

            // Panel superior: los controles se acomodan solos (no se tapan)
            var fila = new FlowLayoutPanel
            {
                Location = new Point(5, 10),
                Size = new Size(660, 85),
                WrapContents = true
            };

            cmbCurso.Width = 240;
            cmbCurso.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCurso.DisplayMember = "Nombre";
            cmbCurso.DataSource = Repositorio.ListarCursos();

            cmbUnidad.Width = 60;
            cmbUnidad.DropDownStyle = ComboBoxStyle.DropDownList;

            txtNombre.Width = 150;

            numValor.Width = 70;
            numValor.Minimum = 0;
            numValor.Maximum = 20;
            numValor.DecimalPlaces = 2;
            numValor.Increment = 0.5M;

            numPeso.Width = 65;
            numPeso.Minimum = 0;
            numPeso.Maximum = 100;
            numPeso.DecimalPlaces = 1;

            btnAgregar.Text = "Agregar";
            btnAgregar.Size = new Size(85, 29);
            btnAgregar.Margin = new Padding(12, 3, 3, 3);
            Estilo.Boton(btnAgregar, Estilo.Principal);
            btnAgregar.Click += (s, e) => Agregar();

            // Fila 1: curso y unidad
            fila.Controls.Add(Estilo.Etiqueta("Curso:"));
            fila.Controls.Add(cmbCurso);
            fila.Controls.Add(Estilo.Etiqueta("Unidad:"));
            fila.Controls.Add(cmbUnidad);
            fila.SetFlowBreak(cmbUnidad, true);

            // Fila 2: evaluación, nota, peso
            fila.Controls.Add(Estilo.Etiqueta("Evaluación:"));
            fila.Controls.Add(txtNombre);
            fila.Controls.Add(Estilo.Etiqueta("Nota:"));
            fila.Controls.Add(numValor);
            fila.Controls.Add(Estilo.Etiqueta("Peso %:"));
            fila.Controls.Add(numPeso);
            fila.Controls.Add(btnAgregar);
            Controls.Add(fila);

            grid.Location = new Point(15, 105);
            grid.Size = new Size(650, 265);
            Estilo.Tabla(grid);
            Controls.Add(grid);

            btnEliminar.Text = "Eliminar seleccionada";
            btnEliminar.Location = new Point(15, 385);
            btnEliminar.Size = new Size(180, 32);
            Estilo.Boton(btnEliminar, Estilo.Peligro);
            btnEliminar.Click += (s, e) => Eliminar();
            Controls.Add(btnEliminar);

            lblInfo.Location = new Point(210, 393);
            lblInfo.AutoSize = true;
            lblInfo.Font = Estilo.Negrita;
            Controls.Add(lblInfo);

            cmbCurso.SelectedIndexChanged += (s, e) => CambioCurso();
            cmbUnidad.SelectedIndexChanged += (s, e) => CargarNotas();
            CambioCurso();
        }

        private void CambioCurso()
        {
            cmbUnidad.Items.Clear();
            if (cmbCurso.SelectedItem is Curso curso)
            {
                for (int i = 1; i <= curso.Unidades; i++)
                    cmbUnidad.Items.Add(i);
                cmbUnidad.SelectedIndex = 0;
            }
            CargarNotas();
        }

        private void CargarNotas()
        {
            if (cmbCurso.SelectedItem is not Curso curso || cmbUnidad.SelectedItem is not int unidad)
            {
                grid.DataSource = null;
                lblInfo.Text = "";
                return;
            }

            var todas = Repositorio.ListarNotas(curso.Id);
            grid.DataSource = todas.Where(n => n.Unidad == unidad).ToList();

            foreach (DataGridViewColumn c in grid.Columns)
            {
                switch (c.Name)
                {
                    case "Id":
                    case "CursoId":
                    case "Unidad":
                        c.Visible = false;
                        break;
                    case "Nombre": c.HeaderText = "Evaluación"; break;
                    case "Valor": c.HeaderText = "Nota"; break;
                    case "Peso": c.HeaderText = "Peso %"; break;
                }
            }

            double peso = Calculos.PesoUnidad(todas, unidad);
            string nota = peso > 0 ? Calculos.NotaUnidad(todas, unidad).ToString() : "—";
            lblInfo.Text = $"Peso registrado: {peso:0.##}%   |   Nota de la unidad: {nota}";
        }

        private void Agregar()
        {
            if (cmbCurso.SelectedItem is not Curso curso || cmbUnidad.SelectedItem is not int unidad)
            {
                MessageBox.Show("Primero crea un curso.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Escribe el nombre de la evaluación.");
                return;
            }
            if (numPeso.Value <= 0)
            {
                MessageBox.Show("El peso debe ser mayor que 0.");
                return;
            }

            var todas = Repositorio.ListarNotas(curso.Id);
            if (Calculos.PesoUnidad(todas, unidad) + (double)numPeso.Value > 100.0001)
            {
                MessageBox.Show("Los pesos de la unidad no pueden sumar más de 100%.");
                return;
            }

            Repositorio.AgregarNota(curso.Id, unidad, txtNombre.Text.Trim(),
                (double)numValor.Value, (double)numPeso.Value);
            txtNombre.Clear();
            CargarNotas();
        }

        private void Eliminar()
        {
            if (grid.CurrentRow?.DataBoundItem is not Nota nota)
            {
                MessageBox.Show("Selecciona una nota.");
                return;
            }
            Repositorio.EliminarNota(nota.Id);
            CargarNotas();
        }
    }
}