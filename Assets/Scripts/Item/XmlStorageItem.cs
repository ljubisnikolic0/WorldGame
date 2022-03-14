﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


public class XmlStorageItem : MonoBehaviour
{

    private const int numberIdInFile = 50; //The number of items in a single file
    private const string defaultSpritePath = "Textures/Icons/Item/I_Map";
    private const string defaultDropModelPath = "Prefabs/Inventory/DraggingItem";

    private ItemEquip resultEquip;
    private ItemConsume resultConsume;
    private ItemOther resultOther;

    //public void SaveItems()
    //{
    //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ItemDatabaseCustom));
    //    FileStream fileStram = new FileStream(Application.dataPath + "/Resources/XML/itemsEquip.xml", FileMode.Create);
    //    xmlSerializer.Serialize(fileStram, ItemDatabaseCustom);
    //    fileStram.Close();
    //}

    public Item GetItemById(int id)
    {
        XmlReader xmlReader = GetXmlReader(id);
        ItemType tempItemType;
        while (xmlReader.Read())
        {
            switch (xmlReader.Name)
            {
                case "item":
                    tempItemType = ItemType.Other;
                    break;
                case "equip":
                    tempItemType = ItemType.Equip;
                    break;
                case "consume":
                    tempItemType = ItemType.Consume;
                    break;
                default: continue;
            }

            if (xmlReader.GetAttribute("id") == id.ToString())
            {
                Item tempItem = new Item(id, xmlReader.GetAttribute("name"));
                tempItem.itemType = tempItemType;

                xmlReader.ReadToDescendant("description"); //<description>       
                tempItem.description = xmlReader.ReadElementContentAsString();

                //<icon path=.../>                                          
                if (xmlReader.ReadToNextSibling("icon") && xmlReader.GetAttribute(0) != "")
                    tempItem.iconPath = xmlReader.GetAttribute(0);
                else
                    tempItem.iconPath = defaultSpritePath;

                //<model path=.../>                                        
                if (xmlReader.ReadToNextSibling("model") && xmlReader.GetAttribute(0) != "")
                    tempItem.dropModelPath = xmlReader.GetAttribute(0);
                else
                    tempItem.dropModelPath = defaultDropModelPath;

                if (xmlReader.ReadToNextSibling("salePrice")) //<salePrice>..</salePrice> 
                    tempItem.salePrice = xmlReader.ReadElementContentAsInt();
                else
                    tempItem.salePrice = 0;

                return tempItem;
            }
            else
            {
                xmlReader.Skip();
            }

        }
        return null;
    }

    public ItemType GetItemTypeById(int id)
    {
        XmlReader xmlReader = GetXmlReader(id);
        ItemType tempItemType;
        while (xmlReader.Read())
        {
            switch (xmlReader.Name)
            {
                case "item":
                    tempItemType = ItemType.Other;
                    break;
                case "equip":
                    tempItemType = ItemType.Equip;
                    break;
                case "consume":
                    tempItemType = ItemType.Consume;
                    break;
                default: continue;
            }

            if (xmlReader.GetAttribute("id") == id.ToString())
                return tempItemType;
            else
                xmlReader.Skip();
        }
        return ItemType.Other;
    }

    /// <summary>
    /// Load item by id to result variables
    /// </summary>
    /// <param name="id">id item</param>
    /// <returns>False - no item in xml storage</returns>
    public Item GetResultItemById(int id)
    {
        XmlReader xmlReader = GetXmlReader(id);
        while (xmlReader.Read())
        {
            switch (xmlReader.Name)
            {
                case "item":
                    if (xmlReader.GetAttribute("id") == id.ToString())
                    {
                        return LoadOtherItem(xmlReader, id);
                    }
                    else
                        xmlReader.Skip();
                    break;
                case "equip":
                    if (xmlReader.GetAttribute("id") == id.ToString())
                    {
                        return LoadEquipItem(xmlReader, id);
                    }
                    else
                        xmlReader.Skip();
                    break;
                case "consume":
                    if (xmlReader.GetAttribute("id") == id.ToString())
                    {
                        return LoadConsumeItem(xmlReader, id);
                    }
                    else
                        xmlReader.Skip();
                    break;
                default: continue;
            }

        }
        return null;
    }

    /// <summary>
    /// Return Item Equip clear from xml storage
    /// </summary>
    /// <param name="id">Id item</param>
    public ItemEquip GetItemEquipById(int id)
    {
        XmlReader xmlReader = GetXmlReader(id);
        while (xmlReader.Read())
        {
            if (xmlReader.Name == "equip")
                if (xmlReader.GetAttribute("id") == id.ToString())
                {
                    //<equip ...>
                    return LoadEquipItem(xmlReader, id);
                }
                else
                    xmlReader.Skip();

            if (xmlReader.Name == "consume" || xmlReader.Name == "item")
                xmlReader.Skip();
        }
        return null;
    }
    /// <summary>
    /// Return Item Consume clear from xml storage
    /// </summary>
    /// <param name="id">Id item</param>
    public ItemConsume GetItemConsumeById(int id)
    {
        XmlReader xmlReader = GetXmlReader(id);
        while (xmlReader.Read())
        {
            if (xmlReader.Name == "consume")
                if (xmlReader.GetAttribute("id") == id.ToString())
                {
                    //<consume ...>
                    return LoadConsumeItem(xmlReader, id);
                }
                else
                    xmlReader.Skip();

            if (xmlReader.Name == "equip" || xmlReader.Name == "item")
                xmlReader.Skip();
        }
        return null;
    }
    /// <summary>
    /// Return Item Other clear from xml storage
    /// </summary>
    /// <param name="id">Id item</param>
    public ItemOther GetItemOtherById(int id)
    {
        XmlReader xmlReader = GetXmlReader(id);
        while (xmlReader.Read())
        {
            if (xmlReader.Name == "item")
                if (xmlReader.GetAttribute("id") == id.ToString())
                {
                    //<item ...>
                    return LoadOtherItem(xmlReader, id);
                }
                else
                    xmlReader.Skip();

            if (xmlReader.Name == "equip" || xmlReader.Name == "consume")
                xmlReader.Skip();
        }
        return null;
    }

    public int GetConsumeMaxInStackById(int id)
    {
        XmlReader xmlReader = GetXmlReader(id);
        while (xmlReader.Read())
        {
            if (xmlReader.Name == "item" || xmlReader.Name == "equip")
            {
                xmlReader.Skip();
                continue;
            }
            if (xmlReader.Name == "consume")
            {
                if (xmlReader.GetAttribute("id") == id.ToString())
                {
                    if (xmlReader.ReadToFollowing("maxInStack")) //<maxInStack>
                        return xmlReader.ReadElementContentAsInt();
                    else
                        return 99;
                }
                xmlReader.Skip();
            }
        }
        return 1;
    }
    private XmlReader GetXmlReader(int id)
    {
        int minIdInFile = (id / numberIdInFile) * numberIdInFile;
        string nameFile = minIdInFile + "-" + (minIdInFile + numberIdInFile - 1);
        TextAsset _TextAsset = Resources.Load("XML/Items/" + nameFile) as TextAsset;
        //XmlReader xmlReader = XmlReader.Create(Application.dataPath + "/Resources/XML/" + arrayFiles[numFile]);  
        TextReader _TextReader = new StringReader(_TextAsset.text);
        XmlReader xmlReader = XmlReader.Create(_TextReader);
        xmlReader.MoveToContent();
        return xmlReader;
    }

    private ItemEquip LoadEquipItem(XmlReader xmlReader, int id)
    {
        ItemEquip itemEquip = new ItemEquip(id, xmlReader.GetAttribute("name"));
        itemEquip.itemEquipType = (ItemEquipType)System.Enum.Parse(typeof(ItemEquipType), xmlReader.GetAttribute("type"));


        xmlReader.ReadToDescendant("description"); //<description>       
        itemEquip.description = xmlReader.ReadElementContentAsString();

        //<icon path=.../>                                          
        if (xmlReader.ReadToNextSibling("icon") && xmlReader.GetAttribute(0) != "")
            itemEquip.iconPath = xmlReader.GetAttribute(0);
        else
            itemEquip.iconPath = defaultSpritePath;

        //<model path=.../>                                        
        if (xmlReader.ReadToNextSibling("model") && xmlReader.GetAttribute(0) != "")
            itemEquip.dropModelPath = xmlReader.GetAttribute(0);
        else
            itemEquip.dropModelPath = defaultDropModelPath;

        if (xmlReader.ReadToNextSibling("salePrice")) //<salePrice>..</salePrice> 
            itemEquip.salePrice = xmlReader.ReadElementContentAsInt();
        else
            itemEquip.salePrice = 0;

        if (xmlReader.ReadToNextSibling("requests"))
        { //<requests lvl=.. str=.. agi=.. vit=.. ene=.. />
            int.TryParse(xmlReader.GetAttribute("lvl"), out itemEquip.requestLevel);
            int.TryParse(xmlReader.GetAttribute("str"), out itemEquip.requestStrenght);
            int.TryParse(xmlReader.GetAttribute("agi"), out itemEquip.requestAgility);
            int.TryParse(xmlReader.GetAttribute("vit"), out itemEquip.requestVitality);
            int.TryParse(xmlReader.GetAttribute("ene"), out itemEquip.requestEnergy);
        }
        else
        {
            itemEquip.requestLevel = 1;
            itemEquip.requestStrenght = 0;
            itemEquip.requestAgility = 0;
            itemEquip.requestVitality = 0;
            itemEquip.requestEnergy = 0;
        }

        //<atributes>
        if (xmlReader.ReadToNextSibling("atributes") && xmlReader.ReadToDescendant("add"))//<add..
        {
            AtributeTypeItem tempType;
            float tempValue;
            do
            {
                tempType = (AtributeTypeItem)System.Enum.Parse(typeof(AtributeTypeItem), xmlReader.GetAttribute(0));
                tempValue = xmlReader.ReadElementContentAsFloat();
                itemEquip.itemAttributes.Add(new AttributeItem(tempType, tempValue));
            } while (xmlReader.ReadToNextSibling("add"));
        }

        xmlReader.Close();
        return itemEquip;
    }
    private ItemOther LoadOtherItem(XmlReader xmlReader, int id)
    {
        ItemOther itemOther = new ItemOther(id, xmlReader.GetAttribute("name"));
        itemOther.itemOtherType = (ItemOtherType)System.Enum.Parse(typeof(ItemOtherType), xmlReader.GetAttribute("type"));

        xmlReader.ReadToDescendant("description"); //<description>       
        itemOther.description = xmlReader.ReadElementContentAsString();

        //<icon path=.../>                                          
        if (xmlReader.ReadToNextSibling("icon") && xmlReader.GetAttribute(0) != "")
            itemOther.iconPath = xmlReader.GetAttribute(0);
        else
            itemOther.iconPath = defaultSpritePath;

        //<model path=.../>                                        
        if (xmlReader.ReadToNextSibling("model") && xmlReader.GetAttribute(0) != "")
            itemOther.dropModelPath = xmlReader.GetAttribute(0);
        else
            itemOther.dropModelPath = defaultDropModelPath;

        if (xmlReader.ReadToNextSibling("salePrice")) //<salePrice>..</salePrice> 
            itemOther.salePrice = xmlReader.ReadElementContentAsInt();
        else
            itemOther.salePrice = 0;

        //<recipe id=""/>
        if (itemOther.itemOtherType == ItemOtherType.Recipe && xmlReader.ReadToNextSibling("recipe"))
            itemOther.RecipeId = int.Parse(xmlReader.GetAttribute("id"));

        xmlReader.Close();
        return itemOther;
    }
    private ItemConsume LoadConsumeItem(XmlReader xmlReader, int id)
    {
        ItemConsume itemConsume = new ItemConsume(id, xmlReader.GetAttribute("name"));

        xmlReader.ReadToDescendant("description"); //<description>       
        itemConsume.description = xmlReader.ReadElementContentAsString();

        //<icon path=.../>                                          
        if (xmlReader.ReadToNextSibling("icon") && xmlReader.GetAttribute(0) != "")
            itemConsume.iconPath = xmlReader.GetAttribute(0);
        else
            itemConsume.iconPath = defaultSpritePath;

        //<model path=.../>                                        
        if (xmlReader.ReadToNextSibling("model") && xmlReader.GetAttribute(0) != "")
            itemConsume.dropModelPath = xmlReader.GetAttribute(0);
        else
            itemConsume.dropModelPath = defaultDropModelPath;

        if (xmlReader.ReadToNextSibling("salePrice")) //<salePrice>..</salePrice> 
            itemConsume.salePrice = xmlReader.ReadElementContentAsInt();
        else
            itemConsume.salePrice = 0;

        if (xmlReader.ReadToFollowing("numberTicks")) //<numberTicks>
            itemConsume.numberTicks = xmlReader.ReadElementContentAsInt();
        else
            itemConsume.numberTicks = 1;

        if (xmlReader.ReadToFollowing("delayTick")) //<delayTick>
            itemConsume.delayTick = xmlReader.ReadElementContentAsFloat();
        else
            itemConsume.delayTick = 1.0f;

        if (xmlReader.ReadToFollowing("maxInStack")) //<maxInStack>
            itemConsume.maxInStack = xmlReader.ReadElementContentAsInt();
        else
            itemConsume.maxInStack = 99;

        //<atributes>
        if (xmlReader.ReadToNextSibling("atributes") && xmlReader.ReadToDescendant("add"))//<add..
        {
            AtributeTypeItem tempType;
            float tempValue;
            do
            {
                tempType = (AtributeTypeItem)System.Enum.Parse(typeof(AtributeTypeItem), xmlReader.GetAttribute(0));
                tempValue = xmlReader.ReadElementContentAsFloat();
                itemConsume.itemAttributes.Add(new AttributeItem(tempType, tempValue));
            } while (xmlReader.ReadToNextSibling("add"));
        }

        xmlReader.Close();
        return itemConsume;
    }

}

