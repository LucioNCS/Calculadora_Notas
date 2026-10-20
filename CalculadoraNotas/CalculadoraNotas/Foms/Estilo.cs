namespace CalculadoraNotas.Forms
{
    public static class Estilo
    {
        public static readonly Color Principal = Color.FromArgb(37, 99, 235);
        public static readonly Color Peligro = Color.FromArgb(220, 38, 38);
        public static readonly Color Fondo = Color.FromArgb(245, 247, 250);

        public static Font Negrita => new("Segoe UI", 10, FontStyle.Bold);
        public static Font Grande => new("Segoe UI", 14);
        public static Font GrandeNegrita => new("Segoe UI", 14, FontStyle.Bold);

        // Etiqueta que se acomoda sola dentro de un FlowLayoutPanel
        public static Label Etiqueta(string texto)
            => new Label
            {
                Text = texto,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(10, 3, 3, 3)
            };

        public static void Formulario(Form f)
        {
            f.BackColor = Fondo;
            f.Font = new Font("Segoe UI", 10);
        }

        public static void Boton(Button b, Color color)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = color;
            b.ForeColor = Color.White;
            b.Cursor = Cursors.Hand;
        }

        public static void Tabla(DataGridView g)
        {
            g.ReadOnly = true;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.AllowUserToResizeRows = false;
            g.MultiSelect = false;
            g.RowHeadersVisible = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.BackgroundColor = Color.White;
            g.BorderStyle = BorderStyle.None;

            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 32;
            g.ColumnHeadersDefaultCellStyle.BackColor = Principal;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Principal;
            g.ColumnHeadersDefaultCellStyle.Font = Negrita;

            g.RowTemplate.Height = 28;
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            g.DefaultCellStyle.SelectionForeColor = Color.Black;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        }
    }
}