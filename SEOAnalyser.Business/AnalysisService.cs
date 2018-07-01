using HtmlAgilityPack;
using SEOAnalyser.Business;
using SEOAnalyser.Business.Interfaces;
using SEOAnalyser.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SEOAnalyser.Web.Business
{
	public class AnalysisService : IAnalysisService
	{
		private string[] StopWords = Constants.StopWord.Split(',').ToArray();
		private char[] Seperators = { '.', '?', '!', ' ', ';', ':', ',' , '\'','\"', '{', '}', '(',')', '-', '_','@', '#', '$', '%', '^', '&','*', '[', ']', '\\', '|', '=', '/', '+'};
		private Regex meta = new Regex(@"<meta\s*(?:(?:\b(\w|-)+\b\s*(?:=\s*(?:""[^""]*""|'" +
						  @"[^']*'|[^""'<> ]+)\s*)?)*)/?\s*>");

		public AnalysisService()
		{
		}

		private string FilterStopWords(string input)
		{
			foreach (string word in StopWords)
			{
				string regexp = @"(?i)\s?\b" + word + @"\b\s?";
				input = Regex.Replace(input, regexp, " ");
			}
			return input;
		}

		public IEnumerable<WordsInPage> CalculateNoOfWordInPage(string input, bool isFilterStopWords, bool isUrl = true)
		{
			if (isUrl)
			{
				return CalculateNoOfWordInPageForUrl(input, isFilterStopWords);
			}

			return CalculateNoOfWordInPageForText(input, isFilterStopWords);
		}

		private IEnumerable<WordsInPage> CalculateNoOfWordInPageForUrl(string input, bool isFilterStopWords)
		{
			List<string> wordsInPageCollection = new List<string>();
			var doc = new HtmlWeb().Load(input);
			var nodes = doc.DocumentNode.SelectSingleNode("//body").DescendantsAndSelf();
			foreach (var node in nodes)
			{
				if (node.NodeType == HtmlNodeType.Text && node.ParentNode.Name != "script")
				{
					var innerText = node.InnerText.Replace(Environment.NewLine, "").Trim();
					innerText = Regex.Replace(innerText, "<.*?>", String.Empty);
					innerText = Regex.Replace(innerText, "\\\".*?\\\":", String.Empty);
					if (isFilterStopWords)
					{
						innerText = FilterStopWords(innerText);
					}
					string[] temps = innerText.Split(Seperators, StringSplitOptions.RemoveEmptyEntries);
					foreach (var temp in temps)
					{
						if (temp != String.Empty)
						{
							wordsInPageCollection.Add(temp);
						}
					}
				}
			}

			return wordsInPageCollection.GroupBy(r => r)
				.Select(grp => new WordsInPage
				{
					WordInPage = grp.Key,
					NoOfWordInPage = grp.Count()
				})
				.Where(x => x.WordInPage.Trim() != String.Empty)
				.OrderByDescending(x => x.NoOfWordInPage).ThenBy(y => y.WordInPage);
		}

		private IEnumerable<WordsInPage> CalculateNoOfWordInPageForText(string input, bool isFilterStopWords)
		{
			if (isFilterStopWords)
			{
				input = FilterStopWords(input);
			}

			string[] resultAraray = input.Split(Seperators, StringSplitOptions.RemoveEmptyEntries);

			return resultAraray.GroupBy(r => r)
				.Select(grp => new WordsInPage
				{
					WordInPage = grp.Key,
					NoOfWordInPage = grp.Count()
				})
				.Where(x => x.WordInPage.Trim() != String.Empty)
				.OrderByDescending(x => x.NoOfWordInPage).ThenBy(y => y.WordInPage);
		}

		public IEnumerable<WordsInMetaTag> CalculateNoOfWordsInMetaTags(string input)
		{
			List<string> wordsInMetaTagCollection = new List<string>();
			var doc = new HtmlWeb().Load(input);
			var metaTags = doc.DocumentNode.SelectNodes("//meta");
			if (metaTags != null)
			{
				foreach (var tag in metaTags)
				{
					if (tag.Attributes["name"] != null && tag.Attributes["content"] != null && tag.Attributes["name"].Value == "description")
					{
						wordsInMetaTagCollection.Add(tag.Attributes["content"].Value);
					}

					if (tag.Attributes["name"] != null && tag.Attributes["content"] != null && tag.Attributes["name"].Value == "keywords")
					{
						wordsInMetaTagCollection.Add(tag.Attributes["content"].Value);
					}
				}
			}

			return wordsInMetaTagCollection.GroupBy(r => r)
				.Select(grp => new WordsInMetaTag
				{
					WordInMetaTag = grp.Key,
					NoOfWordInMetaTags = grp.Count()
				})
				.Where(x => x.WordInMetaTag.Trim() != String.Empty)
				.OrderBy(x => x.NoOfWordInMetaTags).ThenBy(y => y.WordInMetaTag);
		}

		public IEnumerable<ExternalLinks> CalculateNoOfExternalLinks(string input)
		{
			var doc = new HtmlWeb().Load(input);

			List<string> externalLinksCollection = new List<string>();
			foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
			{
				var value = link.Attributes["href"].Value;
				if(value.StartsWith("http"))
				{
					externalLinksCollection.Add(link.Attributes["href"].Value);
				}
			}

			return externalLinksCollection.GroupBy(r => r)
				.Select(grp => new ExternalLinks
				{
					ExternalLink = grp.Key,
					NoOfExternalLinks = grp.Count()
				})
				.Where(x => x.ExternalLink.Trim() != "#")
				.OrderBy(x => x.NoOfExternalLinks).ThenBy(y => y.ExternalLink);
		}
	}
}
