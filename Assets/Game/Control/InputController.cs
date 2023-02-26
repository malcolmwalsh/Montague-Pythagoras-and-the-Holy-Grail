using System;
using System.Reflection;
using Assets.Game.Objects.NPCs;
using Assets.Game.Objects.Players;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Assets.Game.Control
{
    internal class InputController : MonoBehaviour
    {
        #region Private fields

        private Keyboard keyboard;

        #endregion

        #region Properties

        #endregion

        #region Public methods

        public event EventHandler HelpEvent;
        public event EventHandler NewGameEvent;
        public event EventHandler QuitGameEvent;

        public event EventHandler InspectRoomEvent;
        public event EventHandler<MoveInDirectionEventArgs> TryMoveToRoomEvent;

        public event EventHandler TalkToNPCEvent;

        public event EventHandler<RespondToNpcArgs> RespondToNPCEvent;


        public void Awake()
        {
            // Hold a reference to the current keyboard
            keyboard = Keyboard.current;
        }

        public void Update()
        {
            if (!keyboard.anyKey.wasPressedThisFrame) return; // Nothing pressed

            // Loop through all keys and find the one pressed 
            // TODO: Seems very inefficient!
            foreach (KeyControl keyID in keyboard.allKeys)
                if (keyID.wasPressedThisFrame)
                {
                    // What is pressed
                    Key pressedKey = keyID.keyCode;

                    FireEvent(pressedKey);
                }
        }

        public void FireEvent(Key key)
        {
            if (key.Equals(KeyBindings.helpKey) && (HelpEvent is not null))
            {
                // Help
                HelpEvent.Invoke(this, EventArgs.Empty);
            }
            else if (key.Equals(KeyBindings.newGameKey) && (NewGameEvent is not null))
            {
                // New Game
                NewGameEvent.Invoke(this, EventArgs.Empty);
            }
            else if (key.Equals(KeyBindings.quitKey) && (QuitGameEvent is not null))
            {
                // Quit
                QuitGameEvent.Invoke(this, EventArgs.Empty);
            }
            else if (key.Equals(KeyBindings.inspectKey) && (InspectRoomEvent is not null))
            {
                // Inspect room
                InspectRoomEvent.Invoke(this, EventArgs.Empty);
            }
            else if (key.Equals(KeyBindings.talkKey) && (TalkToNPCEvent is not null))
            {
                // Talk to NPC
                TalkToNPCEvent.Invoke(this, EventArgs.Empty);
            }
            else if (KeyBindings.responseKeys.ContainsKey(key) && (RespondToNPCEvent is not null))
            {
                // Respond to NPC
                RespondToNpcArgs args = new()
                {
                    ResponseNum = KeyBindings.responseKeys[key]
                };

                RespondToNPCEvent.Invoke(this, args);
            }
            else if (KeyBindings.movementKeys.ContainsKey(key) && (TryMoveToRoomEvent is not null))
            {
                // Try move
                MoveInDirectionEventArgs args = new()
                {
                    Direction = KeyBindings.movementKeys[key]
                };
                TryMoveToRoomEvent.Invoke(this, args);
            }
            else
            {
                PrintInvalidKeyText(key);
            }
        }

        public void PrintText(string text)
        {
            if (text is null) return;

            print(text);
        }

        public void PrintTextAndPrompt(string text, IHasUI caller)
        {
            // Print the text
            PrintText(text);

            if (caller == null) return;  // No caller, no prompt

            string promptText = caller.Prompt();
            print(promptText);
        }

        public void ClearLog()
        {
            // https://stackoverflow.com/questions/40577412/clear-editor-console-logs-from-script

            var assembly = Assembly.GetAssembly(typeof(Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        public void PrintHelpText()
        {
            ClearLog();

            // Help text
            string text =
                $"Use the {KeyBindings.moveNorthKey}, {KeyBindings.moveSouthKey}, {KeyBindings.moveWestKey} and {KeyBindings.moveEastKey} to move North, South, West and East respectively. " +
                $"Use {KeyBindings.inspectKey} to inspect a room for items. Use {KeyBindings.talkKey} to speak with NPCs. \n" +
                $"Press {KeyBindings.helpKey} for this help at any time. To bravely run away, press {KeyBindings.quitKey} to return to the main menu and then {KeyBindings.quitKey} again to exit the game altogether";

            PrintText(text);
        }

        #endregion

        #region Private methods

        private void PrintInvalidKeyText(Key key)
        {
            string text = $"Invalid key ({key}). Try again, or press {KeyBindings.helpKey} for help";
            PrintText(text);
        }

        #endregion
    }
}