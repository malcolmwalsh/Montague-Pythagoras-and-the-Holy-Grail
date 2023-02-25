using Assets.Game.Objects.Players;
using Assets.Game.Objects.Rooms;

#nullable enable

namespace Assets.Game.Objects.NPCs
{
    public interface INPC : IObject, IHasUI
    {
        string Meet();
        string Describe();
        string Greeting();
        string Talk();
        string Retort(string response);
        void Leave(bool happy);
        void StartConversation(IPlayer player, IRoom room);
    }
}
