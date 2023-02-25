using System.Collections.Generic;
using System.Linq;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.Obstacles;
using Assets.Game.Objects.Players;
using Assets.Game.Objects.Rooms;
using UnityEngine;

namespace Assets.Game.Objects.Doors
{
    public class DoorController : MonoBehaviour, IDoor
    {
        #region Private fields

        private bool blocked;
        [SerializeField] private List<ObstacleController> obstacles;
        [SerializeField] private RoomController roomA;
        [SerializeField] private RoomController roomB;

        [SerializeField] private string description;
        [SerializeField] private string blockedText;
        [SerializeField] private string unblockText;

        #endregion

        #region Properties

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        #endregion

        #region IDoor interface

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
                // No obstacle, unlock if not already
                blocked = false;

            if (!IsBlocked()) return true; // Not locked

            // So we have an obstacle
            bool hasNotItem = false;
            foreach (ObstacleController obstacle in obstacles)
            {
                // What unblocks this obstacle?
                IItem obstacleNemesisItem = obstacle.Nemesis!;

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

        public RoomController GetConnectingRoom(IRoom currentRoom)
        {
            return currentRoom.Equals(roomA.GetComponent<RoomController>())
                ? roomB.GetComponent<RoomController>()
                : roomA.GetComponent<RoomController>();
        }

        public string GetBlockedText()
        {
            return blockedText;
        }

        public string GetUnblockText()
        {
            return unblockText;
        }

        #endregion

        #region IObject interface

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Private methods

        private bool HasObstacle()
        {
            return obstacles.Any();
        }

        #endregion
    }
}