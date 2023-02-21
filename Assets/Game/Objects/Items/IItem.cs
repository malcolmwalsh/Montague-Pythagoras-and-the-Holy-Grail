#nullable enable
using Assets.Game.Objects.Obstacles;

namespace Assets.Game.Objects.Items
{
    public interface IItem : IObject
    {
        // Properties
        IObstacle? NemisisObstacle { get; }

        void SetNemesis(IObstacle obstacle);
    }
}
