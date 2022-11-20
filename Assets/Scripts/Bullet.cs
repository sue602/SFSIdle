using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;

    private float destroyTimeRemaining;
    private float destroyTimeRemainingMax = 10;

    private void Start()
    {
        destroyTimeRemaining = destroyTimeRemainingMax;
    }

    private void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        if (target != null)
        {
            transform.LookAt(target.transform);
        }

        if (destroyTimeRemaining > 0)
        {
            destroyTimeRemaining -= Time.deltaTime;
        }
        else
        {
            Destroy(transform.gameObject);
        }
    }

    public void SetTarget(Transform targetValue,float speedValue)
    {
        target = targetValue;
        speed = speedValue;
    }

    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(transform.gameObject);
    }
}
