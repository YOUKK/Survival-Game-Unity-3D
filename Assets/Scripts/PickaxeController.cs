using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    // 활성화 여부
    public static bool isActivate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    void Update()
    {
        if (isActivate)
        {
            TryAttack();
        }
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                // 충돌했음
                if(hitInfo.transform.tag == "Rock")
				{
                    hitInfo.transform.GetComponent<Rock>().Mining();
				}
                else if(hitInfo.transform.tag == "WeakAnimal")
				{
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }               

                isSwing = false; // 데미지를 한 번만 주기 위해 씀
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
