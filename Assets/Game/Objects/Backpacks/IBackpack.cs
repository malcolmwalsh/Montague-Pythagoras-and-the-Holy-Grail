using System.Collections.Generic;
using Assets.Game.Objects.Items;

namespace Assets.Game.Objects.Backpacks
{
    public interface IBackpack
    {
        bool IsEmpty();
        void AddItem(ItemController item);
        IList<ItemController> GetItems();
        bool HasItem(ItemController item);
    }
}
