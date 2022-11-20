using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShark : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float stopDistance;
    [SerializeField] private float activateTimeRemaining;
    [SerializeField] private float activateTimeRemainingMax;
    [SerializeField] private bool nearTarget = false;
    [SerializeField] private float nearTargetTimeRemaining = -1; // must be -1 before using
    [SerializeField] private float nearTargetTimeRemainingMax;
    [SerializeField] private bool activated = false;

    [SerializeField] private Transform target;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Vector3 weaponPosition;
    [SerializeField] private float canShootTimeRemaining = 0;
    [SerializeField] private float canShootTimeRemainingMax;
    [SerializeField] private bool canShoot = false;
    private float weaponSpeed = 30;
    private GameCycleManager gameCycleManager;

    private void Start()
    {
        activateTimeRemaining = activateTimeRemainingMax;
        canShootTimeRemaining = canShootTimeRemainingMax;
        transform.rotation = new Quaternion(0, 180, 0, 1);
        gameCycleManager = FindObjectOfType<GameCycleManager>();
    }

    private void Update()
    {
        speed = gameCycleManager.valueSpeed;

        if (canShootTimeRemaining > 0)
        {
            canShoot = false;
            canShootTimeRemaining -= Time.deltaTime;
        }
        else
        {
            canShoot = true;
            canShootTimeRemaining = canShootTimeRemainingMax;
        }

        if (activateTimeRemaining > 0)
        {
            activated = false;
        }
        else
        {
            activated = true;
        }

        if (activated)
        {
            if (target == null)
            {
                var asteroidsArray = FindObjectsOfType<Asteroid>();
                target = GetClosestAsteroid(asteroidsArray);
            }
            else
            {
                if (target != null)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                    if (distanceToTarget >= stopDistance)
                    {
                        nearTarget = false;
                        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
                        transform.LookAt(target.transform);
                    }
                    else
                    {
                        nearTarget = true;
                    }
                }

                if (nearTarget)
                {
                    Shoot(weaponSpeed);
                    if (nearTargetTimeRemaining == -1)
                    {
                        nearTargetTimeRemaining = nearTargetTimeRemainingMax;
                        var asteroidsArray = FindObjectsOfType<Asteroid>();
                        target = GetClosestAsteroid(asteroidsArray);
                    }
                    else
                    {
                        if (nearTargetTimeRemaining > 0)
                        {
                            nearTargetTimeRemaining -= Time.deltaTime;
                        }
                        else
                        {
                            // move to home craft
                            target = FindObjectOfType<Planet>().transform;
                            nearTargetTimeRemaining = -1;
                            nearTarget = false;
                        }
                    }
                }
            }
        }
        else
        {
            activateTimeRemaining -= Time.deltaTime;
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }
    }

    private Transform GetClosestAsteroid(Asteroid[] asteroids)
    {
        Asteroid tMin = null;
        Transform tMinTransform = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Asteroid t in asteroids)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }

        if(tMin != null)
        {
            tMinTransform = tMin.transform;
        }

        return tMinTransform;
    }

    private void Shoot(float speedValue)
    {
        if (canShoot)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.SetDamage(gameCycleManager.valueStrength);
            bullet.SetTarget(target, weaponSpeed);
        }
    }
}
