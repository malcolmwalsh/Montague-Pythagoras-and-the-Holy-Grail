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
        [SerializeField] private string? description;

        [SerializeField] private GameObject ui;        
        [SerializeField] private GameObject currentRoom;
        [SerializeField] private GameObject backpack;

        [SerializeField] private ManagerBehaviour? manager;

        // Fields        
        private UIBehaviour uiBehaviour;

        // Properties        
        public ManagerBehaviour? Manager { get => manager; set => manager = value; }
        public string Name { get => name; set => name = value; }
        public string? Description { get => description; set => description = value; }

        // Methods

        // Begin MonoBehaviour
        public void Start()
        {
            //Debug.Log("Player behaviour script starts");

            // Set up UI
            uiBehaviour = ui.GetComponent<UIBehaviour>();
            uiBehaviour.InspectRoomEvent += InspectRoomEvent;
            uiBehaviour.TryMoveToRoomEvent += TryMoveToRoomEvent;
            uiBehaviour.QuitGameEvent += QuitRunEvent;
            uiBehaviour.HelpEvent += HelpTextEvent;
        }
        // End MonoBehaviour

        private void InspectRoomEvent(object sender, EventArgs e)
        {
            InspectRoom(currentRoom!.GetComponent<RoomBehaviour>());
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
            Manager!.QuitRun();
        }

        // Begin IPlayer
        public bool HasItem(IItem item)
        {
            return backpack.GetComponent<BackpackBehaviour>().Contains(item);
        }

        public void AddItem(IItem item)
        {
            this.backpack.GetComponent<BackpackBehaviour>().Add(item);
        }
        // End IPlayer

        public override string ToString()
        {
            return name;
        }

        private void TryMoveIntoNewRoom(CompassDirection direction)
        {
            // Pick up current room
            IRoom currentRoomBehaviour = currentRoom!.GetComponent<RoomBehaviour>();

            // Is there a door there?
            if (!currentRoomBehaviour.HasDoorInDirection(direction))
            {
                // No door in that direction
                // Return text to indicate that to player
                PrintInvalidDirectionText(direction);
            }
            else
            {
                // Is door in that direction
                IDoor selectedDoor = currentRoomBehaviour.GetDoorInDirection(direction)!;

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
                    IRoom newRoomBehaviour = selectedDoor.GetConnectingRoom(currentRoomBehaviour);

                    // Print text informing player that they are moving into new room
                    PrintMovingIntoNewRoomText(currentRoomBehaviour, newRoomBehaviour);

                    // Set new room as current room
                    currentRoom = newRoomBehaviour.GetGameObject();

                    // Describe new room
                    PrintRoomDescriptionText(newRoomBehaviour);

                    // TODO: Check for NPC in room

                    // Check whether this is the final room
                    if (newRoomBehaviour.IsFinalRoom)
                    {
                        WinGame();
                    }
                }
            }
        }

        private void WinGame()
        {
            // Won the game
            Manager!.WinGame();
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
                IItem? item = room.GetItemBehaviour();

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

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }

    public class MoveInDirectionEventArgs : EventArgs
    {
        public CompassDirection Direction { get; set; } 
    }
}