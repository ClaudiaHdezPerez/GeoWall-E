using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Policy;

namespace WallE
{
    public partial class OpenWindow : Form
    {
        public OpenWindow()
        {
            InitializeComponent();
        }

        #region Enlace con el graficador

        // Este método se llama cuando se ejecuta el evento "click" del botón "Start Drawing"
        private void GraphicWindow_Click(object sender, EventArgs e)
        {
            //Se crea una instancia del otro formulario
            var drawingWindow = new Form1();

            // Se esconde el actual y se muestra el instanciado
            this.Hide();
            drawingWindow.ShowDialog();

            // Cuando se haya cerrado la instancia entonces se vuelve a mostrar la principal
            this.Show();
        }
        #endregion

        #region Abre el informe del proyecto

        // Este método se llama cuando se ejecuta el evento "click" del botón "More About"
        private void MoreAbout_Click(object sender, EventArgs e)
        {
            Process process = new();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = Path.Join("..", Path.Join("..", Path.Join("..", "Report\\GeoWall-E.pdf")));
            process.Start();
        }
        #endregion

        #region Salir de la aplicación

        // Este método se llama cuando se ejecuta el evento "click" del botón "Exit"
        private void Exit_Click(object sender, EventArgs e)
        {
            MessageBoxButtons messageBoxButtons = MessageBoxButtons.YesNo;

            DialogResult result1 = MessageBox.Show("Are you sure?", "You are exiting the application",
                    messageBoxButtons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result1 == DialogResult.Yes)
            {
                this.Close();
            }
        }
        #endregion
    }
}
