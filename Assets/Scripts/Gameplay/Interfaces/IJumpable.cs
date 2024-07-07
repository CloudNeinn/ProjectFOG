using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJumpable
{
    float jumpCooldown { get; set; }
    float jumpTimer { get; set; }
    bool jumpEnabled { get; set; } 
    float jumpModifier { get; set; } // 0.3f

    void Jump(float jumpStrength);
}
