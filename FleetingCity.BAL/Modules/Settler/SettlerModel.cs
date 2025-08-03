using System;

namespace FleetingCity.BAL.Model;

public class SettlerModel
{
    public string Id { get; private set; }

    // Additional properties and methods can be added here as needed
    public SettlerModel()
    {
        Id = Guid.NewGuid().ToString(); // Assign a unique ID to the settler
    }
}