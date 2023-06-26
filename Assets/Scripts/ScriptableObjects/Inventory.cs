using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject {

    public Item currentItem;
    public List<Item> items = new List<Item>();
    public int numberOfKeys;
    public int coins;
    public float maxMagic = 10;
    public float currentMagic;

    public void OnEnable()
    {
        currentMagic = maxMagic;
    }

    public void ReduceMagic(float magicCost)
    {
        currentMagic -= magicCost;
    }
    
    public bool CheckForItem(Item item)
    { 
        // Apakah memiliki itemnya? jika iya = player memiliki akses untuk fungsi item
        // Jika tidak  = player tidak memiliki akses untuk fungsi item
        if(items.Contains(item))
        {
            return true;
        }
        return false;
    }

    public void AddItem(Item itemToAdd)
    {
        // Apakah itemnya sebuah kunci?
        if(itemToAdd.isKey)
        {
            numberOfKeys++;
        }
        else
        {
            if(!items.Contains(itemToAdd))
            {
                items.Add(itemToAdd);
            }
        }
    }

}