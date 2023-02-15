#nullable enable
using System.Collections.Generic;
using static Assets.Game.Navigation.Enums;

namespace Assets.Game.Objects.Rooms
{
    internal class StartRoom : IRoom
    {
        // Fields
        ISet<IItem> items;
        IDictionary<CompassDirection, IDoor?> doors;

        // Properties
        // Begin IRoom
        public bool IsStartRoom => true;
        public bool IsFinalRoom => false;
        public ISet<IItem> Items => items;
        public IDictionary<CompassDirection, IDoor?> Doors => doors;        
        // End IRoom
    }
}
