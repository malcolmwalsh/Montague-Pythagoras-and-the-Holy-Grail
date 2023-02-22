#nullable enable
using Assets.Game.Objects.Items;
using UnityEngine;

namespace Assets.Game.Objects.Obstacles
{
    internal class ObstacleBehaviour : MonoBehaviour, IObstacle
    {
        // Parameters
        [SerializeField] private string description;
        [SerializeField] private GameObject? nemesis;

        // Fields


        // Properties
        public IItem? NemesisBehaviour => nemesis?.GetComponent<ItemBehaviour>();
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }

        // Methods
        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
