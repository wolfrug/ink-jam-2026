using Godot;
using GodotInk;
using System;
using System.Collections.Generic;
using MiTale;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot.NativeInterop;

public partial class Inkwriter : CanvasLayer
{
	[Export]
	private InkStory story;

	[Export]
	private VBoxContainer storyTextContainer;

	[Export]
	private Control mainPanel;

	[Export]
	private bool startOnReady = true;
	[Export]
	private bool loadOnReady = true;
	[Export]
	private string startKnot = "start";
	[Export]
	private string customFlow = "";

	[Export]
	private bool continueMaximally = true;

	[Export(PropertyHint.InputName, "show_builtin")] string continueInput = "ui_left";
	private bool awaitingClick = false;

	private bool skippingAwait = false;

	private string fullStoryText = "";

	private List<Button> currentChoices = new List<Button> { };

	protected string saveFilePath = "user://clay_savegame.save";

	protected const string c_customButtonSceneTag = "SET_CUSTOM_BUTTON:";

	protected const string c_customLabelSceneTag = "SET_CUSTOM_LABEL:";

	//private InkArrayFunctions _arrayFunctions;

	// Called when the node enters the scene tree for the first time.
	private static Inkwriter _instance;
	public static Inkwriter instance
	{
		get
		{
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}
	public override void _Ready()
	{
		if (instance == null)
		{
			instance = this;
		}
		var newchild = GD.Load<PackedScene>("res://addons/InkEngine/Scenes/InkArrayFunctions.tscn");
		InkArrayFunctions newFunc = newchild.Instantiate<InkArrayFunctions>();
		AddChild(newFunc);
		newFunc.Init(story);
		if (customFlow != "")
		{
			story.SwitchFlow(customFlow);
		}
		if (loadOnReady)
		{
			LoadGame();
		}
		if (startOnReady)
		{
			Play(startKnot);
		}
	}
	public InkStory Story { get { return story; } }

	public override void _EnterTree()
	{
		GlobalEvents.OnChoiceSelected += GlobalEvent_OnChoiceSelected;
		GlobalEvents.OnGotoKnot += GlobalEvent_OnGoToKnot;
		GlobalEvents.OnRequestSetVariable += GlobalEvent_OnRequestSetVariable;
		base._EnterTree();
	}
	public override void _ExitTree()
	{
		GlobalEvents.OnChoiceSelected -= GlobalEvent_OnChoiceSelected;
		GlobalEvents.OnGotoKnot -= GlobalEvent_OnGoToKnot;
		GlobalEvents.OnRequestSetVariable -= GlobalEvent_OnRequestSetVariable;
		base._ExitTree();
	}

	public void Play(string knot)
	{
		story.ChoosePathString(knot);
		ContinueStory();
	}

	private async void ContinueStory()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame); // We always wait one frame here for...reasons.
		fullStoryText = "";
		if (continueMaximally || !Active) // We also continue maximally if the writer is hidden
		{
			while (story.CanContinue)
			{
				string storyText = story.Continue();
				storyText = storyText.Trim();
				if (story.CurrentTags.Count > 0)
				{
					GlobalEvents.SendOnTagsFound(new InkEventArgs { inkTags = story.CurrentTags });
				}
				Task waitTask = ParseTags();
				await waitTask;
				InkLabel content = AddText(storyText);
				storyTextContainer.AddChild(content);
				content.Init(storyText, story.CurrentTags as List<string>);
				GlobalEvents.SendOnLabelCreated(new InkEventArgs { inkTextLabel = content, inkTags = story.CurrentTags, inktext = storyText });
				//GD.Print("Tag count for label with text: " + storyText + " : " + story.GetCurrentTags().Count);

				fullStoryText += storyText;
			}
		}
		else
		{
			if (story.CanContinue)
			{
				string storyText = story.Continue();
				storyText = storyText.Trim();
				if (story.CurrentTags.Count > 0)
				{
					GlobalEvents.SendOnTagsFound(new InkEventArgs { inkTags = story.CurrentTags });
				}
				Task waitTask = ParseTags();
				await waitTask;
				fullStoryText += storyText;
				if (!string.IsNullOrWhiteSpace(storyText.Trim()))
				{
					InkLabel content = AddText(storyText);
					storyTextContainer.AddChild(content);
					content.Init(storyText, story.CurrentTags as List<string>);
					GlobalEvents.SendOnLabelCreated(new InkEventArgs { inkTextLabel = content, inkTags = story.CurrentTags, inktext = storyText });
					GD.Print("Tag count for label with text: " + storyText + " : " + story.GetCurrentTags().Count);
					if (!skippingAwait)
					{
						awaitingClick = true;
					}
					else
					{
						skippingAwait = false;
						ContinueStory();
					}
				}
				else
				{
					ContinueStory();
				}
			}
		}
		GlobalEvents.SendOnContinue(new InkEventArgs { inktext = fullStoryText });
		//storyText = story.ContinueMaximally();

		if (!story.CanContinue)
		{
			GatherChoices();
		}
	}

	private async Task ParseTags()
	{
		foreach (string tag in story.CurrentTags)
		{
			if (tag.Contains("wait"))
			{
				float waitTime = float.Parse(tag.Replace("wait.", ""));
				waitTime *= 1000; // convert to milliseconds
				await Task.Delay((int)waitTime);
			}
			if (tag.Contains("clear"))
			{
				ClearAll();
			}
			if (tag.Contains("saveGame"))
			{
				SaveGame();
			}
			if (tag.Contains("loadGame"))
			{
				LoadGame();
			}
			if (tag.Contains("quitGame"))
			{
				QuitGame();
			}
			if (tag.Contains("hideWriter"))
			{
				Active = false;
			}
			if (tag.Contains("showWriter"))
			{
				Active = true;
			}
			if (tag.Contains("continue"))
			{
				skippingAwait = true;
			}
		}
	}

	private void GatherChoices()
	{
		awaitingClick = false;
		List<(InkChoice, Button)> allChoices = new List<(InkChoice, Button)> { };
		List<string> allTags = new List<string> { };
		GD.Print("Ink Writer: Gathering choices count is: " + story.CurrentChoices.Count);
		foreach (InkChoice choice in story.CurrentChoices)
		{
			Button addedButton = AddButton(choice);
			allChoices.Add((choice, addedButton));
			allTags.AddRange(choice.Tags);
		}
		GlobalEvents.SendOnContinueFinished(new InkEventArgs { inkChoices = allChoices, inkTags = allTags });
	}

	private Button AddButton(InkChoice choice)
	{
		InkButton button = GetButton(choice);
		button.Init(choice);
		button.Pressed += delegate
		{
			GlobalEvents.SendOnChoiceSelected(new InkEventArgs { inktext = choice.Text, inkTags = choice.Tags as List<string>, inkchoice = choice, inkChoiceButton = button });
		};
		currentChoices.Add(button);
		storyTextContainer.AddChild(button);
		return button;
	}

	private InkButton GetButton(InkChoice choice)
	{
		string buttonPath = GlobalVariables.c_inkButtonScene;
		foreach (string tag in choice.Tags)
		{
			if (tag.Contains(c_customButtonSceneTag))
			{
				buttonPath = tag.Replace(c_customButtonSceneTag, "").Trim();
				GD.Print("Loading custom button from path " + buttonPath);
			}
		}

		var scene = GD.Load<PackedScene>(buttonPath);
		InkButton button = scene.Instantiate<InkButton>();

		return button;
	}

	private InkLabel AddText(string text)
	{
		var scene = GD.Load<PackedScene>(GlobalVariables.c_inkLabelScene);
		InkLabel label = scene.Instantiate<InkLabel>();
		return label;
	}

	private void GlobalEvent_OnChoiceSelected(InkEventArgs args)
	{
		ChooseChoice(args.inkchoice, args.inkChoiceButton);
	}

	private void ChooseChoice(InkChoice choice, Button button)
	{
		currentChoices.RemoveAll((x) => x == null);
		if (currentChoices.Count > 0)
		{
			foreach (Button btn in currentChoices)
			{
				if (WeakRef(btn) != null && btn != button)
				{
					btn.QueueFree();
				}
			}
			if (button != null)
			{
				button.Disabled = true;
			}
		}
		currentChoices.Clear();
		foreach (InkChoice availableChoice in story.CurrentChoices)
		{
			if (availableChoice.Index == choice.Index)
			{
				story.ChooseChoiceIndex(choice.Index);
				ContinueStory();
				break;
			}
		}
	}

	private void GlobalEvent_OnGoToKnot(InkEventArgs args)
	{
		Play(args.inktext);
	}
	private void GlobalEvent_OnRequestSetVariable(InkEventArgs args)
	{
		GD.Print("Ink Writer: Received request to set variable, setting variable " + args.targetVariable + " to " + args.newValue);
		story.StoreVariable(args.targetVariable, args.newValue);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed(continueInput))
		{
			if (awaitingClick && !continueMaximally)
			{
				GD.Print("Continuing on click");
				awaitingClick = false;
				ContinueStory();
			}
		}
	}

	public void ClearAll()
	{
		foreach (Node child in storyTextContainer.GetChildren())
		{
			child.QueueFree();
		}
	}

	public void SaveGame()
	{
		string json = story.SaveState();
		using var saveFile = FileAccess.Open(saveFilePath, FileAccess.ModeFlags.Write);
		saveFile.StoreLine(json);
	}
	public void LoadGame()
	{
		if (!FileAccess.FileExists(saveFilePath))
		{
			return; // Error! We don't have a save to load.
		}
		using var saveFile = FileAccess.Open(saveFilePath, FileAccess.ModeFlags.Read);
		var jsonString = saveFile.GetLine();
		story.LoadState(jsonString);
	}
	public void QuitGame()
	{
		GetTree().Quit();
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
				if (value)
				{
					GlobalEvents.SendOnShowWriter(new InkEventArgs { });
				}
				else
				{
					GlobalEvents.SendOnHideWriter(new InkEventArgs { });
				}
			}
		}
	}
	void SetActive(bool active)
	{
		GD.Print("We are setting ink writer to active: " + active);
		mainPanel.Visible = active;
	}
	void ShowChildren(bool show)
	{
		foreach (Node child in storyTextContainer.GetChildren())
		{
			child.Set("Active", show);
		}
	}

	Tween _tween;
	void FadeIn(bool fadeIn)
	{
		if (_tween != null)
		{
			_tween.Kill();
		}
		_tween = mainPanel.CreateTween();
		_tween.SetTrans(Tween.TransitionType.Sine);
		_tween.SetEase(Tween.EaseType.InOut);
		var mat = mainPanel.Material as ShaderMaterial;
		float current = mat.GetShaderParameter("progress").AsSingle();
		float goal = fadeIn ? 1f : 0f;
		if (fadeIn)
		{
			SetActive(true);
		}
		else
		{
			ShowChildren(false);
		}
		_tween.TweenMethod(
			Callable.From<float>(v => mat.SetShaderParameter("progress", v)),
			current,
			goal,
			1.5f
		);
		if (!fadeIn)
		{
			_tween.TweenCallback(Callable.From(() => SetActive(fadeIn)));
		}
		else
		{
			_tween.TweenCallback(Callable.From(() => ShowChildren(fadeIn)));
		}
	}
}
