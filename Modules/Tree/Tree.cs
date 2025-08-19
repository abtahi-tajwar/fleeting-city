using FleetingCity.BAL.Data;
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

	// publics
	public TreeModel Model;

	public override void _Ready()
	{

		var treeTypeExists = TreeData.Instance.Data.ContainsKey(TreeType);
		if (!treeTypeExists) GD.PushError($"At {GetTree()}: Tree type '{TreeType}' does not exist in TreeData. Please check Tree.json to find out the type of trees");
		if (InteractableNode == null) GD.PushError($"At: {GetTree()}: Pleas assign InteractableNode");

		InteractableNode.InteractionCommand += OnInteractionCommand;
		InteractableNode.InteractionStopCommand += OnInteractionStopCommand;

		Model = new TreeModel(TreeType);

		GlobalTimer.Instance.Timeout += OnTimerTimeout;
	}

	private void OnTimerTimeout()
	{
		GD.Print($"Timer checking in tree");
		Model.UpdateCurrentResources();
		Model.UpdateCurrentStates();
		LoadAnimation();
	}
	private void OnInteractionCommand(string interaction)
	{
		INTERACTION interactionType = Enum.Parse<INTERACTION>(interaction, true);
		if (interactionType == INTERACTION.CHOP_TREE)
		{
			GD.Print($"Chopping tree of type: {TreeType}");
			// Add logic to handle tree chopping
			// For example, remove the tree from the scene or play an animation
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
				TreeSprite.Play("stump");
			} else if (Model.CurrentState.State == TREE_STATE.SAPLING)
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
	

}
