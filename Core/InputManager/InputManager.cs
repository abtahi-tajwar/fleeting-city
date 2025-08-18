using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class InputManager : Node2D
{
	// Singleton instance
	[Export]
	public float TouchFingerRadius = 40.0f;
	[Export]
	public Area2D InputCatchArea;
	public static InputManager Instance { get; private set; }
	public bool IsDragging { get; set; } = false;
	public bool IsPinching { get; set; } = false;
	public Vector2 MouseDragDelta = Vector2.Zero;

	public Dictionary<int, Vector2> RegisteredFingers = new(); // index -> WORLD pos
	public Dictionary<int, Vector2> RegisteredFingersLocal = new(); // index -> Local pos
	public int ActionButtonFinger { get; set; } = -1;
	public int TotalRegisteredFingers = 0;


	//privates
	private bool IsMousePressed = false;
	private bool DraggingJustFinished = false;
	private Dictionary<int, Vector2> _pinchZoomFingerPositions = new Dictionary<int, Vector2>()
	{
		{ 0, Vector2.Zero },
		{ 1, Vector2.Zero }
	};
	private float _pinchZoomInitialDelta = 0f;



	public static Vector2 GlobalMousePosition = Vector2.Zero;

	// privates
	private Vector2 _lastMousePosition;
	public override void _Ready()
	{
		Instance = this;
		CallDeferred(nameof(ConnectToEventBus));
		// InitInputArea();
	}

	private void InitInputArea()
	{
		var inputAreaShape = InputCatchArea.GetNode<CollisionShape2D>("Shape");
		if (inputAreaShape == null)
		{
			GD.PrintErr("Input area shape for input manager is not defined. Please add collision shape named (Shape) for Area2D");
			return;
		}

		if (inputAreaShape.Shape is RectangleShape2D rect)
		{
			// Get viewport size in pixels
			Vector2 vpSize = GetViewportRect().Size;

			// Find active Camera2D
			var cam = GetViewport().GetCamera2D();
			if (cam != null)
			{
				// Apply zoom (zoom.x == zoom.y usually)
				vpSize *= cam.Zoom;
			}

			// Convert to world units (1:1 if no scaling elsewhere)
			rect.Size = vpSize;

			// Position shape centered on camera view
			if (cam != null)
				inputAreaShape.Position = cam.GlobalPosition;
			else
				inputAreaShape.Position = vpSize / 2f;

			GD.Print("Viewport size:", GetViewportRect().Size, " Zoom:", cam?.Zoom, " Collision rect size:", rect.Size);
		}

		// InputCatchArea.InputPickable = true;
		// InputCatchArea.InputEvent += OnInputAreaEvent;
	}




	private void ConnectToEventBus()
	{
		if (EventBus.Instance == null)
		{
			GD.PrintErr("EventBus instance is still null even after deferring.");
			return;
		}
	}

	private MOVEMENT_DIRECTION_ENUM _lastDirection = MOVEMENT_DIRECTION_ENUM.NONE;

	public override void _Process(double delta)
	{
		CapturePlayerMovementInput(delta);
		if (!GameManager.IsPlatformMobile)
		{
			CaptureMovementCommand();
			CalculateGlobalMousePosition();
		}
		// UpdatePinchFingerPositions();
	}
	private void UpdatePinchFingerPositions()
	{
		if (!IsPinching || RegisteredFingersLocal.Count < 2) return;

		var keys = RegisteredFingersLocal.Keys.GetEnumerator();
		keys.MoveNext(); var k1 = keys.Current;
		keys.MoveNext(); var k2 = keys.Current;

		// GD.Print($"Initial Positions: {RegisteredFingersLocal[k1]}, {RegisteredFingersLocal[k2]}, Newest position: {_pinchZoomFingerPositions[0]}, {_pinchZoomFingerPositions[1]}");
		var initialDistance = RegisteredFingersLocal[k1].DistanceTo(RegisteredFingersLocal[k2]);
		var newDistance = _pinchZoomFingerPositions[0].DistanceTo(_pinchZoomFingerPositions[1]);

		EventBus.Instance.EmitPinchZoom(newDistance - initialDistance);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// Treat Ctrl (Win/Linux) or Command (macOS) as the zoom modifier.
        bool ctrlLike = Input.IsKeyPressed(Key.Ctrl) || Input.IsKeyPressed(Key.Meta);

		// Trackpad two-finger scroll on macOS
		if (@event is InputEventPanGesture pan && ctrlLike)
		{
			// On most trackpads: negative Y = scroll up, positive Y = scroll down
			int dir = pan.Delta.Y < 0 ? -1 : +1; // +1 zoom in, -1 zoom out
			EventBus.Instance.EmitScrollZoom(dir);
		}

		if (@event is InputEventMouseButton mouseEvent && !GameManager.IsPlatformMobile)
		{
			if (mouseEvent.Pressed)
			{
				OnMouseDown(GetViewport().GetMousePosition());

				if (mouseEvent.CtrlPressed)
				{
					GD.Print("Ctrl pressed");
					if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
					{
						GD.Print("Wheel up");
						EventBus.Instance.EmitScrollZoom(1);
					}
					else if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
					{
						EventBus.Instance.EmitScrollZoom(-1);
					}
				}

			}
			else
			{
				OnMouseUp(GetGlobalMousePosition());
			}
		}
		else if (@event is InputEventScreenTouch touchEvent && GameManager.IsPlatformMobile)
		{
			var touchPos = CalculateGlobalPointerPosition(touchEvent.Position);
			if (touchEvent.Pressed)
			{
				// register THIS finger
				RegisteredFingers[touchEvent.Index] = touchPos;
				RegisteredFingersLocal[touchEvent.Index] = touchEvent.Position;
				_pinchZoomFingerPositions[touchEvent.Index] = touchEvent.Position;
				TotalRegisteredFingers += 1;

				OnMouseDown(touchEvent.Position);
			}
			else
			{
				// OnMouseUp(touchPos);
				// update final pos, then release THIS finger
				RegisteredFingers[touchEvent.Index] = touchPos;
				RegisteredFingersLocal[touchEvent.Index] = touchEvent.Position;
				OnMouseUp(RegisteredFingers[touchEvent.Index]);
				RegisteredFingers.Remove(touchEvent.Index);
				RegisteredFingersLocal.Remove(touchEvent.Index);
				TotalRegisteredFingers -= 1;
			}
		}


		// Detect motion
		if (@event is InputEventMouseMotion && IsMousePressed && !GameManager.IsPlatformMobile)
		{
			if (IsMousePressed)
			{
				IsDragging = true;
				// Capture mouse movement delta
				var currentMousePosition = GetViewport().GetMousePosition();
				MouseDragDelta = currentMousePosition - _lastMousePosition;
				_lastMousePosition = currentMousePosition;
			}
			// Don't reset dragging here
		}
		else if (@event is InputEventScreenDrag touch && IsMousePressed && GameManager.IsPlatformMobile)
		{
			if (IsMousePressed)
			{
				if (TotalRegisteredFingers == 1)
				{
					IsDragging = true;
					// Capture mouse movement delta
					var currentMousePosition = touch.Position;
					MouseDragDelta = currentMousePosition - _lastMousePosition;
					_lastMousePosition = currentMousePosition;
				}
				else if (TotalRegisteredFingers == 2)
				{
					IsPinching = true;
					if (touch.Index == 0) _pinchZoomFingerPositions[0] = touch.Position;
					else if (touch.Index == 1) _pinchZoomFingerPositions[1] = touch.Position;
					UpdatePinchFingerPositions();
				}
			}
		}

	}

	private void OnMouseDown(Vector2 mousePosition)
	{
		IsMousePressed = true;
		_lastMousePosition = mousePosition;
	}
	private void OnMouseUp(Vector2 mousePosition)
	{
		IsMousePressed = false;
		if (!IsDragging)
		{
			EventBus.Instance.EmitUnitMove(mousePosition);
		}
		IsDragging = false;
		if (TotalRegisteredFingers < 2) IsPinching = false;
	}

	private void CaptureMovementCommand()
	{
		if (Input.IsActionPressed("move_command"))
		{
			ActionManager.CurrentAction = ACTION_ENUM.MOVE;
		}
		else if (Input.IsActionJustReleased("move_command"))
		{
			ActionManager.CurrentAction = ACTION_ENUM.POINT;
		}
		else
		{
			return; // No action change
		}
	}
	private void CapturePlayerMovementInput(double delta)
	{
		Vector2 vec = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		MOVEMENT_DIRECTION_ENUM newDir = MOVEMENT_DIRECTION_ENUM.NONE;

		if (vec.LengthSquared() > 0.01f) // Ignore slight stick noise
		{
			float angle = vec.Angle(); // Radians

			// Convert angle into direction
			if (angle >= -Mathf.Pi / 8 && angle < Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.RIGHT;
			else if (angle >= Mathf.Pi / 8 && angle < 3 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.DOWN_RIGHT;
			else if (angle >= 3 * Mathf.Pi / 8 && angle < 5 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.DOWN;
			else if (angle >= 5 * Mathf.Pi / 8 && angle < 7 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.DOWN_LEFT;
			else if (angle >= 7 * Mathf.Pi / 8 || angle < -7 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.LEFT;
			else if (angle >= -7 * Mathf.Pi / 8 && angle < -5 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.UP_LEFT;
			else if (angle >= -5 * Mathf.Pi / 8 && angle < -3 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.UP;
			else if (angle >= -3 * Mathf.Pi / 8 && angle < -Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.UP_RIGHT;
		}

		if (newDir != _lastDirection)
		{
			_lastDirection = newDir;
			EventBus.Instance.EmitPlayerMove(newDir);
		}
	}

	private void CalculateGlobalMousePosition()
	{
		// if (_camera == null) { GD.PrintErr("No Camera found"); return; }
		var globalPos = GetGlobalMousePosition();
		// var cameraSize = _camera.GetViewportRect().Size;
		// var cameraPos = _camera.Position;
		// var halfX = cameraSize.X / 2;
		// var halfY = cameraSize.Y / 2;
		// var zoom = _camera.Zoom;

		// var finalGlobalPos = new Vector2(globalPos.X - halfX + (cameraPos.X * zoom.X), globalPos.Y - halfY + (cameraPos.Y * zoom.Y));
		// // InputManager.GlobalMousePosition = finalGlobalPos;
		InputManager.GlobalMousePosition = globalPos;
	}

	private Vector2 CalculateGlobalPointerPosition(Vector2 touchPos)
	{

		return GetCanvasTransform().AffineInverse() * touchPos;
	}


}
