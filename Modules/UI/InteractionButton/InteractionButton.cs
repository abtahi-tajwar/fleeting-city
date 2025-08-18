using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class InteractionButton : Control
{
    // privates
    private bool insideInteractionZone = false;
    public override void _Ready()
    {
        this.Visible = false;
        CallDeferred(nameof(ConnectToEventBus));
        Visible = false;
    }
    public override void _Process(double delta)
    {
        if (GameManager.IsPlatformMobile && insideInteractionZone) Visible = true;
        else Visible = false;
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
