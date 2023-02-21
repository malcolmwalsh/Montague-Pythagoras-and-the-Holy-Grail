using Assets.Game.Objects.Items;

namespace Assets.Game.Objects.Backpacks
{
    public interface IBackpack
    {
        void Add(IItem item);
        bool Contains(IItem item);
    }
}
