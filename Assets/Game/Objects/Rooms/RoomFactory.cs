using System;

namespace Assets.Game.Objects.Rooms
{
    internal class RoomFactory : ObjectFactory<IRoom>
    {
        internal override IRoom GetObject(string itemName)
        {
            return itemName switch
            {
                "Closet" => new Room(itemName, "You're currently in a medium-sized closet. ", true, false),
                "Outside" => new Room(itemName, "The freezing wind hits your face as you step outside and into the blizzard. ", false, true),
                "GrandEntrance" => new Room(itemName, "This is a huge entrance room with doors on all four walls. "),
                "Library" => new Room(itemName, "This library has books floor to ceiling. "),
                "DiningHall" => new Room(itemName, "You're in a cavernous room with a large diniing table in the middle. "),
                "BilliardsRoom" => new Room(itemName, "This is a relatively small room with a large billiards table in the centre. "),
                _ => throw new ApplicationException(string.Format($"Room `{itemName}` cannot be created")),
            }; ;
        }
    }
}
