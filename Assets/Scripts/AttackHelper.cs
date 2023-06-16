using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class AttackHelper
{
   static public bool AttackDiceByDice(int attackingForce, int defendingForce, out int attackerLoss, out int defenderLoss)
    {
        int af = attackingForce;
        int df = defendingForce;

        while (af > 0 && df > 0)
        {
            int attackingDice = Random.Range(1, 6);
            int defendingDice = Random.Range(1, 6);

            if (attackingDice > defendingDice)
            {
                df--;
            }
            else
            {
                af--;
            }
        }

        attackerLoss = attackingForce - af;
        defenderLoss = defendingForce - df;

        Debug.Log("AttackingForce " + attackingForce + "; DeffendingForce " + defendingForce
            + "; AttackerLoss " + attackerLoss + "; DenfenderLoss " + defenderLoss);
        //attack succeded?
        return af > 0;
    }
}
