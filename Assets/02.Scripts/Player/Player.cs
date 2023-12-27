using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 5;

    void Update()
    {
        CharacterMovement();
    }

    void CharacterMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        transform.position += (new Vector3(x, y)).normalized * Time.deltaTime * speed;
    }
}
