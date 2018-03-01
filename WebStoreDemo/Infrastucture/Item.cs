using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebStoreDemo.Infrastucture
{
    [JsonConverter(typeof(ItemJsonConverter))]
    public class Item
    {
        public Item(Guid id, string name, string description, decimal price, string image)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.Image = image;
        }
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }
        public string Image { get; }
        
    }

    public class ItemJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Item);
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var target = new Item(
                Guid.Parse(jObject["id"].Value<string>()),
                jObject["name"].Value<string>(),
                jObject["description"].Value<string>(),
                jObject["price"].Value<decimal>(),
                jObject["image"].Value<string>()
            );
        
            return target;
        }
        

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is Item item))
            {
                return;
            }

            var jObject = new JObject
            {
                {"id", item.Id},
                {"price", item.Price},
                {"description", item.Description},
                {"image", item.Image},
                {"name", item.Name}
            };
            jObject.WriteTo(writer);


        }
    }
}
