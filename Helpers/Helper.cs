using System;
using FleetingCity.Enums;
using Godot;

public class Helper
{
    public static MOVEMENT_DIRECTION_ENUM GetMovementDirection(Vector2 from, Vector2 to)
    {
        Vector2 delta = to - from;

        if (delta == Vector2.Zero)
            return MOVEMENT_DIRECTION_ENUM.NONE; // No movement

        float absX = Math.Abs(delta.X);
        float absY = Math.Abs(delta.Y);

        if (absX > absY)
            return delta.X > 0 ? MOVEMENT_DIRECTION_ENUM.RIGHT : MOVEMENT_DIRECTION_ENUM.LEFT;
        else if (absY > absX)
            return delta.Y > 0 ? MOVEMENT_DIRECTION_ENUM.DOWN : MOVEMENT_DIRECTION_ENUM.UP;
        else // Perfect 45 degree
            return delta.X > 0 ? MOVEMENT_DIRECTION_ENUM.RIGHT : MOVEMENT_DIRECTION_ENUM.LEFT;

    }
    public static bool IsMouseOverCollider(Node2D node, Vector2 mousePosition)
    {
        var space = node.GetWorld2D().DirectSpaceState;
        var shape = node.GetNode<CollisionShape2D>("CollisionShape2D").Shape;
        var query = new PhysicsPointQueryParameters2D
        {
            Position = mousePosition,
            CollideWithAreas = true,
            CollideWithBodies = true
        };

        var results = space.IntersectPoint(query);
        foreach (var result in results)
        {
            if (result.TryGetValue("collider", out var obj) && obj.As<Node2D>() == node)
                return true;
        }

        return false;

    }
}