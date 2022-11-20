using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Vector3 normal;
    [SerializeField] private float speed;
    [SerializeField] private bool walk;
    [SerializeField] private float distanceToStop;
    [SerializeField] private Vector3 targetPosition;
    private Animator MyAnimator;
    private string animBoolWalk = "Walk";
    private Planet myPlanet;
    public float changeDirectionTimeRemaining;
    public float changeDirectionTimeRemainingMax;
    private int walkOrStandRandom = 0;
    private int walkOrStandRandomMax = 2;
    private int walkRandomIndex = 1;


    private void Start()
    {
        MyAnimator = GetComponent<Animator>();
        myPlanet = FindObjectOfType<Planet>();

        SetRandomTargetPositionAtSphere();

        changeDirectionTimeRemaining = changeDirectionTimeRemainingMax;
    }

    private void Update()
    {
        Inputing();
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPosition, 0.05f);
    }

    private void Inputing()
    {
        if (changeDirectionTimeRemaining > 0)
        {
            changeDirectionTimeRemaining -= Time.deltaTime;
        }
        else
        {
            walkOrStandRandom = Random.Range(0, walkOrStandRandomMax);
            SetRandomTargetPositionAtSphere();

            if (walkOrStandRandom == walkRandomIndex)
            {
                walk = true;
            }
            else
            {
                walk = false;
            }
            changeDirectionTimeRemaining += changeDirectionTimeRemainingMax;
        }

        if(walkOrStandRandom == walkRandomIndex) // if random say Move!
        {
            if (Vector3.Distance(transform.position, targetPosition) > distanceToStop)
            {
                walk = true;
            }
            else
            {
                walk = false;
            }
        }

        Moving();
        RotateToTarget();
    }

    private void Moving()
    {
        MyAnimator.SetBool(animBoolWalk, walk);
        if (walk)
        {
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }
    }

    private void RotateToTarget()
    {
        Vector3 relative = transform.InverseTransformPoint(targetPosition);
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        float angleLerp = Mathf.Lerp(transform.rotation.y, angle, 0.2f);
        transform.Rotate(0, angleLerp, 0);
    }

    private void SetRandomTargetPositionAtSphere()
    {
        PlanetManager planetManager = myPlanet.GetComponent<PlanetManager>();
        targetPosition = planetManager.GetRandomVerticePosition();
    }
}
