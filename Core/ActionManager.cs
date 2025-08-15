using System;
using FleetingCity.BAL.Enum;
using Godot;

public partial class ActionManager : Node
{
	public static ACTION_ENUM CurrentAction { get; set; }

	private bool _wired;
	public override void _EnterTree() {
		GD.Print("Action manager enter tree also twice?");
	}
	public override void _Ready()
	{
		CurrentAction = ACTION_ENUM.POINT;
		GD.Print($"Parent: {GetParent()?.Name}, Path: {GetPath()}, ID: {GetInstanceId()}");
		CallDeferred(nameof(ConnectToEventBus));
	}

	private void ConnectToEventBus()
	{
		if (_wired || EventBus.Instance == null) return;

		var holdCb = new Callable(this, nameof(OnActionHold));
		var toggleCb = new Callable(this, nameof(OnActionToggle));

		if (!EventBus.Instance.IsConnected("ActionHold", holdCb))
		{
			EventBus.Instance.Connect("ActionHold", holdCb);
		}

		if (!EventBus.Instance.IsConnected("ActionToggle", toggleCb))
		{
			GD.Print("Should I connect? ", false);
			EventBus.Instance.Connect("ActionToggle", toggleCb);
		}

		_wired = true;
	}


	private void OnActionHold(string actionType, bool value)
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
	private void OnActionToggle(string actionType)
	{
		ACTION_ENUM action = (ACTION_ENUM)Enum.Parse(typeof(ACTION_ENUM), actionType, true);
		if (action == ACTION_ENUM.MOVE || action == ACTION_ENUM.POINT)
		{
			GD.Print($"Before Action toggled: {CurrentAction}");
			CurrentAction = (CurrentAction == ACTION_ENUM.MOVE) ? ACTION_ENUM.POINT : ACTION_ENUM.MOVE;
			GD.Print($"After Action toggled: {CurrentAction}");
		}
	}
}
