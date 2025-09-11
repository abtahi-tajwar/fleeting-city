using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class EventBus : Node
{
	//Singleton instance
	public static EventBus Instance { get; private set; }
	// Singals
	[Signal]
	public delegate void ActionHoldEventHandler(string actionType, bool value);
	[Signal]
	public delegate void ActionToggleEventHandler(string actionType);
	[Signal]
	public delegate void PlayerMoveEventHandler(string actionType);
	[Signal]
	public delegate void UnitMoveEventHandler(Vector2 touchPos);
	[Signal]
	public delegate void PinchZoomEventHandler(float pinchZoomDelta);
	[Signal]
	public delegate void ScrollZoomEventHandler(int direction);

	// From Interactables
	[Signal]
	public delegate void InteractionZoneEnteredEventHandler(string interaction, bool isInteractionDisabled);
	[Signal]
	public delegate void InteractionZoneExitedEventHandler(string interaction);
	[Signal]
	public delegate void InteractionPressedEventHandler();
	[Signal]
	public delegate void InteractionStopEventHandler();
	[Signal]
	public delegate void InteractionCommandEventHandler(string interaction, Node2D sender);
	[Signal]
	public delegate void InteractionStopCommandEventHandler(string interactionType);
	[Signal]
	public delegate void SelectedUnitChangedEventHandler();
	[Signal]
	public delegate void InventorySupplyClaimedEventHandler();


	public override void _EnterTree()
	{
		if (Instance == null) Instance = this;
	}
	public override void _ExitTree()
	{
		if (Instance == this) Instance = null;
	}

	#region Action Events
	public void EmitActionHold(ACTION_ENUM actionType, bool value)
	{
		EmitSignal(SignalName.ActionHold, actionType.ToString(), value);
	}
	public void EmitActionToggle(ACTION_ENUM actionType)
	{
		EmitSignal(SignalName.ActionToggle, actionType.ToString());
	}
	#endregion


	#region Movement & Selection Events

	public void EmitPlayerMove(MOVEMENT_DIRECTION_ENUM actionType)
	{
		EmitSignal(SignalName.PlayerMove, actionType.ToString());
	}

	public void EmitUnitMove(Vector2 touchPos)
	{
		EmitSignal(SignalName.UnitMove, touchPos);
	}
	public void EmitSelectedUnitChanged()
	{
		EmitSignal(SignalName.SelectedUnitChanged);
	}
	#endregion


	#region Zoom Events
	public void EmitPinchZoom(float pinchZoomDelta)
	{
		EmitSignal(SignalName.PinchZoom, pinchZoomDelta);
	}
	public void EmitScrollZoom(int direction)
	{
		EmitSignal(SignalName.ScrollZoom, direction);
	}
	#endregion


	#region Interaction Events
	public void EmitInteractionZoneEntered(INTERACTION interaction, bool isInterationDisabled)
	{
		EmitSignal(SignalName.InteractionZoneEntered, interaction.ToString(), isInterationDisabled);
	}
	public void EmitInteractionZoneExited(INTERACTION interaction)
	{
		EmitSignal(SignalName.InteractionZoneExited, interaction.ToString());
	}
	public void EmitInteractionCommand(INTERACTION interactionType, Node2D sender)
	{
		EmitSignal(SignalName.InteractionCommand, interactionType.ToString(), sender);
	}
	public void EmitInteractionStopCommand(INTERACTION interactionType)
	{
		EmitSignal(SignalName.InteractionStopCommand, interactionType.ToString());
	}
	public void EmitInteractionPressed()
	{
		EmitSignal(SignalName.InteractionPressed);
	}

	public void EmitInteractionStop()
	{
		EmitSignal(SignalName.InteractionStop);
	}

	#endregion


	#region Inventory
	public void EmitInventorySupplyClaimed()
	{
		EmitSignal(SignalName.InventorySupplyClaimed);
	}
	#endregion
}
