#nullable enable
using Assets.Game.Objects.Items;
using UnityEngine;

namespace Assets.Game.Objects.Obstacles
{
    public class ObstacleBehaviour : MonoBehaviour, IObstacle
    {
        // Parameters
        [SerializeField] private string description;
        [SerializeField] private ItemBehaviour? nemesis;

        // Fields

        // Properties
        public ItemBehaviour? Nemesis => nemesis;
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }

        // Methods
        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
