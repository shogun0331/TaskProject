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
        _damageText.text = GameUtil.FormatKmt(damage);

       if (damage >= 1000000) // 100만 이상 (m)
        {
            _damageText.color = Color.red;
            
        }
        else if (damage >= 1000) // 1000 이상 (k)
        {
            _damageText.color = Color.yellow;
            
        }
        else
        {
            _damageText.color = Color.white;
            
        }


        _damageText.transform.transform.localPosition = Vector2.zero;
        _damageText.transform.DOLocalMoveY(10f,1).OnComplete(()=>{ObjectPooling.Return(gameObject);}).Restart();
    }
    
}
