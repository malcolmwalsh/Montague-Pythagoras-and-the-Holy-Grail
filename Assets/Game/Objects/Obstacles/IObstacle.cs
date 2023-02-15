namespace Assets.Game.Objects
{
    internal interface IObstacle : IObject
    {
        // Properties
        IItem NemesisItem { get; }

        void SetNemesis(IItem item);
    }
}
