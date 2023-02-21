#nullable enable
using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Assets.Game.Navigation.Enums;

namespace Assets.Game.Objects.Rooms
{
    public class Room : IRoom
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

        private ISet<INPC> npcs= new HashSet<INPC>();

        // Properties
        public string Name => name;
        public string Description => description;

        // Begin IRoom
        public bool IsStartRoom => isStartRoom;
        public bool IsFinalRoom => isFinalRoom;        
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

        public bool HasItem()
        {
            return items.Any();
        }

        public void AddItem(IItem item)
        {
            items.Add(item);
        }

        public IItem? GetItem()
        {
            IItem item = items.FirstOrDefault();

            // Items have been taken
            items.Clear();

            return item;
        }

        public override string ToString()
        {
            return Name;
        }

        public int NumDoors()
        {
            return doors.Count;
        }

        public string DoorLocationText()
        {
            StringBuilder sb = new();

            foreach (KeyValuePair<CompassDirection, IDoor> entry in doors)
            {
                string text = $"There is a door to the {entry.Key}. ";

                sb.Append(text);
            }

            return sb.ToString();
        }

        public void AddNPC(INPC npc)
        {
            npcs.Add(npc);
        }

        public bool HasNPC()
        {
            return npcs.Any();  
        }

        public INPC? GetNPC()
        {
            return npcs.FirstOrDefault();
        }
    }
}
