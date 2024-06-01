using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingRanged : EnemyFlyingAttacker
{
    public bool inSight()
    { 
        return false;
    }
    public bool inRange()
    { 
        return false;
    }    
    public bool isBehind()
    { 
        return false;
    }
    public bool checkIfWall()
    { 
        return false;
    }
    public bool checkIfGround()
    { 
        return false;
    }
    public bool isGrounded()
    { 
        return false;
    }
    public void Attack()
    { 
        
    }
}
