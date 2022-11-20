using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : SpaceObject
{
    public float OrbitRadius = 10f;
    [SerializeField] private Planet center;
    public float HPValue = 10;
    public float HPMax = 10;
    [SerializeField] private Transform HPBar;
    [SerializeField] private Color hpColorFull;
    [SerializeField] private Color hpColorHalf;
    [SerializeField] private Color hpColorMinimal;
    private SpriteRenderer hpRenderer;

    [SerializeField] private Vector2 scaleRange;

    [SerializeField] private AsteroidDestroyed AsteroidDestroyedPrefab;
    [SerializeField] private TextDamageEffect damageEffect;

    void Start()
    {
        Init();
    }

    void LateUpdate()
    {
        HPBar.LookAt(Camera.main.transform.position, -Vector3.up);
    }

    private void Init()
    {
        //float scaleValue = Random.Range(ScaleRange.x, ScaleRange.y);
        //transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

        center = FindObjectOfType<Planet>();

        var angle = Random.Range(0, 2 * Mathf.PI);
        var x = Mathf.Cos(angle) * OrbitRadius;
        var z = Mathf.Sin(angle) * OrbitRadius;

        Vector3 randomPoint = new Vector3(x, transform.position.y, z) + center.transform.position;
        transform.position = randomPoint;
        Orbit orbit = GetComponent<Orbit>();
        orbit.SetTarget(center.transform);
        hpRenderer = HPBar.GetComponent<SpriteRenderer>();

        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        ColorCalculation();
    }

    public void TakeDamage(float damage)
    {
        var effect = Instantiate(damageEffect.gameObject, transform.position, Quaternion.identity);
        TextDamageEffect textDamageEffect = effect.GetComponent<TextDamageEffect>();
        textDamageEffect.SetDamage(damage);
        HPValue -= damage;
        ColorCalculation();
        float XSсale = 5;
        float HPXScale = Mathf.Lerp(0, XSсale,HPValue/ HPMax);
        //Размер по иксу = Mathf.Lerp(HPMin,HBarXScaleMax,здоровье / макс_здоровья)
        HPBar.localScale = new Vector3(HPXScale, HPBar.localScale.y, HPBar.localScale.z);

        if (HPValue <= 0)
        {
            SpawnDestroyedParts();
            Destroy(transform.gameObject);
        }

    }

    private void ColorCalculation()
    {
        float part = HPMax / 3; // 3 - part

        if (HPValue >= HPMax - part)
        {
            hpRenderer.color = hpColorFull;
        }
        else if (HPValue >= HPMax - (part * 2) && HPValue <= HPMax - part)
        {
            hpRenderer.color = hpColorHalf;
        }
        else
        {
            hpRenderer.color = hpColorMinimal;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.transform.GetComponent<Bullet>();

        if (bullet != null)
        {
            TakeDamage(bullet.damage);
        }
    }

    private void SpawnDestroyedParts()
    {
        var part = Instantiate(AsteroidDestroyedPrefab, transform.position, Quaternion.identity);
        part.transform.localScale = transform.localScale;
        ParticleSystem explosionEffect = part.GetComponentInChildren<ParticleSystem>();
        explosionEffect.transform.localScale = transform.localScale;

        var parts = part.GetComponentsInChildren<Rigidbody>();

        float power = 500;
        float radius = 6;

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] != null)
            {
                parts[i].AddExplosionForce(power, transform.position, radius, 0);
            }
        }
    }
}
