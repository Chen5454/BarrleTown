﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSO : ScriptableObject
{
	public string itemName;
	public Sprite itemSprite;
}

public enum itemType
{
	gun,
	shoe,
	armor
}