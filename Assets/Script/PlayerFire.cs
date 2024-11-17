using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//   �÷��̾��� ����
public class PlayerFire : MonoBehaviour
{
    public Transform[] sockets;
    public WeaponInfo myWeapon;
    public Animator anim;

    void Start()
    {
        myWeapon.weaponType = WeaponType.None; // int�� �ϰ� �ʹٸ� int �� �ڷ������ٰ� ������ ��ȯ�� �� �Ŀ� �ؾ� �Ѵ�.
        UIManager.main_ui.SetWeaponInfo(myWeapon);
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(0) && myWeapon.weaponType != WeaponType.None)
        {
            Fire();
        }

        if(Input.GetKeyDown(KeyCode.F) && myWeapon.weaponType != WeaponType.None)
        {
            DropMyWeapon();
        }

    }

    void Fire()
    {
        // ī�޶��� �߾��� �������� �������� ����ĳ��Ʈ�� �Ѵ�.
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        bool result = Physics.Raycast(ray, out hitInfo, myWeapon.range, ~(1 << 6));
        if (result)
        {
            // ����   ���� ������Ʈ�� �±װ� "Enemy"���
            if(hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // ������ ó���� �Ѵ�.

            }

            // �׷��� �ʴٸ� ���� ��ġ�� ���� ����Ʈ�� ����Ѵ�.

            else
            {

            }
        }

        // �Ѿ��� ������ 1 ���ҽ�Ű�� (��, 0���ϰ� ���� �ʵ��� �Ѵ�.)
        myWeapon.ammo = Mathf.Max(--myWeapon.ammo, 0);
        UIManager.main_ui.SetWeaponInfo(myWeapon);
    }


    
    void DropMyWeapon()
    {
        // ���� ���⿡ �ִ� WeaponData ������Ʈ�� DropWeapon �Լ��� �����Ѵ�.
       
        WeaponData data = sockets[(int)myWeapon.weaponType].GetChild(0).GetComponent<WeaponData>(); // public�̶� �����ϴ�.

        if (data != null) // Ŭ������ null �� �� �ֱ���
        {
            data.DropWeapon(myWeapon);

            // ���� ����(myWeapon ����)�� �ʱ�ȭ �Ѵ�.
            myWeapon = new WeaponInfo();
            UIManager.main_ui.SetWeaponInfo(myWeapon);

            if (myWeapon.weaponType == WeaponType.None)
            {
                anim.SetBool("GetPistol", false);
                anim.SetBool("GetRifle", false);
            }
        }
    }

    public void GetWeapon()
    {
        UIManager.main_ui.SetWeaponInfo(myWeapon);
        if(myWeapon.weaponType == WeaponType.PistolType)
        {
            anim.SetBool("GetPistol", true);
            anim.SetBool("GetRifle", false);
        }

        else if(myWeapon.weaponType == WeaponType.RifleType)
        {
            anim.SetBool("GetRifle", true);
            anim.SetBool("GetPistol", false);
        }
    }
}