using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStoreDemo.Infrastucture;
using WebStoreDemo.Models;

namespace WebStoreDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IBasket _basket;

        public HomeController(IItemRepository itemRepository, IBasket basket)
        {
            this._itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            this._basket = basket ?? throw new ArgumentNullException(nameof(basket));
        }
        public IActionResult Index()
        {
            return this.View(new HomeViewModel(this._itemRepository.GetAll()));
        }

        public IActionResult AddToBasket(Guid id)
        {
            this._basket.AddItemToBasket(id);
            this._basket.Save();
            return this.RedirectToAction("Basket");
        }

        public IActionResult RemoveFromBasket(Guid id)
        {
            this._basket.RemoveItemFromBasket(id);
            this._basket.Save();
            return this.RedirectToAction("Basket");
        }

        public IActionResult Basket()
        {
            var model = new BasketViewModel(this._basket.BasketItems);
            return View(model);
        }

        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
