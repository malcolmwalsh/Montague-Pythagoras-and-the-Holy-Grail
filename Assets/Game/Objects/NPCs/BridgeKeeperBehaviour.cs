#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Objects.NPCs
{
    public class BridgeKeeperBehaviour : NPCBehaviour
    {
        // Parameters
        [SerializeField] protected BridgeKeeperBehaviour linkedBridgeKeeper;

        // Fields        
        protected int randomResult;

        // Properties
        public int RandomResult { get => randomResult; }

        // Methods
        public override string Retort(string response)
        {
            // Replacing placeholder text with actual random result value
            this.primaryRetort = UpdateTextWithRandomResult(this.primaryRetort)!;
            this.secondaryRetort = UpdateTextWithRandomResult(this.secondaryRetort);

            return base.Retort(response);
        }

        public override string Talk()
        {
            // Replacing placeholder text with actual random result value
            string modLine = UpdateTextWithRandomResult(primaryLine)!;

            return modLine;
        }

        private string UpdateTextWithRandomResult(string line)
        {
            return line.Replace("#randomResult", RandomResult.ToString());
        }

        public override List<string> GetPlayerResponses()
        {
            // Replacing placeholder text with actual random result value
            List<string> modPlayerResponses = new();

            foreach (string response in playerResponses)
            {
                modPlayerResponses.Add(UpdateTextWithRandomResult(response));
            }

            // Use these ones
            playerResponses = modPlayerResponses;

            return playerResponses;
        }
    }
}
