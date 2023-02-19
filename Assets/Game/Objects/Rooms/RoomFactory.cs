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
                "Closet" => new Room(itemName, "A medium-sized closet", true, false),
                "Outside" => new Room(itemName, "The freezing wind hits your face...", false, true),
                "GrandEntrance" => new Room(itemName, "A huge entrance room with doors on all four walls"),
                "Library" => new Room(itemName, "There's books floor to ceiling."),
                "DiningHall" => new Room(itemName, "An cavernous room with a large diniing table in the middle"),
                "BilliardsRoom" => new Room(itemName, "A small room with a large billiards table in the centre"),
                _ => throw new ApplicationException(string.Format($"Room `{itemName}` cannot be created")),
            }; ;
        }
    }
}
