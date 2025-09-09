using Godot;
using System;

public partial class OpenInventoryButton : PanelContainer
{
	[Export]
	public Button DesktopButton;

	public override void _Ready()
	{
		DesktopButton.Pressed += () =>
		{
			GD.Print("Inventory button pressed");
			InventoryScreen.Instance.Visible = true;
		};
	}
	
}
