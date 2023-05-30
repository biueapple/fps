using UnityEngine;

public class Grenade : Item
{
    public override void Action(Ch opponent)
    {
        if(Input.GetMouseButtonDown(0))
        {
            base.Action(opponent);
            Parabola(Vector3.zero, 3, 3, 3, 0.01f);
            transform.parent = null;
            user.ItemRemove(this);
        }
    }
    protected override void Effect()
    {
        base.Effect();
        for(int i = 0; i < paList.Count; i++)
        {
            paList[i].GetDamage(figure, user);
            if (paList[i].GetComponent<Inter>() != null)
            {
                if (paList[i].GetComponent<Inter>().GetHard())
                    paList[i].GetComponent<Inter>().breaking(user, 50, true);
            }
        }
        paList.Clear();
        Passing(user);
    }

    public override void Active()
    {
        base.Active();
        transform.localEulerAngles = Vector3.zero;
    }
}
