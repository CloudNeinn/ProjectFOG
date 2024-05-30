using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    float attackTime { get; set; }
    float attackCooldown { get; set; }
    float forgetTime { get; set; }
    float forgetCooldown { get; set; }
    float damage { get; set; }
    float knockStrengthX { get; set; }
    float knockStrengthY { get; set; }
    float blockKnockStrengthX { get; set; }
    float blockKnockStrengthY { get; set; }
    bool canAttack { get; set; }

    void Attack();

}
