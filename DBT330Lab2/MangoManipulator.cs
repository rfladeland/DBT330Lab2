using MongoDB.Driver;
using HtmlAgilityPack;
using System.Collections.Generic;
using System;
using System.IO;

namespace DBT330Lab2
{
    public class MangoManipulator
    {
        //https://download.bls.gov/pub/time.series/wm/
        private static string webPrefix = "https://download.bls.gov";
        public MongoClient Client { get; private set; }
        public MangoManipulator()
        {
            Client = new MongoClient("mongodb://dbt330:emSEErTliC4SRRqY4M6XC3UvyEQw3PAKYpJ2hEtBWxkWxEREyqgqrU57VSl8xVreAsMwbq91XgPESEJaTKVOzw==@dbt330.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            System.Console.WriteLine("Connected to mongo Client");
        }

        public void Run()
        {
            System.Console.WriteLine("Please enter a valid BLS website. ");
            string website = System.Console.ReadLine();
            var contents = GetHtmlDocument(website);
            List<string> individualFiles = new List<string>();
            individualFiles.AddRange(GetLinksToDownLoadFiles(contents));
            DownLoadFiles(individualFiles[0]);
        }

        private List<string> GetLinksToDownLoadFiles(string contents)
        {
            List<string> links = new List<string>();
            var temp = contents.Split('>');
            foreach (var item in temp)
            {
                int index = item.IndexOf('"');
                if(item.Contains(('"').ToString()) && index >= 0)
                {
                    links.Add(webPrefix + item.Substring(index + 1, (item.Length - index - 2)));
                }
            }
            links.RemoveAt(0);
            System.Console.Clear();
            return links;
        }

        public void DownLoadFiles(string url)
        {
            if (!Directory.Exists($"C:\\Temp\\DBT330"))
            {
                string dir = $"C:\\Temp\\DBT330\\{url.Substring(url.LastIndexOf('.') + 1)}.dbm";
                Console.WriteLine(dir);
                Console.WriteLine($"Connecting to {url} and reading data.");
                Console.WriteLine("C:\\Temp\\DBT330  Directory doesn't exits. Creating it now.");
                Directory.CreateDirectory($"C:\\Temp\\DBT330");
                List<string> contents = new List<string>();
                List<string> attributeNames = new List<string>();
                List<DynamicDBModel> modelsList = new List<DynamicDBModel>();
                List<string> attributeContents = new List<string>();
                contents.AddRange(GetHtmlDocument(url).Split('\n'));
                contents.RemoveAt(0);
                contents.RemoveAt(contents.Count - 1);
                contents[0] = contents[0].Substring(contents[0].LastIndexOf(">") + 1);
                attributeNames.AddRange(contents[0].Split('\t'));
                for (int i = 1; i < contents.Count; i++)
                {
                    attributeContents.Clear();
                    attributeContents.AddRange(contents[i].Split('\t'));
                    modelsList.Add(new DynamicDBModel() { AttributeNames = attributeNames, AttributeContents = attributeContents });
                }
                Console.WriteLine($"Creating file {dir}");
                using (StreamWriter writer = new StreamWriter(File.Create(dir)))
                {
                    modelsList.ForEach(x => Console.WriteLine(x.ToString()));
                    modelsList.ForEach(x => writer.WriteLine(x.ToString()));
                }
            }
        }

        public string GetHtmlDocument(string url)
        {
            var webPage = new HtmlWeb();
            HtmlDocument pageContents = new HtmlDocument();
            try
            {
                pageContents = webPage.LoadFromBrowser(url);
            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry that was an invalid url. Please enter a valid one.");
                string newUrl = Console.ReadLine();
                GetHtmlDocument(newUrl);
            }
            return pageContents.ParsedText;
        }

    }
}