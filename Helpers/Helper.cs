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
}