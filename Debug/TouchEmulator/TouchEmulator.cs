using Godot;
using System;

public partial class TouchEmulator : Control
{
	[Export]
	public float TouchInputOpacity = 0.5f;
	[Export]
	public Label HintText;
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
		_inputs.Add(new TouchInput(this, Vector2.Zero));
		_currentInputIndex = 0;
		if (!OS.HasFeature("editor")) QueueFree();

	}
	public override void _Process(double delta)
	{
		if (GameManager.IsPlatformMobile)
		{
			Input.MouseMode = Input.MouseModeEnum.Hidden;
			HintText.Visible = true;
		}
		else
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
			HintText.Visible = false;
		}
		// _inputs[_currentInputIndex].Position = GetGlobalMousePosition();
		QueueRedraw(); // repaint now that we moved

		for (int i = 0; i < _inputs.Count; i++)
		{
			_inputs[i].EmitEvents(i);
		}

	}
	public override void _Draw()
	{

		if (GameManager.IsPlatformMobile)
		{
			for (int i = 0; i < _inputs.Count; i++)
			{
				_inputs[i].Draw(i, _currentInputIndex);
			}
		}

	}

	public override void _Input(InputEvent e)
	{
		if (e is InputEventMouseMotion mm)
		{
			_inputs[_currentInputIndex].Position = GetGlobalMousePosition();
		}

		if (e is InputEventKey keyEvent && !keyEvent.Echo)
		{
			if (keyEvent.Pressed)
			{

				if (keyEvent.AltPressed)
				{
					if (keyEvent.Keycode == Key.N)
					{
						_inputs.Add(new TouchInput(this, GetGlobalMousePosition()));
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

					if (keyEvent.Keycode == Key.Z)
					{
						_inputs[_currentInputIndex].Pressed = !_inputs[_currentInputIndex].Pressed;
					}

					if (keyEvent.Keycode == Key.X)
					{
						if (_inputs.Count <= 1)
						{
							GD.Print("Can't remove this input, deletion is only possible when there is more than 1 input");
							return;
						}
						_inputs.RemoveAt(_currentInputIndex);
						_currentInputIndex -= 1;
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

	// Privates
	private TouchEmulator _context;
	// track previous state to avoid spamming
	private bool _lastPressed = false;
	private Vector2 _lastPos = Vector2.Zero;


	public TouchInput(TouchEmulator context, Vector2 position)
	{
		Position = position;
		_context = context;
	}

	public void Draw(int index, int _currentSelectedIndex)
	{
		// Pick a font resource (DynamicFont, BitmapFont, Theme font, etc.)
		var font = _context.GetThemeFont("font", "Label"); // uses default UI font

		string text = $"{index + 1}";

		Color inputColor = this.Pressed ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, _context.TouchInputOpacity);
		// Color + optional width limit + alignment
		_context.DrawCircle(this.Position, InputManager.Instance.TouchFingerRadius, inputColor, true);
		if (index == _currentSelectedIndex) _context.DrawCircle(Position, InputManager.Instance.TouchFingerRadius + 10, inputColor, false, 5);
		_context.DrawString(font, Position, text, HorizontalAlignment.Center, -1, 16, new Color(0, 0, 0));
	}

	public void EmitEvents(int index)
	{
		// 1) DOWN / UP: only when state flips
		if (Pressed != _lastPressed)
		{
			var touch = new InputEventScreenTouch
			{
				Position = Position,
				Index = index,
				Pressed = Pressed
			};
			_context.GetViewport().PushInput(touch);
			_lastPressed = Pressed;
			_lastPos = Position;
			return;
		}

		// 2) DRAG: when pressed and position changed
		if (Pressed && Position != _lastPos)
		{
			var drag = new InputEventScreenDrag
			{
				Position = Position,
				Index = index,
				Relative = Position - _lastPos,
				Pressure = 1.0f
			};
			_context.GetViewport().PushInput(drag);
			_lastPos = Position;
		}
		// 3) Otherwise: do nothing this frame
	}
}
