using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool moving;

    public void goTo(Vector3 pos, float time)
    {
      if (!moving)
      {
        moving = true;
        StartCoroutine(iteratePos(pos, time));
      }
    }

    IEnumerator iteratePos(Vector3 pos, float time)
    {
      float timeStart = Time.time;
      Vector3 startPos = transform.position;

      while ((Time.time - timeStart) / time < 1f)
      {
        transform.position = Vector3.Lerp(startPos, pos, (Time.time - timeStart) / time) + Vector3.up * Mathf.Sin(Mathf.Lerp(0f, 180f, (Time.time - timeStart) / time) * Mathf.Deg2Rad) * 0.5f;
        yield return null;
      }
      transform.position = pos;
      moving = false;
      yield return null;
    }

    public bool isMoving()
    {
      return moving;
    }
}
