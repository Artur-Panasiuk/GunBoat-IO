using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ShipDataListInterface
{
    public ShipDataInterface[] ships;
}
[System.Serializable]
public class ShipDataInterface
{
    public string shipType;
    public float hp;
    public int points;
    public float baseForwardSpeed;
    public float baseForwardSpeedMultiplier;
    public float accelerationRate;
    public float baseRotationSpeed;
}
public class Ship : MonoBehaviour
{
    public struct shipLevelData
    {
        public int hp_lvl;
        public int regeneration_lvl;
        public int speed_lvl;
        public int side_cannon_dmg_lvl;
        public int front_cannon_dmg_lvl;
        public int ram_dmg_lvl;
    }

    public int level;
    public int avaiableLevelPoints;
    public float hp;
    protected int points;
    public float StartingHP;
    protected float baseForwardSpeed;
    protected float baseForwardSpeedMultiplier;
    protected float accelerationRate;
    protected float baseRotationSpeed;
    protected float regenerationValue;

    protected float currentSpeedValue;
    protected SpeedLevel currentSpeedLevel;

    public Transform frontFoam;
    public Transform backFoam;
    public Transform damageSmoke;

    protected ParticleController frontFoamCont;
    protected ParticleController backFoamCont;
    protected ParticleController damageSmokeCont;

    public Transform deathExplosion;

    protected int selectedGunIndex = 0;

    public enum SpeedLevel
    {
        STOP,
        MID,
        MAX
    }

    protected List<Gun> guns;
    protected void SetBaseClassStatistics(string type)
    {
        statsFileLoader loader = GameObject.Find("ScriptObjectAnchor").GetComponent<statsFileLoader>();
        ShipDataInterface singleShip = loader.getShipByType(type);

        hp = singleShip.hp;
        points = singleShip.points;
        baseForwardSpeed = singleShip.baseForwardSpeed;
        baseForwardSpeedMultiplier = singleShip.baseForwardSpeedMultiplier;
        accelerationRate = singleShip.accelerationRate;
        baseRotationSpeed = singleShip.baseRotationSpeed;

    }
    protected ShipDataInterface GetBaseClassStatistics(string type)
    {
        statsFileLoader loader = GameObject.Find("ScriptObjectAnchor").GetComponent<statsFileLoader>();
        return loader.getShipByType(type);
    }
    private void Move()
    {
        float targetSpeed = 0f;

        switch (currentSpeedLevel)
        {
            case SpeedLevel.STOP:
                targetSpeed = 0f;
                break;

            case SpeedLevel.MID:
                targetSpeed = baseForwardSpeed;
                break;

            case SpeedLevel.MAX:
                targetSpeed = baseForwardSpeed * baseForwardSpeedMultiplier;
                break;
        }

        currentSpeedValue = Mathf.MoveTowards(currentSpeedValue, targetSpeed, accelerationRate * Time.deltaTime);

        transform.Translate(Vector2.up * currentSpeedValue * Time.deltaTime);
    }
    private void UpdateParticle()
    {
        shipFoamParticle();
        shipSmokeParticle();
    }
    private void shipFoamParticle()
    {
        if (currentSpeedValue > 0)
        {
            frontFoamCont.isParticleActive = true;
            backFoamCont.isParticleActive = true;
        }
        else
        {
            frontFoamCont.isParticleActive = false;
            backFoamCont.isParticleActive = false;
        }
    }
    private void shipSmokeParticle()
    {
        if (hp <= StartingHP / 2)
        {
            damageSmokeCont.isParticleActive = true;
        }
        else
        {
            damageSmokeCont.isParticleActive = false;
        }
    }
    public void ReduceHp(float damage)
    {
        hp -= damage;
    }
    public float getHP()
    {
        return hp;
    }
    public SpeedLevel getCurrentSpeedLevel()
    {
        return currentSpeedLevel;
    }
    public Gun getCurrentGun()
    {
        return guns[selectedGunIndex];
    }
    protected virtual void CheckDeath()
    {
        if (hp <= 0)
        {
            Transform collisionEffect = Instantiate(deathExplosion, gameObject.transform.position, Quaternion.identity);
            ParticleSystem particleSystem = collisionEffect.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.Play();
            Destroy(collisionEffect.gameObject, particleSystem.main.duration);

            pointsController pointsController = FindAnyObjectByType<pointsController>();
            pointsController.increasePoints(points);

            Destroy(gameObject);
        }
    }
    public void baseStart()
    {
        StartingHP = hp;

        frontFoamCont = gameObject.AddComponent<ParticleController>();
        backFoamCont = gameObject.AddComponent<ParticleController>();
        damageSmokeCont = gameObject.AddComponent<ParticleController>();

        frontFoamCont.SetParticles(frontFoam);
        backFoamCont.SetParticles(backFoam);
        damageSmokeCont.SetParticles(damageSmoke);
    }
    public void baseUpdate()
    {
        Move();
        UpdateParticle();
        CheckDeath();   
    }
}
