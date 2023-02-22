using Assets.Game.Objects.Doors;
using Assets.Game.Objects.Items;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static Assets.Game.Navigation.Enums;

#nullable enable
namespace Assets.Game.Objects.Rooms
{
    public class RoomBehaviour : MonoBehaviour, IRoom
    {
        // Parameters
        [SerializeField] private string description;
        
        [SerializeField] private GameObject? northDoor;
        [SerializeField] private GameObject? eastDoor;        
        [SerializeField] private GameObject? southDoor;
        [SerializeField] private GameObject? westDoor;

        [SerializeField] private bool isStartRoom = false;
        [SerializeField] private bool isFinalRoom = false;

        [SerializeField] private List<GameObject> items;

        // Fields        
        private IDictionary<CompassDirection, IDoor?> behavDoors = new Dictionary<CompassDirection, IDoor?>();

        // Properties
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public bool IsStartRoom { get => isStartRoom; set => isStartRoom = value; }
        public bool IsFinalRoom { get => isFinalRoom; set => isFinalRoom = value; }

        // MonoBehaviour
        public void Start()
        {
            // Set up doors dictionary
            if (northDoor != null) behavDoors.Add(CompassDirection.North, northDoor.GetComponent<DoorBehaviour>());
            if (eastDoor != null) behavDoors.Add(CompassDirection.East, eastDoor.GetComponent<DoorBehaviour>());
            if (southDoor != null) behavDoors.Add(CompassDirection.South, southDoor.GetComponent<DoorBehaviour>());
            if (westDoor != null) behavDoors.Add(CompassDirection.West, westDoor.GetComponent<DoorBehaviour>());
        }

        // Methods
        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public bool HasItem()
        {
            return items.Any();
        }

        public IItem? GetItemBehaviour()
        {
            IItem? item = items.FirstOrDefault()?.GetComponent<ItemBehaviour>();

            // Items have been taken
            items.Clear();

            return item;
        }

        public bool HasDoorInDirection(CompassDirection direction)
        {
            IDoor? door = GetDoorInDirection(direction);

            return door is not null;
        }

        public IDoor? GetDoorInDirection(CompassDirection direction)
        {
            behavDoors.TryGetValue(direction, out IDoor? door);

            return door;
        }

        public override string ToString()
        {
            return Name;
        }

        public int NumDoors()
        {
            return behavDoors.Count;
        }

        public string DoorLocationText()
        {
            StringBuilder sb = new();

            foreach (KeyValuePair<CompassDirection, IDoor?> entry in behavDoors)
            {
                if (entry.Value is not null)
                {
                    string text = $"There is a door to the {entry.Key}. ";

                    sb.Append(text);
                }
            }

            return sb.ToString();
        }
    }
}
