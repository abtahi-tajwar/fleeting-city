using Godot;
using System;

public partial class MousePosition : Control
{
	[Export]
	public bool ShowMousePosition = false;
	public override void _Ready()
	{
		if (!ShowMousePosition)
		{
			Visible = false;
		}
		else
		{
			Visible = true;
		}

		if (!OS.HasFeature("editor"))
		{
			QueueFree();
		}
	}
	public override void _Process(double delta)
	{

		if (ShowMousePosition)
		{
			GetNode<Label>("Label").Text = $"Mouse: {InputManager.GlobalMousePosition}";
		}
	}

}
