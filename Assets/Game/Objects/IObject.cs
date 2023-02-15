namespace Assets.Game.Objects
{
    internal interface IObject
    {
        // Properties
        string Name { get; }
        string Description { get; }

        // Methods
        string ToString()
        {
            return Name;
        }
    }
}
