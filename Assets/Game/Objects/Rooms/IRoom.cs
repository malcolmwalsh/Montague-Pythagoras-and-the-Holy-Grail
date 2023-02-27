using System.Collections.Generic;
using Assets.Game.Objects.Backpacks;
using Assets.Game.Objects.Doors;
using Assets.Game.Objects.NPCs;
using static Assets.Game.Navigation.Enums;

namespace Assets.Game.Objects.Rooms
{
    public interface IRoom : IObject, IBackpack
    {
        #region Properties

        INpc NPC { get; }
        bool IsStartRoom { get; }
        bool IsFinalRoom { get; }

        #endregion

        #region Public methods

        int NumDoors();
        string DoorLocationText();
        bool HasDoorInDirection(CompassDirection direction);
        IDoor GetDoorInDirection(CompassDirection direction);
        bool HasNPC();
        void RemoveNPC(INpc npc);

        #endregion

        IDictionary<CompassDirection, IDoor> GetDoors();
    }
}