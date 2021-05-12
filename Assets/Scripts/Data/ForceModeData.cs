using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForceType", menuName = "Data/Enum/ForceMode")]
public class ForceModeData : EnumData
{
    public override int index { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override string[] names => throw new System.NotImplementedException();

    public enum eType
    {
        Constant,
        InverseLinear,
        InverseSquared
    }

    public eType value;
}
