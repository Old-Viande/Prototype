using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    None,
    SingleAttack,
    MeleeAttack_Max2Min,
    MeleeAttack_Min2Max,
    RangedAttack_Min2Max,
    RangedAttack_Max2Min,
    GroupAttack_HpHigh2Low,
    GroupAttack_HpLow2High,
}
  