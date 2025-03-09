using GB;
using UnityEngine;


public class ProjectTile : MonoBehaviour
{
    enum ProjectTileType{Targeting,Point}
    ProjectTileType projectTileType = ProjectTileType.Targeting;
    Hit _hit;
    Transform _target;
    Vector2 _targetPoint;
    
    [SerializeField] float _speed = 5;
    [SerializeField] string _boomFX;

    Mob _mob;

    bool _isPlaying;


    public ProjectTile SetHit(Hit hit)
    {
        _hit = hit;
        return this;
    }
    public ProjectTile SetTarget(Transform target)
    {
        projectTileType = ProjectTileType.Targeting;
        _target = target;
        _mob = _target.GetComponent<Mob>();
        return this;
    }

    public ProjectTile SetTarget(Vector2 targetPoint)
    {
        projectTileType = ProjectTileType.Point;
        _targetPoint = targetPoint;
        return this;
    }
    public void Play()
    {
        _isPlaying = true;
    }

    void Update()
    {
        if(!_isPlaying) return;

       float dt = GBTime.GetDeltaTime(DEF.T_GAME);
       Vector2 targetPosition = transform.position;
       Vector2 position = transform.position;

        if(projectTileType == ProjectTileType.Targeting)
        {
            if(_target == null)
            {
                Boom();
                ObjectPooling.Return(gameObject);
                _isPlaying = false;
                return;
            }

            if(!_target.gameObject.activeSelf || _mob.IsDead)
            {
                Boom();
                ObjectPooling.Return(gameObject);
                _isPlaying = false;
                return;
            }


            targetPosition = _target.transform.position;
            Vector2 direction = (targetPosition - position).normalized;
            position += direction * _speed * dt;
            if(Vector2.Distance(position,targetPosition) < 0.1f) 
            {
                _target.GetComponent<IBody>().GetHit(_hit);
                Boom();
                ObjectPooling.Return(gameObject);
                _isPlaying = false;
                return;
            }
            transform.position = position;

        }
        else
        {
            targetPosition = _targetPoint;
            Vector2 direction = (targetPosition - position).normalized;
            position += direction * _speed * dt;

            if(Vector2.Distance(position,targetPosition) < 0.1f) 
            {
                Boom();
                _isPlaying = false;
                ObjectPooling.Return(gameObject);
                
                return;
            }

            transform.position = position;


        }
        
    }

    void Boom()
    {
        if(string.IsNullOrEmpty(_boomFX)) return;

        var boom = ObjectPooling.Create(_boomFX);
        if(boom == null) return;
        IBoom b = boom.transform.GetComponent<IBoom>();
        if(b != null) b.SetHit(_hit);
        
        boom.transform.position = transform.position;
        
        

    }




}
