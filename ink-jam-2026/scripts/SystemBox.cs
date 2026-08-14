using Godot;
using System;

public partial class SystemBox : InkUINode
{
	public override void Activate(string tag)
	{
		base.Activate(tag);
		FadeIn(true);
	}
	public override void Deactivate(string tag)
	{
		FadeIn(false);
		base.Deactivate(tag);
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
			Visible = true;
		}
		_tween.TweenProperty(GetNode(GetPath()), "modulate:a", goal, 0.25f);
		if (!fadeIn)
		{
			_tween.TweenCallback(Callable.From(() => Visible = fadeIn));
		}
	}


}
