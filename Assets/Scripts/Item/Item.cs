﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.UI;



public class Item
{
    public string name;
    public int id;
    public ItemType itemType;
    public string description;
	public string iconPath;                                     
    public string dropModelPath; 
    public int salePrice;
    public int quantity;
    public int indexItemInList = 999;

    [XmlIgnore]
    public Sprite iconSprite;
    [XmlIgnore]
    public GameObject dropModelObj;

    public Item() { }

    public Item(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public bool IsEmpty()
    {
        if (name == null)
            return true;
        return false;
    }

    public void LoadResources()
    {
        if (iconSprite == null)
            iconSprite = Resources.Load<Sprite>(iconPath);
        if (dropModelObj == null)
            dropModelObj = Resources.Load(dropModelPath) as GameObject;
    }
		
}


