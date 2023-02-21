using Assets.Game.Objects.Players;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static Assets.Game.Navigation.Enums;

#nullable enable
namespace Assets.Game.Control
{
    internal class UI : MonoBehaviour
    {
        // Constructors
        
        // Fields
        public event EventHandler? HelpEvent;
        public event EventHandler? NewGameEvent;
        public event EventHandler? QuitGameEvent;
        
        public event EventHandler? InspectRoomEvent;
        public event EventHandler<MoveInDirectionEventArgs>? TryMoveToRoomEvent;

        private Keyboard keyboard;

        private IDictionary<Key, CompassDirection>? movementKeys;

        // Methods
        // Begin MonoBehaviour
        public void Awake()
        {
            // Hold a reference to the current keyboard
            keyboard = Keyboard.current;            
        }

        public void Update()
        {
            if(!keyboard.anyKey.wasPressedThisFrame) return;  // Nothing pressed

            // Loop through all keys and find the one pressed 
            // TODO: Seems very inefficient!
            foreach (KeyControl keyID in keyboard.allKeys)
            {
                if (keyID.wasPressedThisFrame)
                {
                    // What is pressed
                    Key pressedKey = keyID.keyCode;

                    FireEvent(pressedKey);
                }
            }
        }
        // End MonoBehaviour

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

        private void PrintInvalidKeyText(Key key)
        {
            string text = $"Invalid key ({key}). Try again, or press {KeyBindings.helpKey} for help";
            UI.PrintText(text);
        }

        // Static
        public static void PrintText(string? text)
        {
            if (text is not null) MonoBehaviour.print(text);
        }

        public static void ClearLog()
        {
            // https://stackoverflow.com/questions/40577412/clear-editor-console-logs-from-script

            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        public static void PrintHelpText()
        {
            UI.ClearLog();

            // Help text
            string text = $"Use the {KeyBindings.moveNorthKey}, {KeyBindings.moveSouthKey}, {KeyBindings.moveWestKey} and {KeyBindings.moveEastKey} to move North, South, West and East respectively. Use {KeyBindings.inspectKey} to inspect a room for items. \n" +
                $"Press {KeyBindings.helpKey} for this help at any time. To bravely run away, press {KeyBindings.quitKey} to return to the main menu and then {KeyBindings.quitKey} again to exit the game altogether";

            UI.PrintText(text);
        }
    }
}

