using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRB;
    public LifeController lifeController;
    public float walkVelocity;
    public float rotationVelocity;
    [HideInInspector] public Interaction playerInteraction;

    float movimentValue;

    private void FixedUpdate()
    {
        if (VirtualJoystick.Instance != null)
        {
            if (VirtualJoystick.Instance.HorizontalAxis() != 0 || VirtualJoystick.Instance.VerticalAxis() != 0)
            {
                DoMoviment();
                return;
            }
        }
        playerAnim.SetBool("Running", false);
        playerRB.velocity = Vector3.zero;
    }

    void DoMoviment()
    {
        movimentValue = Mathf.Abs(VirtualJoystick.Instance.HorizontalAxis()) > Mathf.Abs(VirtualJoystick.Instance.VerticalAxis()) ? VirtualJoystick.Instance.HorizontalAxis() : VirtualJoystick.Instance.VerticalAxis();
        movimentValue *= walkVelocity * Time.deltaTime;
        playerRB.velocity = playerRB.transform.forward * Mathf.Abs(movimentValue);
        UpdateRotation();
        playerAnim.SetBool("Running", true);
    }

    Quaternion directionRot;
    public void UpdateRotation()
    {
        directionRot = Quaternion.Euler(0f, Mathf.Rad2Deg * Mathf.Atan2(VirtualJoystick.Instance.HorizontalAxis(), VirtualJoystick.Instance.VerticalAxis()), 0f);
        directionRot = directionRot * Camera.main.transform.rotation;
        directionRot.x = 0;
        directionRot.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, directionRot, rotationVelocity * Time.deltaTime);
    }

    public void CatchedByMonster()
    {
        lifeController.ChangeLife(-1);
        transform.position = GameManager.Instance.playerInitialPoint.position;
    }

}
