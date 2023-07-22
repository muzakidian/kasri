using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pot : MonoBehaviour
{
    private Animator anim;
    public LootTable thisLoot;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool isSmashed = false;

    public virtual void Smash()
    {
        anim.SetBool("smash", true);
        StartCoroutine(breakCo());

        if (!isSmashed)
        {
            isSmashed = true;
            Debug.Log("making loot");
            MakeLoot();
        }
    }

    // public void Smash()
    // {
    //     anim.SetBool("smash", true);
    //     StartCoroutine(breakCo());
    // }

    IEnumerator breakCo()
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
    }
    private void MakeLoot()
    {
        if (thisLoot != null)
        {
            Powerup current = thisLoot.LootPowerup();
            if (current != null)
            {
                //Menajatuhkan lootingan tepat saat musuh mati
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }
}
