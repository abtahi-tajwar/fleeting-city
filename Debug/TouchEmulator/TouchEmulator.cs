using Godot;
using System;

public partial class TouchEmulator : Control
{
	public override void _Ready()
	{
		
		// Force this node to always be on top of all others
		var layer = GetParent() as CanvasLayer;
		if (layer != null)
		{
			layer.Layer = 2000; // max layer order
		}

	}
	public override void _Process(double delta)
	{
		if (GameManager.IsPlatformMobile) Input.MouseMode = Input.MouseModeEnum.Hidden;
		else Input.MouseMode = Input.MouseModeEnum.Visible;
		QueueRedraw();
	}
	public override void _Draw()
	{
		if (GameManager.IsPlatformMobile)
		{
			DrawCircle(GetGlobalMousePosition(), InputManager.Instance.TouchFingerRadius, new Color(1f, 1f, 1f, 0.3f), true);
		}
	}
}
