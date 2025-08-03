using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class InteractionButton : Control
{
    public override void _Ready()
    {
        this.Visible = false;
        CallDeferred(nameof(ConnectToEventBus));
    }

    private void ConnectToEventBus()
    {
        EventBus.Instance.Connect("InteractionZoneEntered", new Callable(this, nameof(OnInteractionZoneEntered)));
        EventBus.Instance.Connect("InteractionZoneExited", new Callable(this, nameof(OnInteractionZoneExited)));
    }

    private void OnInteractionZoneEntered(string interaction)
    {
        var interactionType = (INTERACTION)Enum.Parse(typeof(INTERACTION), interaction, true);
        this.Visible = true;
    }
    private void OnInteractionZoneExited(string interaction)
    {
        var interactionType = (INTERACTION)Enum.Parse(typeof(INTERACTION), interaction, true);
        this.Visible = false;
        // Handle interaction logic here
    }
}
