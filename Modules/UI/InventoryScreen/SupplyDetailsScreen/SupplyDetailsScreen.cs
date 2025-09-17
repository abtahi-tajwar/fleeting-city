using FleetingCity.BAL.Model;
using Godot;
using System;

public partial class SupplyDetailsScreen : Control
{
	public static SupplyDetailsScreen Instance;
	// publics
#nullable enable
	public ISupplyModel? SelectedSupply { get; set; } = null;
#nullable disable

	// Exports
	[Export(PropertyHint.Range, "0.05,1.5,0.01")] public float SlideDuration = 0.25f;
	[Export] public Tween.TransitionType Transition = Tween.TransitionType.Cubic;
	[Export] public Tween.EaseType Ease = Tween.EaseType.Out;

	[Export]
	public Label Title;
	[Export]
	public TextureRect Icon;
	[Export]
	public bool IsOpen = false;
	[Export]
	public Button CloseDesktopButton;


	// privates
	private bool _isOpen = false;
	private Vector2 _openPosition;
	private Vector2 _closePosition;
	private Tween _tween; // keep a handle so we can interrupt


	public override void _EnterTree()
	{
		if (Instance != null)
		{
			GD.PrintErr("Supply screen is already instantiated! Make sure to delete the duplicate supply screen");
			return;
		}
		Instance = this;
	}

	public override void _Ready()
	{
		_closePosition = Position;
		_openPosition = new Vector2(Position.X - Size.X, Position.Y);
		CloseScreen();


		CloseDesktopButton.Pressed += HandleDesktopCloseButtonPress;
	}
	public void SetSelectedSupply(ISupplyModel supply)
	{
		SelectedSupply = supply;
	}
	public void OpenScreen()
	{
		UpdateSupplyDetails();
		_isOpen = true;
		AnimateTo(_openPosition);

	}
	public void CloseScreen()
	{
		_isOpen = false;
		AnimateTo(_closePosition);
	}
	private void AnimateTo(Vector2 target)
	{
		// If we’re already at target, skip
		if (Position.DistanceTo(target) < 0.5f) { Position = target; return; }

		// Kill any in-flight tween so direction changes feel snappy
		_tween?.Kill();

		_tween = CreateTween();
		_tween.SetTrans(Transition).SetEase(Ease);
		_tween.TweenProperty(this, "position", target, SlideDuration);
		// Optional: when closing finishes, you could hide() to remove from input
		// _tween.Finished += () => { if (!_isOpen) Hide(); else Show(); };
	}


	private void UpdateSupplyDetails()
	{
		if (SelectedSupply != null)
		{
			Title.Text = SelectedSupply.Name;
			var supplyIcon = $"{InventoryScreen.Instance.IconPath}/{SelectedSupply.Id}";
			var fallbackIcon = $"{InventoryScreen.Instance.IconPath}/_fallback.png";
			Texture2D tex;
			if (ResourceLoader.Exists(supplyIcon))                                  // <- prevents error log
				tex = ResourceLoader.Load<Texture2D>(supplyIcon);
			else
				tex = ResourceLoader.Load<Texture2D>(fallbackIcon);

			Icon.Texture = tex;
		}
	}

	private void HandleDesktopCloseButtonPress()
	{
		CloseScreen();
	}

}
