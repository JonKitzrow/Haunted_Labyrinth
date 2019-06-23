using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    bool bridgeX, bridgeZ, bridgeXLast, bridgeZLast;
    public Animator bridgeXAnim, bridgeZAnim;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if (bridgeXLast != bridgeX)
      {
        bridgeXLast = bridgeX;
        Debug.Log("X: " + bridgeX);
        if (bridgeX)
        {
          bridgeXAnim.SetTrigger("Extend");
          Debug.Log("Extend X");
        }
        else
        {
          bridgeXAnim.SetTrigger("Retract");
          Debug.Log("Retract X");
        }
      }

      if (bridgeZLast != bridgeZ)
      {
        bridgeZLast = bridgeZ;
        Debug.Log("Z: " + bridgeZ);
        if (bridgeZ)
        {
          bridgeZAnim.SetTrigger("Extend");
          Debug.Log("Extend Z");
        }
        else
        {
          bridgeZAnim.SetTrigger("Retract");
          Debug.Log("Retract Z");
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
}
