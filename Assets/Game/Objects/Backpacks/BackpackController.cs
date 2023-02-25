using System.Collections.Generic;
using Assets.Game.Objects.Items;
using UnityEngine;

namespace Assets.Game.Objects.Backpacks
{
    public class BackpackController : MonoBehaviour, IBackpack
    {
        #region Private fields

        private readonly ISet<IItem> items = new HashSet<IItem>();

        #endregion

        #region IBackpack interface

        public bool Contains(IItem item)
        {
            return items.Contains(item);
        }

        public void Add(IItem item)
        {
            items.Add(item);
        }

        #endregion
    }
}