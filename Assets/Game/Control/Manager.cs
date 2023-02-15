using Assets.Game.Objects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Assets.Game.Control
{
    internal class Manager : MonoBehaviour
    {
        // Fields
        private bool exitGame = false;
        private bool winGame = false;

        private IPlayer player;

        private ISet<IRoom> rooms;

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
            print("Manager started");

            // Set up all the obstacles

            // Set up all the items

            // Set up all the rooms

            // Hold a reference to the current keyboard
            keyboard = Keyboard.current;

        }

        public void Update()
        {
            if (!Keyboard.current.anyKey.wasPressedThisFrame) return;  // Nothing pressed

            // Loop through all keys and find the one pressed 
            // TODO: Seems very inefficient!
            foreach (KeyControl k in keyboard.allKeys)
            {
                if (k.wasPressedThisFrame)
                {
                    // What is pressed
                    Key pressedKey = k.keyCode;

                    if (pressedKey.Equals(quitKey))
                    {
                        // Quit game
                        QuitGame();
                        break;
                    }
                    else if (pressedKey.Equals(helpKey))
                    {
                        // TODO
                    }
                    else
                    {
                        // TODO
                    }
                }
            }
        }
        // End MonoBehaviour

        private void QuitGame()
        {
            // Quitting game
            exitGame = true;
        }
    }
}
