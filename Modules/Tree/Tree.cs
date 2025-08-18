using FleetingCity.BAL.Data;
using Godot;
using System;

public partial class Tree : Node2D
{
	[Export]
	public string TreeType;

	public override void _Ready()
	{
		var treeTypeExists = TreeData.Instance.Data.ContainsKey(TreeType);
		if (!treeTypeExists) GD.PushError($"Tree type '{TreeType}' does not exist in TreeData. Please check Tree.json to find out the type of trees");
	}
}
