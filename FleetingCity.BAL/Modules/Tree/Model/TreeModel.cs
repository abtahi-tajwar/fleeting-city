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
	public TreeResourceModel Data { get; set; }

	public CurrentTreeResource Clone() => new CurrentTreeResource {
		Data = this.Data,                // keep template reference
		Amount = this.Amount,            // snapshot amount
		LastUpdated = this.LastUpdated
	};


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
	/// True if the root is currently being removed.
	/// </summary>
	public bool IsBeingStumped { get; set; } = false;

	/// <summary>
	/// True if the tree is removed from the world and no longer exists.
	/// </summary>
	public bool IsRemoved { get; set; } = false;

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

		InitializeCurrentResources();
	}

	public void Update()
	{
		UpdateCurrentResources();
		UpdateCurrentStates();
	}
	public void MoveToNextState()
	{
		if (_currentStateIndex != Data.States.Count - 1 && Data.States[_currentStateIndex + 1].State == TREE_STATE.STUMP) return;
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

	private void InitializeCurrentResources()
	{
		foreach (var kvp in Data.ProducedResource)
		{
			if (kvp.IsChoppingRequired || kvp.IsStumpingRequired)
			{
				CurrentResources[kvp.ResourceId] = new()
				{
					Amount = kvp.MaxCapacity,
					LastUpdated = DateTime.UtcNow,
					Data = kvp
				};
				continue;
			}
			CurrentResources[kvp.ResourceId] = new()
			{
				Amount = 0,
				LastUpdated = DateTime.UtcNow,
				Data = kvp
			};
		}
	}
	private void UpdateCurrentResources()
	{
		foreach (var resource in Data.ProducedResource)
		{
			var prodPerState = resource.ProductionPerState;
			prodPerState.TryGetValue(CurrentState.State, out var productionRate);

			if (!resource.IsChoppingRequired && !resource.IsStumpingRequired)
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

	public void StartChopping()
	{
		if (IsBeingChopped || IsFalling) return;

		IsBeingChopped = true;
		IsFalling = false;
	}
	public void StartFalling()
	{
		if (IsFalling) return;

		IsBeingChopped = false;
		IsFalling = true;
	}
	public void CancelChopping()
	{
		if (!IsBeingChopped) return;

		IsBeingChopped = false;
		IsFalling = false;
	}
	public void FinishChopping()
	{
		IsBeingChopped = false;
		IsFalling = false;
		CurrentState = Data.States.FirstOrDefault(s => s.State == TREE_STATE.STUMP);
	}

	public void StartStumping()
	{
		if (IsBeingStumped || IsFalling) return;

		IsBeingStumped = true;
		IsFalling = false;
	}
	public void CancelStumping()
	{
		if (!IsBeingStumped) return;

		IsBeingStumped = false;
		IsFalling = false;
	}
	public void FinishStumping()
	{
		if (!IsBeingStumped) return;
		IsRemoved = true;
	}

	public Dictionary<string, CurrentTreeResource> ClaimResource()
	{
		// Make a shallow copy of the dictionary
		var claimedResources = new Dictionary<string, CurrentTreeResource>();

		foreach (var kvp in CurrentResources)
		{
			if (kvp.Value.Data.IsChoppingRequired && CurrentState.State != TREE_STATE.STUMP) continue;
			if (kvp.Value.Data.IsStumpingRequired && IsRemoved) continue;
			RootProvider.Logger.Dump(kvp.Value);
			claimedResources[kvp.Value.Data.ResourceId] = kvp.Value.Clone();
			kvp.Value.Amount = 0;
			kvp.Value.LastUpdated = DateTime.UtcNow;
		}

		return claimedResources;
	}

	
}
