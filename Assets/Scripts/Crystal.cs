using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private bool moveToAccount = false;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 targetPosition;
    private UIManager uiManager;
    private Rigidbody myRigidbody;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        RandomDestroy();
    }

    private void Update()
    {
        MoveToAccount();
    }

    public void Activate()
    {
        moveToAccount = true;
    }

    private void MoveToAccount()
    {
        if (moveToAccount)
        {

        }
    }

    private void RandomDestroy()
    {
        int randomDestroy = Random.Range(0, 4);

        if(randomDestroy == 1)
        {
            Destroy(transform.gameObject);
        }
    }
}
