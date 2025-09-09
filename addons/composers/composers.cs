#if TOOLS
using Godot;
using System;
using System.Runtime.CompilerServices;

[Tool]
public partial class composers : EditorPlugin
{
	private Window _popup;
	private const string ShortcutName = "composers/open_popup";
	private Shortcut _openPopupShortcut;


	public override void _EnterTree()
	{

		// hook into the scene tree dock when plugin is enabled
		var root = GetEditorInterface().GetEditedSceneRoot();
		GD.Print($"Root name {root.GetType().Name}");




		AddToolMenuItem("Add Interactable Scene", new Callable(this, nameof(AddInteractableScene)));

		WindowUI();
	}


	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		// Clean-up of the plugin goes here.
		// Always remember to remove it from the engine when deactivated.

		var shortcut = new Shortcut();
		shortcut.RemoveUserSignal(ShortcutName);   
		_popup?.QueueFree();
		_popup = null;

	}

	// 3) Listen for the shortcut and open the popup
	public override bool _ForwardCanvasGuiInput(InputEvent e)
	{
		// No IsShortcutPressed() in C# — use MatchesEvent
		if (_openPopupShortcut != null && _openPopupShortcut.MatchesEvent(e))
		{
			// Only on key down, ignore repeats
			if (e is InputEventKey k && k.Pressed && !k.Echo)
			{
				_popup?.PopupCentered();
				return true; // consume
			}
		}
		return false;

	}



	private void WindowUI()
	{
		// 1) Create your popup UI (can be any Control/Window)
		_popup = new Window { Title = "My Composer Popup" };
		_popup.Size = new Vector2I(520, 360);
		_popup.Unresizable = false;

		// Add some UI inside if you want
		var label = new Label { Text = "Hello from a hotkey popup!" };
		_popup.AddChild(label);

		// Attach to the editor’s root so it shows over the editor
		GetEditorInterface().GetBaseControl().AddChild(_popup);

		// 2) Register a shortcut (Ctrl+Shift+P here)
		var shortcut = new Shortcut();
		var arr = new Godot.Collections.Array();   // untyped
		arr.Add(new InputEventKey { Keycode = Key.A, CtrlPressed = true, ShiftPressed = true });
		shortcut.Events = arr;
		

	}



	public void AddInteractableScene()
	{
		var ei = GetEditorInterface();
		var root = ei.GetEditedSceneRoot();
		if (root == null)
		{
			GD.PrintErr("Open a scene first.");
			return;
		}

		// Load the scene you want to instance
		var packed = GD.Load<PackedScene>("res://Composers/Interactable/Interactable.tscn");
		if (packed == null)
		{
			GD.PrintErr("PackedScene not found.");
			return;
		}

		// Choose a parent: selected node if any, else the root
		var selection = ei.GetSelection();
		Node parent = root;
		var selected = selection.GetSelectedNodes();
		if (selected.Count > 0 && selected[0] is Node selNode)
			parent = selNode;

		var inst = packed.Instantiate();

		// Proper editor operation with Undo/Redo + make it part of the edited scene (Owner)
		var ur = GetUndoRedo();
		ur.CreateAction("Add Interactable Scene");
		ur.AddDoMethod(parent, MethodName.AddChild, inst);
		ur.AddDoProperty(inst, "owner", root);               // ensures it saves with the scene
		ur.AddUndoMethod(parent, MethodName.RemoveChild, inst);
		ur.AddUndoMethod(inst, "free");
		ur.CommitAction();

		// Select the new instance so the user can immediately tweak it
		selection.Clear();
		selection.AddNode(inst);

		// (Optional) drop at origin or tweak position if it's 2D/3D
		if (inst is Node2D n2d) n2d.Position = Vector2.Zero;
		// if (inst is Node3D n3d) n3d.Transform = Transform3D.Identity;
	}
}
#endif
