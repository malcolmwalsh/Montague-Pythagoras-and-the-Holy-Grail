using System;

namespace Assets.Game.Objects.Rooms
{
    internal class RoomFactory : ObjectFactory<IRoom>
    {
        internal override IRoom GetObject(string itemName)
        {
            return itemName switch
            {
                "Closet" => new RoomLogic(itemName, "You're currently in a medium-sized closet. ", true, false),
                "Outside" => new RoomLogic(itemName, "The freezing wind hits your face as you step outside and into the blizzard. ", false, true),
                "GrandEntrance" => new RoomLogic(itemName, "This is a huge entrance room with doors on all four walls. "),
                "Library" => new RoomLogic(itemName, "This library has books floor to ceiling. "),
                "DiningHall" => new RoomLogic(itemName, "You're in a cavernous room with a large dining table in the middle. "),
                "BilliardsRoom" => new RoomLogic(itemName, "This is a relatively small room with a large billiards table in the centre. "),
                _ => throw new ApplicationException(string.Format($"Room `{itemName}` cannot be created")),
            }; ;
        }
    }
}
