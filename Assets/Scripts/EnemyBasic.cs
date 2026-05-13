using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  With the implementation of an enemy base super class
//  this means the script for the basic enemy can be condensed down.
public class EnemyBasic : EnemyBase
{
    protected override void Move()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}
