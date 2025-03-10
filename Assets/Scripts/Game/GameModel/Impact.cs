using System.Linq;
using GB;
using UnityEngine;

public class Impact : MonoBehaviour ,IBoom
{
    Hit _hit;

    [SerializeField] float _radius = 1;

    [SerializeField] SPRAnimation _anim;


    void OnEnable()
    {
        _anim.TriggerEvent.AddListener(OnAnimEvent);
        _anim.EndEvent.AddListener(OnAnimEnd);
    }
    void OnDisable()
    {
        _anim.TriggerEvent.RemoveListener(OnAnimEvent);
        _anim.EndEvent.RemoveListener(OnAnimEnd);
    }

    void OnAnimEvent(TriggerData trigger)
    {
        Fire();
    }

    void OnAnimEnd()
    {
        ObjectPooling.Return(gameObject);
    }
    
    public void SetHit(Hit hit)
    {
        _hit = hit;

    }

    void Fire()
    {
     
        Vector2 position = transform.position;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, _radius);

        
        foreach (Collider2D coll in hitColliders)
        {
            IBody mob = coll.GetComponent<IBody>();
            if(mob != null) 
            {
                float ad = _hit.AD * _hit.skillProb.AD_Value;
                float ap = _hit.AP * _hit.skillProb.AP_Value;


                var h =  new Hit
                {
                    player = _hit.player,
                    AD = (int)ad,
                    AP = (int)ap,
                };

                
                if(mob != null) mob.GetHit(h);

            }
        }

    }




}
