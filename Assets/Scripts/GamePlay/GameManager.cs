using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;
    public GameObject PlayerObject;
    public GameObject PlayerUI;
    public GameObject spawnElements;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    public void SpawnPlayer()
    {
        if (PlaceOnPlane.isObjectPlaced)
        {
            Instantiate(PlayerUI);

            Instantiate(PlayerObject, GameObject.FindGameObjectWithTag("Vehicle").transform);
            Instantiate(spawnElements, GameObject.FindGameObjectWithTag("Vehicle").transform);



            // Invoke("EnableForkController", 3f);


        }
    }

    public void EnableForkController()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.GetComponent<ForkController>().enabled = true;

    }
}
