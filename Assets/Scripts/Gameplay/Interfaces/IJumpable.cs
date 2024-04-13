using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJumpable
{
    float jumpCooldown { get; set; }
    float jumpTimer { get; set; }

    float getJumpModifier();
    void Jump(float jumpStrength);
}
