﻿using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Wikisplorer.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;

namespace Wikisplorer
{
    public partial class Map : Form
    {
        private WWScraper wiki;
        private Article lastArticle;
        private Random rand = new Random();
        private Dictionary<Button, List<Button>> linkedArticles; // Any links between scraped articles stored in dictionary
        Dictionary<string, Button> articleButtons;   // buttons in dictionary for quick lookup
        private List<Line> lines = new List<Line>();

        public event EventHandler MapCleared;
        protected virtual void OnMapCleared()
        {
            MapCleared?.Invoke(this, EventArgs.Empty);
        }

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
            int listSize = wiki.ArticlesSet.Count > 10 ? wiki.ArticlesSet.Count : 10;
            List<Point> occupiedPositions = new List<Point>();

            foreach (Article article in wiki.ArticlesSet)
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

                // Event handlers for the created buttons
                button.MouseEnter += articleButton_MouseEnter;
                button.MouseLeave += articleButton_MouseLeave;
                button.MouseClick += articleButton_Click;

                panel1.Controls.Add(button);
                occupiedPositions.Add(location);
            }
        }

        private void Panel1_Paint(object? sender, PaintEventArgs e)
        {
            DrawLineBetweenButtons(e.Graphics);
        }

        private void FillLinkedArticles()
        {
            // articleButtons for tracking buttons on map panel
            foreach (Control control in panel1.Controls)
            {
                if (control is Button button)
                {
                    articleButtons[button.Name] = button; // Article title as the key
                }
            }

            // Find all links between scraped articles
            foreach (Article article in wiki.ArticlesSet)
            {
                Button fromButton = articleButtons[article.Title]; // The source button
                List<Button> linkedButtons = new List<Button>();

                foreach (KeyValuePair<string, int> link in article.AnchorsCount)
                {
                    // Link exists as an article we've scraped
                    if (articleButtons.TryGetValue(link.Key, out Button? toButton))
                    {
                        linkedButtons.Add(toButton);

                        // Create a line between the buttons
                        Point fromPoint = new Point(fromButton.Left + fromButton.Width / 2, fromButton.Top + fromButton.Height / 2);
                        Point toPoint = new Point(toButton.Left + toButton.Width / 2, toButton.Top + toButton.Height / 2);

                        // Create a line and add it to list for quick lookup
                        Line line = new Line(fromPoint, toPoint, Color.Black);
                        lines.Add(line);
                    }
                }

                // Add the final dictionary containing all buttons linked to the fromButton
                linkedArticles.Add(fromButton, linkedButtons);
            }
        }

        private void DrawLineBetweenButtons(Graphics g)
        {

            foreach (var line in lines)
            {
                // Use the line's current color
                using (Pen pen = new Pen(line.Color, 2))
                {
                    g.DrawLine(pen, line.Start, line.End);
                }
            }
        }

        private void RecolorLine(Point start, Point end, Color newColor)
        {
            // Find all lines with the specified start and end points
            var linesToRecolor = lines.Where(line => line.Start == start && line.End == end).ToList();

            // Recolor all matching lines
            foreach (var line in linesToRecolor)
            {
                line.Color = newColor;
            }
        }

        private void articleButton_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Button hoveredButton)
            {
                // Highlights all connections between article buttons
                var connectedLines = lines.Where(line =>
                    (line.Start == new Point(hoveredButton.Left + hoveredButton.Width / 2, hoveredButton.Top + hoveredButton.Height / 2)) ||
                    (line.End == new Point(hoveredButton.Left + hoveredButton.Width / 2, hoveredButton.Top + hoveredButton.Height / 2))
                ).ToList();

                foreach (var line in connectedLines)
                {
                    RecolorLine(line.Start, line.End, Color.Red);
                }
                panel1.Refresh();
            }
        }

        // Unhighlight on mouse leave
        private void articleButton_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Button hoveredButton)
            {
                // Find all lines that starts at the hovered button
                var connectedLines = lines.Where(line =>
                    (line.Start == new Point(hoveredButton.Left + hoveredButton.Width / 2, hoveredButton.Top + hoveredButton.Height / 2)) ||
                    (line.End == new Point(hoveredButton.Left + hoveredButton.Width / 2, hoveredButton.Top + hoveredButton.Height / 2))
                ).ToList();

                // Reset the connected lines to black
                foreach (var line in connectedLines)
                {
                    RecolorLine(line.Start, line.End, Color.Black);
                }
                panel1.Refresh();
            }
        }

        // On click, open the wikipedia article
        private void articleButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button clickedButton)
            {
                string titleURL = clickedButton.Name.Replace(" ", "_");
                string target = "https://en.wikipedia.org/wiki/" + titleURL;

                try
                {
                    ProcessStartInfo psInfo = new ProcessStartInfo
                    {
                        FileName = target,
                        UseShellExecute = true
                    };
                    Process.Start(psInfo);
                }
                catch (System.ComponentModel.Win32Exception noBrowser)
                {
                    if (noBrowser.ErrorCode == -2147467259)
                        MessageBox.Show(noBrowser.Message);
                }
                catch (System.Exception other)
                {
                    MessageBox.Show(other.Message);
                }
            }
        }

        // Capture the map as an image
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

        private void refreshButton_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            linkedArticles.Clear();
            lines.Clear();

            // Reinitialize
            InitializeButtons();
            FillLinkedArticles();

            panel1.Refresh();
        }

        private void saveToJSON_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                saveFileDialog.Title = "Save as JSON file";
                saveFileDialog.FileName = $"Wiki_Map_JSON_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create an object with the data we want to save
                        var dataToSave = new
                        {
                            Articles = wiki.ArticlesSet.Select(a => new
                            {
                                Title = a.Title,
                                Link = a.Link,
                                Anchors = a.AnchorsCount
                            }).ToList(),
                            Connections = lines.Select(l => new
                            {
                                From = GetButtonAtPoint(l.Start)?.Name,
                                To = GetButtonAtPoint(l.End)?.Name
                            }).Where(c => c.From != null && c.To != null).ToList()
                        };

                        // Serialize to JSON
                        var options = new System.Text.Json.JsonSerializerOptions
                        {
                            WriteIndented = true
                        };
                        string json = System.Text.Json.JsonSerializer.Serialize(dataToSave, options);

                        // Write to file
                        File.WriteAllText(saveFileDialog.FileName, json);

                        MessageBox.Show($"JSON file saved to {saveFileDialog.FileName}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving JSON file: {ex.Message}");
                    }
                }
            }
        }

        private void loadAsJSON_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.Title = "Load JSON file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Read JSON file
                        string json = File.ReadAllText(openFileDialog.FileName);

                        // Deserialize JSON
                        var options = new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var loadedData = System.Text.Json.JsonSerializer.Deserialize<WikiMapData>(json, options);

                        panel1.Controls.Clear();
                        linkedArticles.Clear();
                        articleButtons.Clear();
                        lines.Clear();
                        wiki.ArticlesSet.Clear();

                        // Notify Form1 to clear its controls
                        OnMapCleared();

                        // Recreate articles
                        foreach (var articleData in loadedData.Articles)
                        {
                            try
                            {
                                var article = new Article(articleData.Link);
                                wiki.ArticlesSet.Add(article);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Failed to load article {articleData.Title}: {ex.Message}");
                            }
                        }

                        InitializeButtons();

                        foreach (var connection in loadedData.Connections)
                        {
                            if (articleButtons.TryGetValue(connection.From, out var fromButton) &&
                                articleButtons.TryGetValue(connection.To, out var toButton))
                            {
                                Point fromPoint = new Point(
                                    fromButton.Left + fromButton.Width / 2,
                                    fromButton.Top + fromButton.Height / 2);
                                Point toPoint = new Point(
                                    toButton.Left + toButton.Width / 2,
                                    toButton.Top + toButton.Height / 2);

                                lines.Add(new Line(fromPoint, toPoint, Color.Black));
                            }
                        }

                        // Rebuild the linkedArticles dictionary
                        linkedArticles.Clear();
                        FillLinkedArticles();

                        panel1.Refresh();
                        MessageBox.Show("Map loaded successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading JSON file: {ex.Message}");
                    }
                }
            }
        }

        // Find a button at a given center point
        private Button GetButtonAtPoint(Point centerPoint)
        {
            foreach (Control control in panel1.Controls)
            {
                if (control is Button button)
                {
                    // Calculate button's center point
                    Point buttonCenter = new Point(
                        button.Left + button.Width / 2,
                        button.Top + button.Height / 2);

                    if (buttonCenter == centerPoint)
                    {
                        return button;
                    }
                }
            }
            return null;
        }
    }

    // Hold deserialized data for loading JSON files
    public class WikiMapData
    {
        public List<ArticleData> Articles { get; set; }
        public List<ConnectionData> Connections { get; set; }
    }

    public class ArticleData
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public Dictionary<string, int> Anchors { get; set; }
    }

    public class ConnectionData
    {
        public string From { get; set; }
        public string To { get; set; }
    }
}
