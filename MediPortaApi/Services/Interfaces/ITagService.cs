using MediPortaApi.Entities;

namespace MediPortaApi.Services.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetTagsAsync(string sortBy, bool sortDesc);
        Task RefreshTagsAsync();
    }
}
