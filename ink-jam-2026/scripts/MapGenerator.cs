using Godot;
using System;
using GodotInk;
using MiTale;

public partial class MapGenerator : InkUINode
{
	[Export]
	public InkUI inkUI;
	[Export]
	public Node[] parents;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() // Automatically fills up list with named children
	{
		foreach (Node parent in parents)
		{
			foreach (Node child in parent.GetChildren())
			{
				if (child is InkButton)
				{
					inkUI.customButtons.Add(child.Name, child as InkButton);
				}
			}
		}
		// Also adds self!
		inkUI.customActivatorNodes.Add(Name, this);
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




}
