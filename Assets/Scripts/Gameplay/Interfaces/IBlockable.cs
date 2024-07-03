using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockable
{
    bool blockHit { get; set;}
    bool isBlocking { get; set;} 
    float blockTime { get; set;}
    float blockCooldown { get; set;}
    float afterHitTime { get; set;}
    float afterHitCooldown { get; set;}
    float blockingSpeed { get; set;}
    Vector3 blockingBoxSize { get; set;}
    Vector3 blockingBoxOffset { get; set;}
    LayerMask blockingLayers { get; set;}
    float maxBlockForce { get; set;}
    void Block();
    bool inBlockingRange();
}
