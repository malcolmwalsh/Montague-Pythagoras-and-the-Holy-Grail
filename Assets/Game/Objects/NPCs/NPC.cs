namespace Assets.Game.Objects.NPCs
{
    public abstract class NPC : INPC
    {
        // Constructor
        public NPC(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        // Fields
        private readonly string name;
        private readonly string description;

        // Properties
        public string Name => name;
        public string Description => description;

        // Methods
        public abstract void Meet();
        public abstract string Talk();
    }
}
