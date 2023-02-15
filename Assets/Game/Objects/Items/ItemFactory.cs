using System;

namespace Assets.Game.Objects.Items
{
    internal class ItemFactory : ObjectFactory<IItem>
    {
        internal override IItem GetObject(string itemName)
        {
            return itemName switch
            {
                "BrassKey" => new Item(itemName, "A large, heavy key made of a shiny golden brown metal"),
                "WoollyHat" => new Item(itemName, "A beanie made of alpaca wool"),
                "MagicSword" => new Item(itemName, "A heavy sword that whispers riddles to you as you carry it around"),
                _ => throw new ApplicationException(string.Format($"Item `{itemName}` cannot be created")),
            };
        }

    }
}
