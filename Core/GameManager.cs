using Godot;
using System;

public partial class GameManager : Node
{
    [Export]
    public PackedScene SettlerScene;
    [Export]
    public Node2D SettlersHolder;


    // Privates
    private SettlerManager _settlerManager;

    public override void _Ready()
    {
        var settlerManager = new SettlerManager();
        settlerManager.Init(SettlersHolder, SettlerScene);
        _settlerManager = settlerManager;
    }
}
