using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Estado Atual")]
    [SerializeField] private ItemType itemNaMao = ItemType.Nenhum;

    public ItemType ItemNaMao
    {
        get { return itemNaMao; }
        set { itemNaMao = value; }
    }

}