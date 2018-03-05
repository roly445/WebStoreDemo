using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GiantBomb.Api.Model;
using Microsoft.AspNetCore.Mvc;
using WebStoreDemo.Infrastucture.Repositories;

namespace WebStoreDemo.Api
{
    public class GameController : Controller
    {
        private readonly IGameRepository _gameRepository;

        public GameController(IGameRepository gameRepository)
        {
            this._gameRepository = gameRepository;
        }

        public async Task<IEnumerable<Game>> Games(int page = 1)
        {
            return await this._gameRepository.GetGames(page);
        }

        public async Task<IEnumerable<Game>> Search(string term)
        {
            return await this._gameRepository.SearchGames(term);
        }

        public async Task<IEnumerable<Game>> PcGames(int page = 1)
        {
            return await this._gameRepository.GetPcGames(page);
        }
    }
}