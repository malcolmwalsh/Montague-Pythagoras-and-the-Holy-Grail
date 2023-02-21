#nullable enable
namespace Assets.Game.Objects.NPCs
{
    // Constructors
    internal class BridgeKeeper1 : BridgeKeeper
    {
        public BridgeKeeper1(string name, string description, int favouriteNumber) : base(name, description, true, favouriteNumber)
        {
            lines.Add("What is your favourite colour?");
            lines.Add("What is the capital of Assyria?");
            lines.Add("What is my favourite number?");
            lines.Add("What is the airspeed velocity of an unladen swallow?");
        }

        // Fields
        private bool hasBeenTricked = false;  // Will be true when he is defeated
        private string trickQuestion = "Which kind of swallow: African or European?";

        // Methods
        public override string Leave()
        {            
            string text;

            if (!hasBeenTricked)
            {
                // First time or he's not been defeated
                text = "The old man picks up a small wicker basket filled with fruit and merrily skips off";
            }
            else
            {
                text = "The old man looks horrified and then, without warning, he explodes into thousands of geraniums, newts and intestines.";
            }

            return text;            
        }

        public override string? Ask(string question)
        {
            string response;

            if (question == trickQuestion)
            {
                hasBeenTricked = true;

                response = "Er, I don't know that!";
            }
            else
            {
                response = "You're a very naughty boy!";
            }

            return response;
        }
    }
}
