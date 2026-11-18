using UnityEngine;

public class ChickenAppearance : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController[] colors;

    private void Awake()
    {
        RandomizeColor();
    }

    [ContextMenu("Randomize Chicken Color")]
    private void RandomizeColor()
    {
        if (animator == null || colors.Length == 0)
            return;

        int randomIndex = Random.Range(0, colors.Length);

        animator.runtimeAnimatorController = colors[randomIndex];
    }
}
