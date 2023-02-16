#nullable enable
using Assets.Game.Objects;
using System.Collections.Generic;
using static Assets.Game.Navigation.Enums;

internal class Player : IPlayer
{
    // Fields    
    private readonly string name;
    private readonly string description;

    private IRoom? currentRoom;

    private ISet<IItem> items = new HashSet<IItem>();

    // Constructors
    public Player(string name, string description, IRoom currentRoom)
    {
        this.name = name;
        this.description = description;
        this.currentRoom = currentRoom;
    }

    // Properties
    public string Name => name;
    public string Description => description;
    public IRoom? CurrentRoom { get => currentRoom; set => currentRoom = value; }

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
