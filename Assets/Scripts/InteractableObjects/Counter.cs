using System;
using UnityEngine;

public class Counter : Interactable
{
    [SerializeField] private ItemsScriptableObject givenItem;
    [SerializeField] private SpriteRenderer givenItemSprite;
    [SerializeField] private AudioClip pickupSfx;
    [SerializeField] private AudioClip placeSfx;
    AudioSource sfx;

    private void Awake()
    {
        givenItemSprite.sprite = givenItem.sprite;
        sfx = GetComponent<AudioSource>();
    }
    public override void Interact(PlayerInteraction interactor)
    {
        //PlayerInventory inventory = interactor.Inventory;

        // !! looks a bit messy changing the two last variables every time
        if (interactor.Inventory.ItemNaMao == null && givenItem != null)
        {
            sfx.PlayOneShot(pickupSfx);
            interactor.Inventory.ItemNaMao = givenItem;
            givenItem = null;
            givenItemSprite.sprite = null;


        }
        else if (interactor.Inventory.ItemNaMao != null && givenItem == null)
        {
            sfx.PlayOneShot(placeSfx);
            givenItem = interactor.Inventory.ItemNaMao;
            givenItemSprite.sprite = givenItem.sprite;
            interactor.Inventory.ItemNaMao = null;

        }
    }

}