using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo: update is overcharged, split on functions
// there is something very wrong regarding stay on Nest

public class Chicken_Interactable : Interactable
{
    // References
    // DONE
    [SerializeField] private Animator anim;
    [SerializeField] private ChickenAppearance appearance;
    private PlayerInteraction interactor;
    private Nest nest;

    // Settings
    [SerializeField] private float layEggTimer = 5f;

    [Header("Movement Settings")]
    [SerializeField] private float maximumDistance = 5f;
    [SerializeField] private float defaultSpeed = 2f;
    [SerializeField] private Vector3 followOffset = new(-1f, -0.5f, 0f);

    // Movement
    private float speed;
    private Vector3 spawnLocation;
    private Vector3 direction;
    private Vector3 targetPosition;

    private bool isMoving;

    public bool IsMoving => isMoving;

    // State
    public enum ChickenState
    {
        Idle,
        Sleeping,
        Following,
        LayingEgg,
        OnNest,
        ReturningNest,
        ReturningHome
    }

    public ChickenState state;
    public bool isHatched = false;

    // Shared data
    private static List<Chicken_Interactable> chickensList = new();

    // Properties
    public ChickenState State => state;
    public Vector3 Direction => direction;

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

    public override void Interact(PlayerInteraction interactor)
    {
        DialogueManager.Instance?.ShowMessage("Piu!");
        this.interactor = interactor; // <-- armazena para uso posterior
        if (!chickensList.Contains(this))
        {
            chickensList.Add(this);
            UpdateChickenSpeeds();
        }
        state = ChickenState.Following;

    }

    private void Update()
    {


        // each frame this will remove null items from the list and then update the remaining chicken speeds
        if (chickensList.RemoveAll(chicken => chicken == null) > 0)
        {
            UpdateChickenSpeeds();
        }

        switch (state)
        {
            case ChickenState.Idle:
                break;
            case ChickenState.Following:
                Follow();
                break;
            case ChickenState.ReturningHome:
                ReturnHome();
                break;
            case ChickenState.ReturningNest:
                ReturnNest();
                break;
            case ChickenState.OnNest:
                isMoving = false;

                if (nest != null && nest.chicken == this && nest.Item == null)
                {
                    StartCoroutine(LayEgg());
                }
                else
                {
                    state = ChickenState.Sleeping;
                }

                break;
            case ChickenState.Sleeping:
                isMoving = false;

                if (nest != null && nest.chicken == this && nest.Item == null)
                {
                    state = ChickenState.OnNest;
                }

                break;
            case ChickenState.LayingEgg:
                isMoving = false;
                break;
        }

    }

    private void Follow()
    {
        
        int index = chickensList.IndexOf(this);

        // if this is the first chicken, then follow the player. Else follow the last chicken on list.
        if (index == 0)
        {
            targetPosition = interactor.GetComponentInParent<Transform>().position + followOffset;
        }
        else if (index > 0)
        {
            targetPosition = chickensList[index - 1].transform.position + followOffset;
        }
        if (Vector3.Distance(targetPosition, transform.position) > 0.1f)
        {
            isMoving = true;

            direction = (targetPosition - transform.position).normalized;
            transform.position = Vector3.MoveTowards(
                transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(targetPosition, transform.position) > maximumDistance)
            {
                chickensList.Remove(this);
                UpdateChickenSpeeds();
                speed = defaultSpeed;
                state = ChickenState.ReturningHome;
            }
        }
        else
        {
            isMoving = false;
        }
    }

    private void ReturnHome() {
        // reminder: direction we use to animate
        direction = (spawnLocation - transform.position).normalized;

        // MoveTowards helps us and is more adequate when we want to make an object move to given location
        transform.position = Vector3.MoveTowards(transform.position, spawnLocation, speed * Time.deltaTime);

        if (transform.position == spawnLocation)
        {
            state = ChickenState.Idle;
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    private void ReturnNest()
    {
        isMoving = true;

        direction = (targetPosition - transform.position).normalized;

        transform.position = Vector3.MoveTowards(
            transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMoving = false;
            state = ChickenState.OnNest;
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Nest>(out Nest enteredNest))
            return;

        if (enteredNest.chicken == null)
        {
            nest = enteredNest;
            nest.chicken = this;
            state = ChickenState.ReturningNest;
            targetPosition = nest.transform.position;

            chickensList.Remove(this);
            UpdateChickenSpeeds();
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
        }
    }

    private IEnumerator LayEgg()
    {
        state = ChickenState.LayingEgg;

        Nest layingNest = nest;

        yield return new WaitForSeconds(layEggTimer);

        if (state == ChickenState.LayingEgg &&
            layingNest != null &&
            nest == layingNest &&
            layingNest.chicken == this)
        {
            if (layingNest.Item == null &&
                appearance != null &&
                appearance.Egg != null)
            {
                layingNest.Item = appearance.Egg;
            }

            state = ChickenState.OnNest;
        }
    }



}