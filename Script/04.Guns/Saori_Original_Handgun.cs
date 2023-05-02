using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saori_Original_Handgun : Gun
{
    public override void Action(Ch opponent)    //�Ҹ��� �ִϸ��̼� ��� (�߻�)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (magazine > 0 && canShot)
            {
                magazine--;

                audioSource.PlayOneShot(clips[0]);

                animator.SetBool("Shot", true);

                gunFire.Play();

                RPMCoroutine = StartCoroutine(RPMC());
                canShot = false;

                if (opponent != null)
                {
                    user = opponent;
                }
                if (user != null)
                {
                    bullets[magazine].gameObject.SetActive(true);
                    bullets[magazine].transform.position = muzzle.transform.position;
                    bullets[magazine].Fire(muzzle.transform.right, user, figure);
                    user.ScreenShaking(gunScriptble.GetRecoil(), gunScriptble.GetRPM());
                }
            }
            else
            {
                Reload();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        else if (Input.GetMouseButtonDown(1))    //��Ŭ�� ����
        {
            if (user.GetComponent<Animator>().GetBool("Zoom"))
            {
                user.GetComponent<Animator>().SetBool("Zoom", false);
            }
            else
            {
                user.GetComponent<Animator>().SetBool("Zoom", true);
            }
        }
    }

    public override void Active()       //�������� ����� �� �ڵ� ������
    {
        base.Active();
        transform.localEulerAngles = new Vector3(0, -90, 0);
    }
}
