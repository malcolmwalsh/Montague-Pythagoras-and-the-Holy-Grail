#nullable enable
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Obstacles;
using Assets.Game.Objects.Players;
using Assets.Game.Objects.Rooms;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Game.Objects.Doors
{
    public class Door : IDoor
    {
        // Constructors
        public Door(string name, string description, IRoom roomA, IRoom roomB)
        {
            this.name = name;
            this.description = description;

            this.roomA = roomA;
            this.roomB = roomB;
        }

        public Door(string name, string description, IRoom roomA, IRoom roomB, IObstacle obstacle, string blockedText, string unblockText) : this(name, description, roomA, roomB)
        {
            obstacles.Add(obstacle);

            this.blockedText = blockedText;
            this.unblockText = unblockText;

            blocked = true;
        }

        // Fields
        private readonly string name;
        private readonly string description;

        private readonly ISet<IObstacle> obstacles = new HashSet<IObstacle>();
        private readonly IRoom roomA;
        private readonly IRoom roomB;
        private bool blocked;
        private string? blockedText;
        private string? unblockText;

        // Properties
        public string Name => name;
        public string Description => description;

        // Methods
        public bool ConnectsRoom(IRoom room)
        {
            return room.Equals(roomA) || room.Equals(roomB);
        }

        public bool IsBlocked()
        {
            return blocked;
        }

        public void Unblock()
        {
            blocked = false;
        }

        public bool TryTraverse(IPlayer player)
        {
            if (!HasObstacle())
            {
                // No obstacle, unlock if not already
                blocked = false;
            }

            if (!IsBlocked()) return true;  // Not locked

            // So we have an obstacle
            bool hasNotItem = false;
            foreach (IObstacle obstacle in obstacles)
            {
                // What unblocks this obstacle?
                IItem obstacleNemesisItem = obstacle.NemesisItem;

                // Does player have it?
                if (!player.HasItem(obstacleNemesisItem))
                {
                    // Does not have the item needed
                    hasNotItem = true;

                    // We're done here, can't traverse
                    break;
                }
            }

            return !hasNotItem;
        }

        public IRoom GetConnectingRoom(IRoom currentRoom)
        {
            return currentRoom.Equals(roomA) ? roomB : roomA;
        }

        private bool HasObstacle()
        {
            return obstacles.Any();
        }

        public string? GetBlockedText()
        {
            return blockedText;
        }

        public string? GetUnblockText()
        {
            return unblockText;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
