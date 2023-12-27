using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEnent;

    public void CallMoveEnent(Vector2 direction)
    {
        OnMoveEnent?.Invoke(direction);
    }
}
