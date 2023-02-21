using System.Collections.Generic;

namespace Assets.Game.Objects.NPCs
{
    public class BridgeKeeper : NPC
    {
        // Constructors
        public BridgeKeeper(string name, string description) : base(name, description)
        {
            firstLines.Add("Hello tiny human, are you looking for something?");

            secondLines.Add("What is your favourite colour?");
        }

        // Fields
        private bool alreadyInteracted = false; // Will become true after the first interaction with this NPC

        private int lineIndex;

        private IList<string> firstLines = new List<string>(); // Will use these the first time he's encountered
        private IList<string> secondLines = new List<string>(); // Will use thes the second time

        // Properties

        // Methods
        public override void Meet()
        {
            // Meet the NPC

            // We've met him once now
            alreadyInteracted = true;

            // Reset the line number
            lineIndex= 0;
        }

        public override string Talk()
        {
            IList<string> lines;
            if (!alreadyInteracted)
            {
                lines = firstLines;                
            }
            else
            {
                lines = secondLines;
            }

            // Get the next line
            lineIndex++;

            if (lineIndex >= lines.Count)
            {
                // Can't go outside the list length
                lineIndex = lines.Count - 1;
            }            

            return lines[lineIndex];
        }
    }
}
