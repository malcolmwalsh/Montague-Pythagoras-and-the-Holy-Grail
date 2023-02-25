namespace Assets.Game
{
    public interface IHasUI
    {
        // Methods
        void EnableUI();
        void DisableUI();

        string Prompt();
    }
}