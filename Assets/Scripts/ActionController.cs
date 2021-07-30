using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; // 습득 가능한 최대 거리

    private bool pickupActivated = false; // 습득 가능할 시 true

    private RaycastHit hitinfo; // 충돌체 정보 저장

    // 아이템 레이어에만 반응하도록 레이어 마스크를 설정
    [SerializeField]
    private LayerMask layerMask;

    // 필요한 컴포넌트
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;


    void Update()
    {
        TryAction();
        CheckItem();
    }

    private void TryAction()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
            CheckItem();
            CanPickUp();
		}
	}

    private void CanPickUp()
	{
		if (pickupActivated)
		{
            if(hitinfo.transform != null)
			{
                Debug.Log(hitinfo.transform.GetComponent<ItemPickup>().item.itemName + " 획득했습니다");
                theInventory.AcquireItem(hitinfo.transform.GetComponent<ItemPickup>().item);
                Destroy(hitinfo.transform.gameObject);
                InfoDisappear();
			}
		}
	}

    private void CheckItem()
	{
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo, range, layerMask))
		{
            if(hitinfo.transform.tag == "Item")
			{
                ItemInfoAppear();
			}
		}
		else
		{
            InfoDisappear();
		}
	}

    private void ItemInfoAppear()
	{
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitinfo.transform.GetComponent<ItemPickup>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
	}

    private void InfoDisappear()
	{
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

}
