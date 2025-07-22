using Godot;
using System.Threading.Tasks;

public partial class TestCropGrowth : Node2D
{
	private Sprite2D sprite;
	private Texture2D[] stages;
	private int currentStage = 0;

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");

		stages = new Texture2D[]
		{
			GD.Load<Texture2D>("res://assets/game/environment/empty.png"),
			GD.Load<Texture2D>("res://assets/game/environment/seed.png"),
			GD.Load<Texture2D>("res://assets/game/environment/sprout.png"),
			GD.Load<Texture2D>("res://assets/game/environment/growing.png"),
			GD.Load<Texture2D>("res://assets/game/environment/matured.png"),
		};

		// Set initial texture
		sprite.Texture = stages[currentStage];

		// Start growth cycle
		_ = StartGrowthAsync();
	}

	private async Task StartGrowthAsync()
	{
		while (currentStage < stages.Length - 1)
		{
			await ToSignal(GetTree().CreateTimer(2f), "timeout");
			currentStage++;
			sprite.Texture = stages[currentStage];
			GD.Print($"Stage {currentStage} set");
		}
	}
}
