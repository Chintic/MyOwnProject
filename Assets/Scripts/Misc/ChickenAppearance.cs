using UnityEngine;

public class ChickenAppearance : MonoBehaviour
{
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
        RandomizeColor();
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
}