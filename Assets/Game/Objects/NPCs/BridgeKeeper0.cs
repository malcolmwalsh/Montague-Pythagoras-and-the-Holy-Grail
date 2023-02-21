#nullable enable
namespace Assets.Game.Objects.NPCs
{
    internal class BridgeKeeper0 : BridgeKeeper
    {
        // Constructors
        public BridgeKeeper0(string name, string description, int favouriteNumber) : base(name, description, false, favouriteNumber)
        {
            lines.Add("Hello tiny human, are you looking for something?");
            lines.Add("I am a perfectly normal person");
            lines.Add($"My favourite colour is olive and my favourite number is {favouriteNumber}.");
            lines.Add($"A witch once turned me into a newt.");
            lines.Add($"I got better.");
        }

        // Fields


        // Methods
        public override string Leave()
        {
            string text = "The old man picks up a small wicker basket filled with fruit and merrily skips off";

            return text;
        }        

        public override string? Ask(string question)
        {
            return null;
        }
    }
}
