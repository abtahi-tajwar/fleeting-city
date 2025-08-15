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
    [Export]
    public Control MobileButtonContainer;

    private bool _isPressed = false;

    private int _finger = -1;

    public override void _Ready()
    {
        // DesktopButton.Pressed += () =>
        // {
        //     EventBus.Instance.EmitActionChange(ActionType, DesktopButton.ButtonPressed);
        // };
        MobileButton.Pressed += () =>
        {
            InputManager.Instance.ActionButtonFinger = 0;
            EventBus.Instance.EmitActionHold(ActionType, true);
        };
        MobileButton.Released += () =>
        {
            InputManager.Instance.ActionButtonFinger = -1;
            EventBus.Instance.EmitActionHold(ActionType, false);
        };
        // MobileButton.Released += OnMobileReleased;

        // Prevent double subscription if _Ready runs again
        // MobileButtonContainer.GuiInput -= OnMobileContainerGuiInput;
        MobileButtonContainer.GuiInput += OnMobileContainerGuiInput;

    }

    private void OnMobileContainerGuiInput(InputEvent e)
    {
        if (e is InputEventScreenTouch t)
        {
            if (t.Pressed)
            {
                _finger = t.Index;
                // If you also want to hide this finger from gameplay:
                if (InputManager.Instance != null)
                    InputManager.Instance.ActionButtonFinger = _finger;

                // Fire your action if you don’t rely on TouchScreenButton.Pressed
                // EventBus.Instance.EmitActionHold(ActionType, true);
            }
            else if (t.Index == _finger)
            {
                // EventBus.Instance.EmitActionHold(ActionType, false);
                if (InputManager.Instance != null &&
                    InputManager.Instance.ActionButtonFinger == _finger)
                    InputManager.Instance.ActionButtonFinger = -1;

                _finger = -1;
            }

            // Block further propagation (so _UnhandledInput won't see it)
            MobileButtonContainer.AcceptEvent();
        }
        else if (e is InputEventScreenDrag d && d.Index == _finger)
        {
            // Keep blocking while that finger moves on the button
            MobileButtonContainer.AcceptEvent();
        }
    }


    private void OnMobileReleased()
    {
        _isPressed = !_isPressed;
        GD.Print("Mobile button released");
        InputManager.Instance.ActionButtonFinger = 0;
        EventBus.Instance.EmitActionToggle(ActionType);
    }

    public override void _Process(double delta)
    {
        if (_isPressed)
        {
            MobileButton.Modulate = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            MobileButton.Modulate = new Color(1, 1, 1, 1f);
        }
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
