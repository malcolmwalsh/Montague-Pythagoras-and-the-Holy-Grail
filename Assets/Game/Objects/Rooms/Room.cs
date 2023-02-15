﻿#nullable enable
using System;
using System.Collections.Generic;
using static Assets.Game.Navigation.Enums;

namespace Assets.Game.Objects.Rooms
{
    internal class Room : IRoom
    {
        // Constructors
        public Room(string name, string description, bool isStartRoom, bool isFinalRoom)
        {
            this.isFinalRoom = isFinalRoom;
            this.isStartRoom = isStartRoom;
            this.name = name;
            this.description = description;
        }        
        
        public Room(string name, string description) : this(name, description, false, false) { }

        // Fields
        private readonly string name;
        private readonly string description;

        private readonly bool isStartRoom = false;
        private readonly bool isFinalRoom = false;
        private readonly ISet<IItem> items = new HashSet<IItem>();
        private readonly IDictionary<CompassDirection, IDoor> doors = new Dictionary<CompassDirection, IDoor>();

        // Properties
        public string Name => name;
        public string Description => description;

        // Begin IRoom
        public bool IsStartRoom => isStartRoom;
        public bool IsFinalRoom => isFinalRoom;
        public ISet<IItem> Items => items;
        public IDictionary<CompassDirection, IDoor> Doors => doors;
        // End IRoom

        // Methods
        public void AddDoorInDirection(CompassDirection direction, IDoor door)
        {
            // Check
            if (!door.ConnectsRoom(this))
            {
                throw new ApplicationException($"Can't add door `{door}` to room `{this}` as it does not connect it to anything");
            }

            // Check 2
            if (doors.ContainsKey(direction))
            {
                throw new ApplicationException($"Can't add door `{door}` to room `{this}` as it already has a door in direction `{direction}`");
            }

            // Add
            doors.Add(direction, door);
        }
    }
}
