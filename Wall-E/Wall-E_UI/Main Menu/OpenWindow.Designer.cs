namespace WallE
{
    partial class OpenWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenWindow));
            GraphicWindow = new Button();
            Exit = new Button();
            MoreAbout = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // GraphicWindow
            // 
            GraphicWindow.BackColor = Color.Transparent;
            GraphicWindow.BackgroundImageLayout = ImageLayout.None;
            GraphicWindow.FlatStyle = FlatStyle.Flat;
            GraphicWindow.Font = new Font("OCR A Extended", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            GraphicWindow.ForeColor = SystemColors.ControlLightLight;
            GraphicWindow.Location = new Point(53, 196);
            GraphicWindow.Name = "GraphicWindow";
            GraphicWindow.Size = new Size(231, 93);
            GraphicWindow.TabIndex = 0;
            GraphicWindow.Text = "Start Drawing";
            GraphicWindow.UseVisualStyleBackColor = false;
            GraphicWindow.Click += GraphicWindow_Click;
            // 
            // Exit
            // 
            Exit.BackColor = Color.Transparent;
            Exit.FlatStyle = FlatStyle.Flat;
            Exit.Font = new Font("OCR A Extended", 19.8000011F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            Exit.ForeColor = SystemColors.ControlLightLight;
            Exit.Location = new Point(53, 438);
            Exit.Name = "Exit";
            Exit.Size = new Size(231, 93);
            Exit.TabIndex = 2;
            Exit.Text = "Exit";
            Exit.UseVisualStyleBackColor = false;
            Exit.Click += Exit_Click;
            // 
            // MoreAbout
            // 
            MoreAbout.BackColor = Color.Transparent;
            MoreAbout.FlatStyle = FlatStyle.Flat;
            MoreAbout.Font = new Font("OCR A Extended", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            MoreAbout.ForeColor = SystemColors.ControlLightLight;
            MoreAbout.Location = new Point(53, 316);
            MoreAbout.Name = "MoreAbout";
            MoreAbout.Size = new Size(231, 93);
            MoreAbout.TabIndex = 3;
            MoreAbout.Text = "More About";
            MoreAbout.UseVisualStyleBackColor = false;
            MoreAbout.Click += MoreAbout_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("OCR A Extended", 72F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(12, 19);
            label1.Name = "label1";
            label1.Size = new Size(709, 123);
            label1.TabIndex = 4;
            label1.Text = "GeoWall-E";
            // 
            // OpenWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.photo_2023_11_26_16_45_55__2_;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1148, 594);
            Controls.Add(GraphicWindow);
            Controls.Add(label1);
            Controls.Add(MoreAbout);
            Controls.Add(Exit);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "OpenWindow";
            Text = "GeoWall-E";
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button GraphicWindow;
        private Button Exit;
        private Button MoreAbout;
        private Label label1;
    }
}