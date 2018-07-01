using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEOAnalyser.Business.Model;
using System.Collections.Generic;
using System.Linq;

namespace SEOAnalyser.Web.Business.Tests
{
	[TestClass]
	public class ValidateServiceTest
	{
		private ValidateService _target;

		[TestInitialize]
		public void TestInitialize()
		{
			_target = new ValidateService();
		}

		[TestMethod]
		public void ConstructorTest()
		{
			// Assert
			Assert.IsNotNull(_target);
		}

		[TestMethod]
		public void ValidUrl_IsValidUrl_ReturnTrue()
		{
			// Act
			var result = _target.ValidUrl("https://www.google.com");

			// Assert
			Assert.AreEqual(true, result);
		}

		[TestMethod]
		public void ValidUrl_IsNotUrl_ReturnTrue()
		{
			// Act
			var result = _target.ValidUrl("Hello World");

			// Assert
			Assert.AreEqual(false, result);
		}

		[TestMethod]
		public void ValidateWebsite_IsValidUrl_ReturnTrue()
		{
			// Act
			var result = _target.ValidateWebsite("https://www.google.com");

			// Assert
			Assert.AreEqual(true, result);
		}

		[TestMethod]
		public void ValidateWebsite_IsNotUrl_ReturnTrue()
		{
			// Act
			var result = _target.ValidateWebsite("Hello World");

			// Assert
			Assert.AreEqual(false, result);
		}
	}
}
