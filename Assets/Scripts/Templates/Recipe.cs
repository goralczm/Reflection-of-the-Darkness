using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/New Recipe")]
public class Recipe : ScriptableObject
{
    public new string name;
    public List<Item> inputs;
    public Item output;
}
