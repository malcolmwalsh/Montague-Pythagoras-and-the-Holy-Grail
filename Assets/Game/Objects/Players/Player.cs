using Assets.Game.Navigation;
using Assets.Game.Objects;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
    // Fields
    private ISet<IItem> items;    

    // Methods

    // Begin MonoBehaviour
    void Start()
    {
        
    }
    void Update()
    {

    }
    // End MonoBenhaviour

    // Begin IPlayer
    void IPlayer.TryMove(Enums.CompassDirection direction)
    {
        throw new System.NotImplementedException();
    }

    public void InspectRoom()
    {
        throw new System.NotImplementedException();
    }

    bool IPlayer.HasItem(IItem item)
    {
        return items.Contains(item);
    }
    // End IPlayer
   
}
