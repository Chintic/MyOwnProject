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
        if (egg != null && egg.itemName == "Egg" && !isIncubating)
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
        else if (interactor.Inventory.IsHoldingItem("Egg") && egg == null)
        {
            egg = interactor.Inventory.ItemNaMao;
            interactor.Inventory.ItemNaMao = null;
            UpdateSprite();
        }
    }

    private IEnumerator Incubate()
    {
        isIncubating = true;

        ItemsScriptableObject incubatingEgg = egg;

        yield return new WaitForSeconds(incubateTimer);

        // Cancel if the egg was removed or replaced.
        if (egg == null || egg != incubatingEgg)
        {
            isIncubating = false;
            yield break;
        }

        Vector3 spawnPosition =
            transform.position + new Vector3(0f, -1f, 0f);

        GameObject chicken = Instantiate(
            chickenPrefab, spawnPosition, Quaternion.identity);

        Chicken_Interactable interactable =
            chicken.GetComponent<Chicken_Interactable>();

        if (interactable != null)
            interactable.isHatched = true;

        ChickenAppearance appearance =
            chicken.GetComponentInChildren<ChickenAppearance>();

        if (appearance != null)
            appearance.SetColorFromEgg(incubatingEgg);

        egg = null;
        UpdateSprite();

        isIncubating = false;
    }
    private void UpdateSprite()
    {
        eggSprite.sprite = egg != null ? egg.sprite : null;
    }
}