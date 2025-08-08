using Godot;
using System;
using System.Buffers;

public partial class Outliner : Node2D
{
	private Node2D _parent;
	private Rect2 _parentShape;
	[Export]
	public Shape2D Shape;

	public override void _Ready()
	{
		GD.PushWarning("Please remove Outliner Node before actual build. This is a debug Node");
		_parent = GetParent<Node2D>();
		_parentShape = Helper.GetBounds(_parent);
		GD.Print($"Parent position: ({_parentShape.Position.X}, {_parentShape.Position.Y})");
		GD.Print($"Parent size: ({_parentShape.Size.X}, {_parentShape.Size.Y})");
		QueueRedraw();
	}

	public override void _Draw()
	{
		if (Shape is RectangleShape2D rect)
		{
			// RectangleShape2D.Size is full size, center it
			Vector2 position = ToLocal(_parentShape.Position);
			
			// DrawRect(new Rect2(-halfSize, rect.Size), Colors.Red, false, 2f);
			DrawRect(new Rect2(position, _parentShape.Size), Colors.Red, false, 2f);
		}
		else if (Shape is CircleShape2D circle)
		{
			Vector2 position = ToLocal(_parentShape.Position);
			DrawCircle(position, circle.Radius, Colors.Blue);
		}
		else if (Shape is CapsuleShape2D capsule)
		{
			// Approximate a capsule as a rectangle + two circles
			float radius = capsule.Radius;
			float height = capsule.Height;

			// Center rectangle
			Vector2 rectPos = new Vector2(-radius, -height / 2);
			Vector2 rectSize = new Vector2(radius * 2, height);
			DrawRect(new Rect2(rectPos, rectSize), Colors.Green, false, 2f);

			// Top and bottom circles
			DrawCircle(new Vector2(0, -height / 2), radius, Colors.Green);
			DrawCircle(new Vector2(0, height / 2), radius, Colors.Green);
		}
		else
		{
			GD.Print("Shape type not supported in Outliner");
		}
	}


}
