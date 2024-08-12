using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HangType
{
    LowBackHang,
    BackHang,
    SideHang
}

public class BackupWeappnModel : MonoBehaviour
{
    public WeaponType weaponType;

    [SerializeField] private HangType hangType;

    public void Activate(bool activated) => gameObject.SetActive(activated);
    public bool HangTypeIs(HangType hangType) => this.hangType == hangType;
}
