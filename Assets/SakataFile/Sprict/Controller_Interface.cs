using System.Collections;
using System.Collections.Generic;

public enum FighterInputData : long
{
    // システム関連
    DECISION        = 0x000000000001,
    CANCEL          = 0x000000000010,
    // 基本操作関連
    SkillCard1      = 0x000000000100,
    SkillCard2      = 0x000000001000,
    SkillCard3      = 0x000000010000,
    SkillCard4      = 0x000000100000,
    UP              = 0x000001000000,
    DOWN            = 0x000010000000,
    RIGHT           = 0x000100000000,
    LEFT            = 0x001000000000,
    // Kinect関連
    RIGHT_EVASION   = 0x010000000000,
    LEFT_EVASION    = 0x100000000000,   

};

public abstract class Controller_Interface
{
    public FighterInputData Control { get; private set; }

    // 接続状況の確認
    public abstract bool ConnectionCheck();

    // 更新処理
    public abstract void Update();
}
