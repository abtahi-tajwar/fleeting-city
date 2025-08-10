using Godot;
using System;

public partial class UIManager : Node
{
	public override void _Input(InputEvent e)
	{
		// Drop all mouse input on the floor
		if (e is InputEventMouseButton || e is InputEventMouseMotion)
		{
			if (GameManager.IsPlatformMobile)
			{
				GetViewport().SetInputAsHandled(); // stop propagation
				return;
			}
		}
	}
}
