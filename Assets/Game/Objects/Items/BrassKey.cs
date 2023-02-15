#nullable enable
using Assets.Game.Objects;

namespace Assembly_CSharp
{
    internal class BrassKey : IItem
    {
        // Fields
        private IObstacle? nemesisObstacle;

        // Constructors
        internal BrassKey() {}

        // Properties
        public IObstacle? NemisisObstacle => nemesisObstacle;

        // Methods
        public void SetNemesis(IObstacle obstacle)
        {
            nemesisObstacle = obstacle;
        }
    }
}
