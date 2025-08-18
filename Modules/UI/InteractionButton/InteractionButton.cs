using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class InteractionButton : Control
{
	[Export]
	public TouchScreenButton TouchScreenButton;
	// privates
	private bool insideInteractionZone = false;
	public override void _Ready()
	{
		if (TouchScreenButton == null) GD.PrintErr($"At: {GetTree()}, TouchScreenButton is not assigned in InteractionButton.");
		this.Visible = false;
		CallDeferred(nameof(ConnectToEventBus));
		Visible = false;
		
		TouchScreenButton.Released += OnInteractionReleased;
	}
	public override void _Process(double delta)
	{
		if (GameManager.IsPlatformMobile && insideInteractionZone) Visible = true;
		else Visible = false;
	}

	private void OnInteractionReleased()
	{
		EventBus.Instance.EmitInteractionPressed();
	}

	private void ConnectToEventBus()
	{
		EventBus.Instance.Connect("InteractionZoneEntered", new Callable(this, nameof(OnInteractionZoneEntered)));
		EventBus.Instance.Connect("InteractionZoneExited", new Callable(this, nameof(OnInteractionZoneExited)));
	}

	private void OnInteractionZoneEntered(string interaction)
	{
		var interactionType = (INTERACTION)Enum.Parse(typeof(INTERACTION), interaction, true);
		insideInteractionZone = true;
	}
	private void OnInteractionZoneExited(string interaction)
	{
		var interactionType = (INTERACTION)Enum.Parse(typeof(INTERACTION), interaction, true);
		insideInteractionZone = false;
		// Handle interaction logic here
	}
}
