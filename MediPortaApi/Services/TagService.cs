using MediPortaApi.Entities;
using MediPortaApi.Repositories.Interfaces;
using MediPortaApi.Services.Interfaces;

namespace MediPortaApi.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IStackOverflowAPIService _stackOverflowAPIService;

        public TagService(ITagRepository tagRepository, IStackOverflowAPIService stackOverflowAPIService)
        {
            _tagRepository = tagRepository;
            _stackOverflowAPIService = stackOverflowAPIService;
        }

        public async Task<List<Tag>> GetTagsAsync(string sortBy, bool sortDesc)
        {
            var tagsExists = await _tagRepository.TagsExists();

            if(!tagsExists)
            {
                var tags = await _stackOverflowAPIService.GetTagsAsync();
                await _tagRepository.AddTagsAsync(tags);
            }

            return await _tagRepository.GetTagsAsync(sortBy, sortDesc);
        }

        public async Task RefreshTagsAsync()
        {
            var tags = await _stackOverflowAPIService.GetTagsAsync();
            await _tagRepository.RefreshTagsAsync(tags);
        }
    }
}
