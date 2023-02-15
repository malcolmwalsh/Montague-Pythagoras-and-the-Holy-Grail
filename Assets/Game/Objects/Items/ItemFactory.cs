using Assembly_CSharp;
using System;

namespace Assets.Game.Objects.Items
{
    internal class ItemFactory : ObjectFactory<IItem>
    {
        internal override IItem GetObject(string itemName)
        {
            return itemName switch
            {
                "BrassKey" => new BrassKey(),
                "WoollyHat" => new WoollyHat(),
                "MagicSword" => new MagicSword(),
                _ => throw new ApplicationException(string.Format($"Item `{itemName}` cannot be created")),
            };
        }

    }
}
