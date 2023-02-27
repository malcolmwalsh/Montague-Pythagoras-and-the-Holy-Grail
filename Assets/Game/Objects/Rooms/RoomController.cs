using System;
using System.Collections.Generic;
using System.Text;
using Assets.Game.Objects.Backpacks;
using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.NPCs;
using UnityEngine;
using static Assets.Game.Navigation.Enums;

namespace Assets.Game.Objects.Rooms
{
    public class RoomController : MonoBehaviour, IRoom
    {
        #region Private fields

        [SerializeField] private BackpackController backpack;

        [SerializeField] private DoorController eastDoor;
        [SerializeField] private DoorController northDoor;
        [SerializeField] private DoorController southDoor;
        [SerializeField] private DoorController westDoor;

        private readonly IDictionary<CompassDirection, IDoor> doors = new Dictionary<CompassDirection, IDoor>();

        [SerializeField] private string description;

        [SerializeField] private bool isFinalRoom;
        [SerializeField] private bool isStartRoom;

        [SerializeField] private NpcController npc;

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

        public bool IsStartRoom
        {
            get => isStartRoom;
            set => isStartRoom = value;
        }

        public bool IsFinalRoom
        {
            get => isFinalRoom;
            set => isFinalRoom = value;
        }

        public INpc NPC => npc;

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

        public void AddItem(ItemController item)
        {
            backpack.AddItem(item);
        }

        public void RemoveItem(ItemController item)
        {
            backpack.RemoveItem(item);
        }

        #endregion

        #region IObject interface

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        #endregion

        #region IRoom interface

        public bool HasDoorInDirection(CompassDirection direction)
        {
            IDoor door = GetDoorInDirection(direction);

            return door is not null;
        }

        public IDoor GetDoorInDirection(CompassDirection direction)
        {
            doors.TryGetValue(direction, out IDoor door);

            return door;
        }

        public int NumDoors()
        {
            return doors.Count;
        }

        public string DoorLocationText()
        {
            StringBuilder sb = new();

            foreach (KeyValuePair<CompassDirection, IDoor> entry in doors)
            {
                if (entry.Value is null) continue;

                string text = $"There is a door to the {entry.Key}. ";
                sb.Append(text);
            }

            return sb.ToString();
        }

        public bool HasNPC()
        {
            return npc != null;
        }

        public void RemoveNPC(INpc npc)
        {
            if (!npc.Equals(NPC)) throw new Exception("The NPC is not in the room as expected");

            this.npc = null;
        }

        public IDictionary<CompassDirection, IDoor> GetDoors()
        {
            return doors;
        }

        #endregion

        #region Public methods

        public void Start()
        {
            // Set up doors dictionary
            if (northDoor != null) doors.Add(CompassDirection.North, northDoor);
            if (eastDoor != null) doors.Add(CompassDirection.East, eastDoor);
            if (southDoor != null) doors.Add(CompassDirection.South, southDoor);
            if (westDoor != null) doors.Add(CompassDirection.West, westDoor);
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}