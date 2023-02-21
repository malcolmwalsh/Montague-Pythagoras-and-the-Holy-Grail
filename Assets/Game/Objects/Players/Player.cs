#nullable enable
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Rooms;
using System.Collections.Generic;

namespace Assets.Game.Objects.Players
{
    internal class Player : IPlayer
    {
        // Fields    
        private readonly string name;
        private readonly string description;

        private IRoom? currentRoom;

        private ISet<IItem> items = new HashSet<IItem>();

        // Constructors
        public Player(string name, string description, IRoom currentRoom)
        {
            this.name = name;
            this.description = description;
            this.currentRoom = currentRoom;
        }

        // Properties
        public string Name => name;
        public string Description => description;
        public IRoom? CurrentRoom { get => currentRoom; set => currentRoom = value; }

        // Methods

        // Begin IPlayer
        public bool HasItem(IItem item)
        {
            return items.Contains(item);
        }

        public void AddItem(IItem item)
        {
            this.items.Add(item);
        }
        // End IPlayer

        public override string ToString()
        {
            return Name;
        }
    }
}