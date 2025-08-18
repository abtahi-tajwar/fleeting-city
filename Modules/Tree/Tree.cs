using FleetingCity.BAL.Data;
using FleetingCity.BAL.Enum;
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

	public override void _Ready()
	{
		var treeTypeExists = TreeData.Instance.Data.ContainsKey(TreeType);
		if (!treeTypeExists) GD.PushError($"At {GetTree()}: Tree type '{TreeType}' does not exist in TreeData. Please check Tree.json to find out the type of trees");
		if (InteractableNode == null) GD.PushError($"At: {GetTree()}: Pleas assign InteractableNode");

		InteractableNode.InteractionCommand += OnInteractionCommand;
	}
	
	private void OnInteractionCommand(string interaction)
	{
		INTERACTION interactionType = Enum.Parse<INTERACTION>(interaction, true);
		if (interactionType == INTERACTION.CHOP_TREE)
		{
			GD.Print($"Chopping tree of type: {TreeType}");
			// Add logic to handle tree chopping
			// For example, remove the tree from the scene or play an animation
			TreeSprite.Play("chopping_left");
		}
		else
		{
			GD.Print($"Unhandled interaction: {interaction} for tree type: {TreeType}");
		}
	}
}
