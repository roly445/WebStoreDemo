using System.Collections.Generic;
using GiantBomb.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebStoreDemo.Infrastucture.Data;
using WebStoreDemo.Models;

namespace WebStoreDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
        }
        
        public IActionResult Index()
        {
            var promotions = new List<Game>();
            var promotionIds = this._memoryCache.Get<int[]>("promotions");
            if (promotionIds == null)
                return this.RedirectToAction("PreloadAndGoBackToStart", "Utils");
            foreach (var promotionId in promotionIds)
            {
                promotions.Add(this._memoryCache.Get<Game>($"promotion-{promotionId}"));
            }

            var news = new List<Game>();
            var newIds = this._memoryCache.Get<int[]>("new");
            foreach (var newId in newIds)
            {
                news.Add(this._memoryCache.Get<Game>($"new-{newId}"));
            }

            var headlines = new List<Game>();
            var headlineIds = this._memoryCache.Get<int[]>(CacheConstants.Headline);
            foreach (var headlineId in headlineIds)
            {
                headlines.Add(this._memoryCache.Get<Game>($"{CacheConstants.Headline}-{headlineId}"));
            }

            var viewModel = new HomeViewModel
            {
                Promotions = promotions,
                NewGames = news,
                HeadlineGames = headlines

            };
            return this.View(viewModel);
        }
        

        public IActionResult Products()
        {
            return this.View();
        }

        public IActionResult Single()
        {
            return this.View();
        }        
    }
}