using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiantBomb.Api.Model;
using WebStoreDemo.Infrastucture;

namespace WebStoreDemo.Models
{
    public class HomeViewModel
    {
        //public HomeViewModel(IReadOnlyList<Item> items)
        //{
        //    this.Items = items;
        //}
        public List<Game> Promotions { get; set; }
        public List<Game> NewGames { get; set; }

        public List<Game> HeadlineGames { get; set; }
    }
}
