using System.Collections.Generic;
using UnityEngine.InputSystem;
using static Assets.Game.Navigation.Enums;

namespace Assets.Game.Control
{
    internal static class KeyBindings
    {
        // Fields
        internal static Key newGameKey = Key.N;
        internal static Key quitKey = Key.Escape;
        internal static Key helpKey = Key.H;

        internal static Key moveNorthKey = Key.W;
        internal static Key moveSouthKey = Key.S;
        internal static Key moveWestKey = Key.A;
        internal static Key moveEastKey = Key.D;

        internal static Key inspectKey = Key.I;

        internal static Key talkKey = Key.T;

        internal static Key response0Key = Key.Digit1;
        internal static Key response1Key = Key.Digit2;
        internal static Key response2Key = Key.Digit3;

        internal static IDictionary<Key, CompassDirection> movementKeys = new Dictionary<Key, CompassDirection>() {
                { KeyBindings.moveNorthKey, CompassDirection.North },
                { KeyBindings.moveEastKey, CompassDirection.East },
                { KeyBindings.moveSouthKey, CompassDirection.South },
                { KeyBindings.moveWestKey, CompassDirection.West }
            };

        internal static IDictionary<Key, int> responseKeys = new Dictionary<Key, int>() {
                { KeyBindings.response0Key, 0 },
                { KeyBindings.response1Key, 1 },
                { KeyBindings.response2Key, 2 }
            };
    }
}
