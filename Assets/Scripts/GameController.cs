using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
  public int sizeX, sizeZ;
  public float defaultZoom, mapZoom;
  public Transform platform;
  public CameraControl cam;
  public Player player, enemy;
  int playerX, playerZ, enemyX, enemyZ, keys, moves, randomizeX, randomizeZ;
  string state = "init";
  string lastState;
  bool win, lose, isClicking;
  public Canvas canvas;
  public Text text;

  void Start()
  {
    generateMap(sizeX, sizeZ, platform);
    getPlatformScript(Random.Range(sizeX / 2, sizeX), Random.Range(sizeZ / 2, sizeZ)).setDoor(true);
    while (getPlatformScript(enemyX, enemyZ).getIsKey() || getPlatformScript(enemyX, enemyZ).getIsDoor() || (enemyX == 0 && enemyZ == 0))
    {
      enemyX = Random.Range(sizeX / 2, sizeX);
      enemyZ = Random.Range(sizeZ / 2, sizeZ);
    }
    enemy.transform.position = getPlatform(enemyX, enemyZ).position;
    keys = placeKeys(3);
    text.text = "WASD to move. Escape.";
    canvas.enabled = true;
  }

  void Update()
  {
    if (!string.Equals(state, lastState))
    {
      lastState = state;
      Debug.Log(state);
    }

    switch(state)
    {
      case "init":
      {
        if (Input.anyKey || validClick())
        {
          initializeBridges();
          state = "zoomOut";
        }
        break;
      }

      case "zoomOut":
      {
        canvas.enabled = false;
        if (!cam.isMoving())
        {
          camPos(sizeX / 2, sizeZ / 2, mapZoom, 1f);
        }
        if (!cam.isMoving() && (Input.anyKey || validClick()))
        {
          state = "playerIn";
        }
        break;
      }

      case "playerIn":
      {
        canvas.enabled = true;
        text.text = "Player's Turn";
        if (!cam.isMoving())
        {
          camPos(playerX, playerZ, defaultZoom, 1f);
        }
        if (!cam.isMoving())
        {
          if (Input.anyKey || validClick())
          {
            state = "playerRoll";
          }
        }
        break;
      }

      case "playerRoll":
      {
        moves = Random.Range(1, 7);
        text.text = "You get to move " + moves + " spaces";
        state = "prePlayerDirection";
        break;
      }

      case "prePlayerDirection":
      {
        if (Input.anyKey || validClick())
        {
          state = "playerDirection";
        }
        break;
      }

      case "playerDirection":
      {
        if (!cam.isMoving())
        {
          camPos(playerX, playerZ, defaultZoom, 1f);
        }
        randomizeX = playerX;
        randomizeZ = playerZ;

        if (Input.GetKeyDown("a") && playerX > 0 && getPlatformScript(playerX - 1, playerZ).getX())
        {
          playerX--;
          state = "playerMove";
          moves--;
        }
        if (Input.GetKeyDown("d") && playerX + 1 < sizeX && getPlatformScript(playerX, playerZ).getX())
        {
          playerX++;
          state = "playerMove";
          moves--;
        }
        if (Input.GetKeyDown("w") && playerZ > 0 && getPlatformScript(playerX, playerZ - 1).getZ())
        {
          playerZ--;
          state = "playerMove";
          moves--;
        }
        if (Input.GetKeyDown("s") && playerZ + 1 < sizeZ && getPlatformScript(playerX, playerZ).getZ())
        {
          playerZ++;
          state = "playerMove";
          moves--;
        }
        correctPlayerPos();
        break;
      }

      case "playerMove":
      {
        canvas.enabled = false;
        playerPos(playerX, playerZ, 1f);
        if (!cam.isMoving())
        {
          camPos(playerX, playerZ, defaultZoom, 1f);
        }
        if (!player.isMoving())
        {
          if (getPlatformScript(playerX, playerZ).getIsKey())
          {
            keys--;
            getPlatformScript(playerX, playerZ).setKey(false);
          }
          else if (getPlatformScript(playerX, playerZ).getIsDoor())
          {
            if (keys <= 0)
            win = true;
          }

          if (playerX == enemyX && playerZ == enemyZ)
          {
            lose = true;
          }

          state = "randomizePlatforms";

          if (win || lose)
          {
            state = "checkConditions";
          }
        }
        break;
      }

      case "randomizePlatforms":
      {
        Debug.Log("Randomizing");
        if (randomizeX <= playerX && randomizeX > 0)
        {
          if (Random.Range(0, 2) == 0)
          {
            Debug.Log("Flip!");
            getPlatformScript(randomizeX - 1, randomizeZ).setX(!getPlatformScript(randomizeX - 1, randomizeZ).getX());
          }
        }
        if (randomizeX >= playerX && randomizeX + 1 < sizeX)
        {
          if (Random.Range(0, 2) == 0)
          {
            Debug.Log("Flip!");
            getPlatformScript(randomizeX, randomizeZ).setX(!getPlatformScript(randomizeX, randomizeZ).getX());
          }
        }
        if (randomizeZ <= playerZ && randomizeZ > 0)
        {
          if (Random.Range(0, 2) == 0)
          {
            Debug.Log("Flip!");
            getPlatformScript(randomizeX, randomizeZ - 1).setZ(!getPlatformScript(randomizeX, randomizeZ - 1).getZ());
          }
        }
        if (randomizeZ >= playerZ && randomizeZ + 1 < sizeZ)
        {
          if (Random.Range(0, 2) == 0)
          {
            Debug.Log("Flip!");
            getPlatformScript(randomizeX, randomizeZ).setZ(!getPlatformScript(randomizeX, randomizeZ).getZ());
          }
        }

        if (moves > 0)
        {
          state = "playerDirection";
        }
        else
        {
          state = "enemyIn";
        }
        break;
      }

      case "enemyIn":
      {
        canvas.enabled = true;
        text.text = "Enemy's Turn";
        randomizeX = enemyX;
        randomizeZ = enemyZ;
        if (!cam.isMoving())
        {
          camPos(enemyX, enemyZ, defaultZoom, 1f);
        }
        if (!cam.isMoving())
        {
          if (Input.anyKey || validClick())
          {
            state = "enemyRoll";
          }
        }
        break;
      }

      case "enemyRoll":
      {
        moves = Random.Range(0, 7);
        text.text = "They get to move " + moves + " spaces";
        state = "enemyMove";
        break;
      }

      case "preEnemyMove":
      {
        if (Input.anyKey || validClick())
        {
          state = "enemyMove";
        }
        break;
      }

      case "enemyMove":
      {
        if (!cam.isMoving())
        {
          camPos(enemyX, enemyZ, defaultZoom, 1f);
        }

        if (!enemy.isMoving())
        {
          if (Mathf.Abs(playerX - enemyX) >= Mathf.Abs(playerZ - enemyZ))
          {
            if (playerX < enemyX && enemyX > 0 && getPlatformScript(enemyX - 1, enemyZ).getX())
            {
              enemyX--;
              moves--;
            }
            else if (playerX > enemyX && enemyX + 1 < sizeX && getPlatformScript(enemyX, enemyZ).getX())
            {
              enemyX++;
              moves--;
            }
            else
            {
              if (enemyX > 0 && getPlatformScript(enemyX - 1, enemyZ).getX())
              {
                enemyX--;
                moves--;
              }
              else if (enemyX + 1 < sizeX && getPlatformScript(enemyX, enemyZ).getX())
              {
                enemyX++;
                moves--;
              }
              else if (enemyZ > 0 && getPlatformScript(enemyX, enemyZ - 1).getZ())
              {
                enemyZ--;
                moves--;
              }
              else if (enemyZ + 1 < sizeZ && getPlatformScript(enemyX, enemyZ).getZ())
              {
                enemyZ++;
                moves--;
              }
            }
          }
          else if (Mathf.Abs(playerX - enemyX) < Mathf.Abs(playerZ - enemyZ))
          {
            if (playerZ < enemyZ && enemyZ > 0 && getPlatformScript(enemyX, enemyZ - 1).getZ())
            {
              enemyZ--;
              moves--;
            }
            else if (playerZ < enemyZ && enemyZ + 1 < sizeZ && getPlatformScript(enemyX, enemyZ).getZ())
            {
              enemyZ++;
              moves--;
            }
            else
            {
              if (enemyX > 0 && getPlatformScript(enemyX - 1, enemyZ).getX())
              {
                enemyX--;
                moves--;
              }
              else if (enemyX + 1 < sizeX && getPlatformScript(enemyX, enemyZ).getX())
              {
                enemyX++;
                moves--;
              }
              else if (enemyZ > 0 && getPlatformScript(enemyX, enemyZ - 1).getZ())
              {
                enemyZ--;
                moves--;
              }
              else if (enemyZ + 1 < sizeZ && getPlatformScript(enemyX, enemyZ).getZ())
              {
                enemyZ++;
                moves--;
              }
            }
          }
          enemyPos(enemyX, enemyZ, 1f);

          state = "randomizeEnemyPlatforms";

          if (enemyX == playerX && enemyZ == playerZ)
          {
            lose = true;
            state = "checkConditions";
          }
        }
        break;
      }

      case "randomizeEnemyPlatforms":
      {
        if (randomizeX <= enemyX && randomizeX > 0)
        {
          if (Random.Range(0, 2) == 0)
          {
            getPlatformScript(randomizeX - 1, randomizeZ).setX(!getPlatformScript(randomizeX - 1, randomizeZ).getX());
          }
        }
        if (randomizeX >= enemyX && randomizeX + 1 < sizeX)
        {
          if (Random.Range(0, 2) == 0)
          {
            getPlatformScript(randomizeX, randomizeZ).setX(!getPlatformScript(randomizeX, randomizeZ).getX());
          }
        }
        if (randomizeZ <= enemyZ && randomizeZ > 0)
        {
          if (Random.Range(0, 2) == 0)
          {
            getPlatformScript(randomizeX, randomizeZ - 1).setZ(!getPlatformScript(randomizeX, randomizeZ - 1).getZ());
          }
        }
        if (randomizeZ >= enemyZ && randomizeZ + 1 < sizeZ)
        {
          if (Random.Range(0, 2) == 0)
          {
            getPlatformScript(randomizeX, randomizeZ).setZ(!getPlatformScript(randomizeX, randomizeZ).getZ());
          }
        }
        if (moves <= 0)
        {
          state = "checkConditions";
        }
        else
        {
          state = "enemyMove";
        }
        break;
      }

      case "checkConditions":
      {
        canvas.enabled = false;
        if (win)
        {
          state = "win";
        }
        else if (lose)
        {
          state = "gameover";
        }
        else
        {
          state = "zoomOut";
        }
        break;
      }

      case "win":
      {
        if (!cam.isMoving())
        {
          camPos(sizeX / 2, sizeZ / 2 - 1, mapZoom, 1f);
        }
        canvas.enabled = true;
        text.text = "You Win!";
        break;
      }

      case "gameover":
      {
        canvas.enabled = true;
        text.text = "Game Over";
        break;
      }
    }

    if (!Input.GetMouseButton(0))
    {
      isClicking = false;
    }
  }

  void generateMap(int x, int z, Transform p)
  {
    for (int i = 0; i < x; i++)
    {
      for (int j = 0; j < z; j++)
      {
        Transform current = Instantiate(p, new Vector3(i * 2f, 0, j * -2f), Quaternion.identity);
        current.gameObject.name = "Platform " + i + "|" + j;
      }
    }
  }

  Platform getPlatformScript(int x, int z)
  {
    return GameObject.Find("Platform " + x + "|" + z).GetComponent<Platform>();
  }

  Transform getPlatform(int x, int z)
  {
    return GameObject.Find("Platform " + x + "|" + z).transform;
  }

  int placeKeys(int numKeys)
  {
    int i = numKeys;
    Platform p;
    while (i > 0)
    {
      int placeX, placeZ;
      placeX = Random.Range(0, sizeX);
      placeZ = Random.Range(0, sizeZ);
      p = getPlatformScript(placeX, placeZ);
      if (!p.getIsDoor() && !p.getIsKey() && placeX != playerX && placeX != enemyX && placeZ != playerZ && placeZ != enemyZ)
      {
        p.setKey(true);
        i--;
      }
    }
    return numKeys;
  }

  void initializeBridges()
  {
    // Raise all bridges between platforms
    for (int i = 0; i < sizeX; i++)
    {
      for (int j = 0; j < sizeZ; j++)
      {
        if (i < sizeX - 1)
        {
          getPlatformScript(i, j).setX(true);
        }
        if (j < sizeZ - 1)
        {
          getPlatformScript(i, j).setZ(true);
        }
      }
    }
  }

  void camPos(int x, int z, float size, float time)
  {
    if (cam.transform.position != getPlatform(x, z).transform.position)
    {
      cam.goTo(getPlatform(x, z).transform.position, size, time);
    }
  }

  void playerPos(int x, int z, float time)
  {
    if (player.transform.position != getPlatform(x, z).transform.position)
    {
      player.goTo(getPlatform(x, z).transform.position, time);
    }
  }

  void correctPlayerPos()
  {
    if (playerX > sizeX - 1)
    {
      playerX = sizeX - 1;
    }
    if (playerX < 0)
    {
      playerX = 0;
    }
    if (playerZ > sizeZ - 1)
    {
      playerZ = sizeZ - 1;
    }
    if (playerZ < 0)
    {
      playerZ = 0;
    }
  }

  void enemyPos(int x, int z, float time)
  {
    if (enemy.transform.position != getPlatform(x, z).transform.position)
    {
      enemy.goTo(getPlatform(x, z).transform.position, time);
    }
  }

  void correctEnemyPos()
  {
    if (enemyX > sizeX - 1)
    {
      enemyX = sizeX - 1;
    }
    if (enemyX < 0)
    {
      enemyX = 0;
    }
    if (enemyZ > sizeZ - 1)
    {
      enemyZ = sizeZ - 1;
    }
    if (enemyZ < 0)
    {
      enemyZ = 0;
    }
  }

  public bool validClick()
  {
    if (!isClicking && Input.GetMouseButton(0))
    {
      isClicking = true;
      return true;
    }
    else
    {
      return false;
    }
  }
}
