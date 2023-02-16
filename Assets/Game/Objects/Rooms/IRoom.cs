#nullable enable
using System.Collections.Generic;
using System.Linq;
using static Assets.Game.Navigation.Enums;

namespace Assets.Game.Objects
{
    internal interface IRoom : IObject
    {
        // Properties
        bool IsStartRoom { get; }
        bool IsFinalRoom { get; }        
        IDictionary<CompassDirection, IDoor> Doors { get; }

        // Methods
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

        void AddItem(IItem item);
        bool HasItem();
        IItem? GetItem();
    }
}
