using System.Threading.Tasks;
using DSB.Push.Shared.Models;

namespace DSB.Push.Repositories
{
    public interface IPushDataRepository
    {
        Task<PushUser?> GetUser(string personaId);
    }
}