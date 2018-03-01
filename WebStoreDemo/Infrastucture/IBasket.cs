using System;
using System.Collections.Generic;

namespace WebStoreDemo.Infrastucture
{
    public interface IBasket
    {
        IReadOnlyList<BasketItem> BasketItems { get; }

        BasketItem AddItemToBasket(Guid itemId);
        void EmptyBasket();
        void RemoveItemFromBasket(Guid itemId);
        void Save();
    }
}