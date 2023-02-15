namespace Assets.Game.Objects
{
    internal interface IDoor : IObject
    {
        bool ConnectsRoom(IRoom room);

        // Methods
        IRoom GetConnectingRoom(IRoom currentRoom);
        bool IsLocked();
        bool TryTraverse(IPlayer player);
    }
}
