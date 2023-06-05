using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Powerup
{
    public FloatValue playerHealth;
    public FloatValue heartContainers;
    public float amountToIncrease;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerHealth.RuntimeValue += amountToIncrease;
            if(playerHealth.initialValue > heartContainers.RuntimeValue * 2f)
            {
                playerHealth.initialValue = heartContainers.RuntimeValue * 2f;
            }
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }

}