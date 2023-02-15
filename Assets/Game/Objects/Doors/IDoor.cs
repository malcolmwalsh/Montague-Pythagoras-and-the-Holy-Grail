namespace Assets.Game.Objects
{
    internal interface IDoor : IObject
    {
        // Methods
        IRoom GetConnectingRoom(IRoom currentRoom);
        bool IsLocked();
        bool TryTraverse(IPlayer player);
    }
}
