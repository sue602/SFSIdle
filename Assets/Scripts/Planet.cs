using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float OrbitRadius = 2f;
    public Vector2 ScaleRange;
    private SpaceObject star;

    private void Start()
    {
        float scaleValue = Random.Range(ScaleRange.x, ScaleRange.y);
        transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

        star = transform.parent.GetComponent<Star>();

        var angle = Random.Range(0, 2 * Mathf.PI);
        var x = Mathf.Cos(angle) * OrbitRadius;
        var z = Mathf.Sin(angle) * OrbitRadius;

        Vector3 randomPoint = new Vector3(x, transform.position.y, z) + star.transform.position;
        transform.position = randomPoint;
    }
}
