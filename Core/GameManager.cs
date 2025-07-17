using Godot;
using System;

public partial class GameManager : Node
{
    [Export]
    public PackedScene SettlerScene;

    // Privates
    private SettlerManager _settlerManager;

    public override void _Ready()
    {
        _settlerManager = new SettlerManager(SettlerScene);
    }
}
