#nullable enable
using static UnityEngine.Random;

namespace Assets.Game.Objects.NPCs
{
    public class BridgeKeeper0 : BridgeKeeperBehaviour
    {
        // Methods
        public override void Start()
        {
            base.Start();

            // Sample a random value for the airspeed velocity of a sparrow, as you do
            randomResult = Range(46, 63);
        }        
    }
}
