using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSpawner : MonoBehaviour
{
    public LayerMask arenaLayer;
    public GameObject arenaPrefab;
    public Vector3 arenaDimension = new(20f, 0, 20f);

    [Header("Arena spawn distances")]
    public float treesDistance;
    public float breather1;
    public float treesStonesDistance;
    public float breather2;
    public float hollowDistance;
    public float breather3;
    public float trunkDistance;
    public float breather4;
    public float lakeDistance;
    public float breather5;
    public float cageDistance;
    public float breather6;
    public float spidernetDistance;

    Queue<Arena> arena = new();
    public int countTemp = 0;
    PlayerMovement playerMovement;

    public enum OffsetDirection
    {
        Left,
        Right,
        Front,
        FrontLeft,
        FrontRight
    };

    /// <summary>
    /// The directions in which the chunk is to be
    /// spawned relative to the position of chunk
    /// the player is currently on
    /// 
    ///     \  |  /
    ///    --  O  --
    /// 
    /// Here the dashes represent the direction and "o"
    /// represents the player chunk
    /// </summary>
    public Dictionary<OffsetDirection, Vector3> offsetsDict = new Dictionary<OffsetDirection, Vector3>()
    {
        { OffsetDirection.Left, new Vector3(-1,0,0) },
        { OffsetDirection.Right, new Vector3(1,0,0) },
        { OffsetDirection.Front, new Vector3(0,0,1) },
        { OffsetDirection.FrontLeft, new Vector3(-1,0,1) },
        { OffsetDirection.FrontRight, new Vector3(1,0,1) }
    };

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        countTemp = arena.Count;
    }

    public void SpawnArena(Arena _arena)
    {
        int distance = playerMovement.distance;
        Arena.ArenaType _arenaType = CheckArenaTypeToSpawn(distance);
        StartCoroutine(DelayedSpawn(_arena, _arenaType));
        //foreach (OffsetDirection i in offsetsDict.Keys)
        //{
        //    Vector3 pos = _arena.transform.position + Vector3.Scale(offsetsDict[i], arenaDimension);

        //    if (!Physics.CheckSphere(pos, 1f, arenaLayer))
        //    {
        //        Arena temp = arena.Count > 0 ? PopFromPool() : CreateArenaFromPrefab();
        //        temp.transform.SetPositionAndRotation(pos, Quaternion.identity);
        //        temp.gameObject.SetActive(true);

        //        if (i == OffsetDirection.Left || i == OffsetDirection.Right)
        //        {
        //            temp.SpawnField(_arena.arenaType);
        //        }
        //        else
        //        {
        //            temp.SpawnField(_arenaType);
        //        }
        //    }
        //}
    }

    IEnumerator DelayedSpawn(Arena _arena, Arena.ArenaType _arenaType)
    {
        foreach (OffsetDirection i in offsetsDict.Keys)
        {
            Vector3 pos = _arena.transform.position + Vector3.Scale(offsetsDict[i], arenaDimension);

            if (!Physics.CheckSphere(pos, 3f, arenaLayer))
            {
                Arena temp = arena.Count > 0 ? PopFromPool() : CreateArenaFromPrefab();
                temp.transform.SetPositionAndRotation(pos, Quaternion.identity);
                temp.gameObject.SetActive(true);

                if (i == OffsetDirection.Left || i == OffsetDirection.Right)
                {
                    temp.SpawnField(_arena.arenaType);
                }
                else
                {
                    temp.SpawnField(_arenaType);
                }
                yield return new WaitUntil(() => temp.isActiveAndEnabled);
            }
        }
    }

    public Arena.ArenaType CheckArenaTypeToSpawn(int distance)
    {
        Arena.ArenaType temp;
        if (distance < treesDistance)
        {
            temp = Arena.ArenaType.Trees;
        }
        else if (distance < breather1)
        {
            temp = Arena.ArenaType.Breather;
        }
        else if (distance < treesStonesDistance)
        {
            temp = Arena.ArenaType.TreesStones;
        }
        else if (distance < breather2)
        {
            temp = Arena.ArenaType.Breather;
        }
        else if (distance < hollowDistance)
        {
            temp = Arena.ArenaType.Hollow;
        }
        else if (distance < breather3)
        {
            temp = Arena.ArenaType.Breather;
        }
        else if (distance < trunkDistance)
        {
            temp = Arena.ArenaType.Trunk;
        }
        else if (distance < breather4)
        {
            temp = Arena.ArenaType.Breather;
        }
        else if (distance < lakeDistance)
        {
            temp = Arena.ArenaType.Lake;
        }
        else if (distance < breather5)
        {
            temp = Arena.ArenaType.Breather;
        }
        else if (distance < cageDistance)
        {
            temp = Arena.ArenaType.Cage;
        }
        else if (distance < breather6)
        {
            temp = Arena.ArenaType.Breather;
        }
        else if (distance < spidernetDistance)
        {
            temp = Arena.ArenaType.Spidernet;
        }
        else
        {
            temp = Arena.ArenaType.Random;
        }
        if (temp == Arena.ArenaType.Breather)
        {
            return temp;
        }
        else
        {
            return (Arena.ArenaType)Random.Range(1, (int)temp + 1);
        }
    }

    public Arena CreateArenaFromPrefab()
    {
        GameObject temp = Instantiate(arenaPrefab, new Vector3(0, 0, -1000f), Quaternion.identity);
        return temp.GetComponent<Arena>();
    }

    public Arena PopFromPool()
    {
        return arena.Dequeue();
    }

    public void PushToPool(Arena _arena)
    {
        arena.Enqueue(_arena);
        _arena.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Arena.PushToPool += PushToPool;
        Arena.SpawnArena += SpawnArena;
    }

    private void OnDisable()
    {
        Arena.PushToPool -= PushToPool;
        Arena.SpawnArena -= SpawnArena;
    }
}
