namespace Assets.Game.Objects
{
    internal interface IPlayer : IObject
    {
        // Properties
        IRoom CurrentRoom { get; set; }

        // Methods
        bool HasItem(IItem item);
        void AddItem(IItem item);
    }
}
