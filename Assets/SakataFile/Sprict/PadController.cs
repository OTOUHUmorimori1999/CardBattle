using System.Collections;
using System.Collections.Generic;

public class PadController : Controller_Interface
{
    private int m_padNum;

    public PadController(int padNum = 0)
    {
        m_padNum = padNum;
    }

    // 接続状況の確認
    public override bool ConnectionCheck()
    {
        return true;
    }

    // 更新処理
    public override void Update()
    {
        
        throw new System.NotImplementedException();
    }

}
