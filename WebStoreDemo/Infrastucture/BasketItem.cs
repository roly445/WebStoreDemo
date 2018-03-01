using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebStoreDemo.Infrastucture
{
    [JsonConverter(typeof(BasketItemJsonConvertor))]
    public class BasketItem
    {
        public Guid Id => this.Item.Id;
        public Item Item { get; }
        public int Quantity { get; private set; }

        public BasketItem(Item item)
        {
            this.Item = item;
            this.Quantity = 1;
        }

        public void SetQuantity(int amount)
        {
            this.Quantity = amount;
        }
    }

    public class BasketItemJsonConvertor : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is BasketItem item))
            {
                return;
            }

            var t = JToken.FromObject(item.Item);

            var jObject = new JObject
            {
                {"item", t },
                {"quantity", item.Quantity}
            };
            jObject.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            
            var target = new BasketItem(
                jObject["item"].ToObject<Item>());
            target.SetQuantity(jObject["quantity"].Value<int>());
            

            return target;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BasketItem);
        }
    }
}