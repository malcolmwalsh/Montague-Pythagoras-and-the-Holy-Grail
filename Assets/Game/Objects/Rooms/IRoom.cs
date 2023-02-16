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
        ISet<IItem> Items { get; }
        IDictionary<CompassDirection, IDoor> Doors { get; }

        // Methods
        bool HasItem()
        {
            return Items.Any();
        }

        void AddItem(IItem item)
        {
            Items.Add(item);
        }

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
