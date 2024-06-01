using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoxSeeable : ISeeable
{
    Vector3 attackBoxSize { get; set; }
    float attackDistance { get; set; }
    Vector3 sightBoxSize { get; set; }
    float sightDistance { get; set; }
}
