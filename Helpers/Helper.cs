using System;
using FleetingCity.BAL.Enum;
using Godot;

public class Helper
{
    public static MOVEMENT_DIRECTION_ENUM GetMovementDirection(Vector2 from, Vector2 to)
    {
        Vector2 delta = to - from;

        if (delta == Vector2.Zero)
            return MOVEMENT_DIRECTION_ENUM.NONE;

        float absX = Math.Abs(delta.X);
        float absY = Math.Abs(delta.Y);

        // Determine direction based on both axes
        if (absX > 0 && absY > 0)
        {
            if (delta.X > 0 && delta.Y < 0)
                return MOVEMENT_DIRECTION_ENUM.UP_RIGHT;
            if (delta.X < 0 && delta.Y < 0)
                return MOVEMENT_DIRECTION_ENUM.UP_LEFT;
            if (delta.X > 0 && delta.Y > 0)
                return MOVEMENT_DIRECTION_ENUM.DOWN_RIGHT;
            if (delta.X < 0 && delta.Y > 0)
                return MOVEMENT_DIRECTION_ENUM.DOWN_LEFT;
        }
        else if (absX > 0)
        {
            return delta.X > 0 ? MOVEMENT_DIRECTION_ENUM.RIGHT : MOVEMENT_DIRECTION_ENUM.LEFT;
        }
        else if (absY > 0)
        {
            return delta.Y > 0 ? MOVEMENT_DIRECTION_ENUM.DOWN : MOVEMENT_DIRECTION_ENUM.UP;
        }

        return MOVEMENT_DIRECTION_ENUM.NONE; // Fallback (shouldn't hit)
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

    public static Rect2 GetCollisionShapeBounds(CollisionShape2D shapeNode)
    {
        var shape = shapeNode.Shape;
        var transform = shapeNode.GlobalTransform;

        if (shape is RectangleShape2D rect)
        {
            Vector2 extents = rect.Size / 2f;
            Vector2 topLeft = transform.Origin - extents * shapeNode.GlobalScale;
            Vector2 size = rect.Size * shapeNode.GlobalScale;

            return new Rect2(topLeft, size);
        }
        else if (shape is CircleShape2D circle)
        {
            float radius = circle.Radius * shapeNode.GlobalScale.X;
            Vector2 topLeft = shapeNode.GlobalPosition - new Vector2(radius, radius);
            return new Rect2(topLeft, new Vector2(radius * 2, radius * 2));
        }
        else
        {
            GD.Print("Unhandled shape: " + shape);
        }

        return new Rect2();
    }

}