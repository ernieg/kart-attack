namespace MapEditor
{
    partial class MapEditorForm
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
            this.MapPanel = new System.Windows.Forms.Panel();
            this.UpperPanel = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MapNameLabel = new System.Windows.Forms.Label();
            this.MapName = new System.Windows.Forms.Label();
            this.ObjectNameLabel = new System.Windows.Forms.Label();
            this.ObjectNameTextBox = new System.Windows.Forms.TextBox();
            this.ObjectSizeLabel = new System.Windows.Forms.Label();
            this.ObjectSizeNumber = new System.Windows.Forms.NumericUpDown();
            this.XLabel = new System.Windows.Forms.Label();
            this.XNumber = new System.Windows.Forms.NumericUpDown();
            this.YLabel = new System.Windows.Forms.Label();
            this.YNumber = new System.Windows.Forms.NumericUpDown();
            this.PointerButton = new System.Windows.Forms.Button();
            this.ObjectButton = new System.Windows.Forms.Button();
            this.ObjectColorLabel = new System.Windows.Forms.Label();
            this.ColorBox = new System.Windows.Forms.ComboBox();
            this.ColorPanel = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.MapPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectSizeNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // MapPanel
            // 
            this.MapPanel.BackColor = System.Drawing.SystemColors.Control;
            this.MapPanel.Controls.Add(this.UpperPanel);
            this.MapPanel.Location = new System.Drawing.Point(3, 58);
            this.MapPanel.Name = "MapPanel";
            this.MapPanel.Size = new System.Drawing.Size(1280, 720);
            this.MapPanel.TabIndex = 0;
            this.MapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MapPanel_Paint);
            // 
            // UpperPanel
            // 
            this.UpperPanel.BackColor = System.Drawing.Color.Transparent;
            this.UpperPanel.Location = new System.Drawing.Point(0, 0);
            this.UpperPanel.Name = "UpperPanel";
            this.UpperPanel.Size = new System.Drawing.Size(1280, 720);
            this.UpperPanel.TabIndex = 1;
            this.UpperPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.UpperPanel_Paint);
            this.UpperPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MapPanel_MouseClick);
            this.UpperPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapPanel_MouseDown);
            this.UpperPanel.MouseEnter += new System.EventHandler(this.MapPanel_MouseEnter);
            this.UpperPanel.MouseLeave += new System.EventHandler(this.MapPanel_MouseLeave);
            this.UpperPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapPanel_MouseMove);
            this.UpperPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapPanel_MouseUp);
            this.UpperPanel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MapPanel_PreviewKeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1283, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "MainMenu";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMapToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // newMapToolStripMenuItem
            // 
            this.newMapToolStripMenuItem.Name = "newMapToolStripMenuItem";
            this.newMapToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.newMapToolStripMenuItem.Text = "New Map";
            this.newMapToolStripMenuItem.Click += new System.EventHandler(this.newMapToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.saveToolStripMenuItem.Text = "Save As";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            // 
            // MapNameLabel
            // 
            this.MapNameLabel.AutoSize = true;
            this.MapNameLabel.Location = new System.Drawing.Point(12, 35);
            this.MapNameLabel.Name = "MapNameLabel";
            this.MapNameLabel.Size = new System.Drawing.Size(65, 13);
            this.MapNameLabel.TabIndex = 2;
            this.MapNameLabel.Text = "Map Name: ";
            // 
            // MapName
            // 
            this.MapName.AutoSize = true;
            this.MapName.Location = new System.Drawing.Point(71, 35);
            this.MapName.Name = "MapName";
            this.MapName.Size = new System.Drawing.Size(53, 13);
            this.MapName.TabIndex = 3;
            this.MapName.Text = "New Map";
            // 
            // ObjectNameLabel
            // 
            this.ObjectNameLabel.AutoSize = true;
            this.ObjectNameLabel.Location = new System.Drawing.Point(445, 35);
            this.ObjectNameLabel.Name = "ObjectNameLabel";
            this.ObjectNameLabel.Size = new System.Drawing.Size(72, 13);
            this.ObjectNameLabel.TabIndex = 4;
            this.ObjectNameLabel.Text = "Object Name:";
            // 
            // ObjectNameTextBox
            // 
            this.ObjectNameTextBox.Location = new System.Drawing.Point(523, 32);
            this.ObjectNameTextBox.Name = "ObjectNameTextBox";
            this.ObjectNameTextBox.Size = new System.Drawing.Size(131, 20);
            this.ObjectNameTextBox.TabIndex = 5;
            this.ObjectNameTextBox.Text = "Wall";
            // 
            // ObjectSizeLabel
            // 
            this.ObjectSizeLabel.AutoSize = true;
            this.ObjectSizeLabel.Location = new System.Drawing.Point(904, 35);
            this.ObjectSizeLabel.Name = "ObjectSizeLabel";
            this.ObjectSizeLabel.Size = new System.Drawing.Size(64, 13);
            this.ObjectSizeLabel.TabIndex = 6;
            this.ObjectSizeLabel.Text = "Object Size:";
            // 
            // ObjectSizeNumber
            // 
            this.ObjectSizeNumber.Location = new System.Drawing.Point(974, 33);
            this.ObjectSizeNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ObjectSizeNumber.Name = "ObjectSizeNumber";
            this.ObjectSizeNumber.Size = new System.Drawing.Size(57, 20);
            this.ObjectSizeNumber.TabIndex = 7;
            this.ObjectSizeNumber.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // XLabel
            // 
            this.XLabel.AutoSize = true;
            this.XLabel.Location = new System.Drawing.Point(1053, 35);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(53, 13);
            this.XLabel.TabIndex = 8;
            this.XLabel.Text = "X Length:";
            // 
            // XNumber
            // 
            this.XNumber.Location = new System.Drawing.Point(1112, 32);
            this.XNumber.Name = "XNumber";
            this.XNumber.Size = new System.Drawing.Size(45, 20);
            this.XNumber.TabIndex = 9;
            this.XNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XNumber.ValueChanged += new System.EventHandler(this.XNumber_ValueChanged);
            // 
            // YLabel
            // 
            this.YLabel.AutoSize = true;
            this.YLabel.Location = new System.Drawing.Point(1171, 35);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(53, 13);
            this.YLabel.TabIndex = 10;
            this.YLabel.Text = "Y Length:";
            // 
            // YNumber
            // 
            this.YNumber.Location = new System.Drawing.Point(1230, 32);
            this.YNumber.Name = "YNumber";
            this.YNumber.Size = new System.Drawing.Size(42, 20);
            this.YNumber.TabIndex = 11;
            this.YNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.YNumber.ValueChanged += new System.EventHandler(this.YNumber_ValueChanged);
            // 
            // PointerButton
            // 
            this.PointerButton.Enabled = false;
            this.PointerButton.Location = new System.Drawing.Point(284, 29);
            this.PointerButton.Name = "PointerButton";
            this.PointerButton.Size = new System.Drawing.Size(69, 25);
            this.PointerButton.TabIndex = 12;
            this.PointerButton.Text = "Pointer";
            this.PointerButton.UseVisualStyleBackColor = true;
            this.PointerButton.Click += new System.EventHandler(this.PointerButton_Click);
            // 
            // ObjectButton
            // 
            this.ObjectButton.Location = new System.Drawing.Point(359, 29);
            this.ObjectButton.Name = "ObjectButton";
            this.ObjectButton.Size = new System.Drawing.Size(80, 25);
            this.ObjectButton.TabIndex = 13;
            this.ObjectButton.Text = "New Object";
            this.ObjectButton.UseVisualStyleBackColor = true;
            this.ObjectButton.Click += new System.EventHandler(this.ObjectButton_Click);
            // 
            // ObjectColorLabel
            // 
            this.ObjectColorLabel.AutoSize = true;
            this.ObjectColorLabel.Location = new System.Drawing.Point(660, 35);
            this.ObjectColorLabel.Name = "ObjectColorLabel";
            this.ObjectColorLabel.Size = new System.Drawing.Size(68, 13);
            this.ObjectColorLabel.TabIndex = 14;
            this.ObjectColorLabel.Text = "Object Color:";
            // 
            // ColorBox
            // 
            this.ColorBox.FormattingEnabled = true;
            this.ColorBox.Location = new System.Drawing.Point(731, 32);
            this.ColorBox.Name = "ColorBox";
            this.ColorBox.Size = new System.Drawing.Size(117, 21);
            this.ColorBox.TabIndex = 15;
            this.ColorBox.SelectedIndexChanged += new System.EventHandler(this.ColorBox_SelectedIndexChanged);
            // 
            // ColorPanel
            // 
            this.ColorPanel.BackColor = System.Drawing.Color.Black;
            this.ColorPanel.Location = new System.Drawing.Point(854, 29);
            this.ColorPanel.Name = "ColorPanel";
            this.ColorPanel.Size = new System.Drawing.Size(25, 25);
            this.ColorPanel.TabIndex = 16;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MapEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1283, 790);
            this.Controls.Add(this.ColorPanel);
            this.Controls.Add(this.ColorBox);
            this.Controls.Add(this.ObjectColorLabel);
            this.Controls.Add(this.ObjectButton);
            this.Controls.Add(this.PointerButton);
            this.Controls.Add(this.YNumber);
            this.Controls.Add(this.YLabel);
            this.Controls.Add(this.XNumber);
            this.Controls.Add(this.XLabel);
            this.Controls.Add(this.ObjectSizeNumber);
            this.Controls.Add(this.ObjectSizeLabel);
            this.Controls.Add(this.ObjectNameTextBox);
            this.Controls.Add(this.ObjectNameLabel);
            this.Controls.Add(this.MapName);
            this.Controls.Add(this.MapNameLabel);
            this.Controls.Add(this.MapPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1299, 828);
            this.MinimumSize = new System.Drawing.Size(1299, 828);
            this.Name = "MapEditorForm";
            this.Text = "Form1";
            this.MapPanel.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectSizeNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel MapPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.Label MapNameLabel;
        private System.Windows.Forms.Label MapName;
        private System.Windows.Forms.Label ObjectNameLabel;
        private System.Windows.Forms.TextBox ObjectNameTextBox;
        private System.Windows.Forms.Label ObjectSizeLabel;
        private System.Windows.Forms.NumericUpDown ObjectSizeNumber;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.NumericUpDown XNumber;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.NumericUpDown YNumber;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.Button PointerButton;
        private System.Windows.Forms.Button ObjectButton;
        private System.Windows.Forms.ToolStripMenuItem newMapToolStripMenuItem;
        private System.Windows.Forms.Label ObjectColorLabel;
        private System.Windows.Forms.ComboBox ColorBox;
        private System.Windows.Forms.Panel ColorPanel;
        private System.Windows.Forms.Panel UpperPanel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

