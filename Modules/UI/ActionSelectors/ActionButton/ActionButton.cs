using FleetingCity.BAL.Enum;
using Godot;

public partial class ActionButton : Control
{
    [Export]
    public ACTION_ENUM ActionType;
    [Export]
    public ACTION_BUTTON_SCOPE Scope;
    [Export]
    public Button DesktopButton;
    [Export]
    public TouchScreenButton MobileButton;

    public override void _Ready()
    {

        DesktopButton.Pressed += () =>
        {
            EventBus.Instance.EmitActionChange(ActionType, DesktopButton.ButtonPressed);
        };
        MobileButton.Pressed += () =>
        {
            EventBus.Instance.EmitActionChange(ActionType, true);
        };
        MobileButton.Released += () =>
        {
            EventBus.Instance.EmitActionChange(ActionType, false);
        };

    }

    public override void _Process(double delta)
    {
        if (!GameManager.IsPlatformMobile)
        {
            MobileButton.Visible = false;
            DesktopButton.Visible = false;
        }
        else
        {
            MobileButton.Visible = true;
            DesktopButton.Visible = false;
        }
        if (Scope == ACTION_BUTTON_SCOPE.BOTH)
        {
            this.Visible = true;
            return;
        }
        else
        {
            if (UnitSelectionManager.IsPlayerSelected)
            {
                if (Scope != ACTION_BUTTON_SCOPE.HERO)
                {
                    this.Visible = false;
                    return;
                }
            }
            else
            {
                if (UnitSelectionManager.SelectedSettler != null)
                {
                    if (Scope == ACTION_BUTTON_SCOPE.SETTLER)
                    {
                        this.Visible = true;
                        return;
                    }
                }
            }
        }

    }
}
