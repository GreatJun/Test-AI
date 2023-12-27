using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimRotation : MonoBehaviour
{
    private CharacterController _controller;

    private SpriteRenderer _sprite;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _controller.OnLookEvent += OnAim;
    }

    public void OnAim(Vector2 newAimDirection)
    {
        RotateAim(newAimDirection);
    }

    private void RotateAim(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _sprite.flipX = Mathf.Abs(rotZ) > 90;
    }
}
