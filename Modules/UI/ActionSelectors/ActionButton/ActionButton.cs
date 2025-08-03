using FleetingCity.BAL.Enum;
using Godot;

public partial class ActionButton : Control
{
    [Export]
    public ACTION_ENUM ActionType;
    [Export]
    public ACTION_BUTTON_SCOPE Scope;

    public override void _Ready()
    {


        Button btn = GetNode<Button>("Button");

        btn.Pressed += () =>
        {
            EventBus.Instance.EmitActionChange(ActionType, btn.ButtonPressed);
        };
    }

    public override void _Process(double delta)
    {
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
