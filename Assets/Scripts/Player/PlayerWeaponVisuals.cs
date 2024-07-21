using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Player player;
    private Animator anim;
    private bool isEquipingWeapon;


    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeappnModel[] backupWeappnModels;


    [Header("Rig")]
    [SerializeField] private float rigWeightIncreaseRate;
    private bool rigShouldBeIncreased;
    private Rig rig;

    [Header("Left Hand IK")]
    [SerializeField] private float leftHandIKWeightIncreaseRate;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIK_Target;
    private bool shouldIncrease_LeftHandIKWeight;



    private void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
        backupWeappnModels = GetComponentsInChildren<BackupWeappnModel>(true);
    }

    private void Update()
    {
        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    public void PlayReloadAnimation()
    {
        if (isEquipingWeapon)
            return;

        float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;

        anim.SetFloat("ReloadSpeed", reloadSpeed);
        anim.SetTrigger("Reload");
        ReduceRigWeight();
    }

    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = CurrentWeaponModel().equipAnimationType;

        float equipmentSpeed = player.weapon.CurrentWeapon().equipmentSpeed;

        leftHandIK.weight = 0;
        ReduceRigWeight();
        anim.SetTrigger("EquipWeapon");
        anim.SetFloat("EquipType", ((float)equipType));
        anim.SetFloat("EquipSpeed", equipmentSpeed);
        SetBusyGrabbingWeaponTo(true);
    }
    

    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        isEquipingWeapon = busy;
        anim.SetBool("BusyEquipingWeapon", isEquipingWeapon);
    }
    public void SwitchOnCurrentModel()
    {
        int animationIndex = (int)CurrentWeaponModel().holdType;

        SwitchOffWeaponModels();

        SwitchOffBackupWeaponModels();

        if (player.weapon.HasOnlyOneWeapon() == false) 
            SwitchOnBackupWeaponModel();

        SwitchAnimationLayer(animationIndex);
        CurrentWeaponModel().gameObject.SetActive(true);
        AttachLeftHand();
    }
    public void SwitchOffWeaponModels()
    {
        for(int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }
    private void SwitchOffBackupWeaponModels()
    {
        foreach (BackupWeappnModel backupWeappnModel in backupWeappnModels)
        {
            backupWeappnModel.gameObject.SetActive(false);
        }
    }
    public void SwitchOnBackupWeaponModel()
    {
        WeaponType weaponType = player.weapon.BackupWeapon().weaponType;

        foreach(BackupWeappnModel backupWeappnModel in backupWeappnModels)
        {
            if (backupWeappnModel.weaponType == weaponType)
            {
                backupWeappnModel.gameObject.SetActive(true);
            }
        }
    }
    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(layerIndex, 1);
    }



    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;

        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;

        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].weaponType == weaponType)
            {
                weaponModel = weaponModels[i];
            }
        }
        return weaponModel;
    }

    #region Animation Rigging Methods
    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;

        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;
    }
    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncrease_LeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1)
            {
                shouldIncrease_LeftHandIKWeight = false;
            }
        }
    }

    private void UpdateRigWeight()
    {
        if (rigShouldBeIncreased)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (rig.weight >= 1)
            {
                rigShouldBeIncreased = false;
            }
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = 0.15f;
    }
    public void MaximizeRigWeight() => rigShouldBeIncreased = true;
    public void MaximizeLeftHandWeight() => shouldIncrease_LeftHandIKWeight = true;
    #endregion
}


