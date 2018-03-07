using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace SYRIS_Offline
{
    public class Animation//顯示動畫和透過網路連接SERVER
    {
        static public bool m_blnAnimation = false;
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
        static public void Animation_Wait_testLogin(object arg)//執行緒實體函數~測試SERVER連線
        {
            ProgressDialog d = (ProgressDialog)arg;
            String StrBuf = "";
            try
            {
                //MainWnd.m_CS_PHP.m_StrUsername和MainWnd.m_CS_PHP.m_StrPassword再呼叫時會先被設定 [ private void SW_button01_Click(object sender, EventArgs e)//測試連嫌 ]
                StrBuf = MainWnd.m_CS_PHP.loginPHP("v8/log/login_submit", MainWnd.m_CS_PHP.m_StrUsername, MainWnd.m_CS_PHP.m_StrPassword);
            }
            catch
            {
                StrBuf = "-1";
            }
            MainWnd.m_CS_PHP.m_StrResponse = StrBuf;
            d.Invoke(new Action(d.Close));
        }
        static public void Animation_Wait_GetData(object arg)//執行緒實體函數~匯入DB資料
        {
            /*
            API:
            登入 v8/log/login_submit
            取門 v8/door_auth/get_doors
            取人 v8/door_auth/get_users
            取部門 v8/door_auth/get_depts
            取部門和人的關係 v8/door_auth/get_user_ext_group
            登出 v8/log/logout
            */
            String StrJson="";
            ProgressDialog d = (ProgressDialog)arg;
            int step = 1;
            string stbuf = d.m_strMessage;
            do
            {
                d.m_strMessage = stbuf + " (" + step+ "/8)";//顯示進度數值
                
                //Thread.Sleep(100);
                switch (step)
                {
                    case 1://登入
                        try
                        {
                            //MainWnd.m_CS_PHP.m_StrUsername和MainWnd.m_CS_PHP.m_StrPassword再呼叫時會先被設定 [public void MenuClicked(object sender, EventArgs e)//Menubar選單回應函數]
                            StrJson = MainWnd.m_CS_PHP.loginPHP("v8/log/login_submit", MainWnd.m_CS_PHP.m_StrUsername, MainWnd.m_CS_PHP.m_StrPassword);
                        }
                        catch
                        {
                            StrJson = "-1";
                        }
                        if (StrJson == "-1")//連線失敗，IP錯誤
                        {
                            step = 500;//直接跳離開 Animation_Wait_GetData()
                            MessageBox.Show(Language.m_StrSW_loginMsg03, Language.m_StrWM_menu02, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StrJson = "-1";
                        }
                        else if (StrJson.Contains("\"result\":false"))//IP正確但登陸失敗
                        {
                            step = 500;//直接跳離開 Animation_Wait_GetData()
                            MessageBox.Show(Language.m_StrSW_loginMsg04, Language.m_StrWM_menu02, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StrJson = "-1";
                        }
                        else
                        {
                            StrJson = "0";
                            SQLite.SQLite_clearDB();//在卻頂可以登錄系統抓取資料時，先清空本地的DB
                            step++;//切換到下一個狀態
                        }
                        MainWnd.m_CS_PHP.m_StrResponse = StrJson;
                        break;
                    case 2://取門
                        try
                        {
                            StrJson = MainWnd.m_CS_PHP.runPHP("v8/door_auth/get_users");
                            StreamWriter sw = new StreamWriter("users.dat");
                            sw.WriteLine(StrJson);// 寫入文字
                            sw.Close();// 關閉串流
                            StrJson = "0";
                            step++;//切換到下一個狀態
                        }
                        catch
                        {
                            step = 500;//直接跳離開 Animation_Wait_GetData()
                            StrJson = "-2";
                        }
                        MainWnd.m_CS_PHP.m_StrResponse = StrJson;
                        break;
                    case 3://取人
                        try
                        {
                            StrJson = MainWnd.m_CS_PHP.runPHP("v8/door_auth/get_doors");
                            StreamWriter sw = new StreamWriter("doors.dat");
                            sw.WriteLine(StrJson);// 寫入文字
                            sw.Close();// 關閉串流
                            StrJson = "0";
                            step++;//切換到下一個狀態
                        }
                        catch
                        {
                            step = 500;//直接跳離開 Animation_Wait_GetData()
                            StrJson = "-3";
                        }
                        MainWnd.m_CS_PHP.m_StrResponse = StrJson;
                        break;
                    case 4://取部門
                        try
                        {
                            StrJson = MainWnd.m_CS_PHP.runPHP("v8/door_auth/get_depts");
                            StreamWriter sw = new StreamWriter("depts.dat");
                            sw.WriteLine(StrJson);// 寫入文字
                            sw.Close();// 關閉串流
                            StrJson = "0";
                            step++;//切換到下一個狀態
                        }
                        catch
                        {
                            step = 500;//直接跳離開 Animation_Wait_GetData()
                            StrJson = "-4";
                        }
                        MainWnd.m_CS_PHP.m_StrResponse = StrJson;
                        break;
                    case 5://取部門和人的關係
                        try
                        {
                            StrJson = MainWnd.m_CS_PHP.runPHP("v8/door_auth/get_user_ext_group");
                            StreamWriter sw = new StreamWriter("user_ext_group.dat");
                            sw.WriteLine(StrJson);// 寫入文字
                            sw.Close();// 關閉串流
                            StrJson = "0";
                            step++;//切換到下一個狀態
                        }
                        catch
                        {
                            step = 500;//直接跳離開 Animation_Wait_GetData()
                            StrJson = "-5";
                        }
                        MainWnd.m_CS_PHP.m_StrResponse = StrJson;
                        break;
                    case 6://遠端SERVER登出
                        try
                        {
                            StrJson = MainWnd.m_CS_PHP.runPHP("v8/log/logout");//執行登出命令
                            StrJson = "0";
                            step++;//切換到下一個狀態
                        }
                        catch
                        {
                            step = 500;//直接跳離開 Animation_Wait_GetData()
                            StrJson = "-6";
                        }
                        MainWnd.m_CS_PHP.m_StrResponse = StrJson;
                        break;
                    case 7://把下載資料寫入本地SQLite資料庫
                        SQLite.SQLite_Json2DB();//把下載資料寫入本地SQLite資料庫
                        step=500;//大於499 因為下面判斷式要停止無窮迴圈條件，關閉等待動畫
                        break;
                }
                if (step > 499)
                {
                    m_blnAnimation = false;//改變 變數狀態 達到跳離迴圈目的
                }
            } while (m_blnAnimation);//無窮迴圈
            d.Invoke(new Action(d.Close));//關閉等待動畫
        }
    }
}
