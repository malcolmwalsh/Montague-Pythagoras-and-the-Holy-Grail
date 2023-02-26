#region Imports

using System;
using System.Collections.Generic;
using System.Text;
using Assets.Game.Control;
using Assets.Game.Objects.Backpacks;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Players;
using Assets.Game.Objects.Rooms;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

#endregion

namespace Assets.Game.Objects.NPCs
{
    public class NpcController : MonoBehaviour, INpc
    {
        #region Protected fields
        [SerializeField] private BackpackController backpack;

        [SerializeField] protected string correctResponse;

        // Parameters
        [SerializeField] protected string description;
        [SerializeField] protected string greetingText;

        [SerializeField] protected string leaveHappyText;
        [SerializeField] protected string leaveUnhappyText;

        [SerializeField] protected List<string> playerResponses;

        [SerializeField] protected string primaryLine;
        [SerializeField] protected string primaryRetort;
        [SerializeField] protected string secondaryRetort;

        #endregion

        #region Private fields

        private bool correctResponseGiven;

        private bool metBefore;
        private IPlayer engagingPlayer;
        private IRoom currentRoom;

        [SerializeField] private InputController ui;

        #endregion

        #region Properties

        // Properties
        public string Description
        {
            get => description;
            set => description = value;
        }

        #endregion

        #region IHasUI

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
            string text = "Make your choice\n" +
                          $"[Press {KeyBindings.response0Key}, {KeyBindings.response1Key} or {KeyBindings.response2Key}]";

            return text;
        }

        #endregion

        #region INPC

        public string Meet()
        {
            string text = string.Empty;

            if (!metBefore)
                // Add description if first time meeting them
                text = Describe() + "\n";

            text += Greeting();

            metBefore = true; // Have now met once before

            return text;
        }

        public void StartConversation(IPlayer player, IRoom room)
        {
            // Hold a ref to the player and room we're all in
            this.engagingPlayer = player;
            this.currentRoom = room;

            // Enable our UI
            EnableUI();

            ui.PrintText(Talk());

            ui.PrintTextAndPrompt(PlayerResponseOptions(), this);
        }

        #endregion

        #region IObject

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        #endregion

        #region Public methods

        public virtual void Start()
        {
            ui.RespondToNPCEvent += RespondToNpcEvent;
        }

        #endregion

        #region Protected methods

        protected virtual string Talk()
        {
            return primaryLine;
        }

        protected virtual string Retort(string response)
        {
            string text;

            if ((correctResponse != string.Empty) && (secondaryRetort != string.Empty) &&
                response.Equals(correctResponse))
            {
                // Correct response given
                correctResponseGiven = true;

                text = secondaryRetort;
            }
            else if ((correctResponse != string.Empty) && (secondaryRetort != string.Empty) &&
                     !response.Equals(correctResponse))
            {
                // Wrong response given
                correctResponseGiven = false;

                text = primaryRetort;
            }
            else
            {
                // No correct option, always happy
                correctResponseGiven = true;

                text = primaryRetort;
            }

            return text;
        }

        protected virtual IList<string> GetPlayerResponses()
        {
            return playerResponses;
        }

        protected virtual string PlayerResponseOptions()
        {
            int i = 0;
            StringBuilder sb = new();

            foreach (string response in GetPlayerResponses())
            {
                i++;

                sb.Append($"{i}: {response}. ");
            }

            string text = sb.ToString();

            return text;
        }

        protected void Leave(bool happy)
        {
            string text;

            // Is he saying something nice or mean
            if (happy)
            {
                text = leaveHappyText;

                // Drop item
                DropItem();
            }
            else
            {
                text = leaveUnhappyText;

                // Turn player into a newt
                engagingPlayer!.TurnIntoNewt();
            }

            ui.PrintText(text);

            // Remove NPC from room
            currentRoom!.RemoveNPC(this);

            // Disable UI
            DisableUI();

            // Hand back to player
            engagingPlayer!.ConversationOver();
        }

        private void DropItem()
        {
            if (IsEmpty()) return;  // Nothing to do

            // Have items
            IList<ItemController> items = backpack.GetItems();

            foreach (ItemController item in items)
            {
                currentRoom.AddItem(item);
            }
        }

        #endregion

        #region Private methods

        private void RespondToNpcEvent(object sender, RespondToNpcArgs e)
        {
            // Get response chosen
            string response = playerResponses[e.ResponseNum];
            ui.PrintText(response);

            RespondToNpc(response);
        }

        private void RespondToNpc(string response)
        {
            // Call retort
            string retort = Retort(response);
            ui.PrintText(retort);

            // Leave
            Leave(correctResponseGiven);
        }

        private string Describe()
        {
            return description;
        }

        private string Greeting()
        {
            // The NPC greets the player
            return greetingText;
        }

        #endregion

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

        public void AddItem(ItemController item)
        {
            backpack.AddItem(item);
            
        }
    }

    public class RespondToNpcArgs : EventArgs
    {
        #region Properties

        public int ResponseNum { get; set; }

        #endregion
    }
}