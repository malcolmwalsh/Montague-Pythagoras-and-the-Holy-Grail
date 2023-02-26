using Assets.Game.Objects.Items;

namespace Assets.Game.Objects.Backpacks
{
    public interface IBackpack
    {
        void Add(ItemController item);
        bool Contains(ItemController item);
    }
}
