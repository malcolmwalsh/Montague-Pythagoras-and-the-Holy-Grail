using static Assets.Game.Navigation.Enums;

namespace Assets.Game.Objects
{
    internal interface IPlayer : IObject
    {
        // Properties
        IRoom CurrentRoom { get; set; }

        // Methods
        bool HasItem(IItem item);
        void InspectRoom();        
        void TryMove(CompassDirection direction);
    }
}
