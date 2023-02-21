#nullable enable
using UnityEngine;

namespace Assets.Game.Objects.NPCs
{
    public abstract class NPC : INPC
    {
        // Constructor
        public NPC(string name, string description, bool canAskQuestions)
        {
            this.name = name;
            this.description = description;
            this.canAskQuestions = canAskQuestions;
        }

        // Fields
        private readonly new string name;
        private readonly string description;
        protected readonly bool canAskQuestions;

        // Properties
        public string Name => name;
        public string Description => description;

        // Methods
        public abstract string Meet();
        public abstract string? Talk();
        public abstract string Leave();
        public abstract bool CanAskQuestions();
        public abstract string? Ask(string question);
    }
}
