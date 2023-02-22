using System;

namespace Assets.Game.Objects.Items
{
    internal class ItemFactory : ObjectFactory<IItem>
    {
        internal override IItem GetObject(string itemName)
        {
            return itemName switch
            {
                "BrassKey" => new ItemLogic(itemName, "It appears to be a large, heavy key made of a shiny golden brown metal"),
                "WoollyHat" => new ItemLogic(itemName, "It's a beanie made of alpaca wool. It's a sensible choice given the season."),
                "MagicSword" => new ItemLogic(itemName, "Quite the find! It's a large, heavy sword that whispers riddles to you as you carry it around. What could go wrong?"),
                "PinkCowboyHat" => new ItemLogic(itemName, "It's a stunning pink number that's too big for your tiny head but you wear it anyway"),
                _ => throw new ApplicationException(string.Format($"Item `{itemName}` cannot be created")),
            };
        }

    }
}
