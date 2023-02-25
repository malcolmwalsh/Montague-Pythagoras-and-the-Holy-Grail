using UnityEngine;

#nullable enable

namespace Assets.Game.Objects
{
    public interface IObject
    {
        // Properties
        string Description { get; set; }

        // Methods
        GameObject GetGameObject();
    }
}
