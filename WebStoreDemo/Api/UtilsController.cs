using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiantBomb.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebStoreDemo.Infrastucture.Data;
using WebStoreDemo.Infrastucture.Repositories;

namespace WebStoreDemo.Api
{
    [Produces("application/json")]
    
    public class UtilsController : Controller
    {
        private readonly ICacheDbRepository _cacheDbRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IMemoryCache _memoryCache;
        public UtilsController(ICacheDbRepository cacheDbRepository, IGameRepository gameRepository, IMemoryCache memoryCache)
        {
            this._cacheDbRepository = cacheDbRepository;
            this._gameRepository = gameRepository;
            this._memoryCache = memoryCache;
        }


        [Route("api/utils/preload")]
        public async Task<IActionResult> Preload()
        {
            await this.LoadPromotions();
            await this.LoadNew();
            await this.LoadHeadliners();
            return this.Ok();
        }

        
        public async Task<bool> LoadPromotions()
        {
            var gameIds = new int[]
            {
                49101, // Danganronpa V3: Killing Harmony
                62419, // Outcast: Second Contact
                58675, // Gundam Versus
                62436, // theHunter: Call of the Wild
            };
            foreach (var gameId in gameIds)
            {
                if (this._memoryCache.TryGetValue<Game>($"promotion-{gameId}", out _))
                {
                    continue;
                }

                Game game;
                if (this._cacheDbRepository.IsItemInPartition(gameId, "promotions"))
                {
                    game = await this._gameRepository.GetGame(gameId);
                }
                else
                {
                    game = await this._gameRepository.GetGame(gameId);
                    this._cacheDbRepository.AddToCacheDb(game, "promotions");
                }

                this._memoryCache.Set<Game>($"promotion-{gameId}", game);
            }

            this._memoryCache.Set<int[]>("promotions", gameIds);
            return true;
            
        }

        public async Task<bool> LoadNew()
        {
            var gameIds = new int[]
            {
                55081, // Metal Gear Survive
                57414, // Dynasty Warriors 9
                39764, // Bayonetta 2
                44790, // Kingdom Come Deliverance
            };
            foreach (var gameId in gameIds)
            {
                if (this._memoryCache.TryGetValue<Game>($"new-{gameId}", out _))
                {
                    continue;
                }

                Game game;
                if (this._cacheDbRepository.IsItemInPartition(gameId, "new"))
                {
                    game = await this._gameRepository.GetGame(gameId);
                }
                else
                {
                    game = await this._gameRepository.GetGame(gameId);
                    this._cacheDbRepository.AddToCacheDb(game, "new");
                }

                this._memoryCache.Set<Game>($"new-{gameId}", game);
            }

            this._memoryCache.Set<int[]>("new", gameIds);
            return true;

        }



        public async Task<bool> LoadHeadliners()
        {
            var gameIds = new int[]
            {
                59557, // Far Cry 5
                54229, // God of War
                56725, // Red Dead Redemption 2
                
            };
            foreach (var gameId in gameIds)
            {
                if (this._memoryCache.TryGetValue<Game>($"{CacheConstants.Headline}-{gameId}", out _))
                {
                    continue;
                }

                Game game;
                if (this._cacheDbRepository.IsItemInPartition(gameId, CacheConstants.Headline))
                {
                    game = await this._gameRepository.GetGame(gameId);
                }
                else
                {
                    game = await this._gameRepository.GetGame(gameId);
                    this._cacheDbRepository.AddToCacheDb(game, CacheConstants.Headline);
                }

                this._memoryCache.Set<Game>($"{CacheConstants.Headline}-{gameId}", game);
            }

            this._memoryCache.Set<int[]>(CacheConstants.Headline, gameIds);
            return true;

        }
    }
}