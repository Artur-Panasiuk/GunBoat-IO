using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Enemy;

public class Enemy : Ship
{
    public GameObject player;
    const float rotationToPlayerTreshold = 1f;
    protected EnemyState enemyState = EnemyState.CHASE;
    protected bool isFacingPL = false;

    public TextMeshPro enemyInfoText;

    protected bool hasShot = false;
    public float shootRange = 12f;
    protected bool IsAggressive = true;

    public enum EnemyState
    {
        IDLE,
        WANDER,
        CHASE,
        AIMSHOOT,
        ESCAPE
    }
    public virtual void StateMachine()
    {
        switch (enemyState)
        {
            case EnemyState.IDLE:
                currentSpeedLevel = SpeedLevel.STOP;
                break;

            case EnemyState.WANDER:
                currentSpeedLevel = SpeedLevel.MID;
                break;

            case EnemyState.CHASE:
                RotateTowardsPlayer();
                if (IsFacingPlayer())
                {
                    isFacingPL = true;
                    currentSpeedLevel = SpeedLevel.MAX;
                }
                else
                {
                    isFacingPL = false;
                }

                break;

            case EnemyState.AIMSHOOT:
                Gun.GunOrientation orientation = RotateSidewaysToPlayer();
                (bool isLeft, bool isRight) = IsFacingPlayerSideways();

                if (isLeft && orientation == Gun.GunOrientation.LEFT)
                {
                    guns[selectedGunIndex].Shoot(Gun.GunOrientation.LEFT);
                    hasShot = true;
                }
                if (isRight && orientation == Gun.GunOrientation.RIGHT)
                {
                    guns[selectedGunIndex].Shoot(Gun.GunOrientation.RIGHT);
                    hasShot = true;
                }

                break;

            case EnemyState.ESCAPE:
                rotateAwayFromPlayer();
                currentSpeedLevel = SpeedLevel.MAX;

                break;
        }

    }
    public void setState(EnemyState st)
    {
        enemyState = st;
    }
    public float GetRotationToPlayer()
    {
        Vector2 directionToPlayer = player.transform.position - transform.position;
        float retAngle = (Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg) - 90;
        return retAngle;
    }
    public void RotateTowardsPlayer()
    {
        Quaternion targetRotation = Quaternion.AngleAxis(GetRotationToPlayer(), Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, baseRotationSpeed * Time.deltaTime);
    }
    public void rotateAwayFromPlayer()
    {
        Quaternion targetRotation = Quaternion.AngleAxis(GetRotationToPlayer() + 180, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, baseRotationSpeed * Time.deltaTime);
    }
    public Gun.GunOrientation RotateSidewaysToPlayer()
    {
        Gun.GunOrientation orientation;
        float rotationDirection;
        float currentRotation = transform.eulerAngles.z;
        float targetRotation = GetRotationToPlayer() + 90;

        float clockwiseRotation = (targetRotation - currentRotation + 360) % 360;
        float counterclockwiseRotation = (currentRotation - targetRotation + 360) % 360;

        bool rotateClockwise = clockwiseRotation < counterclockwiseRotation;

        if (rotateClockwise)
        {
            rotationDirection = -1f;
            orientation = Gun.GunOrientation.RIGHT;
        }
        else
        {
            rotationDirection = 1f;
            orientation = Gun.GunOrientation.LEFT;
        }

        transform.Rotate(Vector3.forward * rotationDirection * baseRotationSpeed * Time.deltaTime);

        return orientation;
    }
    public bool IsFacingPlayer()
    {
        Vector2 directionToPlayer = player.transform.position - transform.position;
        float angle = Vector2.Angle(transform.right, directionToPlayer.normalized) - 90;

        return Mathf.Abs(angle) < rotationToPlayerTreshold;
    }
    (bool, bool) IsFacingPlayerSideways()
    {
        Vector2 directionToPlayer = player.transform.position - transform.position;
        float left = Vector2.Angle(transform.right, directionToPlayer.normalized);
        float right = left + 180;

        bool isLeft = Mathf.Abs(left) > rotationToPlayerTreshold;
        bool isRight = Mathf.Abs(right) < rotationToPlayerTreshold;

        return (isLeft, isRight);
    }
    public float GetDistanceFromPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        return distance;
    }
    public bool IsOutsideCameraView()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        Bounds bounds = GetComponent<Collider2D>().bounds;

        if (!GeometryUtility.TestPlanesAABB(planes, bounds))
        {
            return true;
        }

        return false;
    }
    private void UpdateUI()
    {
        enemyInfoText.text = $"{(int)hp} HP";
        enemyInfoText.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void enemyStart()
    {
        baseStart();
    }

    public void enemyUpdate()
    {
        baseUpdate();
        StateMachine();
        UpdateUI();
    }
}