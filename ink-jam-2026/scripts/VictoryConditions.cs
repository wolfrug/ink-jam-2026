using Godot;
using System;
using GodotInk;
using MiTale;
using System.Globalization;

public partial class VictoryConditions : InkUINode
{
    [Export] public ProgressBar temperatureBar;
    [Export] public RichTextLabel temperatureText;
    [Export] public ProgressBar moraleBar;
    [Export] public ProgressBar hullBar;

    private const string c_temperatureVariable = "temperature";
    private const string c_moraleVariable = "morale";
    private const string c_hullVariable = "hull";

    public override void _Ready()
    {
        Inkwriter.instance.Story.ObserveVariable(c_temperatureVariable, Callable.From((string varname, Variant newval) => TemperatureChanged((string)newval)));
        Inkwriter.instance.Story.ObserveVariable(c_moraleVariable, Callable.From((string varname, Variant newval) => MoraleChanged((string)newval)));
        Inkwriter.instance.Story.ObserveVariable(c_hullVariable, Callable.From((string varname, Variant newval) => HullChanged((string)newval)));
        temperatureBar.TooltipText = "The current temperature in the ship. If it reaches 50+ or -20 degrees, the ship is considered unliveable";
        moraleBar.TooltipText = "The current Morale in the ship. If this reaches 0%, it means mutiny!";
        hullBar.TooltipText = "The current Hull Integrity of the ship. If this reaches 0%, the ship is destroyed.";
    }

    public override void Activate(string tag)
    {
        base.Activate(tag);
        Visible = true;
    }
    public override void Deactivate(string tag)
    {
        base.Deactivate(tag);
        Visible = false;
    }


    private void HullChanged(string newval)
    {
        hullBar.Value = int.Parse(newval);
    }


    private void MoraleChanged(string newval)
    {
        moraleBar.Value = int.Parse(newval);
    }


    private void TemperatureChanged(string newval)
    {
        temperatureBar.Value = float.Parse(newval, CultureInfo.InvariantCulture);
        temperatureText.Text = string.Format("Temperature: {0:F2}°C", temperatureBar.Value);
    }

}
