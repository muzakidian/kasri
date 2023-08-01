using System.Collections;
using UnityEngine;

public class sampah : pot
{
    public SampahSpawner spawner;
    private bool isHancur = false;

    // Override the Smash function
    public override void Smash()
    {
        if (!isHancur)
        {
            isHancur = true;
            StartCoroutine(DisableObject());
        }
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
    } 
}
