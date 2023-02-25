using Assets.Game.Objects.Players;
using System;
using UnityEngine;

#nullable enable
namespace Assets.Game.Control
{
    public class ManagerBehaviour : MonoBehaviour, IHasUI
    {
        // Parameters
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject ui;

        // Fields        
        private InputBehaviour? uiBehaviour;        
        private PlayerBehaviour? playerBehaviour;

        // Properties
        
        // Methods
        // Begin MonoBehaviour
        public void Awake()
        {
            // Set up UI
            InputBehaviour.PrintText("Creating UI...");            
            uiBehaviour = ui.GetComponent<InputBehaviour>();
            uiBehaviour.HelpEvent += HelpTextEvent;
            uiBehaviour.NewGameEvent += StartNewGameEvent;
            uiBehaviour.QuitGameEvent += ExitGameEvent;
            InputBehaviour.PrintText("...done");
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
            InputBehaviour.PrintHelpText();
        }        

        private void StartNewGameEvent(object sender, EventArgs e)
        {
            StartNewGame();
        }        

        private void StartNewGame()
        {
            InputBehaviour.ClearLog();
            string text = "A new game. You must go into it and love everyone, try to make everyone happy, and bring peace and contentment everywhere you go.\n" +
                "Although you'll do better at the game if you don't";
            InputBehaviour.PrintText(text);

            // Don't want out UI anymore
            DisableUI();

            // Activate player
            player.SetActive(true);

            // Introduction
            PrintIntroduction();
        }

        private void ExitGame()
        {
            string text = "When you're chewing on life's gristle, don't grumble, give a whistle. And this'll help things turn out for the best.";
            InputBehaviour.PrintText(text);

            text = "Exiting game";
            InputBehaviour.PrintText(text);

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

        public void LoseGame(bool isNewt)
        {
            if (isNewt)
            {
                string newtText = "You were always pretty small, but now you're a newt, no longer than the blade of your axe. Your adventure is over.";
                InputBehaviour.PrintText(newtText);
            }

            ExitGame();
        }

        internal void QuitRun()
        {
            string text = "This run is no more";
            InputBehaviour.PrintText(text);

            // Our UI need to be up again now
            EnableUI();

            //// Destroy our player
            //Destroy(player);

            //PrintMainMenu();

            ExitGame();
        }

        public void EnableUI()
        {
            // Enable our ui so we do detect key presses
            uiBehaviour!.enabled = true;  
        }

        public void DisableUI()
        {
            // Shut down our UI so we don't detect key presses
            uiBehaviour!.enabled = false;
        }

        private void PrintIntroduction()
        {
            string text = $"You find yourself in a medium-sized closet, surrounded by various tins, jars, blankets, brooms, and a single pink cowboy hat.\n" +
                $"As nice as the closet is, you'd rather be outside, leaping from tree to tree as they float down the mighty rivers of British Columbia!";

            InputBehaviour.PrintText(text);
        }

        private void PrintMainMenu()
        {
            InputBehaviour.ClearLog();

            // Menu
            string text = $"Welcome to Monague Pythagoras and the Holey Grail\nPress {KeyBindings.helpKey} for help, {KeyBindings.newGameKey} for a new game or {KeyBindings.quitKey} to shuffle of this mortal coil";

            InputBehaviour.PrintText(text);
        }

        private void PrintWinGameText()
        {
            string text = $"You walk off into the freezing night, only stopping briefly to turn around and look back. There's mixed emotions, but overall you feel content.\n";
            text += $"Then, without warning a police car pulls up and the officers jump out and start shouting at you and reaching for their tasers. This is as good as it gets. Well done!";
            
            InputBehaviour.PrintText(text);
        }

        public string Prompt()
        {
            return "prompt here";
        }
    }
}
