using FleetingCity.BAL.Dto;
using FleetingCity.BAL.Model;
using Godot;
using System;

public partial class PlayerInventory : Node2D
{
    private static PlayerInventory _instance;

    public static PlayerInventory Instance
    {
        get
        {
            if (_instance == null)
                GD.PushError("PlayerInventory singleton not initialized yet!");
            return _instance;
        }
    }

    public InventoryModel InventoryModel { get; private set; }

    public override void _EnterTree()
    {
        if (_instance != null && _instance != this)
        {
            GD.PushWarning("Another PlayerInventory instance tried to load. Destroying...");
            QueueFree(); // or do nothing, depending on your needs
            return;
        }

        _instance = this;
        InventoryModel = new InventoryModel();
    }

    public void AddResource(string resourceId, int amount)
    {
        InventoryModel.AddResource(
            new InventoryModelAddResourceDto(resourceId, amount)
        );
    }

    public override void _ExitTree()
    {
        if (_instance == this)
            _instance = null;
    }
}
