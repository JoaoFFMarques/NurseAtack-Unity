using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    public GameObject RightWall;
    public GameObject LeftWall;
    public GameObject[] Npc;
    public int NpcQtd;
    public GameObject[] Enemy;
    public int EnemyQtd;
    private readonly float MaxZ = 5f;
    private readonly float MinZ = -4f;
    private int CurrentNpc;
    private CameraFollow Cam;
    private int Npcs;
    private GameObject Right;
    private GameObject RightDespawner;
    private GameObject Left;
    private GameObject LeftDespawner;
    private GameController Gamecontroller;
    private bool setCam = false;
    private bool StartGo;    
 
    public void Start()
    {
        Gamecontroller = FindObjectOfType(typeof(GameController)) as GameController;
        Cam = FindObjectOfType<CameraFollow>() as CameraFollow;        
    }    
    public void FixedUpdate()
    {
        if(Cam.maxXAndY.x == Cam.transform.position.x)
            Cam.minXAndY.x = Cam.maxXAndY.x;
        FinishCheck();
    }
    public void OnTriggerEnter(Collider collison)
    {
        if(collison.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            Cam.maxXAndY.x = transform.position.x + 22.6f;
            Spawner(Npc, NpcQtd);
            Spawner(Enemy, EnemyQtd);
            BuildWall();
        }
    }    
    private void Spawner(GameObject[] npc, int qtd)
    {
        Vector3 spawnPosition;
        GameObject[] npc_Qtd = new GameObject[qtd];
        for(int i = 0; i < qtd; i++)
        {
            spawnPosition.z = Random.Range(MinZ, MaxZ);
            spawnPosition.x = Random.Range(37, 47);
            spawnPosition = new Vector3(transform.position.x + spawnPosition.x + i, 2.9f, spawnPosition.z);
            npc_Qtd[i] = Instantiate(npc[Random.Range(0, npc.Length)], spawnPosition, Quaternion.identity);
            if(npc_Qtd[i].CompareTag("Boss"))
            {
                Vector3 posY = npc_Qtd[i].transform.position;
                posY.y = 5f;
                npc_Qtd[i].transform.position = new Vector3(posY.x, posY.y, posY.z);
            }
            npc_Qtd[i].transform.SetParent(gameObject.transform);
            CurrentNpc++;
        }
    }
    void BuildWall()
    {
        var rightPos = new Vector3(transform.position.x + 64.2f, 0, 0.2f);
        Right = Instantiate(RightWall, rightPos, Quaternion.identity);
        Right.transform.SetParent(gameObject.transform);
        rightPos = new Vector3(transform.position.x + 69.2f, 0, 0.2f);
        RightDespawner = Instantiate(RightWall, rightPos, Quaternion.identity);
        RightDespawner.transform.SetParent(gameObject.transform);
        RightDespawner.tag = "Despawner";

        var leftPos = new Vector3(transform.position.x - 13.6f, 0, -0.7f);
        Left = Instantiate(LeftWall, leftPos, Quaternion.identity);
        Left.transform.SetParent(gameObject.transform);
        leftPos = new Vector3(transform.position.x - 18.6f, 0, -0.7f);
        LeftDespawner = Instantiate(LeftWall, leftPos, Quaternion.identity);
        LeftDespawner.transform.SetParent(gameObject.transform);
        LeftDespawner.tag = "Despawner";
    }
    void FinishCheck()
    {
        if(CurrentNpc >= NpcQtd+EnemyQtd)
        {
            Npcs = SetQtd();

            if(Npcs <= 0)
            {
                if(!setCam)
                {
                    setCam = true;
                    Cam.maxXAndY.x += 60;
                }
                if(!StartGo)
                {
                    Gamecontroller.StageGoBar();
                    StartGo = true;
                }
                Destroy(Right);
                Destroy(RightDespawner);
                if(Cam.transform.position.x >= transform.position.x + 81.5f)
                {                    
                    Destroy(Left);
                    Destroy(LeftDespawner);
                    gameObject.SetActive(false);
                }
            }
        }
    }
    public int SetQtd()
    {
        return FindObjectsOfType<NpcAI>().Length;
    }
}