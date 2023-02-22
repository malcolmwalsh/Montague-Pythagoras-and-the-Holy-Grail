using Assets.Game.Control;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Rooms;

namespace Assets.Game.Objects.Players
{
    public interface IPlayer : IObject
    {
        // Properties
        IRoom CurrentRoom { get; set; }
        ManagerBehaviour Manager { get; set; }

        // Methods
        bool HasItem(IItem item);
        void AddItem(IItem item);
    }
}
