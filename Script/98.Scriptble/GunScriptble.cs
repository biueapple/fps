using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObj/CreateGunData", order = int.MaxValue)]
public class GunScriptble : ScriptableObject
{
    [SerializeField]
    private int maxMagazine;
    public int MaxMagazine()
    {
        return maxMagazine;
    }

    [SerializeField]
    private float RPM;  //연사속도
    public float GetRPM()
    {
        return RPM;
    }

    [SerializeField]
    private float reloadTime;
    public float ReloadTime()
    {
        return reloadTime;
    }

    [SerializeField]
    private float recoil;
    public float GetRecoil()
    {
        return recoil;
    }
}
