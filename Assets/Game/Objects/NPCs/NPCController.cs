using Assets.Game.Control;
using Assets.Game.Objects.Players;
using Assets.Game.Objects.Rooms;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable enable

namespace Assets.Game.Objects.NPCs
{
    public class NPCController : MonoBehaviour, INPC
    {
        // Parameters
        [SerializeField] protected string description;
        [SerializeField] protected string greetingText;

        [SerializeField] protected string primaryLine;
        [SerializeField] protected string primaryRetort;
        [SerializeField] protected string secondaryRetort;

        [SerializeField] protected List<string> playerResponses;
        [SerializeField] protected string correctResponse;

        [SerializeField] protected string leaveHappyText;
        [SerializeField] protected string leaveUnhappyText;

        [SerializeField] private InputController ui;

        // Fields
        private bool metBefore = false;
        private bool correctResponseGiven;
        private IPlayer? player;
        private IRoom? room;

        // Properties
        public string Name => name;
        public string Description { get => description; set => description = value; }

        // Methods
        public virtual void Start()
        {
            ui.RespondToNPCEvent += RespondToNPCEvent;

            ui.Prompt = Prompt();
        }

        private void RespondToNPCEvent(object sender, RespondToNPCArgs e)
        {
            // Get response chosen
            string response = playerResponses[e.ResponseNum];
            ui.PrintText(response);

            RespondToNPC(response);
        }

        private void RespondToNPC(string response)
        {
            // Call retort
            string retort = Retort(response);
            ui.PrintText(retort);

            // Leave
            Leave(correctResponseGiven);
        }

        public string Meet()
        {
            string text = String.Empty;

            if (!metBefore)
            {
                // Add description if first time meeting them
                text = Describe() + "\n";
            }

            text += Greeting();

            metBefore = true;  // Have now met once before

            return text;
        }

        public string Describe()
        {
            return description;
        }

        public string Greeting()
        {
            // The NPC greets the player
            return greetingText;
        }

        public void StartConversation(IPlayer player, IRoom room)
        {
            // Hold a ref to the player and room we're all in
            this.player = player;
            this.room = room;

            // Enable our UI
            EnableUI();

            ui.PrintText(Talk());

            ui.PrintText(PlayerResponseOptions(), addPrompt: true);
        }

        public virtual string Talk()
        {
            return primaryLine;
        }

        public virtual string Retort(string response)
        {
            string text;

            if ((correctResponse != String.Empty) && (secondaryRetort != String.Empty) && response.Equals(correctResponse))
            {
                // Correct response given
                correctResponseGiven = true;

                text = secondaryRetort;
            }
            else if ((correctResponse != String.Empty) && (secondaryRetort != String.Empty) && !response.Equals(correctResponse))            
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

        public virtual List<string> GetPlayerResponses()
        {
            return playerResponses;
        }

        protected virtual string PlayerResponseOptions()
        {
            int i = 0;
            StringBuilder sb = new();            

            foreach(string response in GetPlayerResponses())
            {
                i++;

                sb.Append($"{i}: {response}. ");
            }

            string text = sb.ToString();

            return text;
        }

        public void Leave(bool happy)
        {
            string text;

            // Is he saying something nice or mean
            if (happy)
            {
                text = leaveHappyText;
            }
            else
            {
                text = leaveUnhappyText;

                // Turn player into a newt
                player!.TurnIntoNewt();

            }
            ui.PrintText(text);

            // Remove NPC from room
            room!.RemoveNPC(this);

            // Disable UI
            DisableUI();

            // Hand back to player
            player!.ConversationOver();            
        }        

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        // Begin IHasUI
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
        // End IHasUI
    }

    public class RespondToNPCArgs : EventArgs
    {
        public int ResponseNum { get; set; }
    }
}
