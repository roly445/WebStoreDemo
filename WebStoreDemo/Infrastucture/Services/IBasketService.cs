using System.Collections.Generic;
using GiantBomb.Api.Model;
using WebStoreDemo.Infrastucture.Data;

namespace WebStoreDemo.Infrastucture.Services
{
    public interface IBasketService
    {
        void AddItem(int id);
        void RemoveItem(int id);
        IReadOnlyList<Game> Items { get; }
    }
}