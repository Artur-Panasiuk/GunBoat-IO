using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public class GunDataListInterface
{
    public GunDataInterface[] guns;
}
[System.Serializable]
public class GunDataInterface
{
    public string gunName;
    public float cooldown;
    public float attackTime;
    public int damage;
}
public class Gun : MonoBehaviour
{
    protected GameObject attackObj;
    private UnityEngine.Transform explosions;
    protected float damage;
    protected float attackTime;
    protected float cooldown;
    protected float cooldownTimer = 0f;
    public string gunName;
    protected GameObject owner;
    protected Sprite icon;

    public enum GunOrientation
    {
        LEFT,
        RIGHT,
        FRONT,
        BACK,
        CUSTOM
    }

    public GunOrientation gunOrientation;
    public void SetBaseClassParameters(string defName, GameObject newOwner, GameObject attackPrefab, UnityEngine.Transform explosion)
    {
        statsFileLoader loader = GameObject.Find("ScriptObjectAnchor").GetComponent<statsFileLoader>();
        GunDataInterface singleGun = loader.getGunByName(defName);

        owner = newOwner;
        gunName = singleGun.gunName;
        attackObj = attackPrefab;
        attackTime = singleGun.attackTime;
        cooldown = singleGun.cooldown;
        damage = singleGun.damage;
        explosions = explosion;
    }
    public GunDataInterface GetBaseClassParameters(string defName)
    {
        statsFileLoader loader = GameObject.Find("ScriptObjectAnchor").GetComponent<statsFileLoader>();
        return loader.getGunByName(defName);
    }
    public void updateDamage(float dmg)
    {
        damage = dmg;
    }
    public (string, GameObject, float, float) GetClassParameters()
    {
        return (gunName, attackObj, attackTime, cooldown);
    }
    public virtual void Shoot(GunOrientation orientation)
    {
        Debug.Log("BAM");
    }
    public void SetIcon(Sprite ic)
    {
        icon = ic;
    }
    public Sprite GetIcon()
    {
        return icon;
    }
    public float GetCoolDownValues()
    {
        return cooldownTimer;
    }
    public float GetDamageValue()
    {
        return damage;
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Ship ship = other.gameObject.GetComponent<Ship>();

        if (ship != null && ship != owner)
        {
            /*
            UnityEngine.Transform collisionEffect = Instantiate(explosions, other.transform.position, Quaternion.identity);
            ParticleSystem particleSystem = collisionEffect.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.Play();
            Destroy(collisionEffect.gameObject, particleSystem.main.duration);
            */

            ship.ReduceHp(damage);
        }
    }
    public void baseUpade()
    {
        cooldownTimer = Mathf.Max(0f, cooldownTimer - Time.deltaTime);
    }
}
