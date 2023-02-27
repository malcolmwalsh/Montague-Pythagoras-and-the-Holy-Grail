using System.Collections.Generic;
using System.Linq;
using Assets.Game.Objects.Items;
using UnityEngine;

namespace Assets.Game.Objects.Backpacks
{
    public class BackpackController : MonoBehaviour, IBackpack
    {
        #region Private fields

        [SerializeField] private List<ItemController> items;

        #endregion

        #region IBackpack interface

        public void AddItem(ItemController item)
        {
            items.Add(item);
        }

        public void RemoveItem(ItemController item)
        {
            items.Remove(item);
        }

        public bool IsEmpty()
        {
            return !items.Any();
        }

        public IList<ItemController> GetItems()
        {
            return items;
        }

        public bool HasItem(ItemController item)
        {
            return items.Contains(item);
        }

        #endregion
    }
}