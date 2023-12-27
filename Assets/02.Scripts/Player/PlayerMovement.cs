using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;

    private Vector2 _moveDirection = Vector2.zero;
    private Rigidbody2D _rigidbody;

    private float speed = 5;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _controller.OnMoveEnent += Move;
    }

    private void FixedUpdate()
    {
        ApplyMoveLogic(_moveDirection);
    }

    private void Move(Vector2 direction)
    {
        _moveDirection = direction;
    }

    private void ApplyMoveLogic(Vector2 direction)
    {
        direction *= speed;
        _rigidbody.velocity = direction;
    }
}
