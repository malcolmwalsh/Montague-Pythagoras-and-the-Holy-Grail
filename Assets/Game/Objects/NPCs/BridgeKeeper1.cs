#nullable enable
namespace Assets.Game.Objects.NPCs
{
    // Constructors
    public class BridgeKeeper1 : BridgeKeeperBehaviour
    {
        // Methods
        public override void Start()
        {
            base.Start();

            // Pick up the value from the other guy
            randomResult = linkedBridgeKeeper.RandomResult;
        }
    }
}
