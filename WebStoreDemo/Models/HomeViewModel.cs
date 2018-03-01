using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStoreDemo.Infrastucture;

namespace WebStoreDemo.Models
{
    public class HomeViewModel
    {
        public HomeViewModel(IReadOnlyList<Item> items)
        {
            this.Items = items;
        }
        public IReadOnlyList<Item> Items { get; }

    }
}
