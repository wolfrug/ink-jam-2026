using Godot;
using System;
using GodotInk;
using MiTale;
using System.Collections.Generic;

public partial class InkBackgroundSprite2D : Sprite2D
{
    [Export]
    public Camera2D camera;
    [Export]
    public bool use2DCamera = true;

    [Export]
    public Godot.Collections.Dictionary<string, Node2D> presetCameraLocations = new Godot.Collections.Dictionary<string, Node2D> { };

    private const string c_setBackground = "SET_SPRITEBACKGROUND:";
    private const string c_moveCamera = "SET_CAMERA_TARGET:";

    private const char c_delimiter = '^';
    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
        GlobalEvents.OnTagsFound += GlobalEvent_OnTagsFound;
        base._EnterTree();
    }
    public override void _ExitTree()
    {
        GlobalEvents.OnTagsFound -= GlobalEvent_OnTagsFound;
        base._ExitTree();
    }
    public override void _Ready()
    {
        if (use2DCamera)
        {
            camera.MakeCurrent();
        }
        base._Ready();
    }


    void GlobalEvent_OnTagsFound(InkEventArgs args)
    {
        //GD.Print("Looking for background tags");
        foreach (string tag in args.inkTags)
        {
            //GD.Print("looking for " + c_setBackground + " in tag " + tag);
            if (tag.Contains(c_setBackground))
            {
                SetBackground(tag);
            }
            if (tag.Contains(c_moveCamera))
            {
                MoveCamera(tag);
            }
        }
    }

    public void MoveCamera(string tag)
    {// Use: #SET_CAMERA_TARGET:45,-74^0.5^1 -> coordinates, zoom value, time
        string id = tag.Replace(c_moveCamera, "");
        string[] stringParts = id.Split(c_delimiter);
        GD.Print("Received SET CAMERA TARGET coords, with parts: " + id);
        string[] coordinates = stringParts[0].Split(',');
        Vector2 coords = Vector2.Zero;
        if (coordinates.Length == 1)
        {
            coords = presetCameraLocations[coordinates[0]].Position;
        }
        else
        {
            coords = new Vector2(coordinates[0].ToFloat(), coordinates[1].ToFloat());
        }
        GD.Print("Getting zoom value out of string " + stringParts[1]);
        float zoomValue = stringParts[1].ToFloat();
        float time = stringParts[2].ToFloat();

        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(camera, PropertyName.Position.ToString(), coords, time).SetTrans(Tween.TransitionType.Sine);
        tween.SetParallel(true);
        tween.TweenProperty(camera, "zoom", new Vector2(zoomValue, zoomValue), time).SetTrans(Tween.TransitionType.Sine);
    }

    Tween _tween;
    void FadeTexture(Texture2D tex)
    {
        if (_tween != null || _tween.IsRunning()) { _tween.Kill(); }
        ;
        _tween = CreateTween();
        //_tween.SetParallel(true); // lets each child's tween run concurrently, each with its own SetDelay below
        _tween.SetTrans(Tween.TransitionType.Sine);
        _tween.SetEase(Tween.EaseType.InOut);
        var mat = Material as ShaderMaterial;
        float current = mat.GetShaderParameter("progress").AsSingle();

        _tween.TweenMethod(
            Callable.From<float>(v => mat.SetShaderParameter("progress", v)),
            current,
            0f,
            1.5f
        );
        _tween.TweenCallback(Callable.From(() => SetTexture2D(tex)));
        if (tex != null)
        {
            _tween.TweenMethod(
               Callable.From<float>(v => mat.SetShaderParameter("progress", v)),
               0f,
               1f,
               1.5f
           );
        }
        else
        {
            GD.Print("Faded out and removed texture from ink Background!");
        }
    }

    void SetBackground(string tag)
    {
        GD.Print("Setting background according to tag " + tag);
        if (use2DCamera)
        {
            camera.MakeCurrent();
        }
        string id = tag.Replace(c_setBackground, "");
        Texture2D tex = GlobalVariables.GetBackgroundTexture2D(id);
        if (tex != Texture)
        {
            FadeTexture(tex);
        }

        /*
        {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(GetNode(GetPath()), "modulate:a", 0f, 0.25f).SetTrans(Tween.TransitionType.Sine);
            tween.TweenCallback(Callable.From(() => SetTexture2D(tex)));
            tween.TweenProperty(GetNode(GetPath()), "modulate:a", 1f, 0.25f).SetTrans(Tween.TransitionType.Sine);
        }
        else
        {
            GD.Print("No Background found: nulling");
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(GetNode(GetPath()), "modulate:a", 0f, 0.25f).SetTrans(Tween.TransitionType.Sine);
        }*/
    }
    public void SetTexture2D(Texture2D texture)
    {
        Texture = texture;
    }
}
