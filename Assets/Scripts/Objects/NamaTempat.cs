using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NamaTempat : MonoBehaviour
{
    public bool needText;
    public string placeName;
    public GameObject text;
    public Text placeText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            if(needText)
            {
                StartCoroutine(placeNameCo());
            }

        }
    }
        private IEnumerator placeNameCo()
    {
        text.SetActive(true);
        placeText.text = placeName;
        yield return new WaitForSeconds(2f);
        text.SetActive(false);
    }
}
