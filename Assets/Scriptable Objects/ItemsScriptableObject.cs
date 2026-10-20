using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemsScriptableObject : ScriptableObject
{
    public string itemName;
    public Sprite sprite;

}
