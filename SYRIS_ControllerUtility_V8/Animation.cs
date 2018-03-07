using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SYRIS_ControllerUtility_V8
{
    public class Animation//顯示動畫和透過網路連接SERVER
    {
        public static void createThreadAnimation(String StrMsg, ParameterizedThreadStart fun)//執行多執行序和顯示等待動畫
        {
            ProgressDialog d = new ProgressDialog();
            Thread t = new Thread(fun);
            t.Start(d);
            d.StartPosition = FormStartPosition.CenterParent;
            d.m_strMessage = StrMsg;
            d.ShowDialog();
        }

        public static void Thread_function_Connect(object arg)//執行序
        {
            ProgressDialog d = (ProgressDialog)arg;
            //....do something....
            if (HW_Communication.ConnectHW(Form1.MainForm.m_StrIP, Form1.MainForm.m_intPort, Form1.MainForm.m_Client))
            {
                //--
                //cmd+AES+CRC
                String Data;
                String Cmd = "";
                String SubData = "";

                byte[] cmd02 = HW_command.createFirmware_ID(true, true);
                Data = HW_Communication.SendCommand(Form1.MainForm.m_Client, cmd02);
                Cmd = "";
                SubData = "";
                if (!HW_Response.analyze_Base_AES_Response(Data, ref Cmd, ref SubData))
                {
                    cmd02 = HW_command.createFirmware_ID(true, true);
                    //Console.WriteLine("cmd3+AES+CRC_input02 - " + ToHexString(cmd03));
                    Data = HW_Communication.SendCommand(Form1.MainForm.m_Client, cmd02);
                    Cmd = "";
                    SubData = "";
                    if (HW_Response.analyze_Base_AES_Response(Data, ref Cmd, ref SubData))
                    {
                        //Console.WriteLine("cmd2+AES+CRC Response- " + SubData);
                    }
                    else
                    {
                        //Console.WriteLine("cmd2+AES+CRC - Error");
                    }
                }
                else
                {
                    //Console.WriteLine("cmd2+AES+CRC Response- " + SubData);
                }
                HW_Response.analyze_ANS_ID_Response(SubData);
                //Console.WriteLine("\tStrSN ~ " + HW_Response.StrSN);
                //Console.WriteLine("\tStrID ~ " + HW_Response.StrID);
                //Console.WriteLine("\tStrSpecial ~ " + HW_Response.StrSpecial);
                Form1.MainForm.m_StrID = HW_Response.StrSN.Substring(2);//去除0x所以用Substring
                Form1.MainForm.m_StrSN = HW_Response.StrID.Substring(2);//去除0x所以用Substring


                byte[] cmd03 = HW_command.createFirmware_version(true, true);
                //Console.WriteLine("cmd3+AES+CRC_input01 - " + ToHexString(cmd03));
                Data = HW_Communication.SendCommand(Form1.MainForm.m_Client, cmd03);
                Cmd = "";
                SubData = "";
                if (!HW_Response.analyze_Base_AES_Response(Data, ref Cmd, ref SubData))
                {
                    cmd03 = HW_command.createFirmware_version(true, true);
                    //Console.WriteLine("cmd3+AES+CRC_input02 - " + ToHexString(cmd03));
                    Data = HW_Communication.SendCommand(Form1.MainForm.m_Client, cmd03);
                    Cmd = "";
                    SubData = "";
                    if (HW_Response.analyze_Base_AES_Response(Data, ref Cmd, ref SubData))
                    {
                        //Console.WriteLine("cmd3+AES+CRC Response- " + SubData);
                    }
                    else
                    {
                        //Console.WriteLine("cmd3+AES+CRC - Error");
                    }
                }
                else
                {
                    //Console.WriteLine("cmd3+AES+CRC Response- " + SubData);
                }
                HW_Response.analyze_ANS_Version_Response(SubData);
                //Console.WriteLine("\tStrModelName ~ " + HW_Response.StrModelName);
                //Console.WriteLine("\tStrVersion ~ " + HW_Response.StrVersion);
                //Console.WriteLine("\tStrBootLoaderVersion ~ " + HW_Response.StrBootLoaderVersion);
                byte[] byteArray = HW_Response.GetBytes(HW_Response.StrModelName.Substring(2));//去除0x所以用Substring
                Form1.MainForm.m_StrModelName = System.Text.Encoding.Default.GetString(byteArray);
                Form1.MainForm.m_StrVersion = HW_Response.StrVersion.Substring(2);//去除0x所以用Substring
                Form1.MainForm.m_StrFirmwareVersion = HW_Response.StrBootLoaderVersion;
                //--

                HW_Communication.DisconnectHW(Form1.MainForm.m_Client);
                Form1.MainForm.m_blnConnect = true;
            }
            else
            {
                Form1.MainForm.m_blnConnect = false;
            }
            //--Thread.Sleep(3000);
            d.Invoke(new Action(d.Close));
        }
    }
}
