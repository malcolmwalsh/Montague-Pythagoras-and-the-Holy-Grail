using Assets.Game.Objects.Items;
using System;

namespace Assets.Game.Objects.Rooms
{
    internal class RoomFactory : ObjectFactory<IRoom>
    {
        internal override IRoom GetObject(string itemName)
        {
            return itemName switch
            {
                "StartRoom" => new StartRoom(),
                "Outside" => new OutsideRoom(),
                "GrandEntrance" => new GrandEntrance(),
                _ => throw new ApplicationException(string.Format($"Room `{itemName}` cannot be created")),
            };
        }
    }
}
