using System;
using System.Collections;
using UnityEngine;

public class Incubator : Interactable
{
    private ItemsScriptableObject egg;

    [SerializeField] private SpriteRenderer eggSprite;
    [SerializeField] private float incubateTimer;
    [SerializeField] private GameObject chickenPrefab;
    private bool isIncubating;

    private void Update()
    {

        if(egg == Resources.Load<ItemsScriptableObject>("Items/Egg_SO") && !isIncubating)
        {
            StartCoroutine(Incubate());
        }
    }

    public override void Interact(PlayerInteraction interactor)
    {
        //PlayerInventory inventory = interactor.Inventory;

        // !! looks a bit messy changing the two last variables every time
        if (interactor.Inventory.ItemNaMao == null && egg != null)
        {
            interactor.Inventory.ItemNaMao = egg;
            egg = null;
            UpdateSprite();

        }
        else if (interactor.Inventory.ItemNaMao == Resources.Load<ItemsScriptableObject>("Items/Egg_SO") && egg == null)
        {
            egg = interactor.Inventory.ItemNaMao;
            interactor.Inventory.ItemNaMao = null;
            UpdateSprite();
        }
    }

    private IEnumerator Incubate()
    {
        isIncubating = true;

        yield return new WaitForSeconds(incubateTimer);

        egg = null;
        UpdateSprite();

        Vector3 spawnPosition = transform.position + new Vector3(0f, -1f, 0f);

        Instantiate(chickenPrefab, spawnPosition, Quaternion.identity);

        isIncubating = false;
    }
    private void UpdateSprite()
    {
        eggSprite.sprite = egg != null ? egg.sprite : null;
    }
}