using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("ゲーム根幹設定部分")]
    [SerializeField] int GameTime = 99;

    [Header("その他共通利用する変数などなど")]
    [SerializeField] GameObject[] Effects;
    public Sprite[] cardImageDatas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //シーン遷移
    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
    }

    //エフェクト発動
    public GameObject GetEffect(int num)
    {
        if(Effects.Length > num)
        {
            if (Effects[num] != null)
            {
                return Effects[num];
            }
        }

        return null;
    }
}
