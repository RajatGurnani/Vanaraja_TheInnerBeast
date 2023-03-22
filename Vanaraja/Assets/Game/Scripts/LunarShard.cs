using DG.Tweening;
using UnityEngine;

public class LunarShard : MonoBehaviour
{
    public GameObject shard;

    public Renderer ren;
    public float range = 10;
    public static bool isActive = false;

    public void ToggleShard()
    {
        if (InjectionBar.goldenPeriod)
        {
            if (!ren.isVisible)
            {
                shard.transform.localPosition = new Vector3(Random.Range(-range, range), 0, 0);
                shard.SetActive(true);
            }
        }
        else
        {
            shard.SetActive(false);
        }
    }

    public void CheckForSpawn(bool value)
    {
        ToggleShard();
    }

    public void DisableShard()
    {
        shard.SetActive(false);
    }

    public void ActivateDeactivate(bool value)
    {
        if (value)
        {
            shard.transform.localPosition = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
            shard.SetActive(true);
            shard.transform.DOScale(Vector3.one, 0.3f);
        }
        else
        {
            shard.transform.DOScale(Vector3.zero, 2f).OnComplete(() => shard.SetActive(false));
        }
    }

    private void OnEnable()
    {
        shard.SetActive(InjectionBar.goldenPeriod);
        //ToggleShard();
        //InjectionBar.GoldenPeriod += CheckForSpawn;
        InjectionBar.GoldenPeriod += ActivateDeactivate;
    }

    private void OnDisable()
    {
        //InjectionBar.GoldenPeriod -= CheckForSpawn;
        InjectionBar.GoldenPeriod -= ActivateDeactivate;
    }
}