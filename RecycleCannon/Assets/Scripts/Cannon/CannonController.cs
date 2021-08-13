using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CannonController : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public TextMeshProUGUI metalText;
    public TextMeshProUGUI plasticText;
    public TextMeshProUGUI organicText;
    public Button metalButton;
    public Button plasticButton;
    public Button organicButton;
    public int initialMetalBullets = 1; 
    public int initialPlasticBullets = 2;
    public int initialOrganicBullets = 4;
    int m_metalBullets = 0;
    int m_plasticBullets = 0;
    int m_organicBullets = 0;
    int metalBullets { get { return m_metalBullets; } set { m_metalBullets = value; UpdateUIInformations(); } }
    int plasticBullets { get { return m_plasticBullets; } set { m_plasticBullets = value; UpdateUIInformations(); } }
    int organicBullets { get { return m_organicBullets; } set { m_organicBullets = value; UpdateUIInformations(); } }
    [HideInInspector] public TrashType currentBulletType = TrashType.ORGANIC;

    private void Awake()
    {
        metalBullets = initialMetalBullets;
        plasticBullets = initialPlasticBullets;
        organicBullets = initialOrganicBullets;
    }

    public void FireClick()
    {
        if (HasBullet()) FireBullet(); else FireEmpty();
    }

    public bool HasBullet() => currentBulletType == TrashType.METAL ? metalBullets > 0 : currentBulletType == TrashType.PLASTIC ? plasticBullets > 0 : organicBullets > 0;

    public void FireBullet()
    {
        GameObject bullet = GameManager.Instance.poolingSystem.GetBulletFromQueue();
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
        bullet.SetActive(true);
        switch (currentBulletType)
        {
            case TrashType.METAL:
                metalBullets--;
                break;
            case TrashType.PLASTIC:
                plasticBullets--;
                break;
            case TrashType.ORGANIC:
                organicBullets--;
                break;
        }
    }

    public void FireEmpty()
    {
        //do something
    }

    public void ChangeBullet(int index)
    {
        currentBulletType = (TrashType)index;
    }

    public void RechargeBullet(TrashType type)
    {
        switch (type)
        {
            case TrashType.METAL: metalBullets += 2; break;
            case TrashType.PLASTIC: plasticBullets += 2; break;
            case TrashType.ORGANIC: organicBullets += 3; break;
        }
    }

    public void UpdateUIInformations()
    {
        metalText.text = "Metal: " + metalBullets;
        metalButton.interactable = metalBullets > 0;
        plasticText.text = "Plastic: " + plasticBullets;
        plasticButton.interactable = plasticBullets > 0;
        organicText.text = "Organic: " + organicBullets;
        organicButton.interactable = organicBullets > 0;
    }

    #region Direction controller

    Quaternion directionRot;
    public float rotationVelocity;
    private void FixedUpdate()
    {
        if (CannonVirtualJoystick.Instance.HorizontalAxis() != 0 && CannonVirtualJoystick.Instance.VerticalAxis() != 0)
        {
            directionRot = Quaternion.Euler(0f, Mathf.Rad2Deg * Mathf.Atan2(CannonVirtualJoystick.Instance.HorizontalAxis(), CannonVirtualJoystick.Instance.VerticalAxis()), 0f);
            directionRot = directionRot * Camera.main.transform.rotation;
            directionRot.x = 0;
            directionRot.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, directionRot, rotationVelocity * Time.deltaTime);
        }
    }

    #endregion

}
