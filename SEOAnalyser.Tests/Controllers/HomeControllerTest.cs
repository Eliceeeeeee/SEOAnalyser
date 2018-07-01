using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SEOAnalyser.Business.Interfaces;
using SEOAnalyser.Business.Model;
using SEOAnalyser.Web.Controllers;
using SEOAnalyser.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SEOAnalyser.Web.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		private HomeController _target;
		private Mock<IValidateService> _validateServiceMock;
		private Mock<IAnalysisService> _analysisServiceMock;
		private Mock<IMapper> _mapperMock;

		[TestInitialize]
		public void TestInitialize()
		{
			_validateServiceMock = new Mock<IValidateService>();
			_analysisServiceMock = new Mock<IAnalysisService>();
			_mapperMock = new Mock<IMapper>();

			_target = new HomeController
			(
				_validateServiceMock.Object,
				_analysisServiceMock.Object,
				_mapperMock.Object
			);
		}

		[TestMethod]
		public void Index_LoadPage_Succeed()
		{
			// Act
			ViewResult result = _target.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Analysis_ValidUrl_ReturnViewModel()
		{
			// Arrange
			SeoAnalyserMainModel seoAnalyserMainModel = new SeoAnalyserMainModel();
			seoAnalyserMainModel.InputViewModel = new InputViewModel
			{
				Input = "https://www.youtube.com",
				IsFilterStopWords = true,
				IsCalculateNoOfWordInPage = true,
				IsCalculateNoOfWordInMetaTags = true,
				IsCalculateNoOfExternalLinks = true
			};

			_validateServiceMock.Setup(x => x.ValidUrl(It.IsAny<string>())).Returns(true);
			_validateServiceMock.Setup(x => x.ValidateWebsite(It.IsAny<string>())).Returns(true);
			_mapperMock.Setup(x => x.Map<IEnumerable<WordsInPageViewModel>>(It.IsAny<IEnumerable<WordsInPage>>()))
				.Returns(GetWordsInPageViewModelViewModelList());
			_mapperMock.Setup(x => x.Map<IEnumerable<WordsInMetaTagsViewModel>>(It.IsAny<IEnumerable<WordsInMetaTag>>()))
				.Returns(GetWordsInMetaTagsViewModelList());
			_mapperMock.Setup(x => x.Map<IEnumerable<ExternalLinksViewModel>>(It.IsAny<IEnumerable<ExternalLinks>>()))
				.Returns(GetExternalLinksViewModelList());
			
			// Act
			var result = _target.Analysis(seoAnalyserMainModel) as ViewResult;

			// Assert
			var viewResult = (ViewResult)result;
			var model = (SeoAnalyserMainModel)viewResult.Model;
			Assert.IsNotNull(model.OutputViewModel.WordsInPages);
			Assert.IsNotNull(model.OutputViewModel.WordsInMetaTags);
			Assert.IsNotNull(model.OutputViewModel.ExternalLinks);
			_analysisServiceMock.Verify(m => m.CalculateNoOfWordInPage(It.IsAny<string>(), true, true), Times.Once);
			_analysisServiceMock.Verify(m => m.CalculateNoOfWordsInMetaTags(It.IsAny<string>()), Times.Once);
			_analysisServiceMock.Verify(m => m.CalculateNoOfExternalLinks(It.IsAny<string>()), Times.Once);
		}

		[TestMethod]
		public void Analysis_ValidInutText_ReturnViewModel()
		{
			// Arrange
			SeoAnalyserMainModel seoAnalyserMainModel = new SeoAnalyserMainModel();
			seoAnalyserMainModel.InputViewModel = new InputViewModel
			{
				Input = "Hello the world!",
				IsFilterStopWords = true,
				IsCalculateNoOfWordInPage = true,
				IsCalculateNoOfWordInMetaTags = true,
				IsCalculateNoOfExternalLinks = true
			};

			_validateServiceMock.Setup(x => x.ValidUrl(It.IsAny<string>())).Returns(false);
			_mapperMock.Setup(x => x.Map<IEnumerable<WordsInPageViewModel>>(It.IsAny<IEnumerable<WordsInPage>>()))
				.Returns(GetWordsInPageViewModelViewModelList());

			// Act
			var result = _target.Analysis(seoAnalyserMainModel) as ViewResult;

			// Assert
			var viewResult = (ViewResult)result;
			var model = (SeoAnalyserMainModel)viewResult.Model;
			Assert.IsNotNull(model.OutputViewModel.WordsInPages);
			Assert.IsNull(model.OutputViewModel.WordsInMetaTags);
			Assert.IsNull(model.OutputViewModel.ExternalLinks);
			_validateServiceMock.Verify(m => m.ValidateWebsite(It.IsAny<string>()), Times.Never);
			_analysisServiceMock.Verify(m => m.CalculateNoOfWordInPage(It.IsAny<string>(), true, false), Times.Once);
			_analysisServiceMock.Verify(m => m.CalculateNoOfWordsInMetaTags(It.IsAny<string>()), Times.Never);
			_analysisServiceMock.Verify(m => m.CalculateNoOfExternalLinks(It.IsAny<string>()), Times.Never);
		}

		[TestMethod]
		public void Analysis_WithModelStateError_ReturnViewModelWithErrorMessage()
		{
			// Arrange
			_target.ModelState.AddModelError("Input", "Required");
			SeoAnalyserMainModel seoAnalyserMainModel = new SeoAnalyserMainModel();

			// Act
			var result = _target.Analysis(seoAnalyserMainModel) as ViewResult;

			// Assert
			Assert.AreEqual(typeof(ViewResult), result.GetType());
			var viewResult = (ViewResult)result;
			Assert.AreEqual(typeof(SeoAnalyserMainModel), viewResult.Model.GetType());
			_validateServiceMock.Verify(m => m.ValidUrl(It.IsAny<string>()), Times.Never);
			_validateServiceMock.Verify(m => m.ValidateWebsite(It.IsAny<string>()), Times.Never);
			_analysisServiceMock.Verify(m => m.CalculateNoOfWordInPage(It.IsAny<string>(), true, true), Times.Never);
			_analysisServiceMock.Verify(m => m.CalculateNoOfWordsInMetaTags(It.IsAny<string>()), Times.Never);
			_analysisServiceMock.Verify(m => m.CalculateNoOfExternalLinks(It.IsAny<string>()), Times.Never);
		}


		[TestMethod]
		public void Analysis_NotResponseUrl_ReturnViewModelWithErrorMessage()
		{
			// Arrange
			SeoAnalyserMainModel seoAnalyserMainModel = new SeoAnalyserMainModel();
			seoAnalyserMainModel.InputViewModel = new InputViewModel
			{
				Input = "https://www.youtube.com",
				IsFilterStopWords = true,
				IsCalculateNoOfWordInPage = true,
				IsCalculateNoOfWordInMetaTags = true,
				IsCalculateNoOfExternalLinks = true
			};

			_validateServiceMock.Setup(x => x.ValidUrl(It.IsAny<string>())).Returns(true);
			_validateServiceMock.Setup(x => x.ValidateWebsite(It.IsAny<string>())).Returns(false);

			// Act
			var result = _target.Analysis(seoAnalyserMainModel) as ViewResult;

			// Assert
			Assert.AreEqual(typeof(ViewResult), result.GetType());
			var viewResult = (ViewResult)result;
			Assert.AreEqual(typeof(SeoAnalyserMainModel), viewResult.Model.GetType());
			_validateServiceMock.Verify(m => m.ValidUrl(It.IsAny<string>()), Times.Once);
			_validateServiceMock.Verify(m => m.ValidateWebsite(It.IsAny<string>()), Times.Once);
			_analysisServiceMock.Verify(m => m.CalculateNoOfWordInPage(It.IsAny<string>(), true, true), Times.Never);
			_analysisServiceMock.Verify(m => m.CalculateNoOfWordsInMetaTags(It.IsAny<string>()), Times.Never);
			_analysisServiceMock.Verify(m => m.CalculateNoOfExternalLinks(It.IsAny<string>()), Times.Never);
		}

		private IEnumerable<WordsInPageViewModel> GetWordsInPageViewModelViewModelList()
		{
			return new List<WordsInPageViewModel>
			{
				GetWordsInPageViewModelViewModel("teststring1"),
				GetWordsInPageViewModelViewModel("teststring2"),
				GetWordsInPageViewModelViewModel("teststring3"),
				GetWordsInPageViewModelViewModel("teststring4")
			};
		}

		private WordsInPageViewModel GetWordsInPageViewModelViewModel(string wordInPage)
		{
			Random random = new Random();
			int randomNo = random.Next(1, 100);
			return new WordsInPageViewModel
			{
				NoOfWordInPage = randomNo,
				WordInPage = wordInPage
			};
		}

		private IEnumerable<WordsInMetaTagsViewModel> GetWordsInMetaTagsViewModelList()
		{
			return new List<WordsInMetaTagsViewModel>
			{
				GetWordsInMetaTagsViewModel("metaTag1"),
				GetWordsInMetaTagsViewModel("metaTag2"),
				GetWordsInMetaTagsViewModel("metaTag3"),
				GetWordsInMetaTagsViewModel("metaTag4")
			};
		}

		private WordsInMetaTagsViewModel GetWordsInMetaTagsViewModel(string wordInPage)
		{
			Random random = new Random();
			int randomNo = random.Next(1, 100);
			return new WordsInMetaTagsViewModel
			{
				NoOfWordInMetaTags = randomNo,
				WordInMetaTag = wordInPage
			};
		}

		private IEnumerable<ExternalLinksViewModel> GetExternalLinksViewModelList()
		{
			return new List<ExternalLinksViewModel>
			{
				GetExternalLinksViewModel("link1"),
				GetExternalLinksViewModel("link2"),
				GetExternalLinksViewModel("link3"),
				GetExternalLinksViewModel("link4")
			};
		}

		private ExternalLinksViewModel GetExternalLinksViewModel(string wordInPage)
		{
			Random random = new Random();
			int randomNo = random.Next(1, 100);
			return new ExternalLinksViewModel
			{
				NoOfExternalLinks = randomNo,
				ExternalLink = wordInPage
			};
		}
	}
}
