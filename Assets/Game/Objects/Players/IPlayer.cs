namespace Assets.Game.Objects.Players
{
    public interface IPlayer : IObject, IHasBackpack
    {
        #region Public methods

        void ConversationOver();
        void TurnIntoNewt();
        void PrintIntroduction();

        #endregion
    }
}