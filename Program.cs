using Wikisplorer.Models;
using static System.Windows.Forms.LinkLabel;

namespace Wikisplorer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            //WWScraper wiki = new WWScraper();
            //wiki.AddArticle("https://en.wikipedia.org/wiki/New_Zealand");
            //wiki.AddArticle("https://en.wikipedia.org/wiki/Australia");
            //wiki.AddArticle("https://en.wikipedia.org/wiki/United_States");
            //wiki.AddArticle("https://en.wikipedia.org/wiki/G20");

            //Application.Run(new Map(wiki, wiki.ArticlesSet.Last()));
            Application.Run(new Form1());
        }
    }
}