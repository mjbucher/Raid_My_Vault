using UnityEngine;
using System.Collections;

public class DungeonManager : MonoBehaviour
{
    public GameObject levelSpawner;
    public int loadSceneLevel;

    int detectionAmount = 0;
    public int detectionLimit = 5;
    float timer = 0;
    [SerializeField]
    private float timerLimit = 300.00f;
    public float Timer { get { return timer; } }
    private GameMaster GM;


    public void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }


    public void CheckRaidingGame()
    {
        if (detectionAmount >= detectionLimit)
        {
            StartCoroutine("StartTimer");
            Debug.Log("Too much detection, starting timer");
        }
    }

    public IEnumerator StartTimer()
    {
        while (true)
        {
            if (timer < timerLimit)
            {
                timer += Time.deltaTime;
                yield return null;
            } else
            {
                Debug.Log("Game Ending");
                EndRaiding();
            }
        }
    }

    public void EndRaiding()
    {

    }

    public void AddDetection ()
    {
        detectionAmount++;
    }
}
