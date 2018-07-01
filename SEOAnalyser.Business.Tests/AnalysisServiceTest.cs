using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEOAnalyser.Business.Model;
using System.Collections.Generic;
using System.Linq;

namespace SEOAnalyser.Web.Business.Tests
{
	[TestClass]
	public class AnalysisServiceTest
	{
		private AnalysisService _target;

		[TestInitialize]
		public void TestInitialize()
		{
			_target = new AnalysisService();
		}

		[TestMethod]
		public void ConstructorTest()
		{
			// Assert
			Assert.IsNotNull(_target);
		}

		[TestMethod]
		public void CalculateNoOfWordInPage_IsValidUrl_ReturnNoOfWordInPage()
		{
			// Act
			var result = _target.CalculateNoOfWordInPage("https://www.google.com", true, true).ToList();

			// Assert
			Assert.IsNotNull(result);
			Assert.AreNotEqual(0, result.Count);
		}

		[TestMethod]
		public void CalculateNoOfWordInPage_IsInputText_ReturnNoOfWordInPage()
		{
			var actualResult = new List<WordsInPage>
			{
				GetWordsInPage("world", 2),
				GetWordsInPage("Hello", 1)
			};

			var result = _target.CalculateNoOfWordInPage("Hello The world world", true, false).ToList();
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Count());
			Assert.AreEqual(actualResult.FirstOrDefault().NoOfWordInPage, result.FirstOrDefault().NoOfWordInPage);
			Assert.AreEqual(actualResult.FirstOrDefault().WordInPage, result.FirstOrDefault().WordInPage);
		}

		private WordsInPage GetWordsInPage(string wordInPage, int noOfWord)
		{
			return new WordsInPage
			{
				NoOfWordInPage = noOfWord,
				WordInPage = wordInPage
			};
		}

		[TestMethod]
		public void CalculateNoOfWordsInMetaTags_IsValidUrl_ReturnNoOfWordsInMetaTags()
		{
			// Act
			var result = _target.CalculateNoOfWordsInMetaTags("https://www.google.com").ToList();

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void CalculateNoOfExternalLinks_IsValidUrl_ReturnNoOfExternalLinks()
		{
			var actualResult = new List<ExternalLinks>
			{
				GetExternalLinks("http://www.exact.com/global/terms-and-conditions-of-use", 1)
			};

			var result = _target.CalculateNoOfExternalLinks("https://apps.exactonline.com/nl/nl-NL/V2").ToList();
			Assert.IsNotNull(result);
			Assert.AreEqual(actualResult.FirstOrDefault().NoOfExternalLinks, result.FirstOrDefault().NoOfExternalLinks);
			Assert.AreEqual(actualResult.FirstOrDefault().ExternalLink, result.FirstOrDefault().ExternalLink);
		}

		private ExternalLinks GetExternalLinks(string externalLink, int noOfLink)
		{
			return new ExternalLinks
			{
				NoOfExternalLinks = noOfLink,
				ExternalLink = externalLink
			};
		}
	}
}
