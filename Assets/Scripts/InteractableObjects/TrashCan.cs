using UnityEngine;

public class TrashCan : Interactable    
{

    // Implementa o método abstrato herdado
    public override void Interact(PlayerInteraction interactor)
    {
        //PlayerInventory inventory = interactor.Inventory;

        if (interactor.Inventory.ItemNaMao == null)
        {
            DialogueManager.Instance?.ShowMessage($"Você não tem nenhum item para descartar!");
        }
        else
        {
            interactor.Inventory.ItemNaMao = null;
            DialogueManager.Instance?.ShowMessage("Você jogou fora seu item.");
        }
    }
}

