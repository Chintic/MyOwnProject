using UnityEngine;

public class TrashCan : Interactable    
{

    public PlayerInventory inventory;
    // Implementa o método abstrato herdado
    public override void Interact(PlayerInteraction interactor)
    {
        PlayerInventory inventory = interactor.Inventory;

        if (inventory.ItemNaMao == ItemType.Nenhum)
        {
            DialogueManager.Instance?.ShowMessage($"Você não tem nenhum item para descartar!");
        }
        else
        {
            inventory.ItemNaMao = ItemType.Nenhum;
            DialogueManager.Instance?.ShowMessage("Você jogou fora seu item.");
        }
    }
}

