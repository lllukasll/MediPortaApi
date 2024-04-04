using MediPortaApi.Entities;

namespace MediPortaApi.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<bool> TagsExists();
        Task AddTagsAsync(List<Tag> tags);
        Task RefreshTagsAsync(List<Tag> tags);
        Task<List<Tag>> GetTagsAsync(string sortBy, bool sortDesc);
    }
}
