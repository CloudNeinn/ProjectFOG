using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVariedAttacksRadial : IVariedAttacks
{
    float shortAttackRadius {get; set;}
    float mediumAttackRadius {get; set;}
    float longAttackRadius {get; set;}
}
