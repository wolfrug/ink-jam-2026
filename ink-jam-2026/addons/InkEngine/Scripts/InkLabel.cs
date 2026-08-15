using Godot;
using GodotInk;
using System;
using MiTale;
using System.Collections.Generic;
using System.Reflection;

public partial class InkLabel : Control
{
	[Export]
	private bool fadeInText = true;
	[Export]
	private bool writeInText = false;
	[Export]
	private float fadeInSpeed = 0.5f;
	[Export]
	private RichTextLabel textLabel;
	[Export]
	private PanelContainer container;

	private const string c_changeThemeTag = "SET_THEME:";

	private List<string> attachedTags = new List<string> { };
	public virtual void Init(string text, List<string> tags = default)
	{
		Text = text;
		if ((fadeInText || writeInText) && WeakRef(this) != null)
		{
			AnimateLabel();
		}
		attachedTags = tags;
		GD.Print("Attached tags count: " + attachedTags.Count);
		if (attachedTags != null && attachedTags.Count > 0)
		{
			foreach (string tag in attachedTags)
			{
				GD.Print("Label: querying tag " + tag);
				if (tag.Contains(c_changeThemeTag))
				{
					SetTheme(tag);
				}
			}
		}
	}
	public string Text
	{
		get
		{
			return textLabel.Text;
		}
		set
		{
			textLabel.Text = value;
		}
	}
	public List<string> Tags
	{
		get
		{
			return attachedTags;
		}
		set
		{
			attachedTags = value;
		}
	}
	public virtual void SetTheme(string tag)
	{
		GD.Print("Setting theme on label to " + tag);
		string cleanedTag = tag.Replace(c_changeThemeTag, "");
		StyleBox newTheme = GlobalVariables.GetStyleBox(cleanedTag);
		if (newTheme != null)
		{
			container.AddThemeStyleboxOverride("Panel", newTheme);
			container.Set("theme_override_styles/panel", newTheme);
		}
	}
	public virtual void AnimateLabel()
	{

		if (fadeInText)
		{
			textLabel.SelfModulate = new Color(textLabel.Modulate, 0f);
			Tween tween = textLabel.CreateTween();
			tween.TweenProperty(textLabel, "self_modulate:a", 1f, 0.55f).SetTrans(Tween.TransitionType.Sine);
		}
		if (writeInText)
		{
			float fadeInTime = textLabel.Text.Length / (fadeInSpeed * 100f);
			textLabel.VisibleRatio = 0f;
			Tween tween2 = textLabel.CreateTween();
			tween2.TweenProperty(textLabel, "visible_ratio", 1f, fadeInTime).SetTrans(Tween.TransitionType.Sine);
		}
	}
	public override void _Ready()
	{
		if (fadeInText)
		{
			AnimateLabel();
		}
		GlobalEvents.SendOnUIShow(new UIEventArgs { targetNode = this });
		base._Ready();
	}

	private bool active_ = true;
	public bool Active
	{
		get
		{
			return active_;
		}
		set
		{
			if (active_ != value)
			{
				FadeIn(value);
				active_ = value;
			}
		}
	}
	void SetActive(bool active)
	{
		//GD.Print("We are setting ink writer to active: " + active);
		Visible = active;
	}

	public override GodotObject _MakeCustomTooltip(string text)
	{
		var scene = GD.Load<PackedScene>(GlobalVariables.c_inkTooltipScene);
		InkTooltip tooltip = scene.Instantiate<InkTooltip>();
		tooltip.textLabel.Text = text.Trim();
		return tooltip;
	}

	Tween _tween;
	void FadeIn(bool fadeIn)
	{
		if (_tween != null)
		{
			_tween.Kill();
		}
		_tween = GetTree().CreateTween();
		_tween.SetTrans(Tween.TransitionType.Sine);
		_tween.SetEase(Tween.EaseType.InOut);
		float goal = fadeIn ? 1f : 0f;
		float current = Modulate.A;
		if (fadeIn)
		{
			SetActive(true);
		}
		_tween.TweenProperty(GetNode(GetPath()), "modulate:a", goal, 0.25f).SetTrans(Tween.TransitionType.Sine);
		if (!fadeIn)
		{
			_tween.TweenCallback(Callable.From(() => SetActive(fadeIn)));
		}
	}
}
