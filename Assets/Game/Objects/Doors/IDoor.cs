namespace Assets.Game.Objects
{
    internal interface IDoor : IObject
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
