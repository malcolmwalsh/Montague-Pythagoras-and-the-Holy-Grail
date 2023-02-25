using Assets.Game.Objects.Items;
using UnityEngine;

namespace Assets.Game.Objects.Obstacles
{
    public class ObstacleController : MonoBehaviour, IObstacle
    {
        #region Private fields

        [SerializeField] private ItemController nemesis;
        [SerializeField] private string description;

        #endregion

        #region Properties

        public ItemController Nemesis => nemesis;

        public string Description
        {
            get => description;
            set => description = value;
        }

        #endregion

        #region IObject interface

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        #endregion
    }
}