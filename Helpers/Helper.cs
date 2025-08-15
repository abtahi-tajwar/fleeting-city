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
    public static bool IsTouchOverCollider(Node2D node, Vector2 mousePosition, float FINGER_RADIUS_PX = 18f)
    {
        var selection = node.GetNodeOrNull<Area2D>("SelectionArea");
        var space = node.GetWorld2D().DirectSpaceState;

        // Keep a roughly constant screen-space radius even if the camera is zoomed.
        float radius = FINGER_RADIUS_PX;
        var cam = node.GetViewport()?.GetCamera2D();
        if (cam != null)
        {
            // If non-uniform zoom, average the axes.
            float zoom = (cam.Zoom.X + cam.Zoom.Y) * 0.5f;
            if (zoom != 0f) radius *= 1f / zoom;
        }

        // Use a small circle shape at the pointer to test overlap with the node.
        var circle = new CircleShape2D { Radius = radius };

        var shapeParams = new PhysicsShapeQueryParameters2D
        {
            Shape = circle,
            Transform = new Transform2D(0f, mousePosition), // position in world coords
            CollideWithAreas = true,
            CollideWithBodies = true
            // You can also set CollisionMask here if you want to limit layers.
        };

        var results = space.IntersectShape(shapeParams); // Godot.Collections.Array<Dictionary>
        foreach (Godot.Collections.Dictionary hit in results)
        {
            var area = hit["collider"].As<Area2D>();
            if (area != null && area == selection)
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

    public static Rect2 GetBounds(Node node)
    {
        switch (node)
        {
            case CollisionShape2D collisionShape:
                if (collisionShape.Shape is RectangleShape2D rect)
                {
                    Vector2 size = rect.Size * collisionShape.Scale;
                    // Local top-left in shape space
                    // Local top-left in shape space
                    Vector2 localTopLeft = -size / 2;

                    // Transform to global coordinates
                    Vector2 globalTopLeft = collisionShape.GlobalTransform * localTopLeft;

                    return new Rect2(globalTopLeft, size);

                }
                else if (collisionShape.Shape is CircleShape2D circle)
                {
                    float radius = circle.Radius * collisionShape.Scale.X;
                    Vector2 localTopLeft = new Vector2(-radius, -radius);
                    Vector2 globalTopLeft = collisionShape.GlobalTransform * localTopLeft;

                    return new Rect2(globalTopLeft, new Vector2(radius * 2, radius * 2));
                }
                break;

            case Sprite2D sprite:
                if (sprite.Texture != null)
                {
                    Vector2 size = sprite.Texture.GetSize() * sprite.Scale;
                    Vector2 position = sprite.GlobalPosition - (size / 2);
                    return new Rect2(position, size);
                }
                break;

            case TextureRect textureRect:
                Vector2 texPosition = textureRect.GlobalPosition;
                Vector2 texSize = textureRect.Size * textureRect.Scale;
                return new Rect2(texPosition, texSize);

            case CollisionPolygon2D poly:
                if (poly.Polygon != null && poly.Polygon.Length > 0)
                {
                    Rect2 localBounds = CalculatePolygonBounds(poly.Polygon);
                    return new Rect2(poly.GlobalPosition + localBounds.Position, localBounds.Size * poly.Scale);
                }
                break;

            case Area2D area:
                Rect2 combined = new Rect2();
                foreach (Node child in area.GetChildren())
                {
                    Rect2 childBounds = GetBounds(child);
                    if (childBounds.HasArea())
                        combined = combined.HasArea() ? combined.Merge(childBounds) : childBounds;
                }
                return combined;
        }

        return new Rect2();
    }

    private static Rect2 CalculatePolygonBounds(Vector2[] points)
    {
        if (points == null || points.Length == 0)
            return new Rect2();

        Vector2 min = points[0];
        Vector2 max = points[0];

        foreach (var p in points)
        {
            min.X = Mathf.Min(min.X, p.X);
            min.Y = Mathf.Min(min.Y, p.Y);
            max.X = Mathf.Max(max.X, p.X);
            max.Y = Mathf.Max(max.Y, p.Y);
        }

        return new Rect2(min, max - min);
    }

    public static Vector2 CalculateGlobalPointerPosition(Node2D context, Vector2 touchPos)
    {

        return context.GetCanvasTransform().AffineInverse() * touchPos;
    }

}