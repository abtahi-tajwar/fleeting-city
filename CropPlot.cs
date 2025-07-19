using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class CropPlot : Area2D
{
    private Sprite2D sprite;
    private List<Texture2D> stages = new();
    private int currentStage = 0;
    private float delayBetweenStages = 2f;
    private bool isGrowing = false;

    public override void _Ready()
    {
        
        sprite = GetNode<Sprite2D>("Sprite2D");

        stages = new List<Texture2D>
        {
            GD.Load<Texture2D>("res://assets/game/environment/empty.png"),
            GD.Load<Texture2D>("res://assets/game/environment/seed.png"),
            GD.Load<Texture2D>("res://assets/game/environment/sprout.png"),
            GD.Load<Texture2D>("res://assets/game/environment/growing.png"),
            GD.Load<Texture2D>("res://assets/game/environment/matured.png"),
        };

        sprite.Texture = stages[currentStage];

        this.InputEvent += OnInputEvent;
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            GD.Print($"Clicked. Current Stage: {currentStage}");

            if (isGrowing)
            {
                GD.Print("Still growing. Click ignored.");
                return;
            }

            if (currentStage == stages.Count - 1)
            {
                // Reset to empty
                currentStage = 0;
                sprite.Texture = stages[currentStage];
                GD.Print("Reset to empty.");
            }
            else
            {
                _ = StartGrowthAsync(); // start growth in background
            }
        }
    }

    private async Task StartGrowthAsync()
    {
        isGrowing = true;

        while (currentStage < stages.Count - 1)
        {
            await ToSignal(GetTree().CreateTimer(delayBetweenStages), "timeout");
            currentStage++;
            sprite.Texture = stages[currentStage];
            GD.Print($"Stage changed to: {currentStage}");
        }

        isGrowing = false;
    }
}