using UnityEngine;

public class Barricade : Inter
{
    private float pointFront;
    private float pointBack;
    private float pointRight;
    private float pointLeft;

    private Vector3 right;
    private Vector3 left;
    private Vector3 front;
    private Vector3 back;

    public Unit t;
    public Mesh mesh;

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        
    }


    void Update()
    {
        //SetPoint(new Vector3(1, 1, 1), t);
    }


    public Vector3 GetBarricade()
    {
        if(pointRight > pointLeft && pointRight > pointFront && pointRight > pointBack)
        {
            return right;
        }
        else if(pointLeft > pointRight && pointLeft > pointFront && pointLeft > pointBack)
        {
            return left;
        }
        else if(pointFront > pointLeft && pointFront > pointRight && pointFront > pointBack)
        {
            return front;
        }
        else if(pointBack > pointLeft && pointBack > pointFront && pointBack > pointRight)
        {
            return back;
        }

        return Vector3.negativeInfinity;
    }

    /// <summary>
    /// ����Ʈ�� 90���� ������ ����ص� �̻����� ���� 0���� ���� �ȉ�
    /// </summary>
    /// <param name="size"></param> �ٸ����̵带 �� ������ ũ��
    /// <param name="_target"></param>  ������ �ٶ� Ÿ��
    public void SetPoint(Vector3 size, Unit _target)
    {
        right = new Vector3(transform.right.x * ((transform.localScale.x * 0.5f + size.x * 0.5f) * 1.1f), 0.1f, transform.right.z * ((transform.localScale.z * 0.5f + size.z * 0.5f) * 1.1f));
        left = new Vector3(-transform.right.x*((transform.localScale.x * 0.5f + size.x * 0.5f) * 1.1f), 0.1f, -transform.right.z * ((transform.localScale.z * 0.5f + size.z * 0.5f) * 1.1f));
        front = new Vector3(transform.forward.x * ((transform.localScale.x * 0.5f + size.x * 0.5f) * 1.1f), 0.1f, transform.forward.z * ((transform.localScale.z * 0.5f + size.z * 0.5f) * 1.1f));
        back = new Vector3(-transform.forward.x * ((transform.localScale.x * 0.5f + size.x * 0.5f) * 1.1f), 0.1f, -transform.forward.z * ((transform.localScale.z * 0.5f + size.z * 0.5f) * 1.1f));

        Collider[] collider;

        collider = Physics.OverlapBox(transform.position + right, size * 0.5f, transform.rotation);
        if (collider.Length > 0)
        {
            pointRight = 0;
        }
        else
        {
            pointRight = Vector3.Angle(transform.right, (_target.transform.position - transform.position).normalized);
        }

        collider = Physics.OverlapBox(transform.position + left, size * 0.5f, transform.rotation);
        if (collider.Length > 0)
        {
            pointLeft = 0;
        }
        else
        {
            pointLeft = Vector3.Angle(-transform.right, (_target.transform.position - transform.position).normalized);
        }

        collider = Physics.OverlapBox(transform.position + front, size * 0.5f, transform.rotation);
        if (collider.Length > 0)
        {
            pointFront = 0;
        }
        else
        {
            pointFront = Vector3.Angle(transform.forward, (_target.transform.position - transform.position).normalized);
        }

        collider = Physics.OverlapBox(transform.position + back, size * 0.5f, transform.rotation);
        if (collider.Length > 0)
        {
            pointBack = 0;
        }
        else
        {
            pointBack = Vector3.Angle(-transform.forward, (_target.transform.position - transform.position).normalized);
        }
    }

    /// <summary>
    /// origin�� ��ġ���� target�� ���̴°�
    /// </summary>
    /// <param name="origin"></param>   �������� ex: (transform.position + front)
    /// <param name="dir"></param>      ����         (target.transform.position - (transform.position + front)).nomal
    /// <param name="range"></param>    ��Ÿ�
    /// <param name="target"></param>   ��ǥ
    public void SetCollider(float range, Unit target, int mask = int.MaxValue)        //mask�� ���ָ� �ִ°ɷ�
    {
        RaycastHit hit;
        if(pointFront > 90)
        {
            if (Physics.Raycast(transform.position + front, (target.transform.position - transform.position + front).normalized, out hit, range, mask))
            {
                if (hit.transform.GetComponent<Unit>() != null)
                {
                    if (hit.transform.GetComponent<Unit>() != target)
                    {
                        pointFront = 0;
                    }
                }
                else
                    pointFront = 0;
            }
        }
        if (pointBack > 90)
        {
            if (Physics.Raycast(transform.position + back, (target.transform.position - transform.position + back).normalized, out hit, range, mask))
            {
                if (hit.transform.GetComponent<Unit>() != null)
                {
                    if (hit.transform.GetComponent<Unit>() != target)
                    {
                        pointBack = 0;
                    }
                }
                else
                    pointBack = 0;
            }
        }
        if (pointRight > 90)
        {
            if (Physics.Raycast(transform.position + right, (target.transform.position - transform.position + right).normalized, out hit, range, mask))
            {
                if (hit.transform.GetComponent<Unit>() != null)
                {
                    if (hit.transform.GetComponent<Unit>() != target)
                    {
                        pointRight = 0;
                    }
                }
                else
                    pointRight = 0;
            }
        }
        if (pointLeft > 90)
        {
            if (Physics.Raycast(transform.position + left, (target.transform.position - transform.position + left).normalized, out hit, range, mask))
            {
                if (hit.transform.GetComponent<Unit>() != null)
                {
                    if (hit.transform.GetComponent<Unit>() != target)
                    {
                        pointLeft = 0;
                    }
                }
                else
                    pointLeft = 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireMesh(mesh, transform.position + front, transform.rotation);
        Gizmos.DrawWireMesh(mesh, transform.position + back, transform.rotation);
        Gizmos.DrawWireMesh(mesh, transform.position + right, transform.rotation);
        Gizmos.DrawWireMesh(mesh, transform.position + left, transform.rotation);
    }
}
