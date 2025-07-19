using Godot;
using System;

public partial class SettlerManager : Node
{
	private PackedScene _settlerScene;
	private Node2D _context;

	public void Init(Node2D context, PackedScene settlerScene)
	{
		// Constructor logic if needed
		if (settlerScene == null)
		{
			GD.PrintErr("SettlerScene is not assigned in the inspector.");
			return;
		}
		_settlerScene = settlerScene;
		_context = context;

		SpawnSettler(new Vector2(10, 20)); // Example position, can be changed
		SpawnSettler(new Vector2(30, 40)); // Example position, can be changed
		SpawnSettler(new Vector2(100, 100)); // Example position, can be changed
	}

	public void SpawnSettler(Vector2 position)
	{
		Settler settler = _settlerScene.Instantiate<Settler>();
		_context.AddChild(settler);
		settler.Position = position; // Set initial position

		// Assign unique ShaderMaterial to this settler's sprite
		var sprite = settler.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (sprite.Material is ShaderMaterial original)
		{
			var uniqueMaterial = (ShaderMaterial)original.Duplicate(true); // Deep copy
			sprite.Material = uniqueMaterial;
		}

	}
}
