using UnityEngine;
using System.Collections;
public class PickUpItem : MonoBehaviour
{
    //public Item item;
    //private Inventory _inventory;
    //private GameObject _player;
    //// Use this for initialization

    //void Start()
    //{
    //    _player = GameObject.FindGameObjectWithTag("Player");
    //    if (_player != null)
    //        _inventory = _player.GetComponent<PlayerGui>().invBag.GetComponent<Inventory>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (_inventory != null && Input.GetKeyDown(KeyCode.E))
    //    {
    //        float distance = Vector3.Distance(this.gameObject.transform.position, _player.transform.position);

    //        if (distance <= 3)
    //        {
    //            bool check = _inventory.checkIfItemAllreadyExist(item.id, item.quantity);
    //            if (check)
    //                Destroy(this.gameObject);
    //            else if (_inventory.ItemsInInventory.Count < (_inventory.width * _inventory.height))
    //            {
    //                _inventory.addItemToInventory(item.id, item.quantity);
    //                _inventory.updateItemList();
    //                _inventory.stackableSettings();
    //                Destroy(this.gameObject);
    //            }

    //        }
    //    }
    //}

}