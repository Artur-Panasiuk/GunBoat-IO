using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class BasicEnemy : Enemy
{
    public GameObject cannonballAttackPrefab;
    private cannonBallAttack cannonsAttack;
    public Transform gunExplosion;

    public void basicEnemyAI()
    {
        if (!IsAggressive && IsOutsideCameraView())
        {
            enemyState = EnemyState.IDLE;
        }

        if(!IsOutsideCameraView() && enemyState == EnemyState.IDLE)
        {
            enemyState = EnemyState.WANDER;
        }

        if(hp < StartingHP && enemyState == EnemyState.WANDER)
        {
            enemyState = EnemyState.CHASE;
            IsAggressive = true;
        }

        if (shootRange > GetDistanceFromPlayer() && enemyState == EnemyState.CHASE)
        {
            enemyState = EnemyState.AIMSHOOT;
        }

        if(hasShot && enemyState == EnemyState.AIMSHOOT)
        {
            hasShot = false;
            enemyState = EnemyState.ESCAPE;
        }

        if(shootRange < GetDistanceFromPlayer() && enemyState == EnemyState.ESCAPE)
        {
            enemyState = EnemyState.CHASE;
        }
    }
    void Start()
    {
        enemyStart();

        SetBaseClassStatistics("Schooner");

        cannonsAttack = gameObject.AddComponent<cannonBallAttack>();
        cannonsAttack.SetBaseClassParameters("cannonballs", gameObject, cannonballAttackPrefab, gunExplosion);

        guns = new List<Gun>
        {
            cannonsAttack
        };
    }
    void Update()
    {
        enemyUpdate();
        basicEnemyAI();
    }
}