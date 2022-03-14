﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// This class is performed after all other done. 
/// It is set in "Script execution order"
/// This was done deliberately in order to upload the data in the last turn
/// </summary>
public class SaveLoadPlayer : MonoBehaviour
{

    private string nameSaveStats = "QWE84RWE5TRETRE7Y";
    private string nameSaveBag = "RT43ERET54EWERGRET6";
    private string nameSaveEquip = "WERGF8DFG45RTYR2TY";
    private string nameSaveStorage = "QWEDSFSS7784RTER5T";


    private StatusPlayer _StatusPlayer;

    void Start()
    {
        _StatusPlayer = gameObject.GetComponent<StatusPlayer>();
        //_PlayerGui.InvBag.InventoryOpen += LoadItemsInBag;

        nameSaveStats += _StatusPlayer.personalName + ".stat";
        nameSaveBag += _StatusPlayer.personalName + ".bag";
        nameSaveEquip += _StatusPlayer.personalName + ".equip";
        nameSaveStorage += _StatusPlayer.personalName + ".stor";

        if (System.IO.File.Exists(System.IO.Path.Combine(Application.persistentDataPath, nameSaveBag)))
            LoadBagFromSave();

        if (System.IO.File.Exists(System.IO.Path.Combine(Application.persistentDataPath, nameSaveEquip)))
            LoadEquipFromSave();
    }

    // Save data player in files
    public void SavePlayerData()
    {
        SaveItemsFromBag();
        SaveItemsFromEquip();
    }
    
    // Inventory Bag items
    public void LoadBagFromSave()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(InventoryBagData));
        string path = System.IO.Path.Combine(Application.persistentDataPath, nameSaveBag);
        FileStream stream = new FileStream(path, FileMode.Open);
        PlayerInterface.Instance.bagInv.LoadSerialization(serializer.Deserialize(stream) as InventoryBagData);
        stream.Close();
        PlayerInterface.Instance.bagInv.InventoryInit -= LoadBagFromSave;
    }
    public void SaveItemsFromBag()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(InventoryBagData));
        string path = System.IO.Path.Combine(Application.persistentDataPath, nameSaveBag);
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, PlayerInterface.Instance.bagInv.GetInventoryData());
        stream.Close();
    }

    // Inventory Character equiped items
    public void LoadEquipFromSave()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(InventoryCharacterData));
        string path = System.IO.Path.Combine(Application.persistentDataPath, nameSaveEquip);
        FileStream stream = new FileStream(path, FileMode.Open);
        PlayerInterface.Instance.characterInv.LoadSerialization(serializer.Deserialize(stream) as InventoryCharacterData);
        stream.Close();
        PlayerInterface.Instance.characterInv.InventoryInit -= LoadEquipFromSave;
    }
    public void SaveItemsFromEquip()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(InventoryCharacterData));
        string path = System.IO.Path.Combine(Application.persistentDataPath, nameSaveEquip);
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, PlayerInterface.Instance.characterInv.GetInventoryData());
        stream.Close();
    }

}
