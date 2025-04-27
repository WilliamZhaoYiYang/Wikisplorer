namespace Wikisplorer
{
    partial class Map
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
            panel2 = new Panel();
            saveToJSON = new Button();
            refreshButton = new Button();
            saveButton = new Button();
            panel1 = new Panel();
            loadAsJSON = new Button();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Controls.Add(loadAsJSON);
            panel2.Controls.Add(saveToJSON);
            panel2.Controls.Add(refreshButton);
            panel2.Controls.Add(saveButton);
            panel2.Controls.Add(panel1);
            panel2.Location = new Point(0, -1);
            panel2.Name = "panel2";
            panel2.Size = new Size(979, 546);
            panel2.TabIndex = 3;
            // 
            // saveToJSON
            // 
            saveToJSON.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            saveToJSON.Location = new Point(740, 498);
            saveToJSON.Name = "saveToJSON";
            saveToJSON.Size = new Size(110, 34);
            saveToJSON.TabIndex = 5;
            saveToJSON.Text = "Save as JSON file";
            saveToJSON.UseVisualStyleBackColor = true;
            saveToJSON.Click += saveToJSON_Click;
            // 
            // refreshButton
            // 
            refreshButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            refreshButton.Location = new Point(125, 498);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(93, 34);
            refreshButton.TabIndex = 4;
            refreshButton.Text = "Refresh";
            refreshButton.UseVisualStyleBackColor = true;
            refreshButton.Click += refreshButton_Click;
            // 
            // saveButton
            // 
            saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            saveButton.Location = new Point(12, 498);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(93, 34);
            saveButton.TabIndex = 3;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.AutoScroll = true;
            panel1.AutoScrollMargin = new Size(50, 50);
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Location = new Point(12, 13);
            panel1.Name = "panel1";
            panel1.Size = new Size(954, 479);
            panel1.TabIndex = 1;
            // 
            // loadAsJSON
            // 
            loadAsJSON.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            loadAsJSON.Location = new Point(856, 498);
            loadAsJSON.Name = "loadAsJSON";
            loadAsJSON.Size = new Size(110, 34);
            loadAsJSON.TabIndex = 6;
            loadAsJSON.Text = "Load a JSON file";
            loadAsJSON.UseVisualStyleBackColor = true;
            loadAsJSON.Click += loadAsJSON_Click;
            // 
            // Map
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(978, 542);
            Controls.Add(panel2);
            Name = "Map";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form2";
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel2;
        private Button saveButton;
        private Panel panel1;
        private Button refreshButton;
        private Button saveToJSON;
        private Button loadAsJSON;
    }
}