namespace Assets.Game.Objects
{
    internal interface IRoom : IObject
    {
        // Properties
        bool IsStartRoom { get; }
        bool IsFinalRoom { get; }

    }
}
