using System;
using Microsoft.AspNetCore.Mvc;
using WebStoreDemo.Infrastucture.Services;
using WebStoreDemo.Models;

namespace WebStoreDemo.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)        
        {
            this._basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }


        public IActionResult Index()
        {
            var model = new BasketViewModel
            {
                Games = this._basketService.Items
            };
            
            return this.View(model);
        }

        public IActionResult AddToBasket(int id)
        {
            this._basketService.AddItem(id);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromBasket(int id)
        {
            this._basketService.RemoveItem(id);
            return RedirectToAction("Index");
        }
    }
}