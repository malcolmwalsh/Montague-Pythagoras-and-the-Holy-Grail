using Assets.Game.Objects.Players;
using System;
using UnityEngine;
using static Assets.Game.Navigation.Enums;

#nullable enable
namespace Assets.Game.Control
{
    public class ManagerBehaviour : MonoBehaviour
    {
        // Parameters
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject ui;

        // Fields        
        private UIBehaviour? uiBehaviour;        
        private PlayerBehaviour? playerBehaviour;

        // Properties
        //public bool WinGame { get => winGame; set => winGame = value; }

        // Methods
        // Begin MonoBehaviour
        public void Awake()
        {
            // Set up UI
            UIBehaviour.PrintText("Creating UI...");            
            uiBehaviour = ui.GetComponent<UIBehaviour>();
            uiBehaviour.HelpEvent += HelpTextEvent;
            uiBehaviour.NewGameEvent += StartNewGameEvent;
            uiBehaviour.QuitGameEvent += ExitGameEvent;
            UIBehaviour.PrintText("...done");
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
            UIBehaviour.PrintHelpText();
        }        

        private void StartNewGameEvent(object sender, EventArgs e)
        {
            StartNewGame();
        }        

        private void StartNewGame()
        {
            UIBehaviour.ClearLog();
            string text = "A new game. You must go into it and love everyone, try to make everyone happy, and bring peace and contentment everywhere you go.\n" +
                "Although you'll do better at the game if you don't";
            UIBehaviour.PrintText(text);

            // Shut down our UI so we don't detect key presses
            uiBehaviour!.enabled = false;

            // Activate player
            player.SetActive(true);

            // Introduction
            PrintIntroduction();
        }

        private void ExitGame()
        {
            string text = "When you're chewing on life's gristle, don't grumble, give a whistle. And this'll help things turn out for the best.";
            UIBehaviour.PrintText(text);

            text = "Exiting game";
            UIBehaviour.PrintText(text);

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

            // Enable our ui
            uiBehaviour!.enabled = true;  // Our UI need to be up again now

            //// Destroy our player
            //Destroy(player);

            //PrintMainMenu();

            ExitGame();
        }

        internal void QuitRun()
        {
            string text = "This run is no more";
            UIBehaviour.PrintText(text);

            // Enable our ui
            uiBehaviour!.enabled = true;  // Our UI need to be up again now

            //// Destroy our player
            //Destroy(player);

            //PrintMainMenu();

            ExitGame();
        }

        private void PrintIntroduction()
        {
            string text = $"You find yourself in a medium-sized closet, surrounded by various tins, jars, blankets, brooms, and a single pink cowboy hat.\n" +
                $"As nice as the closet is, you'd rather be outside, leaping from tree to tree as they float down the mighty rivers of British Columbia!";

            UIBehaviour.PrintText(text);
        }

        private void PrintMainMenu()
        {
            UIBehaviour.ClearLog();

            // Menu
            string text = $"Welcome to Monague Pythagoras and the Holey Grail\nPress {KeyBindings.helpKey} for help, {KeyBindings.newGameKey} for a new game or {KeyBindings.quitKey} to shuffle of this mortal coil";

            UIBehaviour.PrintText(text);
        }

        private void PrintWinGameText()
        {
            string text = $"You walk off into the freezing night, only stopping briefly to turn around and look back. There's mixed emotions, but overall you feel content.\n";
            text += $"Then, without warning a police car pulls up and the officers jump out and start shouting at you and reaching for their tasers. Your adventure is over.";
            
            UIBehaviour.PrintText(text);
        }                
    }
}
