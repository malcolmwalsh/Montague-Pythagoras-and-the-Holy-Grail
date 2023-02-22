using Assets.Game.Control;
using Assets.Game.Objects.Items;

namespace Assets.Game.Objects.Players
{
    public interface IPlayer : IObject
    {
        // Properties
        ManagerBehaviour Manager { get; set; }

        // Methods
        bool HasItem(IItem item);
        void AddItem(IItem item);
    }
}
