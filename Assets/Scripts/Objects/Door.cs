using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    key,
    enemy,
    button
}

public class Door : Interactable
{
    [Header("Door variables")]
    public DoorType thisDoorType;
    public bool open = false;
    public Inventory playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(playerInRange && thisDoorType == DoorType.key)
            {
                //Apakah player punya kunci?
                if(playerInventory.numberOfKeys > 0)
                {
                    //Hapus kunci yg dimiliki player
                    playerInventory.numberOfKeys--;
                    //Jika iya, panggil method buka pintu
                    Open();
                }
            }
        }
    }

    public void Open()
    {
        //Mematikan sprite renderer
        doorSprite.enabled = false;
        //set open to true
        open = true;
        //turn off box collider pintu 
        physicsCollider.enabled = false;
    }

    public void Close()
    {
        //Menghidupkan sprite render
        doorSprite.enabled = true;
        //set open to false
        open = false;
        //turn on box collider pintu
        physicsCollider.enabled = true;
    }
}