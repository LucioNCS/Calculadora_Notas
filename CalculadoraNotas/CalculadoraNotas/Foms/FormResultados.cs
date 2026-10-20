using CalculadoraNotas.Data;
using CalculadoraNotas.Models;

namespace CalculadoraNotas.Forms
{
    public class FormResultados : Form
    {
        private ComboBox cmbCurso = new();
        private ComboBox cmbUnidad = new();
        private NumericUpDown numPeso = new();
        private DataGridView grid = new();
        private Label lblFinal = new();
        private Label lblNecesaria = new();

        public FormResultados()
        {
            Text = "Resultados";
            Size = new Size(620, 500);
            StartPosition = FormStartPosition.CenterParent;
            Estilo.Formulario(this);

            Controls.Add(new Label { Text = "Curso:", Location = new Point(15, 20), AutoSize = true });
            cmbCurso.Location = new Point(70, 16);
            cmbCurso.Width = 250;
            cmbCurso.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCurso.DisplayMember = "Nombre";
            cmbCurso.DataSource = Repositorio.ListarCursos();
            Controls.Add(cmbCurso);

            Controls.Add(new Label { Text = "Unidad:", Location = new Point(335, 20), AutoSize = true });
            cmbUnidad.Location = new Point(395, 16);
            cmbUnidad.Width = 60;
            cmbUnidad.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cmbUnidad);

            Controls.Add(new Label
            {
                Text = "Peso de la siguiente evaluación (%):",
                Location = new Point(15, 60),
                AutoSize = true
            });
            numPeso.Location = new Point(270, 56);
            numPeso.Width = 65;
            numPeso.Minimum = 0;
            numPeso.Maximum = 100;
            numPeso.DecimalPlaces = 1;
            Controls.Add(numPeso);

            grid.Location = new Point(15, 100);
            grid.Size = new Size(570, 150);
            Estilo.Tabla(grid);
            grid.Columns.Add("unidad", "Unidad");
            grid.Columns.Add("peso", "Peso registrado");
            grid.Columns.Add("nota", "Nota");
            grid.Columns.Add("estado", "Estado");
            Controls.Add(grid);

            lblFinal.Location = new Point(15, 265);
            lblFinal.Size = new Size(570, 35);
            lblFinal.Font = Estilo.Grande;
            Controls.Add(lblFinal);

            lblNecesaria.Location = new Point(15, 310);
            lblNecesaria.Size = new Size(570, 130);
            lblNecesaria.Font = new Font("Segoe UI", 11);
            Controls.Add(lblNecesaria);

            cmbCurso.SelectedIndexChanged += (s, e) => CambioCurso();
            cmbUnidad.SelectedIndexChanged += (s, e) => CambioUnidad();
            numPeso.ValueChanged += (s, e) => Calcular();
            CambioCurso();
        }

        private void CambioCurso()
        {
            cmbUnidad.Items.Clear();
            if (cmbCurso.SelectedItem is Curso curso)
            {
                var notas = Repositorio.ListarNotas(curso.Id);
                int elegida = 0;
                for (int i = 1; i <= curso.Unidades; i++)
                    cmbUnidad.Items.Add(i);

                // Selecciona la primera unidad que aún no está completa
                for (int i = 1; i <= curso.Unidades; i++)
                {
                    if (!Calculos.UnidadCompleta(notas, i)) { elegida = i - 1; break; }
                }
                cmbUnidad.SelectedIndex = elegida;
            }
            Calcular();
        }

        private void CambioUnidad()
        {
            if (cmbCurso.SelectedItem is Curso curso && cmbUnidad.SelectedItem is int unidad)
            {
                var notas = Repositorio.ListarNotas(curso.Id);
                double restante = 100 - Calculos.PesoUnidad(notas, unidad);
                numPeso.Value = (decimal)Math.Clamp(restante, 0, 100);
            }
            Calcular();
        }

        private void Calcular()
        {
            grid.Rows.Clear();

            if (cmbCurso.SelectedItem is not Curso curso)
            {
                lblFinal.Font = Estilo.Grande;
                lblFinal.ForeColor = Color.Gray;
                lblFinal.Text = "Crea un curso y registra notas primero.";
                lblNecesaria.Text = "";
                return;
            }

            var notas = Repositorio.ListarNotas(curso.Id);

            // ---- Tabla de unidades ----
            for (int u = 1; u <= curso.Unidades; u++)
            {
                double peso = Calculos.PesoUnidad(notas, u);
                bool completa = Calculos.UnidadCompleta(notas, u);
                string nota = "—";
                string estado = "Sin notas";
                bool aprobada = false;

                if (peso > 0)
                {
                    int n = Calculos.NotaUnidad(notas, u);
                    nota = n.ToString();
                    aprobada = n >= Calculos.NotaAprobatoria;
                    estado = completa ? (aprobada ? "Aprobado" : "Desaprobado") : "En curso";
                }

                int idx = grid.Rows.Add($"Unidad {u}", $"{peso:0.##}%", nota, estado);
                if (completa)
                {
                    if (aprobada) grid.Rows[idx].DefaultCellStyle.Font = Estilo.Negrita;
                    else grid.Rows[idx].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            grid.ClearSelection();

            // ---- Promedio final ----
            int? final = Calculos.PromedioFinal(notas, curso.Unidades);
            if (final == null)
            {
                lblFinal.Font = Estilo.Grande;
                lblFinal.ForeColor = Color.Gray;
                lblFinal.Text = "Promedio final: pendiente (faltan unidades por completar)";
            }
            else if (final >= Calculos.NotaAprobatoria)
            {
                lblFinal.Font = Estilo.GrandeNegrita;
                lblFinal.ForeColor = Color.Black;
                lblFinal.Text = $"Promedio final: {final}  —  APROBADO";
            }
            else
            {
                lblFinal.Font = Estilo.Grande;
                lblFinal.ForeColor = Color.Red;
                lblFinal.Text = $"Promedio final: {final}  —  DESAPROBADO";
            }

            // ---- Nota necesaria ----
            lblNecesaria.Text = TextoNecesaria(curso, notas);
        }

        private string TextoNecesaria(Curso curso, List<Nota> notas)
        {
            if (cmbUnidad.SelectedItem is not int unidad)
                return "";

            if (Calculos.UnidadCompleta(notas, unidad))
                return $"La unidad {unidad} ya está completa (100%). Elige otra unidad.";

            double pesoSig = (double)numPeso.Value;
            if (pesoSig <= 0)
                return "Indica el peso de la siguiente evaluación.";

            double nec = Calculos.NotaNecesaria(
                notas, curso.Unidades, unidad, curso.NotaObjetivo, pesoSig);

            string texto;
            if (nec <= 0)
                texto = $"Con lo que llevas en la unidad {unidad} ya alcanzas tu objetivo de {curso.NotaObjetivo:0.##}.";
            else if (nec > 20)
                texto = $"No alcanzas tu objetivo de {curso.NotaObjetivo:0.##} ni con 20 en la siguiente evaluación (necesitarías {nec:0.00}).";
            else
                texto = $"Para llegar a tu objetivo de {curso.NotaObjetivo:0.##}, necesitas {nec:0.00} en la siguiente evaluación de la unidad {unidad} ({pesoSig:0.##}%).";

            bool otrasPendientes = false;
            for (int u = 1; u <= curso.Unidades; u++)
                if (u != unidad && !Calculos.UnidadCompleta(notas, u)) otrasPendientes = true;

            if (otrasPendientes)
                texto += "\n\n(Las otras unidades sin terminar se asumen en tu nota objetivo.)";

            return texto;
        }
    }
}