using UnityEngine;
using System.Collections;

[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 0)]
public class Item : ScriptableObject
{
    public int id { get; protected set; }
    public new string name { get; protected set; }
    public int maxStack { get; protected set; }
    public bool canStack() { if (maxStack == 0) { return false; } return true; }

    public override string ToString()
    {
        return string.Format("[Item: id={0}, name={1}, maxStack={2}]", id, name, maxStack);
    }
}