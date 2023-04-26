using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/New NPC")]
public class NPC : ScriptableObject
{
    public new string name;
    public TextAsset dialogue;
}
