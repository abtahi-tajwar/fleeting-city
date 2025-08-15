using Godot;
using System;

public partial class InputTracer : Node
{
	public override void _Ready() => Hook(GetTree().Root);

	private void Hook(Node n)
	{
		foreach (var child in n.GetChildren())
		{
			if (child is Control c)
				c.GuiInput += (InputEvent ev) =>
				{
					// GD.Print($"[GUI] {c.GetPath()} got {ev}");
					ProcessDebug.Print($"[GUI] {c.GetPath()} got {ev}");
				};

			if (child is CollisionObject2D co)
				co.InputEvent += (Node vp, InputEvent ev, long idx) =>
					// GD.Print($"[PHYS] {co.GetPath()} got {ev}");
					ProcessDebug.Print($"[PHYS] {co.GetPath()} got {ev}");

			Hook(child);
		}
	}
}
