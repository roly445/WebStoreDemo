using System.Collections.Generic;
using GiantBomb.Api.Model;
using WebStoreDemo.Infrastucture;

namespace WebStoreDemo.Models
{
    public class BasketViewModel
    {
        //public BasketViewModel(IReadOnlyList<BasketItem> basketItems)
        //{
        //    this.BasketItems = basketItems;
        //}

        public IReadOnlyList<Game> Games { get; set; }
    }
}