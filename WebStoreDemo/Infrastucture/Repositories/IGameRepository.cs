using System.Collections.Generic;
using System.Threading.Tasks;
using GiantBomb.Api.Model;

namespace WebStoreDemo.Infrastucture.Repositories
{
    public interface IGameRepository
    {
        Task<Game> GetGame(int id);
        Task<IEnumerable<Game>> GetGames(int page);
        Task<IEnumerable<Game>> SearchGames(string term);
        Task<IEnumerable<Game>> GetPcGames(int page);
    }
}