using UnityEngine;

public class ChickenAppearance : MonoBehaviour
{
    private bool isHatched;
    [System.Serializable]
    private class ColorVariant
    {
        public RuntimeAnimatorController controller;
        public ItemsScriptableObject egg;
    }

    [SerializeField] private Animator animator;
    [SerializeField] private ColorVariant[] colors;

    public ItemsScriptableObject Egg { get; private set; }

    private void Awake()
    {
        isHatched = GetComponentInParent<Chicken_Interactable>().isHatched;
        if(!isHatched)
        {
            RandomizeColor();
        }


    }

    [ContextMenu("Randomize Chicken Color")]
    private void RandomizeColor()
    {
        if (animator == null || colors == null || colors.Length == 0)
            return;

        int randomIndex = Random.Range(0, colors.Length);
        ColorVariant selectedColor = colors[randomIndex];

        if (selectedColor == null ||
            selectedColor.controller == null ||
            selectedColor.egg == null)
        {
            Debug.LogWarning(
                "Assign a controller and egg to every chicken color.",
                this);
            return;
        }

        animator.runtimeAnimatorController = selectedColor.controller;
        Egg = selectedColor.egg;
    }

    public void SetColorFromEgg(ItemsScriptableObject sourceEgg)
    {
        if (sourceEgg == null || animator == null || colors == null)
            return;

        foreach (ColorVariant variant in colors)
        {
            if (variant != null &&
                variant.egg == sourceEgg &&
                variant.controller != null)
            {
                animator.runtimeAnimatorController = variant.controller;
                Egg = variant.egg;
                return;
            }
        }

        Debug.LogWarning("No chicken color matches this egg.", this);
    }
}