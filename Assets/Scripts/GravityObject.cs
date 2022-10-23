using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    public GameObject Planet;
    public new Rigidbody rigidbody;
    private Vector3 planetPos;
    private Transform gravityBody;
    private string planetTag = "Graviter";
    public float gravityForce;
    public bool Gravited;

    void Start()
    {
        // for not characters

        rigidbody = this.GetComponent<Rigidbody>();
        Planet = NearPlanet();
        transform.parent = Planet.transform; // Set parent
        Gravited = true;
        gravityForce = 600;

        // for not characters
    }

    void FixedUpdate()
    {
        if (Gravited)
        {
            Gravity();
        }
    }

    public GameObject NearPlanet()
    {
        float minDistance = 999999999;

        var Planets = GameObject.FindGameObjectsWithTag(planetTag);
        GameObject nearPlanet = null;
        for (int i = 0; i < Planets.Length; i++)
        {
            var planetDistance = Vector3.Distance(this.transform.position, Planets[i].transform.position);
            if (planetDistance < minDistance)
            {
                nearPlanet = Planets[i];
                minDistance = planetDistance;
            }
        }
        return nearPlanet;
    }


    void Gravity()
    {
        gravityBody = this.transform;
        planetPos = Planet.transform.position - gravityBody.position;

        rigidbody.AddForce(planetPos * gravityForce, ForceMode.Force); // Gravity

        Quaternion rotation = Quaternion.FromToRotation(-gravityBody.up, planetPos) * gravityBody.rotation;
        gravityBody.rotation = Quaternion.Slerp(gravityBody.rotation, rotation, 50 * Time.deltaTime); // Rotate body to planet
    }
}
