using Assets.Game.Objects.Obstacles;
using UnityEngine;

namespace Assets.Game.Objects.Items
{
    public class ItemController : MonoBehaviour, IItem
    {
        #region Private fields

        [SerializeField] private ObstacleController nemesis;
        [SerializeField] private string description;

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

        public ObstacleController Nemesis => nemesis;

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
    }
}