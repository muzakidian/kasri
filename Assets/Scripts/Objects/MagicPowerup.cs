using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowerup : Powerup
{

    public Inventory playerInventory;
    public float magicValue;
    [SerializeField] private AudioSource minumSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            minumSound.Play();
            playerInventory.currentMagic += magicValue;
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
