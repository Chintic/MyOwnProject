using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private ItemsScriptableObject itemNaMao = null;
    [SerializeField] private SpriteRenderer itemNaMaoSpriteRenderer;

    public event Action<ItemsScriptableObject> OnHeldItemChange;
    public ItemsScriptableObject ItemNaMao
    {
        get { return itemNaMao; }

        set
        {
            itemNaMao = value;

            if (itemNaMao != null)
            {
                itemNaMaoSpriteRenderer.sprite = itemNaMao.sprite;
            }
            else
            {
                itemNaMaoSpriteRenderer.sprite = null;
            }
            OnHeldItemChange?.Invoke(itemNaMao);

        }
    }

}