using UnityEngine;

public class SpiderAnimationEvent : MonoBehaviour
{
    private Spider _spider;

    void Awake()
    {
        _spider = GetComponentInParent<Spider>();
    }

    public void Fire()
    {
        _spider.Attack();
    }

}
