using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    bool bridgeX, bridgeZ, bridgeXLast, bridgeZLast, isDoor, isKey;
    public Animator bridgeXAnim, bridgeZAnim;
    public GameObject door, key;


    // Start is called before the first frame update
    void Start()
    {
      bridgeX = false;
      bridgeZ = false;
    }

    // Update is called once per frame
    void Update()
    {
      // update platform content
      if (isDoor && !door.activeSelf)
      {
        door.SetActive(true);
      }

      if (isKey && !key.activeSelf)
      {
        key.SetActive(true);
      }

      if (!isKey && key.activeSelf)
      {
        key.SetActive(false);
      }

      // update bridges
      if (bridgeXLast != bridgeX)
      {
        bridgeXLast = bridgeX;
        if (bridgeX)
        {
          bridgeXAnim.SetTrigger("Extend");
        }
        else
        {
          bridgeXAnim.SetTrigger("Retract");
        }
      }

      if (bridgeZLast != bridgeZ)
      {
        bridgeZLast = bridgeZ;
        if (bridgeZ)
        {
          bridgeZAnim.SetTrigger("Extend");
        }
        else
        {
          bridgeZAnim.SetTrigger("Retract");
        }
      }
    }

    public bool getX()
    {
      return bridgeX;
    }

    public bool getZ()
    {
      return bridgeZ;
    }

    public void setX(bool b)
    {
      bridgeX = b;
    }

    public void setZ(bool b)
    {
      bridgeZ = b;
    }

    public void setDoor(bool b)
    {
      isDoor = b;
    }

    public bool getIsDoor()
    {
      return isDoor;
    }

    public void setKey(bool b)
    {
      isKey = b;
    }

    public bool getIsKey()
    {
      return isKey;
    }
}
