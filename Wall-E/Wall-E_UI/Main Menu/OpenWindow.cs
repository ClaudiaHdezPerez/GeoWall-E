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
        private void GraphicWindow_Click(object sender, EventArgs e)
        {
            var drawingWindow = new Form1();

            this.Hide();
            drawingWindow.ShowDialog();

            this.Show();
        }
        #endregion

        #region Abre el informe del proyecto
        private void MoreAbout_Click(object sender, EventArgs e)
        {
            Process process = new();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = Path.Join("..", Path.Join("..", Path.Join("..", "Report\\GeoWall-E.pdf")));
            process.Start();
        }
        #endregion

        #region Salir de la aplicación
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
