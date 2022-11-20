using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDamageEffect : MonoBehaviour
{
    [SerializeField] private float damageValue;
    private TextMeshPro textMesh;
    
    [SerializeField] private float alphaValue = 1;
    [SerializeField] private float alphaMinus;
    [SerializeField] private float speed;

    private Color oldColor;

    private void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        oldColor = textMesh.color;
        textMesh.text = String.Format("{0:0.0}", damageValue).ToString();
    }

    private void Update()
    {
        Logic();
    }

    private void Logic()
    {
        alphaValue = textMesh.color.a;
        float alphaNew = alphaValue - alphaMinus;

        if (alphaValue > 0)
        {
            textMesh.color = new Color(oldColor.r, oldColor.g, oldColor.b, alphaNew);
        }
        else
        {
            Destroy(transform.gameObject);
        }

        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    public void SetDamage(float damage)
    {
        damageValue = damage;
    }
}
