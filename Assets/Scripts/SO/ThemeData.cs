﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Theme Data", menuName = "Forest United/Theme Data", order = 2)]
public class ThemeData : ScriptableObject {

    [Header("Field Sprites")]
    public List<Sprite> FieldSpritesList = new List<Sprite> { };

    [Header("Trees Prefabs")]
    public List<Tree> TreesPrefabsList = new List<Tree> { };

    [Header("Totem Prefabs")]
    public List<Totem> TotemPrefabsList = new List<Totem> { };

    [Header("Types of Fogs")]
    public List<Sprite> FogSpritesList = new List<Sprite> { };

    [Header("Music Library")]
    public List<AudioClip> MusicLibraryList = new List<AudioClip> { };

}
