using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiantBomb.Api;
using GiantBomb.Api.Model;
using WebStoreDemo.Api;
using WebStoreDemo.Infrastucture.Wrappers;

namespace WebStoreDemo.Infrastucture.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IGiantBombRestClient _giantBombRestClient;
        private readonly ICacheDbRepository _cacheDbRepository;

        public GameRepository(IGiantBombRestClientWrapper giantBombRestClient, ICacheDbRepository cacheDbRepository)
        {
            this._giantBombRestClient = giantBombRestClient?.GetClient() ?? throw new ArgumentNullException(nameof(giantBombRestClient));
            this._cacheDbRepository = cacheDbRepository ?? throw new ArgumentNullException(nameof(cacheDbRepository));
        }

        public async Task<IEnumerable<Game>> GetGames(int page)
        {
            var games = await this._giantBombRestClient.GetGamesAsync(page, 20);
            foreach(var game in games)
            {
                if (!this._cacheDbRepository.IsItemInPartition(game.Id, "GameLookup"))
                {
                    this._cacheDbRepository.AddToCacheDb(game, "GameLookup");
                }
            }
            return games;
        }

        public async Task<IEnumerable<Game>> SearchGames(string term)
        {
            var games = await this._giantBombRestClient.SearchForAllGamesAsync(term);
            foreach(var game in games)
            {
                if (!this._cacheDbRepository.IsItemInPartition(game.Id, "GameLookup"))
                {
                    this._cacheDbRepository.AddToCacheDb(game, "GameLookup");
                }
            }
            return games;
        }

        public async Task<Game> GetGame(int id)
        {
            var game = await this._giantBombRestClient.GetGameAsync(id);
            if (!this._cacheDbRepository.IsItemInPartition(game.Id, "GameLookup"))
                {
                    this._cacheDbRepository.AddToCacheDb(game, "GameLookup");
                }
            return game;
        }

        public async Task<IEnumerable<Game>> GetPcGames(int page)
        {
            return await this.GetGamesForPlatform(page, 94);
        }


        public async Task<IEnumerable<Game>> GetGamesForPlatform(int page, int platformId)
        {
            var games = await this._giantBombRestClient.GetListResourceAsync<Game>("games", 
                page: page,
                pageSize: 20,
                filterOptions: new Dictionary<string, object>
                {
                    { "platforms", platformId },
                    { "original_release_date", $"1700-01-01|{DateTime.Now.AddDays(-1):yyyy-MM-dd}"}                    
                },
                sortOptions: new Dictionary<string, SortDirection>
                {
                    {"original_release_date",SortDirection.Descending } 
                }
            );
            
            foreach(var game in games)
            {
                if (!this._cacheDbRepository.IsItemInPartition(game.Id, "GameLookup"))
                {
                    this._cacheDbRepository.AddToCacheDb(game, "GameLookup");
                }
            }
            return games;
        }
    }
}