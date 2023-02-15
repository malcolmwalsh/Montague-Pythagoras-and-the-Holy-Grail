using Assets.Game.Objects;
using System.Collections.Generic;
using static Assets.Game.Navigation.Enums;

namespace Assembly_CSharp
{
    internal class Room : IRoom
    {
        // Fields
        private bool isStartRoom;
        private bool isFinalRoom;

        private IDictionary<CompassDirection, Door?> doors;
        private ISet<IItem> items;

        // Properties
        public bool IsStartRoom => isStartRoom;
        public bool IsFinalRoom => isFinalRoom;

        // Constructors
    }
}
