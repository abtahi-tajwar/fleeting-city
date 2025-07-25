using FleetingCity.Enums;
using Godot;
using System;

public partial class InputManager : Node
{
	public override void _Ready()
	{
		CallDeferred(nameof(ConnectToEventBus));
	}

	private void ConnectToEventBus()
	{
		if (EventBus.Instance == null)
		{
			GD.PrintErr("EventBus instance is still null even after deferring.");
			return;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("move_up"))
		{
			EventBus.Instance.EmitPlayerMove(MOVEMENT_DIRECTION_ENUM.UP);
		}
		else if (@event.IsActionPressed("move_down"))
		{
			EventBus.Instance.EmitPlayerMove(MOVEMENT_DIRECTION_ENUM.DOWN);
		}
		else if (@event.IsActionPressed("move_left"))
		{
			EventBus.Instance.EmitPlayerMove(MOVEMENT_DIRECTION_ENUM.LEFT);
		}
		else if (@event.IsActionPressed("move_right"))
		{
			EventBus.Instance.EmitPlayerMove(MOVEMENT_DIRECTION_ENUM.RIGHT);
		} else if(@event.IsActionReleased("move_up") ||
				  @event.IsActionReleased("move_down") ||
				  @event.IsActionReleased("move_left") ||
				  @event.IsActionReleased("move_right"))
		{
			EventBus.Instance.EmitPlayerMove(MOVEMENT_DIRECTION_ENUM.NONE);
		}
	}
}
