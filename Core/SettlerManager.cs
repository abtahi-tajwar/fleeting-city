using Godot;
using System;

public partial class SettlerManager : Node
{
	private PackedScene _settlerScene;
	public void Init(PackedScene settlerScene)
	{
		// Constructor logic if needed
		if (settlerScene == null)
		{
			GD.PrintErr("SettlerScene is not assigned in the inspector.");
			return;
		}
		_settlerScene = settlerScene;
		// Example of instantiating a Settler
		Settler settler = _settlerScene.Instantiate<Settler>();
		AddChild(settler);
		settler.Position = new Vector2(100, 100); // Set initial position
	}
}
