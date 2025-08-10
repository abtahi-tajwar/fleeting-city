using Godot;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class ProcessDebug : Label
{
	public static ProcessDebug Instance;
	public override void _Ready()
	{
		Instance = this;
		if (!OS.HasFeature("editor")) QueueFree();
	}

	public static void Print(string text)
	{
		Instance.Text = text;
	}
}
