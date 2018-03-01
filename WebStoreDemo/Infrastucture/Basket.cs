using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;

namespace WebStoreDemo.Infrastucture
{
    public class Basket : IBasket
    {
        private readonly List<BasketItem> _basketItems;

        private readonly IItemRepository _itemRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => this._httpContextAccessor.HttpContext.Session;

        public IReadOnlyList<BasketItem> BasketItems => this._basketItems;

        public Basket(IItemRepository itemRepository, IHttpContextAccessor httpContextAccessor)
        {
            this._itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            var basketItemsFromSession = this._session.GetObjectFromJson<List<BasketItem>>("basket");
            this._basketItems = basketItemsFromSession ?? new List<BasketItem>();
        }

        public BasketItem AddItemToBasket(Guid itemId)
        {
            
            var basketItem = this._basketItems.SingleOrDefault(bItem => bItem.Id == itemId);
            if (basketItem == null)
            {
                var item = this._itemRepository.GetById(itemId);
                this._basketItems.Add(new BasketItem(item));
            }
            else
            {
                basketItem.SetQuantity(basketItem.Quantity + 1);
            }

            return basketItem;
        }

        public void RemoveItemFromBasket(Guid itemId)
        {
            var basketItem = this._basketItems.SingleOrDefault(bItem => bItem.Id == itemId);
            this._basketItems.Remove(basketItem);
        }

        public void Save()
        {
            this._session.SetObjectAsJson("basket", this._basketItems);
        }

        public void EmptyBasket()
        {
            this._basketItems.Clear();
        }

    }
}
