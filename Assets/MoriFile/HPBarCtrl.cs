using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarCtrl : MonoBehaviour
{
    [SerializeField] Slider m_slider;
    int m_maxHP;
    int m_nowHP;

    // Start is called before the first frame update
    void Start()
    {
        if(m_slider == null)
        {
            Debug.LogError("HPバーが設定されていません。");
        }

        m_maxHP = (int)m_slider.maxValue;

        //初手はHPMAX
        m_nowHP = m_maxHP;
        m_slider.value = m_nowHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.Space))
        {
            AddDamage(Random.Range(1,10));
        }

        if(m_nowHP <= 0)
        {
            m_nowHP = 0;
        }
    }

    public void AddDamage(int damage)
    {
        m_nowHP -= damage;
        m_slider.value = m_nowHP;
    }

    //reloadがtrueの場合、現在HPがMAXと同数になる
    public void SetMaxHP(int hp, bool reload = false)
    {
        m_maxHP = hp;
        m_nowHP = m_maxHP;
        m_slider.value = m_nowHP;
    }

    public int GetNowHP()
    {
        return m_nowHP;
    }
}
