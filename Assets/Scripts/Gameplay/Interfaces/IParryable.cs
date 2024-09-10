using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParryable : IBlockable
{
    bool isParrying { get; set;}
    float parryTime { get; set;}
    void Parry();
}
