using UnityEngine;

public class Counter : Interactable
{
    [SerializeField] private ItemType itemFornecido;

    public override void Interact(PlayerInteraction interactor)
    {
        PlayerInventory inventory = interactor.Inventory;

        if (inventory.ItemNaMao == ItemType.Nenhum)
        {
            inventory.ItemNaMao = itemFornecido;

            DialogueManager.Instance?.ShowMessage($"Você pegou: {itemFornecido}!");
        }
        else
        {
            DialogueManager.Instance?.ShowMessage("Você já está segurando um item!");
        }
    }
}