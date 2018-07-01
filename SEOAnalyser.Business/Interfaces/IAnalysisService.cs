using SEOAnalyser.Business.Model;
using System.Collections.Generic;

namespace SEOAnalyser.Business.Interfaces
{
	public interface IAnalysisService
	{
		IEnumerable<WordsInPage> CalculateNoOfWordInPage(string Input, bool isFilterStopWords, bool isUrl = true);
		IEnumerable<WordsInMetaTag> CalculateNoOfWordsInMetaTags(string Input);
		IEnumerable<ExternalLinks> CalculateNoOfExternalLinks(string Input);
	}
}
