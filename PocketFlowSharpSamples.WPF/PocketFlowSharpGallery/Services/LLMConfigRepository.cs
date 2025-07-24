using Microsoft.EntityFrameworkCore;
using PocketFlowSharpGallery.Data;
using PocketFlowSharpGallery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketFlowSharpGallery.Services
{
    public class LLMConfigRepository : ILLMConfigRepository
    {
        private readonly PFSDbContext _context;

        public LLMConfigRepository(PFSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LLMConfig>> GetAllAsync()
        {
            return await _context.LLMConfigs.ToListAsync();
        }

        public async Task<LLMConfig> GetByIdAsync(int id)
        {
            return await _context.LLMConfigs.FindAsync(id);
        }

        public async Task AddAsync(LLMConfig config)
        {
            _context.LLMConfigs.Add(config);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LLMConfig config)
        {
            _context.LLMConfigs.Update(config);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var config = await _context.LLMConfigs.FindAsync(id);
            if (config != null)
            {
                _context.LLMConfigs.Remove(config);
                await _context.SaveChangesAsync();
            }
        }
    }
}