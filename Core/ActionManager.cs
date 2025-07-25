using System;
using FleetingCity.Enums;
using Godot;

public partial class ActionManager : Node
{
	public static ACTION_ENUM CurrentAction { get; private set; } = ACTION_ENUM.POINT;

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

		GD.Print("Connecting to EventBus...");
		var result = EventBus.Instance.Connect(
			"ActionChange",
			new Callable(this, nameof(OnActionChanged))
		);
		GD.Print("Connect result: " +    result); // 0 is OK
	}

	private void OnActionChanged(string actionType, bool value)
	{
		ACTION_ENUM action = (ACTION_ENUM)Enum.Parse(typeof(ACTION_ENUM), actionType, true);
		if (action == ACTION_ENUM.MOVE && value)
		{
			GD.Print("Action changed to MOVE");
			CurrentAction = ACTION_ENUM.MOVE;
		}
		else
		{
			CurrentAction = ACTION_ENUM.POINT;
		}
		GD.Print($"Action changed: {actionType}, Value: {ActionManager.CurrentAction}");
	}
}
