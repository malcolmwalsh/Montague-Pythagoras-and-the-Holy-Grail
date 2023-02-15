using Assets.Game.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Assembly_CSharp
{
    internal class Door : IDoor
    {
        // Fields
        private ISet<IObstacle> obstacles;
        private IRoom roomA;
        private IRoom roomB;
        private bool locked;

        // Constructors


        // Methods
        public bool IsLocked()
        {
            return locked;
        }

        public bool TryTraverse(IPlayer player)
        {
            if (!HasObstacle())
            {
                // No obstacle, unlock if not already
                locked = false;
            }

            if (!IsLocked()) return true;  // Not locked

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
    }
}
