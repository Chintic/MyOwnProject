using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private string promptMessage;

    public string PromptMessage => promptMessage;

    public abstract void Interact(PlayerInteraction interactor);
}
