namespace Assets.Game.Objects
{
    internal interface IDoor
    {
        IRoom GetConnectingRoom(IRoom currentRoom);
        bool IsLocked();
        bool TryTraverse(IPlayer player);
    }
}
