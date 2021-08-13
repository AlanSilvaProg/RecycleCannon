using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    public Rigidbody enemyRB;
    public float enemyVelocity;
    public Renderer myRenderer;
    public int timeToApplyDmg;
    public bool isBoss = false;
    float timer = 0;
    Material myMaterial;

    TrashType myType;

    int life = 3;

    bool playerPersuit = false;
    Quaternion originalRotation;

    private void OnDisable()
    {
        transform.rotation = originalRotation;
    }

    private void OnEnable()
    {
        originalRotation = transform.rotation;
        myType = (TrashType)Random.Range(0, GameManager.Instance.collectableInformations.Count);
        if (myMaterial == null) myMaterial = new Material(myRenderer.material);
        myMaterial.color = GameManager.Instance.collectableInformations.First(pre => pre.type == myType).typeColor;
        if(!myRenderer.material.Equals(myMaterial)) myRenderer.material = myMaterial;
        life = !isBoss ? 3 : 20;
    }

    private void FixedUpdate()
    {
        if(!isBoss)
            CheckRotation();
        enemyRB.velocity = transform.forward * enemyVelocity * Time.deltaTime;
    }

    Vector3 target;
    public void CheckRotation()
    {
        if (playerPersuit)
        {
            target = GameManager.Instance.playerInstance.transform.position;
            target.y = transform.position.y;
            transform.LookAt(target);

            if (Vector3.Distance(target, transform.position) < 1)
            {
                timer += Time.deltaTime;
                if (timer > timeToApplyDmg)
                {
                    GameManager.Instance.playerInstance.CatchedByMonster();
                }
            }
            else timer = 0;
        }
        else
        {
            transform.rotation = originalRotation;
        }
    }

    public void OnFire(TrashType bulletType)
    {
        if (isBoss)
            life--;
        else
        {
            switch (myType)
            {
                case TrashType.METAL:
                    if (bulletType.Equals(TrashType.ORGANIC))
                    {
                        life--;
                    }
                    break;
                case TrashType.PLASTIC:
                    if (bulletType.Equals(TrashType.ORGANIC))
                    {
                        life--;
                    }
                    break;
                case TrashType.ORGANIC:
                    if (!bulletType.Equals(myType))
                    {
                        life--;
                    }
                    break;
            }
        }
        if (life <= 0) Die();
    }

    public void Die()
    {
        GameManager.Instance.hordeManager.NewDeathRegister();
        if (isBoss)
        {
            if (life <= 0)
            {
                GameManager.Instance.poolingSystem.DropTrashByType(myType, transform.position + ( Vector3.back * 2 ));
                GameManager.Instance.poolingSystem.DropTrashByType(myType, transform.position + (Vector3.back));
                GameManager.Instance.poolingSystem.DropTrashByType(myType, transform.position + (Vector3.forward * 2));
                GameManager.Instance.kills++;
            }
            gameObject.SetActive(false);
            return;
        }

        if (life <= 0)
        {
            GameManager.Instance.kills++;
            GameManager.Instance.poolingSystem.DropTrashByType(myType, transform.position);
        }
        GameManager.Instance.poolingSystem.PutEnemyOnQueue(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBoss && other.CompareTag("Player")) playerPersuit = true;
        if (other.CompareTag("Finish"))
        {
            GameManager.Instance.RemoveLife();
            Die();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isBoss && other.CompareTag("Player")) { playerPersuit = false; timer = 0; }
    }
}
