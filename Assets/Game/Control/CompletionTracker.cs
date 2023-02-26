using System.Collections.Generic;
using Assets.Game.Objects.Items;
using Assets.Game.Objects.NPCs;
using Assets.Game.Objects.Rooms;
using UnityEngine;

namespace Assets.Game.Control
{
    public class CompletionTracker : MonoBehaviour
    {
        #region Private fields

        private readonly ISet<IItem> itemsObtained = new HashSet<IItem>();
        private readonly ISet<INpc> npcsMet = new HashSet<INpc>();
        private readonly ISet<IRoom> roomsVisited = new HashSet<IRoom>();
        
        private ItemController[] allItems;
        private NpcController[] allNPCs;
        private RoomController[] allRooms;

        #endregion

        #region Public methods

        public void Awake()
        {
            allRooms = FindObjectsOfType<RoomController>();
            allNPCs = FindObjectsOfType<NpcController>();
            allItems = FindObjectsOfType<ItemController>();
        }


        public void Register(IRoom room)
        {
            roomsVisited.Add(room);
        }

        public void Register(INpc npc)
        {
            npcsMet.Add(npc);
        }

        public void Register(IItem item)
        {
            itemsObtained.Add(item);
        }

        public float proportionCompleted()
        {
            int numerator = itemsObtained.Count + roomsVisited.Count + npcsMet.Count;
            int denominator = allItems.Length + allRooms.Length + allNPCs.Length;

            float propComplete = (float)numerator / denominator;

            return propComplete;
        }

        #endregion
    }
}