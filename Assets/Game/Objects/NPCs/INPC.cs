using Assets.Game.Objects.Players;
using Assets.Game.Objects.Rooms;

namespace Assets.Game.Objects.NPCs
{
    public interface INpc : IObject, IHasUI, IHasBackpack
    {
        #region Public methods

        string Meet();
        void StartConversation(IPlayer player, IRoom room);

        #endregion
    }
}