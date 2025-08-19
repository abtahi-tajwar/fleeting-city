using System.Data;
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
	public DateTime LastStateChangeTime { get; set; }

	/// <summary>
	/// True if the tree is currently in a chopping animation/action.
	/// </summary>
	public bool IsBeingChopped { get; set; }

	/// <summary>
	/// True if the tree is currently falling down.
	/// <summary>
	public bool IsFalling { get; set; } = false;


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
		LastStateChangeTime = DateTime.UtcNow;
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
		var currentState = Data.States[_currentStateIndex];
		if (currentState.State == TREE_STATE.STUMP) return;
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

	public void UpdateCurrentStates()
	{
		if (CurrentState.State == TREE_STATE.STUMP)
		{
			IsFalling = false;
			IsBeingChopped = false;
			return;
		}
		if (DateTime.UtcNow - LastStateChangeTime >= TimeSpan.FromMinutes(CurrentState.GrowingDurationInMinutes))
		{
			MoveToNextState();
			LastStateChangeTime = DateTime.UtcNow;
		}
	}

	public void UpdateCurrentResources()
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

	private void StartChopping()
	{
		if (IsBeingChopped || IsFalling) return;

		IsBeingChopped = true;
		IsFalling = false;
	}
	private void StartFalling()
	{
		if (IsFalling || IsBeingChopped) return;

		IsFalling = true;
		IsBeingChopped = false;
	}
	private void CancelChopping()
	{
		if (!IsBeingChopped) return;

		IsBeingChopped = false;
		IsFalling = false;
	}
	private Dictionary<string, CurrentTreeResource> CompleteChopping()
	{
		if (!IsBeingChopped) return null;

		IsBeingChopped = false;
		IsFalling = false;
		var resources = CurrentResources;

		foreach (var resource in Data.ProducedResource)
		{
			if (resource.IsChoppingRequired)
			{
				if (resources.TryGetValue(resource.ResourceId, out var currentResource))
				{
					currentResource.Amount = resource.ProductionPerState[CurrentState.State];
					currentResource.LastUpdated = DateTime.UtcNow;
				}
			}
		}

		CurrentState = Data.States.FirstOrDefault(s => s.State == TREE_STATE.STUMP);

		return resources;
	}
}
