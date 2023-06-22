using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ParticleSystem system;
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void ExplosionParticle(Transform parent)
    {
        Explosion ex = Instantiate(this);
        ex.transform.position = parent.position;
        ex.system.Play();
        Destroy(ex, 1.5f);
    }
}
