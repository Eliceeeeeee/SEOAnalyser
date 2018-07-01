using SEOAnalyser.Business.Interfaces;
using System;
using System.Net;

namespace SEOAnalyser.Web.Business
{
	public class ValidateService : IValidateService
	{
		public ValidateService()
		{
		}
		
		public bool ValidUrl(string inputUrl)
		{
			return (inputUrl.StartsWith("http://") || inputUrl.StartsWith("https://"))
				&& Uri.IsWellFormedUriString(inputUrl, UriKind.RelativeOrAbsolute);
		}

		public bool ValidateWebsite(string inputUrl)
		{
			try
			{
				Uri requestUri = new Uri(inputUrl);
				string host = requestUri.Host;
				if(host == "localhost" || host == "127.0.0.1")
				{
					return false;
				}
				else
				{
					WebRequest request = WebRequest.Create(inputUrl);
					HttpWebResponse response = (HttpWebResponse)request.GetResponse();
					if (response == null || response.StatusCode != HttpStatusCode.OK)
					{
						response.Close();

					}
				}
			}
			catch(Exception e)
			{
				return false;
			}
			return true;
		}
	}
}
