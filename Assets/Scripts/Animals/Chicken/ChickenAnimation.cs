using UnityEngine;

public class ChickenAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private Chicken_Interactable chicken;

    private void Awake()
    {
        chicken = GetComponent<Chicken_Interactable>();
    }
    private void Animate()
    {

        // I will refactor states before proceeding
        anim.SetBool("isMoving", chicken.State == Chicken_Interactable.ChickenState.ReturningNest ||
                                 chicken.State == Chicken_Interactable.ChickenState.ReturningHome ||
                                 chicken.IsMoving);
        anim.SetBool("isSleeping", chicken.State == Chicken_Interactable.ChickenState.Sleeping ||
                                   chicken.State == Chicken_Interactable.ChickenState.LayingEgg);

        if (chicken.Direction.x != 0f)
            anim.SetFloat("X", Mathf.Sign(chicken.Direction.x));
    }

    private void Update()
    {
        Animate();
    }

    
}
