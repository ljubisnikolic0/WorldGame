﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopInventory : Inventory {

	// Use this for initialization
    protected override void Start()
    {
        Initiate();
        base.Start();
        
        typeParentInv = TypeParentInv.shop;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
