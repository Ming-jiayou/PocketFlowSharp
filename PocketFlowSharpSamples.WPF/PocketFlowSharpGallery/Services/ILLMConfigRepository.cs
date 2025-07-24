using PocketFlowSharpGallery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketFlowSharpGallery.Services
{
    public interface ILLMConfigRepository
    {
        Task<IEnumerable<LLMConfig>> GetAllAsync();
        Task<LLMConfig> GetByIdAsync(int id);
        Task AddAsync(LLMConfig config);
        Task UpdateAsync(LLMConfig config);
        Task DeleteAsync(int id);
    }
}