using Godot;
using System;

public partial class FarmUnits : Node2D
{
	// Exports
	[Export]
	public PackedScene Unit;
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

		// Debug
		var spawned = Unit.Instantiate<AnimalFarmUnit>();
		// var spawnPosition = GenerateUnitRandomRoamingLocation();
		this.AddChild(spawned);
		var bounds = Helper.GetCollisionShapeBounds(_roamingAreaShape);
		GD.Print("Bound from ready", bounds.Position, bounds.Size);
		spawned.Position = new Vector2(bounds.Position.X, bounds.Position.Y);

		var spawned2 = Unit.Instantiate<AnimalFarmUnit>();
		// var spawnPosition2 = GenerateUnitRandomRoamingLocation();
		this.AddChild(spawned2);
		spawned.Position = new Vector2(bounds.Size.X, bounds.Size.Y);
	}

	private void SpawnChickens()
	{
		for (int i = 0; i < TotalUnits; i++)
		{
			var spawned = Unit.Instantiate<AnimalFarmUnit>();
			var spawnPosition = GenerateUnitRandomRoamingLocation();
			this.AddChild(spawned);
			spawned.Position = spawnPosition;

			_spawnedUnits.Add(spawned);
		}
	}
	private Vector2 GenerateUnitRandomRoamingLocation()
	{
		var bounds = Helper.GetCollisionShapeBounds(_roamingAreaShape);
		float x = (float)GD.RandRange(bounds.Position.X, bounds.Position.X + bounds.Size.X);
		float y = (float)GD.RandRange(bounds.Position.Y, bounds.Position.Y + bounds.Size.Y);
		GD.Print("Bound location from random", x, y);
		return new Vector2(x, y);
	}
}
