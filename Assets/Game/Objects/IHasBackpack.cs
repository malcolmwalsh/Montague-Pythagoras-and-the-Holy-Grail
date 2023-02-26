using Assets.Game.Objects.Items;

namespace Assets.Game.Objects
{
    public interface IHasBackpack
    {
        bool HasItem(ItemController item);
        void AddItem(ItemController item);
    }
}