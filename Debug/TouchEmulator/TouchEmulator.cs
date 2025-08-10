using Godot;
using System;

public partial class TouchEmulator : Node2D
{
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
			DrawCircle(GetGlobalMousePosition(), InputManager.Instance.TouchFingerRadius, new Color(1f, 1f, 1f, 0.3f), true);
		}
	}
}
