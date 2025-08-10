using Godot;
using System;

public partial class GameManager : Node2D
{
    public static Player Player { get; private set; }
    public static bool IsPlatformMobile { get; set; }

    [Export]
    public bool DebugForMobile { get; set; }

    public override void _Ready()
    {
        if (OS.HasFeature("editor"))
        {
            IsPlatformMobile = DebugForMobile;
        }
        else
        {
            IsPlatformMobile = OS.HasFeature("mobile");
        }
    }

    public static void SetPlayer(Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player), "Player cannot be null.");
        }

        Player = player;
        GD.Print("Player has been set in GameManager.");
    }

    public override void _Input(InputEvent e)
    {
        // Drop all mouse input on the floor
        if (e is InputEventMouseButton || e is InputEventMouseMotion)
        {
            if (GameManager.IsPlatformMobile)
            {
                GetViewport().SetInputAsHandled(); // stop propagation
                return;
            }
        }
    }


}
