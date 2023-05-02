using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Unit
{
    public Rocket rocket_Pre;   //로켓 프리팹
    public GameObject stone_Pre;    //돌 프리팹
    public GameObject stone_;       //돌 경고 프리팹
    public Transform Bezier_1;      //곡선 변수 1
    public Transform Bezier_2;      //곡선 변수 2
    protected List<Rocket> rockets = new List<Rocket>(); //진짜 로켓
    protected List<GameObject> stones = new List<GameObject>();  //진짜 돌
    protected List<GameObject> stones_ = new List<GameObject>();    //돌 경고들
    protected List<List<Vector3>> vector3s = new List<List<Vector3>>();   //이동경로들
    protected List<Vector3Int> stonePosis = new List<Vector3Int>();
    protected List<Vector3> arrival = new List<Vector3>();
    protected List<int> indexs = new List<int>();

    public Transform[] roketG;

    public Ch user;
    public float rocketSpeed;
    public int createStoneCount;

    public Transform muzzle;
    public Transform barrel;
    public float launch_Time;
    public float gun_Damage;

    protected Coroutine loop;

    public AudioSource audioSource;
    public AudioClip[] clip;

    void Start()
    {
        Init();
        loop=StartCoroutine(SkillLoop());
    }


    void Update()
    {
        MoveRocket();
    }


    protected override void Passing(Pa opponent)
    {
        StopCoroutine(loop);
        base.Passing(opponent);
    }
    protected IEnumerator SkillLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(6);
            Skill_Rocket();
            yield return new WaitForSeconds(6);
            SkillClear();
            Skill_Stone();
            yield return new WaitForSeconds(6);
            Skill_Gun();
        }
    }

    public void SkillClear()
    {
        for(int i = 0; i < stones.Count; i++)
        {
            stones[i].transform.GetComponent<Inter>().breaking(this, 10);
        }
        stones.Clear();
        stonePosis.Clear();
    }

    public void Skill_Gun()
    {
        StartCoroutine(gun_Ie());
    }

    protected IEnumerator gun_Ie()
    {
        float ti = 0;

        muzzle.GetComponent<LineRenderer>().enabled = true;
        muzzle.GetComponent<LineRenderer>().SetPosition(0, barrel.transform.position);
        RaycastHit hit;

        while (true)
        {
            ti += Time.deltaTime;

            barrel.LookAt(user.transform);

            if (Physics.Raycast(barrel.position, user.transform.position - barrel.transform.position, out hit))
            {
                muzzle.GetComponent<LineRenderer>().SetPosition(1, hit.transform.position);
            }
           

            if (ti >= launch_Time)
                break;
            yield return null;
        }
        //발사

        audioSource.PlayOneShot(clip[0]);

        if(Physics.Raycast(barrel.position, user.transform.position - barrel.transform.position, out hit, float.MaxValue))
        {
            if (hit.transform.GetComponent<Stone>() != null)
            {
                hit.transform.GetComponent<Stone>().breaking(this, 15);
            }
            else if(hit.transform.GetComponent<Pa>() != null)
            {
                hit.transform.GetComponent<Pa>().GetDamage(gun_Damage, this);
            }
        }

        muzzle.GetComponent<LineRenderer>().enabled = false;

        barrel.transform.localEulerAngles = Vector3.zero;
    }

    public void Skill_Stone()
    {
        
        for(int i = 0; i < createStoneCount; i++)     //소환될 돌의 숫자
        {
            stonePosis.Add(RandomVector(14, stonePosis));
            stones_.Add(Instantiate(stone_, Vector3Y(stonePosis[i], -0.49f), Quaternion.identity));
            stones_[i].transform.eulerAngles = new Vector3(-90, 0, 0);
        }
        StartCoroutine(StoneCreate(2));
    }

    public IEnumerator StoneCreate(float t)
    {
        yield return new WaitForSeconds(t);
        for(int i = 0; i < stones_.Count; i++)
        {
            stones.Add(Instantiate(stone_Pre, stonePosis[i], Quaternion.identity, transform));
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < stones_.Count; i++)
        {
            Destroy(stones_[i]);
        }
        stones_.Clear();
    }

    // rocket 공격
    public void Skill_Rocket()
    {
        Calculation_Path();
    }
    
    protected void MoveRocket()
    {
        for(int i = 0; i < rockets.Count; i++)
        {
            if (vector3s[i].Count <= indexs[i])
            {
                rockets[i].Explosion();
                rockets.RemoveAt(i);
                vector3s.RemoveAt(i);
                indexs.RemoveAt(i);
                continue;
            }
            if (rockets[i] == null)
            {
                rockets.RemoveAt(i);
                vector3s.RemoveAt(i);
                indexs.RemoveAt(i);
                continue;
            }
            rockets[i].transform.position = Vector3.MoveTowards(rockets[i].transform.position, vector3s[i][indexs[i]], rocketSpeed * Time.deltaTime);
            if(rockets[i].transform.position == vector3s[i][indexs[i]])
            {
                indexs[i]++;
            }
        }
    }
    protected void Calculation_Path()
    {
        for(int i = 0; i < 3; i++)
        {
            rockets.Add(Instantiate(rocket_Pre, transform, false));
            rockets[i].Init(this);
            rockets[i].transform.position = roketG[i].transform.position;
            vector3s.Add(new List<Vector3>());
            indexs.Add(0);

            arrival.Add(RandomVector(user.transform.position, 3, arrival));
            Create_Path(roketG[i].position, Bezier_1.position, arrival[i] , i);
        }
        for (int i = 3; i < 6; i++)
        {
            rockets.Add(Instantiate(rocket_Pre, transform, false));
            rockets[i].Init(this);
            rockets[i].transform.position = roketG[i].transform.position;
            vector3s.Add(new List<Vector3>());
            indexs.Add(0);

            arrival.Add(RandomVector(new Vector3(0, 0, 0), 15, arrival));
            Create_Path(roketG[i].position, Bezier_1.position, arrival[i], i);
        }
        //
        for (int i = 6; i < 9; i++)
        {
            rockets.Add(Instantiate(rocket_Pre, transform, false));
            rockets[i].Init(this);
            rockets[i].transform.position = roketG[i].transform.position;
            vector3s.Add(new List<Vector3>());
            indexs.Add(0);

            arrival.Add(RandomVector(user.transform.position, -3, arrival));
            Create_Path(roketG[i].position, Bezier_2.position, arrival[i], i);
        }
        for (int i = 9; i < 12; i++)
        {
            rockets.Add(Instantiate(rocket_Pre, transform, false));
            rockets[i].Init(this);
            rockets[i].transform.position = roketG[i].transform.position;
            vector3s.Add(new List<Vector3>());
            indexs.Add(0);

            arrival.Add(RandomVector(new Vector3(0,0,0), -15, arrival));
            Create_Path(roketG[i].position, Bezier_2.position, arrival[i], i);
        }
        arrival.Clear();
    }
    public void Create_Path(Vector3 pointA, Vector3 pointB, Vector3 pointC, int index)
    {
        float f = 0;
        float[] xs = new float[3];
        float[] ys = new float[3];
        float[] zs = new float[3];

        while (true)
        {
            if (f > 1)
                break;

            xs[0] = pointA.x;
            xs[1] = pointB.x;
            xs[2] = pointC.x;
            float x = Bezier_Curves(xs, f);


            ys[0] = pointA.y;
            ys[1] = pointB.y;
            ys[2] = pointC.y;
            float y = Bezier_Curves(ys, f);


            zs[0] = pointA.z;
            zs[1] = pointB.z;
            zs[2] = pointC.z;
            float z = Bezier_Curves(zs, f);
            vector3s[index].Add(new Vector3(x, y, z));

            f += Time.deltaTime;
        }
    }

    public float Bezier_Curves(float[] floats, float ti)
    {
        float[] fl1 = floats;
        while (true)
        {
            float[] fl2 = new float[fl1.Length - 1];
            for (int i = 0; i < fl2.Length; i++)
            {
                fl2[i] = Mathf.Lerp(fl1[i], fl1[i + 1], ti);
            }

            fl1 = fl2;
            if (fl1.Length <= 1)
            {
                break;
            }
        }
        return fl1[0];
    }
    // rocket 공격


    public Vector3 RandomVector(Vector3 vector, int difference, List<Vector3> list)
    {
        float f1, f3;
        bool ck;
        int test = 0;
        while(true)
        {
            ck = true;
            test++;
            f1 = Random.Range(vector.x, vector.x + difference);
            f3 = Random.Range(vector.z - difference, vector.z + difference);
            for(int i = 0; i < list.Count; i++)
            {
                if (Mathf.Abs(list[i].x - f1) < 1 && Mathf.Abs(list[i].z - f3) < 1)
                {
                    ck = false;
                    break;
                }
            }
            if(ck || test > 100)
            {
                break;
            }
        }
        return new Vector3(f1, 0, f3);
    }
    public Vector3Int RandomVector(int difference, List<Vector3Int> list)
    {
        int f1, f3;
        bool ck;
        int test = 0;
        while (true)
        {
            ck = true;
            test++;
            f1 = Random.Range(-difference, difference);
            f3 = Random.Range(-difference, difference);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].x == f1 && list[i].z == f3)
                {
                    ck = false;
                    break;
                }
            }
            if (ck || test > 100)
            {
                break;
            }
        }
        return new Vector3Int(f1, 10, f3);
    }

    public Vector3 Vector3Y(Vector3 vector ,float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }
}
