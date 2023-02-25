using Assets.Game.Navigation;
using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.NPCs;

#nullable enable

namespace Assets.Game.Objects.Rooms
{
    public interface IRoom : IObject
    {
        // Properties
        INPC? NPC { get; }
        bool IsStartRoom { get; }
        bool IsFinalRoom { get; }

        // Methods
        bool HasItem();
        IItem? GetItemBehaviour();
        int NumDoors();
        string DoorLocationText();
        bool HasDoorInDirection(Enums.CompassDirection direction);
        IDoor? GetDoorInDirection(Enums.CompassDirection direction);
        bool HasNPC();
        void RemoveNPC(INPC npc);
    }
}
