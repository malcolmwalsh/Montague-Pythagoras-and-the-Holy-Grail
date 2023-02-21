using Assets.Game.Objects;
using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Obstacles;
using Assets.Game.Objects.Players;
using Assets.Game.Objects.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
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
        private IPlayer player;

        private System.Collections.Generic.ISet<IDoor> doors;
        private System.Collections.Generic.ISet<IObstacle> obstacles;
        private System.Collections.Generic.ISet<IRoom> rooms;
        private System.Collections.Generic.ISet<IItem> items;
        
        private UI ui;

        // Properties
        //public bool WinGame { get => winGame; set => winGame = value; }

        // Methods
        // Begin MonoBehaviour
        public void Awake()
        {
            UI.PrintText("Manager started");

            // Set up all the obstacles
            UI.PrintText("Creating obstacles...");
            ObjectFactory<IObstacle> obstacleFactory = new ObstacleFactory();
            IObstacle brassLock = obstacleFactory.GetObject("BrassLock");
            IObstacle enemy = obstacleFactory.GetObject("Enemy");
            IObstacle bitterCold = obstacleFactory.GetObject("BitterCold");
            obstacles = new HashSet<IObstacle>() { bitterCold, brassLock, enemy };
            UI.PrintText("...done");

            // Set up all the items
            UI.PrintText("Creating items...");
            ObjectFactory<IItem> itemFactory = new ItemFactory();
            IItem woollyHat = itemFactory.GetObject("WoollyHat");
            IItem magicSword = itemFactory.GetObject("MagicSword");
            IItem brassKey = itemFactory.GetObject("BrassKey");
            IItem pinkCowboyHat = itemFactory.GetObject("PinkCowboyHat");
            items = new HashSet<IItem>() { woollyHat, magicSword, brassKey, pinkCowboyHat };
            UI.PrintText("...done");

            // Set up all the rooms
            UI.PrintText("Creating rooms...");
            ObjectFactory<IRoom> roomFactory = new RoomFactory();
            IRoom closet = roomFactory.GetObject("Closet");
            IRoom outside = roomFactory.GetObject("Outside");
            IRoom grandEntrance = roomFactory.GetObject("GrandEntrance");
            IRoom library = roomFactory.GetObject("Library");
            IRoom diningHall = roomFactory.GetObject("DiningHall");
            IRoom billiardsRoom = roomFactory.GetObject("BilliardsRoom");
            rooms = new HashSet<IRoom>() { closet, outside, grandEntrance, library, diningHall, billiardsRoom };
            UI.PrintText("...done");

            // Set up all the doors
            UI.PrintText("Creating doors...");
            IDoor closetDoor = new Door("ClosetDoor", "A small flimsy door", roomA: closet, roomB: grandEntrance);
            IDoor brassDoor = new Door("BrassDoor", "A huge brass door with a small plaque: 'Library'", library, grandEntrance, brassLock, "A thick-set brass handle is locked tight", "With a satisfying click, the key turns the lock");
            IDoor guardedDoor = new Door("GuardedDoor", "Hard to see the door through the enormous knight in full armour that stands in front of it", billiardsRoom, grandEntrance, enemy, "There's no way to pass the guard without ending up dead, or worse", "\"Tis but a scratch\", shouts the knight, as you step over his limbless torso.");
            IDoor diningDoor = new Door("DiningDoor", "An ornate door with a small plaque: 'Dining hall'", roomA: grandEntrance, roomB: diningHall);
            IDoor exteriorDoor = new Door("ExteriorDoor", "A thick door with a mail slot", outside, diningHall, bitterCold, "There's no way you can survive out there without sensible attire", "Armed with your trusty woolly hat, you gather your things and venture forth.");
            doors = new HashSet<IDoor>() { closetDoor, brassDoor, guardedDoor, diningDoor, exteriorDoor };
            UI.PrintText("...done");

            // Assign nemeses to obstacles
            UI.PrintText("Assigning obstacles...");
            brassLock.SetNemesis(brassKey);
            bitterCold.SetNemesis(woollyHat);
            enemy.SetNemesis(magicSword);
            UI.PrintText("...done");

            // Assign nemeses to items
            UI.PrintText("Assigning items...");
            brassKey.SetNemesis(brassLock);
            woollyHat.SetNemesis(bitterCold);
            magicSword.SetNemesis(enemy);
            UI.PrintText("...done");

            // Assign doors to rooms
            UI.PrintText("Assigning doors to rooms...");
            AssignDoorBetweenRooms(grandEntrance, closet, closetDoor, CompassDirection.South);
            AssignDoorBetweenRooms(grandEntrance, library, brassDoor, CompassDirection.West);
            AssignDoorBetweenRooms(grandEntrance, diningHall, diningDoor, CompassDirection.East);
            AssignDoorBetweenRooms(grandEntrance, billiardsRoom, guardedDoor, CompassDirection.North);
            AssignDoorBetweenRooms(billiardsRoom, outside, exteriorDoor, CompassDirection.North);
            UI.PrintText("...done");

            // Assign items to rooms
            UI.PrintText("Assigning items to rooms...");
            grandEntrance.AddItem(woollyHat);
            diningHall.AddItem(brassKey);
            library.AddItem(magicSword);
            closet.AddItem(pinkCowboyHat);
            UI.PrintText("...done");
            
            // Set up UI
            ui = gameObject.AddComponent<UI>();
            ui.HelpEvent += HelpTextEvent;
            ui.NewGameEvent += StartNewGameEvent;
            ui.QuitGameEvent += ExitGameEvent;
        }

        public void Start()
        {
            // Main menu
            PrintMainMenu();
        }
        // End MonoBehaviour

        private void ExitGameEvent(object sender, EventArgs e)
        {
            ExitGame();
        }

        private void HelpTextEvent(object sender, EventArgs e)
        {
            UI.PrintHelpText();
        }        

        private void StartNewGameEvent(object sender, EventArgs e)
        {
            StartNewGame();
        }        

        private void StartNewGame()
        {
            UI.ClearLog();

            string text = "A new game. You must go into it and love everyone, try to make everyone happy, and bring peace and contentment everywhere you go.\n" +
                "Although you'll do better at the game if you don't";

            UI.PrintText(text);

            // Create a player object
            UI.PrintText("Creating player...");
            player = gameObject.AddComponent<PlayerBehaviour>();
            player.Name = "Montague Pythagoras";
            player.Description = "Our hero, but he is very small";
            player.Manager = this;
            player.CurrentRoom = rooms.First(r => r.IsStartRoom);
            UI.PrintText("...done");

            // Shut down our UI
            ui.enabled = false;

            // Introduction
            PrintIntroduction();
        }

        private void ExitGame()
        {
            string text = "When you're chewing on life's gristle, don't grumble, give a whistle. And this'll help things turn out for the best.";
            UI.PrintText(text);

            text = "Exiting game";
            UI.PrintText(text);

            #if UNITY_STANDALONE
                Application.Quit();
            #endif
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif            
        }

        internal void WinGame()
        {
            PrintWinGameText();

            ExitGame();
        }

        private void PrintIntroduction()
        {
            string text = $"You find yourself in a medium-sized closet, surrounded by various tins, jars, blankets, brooms, and a single pink cowboy hat.\n" +
                $"As nice as the closet is, you'd rather be outside, leaping from tree to tree as they float down the mighty rivers of British Columbia!";

            UI.PrintText(text);
        }

        private void PrintMainMenu()
        {
            UI.ClearLog();

            // Menu
            string text = $"Welcome to Monague Pythagoras and the Holey Grail\nPress {KeyBindings.helpKey} for help, {KeyBindings.newGameKey} for a new game or {KeyBindings.quitKey} to shuffle of this mortal coil";

            UI.PrintText(text);
        }

        private void PrintWinGameText()
        {
            string text = $"You walk off into the freezing night, only stopping briefly to turn around and look back. There's mixed emotions, but overall you feel content.\n";
            text += $"Then, without warning a police car pulls up and the officers jump out and start shouting at you and reaching for their tasers. Your adventure is over.";
            
            UI.PrintText(text);
        }

        public void QuitRun()
        {
            string text = "This run is no more";
            UI.PrintText(text);

            // Enable our ui
            ui.enabled = true;

            // TODO: Destroy our player            

            PrintMainMenu();
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
