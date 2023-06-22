using UnityEngine;

public class Saori_Original_Handgun : Gun
{
    public override void Action(Ch opponent)    //�Ҹ��� �ִϸ��̼� ��� (�߻�)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shot(opponent);
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

    public override void Shot(Ch opponent)
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
            bullets[magazine].gameObject.SetActive(true);
            bullets[magazine].transform.position = muzzle.transform.position;
            if (user != null)
            {
                bullets[magazine].Fire(muzzle.transform.right, user, figure);
                user.ScreenShaking(gunScriptble.GetRecoil(), gunScriptble.GetRPM());
            }
        }
        else
        {
            Reload();
        }
    }

    public override void Active()       //�������� ����� �� �ڵ� ������
    {
        base.Active();
        transform.localEulerAngles = new Vector3(0, -90, 0);
    }

    public override void ZeroSet()
    {
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = new Vector3(0, -90, 0);
    }
}
