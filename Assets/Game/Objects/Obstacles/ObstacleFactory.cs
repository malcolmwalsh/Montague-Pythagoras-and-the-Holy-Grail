using Assets.Game.Objects.Items;
using System;

namespace Assets.Game.Objects.Obstacles
{
    internal class ObstacleFactory : ObjectFactory<IObstacle>
    {
        internal override IObstacle GetObject(string itemName)
        {
            return itemName switch
            {
                "Enemy" => new Enemy(),
                "BrassLock" => new BrassLock(),
                "BitterCold" => new BitterCold(),
                _ => throw new ApplicationException(string.Format($"Obstacle `{itemName}` cannot be created")),
            };
        }
    }
}
