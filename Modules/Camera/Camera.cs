using Godot;
using System;
using System.Drawing;

public partial class Camera : Camera2D
{
	// Exports
	[Export]
	public float CameraSpeed = 50f;
	[Export]
	public float ZoomSensitivity = 50;

	public static Vector2 MovementDistanceFromInitialPosition;
	public static Vector2 CurrentZoom;

	// Privates
	private Node2D _target = null;
	private bool _followTarget = true;
	private Vector2 _initialPosition;
	public float _zoomSensitivityCoefficient = 0.0005f;
	public override void _EnterTree()
	{
		MakeCurrent();
	}
	public override void _Ready()
	{
		_initialPosition = GlobalPosition;
		CallDeferred(nameof(SetDefaultTargetToPlayer));
		CallDeferred(nameof(ConnectToEventBus));

	}

	public override void _Process(double delta)
	{
		MovementDistanceFromInitialPosition = _initialPosition - GlobalPosition;
		CurrentZoom = Zoom;
		if (_followTarget)
		{
			// Smoothly follow the target
			if (_target != null)
			{
				Position = _target.GlobalPosition;
			}

		}
		if (InputManager.Instance.IsDragging)
		{
			_followTarget = false;

			// Instead of scaling delta directly, normalize it and scale manually
			Vector2 deltaTime = InputManager.Instance.MouseDragDelta;

			if (deltaTime.Length() > 0.01f)
			{
				var boundRect = WorldBound.WorldShape.GetRect();
				// Normalize direction but keep consistent movement speed
				Vector2 direction = deltaTime.Normalized();

				// Apply fixed speed in that direction (scale with delta to simulate drag)
				Vector2 newPosition = new Vector2(
					direction.X * CameraSpeed * deltaTime.Length() * (float)delta,
					direction.Y * CameraSpeed * deltaTime.Length() * (float)delta
				);
				Position -= newPosition;
				Position = Position.Clamp(boundRect.Position, boundRect.End);
			}

			// Reset delta
			InputManager.Instance.MouseDragDelta = Vector2.Zero;

		}
	}

	private void SetDefaultTargetToPlayer()
	{
		if (GameManager.Player != null)
		{
			_target = GameManager.Player;
		}
		else
		{
			GD.PrintErr("Player is not set in GameManager. Cannot set camera target.");
		}
	}

	public void RefocusToPlayer(string _)
	{
		_followTarget = true;
	}
	public void OnPinchZoom(float pinchZoomDelta)
	{
		float normalized = Mathf.Sign(pinchZoomDelta); 
		GD.Print($"Pinch zoom delta: {pinchZoomDelta}, {normalized}");
		// pinchZoomDelta > 0 → zoom out, < 0 → zoom in
		// float zoomChange = pinchZoomDelta * ZoomSensitivity; // small number, e.g., 0.001f
		float zoomChange =  2500 * (normalized / ZoomSensitivity) * _zoomSensitivityCoefficient; // small number, e.g., 0.001f

		Zoom += new Vector2(zoomChange, zoomChange);

		// Clamp final zoom between reasonable limits
		Zoom = Zoom.Clamp(new Vector2(1.5f, 1.5f), new Vector2(5f, 5f));

	}
	public void OnScrollZoom(int direction)
	{
		// pinchZoomDelta > 0 → zoom out, < 0 → zoom in
		// float zoomChange = pinchZoomDelta * ZoomSensitivity; // small number, e.g., 0.001f
		float zoomChange =  2500 * (direction / ZoomSensitivity) * _zoomSensitivityCoefficient; // small number, e.g., 0.001f

		Zoom += new Vector2(zoomChange, zoomChange);

		// Clamp final zoom between reasonable limits
		Zoom = Zoom.Clamp(new Vector2(1.5f, 1.5f), new Vector2(5f, 5f));

	}
	private void ConnectToEventBus()
	{
		if (EventBus.Instance == null)
		{
			GD.PrintErr("EventBus instance is still null even after deferring.");
			return;
		}

		EventBus.Instance.Connect(
			"PlayerMove",
			new Callable(this, nameof(RefocusToPlayer))
		);
		EventBus.Instance.Connect(
			"PinchZoom",
			new Callable(this, nameof(OnPinchZoom))
		);
		EventBus.Instance.Connect(
			"ScrollZoom",
			new Callable(this, nameof(OnScrollZoom))
		);
		GD.Print("Connected to EventBus for PlayerMove events from camera.");
	}
}
