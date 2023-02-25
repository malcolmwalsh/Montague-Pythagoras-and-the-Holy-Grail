using Assets.Game.Objects.Items;
using UnityEngine;

#nullable enable

namespace Assets.Game.Objects.Obstacles
{
    public class ObstacleController : MonoBehaviour, IObstacle
    {
        // Parameters
        [SerializeField] private string description;
        [SerializeField] private ItemController? nemesis;

        // Fields

        // Properties
        public ItemController? Nemesis => nemesis;
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }

        // Methods
        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
