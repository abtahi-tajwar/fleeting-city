using Godot;
using System;

public partial class MousePosition : Control
{
	[Export]
	public bool ShowMousePosition = false;
	public override void _Ready()
	{
		GD.PushWarning("Please remove MousePosition Node before actual build. This is a debug Node");
		if (!ShowMousePosition)
		{
			Visible = false;
		}
		else
		{
			Visible = true;
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
