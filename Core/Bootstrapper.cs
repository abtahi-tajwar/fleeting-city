using FleetingCity.BAL.Provider;
using FleetingCity.BAL.Data;
using Godot;
using System;

public partial class Bootstrapper : Node
{
    public override void _Ready()
    {
        RootProvider.Logger = new GodotLogger();
        GameDataLoader.LoadAllGameData();
    }
}
