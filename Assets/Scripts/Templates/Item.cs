using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Item")]
public class Item : ScriptableObject
{
    public new string name;
    [TextArea(10, 10)] public string description;
    public Sprite icon;
}
