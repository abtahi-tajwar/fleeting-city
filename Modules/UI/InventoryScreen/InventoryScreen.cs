using Godot;
using System;


public partial class InventoryScreen : CanvasLayer
{
	public static InventoryScreen Instance { get; private set; }
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
	[Export]
	public Button CloseButtonDesktop;

	public override void _EnterTree()
	{
		if (Instance != null && Instance != this)
		{
			QueueFree();
			return;
		}
		Instance = this;

	}

	public override void _Ready()
	{

		ItemsTabButton.Pressed += OnItemsTabButtonPress;
		ResourcesTabButton.Pressed += OnResourcesTabButtonPress;
		CloseButtonDesktop.Pressed += OnCloseButtonPress;

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

	private void OnCloseButtonPress()
	{
		Visible = false;
	}
}
