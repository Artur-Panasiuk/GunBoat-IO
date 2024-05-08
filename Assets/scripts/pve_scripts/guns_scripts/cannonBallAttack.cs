using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Gun;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class cannonBallAttack : Gun
{
    public override void Shoot(GunOrientation orientation)
    {
        if (cooldownTimer <= 0f)
        {
            GameObject cannonballAttackObject = Instantiate(attackObj, transform.position, Quaternion.identity);

            if (orientation == GunOrientation.LEFT)
            {
                cannonballAttackObject.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 180f);
            }
            else
            {
                cannonballAttackObject.transform.rotation = transform.rotation;
            }

            cannonballAttackObject.transform.parent = transform;

            cooldownTimer = cooldown;

            cannonballAttackObject.GetComponentInChildren<ParticleSystem>().Play();

            Destroy(cannonballAttackObject, attackTime);
        }
    }
    void Update()
    {
        baseUpade();
    }
}
