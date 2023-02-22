using Assets.Game.Objects.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Objects.Backpacks
{
    public class BackpackBehaviour : MonoBehaviour, IBackpack
    {
        // Constructors

        // Fields
        private ISet<IItem> items = new HashSet<IItem>();

        // Methods
        public bool Contains(IItem item)
        {
            return items.Contains(item);
        }

        public void Add(IItem item)
        {
            items.Add(item);
        }
    }
}
