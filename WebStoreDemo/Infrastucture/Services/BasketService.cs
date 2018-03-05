using System;
using System.Collections.Generic;
using System.Linq;
using GiantBomb.Api.Model;
using Microsoft.AspNetCore.Http;
using WebStoreDemo.Infrastucture.Data;
using WebStoreDemo.Infrastucture.Repositories;

namespace WebStoreDemo.Infrastucture.Services
{
    public class BasketService : IBasketService
    {
        private readonly List<Game> _basketItems;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheDbRepository _cacheDbRepository;
        private ISession _session => this._httpContextAccessor.HttpContext.Session;

        public IReadOnlyList<Game> Items => this._basketItems.AsReadOnly();

        public BasketService(IHttpContextAccessor httpContextAccessor, ICacheDbRepository cacheDbRepository)
        {
            this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this._cacheDbRepository = cacheDbRepository ?? throw new ArgumentNullException(nameof(cacheDbRepository));
            
            var basketItemsFromSession = this._session.GetObjectFromJson<List<Game>>("basket");
            this._basketItems = basketItemsFromSession ?? new List<Game>();
        }

        public void AddItem(int id)
        {
            var game = this._cacheDbRepository.GetItemInPartition<Game>("GameLookup", id);
            this._basketItems.Add(game);
            this.Save();
        }

        public void RemoveItem(int id)
        {
            var itemToRemove = this._basketItems.SingleOrDefault(x=>x.Id == id);
            this._basketItems.Remove(itemToRemove);
            this.Save();
        }

        private void Save()
        {
            this._session.SetObjectAsJson("basket", this._basketItems);
        }
    }
}