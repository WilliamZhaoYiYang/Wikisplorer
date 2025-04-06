namespace Wikisplorer
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
            textBox1 = new TextBox();
            enterLink = new Button();
            button2 = new Button();
            label1 = new Label();
            textBox2 = new TextBox();
            label2 = new Label();
            comboBox1 = new ComboBox();
            label3 = new Label();
            comboBox2 = new ComboBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Cascadia Mono", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            textBox1.ForeColor = Color.Gray;
            textBox1.Location = new Point(200, 371);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(485, 21);
            textBox1.TabIndex = 0;
            textBox1.TabStop = false;
            textBox1.Text = "Enter a Wikipedia link";
            textBox1.Enter += textBox1_Enter;
            textBox1.KeyPress += textBox1_KeyPress;
            // 
            // enterLink
            // 
            enterLink.Font = new Font("Cascadia Mono", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            enterLink.Location = new Point(289, 418);
            enterLink.Name = "enterLink";
            enterLink.Size = new Size(131, 44);
            enterLink.TabIndex = 1;
            enterLink.Text = "Enter";
            enterLink.UseVisualStyleBackColor = true;
            enterLink.Click += enterLink_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Cascadia Mono", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.Location = new Point(426, 418);
            button2.Name = "button2";
            button2.Size = new Size(131, 44);
            button2.TabIndex = 1;
            button2.Text = "Generate Map";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Cascadia Mono", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(200, 86);
            label1.Name = "label1";
            label1.Size = new Size(331, 25);
            label1.TabIndex = 2;
            label1.Text = "Links in last entered article";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(200, 124);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(485, 175);
            textBox2.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Cascadia Mono", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(200, 61);
            label2.Name = "label2";
            label2.Size = new Size(243, 25);
            label2.TabIndex = 4;
            label2.Text = "Last entered article:";
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Order Of Appearance", "Title", "Count" });
            comboBox1.Location = new Point(270, 311);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(150, 23);
            comboBox1.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Cascadia Mono", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(200, 311);
            label3.Name = "label3";
            label3.Size = new Size(64, 18);
            label3.TabIndex = 6;
            label3.Text = "Sort By";
            // 
            // comboBox2
            // 
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "Descending", "Ascending" });
            comboBox2.Location = new Point(426, 311);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(105, 23);
            comboBox2.TabIndex = 7;
            // 
            // button1
            // 
            button1.Font = new Font("Cascadia Mono", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(547, 311);
            button1.Name = "button1";
            button1.Size = new Size(66, 23);
            button1.TabIndex = 8;
            button1.Text = "Sort";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGray;
            ClientSize = new Size(862, 538);
            Controls.Add(button1);
            Controls.Add(comboBox2);
            Controls.Add(label3);
            Controls.Add(comboBox1);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(enterLink);
            Controls.Add(textBox1);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            MouseDown += Form1_MouseDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button enterLink;
        private Button button2;
        private Label label1;
        private TextBox textBox2;
        private Label label2;
        private ComboBox comboBox1;
        private Label label3;
        private ComboBox comboBox2;
        private Button button1;
    }
}
