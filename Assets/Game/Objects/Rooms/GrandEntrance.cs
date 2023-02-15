#nullable enable
using static Assets.Game.Navigation.Enums;
using System.Collections.Generic;

namespace Assets.Game.Objects.Rooms
{
    internal class GrandEntrance : IRoom
    {
        // Fields
        ISet<IItem> items;
        IDictionary<CompassDirection, IDoor?> doors;

        // Properties
        // Begin IRoom
        public bool IsStartRoom => false;
        public bool IsFinalRoom => false;
        public ISet<IItem> Items => items;
        public IDictionary<CompassDirection, IDoor?> Doors => doors;
        // End IRoom
    }
}
