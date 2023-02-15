using Assets.Game.Objects.Items;
using System;

namespace Assets.Game.Objects.Obstacles
{
    internal class ObstacleFactory : ObjectFactory<IObstacle>
    {
        internal override IObstacle GetObject(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
