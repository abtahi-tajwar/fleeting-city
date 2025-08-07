using FleetingCity.BAL.Enum;
using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public class AnimalFarmUnitMovementService
{
	private AStarGrid2D _grid;
	private TileMapLayer _ground;
	private CharacterBody2D _context;
	private Godot.Collections.Array<Vector2I> _idPath;

	public AnimalFarmUnitMovementService(CharacterBody2D context, TileMapLayer ground)
	{
		if (ground == null) throw new Exception("Ground tilemap layer cannot be null");
		if (context == null) throw new ArgumentNullException(nameof(context), "Context cannot be null.");

		_context = context;
		_ground = ground;

		if (_ground == null) throw new InvalidOperationException("Animal farm Ground tilemap layer is null");

		_context = context;
		_grid = new AStarGrid2D();
		Rect2I groundCoords = _ground.GetUsedRect();
		_grid.Region = groundCoords;
		_grid.CellSize = new Vector2I(_ground.RenderingQuadrantSize, _ground.RenderingQuadrantSize);
		_grid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Always;
		_grid.Update();
		MarkObstacles(_grid);
	}

	public void MarkObstacles(AStarGrid2D grid)
	{
		foreach (Node2D node in _context.GetTree().GetNodesInGroup("movement_obstacles"))
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
			Vector2I tileTopLeft = _ground.LocalToMap(globalTopLeft) - new Vector2I(paddingX, paddingY);
			Vector2I tileBottomRight = _ground.LocalToMap(globalBottomRight) + new Vector2I(paddingX, paddingY);

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



		}
	}

	public void StartMovementOnClick(Vector2 mouseClickPositionGlobal)
	{
		Vector2I targetPos = _ground.LocalToMap(_context.GetGlobalMousePosition());
		Vector2I playerPos = _ground.LocalToMap(_context.GlobalPosition);

		_idPath = _grid.GetIdPath(playerPos, targetPos);
		if (_idPath.Count > 0)
		{
			_idPath.RemoveAt(0); // Remove the first element which is the current position
		}
	}
	public void StartMovementTo(Vector2 Position)
	{
		var globalPosition = _context.ToGlobal(Position);
		// GD.Print($"Global Position: ({globalPosition.X}, {globalPosition.Y}); LocalPosition: ({Position.X}, {Position.Y})");
		// Vector2I targetPos = _ground.LocalToMap(globalPosition);
		Vector2I targetPos = _ground.LocalToMap(Position);
		Vector2I playerPos = _ground.LocalToMap(_context.Position);

		_idPath = _grid.GetIdPath(playerPos, targetPos);
		if (_idPath.Count > 0)
		{
			_idPath.RemoveAt(0); // Remove the first element which is the current position
		}
	}
	public MOVEMENT_DIRECTION_ENUM Update(double delta)
	{
		if (_idPath == null)
		{
			return MOVEMENT_DIRECTION_ENUM.NONE;
		}
		if (_idPath.Count == 0)
		{
			_idPath = null; // Clear the path when done
			return MOVEMENT_DIRECTION_ENUM.NONE;
		}

		Vector2I nextPosition = _idPath[0];
		Vector2 targetPosition = _ground.MapToLocal(nextPosition);
		_context.Position = _context.Position.MoveToward(targetPosition, 100f * (float)delta);
		if (_context.Position.DistanceTo(targetPosition) < 1f)
		{
			_idPath.RemoveAt(0);
			if (_idPath.Count == 0)
				_idPath = null;
		}
		return Helper.GetMovementDirection(_context.Position, targetPosition);
	}

}
