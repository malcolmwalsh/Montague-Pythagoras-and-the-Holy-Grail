#nullable enable
using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.NPCs;
using System.Collections.Generic;
using static Assets.Game.Navigation.Enums;

namespace Assets.Game.Objects.Rooms
{
    public interface IRoom : IObject
    {
        // Properties
        bool IsStartRoom { get; }
        bool IsFinalRoom { get; }
        IDictionary<CompassDirection, IDoor> Doors { get; }

        // Methods
        void AddItem(IItem item);
        bool HasItem();
        IItem? GetItem();
        int NumDoors();
        string DoorLocationText();
        void AddNPC(INPC npc);
        bool HasNPC();
        INPC? GetNPC();

        bool HasDoorInDirection(CompassDirection direction)
        {
            IDoor? door = GetDoorInDirection(direction);

            return door is not null;
        }

        IDoor? GetDoorInDirection(CompassDirection direction)
        {
            Doors.TryGetValue(direction, out IDoor? door);

            return door;
        }

        void SetDoorInDirection(CompassDirection direction, IDoor door)
        {
            Doors.Add(direction, door);
        }


    }
}
