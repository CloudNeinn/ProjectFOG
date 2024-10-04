using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVariedAttacksRadial : IVariedAttacks
{
    float longAttackRadius { get; set; }
    float mediumAttackRadius { get; set; }
    float shortAttackRadius { get; set; }
}
