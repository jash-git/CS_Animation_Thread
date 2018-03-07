using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SYRIS_Fingerprint
{
    public class Animation//顯示動畫和透過網路連接SERVER
    {
        static public bool m_blnAnimation = false;
        static public String StrtestIp = "";
        static public String StrtestName = "";
        static public String StrtestPassword = "";
        static public int m_intSYFCLibAns;
        static public void createThreadAnimation(String StrMsg, ParameterizedThreadStart fun)//執行多執行序和顯示等待動畫
        {
            m_blnAnimation = true;//設定變數 讓執行緒是無窮迴圈
            ProgressDialog d = new ProgressDialog();//建立執行動畫對話盒
            Thread t = new Thread(fun);//建立執行緒
            t.Start(d);//呼叫對應實體執行
            d.StartPosition = FormStartPosition.CenterParent;
            d.m_strMessage = StrMsg;
            d.ShowDialog();//顯示動畫對話盒
        }
        static public void createThreadAnimationForSYFCLib(String StrMsg, ParameterizedThreadStart fun)//執行多執行序和顯示等待動畫
        {
            m_intSYFCLibAns = -1;
            m_blnAnimation = true;//設定變數 讓執行緒是無窮迴圈
            ProgressDialog d = new ProgressDialog();//建立執行動畫對話盒
            Thread t = new Thread(fun);//建立執行緒
            t.Start(d);//呼叫對應實體執行
            d.StartPosition = FormStartPosition.CenterParent;
            d.m_strMessage = StrMsg;
            //--
            //建立可以獨立存取HW的執行序
            Thread t1 = new Thread(HWThreadFun);//建立執行緒
            t1.Start();//呼叫對應實體執行
            //--
            d.ShowDialog();//顯示動畫對話盒
        }
        static public ContentWnd01 CW01;
        static public TreeView TV;
        static public String StrOutput="";
        static public void HWThreadFun()
        {
            //------------
            //因為SYFCLib.SYFCLib_run()執行後也會咬住，所以要放在另一個執行緒中
            int Ans;
            Ans = SYFCLib.SYFCLib_run();
            if (Ans == 0)
            {
                SYFCLib.SYFCLib_getData();
            }
            else
            {
                if (Ans > -4)//if (Ans > -3)
                {
                    MessageBox.Show(Language.m_StrFingerprint_Msg02, Language.m_Strtr3_Item01, MessageBoxButtons.OK, MessageBoxIcon.Error); //MessageBox.Show("系統異常");
                }
                else
                {
                    MessageBox.Show(Language.m_StrFingerprint_Msg03, Language.m_Strtr3_Item01, MessageBoxButtons.OK, MessageBoxIcon.Error);//MessageBox.Show("設備異常");
                }
            }
            Animation.m_blnAnimation = false;
            m_intSYFCLibAns = Ans;
            //------------
        }
        static public void Animation_Wait_SYFCLibAPI(object arg)
        {
            ProgressDialog d = (ProgressDialog)arg;
            do{
                int State = SYFCLib.SYFCLib_getState();
                switch (State)
                {
                /*
                //12->壓手指
                //13->放手指
                //11->產生指紋特徵
                //00->OK
                //01->設備忙碌
                case 11:
                    d.m_strMessage = Language.m_StrFingerprint_Msg01 + "\n" + Language.m_StrFingerprint_Msg04;//產生指紋特徵...，請稍後
                    break;
                case 12:
                    d.m_strMessage = Language.m_StrFingerprint_Msg01 + "\n" + Language.m_StrFingerprint_Msg05;//請 『按壓』 手指，謝謝
                    break;
                case 13:
                    d.m_strMessage = Language.m_StrFingerprint_Msg01 + "\n" + Language.m_StrFingerprint_Msg06;//請 『放開』 手指，謝謝
                    break;
                default:
                    d.m_strMessage = Language.m_StrFingerprint_Msg01 + "\n" + Language.m_StrFingerprint_Msg07;//設備運算中...，請稍後
                    break;
                 */
                //14->壓手指
                //13->產生指紋特徵
                //00->OK
                //01->設備忙碌
                case 13:
                    d.m_strMessage = Language.m_StrFingerprint_Msg01 + "\n" + Language.m_StrFingerprint_Msg04;//產生指紋特徵...，請稍後
                    break;
                case 14:
                    d.m_strMessage = Language.m_StrFingerprint_Msg01 + "\n" + Language.m_StrFingerprint_Msg05;//請 『按壓』 手指，謝謝
                    break;
                case 15:
                    d.m_strMessage = Language.m_StrFingerprint_Msg01 + "\n" + Language.m_StrFingerprint_Msg05;//請 『按壓』 手指，謝謝
                    break;
                default:
                    d.m_strMessage = Language.m_StrFingerprint_Msg01 + "\n" + Language.m_StrFingerprint_Msg07;//設備運算中...，請稍後
                    break;
                }
                //Thread.Sleep(1000);
            } while (m_blnAnimation);

            d.Invoke(new Action(d.Close));

        }
        static public void Animation_Wait_treeView01(object arg)
        {
            ProgressDialog d = (ProgressDialog)arg;
            CW01.m_Tree_NodeFun_user.getData(TV);
            for (int i = 0; i < CW01.m_Tree_NodeFun_user.m_ALget.Count; i++)
            {
                StrOutput += CW01.m_Tree_NodeFun_user.m_ALget[i] + ",";
            }
            d.Invoke(new Action(d.Close));
        }
        static public void Animation_Wait_treeView02(object arg)
        {
            ProgressDialog d = (ProgressDialog)arg;
            CW01.m_Tree_NodeFun_door.getData(TV);
            for (int i = 0; i < CW01.m_Tree_NodeFun_door.m_ALget.Count; i++)
            {
                StrOutput += CW01.m_Tree_NodeFun_door.m_ALget[i] + ",";
            }
            d.Invoke(new Action(d.Close));
        }
        static public void Animation_Wait_testlogin(object arg)
        {
            ProgressDialog d = (ProgressDialog)arg;
            Boolean blnAns = false;
            blnAns = PgSQL.connect_test(StrtestIp, "5432", StrtestName, StrtestPassword, "v7");
            m_blnAnimation = blnAns;
            d.Invoke(new Action(d.Close));
        }
        static public void Animation_Wait_ImportData(object arg)
        {
            ProgressDialog d = (ProgressDialog)arg;
            Boolean blnAns = false;
            blnAns = PgSQL.importData(MainWnd.m_MW.m_SettingFile.m_StrServerIP, "5432", MainWnd.m_MW.m_SettingFile.m_StrUserName, MainWnd.m_MW.m_SettingFile.m_StrPassword, "v7");
            m_blnAnimation = blnAns;
            d.Invoke(new Action(d.Close));
        }
    }
}
