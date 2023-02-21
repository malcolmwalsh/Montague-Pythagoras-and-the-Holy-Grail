using System.Collections.Generic;

#nullable enable
namespace Assets.Game.Objects.NPCs
{
    public abstract class BridgeKeeper : NPC
    {
        // Constructors
        public BridgeKeeper(string name, string description, bool canAskQuestions, int favouriteNumber) : base(name, description, canAskQuestions)
        {
            this.favouriteNumber = favouriteNumber;
        }

        // Fields
        private int lineIndex = 0;

        protected IList<string> lines = new List<string>(); // Will use these the first time he's encountered        

        protected readonly int favouriteNumber;

        // Properties

        // Methods
        public override string Meet()
        {
            // Meet the NPC

            // Return the intro text;
            return Description;
        }

        public override string? Talk()
        {
            string? line;            

            if (lineIndex >= lines.Count)
            {
                // Run out of lines
                line = null;
            }
            else
            {
                line = lines[lineIndex];
                lineIndex++;
            }

            return line;
        }

        public override bool CanAskQuestions()
        {
            return canAskQuestions;
        }
    }
}
