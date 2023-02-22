using Assets.Game.Control;
using Assets.Game.Objects.Backpacks;
using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Rooms;
using System;
using UnityEngine;
using static Assets.Game.Navigation.Enums;

#nullable enable

namespace Assets.Game.Objects.Players
{
    internal class PlayerBehaviour : MonoBehaviour, IPlayer
    {
        // Parameters
        public GameObject uiObj;

        // Fields
        private string name;
        private string description;

        private ManagerBehaviour manager;
        private IRoom? currentRoom;

        private IBackpack backpack = new BackpackLogic();

        private GameObject myUIObj;
        private UIBehaviour ui;

        // Properties        
        public IRoom? CurrentRoom { get => currentRoom; set => currentRoom = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public ManagerBehaviour Manager { get => manager; set => manager = value; }

        // Methods

        // Begin MonoBehaviour
        public void Start()
        {
            Debug.Log("Player behaviour script starts");

            // Set up UI
            myUIObj = Instantiate(uiObj);
            ui = myUIObj.GetComponent<UIBehaviour>();
            ui.InspectRoomEvent += InspectRoomEvent;
            ui.TryMoveToRoomEvent += TryMoveToRoomEvent;
            ui.QuitGameEvent += QuitRunEvent;
            ui.HelpEvent += HelpTextEvent;
        }
        // End MonoBehaviour

        private void InspectRoomEvent(object sender, EventArgs e)
        {
            InspectRoom(this.currentRoom);
        }

        private void TryMoveToRoomEvent(object sender, MoveInDirectionEventArgs e)
        {
            TryMoveIntoNewRoom(e.Direction);
        }

        private void HelpTextEvent(object sender, EventArgs e)
        {
            UIBehaviour.PrintHelpText();
        }

        private void QuitRunEvent(object sender, EventArgs e)
        {
            // Quitting run

            // Shut down our ui
            ui.enabled = false;

            Manager.QuitRun();
        }

        // Begin IPlayer
        public bool HasItem(IItem item)
        {
            return backpack.Contains(item);
        }

        public void AddItem(IItem item)
        {
            this.backpack.Add(item);
        }
        // End IPlayer

        public override string ToString()
        {
            return Name;
        }

        private void TryMoveIntoNewRoom(CompassDirection direction)
        {
            // Pick up current room
            IRoom currentRoom = this.CurrentRoom!;

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
                    if (!selectedDoor.TryTraverse(this))
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
                    this.CurrentRoom = newRoom;

                    // Describe new room
                    PrintRoomDescriptionText(newRoom);

                    // TODO: Check for NPC in room

                    // Check whether this is the final room
                    if (newRoom.IsFinalRoom)
                    {
                        WinGame();
                    }
                }
            }
        }

        private void WinGame()
        {
            // Won the game

            // Shut down our UI
            ui.enabled = false;

            manager.WinGame();
        }

        private void PrintRoomDescriptionText(IRoom newRoom)
        {
            string text = newRoom.Description;
            UIBehaviour.PrintText(text);
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
                    this.AddItem(item);
                }
            }
            else
            {
                text += " In the end, there's nothing of interest.";
            }

            UIBehaviour.PrintText(text);
        }

        private void PrintInvalidDirectionText(CompassDirection direction)
        {
            string text = $"No door in the direction selected ({direction}). Try again, or press {KeyBindings.inspectKey} to inspect the room again, or press {KeyBindings.helpKey} for help";
            UIBehaviour.PrintText(text);
        }

        private void PrintUnblockingDoorText(IDoor selectedDoor)
        {
            string text = selectedDoor.GetUnblockText();
            UIBehaviour.PrintText(text);
        }

        private void PrintMovingIntoNewRoomText(IRoom currentRoom, IRoom newRoom)
        {
            UIBehaviour.ClearLog();

            string text = $"You open the door and pass from {currentRoom} into {newRoom}";
            UIBehaviour.PrintText(text);
        }

        private void PrintCannotEnterDoorText(IDoor selectedDoor)
        {
            string text = selectedDoor.GetBlockedText();
            UIBehaviour.PrintText(text);
        }

        public void Enable()
        {
            this.enabled = true;
        }

        public void Disable()
        {
            this.enabled = false;
        }
    }

    public class MoveInDirectionEventArgs : EventArgs
    {
        public CompassDirection Direction { get; set; } 
    }
}