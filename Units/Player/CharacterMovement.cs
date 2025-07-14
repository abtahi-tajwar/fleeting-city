using Godot;
using System;

public partial class CharacterMovement : CharacterBody2D
{
    [Export]
    public TileMapLayer Ground { get; set; }
    private AStarGrid2D _grid;

    public override void _Ready()
    {
        _grid = new AStarGrid2D();
        Rect2I groundCoords = Ground.GetUsedRect();
        _grid.Region = groundCoords;
        _grid.CellSize = new Vector2I(Ground.RenderingQuadrantSize, Ground.RenderingQuadrantSize);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.IsPressed())
        {
            Vector2 mousePosition = mouseButtonEvent.Position;
            Vector2I mouseGridPos= Ground.LocalToMap(mousePosition);
            Vector2I globalMousePos = Ground.LocalToMap(GetGlobalMousePosition());
            Vector2 playerPos = Ground.LocalToMap(GlobalPosition);
            GD.Print("Player Position: ", playerPos );
            GD.Print("Global Mouse Position: ", globalMousePos);
            GD.Print("Mouse Position: ", mouseGridPos.X/16, ", ", mouseGridPos.Y/16);
            
        }

    }
}
