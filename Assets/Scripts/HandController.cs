using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // 활성화 여부
    public static bool isActivate = false;

    // 현재 장착된 Hand형 타입 무기
    [SerializeField]
    private Hand currentHand;

    // 공격 중
    private bool isAttack = false; // false일 때 공격
    private bool isSwing = false;

    private RaycastHit hitInfo;

 
    void Update()
    {
        if (isActivate)
        {
            TryAttack();
        }
    }

    private void TryAttack()
	{
        if (Input.GetButton("Fire1"))
		{
			if (!isAttack)
			{
                // 코루틴 실행
                StartCoroutine(AttackCoruntine());
			}
		}
	}
    IEnumerator AttackCoruntine()
	{
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);


        isAttack = false;
	}

    IEnumerator HitCoroutine()
	{
		while (isSwing)
		{
			if (CheckObject())
			{
                // 충돌했음
                isSwing = false; // 데미지를 한 번만 주기 위해 씀
                Debug.Log(hitInfo.transform.name);
			}
            yield return null;
		}
	}

    private bool CheckObject()
	{
        // 충돌할 게 있다
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
		{
            return true;
        }

        // 충돌할 게 없다
        return false;
	}

    public void HandChange(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentHand = _hand;
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero;
        currentHand.gameObject.SetActive(true);
        isActivate = true;
    }
}
