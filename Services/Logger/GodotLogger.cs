using FleetingCity.BAL;
using Godot;

public class GodotLogger : ILogger
{
    public void Log(string message) => GD.Print(message);
}
