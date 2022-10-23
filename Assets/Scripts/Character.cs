using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private bool walk;
    [SerializeField] private float speed;
    private Animator myAnimator;
    private string animBoolWalk = "Walk";

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        myAnimator.SetBool(animBoolWalk, walk);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }
}
