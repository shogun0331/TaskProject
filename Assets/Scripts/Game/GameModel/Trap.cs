using System.Collections;
using System.Collections.Generic;
using GB;
using UnityEngine;

public class Trap : MonoBehaviour, IBoom
{
    Hit _hit;

    [SerializeField] float _radius = 1;
    [SerializeField] float _duration;

    HashSet<GameObject> _hashMobs = new HashSet<GameObject>();

    void OnEnable()
    {
        _time = 0;
        _hashMobs.Clear();
    }

    void OnDisable()
    {
        foreach(var v in _hashMobs)
        {
            if(v.gameObject.activeSelf)
            {
                var mob = v.GetComponent<Mob>();
                float s = _hit.skillProb.SlowValue / 100;
                mob.SetSlow(-s);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var mob = other.GetComponent<Mob>();
        if(mob != null) 
        {
            if(!_hashMobs.Contains(other.gameObject))
            {
                _hashMobs.Add(other.gameObject);
                mob.SetSlow(_hit.skillProb.SlowValue / 100);

                var h =  new Hit
                {
                    player = _hit.player,
                    AD = _hit.AD * (int)_hit.skillProb.AD_Value,
                    AP = _hit.AP * (int)_hit.skillProb.AP_Value,
                };
                mob.GetHit(h);

            }            
        }
    }
    
    public void SetHit(Hit hit)
    {
        _hit = hit;
        _duration = _hit.skillProb.Duration;
        // Fire();
    }

    float _time;

    void Update()
    {
        _time  += GBTime.GetDeltaTime(DEF.T_GAME);

        if(_time > _duration)
        {
            ObjectPooling.Return(gameObject);
        }
    }
    
    // void Fire()
    // {
    //     Vector2 position = transform.position;
    //     Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, _radius);
        
    //     foreach (Collider2D coll in hitColliders)
    //     {
    //         Mob mob = coll.GetComponent<Mob>();
    //         if(mob != null) 
    //         {
    //             var h =  new Hit
    //             {
    //                 player = _hit.player,
    //                 AD = _hit.AD * (int)_hit.skillProb.AD_Value,
    //                 AP = _hit.AP * (int)_hit.skillProb.AP_Value,
    //             };

    //             mob.GetHit(h);

    //         }
    //     }

    // }








}
