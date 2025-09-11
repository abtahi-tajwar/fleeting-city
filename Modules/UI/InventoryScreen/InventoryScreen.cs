using FleetingCity.BAL.Enum;
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
	public InventoryResourcesTab ResourcesTab;
	[Export]
	public Button CloseButtonDesktop;
	[Export]
	public PackedScene SupplySlot;
	[Export]
	public PackedScene SupplyPickableSlot;
	[Export]
	public string IconPath = $"res://assets/game/UI/Inventory/SupplyIcons";

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

		CallDeferred(nameof(ConnectToEventBus));

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

	private void OnSupplyClaimed()
	{
		GD.Print("Supply claimed");
		UpdateResources();
	}

	private void UpdateResources()
	{
		var resources = PlayerInventory.Instance.InventoryModel.Resources;
		var container = ResourcesTab.SupplySlotsContainer;

		// Remove all the slots first
		foreach (Control child in container.GetChildren())
		{
			child.QueueFree();
		}

		foreach (var resource in resources)
		{
			var slot = SupplySlot.Instantiate<InventorySupplySlot>();
			slot.SupplyId = resource.Value.ResourceId;
			slot.SupplyType = SUPPLY_TYPE.RESOURCE;
			slot.Count = resource.Value.Amount;
			container.AddChild(slot);
			slot.RefreshUI();
		}
	}

	private void ConnectToEventBus()
	{
		EventBus.Instance.Connect("InventorySupplyClaimed", new Callable(this, nameof(OnSupplyClaimed)));
	}

}
