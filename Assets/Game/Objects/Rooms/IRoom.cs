using Assets.Game.Navigation;
using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;

#nullable enable
namespace Assets.Game.Objects.Rooms
{
    public interface IRoom : IObject
    {
        // Properties
        bool IsStartRoom { get; }
        bool IsFinalRoom { get; }        

        // Methods
        bool HasItem();
        IItem? GetItemBehaviour();
        int NumDoors();
        string DoorLocationText();
        bool HasDoorInDirection(Enums.CompassDirection direction);
        IDoor? GetDoorInDirection(Enums.CompassDirection direction);
    }
}
