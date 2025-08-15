using System.Security;
using FleetingCity.BAL.Enum;
using Godot;

public partial class ActionButton : Control
{
    [Export]
    public ACTION_ENUM ActionType;
    [Export]
    public ACTION_BUTTON_SCOPE Scope;
    [Export]
    public bool IsMobileOnly = true;
    // [Export]
    // public Button DesktopButton;
    [Export]
    public TouchScreenButton MobileButton;

    public override void _Ready()
    {
        // DesktopButton.Pressed += () =>
        // {
        //     EventBus.Instance.EmitActionChange(ActionType, DesktopButton.ButtonPressed);
        // };
        MobileButton.Pressed += () =>
        {
            InputManager.Instance.ActionButtonFinger = 0;
            EventBus.Instance.EmitActionChange(ActionType, true);
        };
        MobileButton.Released += () =>
        {
            InputManager.Instance.ActionButtonFinger = -1;
            EventBus.Instance.EmitActionChange(ActionType, false);
        };

    }

    public override void _Process(double delta)
    {
        if (Scope == ACTION_BUTTON_SCOPE.BOTH)
        {
            SetVisible(true);
            return;
        }
        else
        {
            if (UnitSelectionManager.IsPlayerSelected)
            {
                if (Scope != ACTION_BUTTON_SCOPE.HERO)
                {
                    SetVisible(false);
                    return;
                }
            }
            else
            {
                if (UnitSelectionManager.SelectedSettler != null)
                {
                    if (Scope == ACTION_BUTTON_SCOPE.SETTLER)
                    {
                        SetVisible(true);
                        return;
                    }
                }
            }
        }

    }

    public void SetVisible(bool isVisible)
    {
        if (!IsMobileOnly)
        {
            Visible = isVisible;
            return;
        }

        if (isVisible)
        {
            if (!GameManager.IsPlatformMobile)
            {
                Visible = false;
                // DesktopButton.Visible = false;
            }
            else
            {
                Visible = true;
                // DesktopButton.Visible = false;
            }
        }
        else
        {
            Visible = false;
        }
    }
}
