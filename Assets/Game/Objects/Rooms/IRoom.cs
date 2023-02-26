using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.NPCs;
using static Assets.Game.Navigation.Enums;

namespace Assets.Game.Objects.Rooms
{
    public interface IRoom : IObject
    {
        #region Properties

        // Properties
        INpc NPC { get; }
        bool IsStartRoom { get; }
        bool IsFinalRoom { get; }

        #endregion

        #region Public methods

        // Methods
        bool HasItem();
        ItemController GetItem();
        int NumDoors();
        string DoorLocationText();
        bool HasDoorInDirection(CompassDirection direction);
        IDoor GetDoorInDirection(CompassDirection direction);
        bool HasNPC();
        void RemoveNPC(INpc npc);

        #endregion
    }
}