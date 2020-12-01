using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] OnlineConnecter connecter;
    [SerializeField] SelectCamera selectCamera;

    [SerializeField] Text charaName;
    [SerializeField] Image charaImage;
    [Space(5)]
    [SerializeField] string nextSceneName;
    
    [System.Serializable]
    public struct CharacterData
    {
        [SerializeField] public string name;
        [SerializeField] public Sprite sprite;
        [SerializeField] public GameObject modelAvatar;
    }
    [Space(10)]
    [SerializeField] CharacterData[] characterDatas;

    GameObject pobj;
    int dataNum;

    // Start is called before the first frame update
    void Start()
    {
        pobj = null;

        if (characterDatas == null)
        {
            //デフォルトはMK46Aになる
            characterDatas = new CharacterData[1];
            characterDatas[0].name = "MK46A";
            characterDatas[0].sprite = null;
        }

        dataNum = 0;

        SetSelectCharacter(characterDatas[0]);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SetSelectCharacter(CharacterData data)
    {
        charaName.text = data.name;
        charaImage.sprite = data.sprite;
    }

    //セレクト移動
    public void SelectShift(bool left = true)
    {
        if(left)
        {
            //１つ前に戻る
            dataNum -= 1;

            //最初のものだった場合
            if (dataNum < 0)
            {
                //最後のものへ
                dataNum = characterDatas.Length - 1;
            }
        }
        else
        {
            //１つ後に進む
            dataNum += 1;

            //配列を超えた場合
            if(dataNum >= characterDatas.Length)
            {
                //最初のものへ
                dataNum = 0;
            }
        }

        SetSelectCharacter(characterDatas[dataNum]);
    }

    public void LogIn()
    {
        //接続
        connecter.Connect();
        
        if(connecter.GetConnected())
        {
            //アバター召喚
            CreateAvatar();

            //対戦相手待ちへ
            selectCamera.changeRotFlag();
        }
        else
        {
            Debug.LogError("not connected.");
        }
    }

    public void LogOut()
    {
        connecter.ReaveNetwork();

        Destroy(pobj.gameObject);
        pobj = null;

        selectCamera.changeRotFlag();
    }

    public void CreateAvatar()
    {
        pobj = Instantiate(characterDatas[dataNum].modelAvatar);
        WaitRival wr = pobj.AddComponent<WaitRival>();
    }
}
