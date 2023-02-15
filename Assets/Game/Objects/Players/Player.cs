using Assets.Game.Objects;
using System.Collections.Generic;
using static Assets.Game.Navigation.Enums;

internal class Player : IPlayer
{
    // Fields    
    private readonly string name;
    private readonly string description;

    private ISet<IItem> items;

    // Constructors
    public Player(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    // Properties
    public string Name => name;
    public string Description => description;

    // Methods

    // Begin IPlayer
    public void TryMove(CompassDirection direction)
    {
        // TODO
        throw new System.NotImplementedException();
    }

    public void InspectRoom()
    {
        // TODO
        throw new System.NotImplementedException();
    }

    public bool HasItem(IItem item)
    {
        return items.Contains(item);
    }
    // End IPlayer
   
}
