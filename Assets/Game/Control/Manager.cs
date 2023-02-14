using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Game.Control
{
    internal class Manager : MonoBehaviour
    {
        // Fields
        private bool exitGame = false;
        private bool winGame = false;

        //private IPlayer player;

        //private ISet<IRoom> rooms;

        private Key quitKey = Key.Escape;
        private Key helpKey = Key.H;
        private Key moveWestKey = Key.W;
        private Key moveSouthKey = Key.S;
        private Key moveEastKey = Key.E;
        private Key moveNorthKey = Key.N;

        private Keyboard keyboard;

        // Methods
        // Begin MonoBehaviour
        public void Start()
        {
            // Set up all the obstacles

            // Set up all the items

            // Set up all the rooms

            // Hold a reference to the current keyboard
            keyboard = Keyboard.current;

        }
    }
}
