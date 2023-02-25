using Assets.Game.Objects.Obstacles;

#nullable enable

namespace Assets.Game.Objects.Items
{
    public interface IItem : IObject
    {
        // Properties
        ObstacleController Nemesis { get; }
    }
}
