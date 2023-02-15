#nullable enable
using Unity.VisualScripting;

namespace Assets.Game.Objects.Obstacles
{
    internal class Obstacle : IObstacle
    {
        // Fields
        private readonly string name;
        private readonly string description;

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
        public string Name => name;
        public string Description => description;

        // Methods
        public void SetNemesis(IItem item)
        {
            this.nemesisItem = item;
        }
    }
}
