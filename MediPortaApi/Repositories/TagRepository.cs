using MediPortaApi.Entities;
using MediPortaApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MediPortaApi.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TagsExists()
        {
            return await _context.Tags.AnyAsync();
        }

        public async Task AddTagsAsync(List<Tag> tags)
        {
            await _context.Tags.AddRangeAsync(tags);
            await _context.SaveChangesAsync();
        }

        public async Task RefreshTagsAsync(List<Tag> tags)
        {
            await _context.Tags.ExecuteDeleteAsync();
            await AddTagsAsync(tags);
        }

        public async Task<List<Tag>> GetTagsAsync(string sortBy, bool sortDesc)
        {
            var query = _context.Tags.AsQueryable();

            switch (sortBy)
            {
                case "name":
                    query = sortDesc ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name);
                    break;
                case "percentage":
                    query = sortDesc ? query.OrderByDescending(t => t.Percentage) : query.OrderBy(t => t.Percentage);
                    break;
                default:
                    return query.ToList();
            }

            return await query.ToListAsync();
        }
    }
}
