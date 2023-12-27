using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;

    private Rigidbody2D _rigidbody;

    private Animator _animator;

    private float speed = 5;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _controller.OnMoveEvent += Move;
    }

    private void Move(Vector2 direction)
    {
        ApplyMoveLogic(direction);

        MoveAnimation(direction);
    }

    private void ApplyMoveLogic(Vector2 direction)
    {
        direction *= speed;
        _rigidbody.velocity = direction;
    }

    private void MoveAnimation(Vector2 direction)
    {
        if (direction.magnitude > 0.1)
            _animator.SetBool("IsRun", true);
        else
            _animator.SetBool("IsRun", false);
    }
}
