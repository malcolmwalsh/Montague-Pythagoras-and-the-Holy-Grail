using System;
using Assets.Game.Objects.Players;
using UnityEditor;
using UnityEngine;

namespace Assets.Game.Control
{
    public class GameManager : MonoBehaviour, IHasUI
    {
        #region Private fields

        [SerializeField] private PlayerController player;
        [SerializeField] private InputController ui;

        #endregion

        #region IHasUI interface

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

        public string Prompt()
        {
            string text =
                $"Press {KeyBindings.helpKey} for help, {KeyBindings.newGameKey} for a new game or {KeyBindings.quitKey} to shuffle of this mortal coil";

            return text;
        }

        #endregion

        #region Public methods

        public void Awake()
        {
            // Set up UI
            ui.HelpEvent += HelpTextEvent;
            ui.NewGameEvent += StartNewGameEvent;
            ui.QuitGameEvent += ExitGameEvent;
        }

        public void Start()
        {
            // Main menu
            PrintMainMenu();
        }

        public void LoseGame(bool isNewt)
        {
            if (isNewt)
            {
                string newtText =
                    "You were always pretty small, but now you're a newt, no longer than the blade of your axe. Your adventure is over.";
                ui.PrintText(newtText);
            }

            ExitGame();
        }

        #endregion

        #region Internal methods

        internal void WinGame()
        {
            PrintWinGameText();

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

        #endregion

        #region Private methods

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
            string text =
                "A new game. You must go into it and love everyone, try to make everyone happy, and bring peace and contentment everywhere you go.\n" +
                "Although you'll do better at the game if you don't";
            ui.PrintText(text);

            // Don't want out UI anymore
            DisableUI();

            // Introduction
            player.PrintIntroduction();
        }

        private void ExitGame()
        {
            string text =
                "When you're chewing on life's gristle, don't grumble, give a whistle. And this'll help things turn out for the best.";
            ui.PrintText(text);

            text = "Exited game";
            ui.PrintText(text);

#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }

        private void PrintMainMenu()
        {
            ui.ClearLog();

            // Menu
            string text = $"Welcome to Montague Pythagoras and the Holy Grail";

            ui.PrintTextAndPrompt(text, this);
        }

        private void PrintWinGameText()
        {
            string text =
                "You walk off into the freezing night, only stopping briefly to turn around and look back. There's mixed emotions, but overall you feel content.\n" +
                "Then, without warning a police car pulls up and the officers jump out and start shouting at you and reaching for their tasers. This is as good as it gets. Well done!";

            ui.PrintText(text);
        }

        #endregion
    }
}