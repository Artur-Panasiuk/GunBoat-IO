using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Ship
{
    private int exp;
    shipLevelData lvlData;
    private int currentScore;
    private int newScore;
    private int nextLvlPtsReq;
    private pointsController ptsCont;
    private levelController lvlCont;

    public GameObject cannonballAttackPrefab;
    private cannonBallAttack cannonsAttack;
    public Sprite cannonballAttackIcon;
    public Transform gunExplosion;

    private bool isTurningLeft;
    private bool isTurningRight;

    public void FireLeft()
    {
        guns[selectedGunIndex].Shoot(Gun.GunOrientation.LEFT);
    }
    public void FireRight()
    {
        guns[selectedGunIndex].Shoot(Gun.GunOrientation.RIGHT);
    }
    public void TurnLeft()
    {
        isTurningLeft = true;
    }
    public void TurnRight()
    {
        isTurningRight = true;
    }
    public void OnLeftButtonUp()
    {
        isTurningLeft = false;
    }
    public void OnRightButtonUp()
    {
        isTurningRight = false;
    }
    public void turnUp() 
    {
        if (currentSpeedLevel < SpeedLevel.MAX)
        {
            currentSpeedLevel++;
        }
    }
    public void turnDown() 
    {
        if (currentSpeedLevel > SpeedLevel.STOP)
        {
            currentSpeedLevel--;
        }
    }
    public void handleTurning()
    {
        if (isTurningLeft)
        {
            transform.Rotate(Vector3.forward, 1 * baseRotationSpeed * Time.deltaTime);
        }
        else if (isTurningRight)
        {
            transform.Rotate(Vector3.forward, -1 * baseRotationSpeed * Time.deltaTime);
        }
    }
    //PC TESTING
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && currentSpeedLevel < SpeedLevel.MAX)
        {
            currentSpeedLevel++;
        }
        if (Input.GetKeyDown(KeyCode.S) && currentSpeedLevel > SpeedLevel.STOP)
        {
            currentSpeedLevel--;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            guns[selectedGunIndex].Shoot(Gun.GunOrientation.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            guns[selectedGunIndex].Shoot(Gun.GunOrientation.RIGHT);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ptsCont.increasePoints(100);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ptsCont.increasePoints(500);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            avaiableLevelPoints++;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            avaiableLevelPoints += 5;
        }

        float rotationInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.forward, -rotationInput * baseRotationSpeed * Time.deltaTime);
    }
    public SpeedLevel getSpeedLevel() 
    {
        return currentSpeedLevel;
    }
    public Sprite getCurrentGunIcon()
    {
        return guns[selectedGunIndex].GetIcon();
    }
    private void ResetScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }
    protected override void CheckDeath()
    {
        if(hp <= 0)
        {
            SceneManager.LoadScene("Menu");
        }
    }
    public void statsInit()
    {
        exp = 0;
        level = 1;
        avaiableLevelPoints = 0;
        currentScore = 0;
        nextLvlPtsReq = (int)lvlCont.calculateScalableValue(level + 1, 100, 1.4f);
    }
    public void statsUpdate()
    {
        newScore = ptsCont.getScore();
        if (newScore != currentScore)
        {
            exp += newScore - currentScore;
            currentScore = newScore;
            while(exp >=  nextLvlPtsReq)
            {
                level += 1;
                avaiableLevelPoints += 1;
                exp -= nextLvlPtsReq;
                nextLvlPtsReq = (int)lvlCont.calculateScalableValue(level, 100, 1.4f);
            }
        }
    }
    public void increaseHpLvl()
    {
        lvlCont.upgradeStatistic(ref lvlData.hp_lvl, ref avaiableLevelPoints);
        updateStats();
    }
    public void increaseRegenLvl()
    {
        lvlCont.upgradeStatistic(ref lvlData.regeneration_lvl, ref avaiableLevelPoints);
        updateStats();
    }
    public void increaseSpdLvl()
    {
        lvlCont.upgradeStatistic(ref lvlData.speed_lvl, ref avaiableLevelPoints);
        updateStats();
    }
    public void increaseSCDLvl()
    {
        lvlCont.upgradeStatistic(ref lvlData.side_cannon_dmg_lvl, ref avaiableLevelPoints);
        updateStats();
    }
    public void increaseFCDLvl()
    {
        lvlCont.upgradeStatistic(ref lvlData.front_cannon_dmg_lvl, ref avaiableLevelPoints);
        updateStats();
    }
    public void increaseRamLvl()
    {
        lvlCont.upgradeStatistic(ref lvlData.ram_dmg_lvl, ref avaiableLevelPoints);
        updateStats();
    }
    public void updateStats()
    {
        ShipDataInterface shipInfo = GetBaseClassStatistics("Player");
        GunDataInterface gunInfo = cannonsAttack.GetBaseClassParameters("cannonballs");

        StartingHP = lvlCont.calculateScalableValue(lvlData.hp_lvl, shipInfo.hp, 1.2f);
        baseForwardSpeed = lvlCont.calculateScalableValue(lvlData.speed_lvl, shipInfo.baseForwardSpeed, 1.6f);
        cannonsAttack.updateDamage(lvlCont.calculateScalableValue(lvlData.side_cannon_dmg_lvl, gunInfo.damage, 1.6f));
        updateGunList();

        //TO DO:
        //-ADD REGEN, FCD, RAM
        regenerationValue = lvlCont.calculateScalableValue(lvlData.regeneration_lvl, 0.05f, 1.25f);
    }
    private void updateRegeneration()
    {
        if(hp < StartingHP)
        {
            hp += regenerationValue;
        }
    }
    private void updateGunList()
    {
        guns = new List<Gun>
        {
            cannonsAttack
        };
    }
    public void printInfo()
    {
        Debug.Log($"Lvl: {level} ({currentScore}/{nextLvlPtsReq});\n" +
            $"HP: {hp} ({lvlData.hp_lvl})\n" +
            $"Speed: {baseForwardSpeed} ({lvlData.speed_lvl})\n" +
            $"SCD: {guns[selectedGunIndex].GetDamageValue()} ({lvlData.side_cannon_dmg_lvl})");
    }
    public float GetNextLevelRequiredPoints()
    {
        return nextLvlPtsReq;
    }
    public int GetLevel()
    {
        return level;
    }
    public int GetExp()
    {
        return exp;
    }
    public float GetStartingHP()
    {
        return StartingHP;
    }
    public int GetAvaiablePoints()
    {
        return avaiableLevelPoints;
    }
    public shipLevelData GetShipLevelData()
    {
        return lvlData;
    }
    void Start()
    {
        baseStart();

        SetBaseClassStatistics("Player");

        ptsCont = GameObject.Find("ScriptObjectAnchor").GetComponent<pointsController>();
        lvlCont = GameObject.Find("ScriptObjectAnchor").GetComponent<levelController>();

        statsInit();

        cannonsAttack = gameObject.AddComponent<cannonBallAttack>();
        cannonsAttack.SetBaseClassParameters("cannonballs", gameObject, cannonballAttackPrefab, gunExplosion);
        cannonsAttack.SetIcon(cannonballAttackIcon);

        updateGunList();
    }
    void Update()
    {
        baseUpdate();
        handleTurning();
        statsUpdate();
        updateRegeneration();
    }
}
