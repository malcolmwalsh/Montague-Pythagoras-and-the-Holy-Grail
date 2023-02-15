#nullable enable
namespace Assets.Game.Objects
{
    internal class BrassLock : IObstacle
    {
        // Fields
        private IItem? nemesisItem;

        // Properties
        public IItem? NemesisItem => nemesisItem;

        // Methods
        public void SetNemesis(IItem item)
        {
            nemesisItem = item;
        }
    }
}
