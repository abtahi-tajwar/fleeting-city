using Godot;
using System;

public partial class GameManager : Node
{
    public static Player Player { get; private set; }

    public static void SetPlayer(Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player), "Player cannot be null.");
        }

        Player = player;
        GD.Print("Player has been set in GameManager.");
    }

}
