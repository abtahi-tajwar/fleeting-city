using Godot;
using FleetingCity.BAL.Model;
using System;

public partial class ChickenCoup : AnimalFarm
{
    private ChickenCoupModel _model;
    public override void _Ready()
    {
        _model = new ChickenCoupModel();
        foreach (var item in _model.ProducedResource)
        {
            GD.Print($"Produced item", item.Resource.Name);
        }
    }
}
