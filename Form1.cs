using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NETSDKDLL_DOTNET;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Threading;

namespace DOTNET_PLAY
{
    public partial class Form1 : Form
    {

        NETSDKDLL_DOTNET.fSerialDataCallBack fSeriaDataCallback = null;
        NETSDKDLL_DOTNET.fRealDataCallBack fReadlCallBack = null;
        NETSDKDLL_DOTNET.fDecCallBackFunction fDecodecall = null;
        NETSDKDLL_DOTNET.StatusEventCallBack fStateEvent = null;

        public Form1()
        {
            InitializeComponent();

            NETSDKDLL_DOTNET.PLAYERDLL.IP_TPS_Init();
            NETSDKDLL_DOTNET.NETSDKDLL.IP_NET_DVR_Init();
            fDecodecall = new fDecCallBackFunction(OnDecCallBackFunction);
            fReadlCallBack = new fRealDataCallBack(OnRealDataCallBack);
            fStateEvent = new NETSDKDLL_DOTNET.StatusEventCallBack(OnStatusEventCallBack);
            //fSeriaDataCallback = new fSerialDataCallBack();
            NETSDKDLL.IP_NET_DVR_SetStatusEventCallBack(fStateEvent, IntPtr.Zero);
        }




        /// <summary>
        /// 收到设备端传过来的485数据
        /// </summary>
        /// <param name="lUser"></param>
        /// <param name="pRecvDataBuffer"></param>
        /// <param name="dwBufSize"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        public int OnSeriaDataCallback(int lUser, IntPtr pRecvDataBuffer, int dwBufSize, IntPtr pUser)
        {

            return 0;
        }

        


        public int lRealStreamId = 0;
        public int nPortId = 0;


        //NETSDKDLL_DOTNET.fSerialDataCallBack fSeriaDataCallback = null;



        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            NETSDKDLL_DOTNET.NETSDKDLL.IP_NET_DVR_Cleanup();
            NETSDKDLL_DOTNET.PLAYERDLL.IP_TPS_ReleaseAll();
        }

        public int OnRealDataCallBack(int lRealHandle, int dwDataType, IntPtr pBuffer, int dwBufSize, ref FRAME_EXTDATA pExtData)
        {
            IntPtr pWnd = pExtData.pUserData;
            if (dwDataType == 0)
            {
                return PLAYERDLL.IP_TPS_InputVideoData(1, pBuffer, dwBufSize, pExtData.bIsKey, (int)pExtData.timestamp);
            }
            else if (dwDataType == 1)
            {

            }
            else if (dwDataType == 2)
            {
                STREAM_AV_PARAM avParam = new STREAM_AV_PARAM();
                avParam = (STREAM_AV_PARAM)Marshal.PtrToStructure(pBuffer, avParam.GetType());
                int size = Marshal.SizeOf(typeof(VIDEO_PARAM));
                IntPtr pVideoParam = Marshal.AllocHGlobal(10240);
                Marshal.StructureToPtr(avParam.videoParam, pVideoParam, false);
                NETSDKDLL_DOTNET.PLAYERDLL.IP_TPS_OpenStream(1, pVideoParam, size, 0, 40);
                IntPtr p = new IntPtr(1);
                NETSDKDLL_DOTNET.PLAYERDLL.IP_TPS_SetDecCallBack(1, fDecodecall,p);
                Marshal.FreeHGlobal(pVideoParam);
                if (avParam.bHaveAudio != 0)
                {//说明有音频 
                    int haveaudio = 1;
                }
                //NETSDKDLL_DOTNET.PLAYERDLL.IP_TPS_Play(1, pWnd);
                NETSDKDLL_DOTNET.PLAYERDLL.IP_TPS_Play(1, IntPtr.Zero);
            }
            return 0;
        }

        public bool haveLogin = false;

        public int OnStatusEventCallBack(int lUser, int nStateCode, IntPtr pResponse, IntPtr pUser)
        {
            if (nStateCode == (int)NETSDKDLL_DOTNET.enumNetSatateEvent.EVENT_LOGINOK)
            {
                haveLogin = true;
            }
            else if (nStateCode == (int)NETSDKDLL_DOTNET.enumNetSatateEvent.EVENT_LOGINFAILED || nStateCode == (int)NETSDKDLL_DOTNET.enumNetSatateEvent.EVENT_LOGIN_USERERROR)
            {
                haveLogin = false;
            }
            return 0;
        }


        public int OnDecCallBackFunction(int nPort, IntPtr pBuf, int nSize, ref FRAME_INFO pFrameInfo, IntPtr pUser, int nReserved2)
        {
            return 0;
        }


        private void Bplay_Click(object sender, EventArgs e)
        {



            if (nLoginId == 0)
            {
                MessageBox.Show(this, "please login the device first!");
                return;
            }
            else if (!haveLogin)
            {
                MessageBox.Show(this, "device online state error,please play video after login device!");
                return;
            }
            if (lRealStreamId != 0)
            {
                NETSDKDLL_DOTNET.NETSDKDLL.IP_NET_DVR_StopRealPlay(lRealStreamId);
            }
            if (nPortId != 0)
            {//如果当前正在播放，则停止解码
                NETSDKDLL_DOTNET.PLAYERDLL.IP_TPS_CloseStream(nPortId);
            }
            NETSDKDLL_DOTNET.IP_NET_DVR_CLIENTINFO clientInfo = new IP_NET_DVR_CLIENTINFO();
            NETSDKDLL_DOTNET.USRE_VIDEOINFO puser = new USRE_VIDEOINFO();
            clientInfo.lChannel = 1;
            puser.nVideoPort = 554;
            if (CBmainstream.Checked)
            {
                puser.nVideoChannle = 0;//IPC设备的子码流
            }
            else
            {
                puser.nVideoChannle = 1;//IPC设备的子码流
            }
            puser.pUserData = pictureBox1.Handle;
            nPortId = 1;
            lRealStreamId = NETSDKDLL_DOTNET.NETSDKDLL.IP_NET_DVR_RealPlay(nLoginId, ref clientInfo, fReadlCallBack, ref puser, 1);
        
            if (lRealStreamId == 0)
            {
                MessageBox.Show(this, "please login the device first!");
                return;
            }

           
        }


        private void BDecode_Click(object sender, EventArgs e)
        {

        }

        public int nLoginId = 0;
        private void BLogin_Click(object sender, EventArgs e)
        {
            if (nLoginId != 0)
            {
                NETSDKDLL_DOTNET.NETSDKDLL.IP_NET_DVR_Logout(nLoginId);
                haveLogin = false;
            }
            
            NETSDKDLL_DOTNET.IP_NET_DVR_DEVICEINFO devInfo = new IP_NET_DVR_DEVICEINFO();
            nLoginId = NETSDKDLL_DOTNET.NETSDKDLL.IP_NET_DVR_Login(Tip.Text, short.Parse(Tport.Text), "admin", "123456", ref devInfo);
        }

        private void Bstart_Click(object sender, EventArgs e)
        {
            if(nLoginId==0)
            {
                MessageBox.Show(this,"请先登录","");
                return;
            }
            int nRet= NETSDKDLL_DOTNET.NETSDKDLL.IP_NET_DVR_SerialStart(nLoginId,fSeriaDataCallback,IntPtr.Zero);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}