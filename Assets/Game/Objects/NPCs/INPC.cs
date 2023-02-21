#nullable enable

namespace Assets.Game.Objects.NPCs
{
    public interface INPC : IObject
    {
        string Meet();
        string? Talk();
        string Leave();
        string? Ask(string question);
        bool CanAskQuestions();
    }
}
