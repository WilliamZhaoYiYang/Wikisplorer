using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Wikisplorer
{
    public partial class Map : Form
    {
        private WWScraper wiki;
        private Article lastArticle;
        private Random rand = new Random();

        public Map(WWScraper Wiki, Article LastArticle)
        {
            wiki = Wiki;
            lastArticle = LastArticle;

            InitializeComponent();
            InitializePanelWithButtons();
        }

        private void InitializePanelWithButtons()
        {
            // Add multiple buttons, some off-screen
            //for (int i = 0; i < 15; i++)
            //{
            //    Button button = new Button
            //    {
            //        Text = "Button " + (i + 1),
            //        Size = new Size(100, 30),
            //        Location = new Point(i * 50, i * 50) // Some buttons will be off-screen
            //    };
            //    panel1.Controls.Add(button);
            //}
            int i = 1;

            foreach (Article article in wiki.ArticlesList)
            {
                Button button = new Button
                {
                    Text = article.Title,
                    Size = new Size(75, 75),
                    Location = new Point(rand.Next(50, 101) * i, rand.Next(50, 101) * i),
                    BackColor = SystemColors.ButtonHighlight
                };
                Console.WriteLine(button.Location);
                panel1.Controls.Add(button);

                i += 2;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            panel1.AutoScrollPosition = new Point(0, 0);
            panel1.Refresh();

            // Autosize so everything inside the panel is captured
            panel1.AutoSize = true;
            panel1.Refresh();

            using (Bitmap bmp = new Bitmap(panel1.Width, panel1.Height))
            {
                panel1.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PNG Image|*.png";
                    saveFileDialog.Title = "Save Panel Image";
                    saveFileDialog.FileName = $"Wiki_Map_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        bmp.Save(saveFileDialog.FileName, ImageFormat.Png);
                        MessageBox.Show($"Panel image saved to {saveFileDialog.FileName}");
                    }
                }
            }

            // Reset AutoSize
            panel1.AutoSize = false;
            panel1.Refresh();
        }
    }
}
