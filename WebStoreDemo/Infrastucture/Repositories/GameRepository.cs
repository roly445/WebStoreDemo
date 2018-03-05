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

        public GameRepository(IGiantBombRestClientWrapper giantBombRestClient)
        {
            this._giantBombRestClient = giantBombRestClient?.GetClient() ?? throw new ArgumentNullException(nameof(giantBombRestClient));
        }

        public async Task<IEnumerable<Game>> GetGames(int page)
        {
            var games = await this._giantBombRestClient.GetGamesAsync(page, 20);
            return games;
        }

        public async Task<IEnumerable<Game>> SearchGames(string term)
        {
            var games = await this._giantBombRestClient.SearchForAllGamesAsync(term);
            return games;
        }

        public async Task<Game> GetGame(int id)
        {
            var game = await this._giantBombRestClient.GetGameAsync(id);
            return game;
        }

        public async Task<IEnumerable<Game>> GetPcGames(int page)
        {
            return await this.GetGamesForPlatform(page, 94);
        }


        public async Task<IEnumerable<Game>> GetGamesForPlatform(int page, int platformId)
        {
            //var g = this._giantBombRestClient.GetListResource("games",
            //    page: page,
            //    pageSize: 20,
            //    filterOptions: new Dictionary<string, object>
            //    {
            //        // {"platforms", platformId },
            //        { "sort", "original_release_date:desc" }
            //    });
            var games = await this._giantBombRestClient.GetListResourceAsync<Game>("games", 
                page: page,
                pageSize: 20,
                filterOptions: new Dictionary<string, object>
                {
                   {"platforms", platformId },
                    { "original_release_date",$"1700-01-01|{DateTime.Now.AddDays(-1):yyyy-MM-dd}"}
                    
                },
                sortOptions: new Dictionary<string, SortDirection>
                    { {"original_release_date",SortDirection.Descending } }
                    );
            return games;
        }
    }
}