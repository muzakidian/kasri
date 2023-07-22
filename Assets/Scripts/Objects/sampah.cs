using System.Collections;
using UnityEngine;

public class sampah : pot
{
    private bool isSmashed = false;

    // Override the Smash function
    public override void Smash()
    {
        if (!isSmashed)
        {
            isSmashed = true;
            StartCoroutine(DisableObject());
        }
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
    }
}
