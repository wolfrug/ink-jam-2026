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
        LateUpdate();
    }
    public async Task LateUpdate()
    {
        await Task.Delay(1000);
        GD.Print("Executing tooltip task with text " + textLabel.Text);
        textLabel.Size = Vector2.Zero;
    }

}
