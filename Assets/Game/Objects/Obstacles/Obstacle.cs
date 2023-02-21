#nullable enable
using Assets.Game.Objects.Items;

namespace Assets.Game.Objects.Obstacles
{
    internal class Obstacle : IObstacle
    {
        // Fields
        private string name;
        private string description;

        private IItem? nemesisItem;

        // Constructors
        public Obstacle(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public Obstacle(string name, string description, IItem? nemesisItem) : this(name, description)
        {
            this.nemesisItem = nemesisItem;
        }

        // Properties
        public IItem? NemesisItem => nemesisItem;
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }

        // Methods
        public void SetNemesis(IItem item)
        {
            this.nemesisItem = item;
        }
    }
}
