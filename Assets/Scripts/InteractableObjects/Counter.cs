using System;
using UnityEngine;

public class Counter : Interactable
{
    [SerializeField] private ItemsScriptableObject givenItem;
    [SerializeField] private SpriteRenderer givenItemSprite;

    private void Awake()
    {
        givenItemSprite.sprite = givenItem.sprite;
    }
    public override void Interact(PlayerInteraction interactor)
    {
        PlayerInventory inventory = interactor.Inventory;

        if (inventory.ItemNaMao == null)
        {
            inventory.ItemNaMao = givenItem;
            DialogueManager.Instance?.ShowMessage($"Você pegou: {givenItem.itemName}!");
            givenItem = null;
            givenItemSprite = null;


        }
        else
        {
            DialogueManager.Instance?.ShowMessage("Você já está segurando um item!");
        }
    }

}