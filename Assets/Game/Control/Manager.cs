using Assembly_CSharp;
using Assets.Game.Objects;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Obstacles;
using Assets.Game.Objects.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private Key inspectKey = Key.I;

        private Key moveNorthKey = Key.W;
        private Key moveSouthKey = Key.S;
        private Key moveWestKey = Key.A;        
        private Key moveEastKey = Key.D;        
        private IDictionary<Key, CompassDirection> movementKeys;

        private Keyboard keyboard;

        // Methods
        // Begin MonoBehaviour
        public void Start()
        {
            PrintText("Manager started");

            // Set up all the obstacles
            PrintText("Creating obstacles...");
            ObjectFactory<IObstacle> obstacleFactory = new ObstacleFactory();
            IObstacle brassLock = obstacleFactory.GetObject("BrassLock");
            IObstacle enemy = obstacleFactory.GetObject("Enemy");
            IObstacle bitterCold = obstacleFactory.GetObject("BitterCold");
            obstacles = new HashSet<IObstacle>() { bitterCold, brassLock, enemy };
            PrintText("...done");

            // Set up all the items
            PrintText("Creating items...");
            ObjectFactory<IItem> itemFactory = new ItemFactory();
            IItem woollyHat = itemFactory.GetObject("WoollyHat");
            IItem magicSword = itemFactory.GetObject("MagicSword");
            IItem brassKey = itemFactory.GetObject("BrassKey");
            items = new HashSet<IItem>() { woollyHat, magicSword, brassKey };
            PrintText("...done");

            // Set up all the rooms
            PrintText("Creating rooms...");
            ObjectFactory<IRoom> roomFactory = new RoomFactory();
            IRoom startRoom = roomFactory.GetObject("StartRoom");
            IRoom outside = roomFactory.GetObject("Outside");
            IRoom grandEntrance = roomFactory.GetObject("GrandEntrance");
            IRoom library = roomFactory.GetObject("Library");
            IRoom diningHall = roomFactory.GetObject("DiningHall");
            IRoom billiardsRoom = roomFactory.GetObject("BilliardsRoom");
            rooms = new HashSet<IRoom>() { startRoom, outside, grandEntrance, library, diningHall, billiardsRoom };
            PrintText("...done");

            // Set up all the doors
            PrintText("Creating doors...");
            IDoor closetDoor = new Door("ClosetDoor", "A small flimsy door", roomA: startRoom, roomB: grandEntrance);
            IDoor brassDoor = new Door("BrassDoor", "A huge brass door with a small plaque: 'Library'", library, grandEntrance, brassLock, "A thick-set brass handle is locked tight", "With a satisfying click, the key turns the lock");
            IDoor guardedDoor = new Door("GuardedDoor", "Hard to see the door through the enormous knight in full armour that stands in front of it", billiardsRoom, grandEntrance, enemy, "There's no way to pass the guard without ending up dead, or worse", "\"Tis but a scratch\", shouts the knight, as you step over his limbless torso.");
            IDoor diningDoor = new Door("DiningDoor", "An ornate door with a small plaque: 'Dining hall'", roomA: grandEntrance, roomB: diningHall);
            IDoor exteriorDoor = new Door("ExteriorDoor", "A thick door with a mail slot", outside, diningHall, bitterCold, "There's no way you can survive out there without sensible attire", "Armed with your trusty woolly hat, you gather your things and venture forth.");
            doors = new HashSet<IDoor>() { closetDoor, brassDoor, guardedDoor, diningDoor, exteriorDoor };
            PrintText("...done");

            // Assign nemeses to obstacles
            PrintText("Assigning obstacles...");
            brassLock.SetNemesis(brassKey);
            bitterCold.SetNemesis(woollyHat);
            enemy.SetNemesis(magicSword);
            PrintText("...done");

            // Assign nemeses to items
            PrintText("Assigning items...");
            brassKey.SetNemesis(brassLock);
            woollyHat.SetNemesis(bitterCold);
            magicSword.SetNemesis(enemy);
            PrintText("...done");

            // Assign doors to rooms
            PrintText("Assigning doors to rooms...");
            AssignDoorBetweenRooms(grandEntrance, startRoom, closetDoor, CompassDirection.South);
            AssignDoorBetweenRooms(grandEntrance, library, brassDoor, CompassDirection.West);
            AssignDoorBetweenRooms(grandEntrance, diningHall, diningDoor, CompassDirection.East);
            AssignDoorBetweenRooms(grandEntrance, billiardsRoom, guardedDoor, CompassDirection.North);
            AssignDoorBetweenRooms(billiardsRoom, outside, exteriorDoor, CompassDirection.North);
            PrintText("...done");

            // Assign items to rooms
            PrintText("Assigning items to rooms...");
            grandEntrance.AddItem(woollyHat);
            diningHall.AddItem(brassKey);
            library.AddItem(magicSword);
            PrintText("...done");

            // Create a player object
            PrintText("Creating player...");
            player = new Player("Montague Pythagoras", "Our hero, but he is very small", startRoom);
            PrintText("...done");

            // Hold a reference to the current keyboard
            keyboard = Keyboard.current;

            // Put movement keys in a dictionary
            movementKeys = new Dictionary<Key, CompassDirection>() { 
                { moveNorthKey, CompassDirection.North },
                { moveEastKey, CompassDirection.East},
                { moveSouthKey, CompassDirection.South},
                { moveWestKey, CompassDirection.West } 
            };
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
                    }
                    else if (pressedKey.Equals(helpKey))
                    {
                        // Help
                        PrintHelpText();
                    }
                    else if (pressedKey.Equals(inspectKey))
                    {
                        // Inspect room
                        InspectRoom(player.CurrentRoom);
                    }
                    else if (movementKeys.Keys.Contains(pressedKey))
                    {
                        // Wants to move to another room
                        TryMoveIntoNewRoom(pressedKey);
                    }
                    else
                    {
                        // Invalid key
                        PrintInvalidKeyText(pressedKey);
                    }

                    break;  // Stop looping, found the key
                }
            }

            if (winGame)
            {
                // Won the game!
                PrintWinGameText();
            }

            if (exitGame)
            {
                // TODO: Exit the game
            }
        }
        // End MonoBehaviour

        private void PrintWinGameText()
        {
            throw new NotImplementedException();
        }

        private void PrintMovingIntoNewRoomText(IRoom currentRoom, IRoom newRoom)
        {
            string text = $"You open the door and pass from {currentRoom} into {newRoom}";
            PrintText(text);
        }

        private void PrintCannotEnterDoorText(IDoor selectedDoor)
        {
            string text = selectedDoor.GetBlockedText();
            PrintText(text);
        }        

        private void PrintText(string text)
        {
            print(text);
        }

        private void PrintInvalidKeyText(Key key)
        {
            string text = $"Invalid key ({key}). Try again, or press {helpKey} for help";
            PrintText(text);
        }

        private void PrintInvalidDirectionText(CompassDirection direction)
        {
            string text = $"No door in the direction selected ({direction}). Try again, or press {inspectKey} to inspect the room again, or press {helpKey} for help";
            PrintText(text);
        }

        private void PrintUnblockingDoorText(IDoor selectedDoor)
        {
            string text = selectedDoor.GetUnblockText();
            PrintText(text);
        }

        private void TryMoveIntoNewRoom(Key pressedKey)
        {
            // Get direction from dictionary
            CompassDirection direction = movementKeys[pressedKey];

            // Pick up current room from player
            IRoom currentRoom = player.CurrentRoom;

            // Is there a door there?
            if (!currentRoom.HasDoorInDirection(direction))
            {
                // No door in that direction
                // Return text to indicate that to player
                PrintInvalidDirectionText(direction);
            }
            else
            {
                // Is door in that direction
                IDoor selectedDoor = currentRoom.GetDoorInDirection(direction)!;

                // Is the door blocked / locked
                if (selectedDoor.IsBlocked())
                {
                    // Door is blocked

                    // Check player can unlock door
                    if (!selectedDoor.TryTraverse(player))
                    {
                        // Cannot unblock door

                        // Return text that indicates that
                        PrintCannotEnterDoorText(selectedDoor);
                    }
                    else
                    {
                        // Can unblock door

                        // Print text that door is unlocked by using item
                        PrintUnblockingDoorText(selectedDoor);

                        // Unblock door
                        selectedDoor.Unblock();
                    }
                }

                // Door was either unblocked already or has been unblocked	
                if (!selectedDoor.IsBlocked())
                {
                    // Get room that connects with the current room using this door	                                
                    IRoom newRoom = selectedDoor.GetConnectingRoom(currentRoom);

                    // Print text informing player that they are moving into new room
                    PrintMovingIntoNewRoomText(currentRoom, newRoom);

                    // Set new room as current room
                    player.CurrentRoom = newRoom;

                    // Describe new room
                    PrintRoomDescriptionText(newRoom);

                    // TODO: Check for NPC in room

                    // Check whether this is the final room
                    if (newRoom.IsFinalRoom)
                    {
                        // Set win
                        winGame = true;
                    }
                }
            }
        }

        private void PrintRoomDescriptionText(IRoom newRoom)
        {
            string text = newRoom.Description;
            PrintText(text);
        }

        private void PrintHelpText()
        {
            // TODO: Help text
            throw new NotImplementedException();
        }

        private void InspectRoom(IRoom room)
        {
            // TODO: Has to tell the player where the doors are


            string text = $"You walk around the room slowly, pushing and prodding at things.";

            // Check for items in the room
            if (room.HasItem())
            {
                text += "A shiny object grabs your attention.\n";

                // Get the item
                IItem? item = room.GetItem();

                if (item != null)
                {
                    // Shouldn't be null as we checked above
                    text += $"You see a {item} and greedily put it in your backpack.";

                    // Player now has this item
                    player.AddItem(item);
                }
            }
            else
            {
                text += "In the end, there's nothing of interest.";
            }

            PrintText(text);
        }        

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
