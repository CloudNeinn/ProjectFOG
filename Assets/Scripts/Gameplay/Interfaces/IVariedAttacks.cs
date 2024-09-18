using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVariedAttacks
{
    Vector2 longAttackRangeBox { get; set; }
    Vector2 longAttackRange { get; set; }
    Vector2 mediumAttackRangeBox { get; set; }
    Vector2 mediumAttackRange { get; set; }
    Vector2 closeAttackRangeBox { get; set; }
    Vector2 closeAttackRange { get; set; }
    void longAttack();
    void mediumAttack();
    void closeAttack();
}
