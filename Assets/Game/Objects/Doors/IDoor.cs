using Assets.Game.Objects.Players;
using Assets.Game.Objects.Rooms;

namespace Assets.Game.Objects.Doors
{
    public interface IDoor : IObject
    {
        bool ConnectsRoom(IRoom room);
        string GetBlockedText();

        // Methods
        IRoom GetConnectingRoom(IRoom currentRoom);
        string GetUnblockText();
        bool IsBlocked();
        bool TryTraverse(IPlayer player);
        void Unblock();
    }
}
