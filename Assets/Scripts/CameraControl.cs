using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera cam;
    bool interruptMove;
    bool moving;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void goTo(Vector3 pos, float size, float time)
    {
      moving = true;
      StartCoroutine(iteratePos(pos, size, time));
    }

    IEnumerator iteratePos(Vector3 pos, float size, float time)
    {
      float timeStart = Time.time;
      Vector3 startPos = transform.position;
      float startSize = cam.orthographicSize;
      interruptMove = true;
      yield return new WaitForSeconds(0.01f);
      interruptMove = false;

      while ((Time.time - timeStart) / time < 1f)
      {
        transform.position = Vector3.Lerp(startPos, pos, (Time.time - timeStart) / time);
        cam.orthographicSize = Mathf.Lerp(startSize, size, (Time.time - timeStart) / time);
        if (interruptMove)
        {
          break;
        }
        yield return null;
      }
      transform.position = pos;
      cam.orthographicSize = size;
      moving = false;
      yield return null;
    }

    public bool isMoving()
    {
      return moving;
    }
}
