using System.Runtime;
using System.Runtime.CompilerServices;
using FleetingCity.BAL.Data;
using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Provider;
using FleetingCity.BAL.Utils;

namespace FleetingCity.BAL.Model;

public class CurrentTreeResource
{
	public float Amount { get; set; }
	public DateTime LastUpdated { get; set; }
};
public class TreeModel
{
	// Data types


	public string Id { get; set; }

	public TreeDataModel Data { get; set; }

	/// <summary>
	/// The current lifecycle state of the tree.
	/// </summary>
	public TreeStateModel CurrentState { get; set; }

	/// <summary>
	/// True if the tree is currently in a chopping animation/action.
	/// </summary>
	public bool IsBeingChopped { get; set; }


	public readonly Dictionary<string, CurrentTreeResource> CurrentResources = new();



	// privates
	private int _currentStateIndex = 0;

	public TreeModel(string treeName, TREE_STATE initialState = TREE_STATE.SAPLING)
	{
		if (TreeData.Instance == null)
		{
			throw new InvalidOperationException("TreeData singleton is not initialized.");
		}
		// Load the tree data from the TreeData singleton
		if (!TreeData.Instance.Data.ContainsKey(treeName))
		{
			throw new KeyNotFoundException($"Tree data for '{treeName}' not found.");
		}
		Id = Guid.NewGuid().ToString();
		IsBeingChopped = false;

		Data = TreeData.Instance.Data[treeName];
		
		CurrentState = Data.States.FirstOrDefault(s => s.State == initialState);
		_currentStateIndex = Data.States.IndexOf(CurrentState);
		
		if (CurrentState == null)
		{
			throw new ArgumentException($"Initial state '{initialState}' not found in tree data.");
		}
		foreach (var kvp in Data.ProducedResource)
		{
			CurrentResources[kvp.ResourceId] = new()
			{
				Amount = 0,
				LastUpdated = DateTime.UtcNow
			};
		}
	}

	public void MoveToNextState()
	{
		if (_currentStateIndex < Data.States.Count - 1)
		{
			_currentStateIndex++;
			CurrentState = Data.States[_currentStateIndex];
			UpdateCurrentResources();
		}
		else
		{
			// If already at the last state, do nothing or handle accordingly
			CurrentState = Data.States[^1];
		}
	}

	private void UpdateCurrentResources()
	{
		foreach (var resource in Data.ProducedResource)
		{
			var prodPerState = resource.ProductionPerState;
			prodPerState.TryGetValue(CurrentState.State, out var productionRate);
			if (!resource.IsChoppingRequired)
			{
				if (HasResourcePassedInterval(resource))
				{
					var currentRes = CurrentResources[resource.ResourceId];
					currentRes.Amount = Math.Min(currentRes.Amount + productionRate, resource.MaxCapacity);
				}
			}
			else
			{
				CurrentResources[resource.ResourceId].Amount = productionRate;
			}
		}
	}

	private bool HasResourcePassedInterval(TreeResourceModel resource)
	{
		if (resource.ProductionIntervalInMinutes == null) return false;
		if (DateTime.Now - CurrentResources[resource.ResourceId].LastUpdated >= TimeSpan.FromMinutes((double)resource.ProductionIntervalInMinutes))
		{
			return true;
		}
		return false;
	}
}
