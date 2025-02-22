using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;

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
            InitializeButtons();

            panel1.Paint += Panel1_Paint;
        }

        private void InitializeButtons()
        {
            // Create buttons based on the number of articles scraped
            int i = 1;

            foreach (Article article in wiki.ArticlesList)
            {
                Button button = new Button
                {
                    Name = article.Title,
                    Text = article.Title,
                    Size = new Size(75, 75),
                    Location = new Point(rand.Next(50, 101) * i, rand.Next(50, 101) * i),
                    BackColor = SystemColors.ButtonHighlight
                };
                panel1.Controls.Add(button);

                i += 2;
            }

            Console.WriteLine("Buttons added");
        }

        private void Panel1_Paint(object? sender, PaintEventArgs e)
        {
            DrawLineBetweenButtons(e.Graphics);
        }

        private void DrawLineBetweenButtons(Graphics g)
        {
            Console.WriteLine("Painting");
            Dictionary<string, Button> articleButtons = new Dictionary<string, Button>();

            // Store buttons in a dictionary for quick lookup
            foreach (Control control in panel1.Controls)
            {
                if (control is Button button)
                {
                    articleButtons[button.Name] = button; // Article title as the key
                }
            }


            // Find all links between articles
            foreach (Article article in wiki.ArticlesList)
            {
                Button fromButton = articleButtons[article.Title]; // The source button

                foreach (KeyValuePair<string, int> link in article.AnchorsCount)
                {
                    
                    // Linked article exists in our dictionary of scraped articles
                    if (articleButtons.TryGetValue(link.Key, out Button? toButton))
                    {
                        Console.WriteLine($"{article.Title}, {link.Key}");
                        // Get the center positions of both buttons
                        Point fromPoint = new Point(fromButton.Left + fromButton.Width / 2, fromButton.Top + fromButton.Height / 2);
                        Point toPoint = new Point(toButton.Left + toButton.Width / 2, toButton.Top + toButton.Height / 2);

                        g.DrawLine(Pens.Black, fromPoint, toPoint);
                    }
                }
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
