using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour
{
    public Rigidbody bulletRb;
    public Renderer myRenderer;
    Material myMaterial;
    public float bulletVelocity = 500;
    public float lifeTime = 3;
    TrashType myType;

    float time = 0;

    private void OnEnable()
    {
        time = 0;
        if (myMaterial == null) myMaterial = new Material(myRenderer.material);
        myType = GameManager.Instance.cannonInstance.currentBulletType;
        myMaterial.color = GameManager.Instance.collectableInformations.First(pre => pre.type == myType).typeColor;
        if (!myRenderer.material.Equals(myMaterial)) myRenderer.material = myMaterial;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bulletRb.velocity = transform.forward * bulletVelocity * Time.deltaTime;
        time += Time.deltaTime;
        if (time >= lifeTime)
            GameManager.Instance.poolingSystem.PutBulletOnQueue(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.OnFire(myType);
            GameManager.Instance.poolingSystem.PutBulletOnQueue(gameObject);
        }
    }
}
