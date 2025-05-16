using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketFlowSharp
{
    public interface IAsyncNode
    {
        Task<object> RunAsync(Dictionary<string, object> shared);
    }
} 