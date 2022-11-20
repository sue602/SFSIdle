using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float speed = 0;
    [SerializeField] private float speedMin = 1;
    [SerializeField] private float speedMax = 1.5f;
    public bool Inverse = false;
    private void Start()
    {
        speed = Random.Range(speedMin, speedMax);
        int randomDirection = Random.Range(0, 2);
        if(randomDirection == 0)
        {
            speed = -speed;
        }
        else
        {
            speed = +speed;
        }
    }

    private void Update()
    {
        target = FindObjectOfType<Planet>().transform;
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
    }

    public void SetTarget(Transform value)
    {
        target = value;
    }
}
