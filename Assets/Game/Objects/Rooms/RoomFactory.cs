using Assets.Game.Objects.Items;

namespace Assets.Game.Objects.Rooms
{
    internal class RoomFactory : ObjectFactory<IRoom>
    {
        internal override IRoom GetObject(string itemName)
        {
            throw new System.NotImplementedException();
        }
    }
}
