using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private ItemsScriptableObject itemNaMao = null;
    [SerializeField] private SpriteRenderer itemNaMaoSpriteRenderer;

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

        }
    }

}