using PocketFlowSharpGallery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketFlowSharpGallery.Services
{
    public interface ISearchEngineConfigRepository
    {
        Task<IEnumerable<SearchEngineConfig>> GetAllAsync();
        Task<SearchEngineConfig> GetByIdAsync(int id);
        Task AddAsync(SearchEngineConfig config);
        Task UpdateAsync(SearchEngineConfig config);
        Task DeleteAsync(int id);
    }
} 