using System.Collections.Generic;
using System.IO;
using UnityEngine;

// todo: update is overcharged, split on functions
// there is something very wrong regarding stay on Nest

public class Chicken_Interactable : Interactable
{
    // References
    [SerializeField] private Animator anim;
    private PlayerInteraction interactor;

    private Nest nest;

    // Settings
    [Header("Movement Settings")]
    [SerializeField] private float maximumDistance = 5f;
    [SerializeField] private float defaultSpeed = 2f;
    private float speed;
    [SerializeField] private Vector3 followOffset = new(-1f, -0.5f, 0f);

    // State
    private bool follow;
    private bool onNest;
    private Vector3 spawnLocation;
    private Vector3 direction;
    private Vector3 targetPosition;
    

    private enum ChickenState
    {
        Idle,
        Walking,
        Sleeping
    }

    private ChickenState state;

    // Shared data
    private static List<Chicken_Interactable> chickensList = new();

    // Those methods are needed to prevent static lists from keeping loaded even when I stop playing or remove chickens from list when I move between scenes

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticState()
    {
        chickensList.Clear();
    }

    private void OnDestroy()
    {
        if (chickensList.Remove(this))
        {
            UpdateChickenSpeeds();
        }
    }

    private void Awake()
    {
        spawnLocation = transform.position;
        speed = defaultSpeed;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Interact(PlayerInteraction interactor)
    {
        DialogueManager.Instance?.ShowMessage("Piu!");
        this.interactor = interactor; // <-- armazena para uso posterior
        if (!chickensList.Contains(this))
        {
            chickensList.Add(this);
            UpdateChickenSpeeds();
        }
        follow = true;

        // forces the chicken to "leave" the nest
        onNest = false;
    }

    private void Update()
    {
        if (chickensList.RemoveAll(chicken => chicken == null) > 0)
        {
            UpdateChickenSpeeds();
        }

        int index = chickensList.IndexOf(this);


        // if this is the first chicken, then follow the player. Else follow the last chicken on list.
        if (follow && index == 0)
        {
            targetPosition = interactor.GetComponentInParent<Transform>().position + followOffset;
        }
        else if (follow && index > 0)
        {
            targetPosition = chickensList[index - 1].transform.position + followOffset;
        }

        if (follow && Vector3.Distance(targetPosition, transform.position) > 0.1f)
        {
            state = ChickenState.Walking;
            // Move the chicken towards the player
            // reminder: direction we use to animate
            direction = (targetPosition - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if(Vector3.Distance(targetPosition, transform.position) > maximumDistance)
            {
                follow = false;
                chickensList.Remove(this);
                speed = 2f;
            }
        }

        else if (!follow && !onNest)
        {
            // reminder: direction we use to animate
            direction = (spawnLocation - transform.position).normalized;

            // MoveTowards helps us and is more adequate when we want to make an object move to given location
            transform.position = Vector3.MoveTowards(transform.position, spawnLocation, speed * Time.deltaTime);

            if (transform.position == spawnLocation)
            {
                state = ChickenState.Idle;
            }
            else
            {
                state = ChickenState.Walking;
            }
        }

        else if (!onNest)
        {
            state = ChickenState.Idle;
        }

        else if (onNest)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if(transform.position == targetPosition)
            {
                state = ChickenState.Sleeping;
                LayEgg();
            }
        }

        Animate();
        
    }

    private void Animate()
    {
        anim.SetBool("isMoving", state == ChickenState.Walking);
        anim.SetBool("isSleeping", state == ChickenState.Sleeping);

        if (direction.x != 0f)
            anim.SetFloat("X", Mathf.Sign(direction.x));
    }

    private static void UpdateChickenSpeeds()
    {
        chickensList.RemoveAll(chicken => chicken == null);
        for (int i = 0; i < chickensList.Count; i++)
        {
            Chicken_Interactable chicken = chickensList[i];
            chicken.speed = chicken.defaultSpeed * Mathf.Pow(0.9f, i);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Nest>(out Nest exitingNest))
            return;

        if (exitingNest == nest && nest.chicken == this)
        {
            nest.chicken = null;
            nest = null;
            onNest = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Nest>(out Nest enteredNest))
            return;

        if (enteredNest.chicken == null)
        {
            nest = enteredNest;
            nest.chicken = this;
            onNest = true;
            follow = false;
            targetPosition = nest.transform.position;

            chickensList.Remove(this);
            UpdateChickenSpeeds();
        }
    }

    private void LayEgg()
    {
        if(nest.Item == null)
        {
            Debug.Log(nest);
            // Resources.Load will load a Scriptable object of given name
            nest.Item = Resources.Load<ItemsScriptableObject>("Items/Egg_SO");
        }
        
    }



}
