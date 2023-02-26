using Assets.Game.Objects.Items;

namespace Assets.Game.Objects.Players
{
    public interface IPlayer : IObject
    {
        #region Public methods

        bool HasItem(IItem item);
        void ConversationOver();
        void TurnIntoNewt();
        void PrintIntroduction();

        #endregion
    }
}