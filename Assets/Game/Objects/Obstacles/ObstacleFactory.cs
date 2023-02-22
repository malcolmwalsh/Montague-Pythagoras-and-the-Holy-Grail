using System;

namespace Assets.Game.Objects.Obstacles
{
    internal class ObstacleFactory : ObjectFactory<IObstacle>
    {
        internal override IObstacle GetObject(string itemName)
        {
            return itemName switch
            {
                "Enemy" => new ObstacleLogic(itemName, "A massive knight in full plate armour"),
                "BrassLock" => new ObstacleLogic(itemName, "A large lock made of brass"),
                "BitterCold" => new ObstacleLogic(itemName, "The wind is frigid and the snow swirls and dances"),
                _ => throw new ApplicationException(string.Format($"Obstacle `{itemName}` cannot be created")),
            };
        }
    }
}
