using Godot;
using System;
using System.Collections.Generic;
using GodotInk;
using MiTale;

public partial class InkItemList : ItemList
{
	[Export]
	private string inventoryStackVariable;
	public Dictionary<string, int> InventoryDictionary = new Dictionary<string, int> { };

	[Export] private string c_curItemVar = "curItem";
	[Export] private string c_itemsInventory = "items";
	[Export] private string c_cancelItemName = "None";
	private const string c_getdisplaynamefunction = "GetDisplayName";
	private const string c_getdescriptionFunction = "GetDescription";
	private string currentSelectedItem = "";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Inkwriter.instance.Story.ObserveVariable(inventoryStackVariable, Callable.From((string varname, Variant newval) => VariableChanged((string)newval)));
		ItemSelected += (long newval) => SetSelectedItem((int)newval);
	}

	public void VariableChanged(string newval)
	{
		GD.Print("New value is: " + newval);
		List<KeyValuePair<string, string>> dict = InkArrays.DeSerializeProtoDictionary(newval);

		InventoryDictionary.Clear();
		Clear();
		bool hasSelectedItem = false;
		if (dict.Count == 0)
		{
			SetSelectedItem(-1);
		}
		foreach (KeyValuePair<string, string> kvp in dict)
		{
			if (InventoryDictionary.TryGetValue(kvp.Key, out int index))
			{
				EditInventoryItem(index, GetDisplayName(kvp.Key) + " (" + kvp.Value + ")");
			}
			else
			{
				NewInventoryItem(kvp.Key, GetDisplayName(kvp.Key) + (kvp.Value != "-1" ? " (" + kvp.Value + ")" : ""));
			}
			if (kvp.Key == currentSelectedItem)
			{
				Select(InventoryDictionary[kvp.Key]);
				hasSelectedItem = true;
			}
		}
		NewInventoryItem(c_cancelItemName, GetDisplayName(c_cancelItemName));
		if (!hasSelectedItem)
		{
			SetSelectedItem(-1);
		}

	}

	public virtual string GetDisplayName(string id)
	{
		string textOutput;
		Ink.Runtime.InkList list = new Ink.Runtime.InkList(c_itemsInventory, Inkwriter.instance.Story.runtimeStory);
		//list.SetInitialOriginName("items");
		list.AddItem(id);
		var returnValue = Inkwriter.instance.Story.runtimeStory.EvaluateFunction(c_getdisplaynamefunction, out textOutput, new object[] { list });
		string noBBCode = GlobalVariables.RemoveBBCode((string)returnValue);
		return noBBCode;
	}
	public virtual string GetDescription(string id)
	{
		string textOutput;
		Ink.Runtime.InkList list = new Ink.Runtime.InkList(c_itemsInventory, Inkwriter.instance.Story.runtimeStory);
		//list.SetInitialOriginName("items");
		list.AddItem(id);
		var returnValue = Inkwriter.instance.Story.runtimeStory.EvaluateFunction(c_getdescriptionFunction, out textOutput, new object[] { list });
		GD.Print("InkItemList: received GetDescription of item with id " + id + ": " + (string)returnValue);
		return (string)returnValue;
	}

	public override GodotObject _MakeCustomTooltip(string text)
	{
		var scene = GD.Load<PackedScene>(GlobalVariables.c_inkTooltipScene);
		InkTooltip tooltip = scene.Instantiate<InkTooltip>();
		tooltip.SetTooltipText(text);
		//tooltip.textLabel.Text = ""; // null previous text
		//tooltip.textLabel.AppendText(text);
		return tooltip;
	}

	public virtual void NewInventoryItem(string id, string itemName, Texture2D icon = null, bool selectable = true)
	{
		if (!InventoryDictionary.ContainsKey(id))
		{
			int index = AddItem(itemName, icon, selectable);
			SetItemTooltip(index, GetDescription(id));
			InventoryDictionary.Add(id, index);
		}
	}
	public virtual void EditInventoryItem(int index, string itemName, Texture2D icon = null, bool selectable = true)
	{
		SetItemText(index, itemName);
		SetItemIcon(index, icon);
		SetItemSelectable(index, selectable);
	}
	public virtual void SetSelectedItem(int index)
	{
		string id = c_cancelItemName;
		if (InventoryDictionary.ContainsValue(index))
		{
			foreach (string keyVar in InventoryDictionary.Keys)
			{
				if (InventoryDictionary[keyVar] == index)
				{
					id = keyVar;
				}
			}
		}

		Ink.Runtime.InkList list = new Ink.Runtime.InkList(c_itemsInventory, Inkwriter.instance.Story.runtimeStory);
		//list.SetInitialOriginName("items");
		list.AddItem(id);

		Inkwriter.instance.Story.runtimeStory.variablesState[c_curItemVar] = list;
		currentSelectedItem = id;
		GD.Print("Selected item is: " + id);
	}
}
