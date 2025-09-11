using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class InventorySupplySlot : PanelContainer
{
	// exports
	[Export]
	public Control CountLabelContainer;
	[Export]
	public TextureRect Icon;

	// publics
	public int Count { get; set; } = 0;
	public string SupplyId;
	public SUPPLY_TYPE SupplyType;

	// privates
	private Label _countLabel;


	public override void _Ready()
	{
		_countLabel = CountLabelContainer.GetNode<Label>("Label");
		if (_countLabel == null) GD.PrintErr("No child Label found inside Count container");
	}
	public override void _Process(double delta)
	{
		if (Count < 2) CountLabelContainer.Visible = false;
		else
		{
			CountLabelContainer.Visible = true;
			_countLabel.Text = Count.ToString();
		}
	}

	public void RefreshUI()
	{
		var basePath = InventoryScreen.Instance.IconPath;                 // e.g. "res://assets/game/UI/Inventory/SupplyIcons"
		var wanted = $"{basePath}/{SupplyId}.png";
		var fallback = $"{basePath}/_fallback.png";

		Texture2D tex;
		if (ResourceLoader.Exists(wanted))                                  // <- prevents error log
			tex = ResourceLoader.Load<Texture2D>(wanted);
		else
			tex = ResourceLoader.Load<Texture2D>(fallback);

		Icon.Texture = tex;
		_countLabel.Text = Count.ToString();

	}
}
