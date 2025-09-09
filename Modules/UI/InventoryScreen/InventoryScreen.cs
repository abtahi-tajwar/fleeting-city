using Godot;
using System;


public partial class InventoryScreen : Control
{
	[Export]
	public INVENTORY_SCREEN CurrentScreen;
	[Export]
	public Button ItemsTabButton;
	[Export]
	public Button ResourcesTabButton;
	[Export]
	public Control ItemsTab;
	[Export]
	public Control ResourcesTab;

	public override void _Ready()
	{

		ItemsTabButton.Pressed += OnItemsTabButtonPress;
		ResourcesTabButton.Pressed += OnResourcesTabButtonPress;
	}

	public override void _Process(double delta)
	{
	}

	private void OnItemsTabButtonPress()
	{
		ItemsTabButton.ButtonPressed = true;
		ResourcesTabButton.ButtonPressed = false;
		ItemsTab.Visible = true;
		ResourcesTab.Visible = false;
	}
	private void OnResourcesTabButtonPress()
	{
		ResourcesTabButton.ButtonPressed = true;
		ItemsTabButton.ButtonPressed = false;
		ItemsTab.Visible = false;
		ResourcesTab.Visible = true;
	}
}
