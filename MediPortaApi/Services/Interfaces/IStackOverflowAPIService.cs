using MediPortaApi.Entities;

namespace MediPortaApi.Services.Interfaces
{
    public interface IStackOverflowAPIService
    {
        Task<List<Tag>> GetTagsAsync();
    }
}
