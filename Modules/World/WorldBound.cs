using Godot;
using System;

public partial class WorldBound : Area2D
{
    public static Shape2D WorldShape { get; private set; }
    public override void _Ready()
    {
        // Connect to the body entered signal to handle collisions
        var shape = GetNode<CollisionShape2D>("BoundaryShape").Shape;
        WorldShape = shape;

        if (shape == null) throw new Exception("BoundaryShape does not have a valid shape assigned.");
    }
}
