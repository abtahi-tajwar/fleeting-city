using Godot;
using System;

public partial class UnitSelectionManager : Node
{
	// Statics
	public static bool IsPlayerSelected { get; set; } = true;
	public static Settler SelectedSettler { get; set; } = null;
	public Node2D _unit { get; set; }

	public UnitSelectionManager(Node2D unit)
	{
		if (unit == null) throw new ArgumentNullException(nameof(unit), "Unit cannot be null.");

		_unit = unit;
	}


	public void SelectUnit()
	{
		if (_unit is Player)
		{
			IsPlayerSelected = true;
			SelectedSettler = null;
		}
		else if (_unit is Settler settler)
		{
			IsPlayerSelected = false;
			SelectedSettler = settler;
		}
		else
		{
			IsPlayerSelected = false;
			SelectedSettler = null;
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
