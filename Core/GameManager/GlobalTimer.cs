using Godot;

public partial class GlobalTimer : Godot.Timer
{
	public static GlobalTimer Instance { get; private set; }

	public override void _EnterTree()
	{
		Instance = this;
		WaitTime = 1.0f; // Set the timer to tick every second
		Start();
	}

	public void ResetTimer()
	{
		Stop();
		Start();
	}
}
