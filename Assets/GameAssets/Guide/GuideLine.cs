using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class GuideLine : MonoBehaviour
{
    LineRenderer _Line;
    
    void Awake()
    {
        _Line = GetComponent<LineRenderer>();
        _Line.positionCount = 2;
        
    }

    public void SetPoint(Vector2 pointA, Vector2 pointB)
    {
        if (_Line == null) return;
         _Line.SetPosition(0, pointA);
         _Line.SetPosition(1, pointB);
    }


}
