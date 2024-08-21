using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityAltarScript : MonoBehaviour
{
    private enum AbilityToGet
    {
        doubleJump,
        highJump,
        Dash,
        specialAttack,
        barrier,
        deflectProjectile,
        wallJump,
        glider,
        teleport,
        hook
    }

    [SerializeField] private AbilityToGet abilityToChange;
    [SerializeField] private bool abilityObtained;
    
    void Update()
    {
        if(characterControl.Instance.transform.position.x <= transform.position.x + 2 && 
        characterControl.Instance.transform.position.x >= transform.position.x - 2 && 
        characterControl.Instance.transform.position.y <= transform.position.y + 2 && 
        characterControl.Instance.transform.position.y >= transform.position.y - 2 )
        {
            if(characterControl.Instance._use1Input && !abilityObtained) // Input.GetKeyDown(KeyCode.E)
            {   
                abilityObtained = true;
            }
        }

        if (abilityToChange == AbilityToGet.doubleJump && characterControl.Instance.hasDoubleJump == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.highJump && characterControl.Instance.hasHighJump == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.Dash && characterControl.Instance.hasDash == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.specialAttack && characterControl.Instance.hasSpecialAttack == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.barrier && characterControl.Instance.hasBarrier == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.deflectProjectile && characterControl.Instance.hasDeflectProjectile == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.wallJump && characterControl.Instance.hasWallJump == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.glider && characterControl.Instance.hasGlider == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.teleport && characterControl.Instance.hasTeleport == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.hook && characterControl.Instance.hasHook == true) abilityObtained = true;
        /*
        const abilityMap = {
            AbilityToGet.doubleJump: 'hasDoubleJump',
            AbilityToGet.highJump: 'hasHighJump',
            AbilityToGet.Dash: 'hasDash',
            AbilityToGet.specialAttack: 'hasSpecialAttack',
            AbilityToGet.barrier: 'hasBarrier',
            AbilityToGet.deflectProjectile: 'hasDeflectProjectile',
        };

        if (abilityMap[abilityToChange] && characterControl.Instance[abilityMap[abilityToChange]]) {
            abilityObtained = true;
            characterControl.Instance[abilityMap[abilityToChange]] = true;
        }

        */

        if(abilityObtained)
        {
            switch(abilityToChange)
            {
                case AbilityToGet.doubleJump:
                    characterControl.Instance.hasDoubleJump = true;
                    break;
                case AbilityToGet.highJump:
                    characterControl.Instance.hasHighJump = true;
                    break;
                case AbilityToGet.Dash:
                    characterControl.Instance.hasDash = true;
                    break;
                case AbilityToGet.specialAttack:
                    characterControl.Instance.hasSpecialAttack = true;
                    break;
                case AbilityToGet.barrier:
                    characterControl.Instance.hasBarrier = true;
                    break;
                case AbilityToGet.deflectProjectile:
                    characterControl.Instance.hasDeflectProjectile = true;
                    break;
                case AbilityToGet.wallJump:
                    characterControl.Instance.hasWallJump = true;
                    break;
                case AbilityToGet.glider:
                    characterControl.Instance.hasGlider = true;
                    break;
                case AbilityToGet.teleport:
                    characterControl.Instance.hasTeleport = true;
                    break;
                case AbilityToGet.hook:
                    characterControl.Instance.hasHook = true;
                    break;
            }
        }
    }
}
