using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public float Speed { get; set; } = 200f;
	
	public Vector2 ScreenResolution;
}
