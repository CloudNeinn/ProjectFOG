using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create barrier")]
public class BarrierItem : Item
{
    public override void Execute()
    {
        //Debug.Log("BarrierItem item active");
        if(!playerHealthManager.Instance.barrierActive && playerHealthManager.Instance.barrierCooldownCounter > 0) playerHealthManager.Instance.barrierCooldownCounter -= Time.deltaTime;
        if(playerHealthManager.Instance.barrierActive)
        {
            playerHealthManager.Instance.barrierTimeCounter -= Time.deltaTime;
            if(playerHealthManager.Instance.barrierTimeCounter <= 0) 
            {
                playerHealthManager.Instance.barrierAmount = 0;
                playerHealthManager.Instance.barrierActive = false;
            }
            //playerHealthManager.Instance.barrierBar.fillAmount = playerHealthManager.Instance.barrierAmount / playerHealthManager.Instance.maxBarrier;
        }
        if(characterControl.Instance.hasBarrier && /*characterControl.Instance._activeItem1Input*/Input.GetKeyDown(KeyCode.B) && !playerHealthManager.Instance.barrierActive && playerHealthManager.Instance.barrierCooldownCounter <= 0)
        {
            playerHealthManager.Instance.barrierActive = true;
            playerHealthManager.Instance.barrierCooldownCounter = playerHealthManager.Instance.barrierCooldownTimer;
            playerHealthManager.Instance.barrierAmount = playerHealthManager.Instance.maxBarrier;
            playerHealthManager.Instance.barrierTimeCounter = playerHealthManager.Instance.barrierTimer;
        }
    }
}
