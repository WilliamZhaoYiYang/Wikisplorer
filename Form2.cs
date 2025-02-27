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
        private Dictionary<Button, List<Button>> linkedArticles;
        Dictionary<string, Button> articleButtons;   // Store buttons in a dictionary for quick lookup

        public Map(WWScraper Wiki, Article LastArticle)
        {
            wiki = Wiki;
            lastArticle = LastArticle;
            linkedArticles = new Dictionary<Button, List<Button>>();
            articleButtons = new Dictionary<string, Button>();

            InitializeComponent();
            InitializeButtons();
            FillLinkedArticles();

            panel1.Paint += Panel1_Paint;
        }

        private void InitializeButtons()
        {
            int buttonSize = 75;
            int padding = 20; // Minimum spacing between buttons
            int maxAttempts = 50; // Avoid infinite loops if space is tight
            int listSize = wiki.ArticlesList.Count > 10 ? wiki.ArticlesList.Count : 10;
            List<Point> occupiedPositions = new List<Point>();

            foreach (Article article in wiki.ArticlesList)
            {
                Point location;
                int attempts = 0;
                bool validPosition;

                do
                {
                    // Generate a random position based on number of buttons
                    location = new Point(rand.Next(50, panel1.Width * (listSize / 10) - buttonSize - 50),
                                         rand.Next(50, panel1.Height * (listSize / 10) - buttonSize - 50));

                    // Check if the new position is far enough from existing buttons
                    validPosition = !occupiedPositions.Any(p =>
                        Math.Abs(p.X - location.X) < buttonSize + padding &&
                        Math.Abs(p.Y - location.Y) < buttonSize + padding);

                    attempts++;
                }
                while (!validPosition && attempts < maxAttempts);

                // Add button to the panel
                Button button = new Button
                {
                    Name = article.Title,
                    Text = article.Title,
                    Size = new Size(buttonSize, buttonSize),
                    Location = location,
                    BackColor = SystemColors.ButtonHighlight
                };

                panel1.Controls.Add(button);
                occupiedPositions.Add(location);
            }

            Console.WriteLine("Buttons added");
        }

        private void Panel1_Paint(object? sender, PaintEventArgs e)
        {
            DrawLineBetweenButtons(e.Graphics);
        }

        private void FillLinkedArticles()
        {
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
                List<Button> linkedButtons = new List<Button>();

                foreach (KeyValuePair<string, int> link in article.AnchorsCount)
                {

                    // Link exists as an article we've scraped
                    if (articleButtons.TryGetValue(link.Key, out Button? toButton))
                    {
                        Console.WriteLine($"{article.Title} contains the article: {link.Key}");
                        linkedButtons.Add(toButton);
                    }
                }

                // Add the final dictionary containing all buttons linked to the fromButton
                linkedArticles.Add(fromButton, linkedButtons);
            }
        }

        private void DrawLineBetweenButtons(Graphics g)
        {
            Console.WriteLine("Painting");

            foreach (KeyValuePair<Button, List<Button>> Buttons in linkedArticles)
            {
                Button fromButton = Buttons.Key;
                List<Button> toButtons = Buttons.Value;

                foreach (Button toButton in toButtons)
                {
                    // Get the center positions of both buttons
                    Point fromPoint = new Point(fromButton.Left + fromButton.Width / 2, fromButton.Top + fromButton.Height / 2);
                    Point toPoint = new Point(toButton.Left + toButton.Width / 2, toButton.Top + toButton.Height / 2);

                    Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
                    g.DrawLine(blackPen, fromPoint, toPoint);
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
