using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] Weapon weapon;

    private void Start()
    {
        Debug.Log(weapon.PrintStats());
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.position += Vector3.forward * Input.GetAxis("Vertical") * _moveSpeed;
        }

        if(Input.GetAxis("Horizontal") != 0)
        {
            transform.position += Vector3.right * Input.GetAxis("Horizontal") * _moveSpeed;
        }
    }
}
