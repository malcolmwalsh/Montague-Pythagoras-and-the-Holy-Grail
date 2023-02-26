using System.Collections.Generic;
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

        public bool Contains(ItemController item)
        {
            return items.Contains(item);
        }

        public void Add(ItemController item)
        {
            items.Add(item);
        }

        #endregion
    }
}