using Assets.Game.Objects;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Obstacles;
using Assets.Game.Objects.Rooms;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

#nullable enable
namespace Assets.Game.Control
{
    internal class Manager : MonoBehaviour
    {
        // Fields
        private bool exitGame = false;
        private bool winGame = false;

        private IPlayer player;

        private ISet<IDoor> doors;
        private ISet<IObstacle> obstacles;
        private ISet<IRoom> rooms;
        private ISet<IItem> items;

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
            print("Creating obstacles...");
            ObjectFactory<IObstacle> obstacleFactory = new ObstacleFactory();
            IObstacle brassLock = obstacleFactory.GetObject("BrassLock");
            IObstacle enemy = obstacleFactory.GetObject("Enemy");
            IObstacle bitterCold = obstacleFactory.GetObject("BitterCold");
            obstacles = new HashSet<IObstacle>(new List<IObstacle>() { bitterCold, brassLock, enemy });
            print("...done");

            // Set up all the items
            print("Creating items...");
            ObjectFactory<IItem> itemFactory = new ItemFactory();
            IItem woollyHat = itemFactory.GetObject("WoollyHat");
            IItem magicSword = itemFactory.GetObject("MagicSword");
            IItem brassKey = itemFactory.GetObject("BrassKey");
            items = new HashSet<IItem>(new List<IItem>() { woollyHat, magicSword, brassKey });
            print("...done");

            // Set up all the rooms
            print("Creating rooms...");
            ObjectFactory<IRoom> roomFactory = new RoomFactory();
            print("...done");

            // Assign nemeses to obstacles
            print("Assigning to obstacles...");
            brassLock.SetNemesis(brassKey);
            bitterCold.SetNemesis(woollyHat);
            enemy.SetNemesis(magicSword);
            print("...done");

            // Assign nemeses to items
            print("Assigning to items...");
            brassKey.SetNemesis(brassLock);
            woollyHat.SetNemesis(bitterCold);
            magicSword.SetNemesis(enemy);
            print("...done");

            // Assign obstacles to rooms
            print("Assigning to rooms...");
            print("...done");

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
