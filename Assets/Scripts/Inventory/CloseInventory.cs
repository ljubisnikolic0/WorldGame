using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CloseInventory : MonoBehaviour, IPointerClickHandler
{

    Inventory inv;
    void Start()
    {
        inv = transform.parent.GetComponent<Inventory>();

    }
		
	public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inv.CloseInventory();
        }
    }
}
