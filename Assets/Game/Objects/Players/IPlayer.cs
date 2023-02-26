using Assets.Game.Objects.Backpacks;

namespace Assets.Game.Objects.Players
{
    public interface IPlayer : IObject, IBackpack
    {
        #region Public methods

        void ConversationOver();
        void TurnIntoNewt();
        void PrintIntroduction();

        #endregion
    }
}