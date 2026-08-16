using Godot;
using System;
using GodotInk;
using MiTale;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class RoomMachine : InkUINode
{
    [Export] private RichTextLabel taskName;
    [Export] private InkButton assignCrew;
    [Export] private RichTextLabel assignedCrew;
    [Export] private InkButton assignItem;
    [Export] private RichTextLabel assignedItem;

    [Export] private ProgressBar currentBar;
    [Export] private Slider targetSlider;
    [Export] private ProgressBar targetBar;
    [Export] private Slider playerSlider;

    [Export] private InkItemList characterList;
    [Export] private InkItemList itemList;
    [Export] private InkItemList taskList;

    private string currentTask = "";
    List<KeyValuePair<string, string>> deserializedDictionary = new List<KeyValuePair<string, string>>();

    private string crew_assigned_id = "";
    private string item_assigned_id = "";

    private const string c_defaultCrewText = "<No Crew Assigned>";
    private const string c_defaultItemText = "<No Item Assigned>";

    private const string c_assignCrewFunction = "AssignCrewReturn";
    private const string c_unassignCrewFunction = "UnassignCrewReturn";

    private const string c_assignItemFunction = "AssignItemReturn";
    private const string c_unassignItemFunction = "UnassignItemReturn";

    private const string c_getTaskDictionary = "GetTaskVariable";

    public override void _Ready()
    {
        taskList.ItemClicked += ChangeTask;
        assignedCrew.Text = c_defaultCrewText;
        assignedItem.Text = c_defaultItemText;
        playerSlider.ValueChanged += UpdateTargetBar;
        assignCrew.Pressed += AddOrRemoveWorker;
        assignItem.Pressed += AddOrRemoveItem;
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


    private void ChangeTask(long index, Vector2 atPosition, long mouseButtonIndex)
    {
        string id = taskList.GetIdByIndex((int)index);
        string taskString = (string)Inkwriter.instance.Story.runtimeStory.EvaluateFunction(c_getTaskDictionary, out string textOutput, new object[] { id });
        UpdateTask(taskString);
    }
    private void UpdateTask(string taskString)
    {
        GD.Print("RoomMachine: Received task string: " + taskString);

        if (taskString != "")
        {
            //TaskId, TaskName, TaskVariable, TaskCurState, TaskMinimumState, TaskPlayerSetState, TaskWorker, TaskItem, TaskItemRequirement, TaskWorkerRequirement
            // Set up task
            deserializedDictionary = InkArrays.DeSerializeProtoDictionary(taskString);
            string workerId = deserializedDictionary.Find((x) => x.Key == "TaskWorker").Value;
            crew_assigned_id = workerId;
            assignedCrew.Text = characterList.GetDisplayName(workerId);
            string itemId = deserializedDictionary.Find((x) => x.Key == "TaskItem").Value;
            item_assigned_id = itemId;
            assignedItem.Text = itemList.GetDisplayName(itemId);
            currentTask = deserializedDictionary.Find((x) => x.Key == "TaskVariable").Value;
            taskName.Text = deserializedDictionary.Find((x) => x.Key == "TaskName").Value;
            currentBar.Value = int.Parse(deserializedDictionary.Find((x) => x.Key == "TaskCurState").Value);
            targetSlider.Value = int.Parse(deserializedDictionary.Find((x) => x.Key == "TaskMinimumState").Value);
            playerSlider.Editable = true;
            playerSlider.Value = int.Parse(deserializedDictionary.Find((x) => x.Key == "TaskPlayerSetState").Value);
            assignCrew.Disabled = false;
            assignItem.Disabled = false;
            Activate("");
        }
        else
        {
            SaveToTaskVariable();
            playerSlider.Editable = false;
            currentTask = "";
            assignedCrew.Text = c_defaultCrewText;
            crew_assigned_id = characterList.c_cancelItemName;
            assignedItem.Text = c_defaultItemText;
            item_assigned_id = itemList.c_cancelItemName;
            taskName.Text = "<Select a Task>";
            currentBar.Value = 0;
            targetSlider.Value = 0;
            playerSlider.Value = 0;
            assignCrew.Disabled = true;
            assignItem.Disabled = true;
            Deactivate("");
        }
    }
    private void SaveToTaskVariable()
    {
        if (currentTask != "")
        {
            GD.Print("RoomMachine: saving to task variable: " + item_assigned_id + crew_assigned_id);
            deserializedDictionary.RemoveAll(x => x.Key == "TaskPlayerSetState");
            deserializedDictionary.Add(new KeyValuePair<string, string>("TaskPlayerSetState", playerSlider.Value.ToString()));

            deserializedDictionary.RemoveAll(x => x.Key == "TaskWorker");
            deserializedDictionary.Add(new KeyValuePair<string, string>("TaskWorker", crew_assigned_id));

            deserializedDictionary.RemoveAll(x => x.Key == "TaskItem");
            deserializedDictionary.Add(new KeyValuePair<string, string>("TaskItem", item_assigned_id));

            string serializedDictionary = InkArrays.SerializeProtoDictionary(deserializedDictionary);
            GD.Print("RoomMachine: saving serialized dictionary " + serializedDictionary + " to variable " + currentTask);
            Inkwriter.instance.Story.runtimeStory.variablesState[currentTask] = serializedDictionary;
        }
    }


    void UpdateTargetBar(double valueChange)
    {
        targetBar.Value = valueChange;
    }

    void AssignWorker(string id)
    {
        if (id == characterList.c_cancelItemName)
        {
            UnassignWorker();
        }
        else
        {
            string returnVal = (string)Inkwriter.instance.Story.runtimeStory.EvaluateFunction(c_assignCrewFunction, out string textOutput, new object[] { id, InkArrays.SerializeProtoDictionary(deserializedDictionary) });
            GD.Print("RoomMachine: received this as return val after assignworker: " + returnVal);
            deserializedDictionary = InkArrays.DeSerializeProtoDictionary(returnVal);
            crew_assigned_id = id;
            assignedCrew.Text = characterList.GetDisplayName(id);

        }
        SaveToTaskVariable();
    }
    void UnassignWorker()
    {
        if (crew_assigned_id != characterList.c_cancelItemName)
        {
            string returnVal = (string)Inkwriter.instance.Story.runtimeStory.EvaluateFunction(c_unassignCrewFunction, out string textOutput, new object[] { InkArrays.SerializeProtoDictionary(deserializedDictionary) });
            deserializedDictionary = InkArrays.DeSerializeProtoDictionary(returnVal);
        }
        assignedCrew.Text = c_defaultCrewText;
        crew_assigned_id = characterList.c_cancelItemName;
        SaveToTaskVariable();
    }

    void AssignItem(string id)
    {
        GD.Print("RoomMachine: Attempting to assign item with ID " + id);
        if (id == itemList.c_cancelItemName)
        {
            UnAssignItem();
        }
        else
        {
            string returnVal = (string)Inkwriter.instance.Story.runtimeStory.EvaluateFunction(c_assignItemFunction, out string textOutput, new object[] { id, InkArrays.SerializeProtoDictionary(deserializedDictionary) });
            deserializedDictionary = InkArrays.DeSerializeProtoDictionary(returnVal);
            GD.Print("RoomMachine: received this as return val after assignitem: " + returnVal);
            item_assigned_id = id;
            assignedItem.Text = itemList.GetDisplayName(id);
        }
        SaveToTaskVariable();
    }
    void UnAssignItem()
    {
        if (item_assigned_id != itemList.c_cancelItemName)
        {
            string returnVal = (string)Inkwriter.instance.Story.runtimeStory.EvaluateFunction(c_unassignItemFunction, out string textOutput, new object[] { InkArrays.SerializeProtoDictionary(deserializedDictionary) });
            deserializedDictionary = InkArrays.DeSerializeProtoDictionary(returnVal);
            GD.Print("RoomMachine: received this as return val after unassignitem: " + returnVal);
        }
        assignedItem.Text = c_defaultItemText;
        item_assigned_id = itemList.c_cancelItemName;
        SaveToTaskVariable();
    }

    void AddOrRemoveWorker()
    {
        GD.Print("Pressed add or remove worker");
        if (characterList.currentSelectedItem == characterList.c_cancelItemName)
        {
            UnassignWorker();
        }
        else
        {
            string selectedItem = characterList.currentSelectedItem;
            AssignWorker(selectedItem);
        }
    }
    void AddOrRemoveItem()
    {
        GD.Print("Pressed add or remove item");
        if (itemList.currentSelectedItem == itemList.c_cancelItemName)
        {
            UnAssignItem();
        }
        else
        {
            string selectedItem = itemList.currentSelectedItem;
            AssignItem(selectedItem);
        }
    }


}
