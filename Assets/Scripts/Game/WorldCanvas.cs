using GB;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    void Awake()
    {
        ODataBaseManager.Set("WorldCanvas",transform);
    }

}
