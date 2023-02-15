# nullable enable
namespace Assets.Game.Objects.Items
{
    internal class MagicSword : IItem
    {
        // Fields
        private IObstacle? nemesisObstacle;

        // Properties
        public IObstacle? NemisisObstacle => nemesisObstacle;

        // Constructors

        // Methods
        public void SetNemesis(IObstacle obstacle)
        {
            this.nemesisObstacle = obstacle;
        }
    }
}
