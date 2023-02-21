#nullable enable
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Obstacles;

namespace Assets.Game.Objects.Items
{
    internal class Item : IItem
    {
        // Fields
        private string name;
        private string description;

        private IObstacle? nemesisObstacle;

        // Constructors
        public Item(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public Item(string name, string description, IObstacle? nemesisObstacle) : this(name, description)
        {
            this.nemesisObstacle = nemesisObstacle;
        }

        // Properties
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }

        public IObstacle? NemisisObstacle => nemesisObstacle;

        // Methods
        public void SetNemesis(IObstacle obstacle)
        {
            nemesisObstacle = obstacle;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
