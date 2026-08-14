using Godot;
using Godot.Collections;
using System;
using System.Linq;

namespace MiTale
{
    public static class GlobalVariables
    {

        public static string c_inkButtonScene = "res://addons/InkEngine/Scenes/InkButton.tscn";
        public static string c_inkLabelScene = "res://addons/InkEngine/Scenes/InkLabel.tscn";
        public static string c_inkInventoryButtonScene = "res://addons/InkEngine/Scenes/InkInventoryButton.tscn";
        public static string c_inkTooltipScene = "res://addons/InkEngine/Scenes/InkTooltip.tscn";

        //private static string c_characterDataPath = "res://Scripts/CharacterStateStuff/CharacterData/";

        public static Texture2D GetPortraitTexture2D(string id)
        {
            id = id.Trim();
            string start = "res://addons/InkEngine/Graphics/Portraits/";
            string finalString = start + id;
            if (ResourceLoader.Exists(finalString))
            {
                Texture2D loadedTexture = ResourceLoader.Load(finalString) as Texture2D;
                return loadedTexture;
            }
            else
            {
                GD.Print("No portrait with ID " + id + " found!");
                return null;
            }
        }
        public static Texture2D GetBackgroundTexture2D(string id)
        {
            id = id.Trim();
            string start = "res://addons/InkEngine/Graphics/Backgrounds/";

            string finalString = start + id;
            if (ResourceLoader.Exists(finalString))
            {
                Texture2D loadedTexture = ResourceLoader.Load(finalString) as Texture2D;
                return loadedTexture;
            }
            else
            {
                GD.Print("No background with ID " + id + " found!");
                return null;
            }
        }
        public static Texture2D GetIconTexture2D(string id)
        {
            id = id.Trim();
            string start = "res://addons/InkEngine/Graphics/Icons/";
            string finalString = start + id;
            if (ResourceLoader.Exists(finalString))
            {
                Texture2D loadedTexture = ResourceLoader.Load(finalString) as Texture2D;
                return loadedTexture;
            }
            else
            {
                GD.Print("No icon with ID " + id + " found!");
                return null;
            }
        }
        public static Texture2D GetTexture2D(string id)
        { // Call: GlobalVariables.GetTexture2D("res://...image path") OR use ID (will look through all possible paths)
            id = id.Trim();
            Texture2D attempt = GetIconTexture2D(id);
            if (attempt != null)
            {
                return attempt;
            }
            attempt = GetPortraitTexture2D(id);
            if (attempt != null)
            {
                return attempt;
            }
            attempt = GetBackgroundTexture2D(id);
            if (attempt != null)
            {
                return attempt;
            }
            if (ResourceLoader.Exists(id))
            {
                return ResourceLoader.Load(id) as Texture2D;
            }
            else
            {
                GD.Print("Could not find texture2d at path " + id);
                return null;
            }
        }
        public static StyleBox GetStyleBox(string id)
        {
            id = id.Trim();
            string start = "res://addons/InkEngine/UI/";
            string finalString = start + id;
            if (ResourceLoader.Exists(finalString))
            {
                StyleBox loadedTheme = ResourceLoader.Load(finalString) as StyleBox;
                return loadedTheme;
            }
            else
            {
                GD.Print("No stylebox with ID " + id + " found!");
                return null;
            }
        }
        /*
                private static Godot.Collections.Dictionary<string, CharacterData> _allCharacterdatas = new Godot.Collections.Dictionary<string, CharacterData> { };

                public static Godot.Collections.Dictionary<string, CharacterData> AllCharacterdatas
                {
                    get
                    {
                        if (_allCharacterdatas.Count == 0)
                        {
                            LoadAllCharacterDatas();
                        }
                        return _allCharacterdatas;
                    }
                }

                private static void LoadAllCharacterDatas()
                {
                    string[] allContents = ResourceLoader.ListDirectory(c_characterDataPath);
                    foreach (string path in allContents)
                    {
                        if (ResourceLoader.Exists(c_characterDataPath + path))
                        {
                            var resource = ResourceLoader.Load(c_characterDataPath + path);
                            if (resource is CharacterData)
                            {
                                _allCharacterdatas.Add((resource as CharacterData).Id, resource as CharacterData);
                                GD.Print("Loaded " + (resource as CharacterData).Id + " into all character types");
                            }
                        }
                    }
                }
                */
    }
}