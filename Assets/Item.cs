using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ScriptableObject/Resource")]
public class Item : ScriptableObject
{
    public string name;
    public TileBase tile;
    public Sprite image;
    public TypeItem type;
}

public enum TypeItem
{
    Food,
    Material,
    Tool
}