using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    ArenaSpawner arenaSpawner;
    Transform playerTransform;
    PlayerMovement playerMovement;

    public ArenaType arenaType;
    public float hidingDistance;
    public bool playerContact = false;

    public enum ArenaType
    {
        Breather,
        Trees,
        TreesStones,
        Hollow,
        Trunk,
        Lake,
        Cage,
        Spidernet,
        Random
    }

    public GameObject breatherTerrain;
    public List<GameObject> treesTerrain;
    public List<GameObject> treesStonesTerrain;
    public List<GameObject> hollowTerrain;
    public List<GameObject> trunkTerrain;
    public List<GameObject> lakeTerrain;
    public List<GameObject> cageTerrain;
    public List<GameObject> spidernetTerrain;


    public static System.Action<Arena> PushToPool;
    public static System.Action<Arena> SpawnArena;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerMovement>();
        playerTransform = playerMovement.transform;
        arenaSpawner = GameObject.FindGameObjectWithTag(Tags.ArenaSpawner).GetComponent<ArenaSpawner>();
    }

    public void SpawnField(ArenaType _arenaType = ArenaType.Breather)
    {
        arenaType = _arenaType;
        switch (_arenaType)
        {
            case ArenaType.Breather:
                breatherTerrain.SetActive(true);
                break;
            case ArenaType.Trees:
                SelectRandom(treesTerrain);
                break;
            case ArenaType.TreesStones:
                SelectRandom(treesStonesTerrain);
                break;
            case ArenaType.Hollow:
                SelectRandom(hollowTerrain);
                break;
            case ArenaType.Trunk:
                SelectRandom(trunkTerrain);
                break;
            case ArenaType.Lake:
                SelectRandom(lakeTerrain);
                break;
            case ArenaType.Cage:
                SelectRandom(cageTerrain);
                break;
            case ArenaType.Spidernet:
                SelectRandom(spidernetTerrain);
                break;
            case ArenaType.Random:
                SpawnField((ArenaType)Random.Range(0, 8));
                break;
            default:
                break;
        }
    }

    public void SelectRandom(List<GameObject> _gameObjects)
    {
        _gameObjects[Random.Range(0, _gameObjects.Count)].SetActive(true);
    }

    private void Update() => ReturnToPoolCheck();
    public void ReturnToPoolCheck()
    {
        if (playerTransform.position.z - transform.position.z >= hidingDistance)
        {
            PushToPool?.Invoke(this);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // If player makes contact with the arena for fist time, then spawn arenas
    //    if (collision.collider.CompareTag(Tags.Player) && !playerContact)
    //    {
    //        playerContact = true;
    //        SpawnArena?.Invoke(this);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player) && !playerContact)
        {
            playerContact = true;
            SpawnArena?.Invoke(this);
        }
    }

    public void ResetArena()
    {
        playerContact = false;

        breatherTerrain.SetActive(false);
        treesTerrain.ForEach(x => x.SetActive(false));
        treesStonesTerrain.ForEach(x => x.SetActive(false));
        hollowTerrain.ForEach(x => x.SetActive(false));
        trunkTerrain.ForEach(x => x.SetActive(false));
        lakeTerrain.ForEach(x => x.SetActive(false));
        cageTerrain.ForEach(x => x.SetActive(false));
        spidernetTerrain.ForEach(x => x.SetActive(false));
    }

    private void OnDisable()
    {
        ResetArena();
    }
}
