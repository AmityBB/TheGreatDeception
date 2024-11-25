using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    public List<Transform> KillerSpawns;
    public List<PlayerMovementOnMap> player;
    [SerializeField]
    private Camera Cam;
    public bool Active;
    private GameObject clone;
    [SerializeField]
    private GameObject[] Minigames;
    [SerializeField]
    private GameObject Killer;
    public GameObject confirmButton;
    private int playerWithTurn;
    [SerializeField]
    private List<GameObject> trashItems;
    

    void Start()
    {
        Cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            StartRound();
        }
    }

    void StartRound()
    {
        playerWithTurn = 0;
        player[0].MyTurn();
    }

    public void PrintMinigame()
    {
        if (!Active)
        {
            Cam.transform.rotation = new Quaternion(0, 0, 0, 0);
            clone = Instantiate(Minigames[0], Cam.transform.position + (Cam.transform.forward * 16), Quaternion.identity);
            Active = true;
        }
        else
        {
            Cam.transform.rotation = Quaternion.Euler(60, 0, 0);
            Destroy(clone);
            Active = false;
        }
    }

    public void UVMinigame()
    {
        if (!Active)
        {
            for (int i = 0; i < player.Count; i++)
            {
                player[i].GetComponent<PlayerMovementOnMap>().enabled = false;
                player[i].GetComponent<PlayerMovementInMini>().enabled = true;
            }
            clone = Instantiate(Killer, KillerSpawns[Random.Range(0, 7)].position, Quaternion.identity);
            StartCoroutine(UVGameTimer());
            Active = true;
        }
        else
        {
            for (int i = 0; i < player.Count; i++)
            {
                player[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                player[i].GetComponent<PlayerMovementOnMap>().enabled = true;
                player[i].GetComponent<PlayerMovementInMini>().enabled = false;
                Destroy(clone);
            }
            Active = false;
        }
    }

    IEnumerator UVGameTimer()
    {
        yield return new WaitForSecondsRealtime(60);
        UVMinigame();
    }

    public void SortingMinigame()
    {
        if (!Active)
        {
            clone = Instantiate(Minigames[1], Cam.transform.position + (Cam.transform.forward * 16), Quaternion.identity);
            clone.GetComponent<SorteerMinigame>().active = true;
            Active = true;
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                Destroy(FindObjectOfType<WeaponTrash>().gameObject);
                if(i==8) { Destroy(clone); }
            }
            
            Active = false;
        }
    }

    public void TurnEnd()
    {
        if(playerWithTurn < player.Count-1) 
        {
            playerWithTurn++;
        }
        player[playerWithTurn].MyTurn();
    }
}
