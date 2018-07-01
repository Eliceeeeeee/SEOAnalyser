using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SEOAnalyser.Web.Models
{
	public class SeoAnalyserMainModel
	{
		public InputViewModel InputViewModel { get; set; }
		public OutputViewModel OutputViewModel { get; set; }
	}

	public class InputViewModel
	{
		[Required]
		public string Input { get; set; }
		public bool IsFilterStopWords { get; set; }
		public bool IsCalculateNoOfWordInPage { get; set; }
		public bool IsCalculateNoOfWordInMetaTags { get; set; }
		public bool IsCalculateNoOfExternalLinks { get; set; }
	}

	public class OutputViewModel
	{
		public IEnumerable<WordsInPageViewModel> WordsInPages { get; set; }
		public IEnumerable<WordsInMetaTagsViewModel> WordsInMetaTags { get; set; }
		public IEnumerable<ExternalLinksViewModel> ExternalLinks { get; set; }
	}
}