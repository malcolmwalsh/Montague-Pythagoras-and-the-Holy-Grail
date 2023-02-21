using Assets.Game.Objects;
using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.NPCs;
using Assets.Game.Objects.Obstacles;
using Assets.Game.Objects.Players;
using Assets.Game.Objects.Rooms;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static Assets.Game.Navigation.Enums;

#nullable enable
namespace Assets.Game.Control
{
    public class Manager : MonoBehaviour
    {
        // Fields
        private bool exitGame = false;
        private bool winGame = false;

        private IPlayer? player;

        private ISet<IDoor>? doors;
        private ISet<IObstacle>? obstacles;
        private ISet<IRoom>? rooms;
        private ISet<IItem>? items;
        private ISet<INPC>? npcs;

        private Key newGameKey = Key.N;
        private Key quitKey = Key.Escape;

        private Key helpKey = Key.H;
        private Key inspectKey = Key.I;

        private Key moveNorthKey = Key.W;
        private Key moveSouthKey = Key.S;
        private Key moveWestKey = Key.A;        
        private Key moveEastKey = Key.D;        
        private IDictionary<Key, CompassDirection>? movementKeys;

        private Keyboard? keyboard;

        private enum GameState
        {
            MainMenu, 
            Navigation, 
            NPCInteraction
        }
        private GameState gameState = GameState.MainMenu;  // Start in main menu state

        // Properties
        public bool WinGame { get => winGame; set => winGame = value; }

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
            IItem pinkCowboyHat = itemFactory.GetObject("PinkCowboyHat");
            items = new HashSet<IItem>() { woollyHat, magicSword, brassKey, pinkCowboyHat };
            PrintText("...done");

            // Set up all the rooms
            PrintText("Creating rooms...");
            ObjectFactory<IRoom> roomFactory = new RoomFactory();
            IRoom closet = roomFactory.GetObject("Closet");
            IRoom outside = roomFactory.GetObject("Outside");
            IRoom grandEntrance = roomFactory.GetObject("GrandEntrance");
            IRoom library = roomFactory.GetObject("Library");
            IRoom diningHall = roomFactory.GetObject("DiningHall");
            IRoom billiardsRoom = roomFactory.GetObject("BilliardsRoom");
            rooms = new HashSet<IRoom>() { closet, outside, grandEntrance, library, diningHall, billiardsRoom };
            PrintText("...done");

            // Set up all the doors
            PrintText("Creating doors...");
            IDoor closetDoor = new Door("ClosetDoor", "A small flimsy door", roomA: closet, roomB: grandEntrance);
            IDoor brassDoor = new Door("BrassDoor", "A huge brass door with a small plaque: 'Library'", library, grandEntrance, brassLock, "A thick-set brass handle is locked tight", "With a satisfying click, the key turns the lock");
            IDoor guardedDoor = new Door("GuardedDoor", "Hard to see the door through the enormous knight in full armour that stands in front of it", billiardsRoom, grandEntrance, enemy, "There's no way to pass the guard without ending up dead, or worse", "\"Tis but a scratch\", shouts the knight, as you step over his limbless torso.");
            IDoor diningDoor = new Door("DiningDoor", "An ornate door with a small plaque: 'Dining hall'", roomA: grandEntrance, roomB: diningHall);
            IDoor exteriorDoor = new Door("ExteriorDoor", "A thick door with a mail slot", outside, diningHall, bitterCold, "There's no way you can survive out there without sensible attire", "Armed with your trusty woolly hat, you gather your things and venture forth.");
            doors = new HashSet<IDoor>() { closetDoor, brassDoor, guardedDoor, diningDoor, exteriorDoor };
            PrintText("...done");

            // Set up all the doors
            PrintText("Creating NPCs...");
            int favouriteNumber = Random.Range(23, 43);
            INPC bridgeKeeper0 = new BridgeKeeper0("The BridgeKeeper", "An old, hagged man smelling of spam approaches you.", favouriteNumber);
            INPC bridgeKeeper1 = new BridgeKeeper1("The BridgeKeeper", "The old, hagged man from earlier skips towards you.", favouriteNumber);
            npcs = new HashSet<INPC>() { bridgeKeeper0, bridgeKeeper1 };
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
            AssignDoorBetweenRooms(grandEntrance, closet, closetDoor, CompassDirection.South);
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
            closet.AddItem(pinkCowboyHat);
            PrintText("...done");

            // Assign NPC to room
            PrintText("Assigning NPCs to rooms...");
            IList<IRoom> roomsOKWithNPC = rooms.Where(r => !r.IsFinalRoom).Where(r => !r.IsStartRoom).ToList<IRoom>();

            // TODO: Need to avoid the bridgekeeper0 being in the first, final and penultimate room the first time

            // Randomly choose a room for the bridgeKeeper to first appear in
            int roomIndex = Random.Range(0, roomsOKWithNPC.Count() - 1);
            IRoom roomForNPC = roomsOKWithNPC[roomIndex];
            roomForNPC.AddNPC(bridgeKeeper0);

            // Add second bridgekeeper to penultimate room
            // TODO: Add isPenultimateRoom flag on rooms
            billiardsRoom.AddNPC(bridgeKeeper1);
            PrintText("...done");

            // Create a player object
            PrintText("Creating player...");
            player = new Player("Montague Pythagoras", "Our hero, but he is very small", closet);
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

            // Main menu
            PrintMainMenu();
        }

        public void Update()
        {
            if (!Keyboard.current.anyKey.wasPressedThisFrame) return;  // Nothing pressed

            // Loop through all keys and find the one pressed 
            // TODO: Seems very inefficient!
            foreach (KeyControl keyID in keyboard!.allKeys)
            {
                if (keyID.wasPressedThisFrame)
                {
                    // What is pressed
                    Key pressedKey = keyID.keyCode;

                    if (pressedKey.Equals(helpKey))
                    {
                        // Help
                        PrintHelpText();
                    } 
                    else if (gameState.Equals(GameState.MainMenu))
                    {
                        // In main menu

                        if (pressedKey.Equals(quitKey))
                        {
                            // Quit game (will set exitGame = true)
                            QuitGame();
                        }
                        else if (pressedKey.Equals(newGameKey))
                        {
                            // New game
                            StartNewGame();

                            // Introduction
                            PrintIntroduction();

                            // Inspect initial room
                            InspectRoom(player!.CurrentRoom);
                        }
                        else
                        {
                            // Invalid key
                            PrintInvalidKeyText(pressedKey);
                        }
                    }
                    else if (gameState.Equals(GameState.Navigation))
                    {
                        // Navigating the game

                        if (pressedKey.Equals(inspectKey))
                        {
                            // Inspect room
                            InspectRoom(player!.CurrentRoom);
                        }
                        else if (movementKeys!.Keys.Contains(pressedKey))
                        {
                            // Wants to move to another room
                            TryMoveIntoNewRoom(pressedKey);
                        }
                        else if (pressedKey.Equals(quitKey))
                        {
                            // Quit run
                            QuitRun();

                            // Main menu
                            PrintMainMenu();
                        }
                        else
                        {
                            // Invalid key
                            PrintInvalidKeyText(pressedKey);
                        }
                    }                    
                }
            }

            if (winGame)
            {
                // Won the game!
                PrintWinGameText();
            }

            if (exitGame)
            {
                // Exit the game
                ExitGame();
            }
        }
        // End MonoBehaviour

        private void ExitGame()
        {
            string text = "When you're chewing on life's gristle, don't grumble, give a whistle. And this'll help things turn out for the best.";
            PrintText(text);

            #if UNITY_STANDALONE
                Application.Quit();
            #endif
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif            
        }        

        private void StartNewGame()
        {
            // For the menu options, now in a game
            this.gameState = GameState.Navigation;

            ClearLog();

            string text = "A new game. You must go into it and love everyone, try to make everyone happy, and bring peace and contentment everywhere you go.\n" +
                "Although you'll do better at the game if you don't";

            PrintText(text);
        }

        private void PrintIntroduction()
        {
            string text = $"You find yourself in a medium-sized closet, surrounded by various tins, jars, blankets, brooms, and a single pink cowboy hat.\n" +
                $"As nice as the closet is, you'd rather be outside, leaping from tree to tree as they float down the mighty rivers of British Columbia!";

            PrintText(text);
        }

        private void PrintMainMenu()
        {
            ClearLog();

            // Menu
            string text = $"Welcome to Monague Pythagoras and the Holey Grail\nPress {helpKey} for help, {newGameKey} for a new game or {quitKey} to shuffle of this mortal coil";

            PrintText(text);
        }

        private void PrintWinGameText()
        {
            // For the key press detection, no longer in a game
            this.gameState = GameState.MainMenu;

            string text = $"You walk off into the freezing night, only stopping briefly to turn around and look back. There's mixed emotions, but overall you feel content.\n";
            text += $"Then, without warning a police car pulls up and the officers jump out and start shouting at you and reaching for their tasers.";
            
            PrintText(text);
        }

        private void PrintMovingIntoNewRoomText(IRoom currentRoom, IRoom newRoom)
        {
            ClearLog();

            string text = $"You open the door and pass from {currentRoom} into {newRoom}";
            PrintText(text);
        }

        private void PrintCannotEnterDoorText(IDoor selectedDoor)
        {
            string text = selectedDoor.GetBlockedText();
            PrintText(text);
        }

        private void PrintText(string? text)
        {
            if (text is not null) { 
                print(text);
            }
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
            CompassDirection direction = movementKeys![pressedKey];

            // Pick up current room from player
            IRoom currentRoom = player!.CurrentRoom;

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

                    // Check for NPC in room
                    if (newRoom.HasNPC())
                    {
                        // There's an NPC here
                        INPC npc = newRoom.GetNPC()!;

                        InteractWithNPC(npc);
                    }

                    // Check whether this is the final room
                    if (newRoom.IsFinalRoom)
                    {
                        // Set win
                        winGame = true;
                        exitGame = true;
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
            ClearLog();

            // Help text
            string text = $"Use the {moveNorthKey}, {moveSouthKey}, {moveWestKey} and {moveEastKey} to move North, South, West and East respectively. Use {inspectKey} to inspect a room for items. \n" +
                $"Press {helpKey} for this help at any time. To bravely run away, press {quitKey} to return to the main menu and then {quitKey} again to exit the game altogether";

            PrintText(text);
        }

        private void InspectRoom(IRoom room)
        {
            string text = room.DoorLocationText();

            text += $"\nYou walk around the room slowly, pushing and prodding at things.";

            // Check for items in the room
            if (room.HasItem())
            {
                // Get the item
                IItem? item = room.GetItem();

                if (item != null)
                {
                    // Shouldn't be null as we checked above
                    text += $" You see a {item}. {item.Description}";

                    // Player now has this item
                    player!.AddItem(item);
                }
            }
            else
            {
                text += " In the end, there's nothing of interest.";
            }

            PrintText(text);
        }

        private void QuitRun()
        {
            // For the menu options, no longer in a game
            this.gameState = GameState.MainMenu;

            string text = "This run is no more";

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

        private void ClearLog()
        {
            // https://stackoverflow.com/questions/40577412/clear-editor-console-logs-from-script

            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        private void InteractWithNPC(INPC npc)
        {
            // Meet the npc
            string line = npc.Meet();
            PrintText(line);

            // Interact with npc
            string? chat = npc.Talk();
            while (chat is not null)
            {                
                PrintText(chat);

                chat = npc.Talk();
            }

            // Ask NPC a question
            if (npc.CanAskQuestions())
            {
                string question = "Which kind of swallow: African or European?";
                PrintText(question);

                string? response = npc.Ask(question);
                PrintText(response);
            }

            // NPC leaves
            line = npc.Leave();
            PrintText(line);
        }
    }
}
