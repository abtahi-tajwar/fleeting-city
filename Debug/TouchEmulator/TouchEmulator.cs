using Godot;
using System;

public partial class TouchEmulator : Control
{
	[Export]
	public float TouchInputOpacity = 0.5f;
	private List<TouchInput> _inputs = new List<TouchInput>();
	private int _currentInputIndex;
	private bool _altMMode = false;


	public override void _Ready()
	{
		// Force this node to always be on top of all others
		var layer = GetParent() as CanvasLayer;
		if (layer != null)
		{
			layer.Layer = 2000; // max layer order
		}
		_inputs.Add(new TouchInput(Vector2.Zero));
		_currentInputIndex = 0;
		if (!OS.HasFeature("editor")) QueueFree();

	}
	public override void _Process(double delta)
	{
		if (GameManager.IsPlatformMobile) Input.MouseMode = Input.MouseModeEnum.Hidden;
		else Input.MouseMode = Input.MouseModeEnum.Visible;
		_inputs[_currentInputIndex].Position = GetGlobalMousePosition();
		QueueRedraw(); // repaint now that we moved

	}
	public override void _Draw()
	{

		if (GameManager.IsPlatformMobile)
		{
			for (int i = 0; i < _inputs.Count; i++)
			{
				// Pick a font resource (DynamicFont, BitmapFont, Theme font, etc.)
				var font = GetThemeFont("font", "Label"); // uses default UI font

				string text = $"{i + 1}";

				// Color + optional width limit + alignment
				DrawString(font, _inputs[i].Position, text, HorizontalAlignment.Center, -1, 16, new Color(0, 0, 0));
				DrawCircle(_inputs[i].Position, InputManager.Instance.TouchFingerRadius, new Color(1f, 1f, 1f, TouchInputOpacity), true);
				if (i == _currentInputIndex) DrawCircle(_inputs[i].Position, InputManager.Instance.TouchFingerRadius + 10, new Color(1f, 1f, 1f, TouchInputOpacity), false, 5);
			}
		}

	}

	public override void _Input(InputEvent e)
	{

		if (e is InputEventKey keyEvent && !keyEvent.Echo)
		{
			if (keyEvent.Pressed)
			{

				if (keyEvent.AltPressed)
				{
					if (keyEvent.Keycode == Key.N)
					{
						_inputs.Add(new TouchInput(GetGlobalMousePosition()));
						_currentInputIndex = _inputs.Count - 1;
					}

					if (keyEvent.Keycode == Key.M)
					{
						_altMMode = true;

					}
					// If in Alt+M mode, check for numbers 1–5
					if (_altMMode && keyEvent.Keycode >= Key.Key1 && keyEvent.Keycode <= Key.Key5)
					{
						int idx = (int)keyEvent.Keycode - (int)Key.Key1;
						SelectInput(idx);
					}
				}
			}
			else
			{
				if (keyEvent.Keycode == Key.M)
				{
					_altMMode = false; // leave mode if you want
				}
			}

		}

	}

	private void SelectInput(int index)
	{
		if (index >= _inputs.Count)
		{
			GD.Print($"No input {index + 1} created to select");
			return;
		}
		_currentInputIndex = index;
	}

}

public class TouchInput
{
	public Vector2 Position { get; set; } = Vector2.Zero;
	public bool Pressed { get; set; } = false;

	public TouchInput(Vector2 position)
	{
		Position = position;
	}

}
