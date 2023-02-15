#nullable enable
namespace Assets.Game.Objects.Items
{
    internal class Item : IItem
    {
        // Fields
        private readonly string name;
        private readonly string description;

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
        public string Name => name;
        public string Description => description;

        public IObstacle? NemisisObstacle => nemesisObstacle;

        // Methods
        public void SetNemesis(IObstacle obstacle)
        {
            nemesisObstacle = obstacle;
        }
    }
}
