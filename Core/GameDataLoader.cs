using FleetingCity.BAL.Data;
using Godot;
using System;

public partial class GameDataLoader : Node
{
    public override void _Ready()
    {
        _ = new FoodResourceData();
    }
}
