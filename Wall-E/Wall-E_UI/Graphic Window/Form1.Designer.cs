namespace WallE
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Grapher = new PictureBox();
            Input = new TextBox();
            Compile = new Button();
            Save = new Button();
            Clear = new Button();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            View_Files = new Button();
            MoveUp = new Button();
            MoveDown = new Button();
            MoveRight = new Button();
            MoveLeft = new Button();
            pictureBox2 = new PictureBox();
            GoBack = new Button();
            Run = new Button();
            label1 = new Label();
            Reset = new Button();
            ZoomPlus = new Button();
            ZoomMinus = new Button();
            Zoom = new Label();
            StopDrawing = new Button();
            ((System.ComponentModel.ISupportInitialize)Grapher).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // Grapher
            // 
            Grapher.BackgroundImageLayout = ImageLayout.Stretch;
            Grapher.BorderStyle = BorderStyle.FixedSingle;
            Grapher.InitialImage = null;
            Grapher.Location = new Point(627, 12);
            Grapher.Name = "Grapher";
            Grapher.Size = new Size(1285, 964);
            Grapher.TabIndex = 0;
            Grapher.TabStop = false;
            // 
            // Input
            // 
            Input.Font = new Font("Consolas", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            Input.Location = new Point(5, 295);
            Input.Multiline = true;
            Input.Name = "Input";
            Input.ScrollBars = ScrollBars.Vertical;
            Input.Size = new Size(616, 638);
            Input.TabIndex = 1;
            // 
            // Compile
            // 
            Compile.BackgroundImageLayout = ImageLayout.Stretch;
            Compile.FlatAppearance.BorderColor = Color.Black;
            Compile.FlatStyle = FlatStyle.Flat;
            Compile.Font = new Font("Bradley Hand ITC", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            Compile.ForeColor = SystemColors.ControlText;
            Compile.Location = new Point(395, 939);
            Compile.Name = "Compile";
            Compile.Size = new Size(110, 37);
            Compile.TabIndex = 2;
            Compile.Text = "Compile";
            Compile.UseVisualStyleBackColor = true;
            Compile.Click += Compile_Click;
            // 
            // Save
            // 
            Save.BackgroundImageLayout = ImageLayout.Stretch;
            Save.FlatAppearance.BorderColor = Color.Black;
            Save.FlatStyle = FlatStyle.Flat;
            Save.Font = new Font("Bradley Hand ITC", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            Save.ForeColor = SystemColors.ControlText;
            Save.Location = new Point(221, 939);
            Save.Name = "Save";
            Save.Size = new Size(96, 37);
            Save.TabIndex = 3;
            Save.Text = " Save";
            Save.UseVisualStyleBackColor = true;
            Save.Click += Save_Click;
            // 
            // Clear
            // 
            Clear.BackgroundImageLayout = ImageLayout.Stretch;
            Clear.FlatAppearance.BorderColor = Color.Black;
            Clear.FlatStyle = FlatStyle.Flat;
            Clear.Font = new Font("Bradley Hand ITC", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            Clear.ForeColor = SystemColors.ControlText;
            Clear.Location = new Point(119, 939);
            Clear.Name = "Clear";
            Clear.Size = new Size(96, 37);
            Clear.TabIndex = 4;
            Clear.Text = "Clear";
            Clear.UseVisualStyleBackColor = true;
            Clear.Click += Clear_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(-32, 43);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(214, 246);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.BackColor = Color.White;
            label2.Font = new Font("Bradley Hand ITC", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = SystemColors.ControlText;
            label2.ImageAlign = ContentAlignment.BottomLeft;
            label2.Location = new Point(159, 251);
            label2.Name = "label2";
            label2.Size = new Size(250, 38);
            label2.TabIndex = 8;
            label2.Text = "Code Here:";
            label2.TextAlign = ContentAlignment.BottomCenter;
            // 
            // View_Files
            // 
            View_Files.BackgroundImageLayout = ImageLayout.Stretch;
            View_Files.FlatAppearance.BorderColor = Color.Black;
            View_Files.FlatStyle = FlatStyle.Flat;
            View_Files.Font = new Font("Bradley Hand ITC", 10.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            View_Files.ForeColor = SystemColors.ControlText;
            View_Files.Location = new Point(5, 939);
            View_Files.Name = "View_Files";
            View_Files.Size = new Size(108, 37);
            View_Files.TabIndex = 9;
            View_Files.Text = "View Files";
            View_Files.UseVisualStyleBackColor = true;
            View_Files.Click += View_Files_Click;
            // 
            // MoveUp
            // 
            MoveUp.AutoSize = true;
            MoveUp.FlatAppearance.BorderColor = Color.Black;
            MoveUp.FlatAppearance.MouseDownBackColor = Color.Black;
            MoveUp.FlatAppearance.MouseOverBackColor = Color.Black;
            MoveUp.FlatStyle = FlatStyle.Flat;
            MoveUp.Font = new Font("Century Schoolbook", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            MoveUp.ForeColor = SystemColors.ControlText;
            MoveUp.Location = new Point(487, 215);
            MoveUp.Name = "MoveUp";
            MoveUp.Size = new Size(52, 37);
            MoveUp.TabIndex = 10;
            MoveUp.Text = " ↑";
            MoveUp.UseVisualStyleBackColor = true;
            MoveUp.Click += MoveUp_Click;
            // 
            // MoveDown
            // 
            MoveDown.FlatAppearance.BorderColor = Color.Black;
            MoveDown.FlatStyle = FlatStyle.Flat;
            MoveDown.Font = new Font("Century Schoolbook", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            MoveDown.ForeColor = SystemColors.ControlText;
            MoveDown.Location = new Point(487, 255);
            MoveDown.Name = "MoveDown";
            MoveDown.Size = new Size(52, 34);
            MoveDown.TabIndex = 11;
            MoveDown.Text = " ↓";
            MoveDown.UseVisualStyleBackColor = true;
            MoveDown.Click += MoveDown_Click;
            // 
            // MoveRight
            // 
            MoveRight.FlatAppearance.BorderColor = Color.Black;
            MoveRight.FlatStyle = FlatStyle.Flat;
            MoveRight.Font = new Font("Century Schoolbook", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point);
            MoveRight.ForeColor = SystemColors.ControlText;
            MoveRight.Location = new Point(545, 215);
            MoveRight.Name = "MoveRight";
            MoveRight.Size = new Size(49, 74);
            MoveRight.TabIndex = 12;
            MoveRight.Text = "→";
            MoveRight.UseVisualStyleBackColor = true;
            MoveRight.Click += MoveRight_Click;
            // 
            // MoveLeft
            // 
            MoveLeft.FlatAppearance.BorderColor = Color.Black;
            MoveLeft.FlatStyle = FlatStyle.Flat;
            MoveLeft.Font = new Font("Century Schoolbook", 19.8000011F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            MoveLeft.ForeColor = SystemColors.ControlText;
            MoveLeft.Location = new Point(432, 215);
            MoveLeft.Name = "MoveLeft";
            MoveLeft.Size = new Size(49, 74);
            MoveLeft.TabIndex = 13;
            MoveLeft.Text = "←";
            MoveLeft.UseVisualStyleBackColor = true;
            MoveLeft.Click += MoveLeft_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImage = Properties.Resources.eva_removebg_preview;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(221, 30);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(201, 218);
            pictureBox2.TabIndex = 14;
            pictureBox2.TabStop = false;
            // 
            // GoBack
            // 
            GoBack.FlatStyle = FlatStyle.Flat;
            GoBack.Font = new Font("Bradley Hand ITC", 16.2F, FontStyle.Bold, GraphicsUnit.Point);
            GoBack.Location = new Point(3, 4);
            GoBack.Name = "GoBack";
            GoBack.Size = new Size(167, 51);
            GoBack.TabIndex = 15;
            GoBack.Text = "← Go Back";
            GoBack.UseVisualStyleBackColor = true;
            GoBack.Click += GoBack_Click;
            // 
            // Run
            // 
            Run.FlatStyle = FlatStyle.Flat;
            Run.Font = new Font("Bradley Hand ITC", 13.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            Run.Location = new Point(511, 939);
            Run.Name = "Run";
            Run.Size = new Size(110, 37);
            Run.TabIndex = 16;
            Run.Text = "Run";
            Run.UseVisualStyleBackColor = true;
            Run.Click += Run_Click;
            // 
            // label1
            // 
            label1.Font = new Font("Bradley Hand ITC", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(454, 182);
            label1.Name = "label1";
            label1.Size = new Size(118, 43);
            label1.TabIndex = 17;
            label1.Text = "Move it";
            // 
            // Reset
            // 
            Reset.FlatStyle = FlatStyle.Popup;
            Reset.Font = new Font("Bradley Hand ITC", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            Reset.Location = new Point(497, 12);
            Reset.Name = "Reset";
            Reset.Size = new Size(124, 62);
            Reset.TabIndex = 18;
            Reset.Text = "Reset coordinates";
            Reset.UseVisualStyleBackColor = true;
            Reset.Click += Reset_Click;
            // 
            // ZoomPlus
            // 
            ZoomPlus.FlatStyle = FlatStyle.Flat;
            ZoomPlus.Font = new Font("Century Schoolbook", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point);
            ZoomPlus.Location = new Point(453, 123);
            ZoomPlus.Name = "ZoomPlus";
            ZoomPlus.Size = new Size(52, 56);
            ZoomPlus.TabIndex = 19;
            ZoomPlus.Text = "+";
            ZoomPlus.UseVisualStyleBackColor = true;
            ZoomPlus.Click += ZoomPlus_Click;
            // 
            // ZoomMinus
            // 
            ZoomMinus.FlatStyle = FlatStyle.Flat;
            ZoomMinus.Font = new Font("Century Schoolbook", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point);
            ZoomMinus.Location = new Point(511, 123);
            ZoomMinus.Name = "ZoomMinus";
            ZoomMinus.Size = new Size(51, 56);
            ZoomMinus.TabIndex = 20;
            ZoomMinus.Text = "-";
            ZoomMinus.TextAlign = ContentAlignment.TopCenter;
            ZoomMinus.UseVisualStyleBackColor = true;
            ZoomMinus.Click += ZoomMinus_Click;
            // 
            // Zoom
            // 
            Zoom.Font = new Font("Bradley Hand ITC", 16.2F, FontStyle.Bold, GraphicsUnit.Point);
            Zoom.Location = new Point(454, 87);
            Zoom.Name = "Zoom";
            Zoom.Size = new Size(110, 33);
            Zoom.TabIndex = 21;
            Zoom.Text = "Zoom";
            Zoom.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // StopDrawing
            // 
            StopDrawing.AutoSize = true;
            StopDrawing.FlatStyle = FlatStyle.Flat;
            StopDrawing.Font = new Font("Bradley Hand ITC", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            StopDrawing.Location = new Point(368, 12);
            StopDrawing.Name = "StopDrawing";
            StopDrawing.Size = new Size(123, 62);
            StopDrawing.TabIndex = 22;
            StopDrawing.Text = "Stop \r\nDrawing";
            StopDrawing.UseVisualStyleBackColor = true;
            StopDrawing.Click += StopDrawing_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(1924, 1043);
            Controls.Add(StopDrawing);
            Controls.Add(MoveRight);
            Controls.Add(MoveLeft);
            Controls.Add(MoveUp);
            Controls.Add(Zoom);
            Controls.Add(ZoomMinus);
            Controls.Add(ZoomPlus);
            Controls.Add(Reset);
            Controls.Add(label1);
            Controls.Add(Run);
            Controls.Add(GoBack);
            Controls.Add(pictureBox2);
            Controls.Add(label2);
            Controls.Add(MoveDown);
            Controls.Add(View_Files);
            Controls.Add(Clear);
            Controls.Add(Save);
            Controls.Add(Compile);
            Controls.Add(Input);
            Controls.Add(Grapher);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "GeoWall-E";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)Grapher).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox Grapher;
        private TextBox Input;
        private Button Compile;
        private Button Save;
        private Button Clear;
        private PictureBox pictureBox1;
        private Label label2;
        private Button View_Files;
        private Button MoveUp;
        private Button MoveDown;
        private Button MoveRight;
        private Button MoveLeft;
        private PictureBox pictureBox2;
        private Button GoBack;
        private Button Run;
        private Label label1;
        private Button Reset;
        private Button ZoomPlus;
        private Button ZoomMinus;
        private Label Zoom;
        private Button StopDrawing;
    }
}