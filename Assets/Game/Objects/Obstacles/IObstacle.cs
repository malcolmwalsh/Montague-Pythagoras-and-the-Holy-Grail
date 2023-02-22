using Assets.Game.Objects.Items;

namespace Assets.Game.Objects.Obstacles
{
    public interface IObstacle : IObject
    {
        // Properties
        IItem NemesisBehaviour { get; }
    }
}
