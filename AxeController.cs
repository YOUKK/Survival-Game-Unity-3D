﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    // 활성화 여부
    public static bool isActivate = false;

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
                isSwing = false; // 데미지를 한 번만 주기 위해 씀
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }
}