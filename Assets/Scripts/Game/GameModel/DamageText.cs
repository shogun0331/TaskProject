using DG.Tweening;
using GB;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField] Text _damageText;

    void OnEnable()
    {
        transform.SetParent(ODataBaseManager.Get<Transform>("WorldCanvas"));
        transform.localScale = Vector2.one;
    }

    public void SetDamage(int damage)
    {
        _damageText.text = damage.ToString("N0");
        _damageText.transform.transform.localPosition = Vector2.zero;
        _damageText.transform.DOLocalMoveY(10f,1).OnComplete(()=>{ObjectPooling.Return(gameObject);}).Restart();
    }
    
}
