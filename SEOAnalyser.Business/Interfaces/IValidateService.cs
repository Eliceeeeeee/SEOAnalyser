namespace SEOAnalyser.Business.Interfaces
{
	public interface IValidateService
	{
		bool ValidUrl(string inputUrl);
		bool ValidateWebsite(string inputUrl);
	}
}
