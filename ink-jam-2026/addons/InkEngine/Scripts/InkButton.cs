using Godot;
using GodotInk;
using System;
using System.Collections.Generic;
using MiTale;

public partial class InkButton : Button
{
	[Export]
	public string onClickedSFX = "PLAY_SFX:Audio/SFX/button_click.wav";
	[Export]
	public string onHoverSFX = "PLAY_SFX:Audio/SFX/button_hover.wav";
	protected const string c_tagIcon = "SET_ICON:";
	protected const string c_disabledIcon = "DISABLED";
	protected const string c_changeThemeTag = "SET_THEME:";
	public Action OnPressedDelegate;

	public override void _Ready()
	{
		Pressed += delegate
		{
			InkAudioPlayer.instance?.PlayAudioStream(onClickedSFX);
		};
		MouseEntered += delegate
		{
			InkAudioPlayer.instance?.PlayAudioStream(onHoverSFX);
		};
		GlobalEvents.SendOnUIShow(new UIEventArgs { targetNode = this });
		base._Ready();
	}

	public virtual void Init(InkChoice choice)
	{
		//GD.Print("Initing ink button");
		Text = choice.Text;
		ParseTags(choice);
		if (choice.Tags.Count > 0)
		{
			GlobalEvents.SendOnTagsFound(new InkEventArgs { inkTags = choice.Tags, inkchoice = choice, inkChoiceButton = this });
		}
	}
	protected virtual void ParseTags(InkChoice choice)
	{
		foreach (string tag in choice.Tags)
		{
			if (tag.Contains(c_tagIcon))
			{
				SetIcon(tag);
			}
			if (tag.Contains(c_disabledIcon))
			{
				Disabled = true;
			}
			if (tag.Contains(c_changeThemeTag))
			{
				SetTheme(tag);
			}
		}
	}

	protected virtual void SetIcon(string tag)
	{
		GD.Print("Setting icon with tag " + tag);
		string iconId = tag.Replace(c_tagIcon, "");
		Texture2D texture = GlobalVariables.GetIconTexture2D(iconId);
		if (texture != null)
		{
			Icon = texture;
		}
		else
		{
			GD.Print("No icon with ID " + tag + " found - nulling");
			Icon = null;
		}
	}
	public virtual void SetTheme(string tag)
	{
		GD.Print("Setting theme on label to " + tag);
		string cleanedTag = tag.Replace(c_changeThemeTag, "");
		StyleBox newTheme = GlobalVariables.GetStyleBox(cleanedTag);
		if (newTheme != null)
		{
			AddThemeStyleboxOverride("Normal", newTheme);
			Set("theme_override_styles/normal", newTheme);
		}
	}
	public virtual void SendPressedSignal()
	{
		EmitSignal(SignalName.Pressed);
	}
	public virtual void RemoveCustomAction()
	{
		if (OnPressedDelegate != null)
		{
			Pressed -= OnPressedDelegate;
			OnPressedDelegate = null;
		}
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
