using Assets.Game.Objects.Players;
using System;
using UnityEngine;

#nullable enable
namespace Assets.Game.Control
{
    public class ManagerBehaviour : MonoBehaviour, IHasUI
    {
        // Parameters
        [SerializeField] private PlayerBehaviour player;
        [SerializeField] private InputBehaviour ui;

        // Fields        

        // Properties
        
        // Methods
        // Begin MonoBehaviour
        public void Awake()
        {
            // Set up UI
            ui.HelpEvent += HelpTextEvent;
            ui.NewGameEvent += StartNewGameEvent;
            ui.QuitGameEvent += ExitGameEvent;

            ui.Prompt = Prompt();
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
            ui.PrintHelpText();
        }        

        private void StartNewGameEvent(object sender, EventArgs e)
        {
            StartNewGame();
        }        

        private void StartNewGame()
        {
            ui.ClearLog();
            string text = "A new game. You must go into it and love everyone, try to make everyone happy, and bring peace and contentment everywhere you go.\n" +
                "Although you'll do better at the game if you don't";
            ui.PrintText(text);

            // Don't want out UI anymore
            DisableUI();

            // Introduction
            player.PrintIntroduction();
        }

        private void ExitGame()
        {
            string text = "When you're chewing on life's gristle, don't grumble, give a whistle. And this'll help things turn out for the best.";
            ui.PrintText(text);

            text = "Exited Game";
            ui.PrintText(text);

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
                ui.PrintText(newtText);
            }

            ExitGame();
        }

        internal void QuitRun()
        {
            string text = "This run is no more";
            ui.PrintText(text);

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
            ui.enabled = true;  
        }

        public void DisableUI()
        {
            // Shut down our UI so we don't detect key presses
            ui.enabled = false;
        }

        private void PrintMainMenu()
        {
            ui.ClearLog();

            // Menu
            string text = $"Welcome to Monague Pythagoras and the Holey Grail";                

            ui.PrintText(text, addPrompt: true);
        }

        private void PrintWinGameText()
        {
            string text = $"You walk off into the freezing night, only stopping briefly to turn around and look back. There's mixed emotions, but overall you feel content.\n";
            text += $"Then, without warning a police car pulls up and the officers jump out and start shouting at you and reaching for their tasers. This is as good as it gets. Well done!";
            
            ui.PrintText(text);
        }

        public string Prompt()
        {
            string text = $"Press {KeyBindings.helpKey} for help, {KeyBindings.newGameKey} for a new game or {KeyBindings.quitKey} to shuffle of this mortal coil";

            return text;
        }
    }
}
