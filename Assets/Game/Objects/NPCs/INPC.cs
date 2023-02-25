#nullable enable

#region Imports

using Assets.Game.Objects.Players;
using Assets.Game.Objects.Rooms;

#endregion

namespace Assets.Game.Objects.NPCs
{
    public interface INpc : IObject, IHasUI
    {
        #region Public methods

        string Meet();
        void StartConversation(IPlayer player, IRoom room);

        #endregion
    }
}