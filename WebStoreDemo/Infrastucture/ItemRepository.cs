using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace WebStoreDemo.Infrastucture
{
    public interface IItemRepository
    {
        Item GetById(Guid id);
        IReadOnlyList<Item> GetAll();
    }

    public class ItemRepository : IItemRepository
    {
        private static IReadOnlyList<Item> _items;

        public ItemRepository()
        {
            if (_items == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceStream = assembly.GetManifestResourceStream("WebStoreDemo.Infrastucture.data.json");

                using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
                {
                    var data = reader.ReadToEnd();
                    var list = JsonConvert.DeserializeObject<List<Item>>(data);
                    _items = list.AsReadOnly();
                }
            }
        }

        public Item GetById(Guid id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public IReadOnlyList<Item> GetAll()
        {
            return _items;
        }
    }
}