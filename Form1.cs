// TODO
// Add a refresh button to the map form so positions can be randomized again
// On button hover, show basic info about article at the bottom of map
// On button click, go to the wikipedia article
// Add a save as JSON file so that previous generated maps can be loaded in
// On hover over save as file button, describe what saving as JSON does

using System.Media;
using System.Text.RegularExpressions; 

namespace Wikisplorer
{
    public partial class Form1 : Form
    {
        private Map map;

        private WWScraper wiki;
        private Article lastArticle;

        public Form1()
        {
            InitializeComponent();

            wiki = new WWScraper();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        // Textbox is selected
        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1_SetText(
                "",
                new Font(textBox1.Font, FontStyle.Regular),
                System.Drawing.Color.Black,
                false);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter pressed
            if (e.KeyChar == '\r')
            {
                GetWikipediaArticle(textBox1.Text);
            }
        }
        private void textBox1_SetText(string text, Font font, Color color, bool error)
        {
            if (error)
            {
                SystemSounds.Hand.Play();
            }
            textBox1.Text = text;
            textBox1.Font = font;
            textBox1.ForeColor = color;
        }

        private void enterLink_Click(object sender, EventArgs e)
        {
            GetWikipediaArticle(textBox1.Text);
        }

        // Lose focus on textbox when clicked off anywhere
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            this.ActiveControl = null;
            textBox1_SetText(
                "Enter a Wikipedia link",
                new Font(textBox1.Font, FontStyle.Italic),
                System.Drawing.Color.Gray,
                false);
        }

        private bool ValidWikipediaLink(string link, out string errorMessage)
        {
            // Confirm link string is not empty
            if (string.IsNullOrWhiteSpace(link))
            {
                errorMessage = "Please enter a link";
                return false;
            }

            // Confirm link is valid
            string pattern = @"^https:\/\/en\.wikipedia\.org\/wiki\/.+$";
            if (Regex.IsMatch(link, pattern))
            {
                errorMessage = "";
                return true;
            }
            else
            {
                errorMessage = "Invalid link. Please ensure it starts with 'https://en.wikipedia.org/wiki/'";
                return false;
            }
        }

        private async void GetWikipediaArticle(string link)
        {
            string errorMsg;
            // Check if valid link exists in textbox
            if (ValidWikipediaLink(textBox1.Text, out errorMsg))
            {
                // Show loading form
                LoadingForm loadingForm = new LoadingForm();
                loadingForm.Show();
                loadingForm.Refresh();
                DisableAllControls(this);

                // Run web scraping in a separate thread to avoid blocking the UI
                bool articleAdded = await Task.Run(() => wiki.AddArticle(link));

                loadingForm.Close();
                EnableAllControls(this);

                if (articleAdded)
                {
                    // article exists
                    this.ActiveControl = null;
                    textBox1_SetText(
                        "Article successfully added",
                        new Font(textBox1.Font, FontStyle.Bold),
                        System.Drawing.Color.Green,
                        false);

                    lastArticle = wiki.ArticlesSet.Last();
                    DisplayTitle();
                    DisplayLinks(lastArticle.AnchorsCount);
                }
                else
                {
                    // article not found
                    this.ActiveControl = null;
                    textBox1_SetText(
                        "Article does not exist or has already been scraped",
                        new Font(textBox1.Font, FontStyle.Bold),
                        System.Drawing.Color.Red,
                        true);
                }
            }
            else
            {
                // Not a valid link
                this.ActiveControl = null;
                textBox1_SetText(
                    errorMsg,
                    new Font(textBox1.Font, FontStyle.Bold),
                    System.Drawing.Color.Red,
                    true);
            }
        }

        // Display title of last entered article
        private void DisplayTitle()
        {
            string title = lastArticle.Title;
            label2.Text = $"Last entered article: {title}";
        }

        // Display links of last entered article
        private void DisplayLinks(Dictionary<string, int> anchorsCount)
        {
            textBox2.Text = "";

            foreach (KeyValuePair<string, int> element in anchorsCount)
            {
                // Console.WriteLine("{0}: {1}\n", element.Key, element.Value);
                textBox2.AppendText($"{element.Key}: {element.Value} {Environment.NewLine}");
            }

            textBox2.SelectionStart = 0;
            textBox2.ScrollToCaret();
        }

        // To sort by order of appearance, we'd just use the original anchorsCount
        // To sort by ascending, we'd reverse the order of the original anchorsCount
        private Dictionary<string, int> SortLinks()
        {
            Dictionary<string, int>? sortedDict = null;

            if (comboBox1.SelectedIndex == 0) // Sort by order of appearance
            {
                if (comboBox2.SelectedIndex == 0)
                {
                    // Descending (last to appear)
                    sortedDict = lastArticle.AnchorsCount
                        .Reverse()
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                else
                {
                    // Ascending (first to appear)
                    sortedDict = lastArticle.AnchorsCount;
                }
            }
            else if (comboBox1.SelectedIndex == 1)  // Sort by title of link
            {
                if (comboBox2.SelectedIndex == 0)
                {
                    // Descending
                    sortedDict = lastArticle.AnchorsCount.ToList()
                            .OrderByDescending(kvp => kvp.Key)
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                else
                {
                    // Ascending
                    sortedDict = lastArticle.AnchorsCount.ToList()
                            .OrderBy(kvp => kvp.Key)
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
            }
            else if (comboBox1.SelectedIndex == 2)  // Sort by count
            {
                if (comboBox2.SelectedIndex == 0)
                {
                    // Descending
                    sortedDict = lastArticle.AnchorsCount.ToList()
                            .OrderByDescending(kvp => kvp.Value)
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                else
                {
                    // Ascending
                    sortedDict = lastArticle.AnchorsCount.ToList()
                            .OrderBy(kvp => kvp.Value)
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
            }

            return sortedDict;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lastArticle == null)
            {
                textBox1_SetText(
                "No article found, please enter a link",
                new Font(textBox1.Font, FontStyle.Bold),
                System.Drawing.Color.Red,
                true);
                return;
            }

            var sortedDict = SortLinks();

            if (sortedDict == null)
            {
                textBox1_SetText(
                "Error while sorting, have you filled all the sort by fields?",
                new Font(textBox1.Font, FontStyle.Bold),
                System.Drawing.Color.Red,
                true);
                return;
            }

            DisplayLinks(sortedDict);
        }

        // Generate map
        private void button2_Click(object sender, EventArgs e)
        {
            if (wiki.ArticlesSet.Count() > 0)
            {
                map = new Map(wiki, lastArticle);
                map.Show();
            } else
            {
                textBox1_SetText(
                "No articles entered to generate a map from",
                new Font(textBox1.Font, FontStyle.Bold),
                System.Drawing.Color.Red,
                true);
            }
        }

        private void DisableAllControls(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                control.Enabled = false;
                if (control.HasChildren)
                {
                    DisableAllControls(control);
                }
            }
        }

        private void EnableAllControls(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                control.Enabled = true;
                if (control.HasChildren)
                {
                    DisableAllControls(control);
                }
            }
        }
    }
}

// https://en.wikipedia.org/wiki/New_Zealand