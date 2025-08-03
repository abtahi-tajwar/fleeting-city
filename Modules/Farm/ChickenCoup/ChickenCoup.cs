using Godot;
using FleetingCity.BAL.Model;
using System;

public partial class ChickenCoup : AnimalFarm
{
    private ChickenCoupModel _model;
    public override void _Ready()
    {
        _model = new ChickenCoupModel();
    }
}
