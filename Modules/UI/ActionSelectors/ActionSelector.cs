using Godot;
using System;

public partial class ActionSelector : Node
{   
    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child.IsInGroup("ActionButton"))
            {
                if (child is Button button)
                {
                    button.Pressed += () =>
                    {
                        if (button.HasMeta("ActionType"))
                        {
                            string actionType = button.GetMeta("ActionType").ToString();
                        }
                    };
                }
            }
        }
    }
}
