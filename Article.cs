using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HtmlNode = HtmlAgilityPack.HtmlNode;

namespace Wikisplorer
{
    public class Article
    {
        private string title;
        private HtmlNode content;
        private string link;
        private string fullText;
        Dictionary<string, int> anchorsCount;

        public string Title
        {
            get { return title; }
        }

        public string Link
        {
            get { return link; }
        }

        public Dictionary<string, int> AnchorsCount
        {
            get { return anchorsCount; }
        }

        public Article(string url)
        {
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync(url).Result;

            // No article with the link exists
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Article does not exist.");
            }

            // Continue scraping if article exists
            link = url;
            title = ExtractTitle(url);

            // Get the article html document
            var html = httpClient.GetStringAsync(url).Result;
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);

            // Get text of the article
            content = htmlDocument.GetElementbyId("mw-content-text");
            var paragraphs = content.Descendants("p").ToList();

            fullText = LoadText(paragraphs);
            anchorsCount = CountAnchors(paragraphs, fullText);
        }

        // Get title of article from link
        private string ExtractTitle(string url)
        {
            return url.Split('/').Last().Replace("_", " ");
        }

        // Returns a string of the article text
        static string LoadText(List<HtmlAgilityPack.HtmlNode> paragraphs)
        {
            List<string> listText = new List<string>();

            foreach (var paragraph in paragraphs)
            {
                // Decode HTML entities in the inner text
                string decodedText = HtmlAgilityPack.HtmlEntity.DeEntitize(paragraph.InnerText);
                listText.Add(decodedText);
            }

            return string.Join("", listText.ToArray()).ToLower();
        }

        // Return a dictionary where
        // the key is the title of the linked topic
        // and the value is the number of times the title of the linked topic appears in the article
        static Dictionary<string, int> CountAnchors(List<HtmlAgilityPack.HtmlNode> paragraphs, string fullText)
        {
            Dictionary<string, int> linksCount = new Dictionary<string, int>();

            foreach (var paragraph in paragraphs)
            {
                var anchorTags = paragraph.SelectNodes("//p/a").ToList();

                foreach (var anchor in anchorTags)
                {
                    // Decode HTML entities in the anchor title and text
                    string linkTitle = HtmlAgilityPack.HtmlEntity.DeEntitize(anchor.GetAttributeValue("title", "null"));

                    int titleCount = Regex.Matches(fullText, Regex.Escape(linkTitle.ToLower())).Count;

                    // Check if the key already exists
                    if (linksCount.ContainsKey(linkTitle))
                    {
                        continue;
                    }
                    else
                    {
                        // COUNTRY MUSIC IN THE ARTICLE IS JUST REFFERED TO AS COUNTRY
                        // WHICH RESULTS IN A COUNT OF 43
                        // SO WE CANNOT COUNT LINK TEXT + TITLE

                        // In cases where the link finds zero matches
                        // We know the link appears at least once in the article
                        if (titleCount == 0)
                        {
                            titleCount += 1;
                        }

                        // Add the key with the calculated count
                        linksCount.Add(linkTitle, titleCount);
                    }
                }

            }
            return linksCount;
        }

        public override string ToString()
        {
            return $"Article: {Title} ({link})";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Article other = (Article)obj;
            return Link == other.Link;
        }

        public override int GetHashCode()
        {
            // Combination of article title and link to generate its hash code
            return (Link?.GetHashCode() ?? 0);
        }
    }
}
