using FleetingCity.BAL.Data;
using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class InteractionButton : Control
{
	[Export]
	public TouchScreenButton InteractButton;
	[Export]
	public TouchScreenButton CancelButton;
	[Export]
	public TextureRect ButtonIcon;
	[Export]
	public Label ButtonLabel;
	// privates
	private bool _insideInteractionZone = false;
	private bool _isInteractionDisabled = false;
	private INTERACTION _currentInteractionType;

	public string IconsPath = "res://Modules/UI/InteractionButton/Icons";
	public override void _Ready()
	{
		if (InteractButton == null) GD.PrintErr($"At: {GetTree()}, TouchScreenButton is not assigned in InteractionButton.");
		this.Visible = false;
		CallDeferred(nameof(ConnectToEventBus));
		Visible = false;

		InteractButton.Released += OnInteractionReleased;
		CancelButton.Released += OnInteractionCancelled;
	}
	public override void _Process(double delta)
	{
		if (GameManager.IsPlatformMobile && _insideInteractionZone) Visible = true;
		else Visible = false;
	}

	private void OnInteractionReleased()
	{
		EventBus.Instance.EmitInteractionPressed();
	}

	private void OnInteractionCancelled()
	{
		EventBus.Instance.EmitInteractionStop();
		ToggleToInteractionButton();
	}

	private void ConnectToEventBus()
	{
		EventBus.Instance.Connect("InteractionZoneEntered", new Callable(this, nameof(OnInteractionZoneEntered)));
		EventBus.Instance.Connect("InteractionZoneExited", new Callable(this, nameof(OnInteractionZoneExited)));
		EventBus.Instance.Connect(EventBus.SignalName.InteractionCommand, new Callable(this, nameof(OnInteractionCommand)));
		EventBus.Instance.Connect(EventBus.SignalName.InteractionStopCommand, new Callable(this, nameof(OnInteractionStopCommand)));
	}

	private void OnInteractionCommand(string interactionType, Node2D sender) {
		ToggleToCancelButton();
	}
	private void OnInteractionStopCommand(string interactionType)
	{
		_currentInteractionType = (INTERACTION)Enum.Parse(typeof(INTERACTION), interactionType, true);
		UpdateButtonAppearance();
		ToggleToInteractionButton();
	}
	private void OnInteractionZoneEntered(string interaction, bool isInteractionDisabled)
	{
		if (isInteractionDisabled) return;
		var interactionType = (INTERACTION)Enum.Parse(typeof(INTERACTION), interaction, true);
		_currentInteractionType = interactionType;
		UpdateButtonAppearance();
		_insideInteractionZone = true;
	}
	private void OnInteractionZoneExited(string interaction)
	{
		var interactionType = (INTERACTION)Enum.Parse(typeof(INTERACTION), interaction, true);
		_insideInteractionZone = false;
		// Handle interaction logic here
	}

	private void UpdateButtonAppearance()
	{
		LoadIconByType(_currentInteractionType);
		LoadButtonLabelByType(_currentInteractionType);
	}

	private void LoadIconByType(INTERACTION interactionType)
	{
		string iconPath = $"{IconsPath}/{interactionType.ToString()}.png";
		var texture = ResourceLoader.Load<Texture2D>(iconPath);
		if (texture != null)
		{
			ButtonIcon.Texture = texture;
		}
		else
		{
			GD.PrintErr($"Icon not found for interaction type: {interactionType}");
		}
	}

	private void LoadButtonLabelByType(INTERACTION interactionType)
	{
		var hintData = InteractableHintData.Instance.Data[interactionType.ToString()];
		ButtonLabel.Text = hintData.Label.ToUpper();
	}

	private void ToggleToCancelButton()
	{
		CancelButton.Visible = true;
		InteractButton.Visible = false;
		ButtonLabel.Text = "CANCEL";
		ButtonIcon.Texture = ResourceLoader.Load<Texture2D>($"{IconsPath}/CANCEL.png");
	}
	private void ToggleToInteractionButton()
	{
		CancelButton.Visible = false;
		InteractButton.Visible = true;
		UpdateButtonAppearance();
	}
}
