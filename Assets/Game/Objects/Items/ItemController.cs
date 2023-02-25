using Assets.Game.Objects.Obstacles;
using UnityEngine;

#nullable enable

namespace Assets.Game.Objects.Items
{
    public class ItemController : MonoBehaviour, IItem
    {
        // Parameters
        [SerializeField] private string description;
        [SerializeField] private ObstacleController nemesis;

        // Fields        
        
        // Properties
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public ObstacleController Nemisis => nemesis;

        // Methods
        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
