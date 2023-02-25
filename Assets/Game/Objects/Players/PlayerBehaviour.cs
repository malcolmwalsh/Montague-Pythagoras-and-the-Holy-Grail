using Assets.Game.Control;
using Assets.Game.Objects.Backpacks;
using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.NPCs;
using Assets.Game.Objects.Rooms;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static Assets.Game.Navigation.Enums;

#nullable enable
namespace Assets.Game.Objects.Players
{
    internal class PlayerBehaviour : MonoBehaviour, IPlayer, IHasUI
    {
        // Parameters
        [SerializeField] private string description;

        [SerializeField] private InputBehaviour ui;
        [SerializeField] private GameObject currentRoom;
        [SerializeField] private GameObject backpack;

        [SerializeField] private ManagerBehaviour? manager;

        // Fields                
        private bool isNewt;

        // Properties        
        public ManagerBehaviour? Manager { get => manager; set => manager = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }

        // Methods

        // Begin MonoBehaviour
        public void Start()
        {
            //Debug.Log("Player behaviour script starts");

            // Set up UI
            ui.InspectRoomEvent += InspectRoomEvent;
            ui.TryMoveToRoomEvent += TryMoveToRoomEvent;
            ui.QuitGameEvent += QuitRunEvent;
            ui.HelpEvent += HelpTextEvent;            
            ui.TalkToNPCEvent += TalkToNPCEvent;

            ui.Prompt = Prompt();

            // Don't need it yet
            DisableUI();
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
            ui.PrintHelpText();
        }

        private void TalkToNPCEvent(object sender, EventArgs e)
        {
            TalkToNPC();
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

                    // has NPC?
                    bool hasNPC = newRoomBehaviour.HasNPC();

                    // Describe new room
                    PrintRoomDescriptionText(newRoomBehaviour, !hasNPC);

                    // Check for NPC in room
                    if (hasNPC)
                    {
                        // Get the NPC
                        INPC npc = newRoomBehaviour.NPC!;

                        // Meet the NPC
                        string meetText = npc.Meet();
                        ui.PrintText(meetText, addPrompt: true);
                    }

                    // Check whether this is the final room
                    if (newRoomBehaviour.IsFinalRoom)
                    {
                        WinGame();
                    }
                }
            }
        }

        private void TalkToNPC()
        {
            // Get the NPC
            INPC? npc = currentRoom.GetComponent<RoomBehaviour>().NPC;

            if (npc != null)
            {
                // There's someone there

                // Disable our UI
                DisableUI();

                // Start talking
                npc.StartConversation(this, currentRoom.GetComponent<IRoom>());
            }
            else
            {
                string text = "Talking to yourself won't get you back to chopping wood";
                ui.PrintText(text);
            }
        }

        public void ConversationOver()
        {
            // Check if newt
            if (isNewt)
            {
                LoseGame();
            }
            else
            {
                // We're OK

                // Enable UI
                EnableUI();
            }
        }

        private void WinGame()
        {
            // Won the game
            Manager!.WinGame();
        }

        private void LoseGame()
        {
            // Lost the game
            Manager!.LoseGame(true);
        }

        private void PrintRoomDescriptionText(IRoom newRoom, bool addPrompt = false)
        {
            string text = newRoom.Description;

            ui.PrintText(text, addPrompt: addPrompt);
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

            ui.PrintText(text, addPrompt: true);
        }

        private void PrintInvalidDirectionText(CompassDirection direction)
        {
            string text = $"No door in the direction selected ({direction})";

            ui.PrintText(text, addPrompt: true);
        }

        private void PrintUnblockingDoorText(IDoor selectedDoor)
        {
            string text = selectedDoor.GetUnblockText();
            ui.PrintText(text);
        }

        private void PrintMovingIntoNewRoomText(IRoom currentRoom, IRoom newRoom)
        {
            ui.ClearLog();

            string text = $"You open the door and pass from {currentRoom} into {newRoom}";
            ui.PrintText(text);
        }

        private void PrintCannotEnterDoorText(IDoor selectedDoor)
        {
            string text = selectedDoor.GetBlockedText();
            ui.PrintText(text);
        }        

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void EnableUI()
        {
            // Enable our ui so we do detect key presses
            ui!.enabled = true;
        }

        public void DisableUI()
        {
            // Shut down our UI so we don't detect key presses
            ui!.enabled = false;
        }

        public void TurnIntoNewt()
        {
            this.isNewt = true;
        }

        public string Prompt()
        {
            string text = "Make your choice\n" +
                $"[{String.Join(", ", KeyBindings.movementKeys)} to move; {KeyBindings.inspectKey} to inspect; " +
                $"{KeyBindings.talkKey} to talk; {KeyBindings.helpKey} for help; {KeyBindings.quitKey} to quit]";

            return text;
        }

        public void PrintIntroduction()
        {
            // Enable our UI as we're in charge now
            EnableUI();

            string text = $"You find yourself in a medium-sized closet, surrounded by various tins, jars, blankets, brooms, and a single pink cowboy hat.\n" +
                $"As nice as the closet is, you'd rather be outside, leaping from tree to tree as they float down the mighty rivers of British Columbia!";

            ui.PrintText(text, addPrompt: true);
        }
    }

    public class MoveInDirectionEventArgs : EventArgs
    {
        public CompassDirection Direction { get; set; } 
    }
}