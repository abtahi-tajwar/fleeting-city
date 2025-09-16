using FleetingCity.BAL.Model;
using Godot;
using System;

public partial class SupplyDetailsScreen : Control
{
	public SupplyDetailsScreen Instance;

	private bool _isOpen = false;
#nullable enable
	public ISupplyModel? SelectedSupply { get; set; } = null;
#nullable disable

	// Exports
	[Export]
	public Label Title;
	[Export]
	public TextureRect Icon;

	public override void _EnterTree()
	{
		if (Instance != null)
		{
			GD.PrintErr("Supply screen is already instantiated! Make sure to delete the duplicate supply screen");
			return;
		}
		Instance = this;
	}
	public void SetSelectedSupply(ISupplyModel supply)
	{
		SelectedSupply = supply;
	}
	public void OpenScreen()
	{
		UpdateSupplyDetails();
		_isOpen = true;
	}
	public void CloseScreen()
	{
		_isOpen = false;
	}

	private void UpdateSupplyDetails()
	{
		if (SelectedSupply != null)
		{
			Title.Text = SelectedSupply.Name;
			var supplyIcon = $"{InventoryScreen.Instance.IconPath}/{SelectedSupply.Id}";
			var fallbackIcon = $"{InventoryScreen.Instance.IconPath}/_fallback.png";
			Texture2D tex;
			if (ResourceLoader.Exists(supplyIcon))                                  // <- prevents error log
				tex = ResourceLoader.Load<Texture2D>(supplyIcon);
			else
				tex = ResourceLoader.Load<Texture2D>(fallbackIcon);

			Icon.Texture = tex;
		}
	}

}
