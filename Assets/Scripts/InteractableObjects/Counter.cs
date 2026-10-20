using System;
using UnityEngine;

public class Counter : Interactable
{
    [SerializeField] private ItemsScriptableObject itemFornecido;
    [SerializeField] private SpriteRenderer itemGivenSprite;

    private void Awake()
    {
        itemGivenSprite.sprite = itemFornecido.sprite;
    }
    public override void Interact(PlayerInteraction interactor)
    {
        PlayerInventory inventory = interactor.Inventory;

        if (inventory.ItemNaMao == null)
        {
            inventory.ItemNaMao = itemFornecido;

            DialogueManager.Instance?.ShowMessage($"Você pegou: {itemFornecido.itemName}!");
        }
        else
        {
            DialogueManager.Instance?.ShowMessage("Você já está segurando um item!");
        }
    }

}