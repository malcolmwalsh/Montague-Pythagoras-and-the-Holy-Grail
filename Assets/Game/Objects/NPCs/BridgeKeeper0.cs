using static UnityEngine.Random;

#nullable enable

namespace Assets.Game.Objects.NPCs
{
    public class BridgeKeeper0 : BridgeKeeperController
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
