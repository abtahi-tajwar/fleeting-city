namespace FleetingCity.BAL;

public interface ILogger
{
    void Log(string message);
    void Dump(object obj);
}