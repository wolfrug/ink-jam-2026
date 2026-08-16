using Godot;
using Ink.Parsed;
using System;
using System.Threading.Tasks;

public partial class InkTooltip : Node
{
    [Export] public RichTextLabel textLabel;

    public void SetTooltipText(string text)
    {
        textLabel.Text = text;
    }

}
