namespace Assets.Game.Objects.Items
{
    internal abstract class ObjectFactory<S> where S : IObject
    {
        internal abstract S GetObject(string itemName);
    }
}
