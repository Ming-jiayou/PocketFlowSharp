using Microsoft.EntityFrameworkCore;
using PocketFlowSharpGallery.Data;
using PocketFlowSharpGallery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketFlowSharpGallery.Services
{
    public class SearchEngineConfigRepository : ISearchEngineConfigRepository
    {
        private readonly PFSDbContext _context;

        public SearchEngineConfigRepository(PFSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SearchEngineConfig>> GetAllAsync()
        {
            return await _context.SearchEngineConfigs.ToListAsync();
        }

        public async Task<SearchEngineConfig> GetByIdAsync(int id)
        {
            return await _context.SearchEngineConfigs.FindAsync(id);
        }

        public async Task AddAsync(SearchEngineConfig config)
        {
            _context.SearchEngineConfigs.Add(config);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SearchEngineConfig config)
        {
            _context.SearchEngineConfigs.Update(config);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var config = await _context.SearchEngineConfigs.FindAsync(id);
            if (config != null)
            {
                _context.SearchEngineConfigs.Remove(config);
                await _context.SaveChangesAsync();
            }
        }
    }
} 