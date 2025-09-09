using System.Runtime.CompilerServices;
using FleetingCity.BAL.Data;
using FleetingCity.BAL.Dto;
using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Model;
using Godot;

public partial class Tree : Node2D
{
	[Export]
	public string TreeType;
	[Export]
	public Interactable InteractableNode;
	[Export]
	public AnimatedSprite2D TreeSprite;
	[Export]
	public Godot.Timer ChopTimer;
	[Export]
	public Godot.Timer FallTimer;
	[Export]
	public Godot.Timer StumpTimer;
	[Export]
	public ProgressBar ProgressBar;

	[Export]
	public TREE_STATE InitialState = TREE_STATE.SAPLING;

	// publics
	public TreeModel Model;

	public override void _Ready()
	{

		var treeTypeExists = TreeData.Instance.Data.ContainsKey(TreeType);
		if (!treeTypeExists) GD.PushError($"At {GetTree()}: Tree type '{TreeType}' does not exist in TreeData. Please check Tree.json to find out the type of trees");
		if (InteractableNode == null) GD.PushError($"At: {GetTree()}: Pleas assign InteractableNode");

		InteractableNode.InteractionCommand += OnInteractionCommand;
		InteractableNode.InteractionStopCommand += OnInteractionStopCommand;

		Model = new TreeModel(TreeType, InitialState);
		Model.Id = $"{Name}";
		LoadAnimation();
		GlobalTimer.Instance.Timeout += OnTimerTimeout;
		ChopTimer.Timeout += OnChopTimerTimeout;
		FallTimer.Timeout += FinishChopping;
		StumpTimer.Timeout += FinishStumping;

		ProgressBar.Visible = false;
		ProgressBar.Value = 0;
	}
	public override void _Process(double delta)
	{
		if (!ChopTimer.IsStopped())
		{
			ProgressBar.Visible = true;
			ProgressBar.Value = ChopTimer.TimeLeft / ChopTimer.WaitTime * 100;
		}
		else if (!StumpTimer.IsStopped())
		{
			ProgressBar.Visible = true;
			ProgressBar.Value = StumpTimer.TimeLeft / StumpTimer.WaitTime * 100;
		}
		else
		{
			ProgressBar.Visible = false;
			ProgressBar.Value = 0;
		}
	}

	private void OnTimerTimeout()
	{
		Model.Update();
		LoadAnimation();
	}
	private void OnChopTimerTimeout()
	{
		Model.StartFalling();
		FallTimer.Start();
		LoadAnimation();
	}

	private void FinishChopping()
	{
		Model.FinishChopping();
		ClaimResources();
		LoadAnimation();
		InteractableNode.FinishInteraction(INTERACTION.STUMP_TREE);
		InteractableNode.UpdateInteraction(INTERACTION.STUMP_TREE);
	}
	private void FinishStumping()
	{
		GD.Print("Stumping should finish");
		Model.FinishStumping();
		ClaimResources();
		LoadAnimation();
		InteractableNode.FinishInteraction(null);
		InteractableNode.DisableInteraction();

		RemoveNode();
	}

	private void ClaimResources()
	{
		var resources = Model.ClaimResource();

		if (resources != null && resources.Count > 0)
		{
			foreach (var resource in resources)
			{
				PlayerInventory.Instance.AddResource(resource.Key, (int)Math.Floor(resource.Value.Amount));
				GD.Print($"Chopped {resource.Value.Amount} of {resource.Key}");
			}
		}
	}
	private void OnInteractionCommand(string interaction)
	{
		INTERACTION interactionType = Enum.Parse<INTERACTION>(interaction, true);
		if (interactionType == INTERACTION.CHOP_TREE)
		{
			Model.StartChopping();
			ChopTimer.Start();
			LoadAnimation();
		}
		else if (interactionType == INTERACTION.STUMP_TREE)
		{
			if (Model.IsBeingChopped || Model.IsFalling)
			{
				GD.Print("Cannot remove trunk while tree is being chopped or is falling.");
				return;
			}
			GD.Print("Stumping should start");
			Model.StartStumping();
			StumpTimer.Start();
			LoadAnimation();
		}

		else
		{
			GD.Print($"Unhandled interaction: {interaction} for tree type: {TreeType}");
		}
	}
	private void OnInteractionStopCommand(string interaction)
	{
		INTERACTION interactionType = Enum.Parse<INTERACTION>(interaction, true);
		if (interactionType == INTERACTION.CHOP_TREE)
		{
			if (Model.IsFalling)
			{
				GD.Print("Cannot stop chopping while tree is falling.");
				return;
			}
			Model.CancelChopping();
			ChopTimer.Stop();
			LoadAnimation();
		}
		else if (interactionType == INTERACTION.STUMP_TREE)
		{
			Model.CancelStumping();
			StumpTimer.Stop();
			LoadAnimation();
		}
		else
		{
			GD.Print($"Unhandled interaction: {interaction} for tree type: {TreeType}");
		}
	}

	public void LoadAnimation()
	{
		if (TreeSprite == null)
		{
			GD.PushError($"At {GetTree()}: TreeSprite is not assigned. Please assign it in the editor.");
			return;
		}

		if (Model.IsFalling)
		{
			TreeSprite.Play("falling");
		}
		else if (Model.IsBeingChopped)
		{
			TreeSprite.Play("chopping_left");
		}
		else
		{
			if (Model.CurrentState.State == TREE_STATE.STUMP)
			{
				if (Model.IsBeingStumped)
				{
					TreeSprite.Play("stumping");
				}
				else if (Model.IsRemoved)
				{
					TreeSprite.Play("removed");
				}
				else
				{
					TreeSprite.Play("stump");
				}
			}
			else if (Model.CurrentState.State == TREE_STATE.SAPLING)
			{
				TreeSprite.Play("sapling");
			}
			else if (Model.CurrentState.State == TREE_STATE.YOUNG)
			{
				TreeSprite.Play("young");
			}
			else if (Model.CurrentState.State == TREE_STATE.MATURE)
			{
				TreeSprite.Play("mature");
			}
			else
			{
				GD.PushError($"At {GetTree()}: Unhandled tree state: {Model.CurrentState.State}");
				TreeSprite.Play("idle");

			}
		}
	}


	public void RemoveNode()
	{
		if (Model.IsRemoved) QueueFree();
		else GD.PushError($"At {GetTree()}: Cannot remove tree node because it is not removed yet. Please ensure the tree is removed before calling this method.");
	}

}
