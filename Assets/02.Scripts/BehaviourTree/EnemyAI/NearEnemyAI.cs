using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NearEnemyAI : Enemy
{
    public NearEnemyAI() : base()
    {
        this._detectDistance = 4;
        this._attackDistance = 1;
        this._movementSpeed = 3;
    }
}
