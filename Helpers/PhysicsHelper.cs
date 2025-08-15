using Godot;
using System;

public partial class PhysicsHelper
{
    // POINT ∈ RECT (rect = CollisionShape2D with RectangleShape2D)
    public static bool PointOverlapsCollisionRect(Vector2 inputMousePosition, CollisionShape2D rectNode)
    {
        if (rectNode?.Shape is not RectangleShape2D rs) return false;

        // World → rect local
        var pLocal = rectNode.GlobalTransform.AffineInverse() * inputMousePosition;

        // Rect is centered at (0,0) in local space
        var half = rs.Size / 2f;

        return MathF.Abs(pLocal.X) <= half.X &&
               MathF.Abs(pLocal.Y) <= half.Y;

    }

    // CIRCLE ⟳ vs RECT ▭  (center/radius in WORLD units)
    public static bool CircleOverlapsCollisionRect(Vector2 centerWorld, float radiusWorld, CollisionShape2D rectNode)
    {
        if (rectNode?.Shape is not RectangleShape2D rs) return false;

        // World -> rect local
        var cLocal = rectNode.GlobalTransform.AffineInverse() * centerWorld;

        // Axis-aligned rect in local, centered at (0,0)
        var half = rs.Size / 2f;

        // Closest point on/in rect to circle center (in local)
        var qLocal = new Vector2(
            Mathf.Clamp(cLocal.X, -half.X, half.X),
            Mathf.Clamp(cLocal.Y, -half.Y, half.Y)
        );

        var dLocal = cLocal - qLocal;

        // Convert that local delta back to WORLD-length by applying rect's scale
        var gt = rectNode.GlobalTransform;              // columns are scaled/rotated axes
        float sx = gt.X.Length();                       // scale along local X
        float sy = gt.Y.Length();                       // scale along local Y
        float distSqWorld = (dLocal.X * sx) * (dLocal.X * sx) + (dLocal.Y * sy) * (dLocal.Y * sy);

        return distSqWorld <= radiusWorld * radiusWorld;
    }

}
