using FleetingCity.BAL.Data;
using FleetingCity.BAL.Enum;
using Godot;
using Microsoft.VisualBasic;
using System;

public partial class Interactable : Node
{
	private Area2D _area;

	// Exports
	[Export]
	public INTERACTION InteractionType;
	[Export]
	public Label HintLabel;

	// Signals
	[Signal]
	public delegate void InteractionZoneEnteredEventHandler(CharacterBody2D character);
	[Signal]
	public delegate void InteractionZoneExitedEventHandler(CharacterBody2D character);
	[Signal]
	public delegate void InteractionCommandEventHandler(string interactionType);
	[Signal]
	public delegate void InteractionStopCommandEventHandler(string interactionType);

	// publics
	public Player InteractingCharacter { get; private set; }

	// Privates
	private string _interactionKeyName;
	private string _interactionCancelKeyName;


	public override void _Ready()
	{
		_interactionKeyName = GetActionKeyName("interact");
		_interactionCancelKeyName = GetActionKeyName("stop_interact");
		foreach (var item in InteractableHintData.Instance.Data)
		{
			GD.Print($"id: {item.Key}, label: {item.Value.Label}");
		}
		if (HintLabel != null
			|| (GameManager.IsPlatformMobile && HintLabel != null)) HintLabel.Visible = false;
		_area = GetNode<Area2D>("InteractBoundary");

		if (_area == null)
		{
			GD.PrintErr("Area2D node not found in Interactable.");
		}
		else
		{
			_area.BodyEntered += OnBodyEntered;
			_area.BodyExited += OnBodyExited;
		}

		CallDeferred(nameof(ConnectToEventBus));
	}

	public override void _Process(double delta)
	{
		// if (!GameManager.IsPlatformMobile && InteractingCharacter != null)
		// {
		// 	CaptureInteractionCommand();
		// }
	}

	private void ConnectToEventBus()
	{
		EventBus.Instance.Connect("InteractionPressed", new Callable(this, nameof(OnInteractionPressed)));
		EventBus.Instance.Connect("InteractionStop", new Callable(this, nameof(OnInteractionStop)));
	}

	private void OnInteractionPressed()
	{
		if (InteractingCharacter != null)
		{
			EventBus.Instance.EmitInteractionCommand(InteractionType);
			EmitSignal(SignalName.InteractionCommand, InteractionType.ToString());
			GD.Print($"Interaction command emitted for {InteractionType}");
			HintLabel.Text = $"Press {_interactionCancelKeyName} to stop {InteractionType}";
		}
	}

	private void OnInteractionStop()
	{
		if (InteractingCharacter != null)
		{
			EventBus.Instance.EmitInteractionStopCommand(InteractionType.ToString());
			EmitSignal(SignalName.InteractionStopCommand, InteractionType.ToString());
			SetHint();
		}
	}

	public void UpdateInteraction(INTERACTION newInteraction)
	{
		InteractionType = newInteraction;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player character)
		{
			InteractingCharacter = character;
			if (HintLabel != null && !GameManager.IsPlatformMobile)
			{
				HintLabel.Visible = true;
				foreach (var item in InteractableHintData.Instance.Data)
				{
					GD.Print($"id: {item.Key}, label: {item.Value.Label}");
				}
				SetHint();
			}
			EmitSignal(SignalName.InteractionZoneEntered, character);
			EventBus.Instance.EmitInteractionZoneEntered(InteractionType);
		}
	}
	private void OnBodyExited(Node body)
	{
		if (body is Player character)
		{
			if (HintLabel != null) HintLabel.Visible = false;
			EmitSignal(SignalName.InteractionZoneExited, body);
			EventBus.Instance.EmitInteractionZoneExited(InteractionType);
		}
	}

	private string GetActionKeyName(string actionName)
	{
		var events = InputMap.ActionGetEvents(actionName);

		foreach (var e in events)
		{
			if (e is InputEventKey keyEvent)
			{
				return OS.GetKeycodeString(keyEvent.PhysicalKeycode);
			}
		}

		return null; // no key bound
	}
	private void SetHint()
	{
		var hintData = InteractableHintData.Instance.Data[InteractionType.ToString()];
		HintLabel.Text = (hintData != null) ? GetHintLabel(hintData.Label) : GetHintLabel("Interact");
	}
	private string GetHintLabel(string interactionType)
	{
		return $"Press {_interactionKeyName} to {interactionType}";
	}

	private bool CheckIfCharacterSelected(CharacterBody2D character)
	{
		if (character == null)
		{
			GD.PrintErr("Character is null in Interactable.");
			return false;
		}
		else if (character is Player)
		{
			GD.Print("Checking if Player is selected.");
			return UnitSelectionManager.IsPlayerSelected;
		}
		else if (character is Settler s)
		{
			GD.Print("Checking if Settler is selected.");
			return UnitSelectionManager.SelectedSettler != null && UnitSelectionManager.SelectedSettler.Model.Id == s.Model.Id;
		}
		return false;
	}

}
