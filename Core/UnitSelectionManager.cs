using Godot;
using System;

public partial class UnitSelectionManager : Node
{
	// Statics
	public static bool IsPlayerSelected { get; set; } = false;
	public static string? SelectedSettlerId { get; set; } = null;
	public Node2D _unit { get; set; }

	public UnitSelectionManager(Node2D unit)
	{
		if (unit == null) throw new ArgumentNullException(nameof(unit), "Unit cannot be null.");
		_unit = unit;
	}

	public void SelectPointedUnit(Vector2 mousePos)
	{
		var isMouseOver = Helper.IsMouseOverCollider(_unit, mousePos);
		if (!isMouseOver)
		{
			return;
		}
		if (_unit is Player player)
		{
			IsPlayerSelected = true;
			SelectedSettlerId = null; // Clear any selected settler ID
			GD.Print("Player unit selected.");
		}
		else if (_unit is Settler settler)
		{
			IsPlayerSelected = false;
			SelectedSettlerId = settler.UniqueId;
			GD.Print($"Settler unit selected with UniqueId: {settler.UniqueId}");
		}
		else
		{
			IsPlayerSelected = false;
			SelectedSettlerId = null;
			GD.Print("Unit selection failed. Not a valid unit type.");
		}
	}

	public static void SetOutline(Node2D node, bool enabled)
	{
		var sprite = node.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var material = sprite.Material as ShaderMaterial;

		if (material != null)
		{
			material.SetShaderParameter("show_outline", enabled);
		}
	}
}
