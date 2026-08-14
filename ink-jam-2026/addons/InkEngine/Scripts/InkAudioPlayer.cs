using Godot;
using MiTale;
using System;
using System.Collections.Generic;

public partial class InkAudioPlayer : AudioStreamPlayer
{
    public static InkAudioPlayer instance;
    private const string c_playMusicTag = "PLAY_MUSIC:";
    private const string c_playSFXTag = "PLAY_SFX:";
    private const string c_playAmbientTag = "PLAY_AMBIENCE:";
    private const char c_delimiter = '^';

    private List<AudioStreamPlayer> audioPlayerPool = new List<AudioStreamPlayer> { };

    public override void _EnterTree()
    {
        if (instance == null)
        {
            instance = this;
        }
        GlobalEvents.OnTagsFound += GlobalEvent_OnTagsFound;
        base._EnterTree();
    }
    public override void _ExitTree()
    {
        if (instance == this)
        {
            instance = null;
        }
        GlobalEvents.OnTagsFound -= GlobalEvent_OnTagsFound;
        base._ExitTree();
    }
    private void GlobalEvent_OnTagsFound(InkEventArgs args)
    {
        foreach (string tag in args.inkTags)
        {
            PlayAudioStream(tag);
        }
    }
    public void PlayAudioStream(string tag)
    {
        if (tag.Contains(c_playMusicTag))
        {
            PlayMusic(tag);
        }
        if (tag.Contains(c_playAmbientTag))
        {
            PlayAmbience(tag);
        }
        if (tag.Contains(c_playSFXTag))
        {
            PlaySFX(tag);
        }
    }

    public void PlayMusic(string tag)
    {
        //GD.Print("Looking to play music at location " + tag);
        tag = tag.Replace(c_playMusicTag, "");
        string[] stringParts = tag.Split(c_delimiter);
        string filePath = stringParts[0];
        AudioStream stream = LoadAudioStream(filePath);
        if (stream != null && Stream != stream)
        {
            PlayAudioStream(stream, true);
        }
    }

    public void PlaySFX(string tag)
    {
        //GD.Print("Looking to play sfx at location " + tag);
        tag = tag.Replace(c_playSFXTag, "");
        string[] stringParts = tag.Split(c_delimiter);
        string filePath = stringParts[0];
        AudioStream stream = LoadAudioStream(filePath);
        if (stream != null)
        {
            if (audioPlayerPool.Count > 0)
            {
                audioPlayerPool[0].Stream = stream;
                audioPlayerPool[0].Play();
                audioPlayerPool.RemoveAt(0);
            }
            else
            {
                AudioStreamPlayer newPlayer = new AudioStreamPlayer();
                Action finishedAction = () => audioPlayerPool.Add(newPlayer);
                AddChild(newPlayer);
                newPlayer.Finished += finishedAction;
                newPlayer.Stream = stream;
                newPlayer.Play();
            }
        }
    }

    public void PlayAmbience(string tag)
    {
        //GD.Print("Looking to play ambience at location " + tag);
        tag = tag.Replace(c_playAmbientTag, "");
        string[] stringParts = tag.Split(c_delimiter);
        string filePath = stringParts[0];
        AudioStream stream = LoadAudioStream(filePath);
        if (stream != null && Stream != stream)
        {
            PlayAudioStream(stream, true);
        }
    }

    private Action loopAction;
    private void PlayAudioStream(AudioStream stream, bool isLooping)
    {
        Stream = stream;
        Play();
        loopAction = () => Play();
        if (isLooping)
        {
            Finished += loopAction;
        }
        else
        {
            Finished -= loopAction;
        }
    }

    public AudioStream LoadAudioStream(string filePath)
    {
        filePath = "res://" + filePath;
        if (ResourceLoader.Exists(filePath))
        {
            AudioStream loadedStream = ResourceLoader.Load(filePath) as AudioStream;
            return loadedStream;
        }
        else
        {
            GD.Print("No Audio Stream found at " + filePath);
            return null;
        }
    }
}
