using System;

namespace FleetingCity.BAL.Modules;

public class SettlerModel
{
    public string UniqueId { get; private set; }

    // Additional properties and methods can be added here as needed
    public SettlerModel()
    {
        UniqueId = Guid.NewGuid().ToString(); // Assign a unique ID to the settler
    }
}