using AutoMapper;
using SEOAnalyser.Business.Model;
using SEOAnalyser.Web.Models;

namespace SEOAnalyser.Mapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<WordsInPage, WordsInPageViewModel>()
				.ForMember(dest => dest.NoOfWordInPage, source => source.MapFrom(model => model.NoOfWordInPage))
				.ForMember(dest => dest.WordInPage, source => source.MapFrom(model => model.WordInPage));

			CreateMap<WordsInMetaTag, WordsInMetaTagsViewModel>()
				.ForMember(dest => dest.NoOfWordInMetaTags, source => source.MapFrom(model => model.NoOfWordInMetaTags))
				.ForMember(dest => dest.WordInMetaTag, source => source.MapFrom(model => model.WordInMetaTag));

			CreateMap<ExternalLinks, ExternalLinksViewModel>()
				.ForMember(dest => dest.NoOfExternalLinks, source => source.MapFrom(model => model.NoOfExternalLinks))
				.ForMember(dest => dest.ExternalLink, source => source.MapFrom(model => model.ExternalLink));
			
		}
	}
}