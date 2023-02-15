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
                "Enemy" => new Obstacle(itemName, "A massive knight in full plate armour"),
                "BrassLock" => new Obstacle(itemName, "A large lock made of brass"),
                "BitterCold" => new Obstacle(itemName, "The wind is frigid and the snow swirls and dances"),
                _ => throw new ApplicationException(string.Format($"Obstacle `{itemName}` cannot be created")),
            };
        }
    }
}
