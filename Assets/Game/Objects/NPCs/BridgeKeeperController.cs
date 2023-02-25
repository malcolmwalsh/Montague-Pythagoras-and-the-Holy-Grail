using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Objects.NPCs
{
    public class BridgeKeeperController : NpcController
    {
        #region Protected fields

        [SerializeField] protected BridgeKeeperController linkedBridgeKeeper;


        protected int randomResult;

        #endregion

        #region Properties

        public int RandomResult => randomResult;

        #endregion

        #region Protected methods

        protected override string Retort(string response)
        {
            // Replacing placeholder text with actual random result value
            this.primaryRetort = UpdateTextWithRandomResult(this.primaryRetort)!;
            this.secondaryRetort = UpdateTextWithRandomResult(this.secondaryRetort);

            return base.Retort(response);
        }

        protected override string Talk()
        {
            // Replacing placeholder text with actual random result value
            string modLine = UpdateTextWithRandomResult(primaryLine)!;

            return modLine;
        }

        protected override IList<string> GetPlayerResponses()
        {
            // Replacing placeholder text with actual random result value
            List<string> modPlayerResponses = playerResponses.Select(response => UpdateTextWithRandomResult(response)).ToList();

            // Use these ones
            playerResponses = modPlayerResponses;

            return playerResponses;
        }

        #endregion

        #region Private methods

        private string UpdateTextWithRandomResult(string line)
        {
            return line.Replace("#randomResult", RandomResult.ToString());
        }

        #endregion
    }
}