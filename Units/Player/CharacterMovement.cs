using Godot;
using System;

public partial class CharacterMovement : CharacterBody2D
{
    [Export]
    public TileMapLayer Ground { get; set; }
    private AStarGrid2D _grid;
    private Godot.Collections.Array<Vector2I> _idPath;
    private Vector2I _targetPosition;

    public override void _Ready()
    {
        _grid = new AStarGrid2D();
        Rect2I groundCoords = Ground.GetUsedRect();
        _grid.Region = groundCoords;
        _grid.CellSize = new Vector2I(Ground.RenderingQuadrantSize, Ground.RenderingQuadrantSize);
        _grid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Always;
        _grid.Update();
    }
    public override void _PhysicsProcess(double delta)
    {
        if (_idPath != null)
        {
            if (_idPath.Count == 0)
            {
                _idPath = null; // Clear the path when done
                return;
            }

            Vector2I nextPosition = _idPath[0];
            Vector2 targetPosition = Ground.MapToLocal(nextPosition);
            GlobalPosition = GlobalPosition.MoveToward(targetPosition, 100f * (float)delta);
            if (GlobalPosition.DistanceTo(targetPosition) < 1f)
            {
                _idPath.RemoveAt(0);
                if (_idPath.Count == 0)
                    _idPath = null;
            }
            GD.Print("Current ID Path: ", targetPosition, _idPath);

        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.IsPressed())
        {
            Vector2I targetPos = Ground.LocalToMap(GetGlobalMousePosition());
            Vector2I playerPos = Ground.LocalToMap(GlobalPosition);
            GD.Print("Player Position: ", playerPos);
            GD.Print("Global Mouse Position: ", targetPos);

            _targetPosition = targetPos;
            _idPath = _grid.GetIdPath(playerPos, targetPos);
            GD.Print("Path found: ", _idPath);
            _idPath.RemoveAt(0); // Remove the first element which is the current position
        }

    }
}
