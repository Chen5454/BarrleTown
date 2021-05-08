using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public enum ItemType
    {
        Barrel,
        Weapon,
        Armor,
        Shoes,
    }

    public ItemType itemType;
}
