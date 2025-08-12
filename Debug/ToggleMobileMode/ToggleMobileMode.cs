using Godot;

public partial class ToggleMobileMode : Label
{
	public override void _Ready()
	{
		if (!OS.HasFeature("editor")) Visible = false;
		UpdateText();
	}

	public override void _UnhandledInput(InputEvent e)
	{
		if (!OS.HasFeature("editor")) return;

		// Fires once on key-down, after UI has had a chance to consume it
		if (e.IsActionPressed("toggle_mobile_debug"))
		{
			GameManager.IsPlatformMobile = !GameManager.IsPlatformMobile;
			UpdateText();
		}
	}

	// // If you prefer keeping it in _Process, use JustPressed:
	// public override void _Process(double delta)
	// {
	// 	if (OS.HasFeature("editor") && Input.IsActionJustPressed("toggle_mobile_debug"))
	// 	{
	// 		GameManager.IsPlatformMobile = !GameManager.IsPlatformMobile;
	// 		UpdateText();
	// 	}
	// }

	private void UpdateText()
	{
		Text = GameManager.IsPlatformMobile
			? "Switch to Desktop (Cmd + P)"
			: "Switch to Mobile (Cmd + P)";
	}
}
