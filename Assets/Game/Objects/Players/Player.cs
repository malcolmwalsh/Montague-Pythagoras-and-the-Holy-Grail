#nullable enable
using Assets.Game.Objects.Backpacks;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Rooms;

namespace Assets.Game.Objects.Players
{
    internal class Player : IPlayer
    {
        // Fields    
        private readonly string name;
        private readonly string description;

        private IRoom? currentRoom;

        private IBackpack backpack = new Backpack();        

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
            return backpack.Contains(item);
        }

        public void AddItem(IItem item)
        {
            this.backpack.Add(item);
        }
        // End IPlayer

        public override string ToString()
        {
            return Name;
        }
    }
}