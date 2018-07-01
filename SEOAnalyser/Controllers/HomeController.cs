using AutoMapper;
using System.Collections.Generic;
using System.Web.Mvc;
using SEOAnalyser.Web.Models;
using SEOAnalyser.Business.Interfaces;

namespace SEOAnalyser.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IValidateService _validateService;
		private readonly IAnalysisService _analysisService;
		private readonly IMapper _mapper;

		public HomeController(IValidateService validateService, IAnalysisService analysisService, IMapper mapper)
		{
			_validateService = validateService;
			_analysisService = analysisService;
			_mapper = mapper;
		}

		public ActionResult Index()
		{
			var model = new SeoAnalyserMainModel();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Analysis(SeoAnalyserMainModel model)
		{
			if (ModelState.IsValid)
			{
				bool isUrL = _validateService.ValidUrl(model.InputViewModel.Input);

				if (isUrL)
				{
					if (!_validateService.ValidateWebsite(model.InputViewModel.Input))
					{
						ModelState.AddModelError("Input", "Not a responding website");
						return View("Index", model);
					}
					model.OutputViewModel = AnalysisURL(model.InputViewModel);
				}
				else
				{
					model.OutputViewModel = AnalysisText(model.InputViewModel);
				}
			}
			return View("Index", model);
		}

		
		private OutputViewModel AnalysisText(InputViewModel model)
		{
			var outputModel = new OutputViewModel();
			if (model.IsCalculateNoOfWordInPage)
			{
				outputModel.WordsInPages = _mapper.Map<IEnumerable<WordsInPageViewModel>>(_analysisService.CalculateNoOfWordInPage(model.Input, model.IsFilterStopWords, false));
			}
			return outputModel;
		}

		private OutputViewModel AnalysisURL(InputViewModel model)
		{
			var outputModel = new OutputViewModel();

			if (model.IsCalculateNoOfWordInPage)
			{
				outputModel.WordsInPages = _mapper.Map<IEnumerable<WordsInPageViewModel>>(_analysisService.CalculateNoOfWordInPage(model.Input, model.IsFilterStopWords));
			}
			if (model.IsCalculateNoOfWordInMetaTags)
			{
				outputModel.WordsInMetaTags = _mapper.Map<IEnumerable<WordsInMetaTagsViewModel>>(_analysisService.CalculateNoOfWordsInMetaTags(model.Input));
			}
			if (model.IsCalculateNoOfExternalLinks)
			{
				outputModel.ExternalLinks = _mapper.Map<IEnumerable<ExternalLinksViewModel>>(_analysisService.CalculateNoOfExternalLinks(model.Input));
			}
			return outputModel;
		}
	}
}