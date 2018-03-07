CS_Animation_Thread(C#多執行緒+等待動畫)
	SYRIS_Offline - 2016/09/23 
		[
			建立等待動畫，並建立執行緒執行去執行
		]
		static public void createThreadAnimation(String StrMsg, ParameterizedThreadStart fun)
		static public void Animation_Wait_testLogin(object arg)
		Animation_Wait_GetData(object arg)

	SYRIS_V8 - 2016/10/18 
		[
			建立等待動畫，並建立執行緒執行去執行
		]
		static public void createThreadAnimation(String StrMsg, ParameterizedThreadStart fun)
		static public void Animation_Wait_testLogin(object arg)
		static public void Animation_Wait_GetData(object arg)

	SYRIS_Fingerprint - 2016/10/31
		[
				Animation.createThreadAnimation(CWnd00_button1.Text, Animation.Animation_Wait_testlogin);-單純 建立等待動畫，並建立執行緒執行去執行
				Animation.createThreadAnimation(CWnd00_button3.Text, Animation.Animation_Wait_ImportData);-單純 建立等待動畫，並建立執行緒執行去執行
				Animation.createThreadAnimation(this.Text, Animation.Animation_Wait_treeView01);-ui元件複製到動畫物件中 + 單純 建立等待動畫，並建立執行緒執行去執行
				Animation.createThreadAnimation(this.Text, Animation.Animation_Wait_treeView02);-ui元件複製到動畫物件中 + 單純 建立等待動畫，並建立執行緒執行去執行
				Animation.createThreadAnimationForSYFCLib(Language.m_StrFingerprint_Msg01, Animation.Animation_Wait_SYFCLibAPI);-單純 建立等待動畫，但建立一個以上的執行緒
		]
		static public void createThreadAnimation(String StrMsg, ParameterizedThreadStart fun)
		static public void createThreadAnimationForSYFCLib(String StrMsg, ParameterizedThreadStart fun)
		static public void HWThreadFun()
		static public void Animation_Wait_SYFCLibAPI(object arg)
		static public void Animation_Wait_treeView01(object arg)
		static public void Animation_Wait_treeView02(object arg)
		static public void Animation_Wait_testlogin(object arg)
		static public void Animation_Wait_ImportData(object arg)

	SYRIS_ControllerUtility_V8 - 2017/04/26 
		[
			建立等待動畫，並建立執行緒執行去執行
		]
		public static void createThreadAnimation(String StrMsg, ParameterizedThreadStart fun)
		public static void Thread_function_Connect(object arg)