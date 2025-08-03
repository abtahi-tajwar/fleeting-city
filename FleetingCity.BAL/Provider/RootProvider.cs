namespace FleetingCity.BAL.Provider;

public class RootProvider
{
    public static ILogger Logger;
    public RootProvider(ILogger logger)
    {
        Logger = logger;
    }
}