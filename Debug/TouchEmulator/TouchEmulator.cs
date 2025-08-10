using Godot;
using System;

public partial class TouchEmulator : Node2D
{
	[Export]
	public float TouchFingerRadius = 15.0f;
	public override void _Ready()
	{
		if (GameManager.IsPlatformMobile) Input.MouseMode = Input.MouseModeEnum.Hidden;
	}
	public override void _Process(double delta)
	{
		QueueRedraw();
	}
	public override void _Draw()
	{
		if (GameManager.IsPlatformMobile)
		{
			DrawCircle(GetGlobalMousePosition(), TouchFingerRadius, new Color(1f, 1f, 1f, 0.3f), true);
		}
	}
}
