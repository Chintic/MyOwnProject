using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interação")]
    [SerializeField] private string promptMessage = "Pressione [E] para interagir";

    public string PromptMessage => promptMessage;

    public abstract void Interact(GameObject interactor);
}
