using Godot;
using GodotInk;
using System;
using System.Collections.Generic;


namespace MiTale
{
    public static class GlobalEvents
    {
        public delegate void InkDialogueEvent(InkEventArgs eventArgs);

        public static event InkDialogueEvent OnContinue;
        public static event InkDialogueEvent OnContinueFinished;
        public static event InkDialogueEvent OnChoiceSelected;
        public static event InkDialogueEvent OnTagsFound;
        public static event InkDialogueEvent OnLabelCreated;
        public static event InkDialogueEvent OnGotoKnot;
        public static event InkDialogueEvent OnShowWriter;
        public static event InkDialogueEvent OnHideWriter;
        public static event InkDialogueEvent OnRequestSetVariable;


        public static void SendOnContinue(InkEventArgs eventArgs)
        {
            OnContinue?.Invoke(eventArgs);
        }
        public static void SendOnContinueFinished(InkEventArgs eventArgs)
        {
            OnContinueFinished?.Invoke(eventArgs);
        }
        public static void SendOnChoiceSelected(InkEventArgs eventArgs)
        {
            OnChoiceSelected?.Invoke(eventArgs);
        }
        public static void SendOnTagsFound(InkEventArgs eventArgs)
        {
            OnTagsFound?.Invoke(eventArgs);
        }
        public static void SendOnLabelCreated(InkEventArgs eventArgs)
        {
            OnLabelCreated?.Invoke(eventArgs);
        }

        public static void SendOnGoToKnot(InkEventArgs eventArgs)
        {
            OnGotoKnot?.Invoke(eventArgs);
        }
        public static void SendOnShowWriter(InkEventArgs eventArgs)
        {
            OnShowWriter?.Invoke(eventArgs);
        }
        public static void SendOnHideWriter(InkEventArgs eventArgs)
        {
            OnHideWriter?.Invoke(eventArgs);
        }

        public static void SendOnRequestSetVariable(InkEventArgs eventArgs)
        {
            OnRequestSetVariable?.Invoke(eventArgs);
        }


        // UI Events
        public delegate void UIChangeEvent(UIEventArgs eventArgs);

        public static event UIChangeEvent OnUIShow;
        public static event UIChangeEvent OnUIHide;

        public static void SendOnUIShow(UIEventArgs eventArgs)
        {
            OnUIShow?.Invoke(eventArgs);
        }
        public static void SendOnUIHide(UIEventArgs eventArgs)
        {
            OnUIHide?.Invoke(eventArgs);
        }
    }

    public class InkEventArgs : EventArgs
    {
        public string inktext;
        public InkLabel inkTextLabel;
        public IReadOnlyList<string> inkTags;
        public List<(InkChoice, Button)> inkChoices = new List<(InkChoice, Button)> { }; // These are all the choices, if any, once Continue is done
        public InkChoice inkchoice; // This is the specific choice (and button) being clicked on etc
        public Button inkChoiceButton;
        public string targetVariable; // This is for requesting a variable change
        public Variant newValue; // this is the new value
    }

    // Other event types
    public class UIEventArgs : EventArgs
    {
        public Node targetNode;
    }

}