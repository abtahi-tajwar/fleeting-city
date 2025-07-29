using Godot;
using System;

public partial class AnimalFarm : Node
{
    [Export]
    public Interactable Interactable;


    public override void _Ready()
    {
        if (Interactable != null)
        {
            Interactable.InteractionStart += OnInteractionStart;
            Interactable.InteractionStart += OnInteractionEnd;
        }
        else
        {
            GD.PrintErr("Interactable node is null in ", this.Name);
        }
    }

    public void OnInteractionStart(CharacterBody2D character)
    {
        if (character is Player player)
        {
            GD.Print("Player started interaction with AnimalFarm: " + player.Name);
            // Handle player interaction logic here
        }
        else if (character is Settler settler)
        {
            GD.Print("Settler started interaction with AnimalFarm: " + settler.Name);
            // Handle settler interaction logic here
        }
        else
        {
            GD.PrintS("Unknown character type interacting with AnimalFarm: " + character.Name);
        }
    }
    public void OnInteractionEnd(CharacterBody2D character)
    {
        
    }
}
