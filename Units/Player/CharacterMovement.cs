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
        MarkObstacles(_grid);

    }
    public void MarkObstacles(AStarGrid2D grid)
    {
        foreach (Node2D node in GetTree().GetNodesInGroup("movement_obstacles"))
        {
            CollisionShape2D shape = node.GetNode<CollisionShape2D>("CollisionShape2D");
            Rect2 bounds = shape.Shape.GetRect();

            // Optional: Add padding to avoid visual overlap (in tiles)
            int paddingX = 0;
            int paddingY = 0;

            // Get global-space bounding box of the obstacle
            Vector2 globalTopLeft = node.ToGlobal(bounds.Position);
            Vector2 globalBottomRight = node.ToGlobal(bounds.Position + bounds.Size);

            // Convert to tilemap cell coordinates
            Vector2I tileTopLeft = Ground.LocalToMap(globalTopLeft) - new Vector2I(paddingX, paddingY);
            Vector2I tileBottomRight = Ground.LocalToMap(globalBottomRight) + new Vector2I(paddingX, paddingY);

            // Clamp values to AStarGrid region if needed
            tileTopLeft = tileTopLeft.Clamp(Vector2I.Zero, _grid.Region.Size);
            tileBottomRight = tileBottomRight.Clamp(Vector2I.Zero, _grid.Region.Size);


            for (int x = tileTopLeft.X; x <= tileBottomRight.X; x++)
            {
                for (int y = tileTopLeft.Y; y <= tileBottomRight.Y; y++)
                {
                    Vector2I tile = new Vector2I(x, y);
                    _grid.SetPointSolid(tile, true);
                }
            }

            // Vector2I obstaclePos = Ground.LocalToMap(node.GlobalPosition);
            // GD.Print("Obstacle Position: ", obstaclePos);

            // grid.SetPointSolid(obstaclePos, true);


        }
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
            if (_idPath.Count > 0)
            {
                _idPath.RemoveAt(0); // Remove the first element which is the current position

            }
        }

    }
}
