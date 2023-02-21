namespace Assets.Game.Objects
{
    internal abstract class ObjectFactory<S> where S : IObject
    {
        internal abstract S GetObject(string itemName);
    }
}
