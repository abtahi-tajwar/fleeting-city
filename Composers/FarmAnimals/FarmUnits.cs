using Godot;
using System;

public partial class FarmUnits : Node2D
{
	// Exports
	[Export]
	public PackedScene Unit;
	[Export]
	public TileMapLayer Ground;
	// publics
	public int TotalUnits = 3;

	private CollisionShape2D _roamingAreaShape;
	private Vector2 _targetRoamingLocation;
	private List<AnimalFarmUnit> _spawnedUnits = new List<AnimalFarmUnit>();

	public override void _Ready()
	{
		var roamingArea = GetNodeOrNull<Area2D>("RoamingArea");
		if (roamingArea == null) throw new Exception("No roaming area found. Please add roaming area Area2D exactly named 'RoamingArea' for roaming farm units");
		var roamingAreaShape = roamingArea.GetNodeOrNull<CollisionShape2D>("Shape");
		if (roamingAreaShape == null) throw new Exception("Please add shape to your roaming area with CollisionShape2D exactly named 'Shape'");
		_roamingAreaShape = roamingAreaShape;

		SpawnChickens();

	}

	private void SpawnChickens()
	{
		for (int i = 0; i < TotalUnits; i++)
		{
			var spawned = Unit.Instantiate<AnimalFarmUnit>();
			var spawnPosition = GenerateUnitRandomRoamingLocation();
			this.AddChild(spawned);
			spawned.Setup(Ground);
			spawned.Position = spawnPosition;

			_spawnedUnits.Add(spawned);


			spawned.MovementTimerTimout += () =>
			{
				spawned.HandleMove(GenerateUnitRandomRoamingLocation());
			};
		}
	}
	private Vector2 GenerateUnitRandomRoamingLocation()
	{
		var bounds = Helper.GetCollisionShapeBounds(_roamingAreaShape);
		float x = (float)GD.RandRange(0, bounds.Size.X);
		float y = (float)GD.RandRange(0, bounds.Size.Y);
		return new Vector2(x, y);
	}

}
