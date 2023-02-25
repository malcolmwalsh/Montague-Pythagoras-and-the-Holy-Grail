using Assets.Game.Objects.Players;
using Assets.Game.Objects.Rooms;

#nullable enable

namespace Assets.Game.Objects.NPCs
{
    public interface INPC : IObject, IHasUI
    {
        string Meet();        
        void StartConversation(IPlayer player, IRoom room);
    }
}
