using System;
using System.Linq;
using System.Reflection;
using FleetingCity.BAL.Provider;

namespace FleetingCity.BAL.Data
{
    public static class GameDataLoader
    {
        public static void LoadAllGameData()
        {
            var gameDataTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    typeof(IGameDataMarker).IsAssignableFrom(t) &&
                    t is { IsClass: true, IsAbstract: false } &&
                    t.GetConstructor(Type.EmptyTypes) != null
                );

            foreach (var type in gameDataTypes)
            {
                try
                {
                    Activator.CreateInstance(type);
                    RootProvider.Logger?.Log($"✅ Loaded {type.Name}");
                }
                catch (Exception ex)
                {
                    RootProvider.Logger?.Log($"❌ Failed to load {type.Name}: {ex.Message}");
                }
            }
        }
    }
}
