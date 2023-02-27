using System;
using System.Collections.Generic;
using Assets.Game.Control;
using Assets.Game.Objects.Backpacks;
using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.NPCs;
using Assets.Game.Objects.Rooms;
using UnityEngine;
using static Assets.Game.Navigation.Enums;
using static UnityEditor.Progress;

namespace Assets.Game.Objects.Players
{
    internal class PlayerController : MonoBehaviour, IPlayer, IHasUI
    {
        #region Private fields

        [SerializeField] private BackpackController backpack;

        private bool isNewt;
        [SerializeField] private CompletionTracker completionTracker;
        [SerializeField] private GameManager manager;
        [SerializeField] private InputController ui;
        [SerializeField] private RoomController currentRoom;
        [SerializeField] private string description;

        #endregion

        #region Properties

        public string Description
        {
            get => description;
            set => description = value;
        }

        #endregion

        #region IBackpack interface

        public IList<ItemController> GetItems()
        {
            return backpack.GetItems();
        }

        public bool HasItem(ItemController item)
        {
            return backpack.HasItem(item);
        }

        public bool IsEmpty()
        {
            return backpack.IsEmpty();
        }

        public void RemoveItem(ItemController item)
        {
            this.backpack.RemoveItem(item);
        }

        public void AddItem(ItemController item)
        {
            this.backpack.AddItem(item);

            completionTracker.Register(item);
        }

        #endregion

        #region IHasUI interface

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

        public string Prompt()
        {
            // Get prop complete
            float percComplete = completionTracker.percentageCompleted();

            string text = $"Make your choice    [{percComplete:0.0}% complete]\n" +
                          $"[{string.Join(", ", KeyBindings.movementKeys)} to move; {KeyBindings.inspectKey} to inspect; " +
                          $"{KeyBindings.talkKey} to talk; {KeyBindings.helpKey} for help; {KeyBindings.quitKey} to quit]";

            return text;
        }

        #endregion

        #region IObject interface

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        #endregion

        #region IPlayer interface

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

                ui.PrintPrompt(this);
            }
        }

        public void TurnIntoNewt()
        {
            this.isNewt = true;
        }

        public void PrintIntroduction()
        {
            // Enable our UI as we're in charge now
            EnableUI();

            string text =
                $"You find yourself in a medium-sized closet, surrounded by various tins, jars, blankets, brooms, and a single pink cowboy hat.\n" +
                $"As nice as the closet is, you'd rather be outside, leaping from tree to tree as they float down the mighty rivers of British Columbia!";

            // We start in a room
            completionTracker.Register(currentRoom);

            ui.PrintTextAndPrompt(text, this);
        }

        #endregion

        #region Public methods

        public void Start()
        {
            //Debug.Log("Player behaviour script starts");

            // Set up UI
            ui.InspectRoomEvent += InspectRoomEvent;
            ui.TryMoveToRoomEvent += TryMoveToRoomEvent;
            ui.QuitGameEvent += QuitRunEvent;
            ui.HelpEvent += HelpTextEvent;
            ui.TalkToNPCEvent += TalkToNPCEvent;

            // Don't need it yet
            DisableUI();
        }

        public override string ToString()
        {
            return name;
        }

        #endregion

        #region Private methods

        private void InspectRoomEvent(object sender, EventArgs e)
        {
            InspectRoom(currentRoom);
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
            manager!.QuitRun();
        }

        private void TryMoveIntoNewRoom(CompassDirection direction)
        {
            ui.ClearLog();

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
                        ui.PrintText(selectedDoor.GetBlockedText());

                        // Print text that door is unlocked by using item
                        PrintUnblockingDoorText(selectedDoor);

                        // Unblock door
                        selectedDoor.Unblock();
                    }
                }

                if (selectedDoor.IsBlocked()) return; // Door is blocked

                // Door was either unblocked already or has been unblocked	

                // Get room that connects with the current room using this door	                                
                RoomController newRoom = selectedDoor.GetConnectingRoom(currentRoom);

                // Print text informing player that they are moving into new room
                PrintMovingIntoNewRoomText(currentRoom, newRoom);

                // Set new room as current room
                currentRoom = newRoom;

                // Register the room
                completionTracker.Register(newRoom);

                // has NPC?
                bool hasNPC = newRoom.HasNPC();

                // Describe new room
                PrintRoomDescriptionText(!hasNPC);

                // Check for NPC in room
                if (hasNPC)
                {
                    // Get the NPC
                    INpc npc = newRoom.NPC!;

                    // Meet the NPC
                    string meetText = npc.Meet();
                    ui.PrintTextAndPrompt(meetText, this);
                }

                // Check whether this is the final room
                if (newRoom.IsFinalRoom) WinGame();
            }
        }

        private void TalkToNPC()
        {
            ui.ClearLog();

            // Get the NPC
            INpc npc = currentRoom.NPC;

            if (npc != null)
            {
                // There's someone there

                // Disable our UI
                DisableUI();

                // Register we've met them
                completionTracker.Register(npc);

                // Start talking
                npc.StartConversation(this, currentRoom);
            }
            else
            {
                string text = "Talking to yourself won't get you back to chopping wood";
                ui.PrintTextAndPrompt(text, this);
            }
        }

        private void WinGame()
        {
            // Won the game
            manager!.WinGame();
        }

        private void LoseGame()
        {
            // Lost the game
            manager!.LoseGame(true);
        }

        private void PrintRoomDescriptionText(bool addPrompt = false)
        {
            string text = currentRoom.Description;

            if (addPrompt)
                ui.PrintTextAndPrompt(text, this);
            else
                ui.PrintText(text);
        }

        private void InspectRoom(IRoom room)
        {
            ui.ClearLog();

            // Describe items
            string text = $"You walk around the room slowly, pushing and prodding at things.\n";

            // Check for items in the room
            if (!room.IsEmpty())
            {
                // Get the items
                IList<ItemController> items = room.GetItems();

                foreach (ItemController item in items)
                {
                    if (item != null)
                    {
                        // Shouldn't be null as we checked above
                        text += $"You see a {item}. {item.Description}. You put it in your black lumberjack backpack";

                        // Player now has this item
                        this.AddItem(item);
                    }
                }

                // Remove all from room
                foreach (ItemController item in new List<ItemController>(items))
                {
                    room.RemoveItem(item);
                }
            }
            else
            {
                text += " In the end, there's nothing of interest.";
            }

            // Print item text
            ui.PrintText(text);

            // Describe NPC
            if (room.HasNPC())
            {
                INpc npc = room.NPC;

                string npcText = $"Watching your every move is {npc.Description}";
                ui.PrintText(npcText);
            }

            // Describe doors
            IDictionary<CompassDirection, IDoor> doors = room.GetDoors();
            foreach (KeyValuePair<CompassDirection, IDoor> entry in doors)
            {
                string doorText = $"To the {entry.Key}, {entry.Value.Description}";
                ui.PrintText(doorText);
            }

            // Final prompt
            ui.PrintPrompt(this);
        }

        private void PrintInvalidDirectionText(CompassDirection direction)
        {
            string text = $"No door in the direction selected ({direction})";

            ui.PrintTextAndPrompt(text, this);
        }

        private void PrintUnblockingDoorText(IDoor selectedDoor)
        {
            string text = selectedDoor.GetUnblockText();
            ui.PrintText(text);
        }

        private void PrintMovingIntoNewRoomText(IRoom oldRoom, IRoom newRoom)
        {
            string text = $"You open the door and pass from {oldRoom} into {newRoom}";
            ui.PrintText(text);
        }

        private void PrintCannotEnterDoorText(IDoor selectedDoor)
        {
            string text = $"{selectedDoor.GetBlockedText()}\n" +
                          $"You cannot go that way just now.";
            ui.PrintTextAndPrompt(text, this);
        }

        #endregion
    }

    public class MoveInDirectionEventArgs : EventArgs
    {
        #region Properties

        public CompassDirection Direction { get; set; }

        #endregion
    }
}