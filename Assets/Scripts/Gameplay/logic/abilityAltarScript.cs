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
        wallJump
    }

    [SerializeField] private AbilityToGet abilityToChange;
    [SerializeField] private bool abilityObtained;
    private characterControl charCon;   

    // Start is called before the first frame update
    void Start()
    {
        charCon = GameObject.FindObjectOfType<characterControl>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(charCon.transform.position.x <= transform.position.x + 2 && 
        charCon.transform.position.x >= transform.position.x - 2 && 
        charCon.transform.position.y <= transform.position.y + 2 && 
        charCon.transform.position.y >= transform.position.y - 2 )
        {
            if(Input.GetKeyDown(KeyCode.E) && !abilityObtained)
            {   
                abilityObtained = true;
            }
        }

        if (abilityToChange == AbilityToGet.doubleJump && charCon.hasDoubleJump == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.highJump && charCon.hasHighJump == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.Dash && charCon.hasDash == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.specialAttack && charCon.hasSpecialAttack == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.barrier && charCon.hasBarrier == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.deflectProjectile && charCon.hasDeflectProjectile == true) abilityObtained = true;
        else if (abilityToChange == AbilityToGet.wallJump && charCon.hasWallJump == true) abilityObtained = true;
        /*
        const abilityMap = {
            AbilityToGet.doubleJump: 'hasDoubleJump',
            AbilityToGet.highJump: 'hasHighJump',
            AbilityToGet.Dash: 'hasDash',
            AbilityToGet.specialAttack: 'hasSpecialAttack',
            AbilityToGet.barrier: 'hasBarrier',
            AbilityToGet.deflectProjectile: 'hasDeflectProjectile',
        };

        if (abilityMap[abilityToChange] && charCon[abilityMap[abilityToChange]]) {
            abilityObtained = true;
            charCon[abilityMap[abilityToChange]] = true;
        }

        */

        if(abilityObtained)
        {
            switch(abilityToChange)
            {
                case AbilityToGet.doubleJump:
                    charCon.hasDoubleJump = true;
                    break;
                case AbilityToGet.highJump:
                    charCon.hasHighJump = true;
                    break;
                case AbilityToGet.Dash:
                    charCon.hasDash = true;
                    break;
                case AbilityToGet.specialAttack:
                    charCon.hasSpecialAttack = true;
                    break;
                case AbilityToGet.barrier:
                    charCon.hasBarrier = true;
                    break;
                case AbilityToGet.deflectProjectile:
                    charCon.hasDeflectProjectile = true;
                    break;
                case AbilityToGet.wallJump:
                    charCon.hasWallJump = true;
                    break;
            }
        }
    }
}
