using Assembly_CSharp;
using Assets.Game.Objects;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Obstacles;
using Assets.Game.Objects.Rooms;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static Assets.Game.Navigation.Enums;

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
            IRoom startRoom = roomFactory.GetObject("StartRoom");
            IRoom outside = roomFactory.GetObject("Outside");
            IRoom grandEntrance = roomFactory.GetObject("GrandEntrance");
            IRoom library = roomFactory.GetObject("Library");
            IRoom diningHall = roomFactory.GetObject("DiningHall");
            IRoom billiardsRoom = roomFactory.GetObject("BilliardsRoom");
            rooms = new HashSet<IRoom>(new List<IRoom>() { startRoom, outside, grandEntrance });
            print("...done");

            // Set up all the doors
            print("Creating doors...");
            IDoor closetDoor = new Door("ClosetDoor", "A small flimsy door", roomA: startRoom, roomB: grandEntrance);
            IDoor brassDoor = new Door("BrassDoor", "A huge brass door with a small plaque: 'Library'", library, grandEntrance, brassLock);
            IDoor guardedDoor = new Door("GuardedDoor", "Hard to see the door through the enormous knight in full armour that stands in front of it", billiardsRoom,grandEntrance, enemy);
            IDoor diningDoor = new Door("DiningDoor", "An ornate door with a small plaque: 'Dining hall'", roomA: grandEntrance, roomB: diningHall);
            IDoor exteriorDoor = new Door("ExteriorDoor", "A thick door with a mail slot", outside, diningHall, bitterCold);
            doors = new HashSet<IDoor>(new List<IDoor>() { closetDoor, brassDoor, guardedDoor, diningDoor, exteriorDoor });
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

            // Assign doors to rooms
            print("Assigning to rooms...");
            AssignDoorBetweenRooms(grandEntrance, startRoom, closetDoor, CompassDirection.South);
            AssignDoorBetweenRooms(grandEntrance, library, brassDoor, CompassDirection.West);
            AssignDoorBetweenRooms(grandEntrance, diningHall, diningDoor, CompassDirection.East);
            AssignDoorBetweenRooms(grandEntrance, billiardsRoom, guardedDoor, CompassDirection.North);
            AssignDoorBetweenRooms(billiardsRoom, outside, exteriorDoor, CompassDirection.North);
            print("...done");

            // Create a player object
            print("Creating player...");
            player = new Player("Player", "Our hero, but he is very small");
            print("...done");

            // Hold a reference to the current keyboard
            keyboard = Keyboard.current;
        }

        public void Update()
        {
            if (!Keyboard.current.anyKey.wasPressedThisFrame) return;  // Nothing pressed

            // Loop through all keys and find the one pressed 
            // TODO: Seems very inefficient!
            foreach (KeyControl keyID in keyboard.allKeys)
            {
                if (keyID.wasPressedThisFrame)
                {
                    // What is pressed
                    Key pressedKey = keyID.keyCode;

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

        private static void AssignDoorBetweenRooms(IRoom roomA,
                                                   IRoom roomB,
                                                   IDoor door,
                                                   CompassDirection direction)
        {
            CompassDirection oppositeDirection = GetOppositeDirection(direction);

            roomA.SetDoorInDirection(direction, door);
            roomB.SetDoorInDirection(oppositeDirection, door);
        }

        private static CompassDirection GetOppositeDirection(CompassDirection direction)
        {
            return direction switch
            {
                CompassDirection.North => CompassDirection.South,
                CompassDirection.South => CompassDirection.North,
                CompassDirection.East => CompassDirection.West,
                CompassDirection.West => CompassDirection.East,
                _ => throw new System.NotImplementedException($"Unknown direction: {direction}"),
            };
        }
    }
}
