using Godot;
using System;

public partial class SettlerManager : Node2D
{
	[Export]
	public PackedScene SettlerScene;
	[Export]
	public Node2D SpawnPoints;

	public override void _Ready()
	{
		// SpawnSettler(new Vector2(10, 20)); // Example position, can be changed
		// SpawnSettler(new Vector2(30, 40)); // Example position, can be changed

		// SpawnSettler(new Vector2(100, 100)); // Example position, can be changed
		if (SpawnPoints != null)
		{
			var SpawnPointsChildren = SpawnPoints.GetChildren();
			foreach (Node2D spawnPoint in SpawnPointsChildren)
			{
				if (spawnPoint is Node2D point)
				{
					SpawnSettler(point.GlobalPosition);
				}
				else
				{
					GD.PrintErr("Spawn point is not a Node2D: " + spawnPoint.Name);
				}
			}
		}
		else
		{
			GD.Print("No Settler will be spawned. SpawnPoints is null.");
		}


	}

	public void SpawnSettler(Vector2 position)
	{
		Settler settler = SettlerScene.Instantiate<Settler>();
		this.AddChild(settler);
		settler.Position = position; // Set initial position

		// Assign unique ShaderMaterial to this settler's sprite
		var sprite = settler.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (sprite.Material is ShaderMaterial original)
		{
			var uniqueMaterial = (ShaderMaterial)original.Duplicate(true); // Deep copy
			sprite.Material = uniqueMaterial;
		}
		settler.ZIndex = 1;
		settler.YSortEnabled = true;

	}
}
