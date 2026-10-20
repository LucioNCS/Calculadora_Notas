using CalculadoraNotas.Data;
using CalculadoraNotas.Forms;

namespace CalculadoraNotas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Conexion.Inicializar();

            Text = "Calculadora de Notas";
            Size = new Size(330, 310);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Estilo.Formulario(this);

            Controls.Add(new Label
            {
                Text = "Calculadora de Notas",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Estilo.Principal,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 20),
                Size = new Size(314, 40)
            });

            Boton("Cursos", 80, () => { using var f = new FormCursos(); f.ShowDialog(this); });
            Boton("Notas", 135, () => { using var f = new FormNotas(); f.ShowDialog(this); });
            Boton("Resultados", 190, () => { using var f = new FormResultados(); f.ShowDialog(this); });
        }

        private void Boton(string texto, int y, Action abrir)
        {
            var b = new Button
            {
                Text = texto,
                Location = new Point(45, y),
                Size = new Size(220, 42)
            };
            Estilo.Boton(b, Estilo.Principal);
            b.Click += (s, e) => abrir();
            Controls.Add(b);
        }
    }
}