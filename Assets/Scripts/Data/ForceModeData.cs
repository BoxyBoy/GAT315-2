using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForceType", menuName = "Data/Enum/ForceMode")]
public class ForceModeData : EnumData
{
    public override int index { get => (int)value; set => this.value = (eType)value; }

    public override string[] names => Enum.GetNames(typeof(eType));

    public enum eType
    {
        Constant,
        InverseLinear,
        InverseSquared
    }

    public eType value;
}
