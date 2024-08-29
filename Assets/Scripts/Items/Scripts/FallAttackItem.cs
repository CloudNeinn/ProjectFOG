using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create fall attack")]
public class FallAttackItem : Item
{
    [Header ("Fall Attack")]
    private float fallSpeedBeforeCTRL;   
    private bool fallAttackActive;
    private double fallStartingHeight;
    private double fallEndingHeight;
    private double fallHeight;
    private bool gonnaAttack;
    public double heightToDealDamage;
    public override void Execute()
    {
        if(UserInput.Instance._crouchAction.IsPressed() && !characterControl.Instance.isGrounded())
        {
            fallAttackActive = true;
            fallStartingHeight = characterControl.Instance.transform.position.y;
            if(fallSpeedBeforeCTRL == 0) fallSpeedBeforeCTRL = characterControl.Instance.myrigidbody.velocity.y;
        }
        if(characterControl.Instance.isGrounded() && fallAttackActive)
        {
            fallSpeedBeforeCTRL = 0;
            fallEndingHeight = characterControl.Instance.transform.position.y;
            fallHeight = fallStartingHeight - fallEndingHeight;
        }
        else fallHeight = 0;
        if(fallHeight >= heightToDealDamage && characterControl.Instance.isGrounded() && characterControl.Instance.isCrouching())
        {
            fastFallAttack();
            gonnaAttack = true;
        }
        else gonnaAttack = false;
    }

    public void fastFallAttack()
    {
        fallHeight = 0;
        fallAttackActive = false;
        //characterAttack.Instance.fastFallAttack = true;
        characterAttack.Instance.fallAttackHitEnemies();
    } 
}
