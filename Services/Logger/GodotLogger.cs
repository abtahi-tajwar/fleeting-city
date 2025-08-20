using FleetingCity.BAL;
using Godot;

public class GodotLogger : ILogger
{
    public void Log(string message) => GD.Print(message);
    public void Dump(object obj)
    {
        if (obj == null)
        {
            GD.Print("null");
            return;
        }

        var props = obj.GetType().GetProperties();
        foreach (var p in props)
        {
            GD.Print($"{p.Name} = {p.GetValue(obj)}");
        }
    }

}
