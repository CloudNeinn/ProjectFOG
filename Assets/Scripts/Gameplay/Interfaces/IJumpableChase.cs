using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJumpableChase : IJumpable
{
    float jumpNodeHeightRequirement { get; set; } //0.8f
    float getJumpModifier();
}
