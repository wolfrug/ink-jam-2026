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

	private const string c_curItemVar = "curItem";

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

		foreach (KeyValuePair<string, string> kvp in dict)
		{
			if (InventoryDictionary.TryGetValue(kvp.Key, out int index))
			{
				EditInventoryItem(index, kvp.Key + " (" + kvp.Value + ")");
			}
			else
			{
				NewInventoryItem(kvp.Key, kvp.Key + " (" + kvp.Value + ")");
			}
		}
	}

	public virtual void NewInventoryItem(string id, string itemName, Texture2D icon = null, bool selectable = true)
	{
		int index = AddItem(itemName, icon, selectable);
		InventoryDictionary.Add(id, index);
	}
	public virtual void EditInventoryItem(int index, string itemName, Texture2D icon = null, bool selectable = true)
	{
		SetItemText(index, itemName);
		SetItemIcon(index, icon);
		SetItemSelectable(index, selectable);
	}
	public virtual void SetSelectedItem(int index)
	{
		string id = "";
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
		Inkwriter.instance.Story.StoreVariable(c_curItemVar, id);

		GD.Print("Selected item is: " + id);
	}
}
