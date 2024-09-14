using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVariedAttacks
{
    float longAttackRange { get; set; }
    float mediumAttackRange { get; set; }
    float closeAttackRange { get; set; }
    void longAttack();
    void mediumAttack();
    void closeAttack();
}
