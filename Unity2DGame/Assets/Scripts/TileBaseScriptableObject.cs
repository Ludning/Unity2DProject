using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileBaseScriptableObject")]
public class TileBaseScriptableObject : ScriptableObject
{
    public List<TileBaseData> grass;
    public List<TileBaseData> road;
    public List<TileBaseData> stone;
    public List<TileBaseData> wall;
    public List<TileBaseData> shadow;
}

[Serializable]
public class TileBaseData
{
    public TileBase tileBase;
    public TileBaseDirType Up;
    public TileBaseDirType Down;
    public TileBaseDirType Left;
    public TileBaseDirType Right;
}

[Serializable]
public enum TileBaseDirType
{
    Grass,
    Road_Full,
    Road_Left,
    Road_Right,
    Stone,
    Wall,
}