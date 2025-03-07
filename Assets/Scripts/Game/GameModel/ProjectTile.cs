using System;
using System.Collections;
using System.Collections.Generic;
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

    void Update()
    {
       float dt = GBTime.GetDeltaTime(DEF.T_GAME);
       Vector2 targetPosition = transform.position;
       Vector2 position = transform.position;

        if(projectTileType == ProjectTileType.Targeting)
        {
            if(!_target.gameObject.activeSelf || _mob.IsDead)
            {
                Boom();
                ObjectPooling.Return(gameObject);
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
                ObjectPooling.Return(gameObject);
                return;
            }

            transform.position = position;


        }
        
    }

    void Boom()
    {
        if(string.IsNullOrEmpty(_boomFX)) return;

        var boom = ObjectPooling.Create("FX/"+_boomFX);
        if(boom == null) return;
        boom.transform.position = transform.position;

    }




}
