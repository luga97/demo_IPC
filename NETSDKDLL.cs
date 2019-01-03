using System;
using System.Collections.Generic;
using System.Text;

namespace NETSDKDLL_DOTNET
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Net.Sockets;
    using System.Threading;

    public enum REPLAY_IPC_ACTION
    {
        ACTION_PLAY = 0,
        ACTION_PAUSE,
        ACTION_RESUME,
        ACTION_FAST,
        ACTION_SLOW,
        ACTION_SEEK,
        ACTION_FRAMESKIP,
        ACTION_STOP,
        ACTION_CAPIMG = 10,
        ACTION_CHANGE_SOUND,
        ACTION_RECV_DECODEPARAM,
    };



    public struct LANConfig
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] MACAddress;
        public int dhcpEnable;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] IPAddress;
        public byte[] netMask;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] gateWay;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] DNS1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] DNS2;
    };

    public struct StreamAccessConfig
    {
        public int auth;
        public int videoPort;
        public int rtpoverrtsp;//added by johnnyling 20090323
        public int ptzPort;
        public int webPort;
    };


    public struct UserAccount
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] userName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] password;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] group;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] status;
    };

    public struct UserConfig
    {
        public int count;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 20)]
        public UserAccount[] accounts;
    };


    public struct IPC_ENTRY
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] ipc_sn;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] deviceType;
        public UserConfig userCfg;
        public StreamAccessConfig streamCfg;
        public LANConfig lanCfg;
    };





    //回放时数据头部信息
    public struct UpdPackHead
    {
        public int frame_timestamp;//此帧对应的时间戳，用于音视频同步，一帧中的不同包时间戳相同
        public int keyframe_timestamp;//如果是非I帧，记录其前一I帧的timestamp，如果解码器没有收到前面那个I帧，所有非I帧丢掉丢包不解码
        public short pack_seq;//包序号-65535，到最大后从开始
        public short payload_size;//此包中包含有效数据的长度
        public byte pack_type;//0x01第一包，x10最后一包, 0x11第一包也是最后一包，x00中间包
        public byte frame_type;//帧类型：I帧，：非I帧
        public byte stream_type;//0: video, 1: audio
        public byte stream_index;//流ID号
        public int frame_index; //新增加字段 
    };



    //用到的结构定义
    public struct FRAMNE_INFO
    {
        public int bIsVideo;
        public int bIsKeyFrame;
        public double TimeStamp;
    };


    public struct ALARM_TIME
    {
        public int year;
        public int month;
        public int day;
        public int wday;
        public int hour;
        public int minute;
        public int second;
    };

    public struct ALARM_ITEM
    {
        public ALARM_TIME time;
        public int code;
        public int flag;
        public int level;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] data;
    };



    public struct IP_NET_DVR_CLIENTINFO
    {
        public int lChannel;
        public int lLinkMode;
        public IntPtr hPlayWnd;
        public IntPtr sMultiCastIP;
    };



    public struct IP_NET_DVR_DEVICEINFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] sSerialNumber;
        public byte byAlarmInPortNum;
        public byte byAlarmOutPortNum;
        public byte byDiskNum;
        public byte byDVRType;
        public byte byChanNum;
        public byte byStartChan;
    };




    public struct USRE_VIDEOINFO
    {
        public int nVideoPort;
        public int bIsTcp;
        public int nVideoChannle;
        public IntPtr pUserData;
    };


    public enum ERROR_CODE
    {
        ERR_NOT_FIND_DEVICE = -9000002,
        ERR_OPEN_AUDIOCAPTURE_FAIL,
        ERR_START_AUDIOCAPTURE_FAIL,
        ERR_AUDIO_PARAM_ERROR,//对讲参数不一致
        ERR_AUDIO_NOT_START,//对讲未启动
        ERR_DEV_NOT_CONNECTED,
        ERR_DEV_NOT_LOGIN,
        ERR_MSGTYPE_ERROR,
        ERR_OUTOFF_MEMORY,
        ERR_INIT_SOCKET_ERROR,
        ERR_PARAM_ERROR,
        ERR_NOT_DEV_EXIST,
        ERR_START_THREADERROR,
        ERR_NOT_FIND_STREAM,
        ERR_ISUPLOADING_ERROR,
        ERR_ISDOWNLOADING_ERROR,
        ERR_IS_STARTAUDIO_ERROR,
        ERR_ISFINISH_ERROR,
        ERR_NOT_DOWNLOAD_MODE_ERROR,
        ERR_PTZCMD_ACTION_ERROR,
        ERR_LOC_FILE_ERROR,
        ERR_NOT_REPLAY_MODE_ERROR,
        ERR_PLAY_ACTION_ERROR,
        ERR_NOT_ALLOW_REPLAY_ERROR,
        ERR_MEMORY_SIZE_ERROR,
        ERR_XML_FORMAT_ERROR,
        ERR_CREATE_SOCKET_ERROR,
        ERR_SEND_MODIFYCMD_ERROR,
        ERR_NOT_STARTTALK_MODE_ERROR,
        ERR_RECORD_MEDIA_PARAM_ERROR,
        ERR_RECORD_CREATEERROR,
        ERR_RECORD_ISRECORDING,
        ERR_RECORD_FILEMAXSECONDS_ERROR,
        ERR_RECORD_ALLRECORDSECONDS_ERROR,
        ERR_RECORD_NOTRUNNING,
        ERR_RECORD_STREAMPARAM_ERROR,
        ERR_RECORD_WRITETEMPBUFFER_ERROR,
        ERR_RECORD_ISNOTRECORDSTREAM_MODE,
        ERR_RECORD_NOTINPUTSTREAM_MODE,
        ERR_RECORD_FILEPATH_ERROR,
        ERR_DLL_NOINITORRELEASE_ERROR,
        ERR_SERIAL_NOT_START,
    };


    public enum enumNetSatateEvent
    {
        EVENT_CONNECTING,
        EVENT_CONNECTOK,
        EVENT_CONNECTFAILED,
        EVENT_SOCKETERROR,
        EVENT_LOGINOK,
        EVENT_LOGINFAILED,
        EVENT_STARTAUDIOOK,
        EVENT_STARTAUDIOFAILED,
        EVENT_STOPAUDIOOK,
        EVENT_STOPAUDIOFAILED,
        EVENT_SENDPTZOK,
        EVENT_SENDPTZFAILED,
        EVENT_SENDAUXOK,
        EVENT_SENDAUXFAILED,
        EVENT_UPLOADOK,
        EVENT_UPLOADFAILED,
        EVENT_DOWNLOADOK,
        EVENT_DOWNLOADFAILED,
        EVENT_REMOVEOK,
        EVENT_REMOVEFAILED,
        EVENT_SENDPTZERROR,
        EVENT_PTZPRESETINFO,
        EVNET_PTZNOPRESETINFO,
        EVENT_PTZALARM,
        EVENT_RECVVIDEOPARAM,
        EVENT_RECVAUDIOPARAM,
        EVENT_CONNECTRTSPERROR,
        EVENT_CONNECTRTSPOK,
        EVENT_RTSPTHREADEXIT,
        EVENT_URLERROR,
        EVENT_RECVVIDEOAUDIOPARAM,
        EVENT_LOGIN_USERERROR,
        EVENT_LOGOUT_FINISH,			//登录线程已停止
        EVENT_LOGIN_RECONNECT,			//进行重新登录
        EVENT_LOGIN_HEARTBEAT_LOST,		//心跳丢失
        EVENT_STARTAUDIO_ISBUSY,		//
        EVENT_STARTAUDIO_PARAMERROR,
        EVENT_STARTAUDIO_AUDIODDISABLED,
        EVENT_CONNECT_RTSPSERVER_ERROR,
        EVENT_CREATE_RTSPCLIENT_ERROR,
        EVENT_GET_RTSP_CMDOPTION_ERROR,
        EVENT_RTSP_AUTHERROR,
        EVNET_RECORD_FILEBEGIN,
        EVENT_RECORD_FILEEND,
        EVENT_RECORD_TASKEND,
        EVENT_RECORD_DISKFREESPACE_TOOLOW,
        EVNET_RECORD_FILEBEGIN_ERROR,
        EVNET_RECORD_WRITE_FILE_ERROR,
        EVENT_RECORD_INITAVIHEAD_ERROR,
        EVENT_RECORD_MEDIA_PARAM_ERROR,    //49
        EVENT_NVR_CHANNELS,
        EVENT_NVR_IPC_STATUS,
        EVENT_SYSTEMREBOOT_ANDRELOGINOK,
        EVENT_NETWORKRESET_ANDRE
    };








    public enum PTZ_CMD_TYPE
    {
        LIGHT_PWRON = 2,// 2 接通灯光电源 
        WIPER_PWRON,// 3 接通雨刷开关 
        FAN_PWRON,// 4 接通风扇开关 
        HEATER_PWRON,// 5 接通加热器开关 
        AUX_PWRON1,// 6 接通辅助设备开关 
        AUX_PWRON2,// 7 接通辅助设备开关 
        ZOOM_IN = 11,// 焦距变大(倍率变大) 
        ZOOM_OUT, //12 焦距变小(倍率变小) 
        FOCUS_NEAR, //13 焦点前调 
        FOCUS_FAR, //14 焦点后调 
        IRIS_OPEN, //15 光圈扩大 
        IRIS_CLOSE,// 16 光圈缩小 
        TILT_UP,// 21 云台上仰 
        TILT_DOWN, //22 云台下俯 
        PAN_LEFT,// 23 云台左转 
        PAN_RIGHT,// 24 云台右转 
        UP_LEFT,// 25 云台上仰和左转 
        UP_RIGHT,// 26 云台上仰和右转 
        DOWN_LEFT, //27 云台下俯和左转 
        DOWN_RIGHT,// 28 云台下俯和右转 
        PAN_AUTO,// 29 云台左右自动扫描 
        STOPACTION
    };


    public enum PTZ_PRESET_TYPE
    {
        SET_PRESET = 8,
        CLE_PRESET = 9,
        GOTO_PRESET = 39
    };





    public struct VIDEO_PARAM
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] codec;
        public int width;
        public int height;
        public int colorbits;
        public int framerate;
        public int bitrate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] vol_data;
        public int vol_length;
    };

    public struct AUDIO_PARAM
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] codec;
        public int samplerate;
        public int bitspersample;
        public int channels;
        public int framerate;
        public int bitrate;
    };



    public struct STREAM_AV_PARAM
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] ProtocolName;	//==AV_FALG
        public short bHaveVideo;//0 表示没有视频参数
        public short bHaveAudio;//0 表示没有音频参数
        public VIDEO_PARAM videoParam;//视频参数
        public AUDIO_PARAM audioParam;//音频参数
    };




    public struct FRAME_EXTDATA
    {
        public int bIsKey;
        public double timestamp;
        public IntPtr pUserData;
    };

    public struct IP_NET_DVR_ALARMER
    {
 
    };


    public delegate int MSGCallBack(int lCommand, ref IP_NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, int BufLen, IntPtr pUser);
    public delegate int StatusEventCallBack(int lUser, int nStateCode, IntPtr pResponse, IntPtr pUser);
    public delegate int AUXResponseCallBack(int lUser, int nType, IntPtr pResponse, IntPtr pUser);
    public delegate int fVoiceDataCallBack(int lVoiceComHandle, IntPtr pRecvDataBuffer, int dwBufSize, byte byAudioFlag, ref FRAME_EXTDATA pUser);
    public delegate int fRealDataCallBack(int lRealHandle, int dwDataType, IntPtr pBuffer, int dwBufSize, ref FRAME_EXTDATA pExtData);
    public delegate int fPlayActionEventCallBack(int lUser, int nType, int nFlag, IntPtr pData, IntPtr pUser);
    public delegate int fExceptionCallBack(int dwType, int lUserID, int lHandle, IntPtr pUser);



    public delegate int fEncodeAudioCallBack(int lType, long lPara1, long lPara2);//add 20130217 play file by zfuwen,lType==0x10000002
    public delegate int fSerialDataCallBack(int lUser, IntPtr pRecvDataBuffer, int dwBufSize, IntPtr pUser);




    public enum FILE_TYPE
    {
        LOG_FILE,
        RECORD_FILE,
        CONFIG_FILE,
        UPDATE_FILE
    };

    public class NETSDKDLL
    {


        /// <summary>
        /// 初始化，调用库前应该先调用此函数
        /// </summary>
        /// <returns></returns>
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_Init();
        /// <summary>
        /// 释放所有资源，退出进程前应该调用一次此函数
        /// </summary>
        /// <returns></returns>
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_Cleanup();
        /// <summary>
        /// 注意：从V1.0.2.7默认不会自动重连，如果想要设备进行自动重启，要登录设置之前要调用IP_NET_DVR_SetAutoReconnect(0,1)
        /// </summary>
        /// <param name="lUserID"></param>
        /// <param name="bAutoReconnect"></param>
        /// <returns></returns>
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetAutoReconnect(int lUserID, int bAutoReconnect);

        /// <summary>
        /// 使用于不自动重连模式，调用此函数，设备会再登录一次
        /// </summary>
        /// <param name="lUserID"></param>
        /// <returns></returns>
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_Reconnect(int lUserID);

        /// <summary>
        /// 登录设备,0表示失败
        /// </summary>
        /// <param name="sDVRIP"></param>
        /// <param name="wDVRPort"></param>
        /// <param name="sUserName"></param>
        /// <param name="sPassword"></param>
        /// <param name="lpDeviceInfo"></param>
        /// <returns></returns>
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_Login(string sDVRIP, short wDVRPort, string sUserName, string sPassword, ref IP_NET_DVR_DEVICEINFO lpDeviceInfo);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="lUserID"></param>
        /// <returns></returns>
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_Logout(int lUserID);
        /// <summary>
        /// 退出所有登录，注意：如果同一进程里使用多个时，不要调用此函数，否则所将所有线程里的登录都退出
        /// </summary>
        /// <returns></returns>
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_LogoutAll();


        //全局callback
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetDVRMessage(UInt32 nMessage, IntPtr hWnd);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetExceptionCallBack(UInt32 nMessage, IntPtr hWnd, fExceptionCallBack cbExceptionCallBack, IntPtr pUser);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetAUXResponseCallBack(AUXResponseCallBack fAUXCallBack, IntPtr pUser);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetStatusEventCallBack(StatusEventCallBack fStatusEventCallBack, IntPtr pUser);




        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetSDKBuildVersion();
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetSDKVersion();
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetLogToFile(int bLogEnable, string strLogDir, int bAutoDel);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_RealPlay(int lUserID, ref IP_NET_DVR_CLIENTINFO lpClientInfo, fRealDataCallBack cbRealDataCallBack, ref USRE_VIDEOINFO pUser, int bBlocked);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_RealPlayEx(string serverip, string user, string pass, fRealDataCallBack cbRealDataCallBack, ref USRE_VIDEOINFO pUser, int bBlocked);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StopRealPlay(int lRealHandle);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StopAllRealPlay();
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetVideoParam(int lRealHandle, ref VIDEO_PARAM pParam);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetAudioParam(int lRealHandle, ref AUDIO_PARAM pParam);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetRealDataCallBack(fRealDataCallBack cbRealDataCallBack, IntPtr dwUser);


        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetDVRMessageCallBack(MSGCallBack fMessageCallBack, IntPtr pUser);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_PTZControl(int lUser, int dwPTZCommand, int nTspeed, int nSpeed);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_PTZPreset(int lUser, int dwPTZPresetCmd, int dwPresetIndex);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_PTZControlEx(int lUser, string pXml);

        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_FormatDisk(int lUserID, int lDiskNumber);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetFormatProgress(int lFormatHandle, ref int pCurrentFormatDisk, ref int pCurrentDiskPos, ref int pFormatStatic);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_Upgrade(int lUserID, string sFileName);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetUpgradeProgress(int lUserID);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetUpgradeState(int lUserID);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_CloseUpgradeHandle(int lUserID);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_FindDVRLogFile(int lUserID);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_RestoreConfig(int lUserID);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetConfigFile(int lUserID, string sFileName);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetConfigFile(int lUserID, string sFileName);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_RebootDVR(int lUserID);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_ShutDownDVR(int lUserID);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetDVRConfig(int lUserID, int dwCommand, int lChannel, IntPtr lpOutBuffer, int dwOutBufferSize, ref int lpBytesReturned);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SystemControl(int lUserID, int nCmdValue, int flag, string pXml);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetDVRConfig(int lUserID, int dwCommand, int lChannel, string pXml, int dwInBufferSize);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetDeviceAbility(int lUserID);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_WriteAUXStringEx(int lUserID, string pMsgType, int nCode, int flag, string pXml);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_CreateIFrame(UInt32 lUserId, int bIsSubStream);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetUserData(UInt32 lUserId, IntPtr pOutBuffer, ref int nInOutLen);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetUserData(UInt32 lUserId, IntPtr pBuffer, int len);


        //文件上传下载
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetFileByName(int lUserID, int nFileType, string sDVRFileName, string saveDir);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StopGetFile(int lFileHandle);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetDownloadState(int lFileHandle);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetDownloadPos(int lFileHandle);

        //ipc文件回放
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_ControlPlay(int lUserID, int Action, int param);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetReplayAblity(int lUserID);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_PlayDeviceFile(int lUserID, string filenme);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetReplayDataCallBack(fRealDataCallBack cbReplayDataCallBack, IntPtr dwUser);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetPlayActionEventCallBack(fPlayActionEventCallBack cbActionCallback, IntPtr dwUser);


        //设备搜索功能
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StartSearchIPC();
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StopSearchIPC();
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetSearchIPCCount();
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetIPCInfo(int index, ref IPC_ENTRY pIPCInfo);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_ModifyIPC(int index, ref IPC_ENTRY pIPCInfo);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetIPCInfoXML(int index, StringBuilder pXMLInfo, int maxLen);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_ModifyIPCXML(int index, string strXML);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetOneIPAddress(StringBuilder strResult, int nSize);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetNetworkParam(UInt32 nParamIndex, StringBuilder strResult, int nSize);

        //广播音频相关(对讲)
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StartTalk(int audiotype, int samplerate, int bitspersample, int channels);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StopTalk();
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_AddTalk(int lUserID);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_RemoveTalk(int lUserID);

        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StartVoiceCom(int lUserID, int dwVoiceChan, int bNeedCBNoEncData, fVoiceDataCallBack cbVoiceDataCallBack, IntPtr pUser);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetVoiceComClientVolume(int lVoiceComHandle, short wVolume);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StopVoiceCom(int lVoiceComHandle);




        /// <summary>
        /// 本地收流式启动录像
        /// </summary>
        /// <param name="lRealHandle">请求流句柄</param>
        /// <param name="fileNameOrPath">录像文件目录或文件名，如果文件名以.avi结尾，则会只录制一个文件，此时nAllRecordMaxSeconds无效，否则会当前目录，当作目录时，会以每nFileMaxSeconds秒录制一个文件录nAllRecordMaxSeconds秒，同时文件会保存到当前日期为目录，录制时间为文件名的文件中</param>
        /// <param name="nFileMaxSeconds">单文件最大时间，为10至3600，当为高清时，此值不要超过1800，否则文件可能无法播放，单位秒</param>
        /// <param name="nAllRecordMaxSeconds">总共录制多长时间，如果长时间录像，可以设为1年的时间，如24*60*60*360即录制360天，单位秒</param>
        /// <returns></returns>
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StartRecord(int lRealHandle, string fileNameOrPath, int nFileMaxSeconds, int nAllRecordMaxSeconds);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StopRecord(int lRealHandle);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_GetLastErrorCode(int nType);
        [DllImport("NetSDKDLL.dll")]
        /// <summary>
        /// 外部收流式启动录像
        /// </summary>
        /// <param name="lRealHandle">解码相关参数，与状态返回事件EVENT_RECVVIDEOAUDIOPARAM中的pResonse参数完全一样</param>
        /// <param name="fileNameOrPath">录像文件目录或文件名，如果文件名以.avi结尾，则会只录制一个文件，此时nAllRecordMaxSeconds无效，否则会当前目录，当作目录时，会以每nFileMaxSeconds秒录制一个文件录nAllRecordMaxSeconds秒，同时文件会保存到当前日期为目录，录制时间为文件名的文件中</param>
        /// <param name="nFileMaxSeconds">单文件最大时间，为10至3600，当为高清时，此值不要超过1800，否则文件可能无法播放，单位秒</param>
        /// <param name="nAllRecordMaxSeconds">总共录制多长时间，如果长时间录像，可以设为1年的时间，如24*60*60*360即录制360天，单位秒</param>
        /// <returns></returns>
        public static extern int IP_NET_DVR_StartRecordStream(ref STREAM_AV_PARAM pAvParam, string fileNameOrPath, int nFileMaxSeconds, int nAllRecordMaxSeconds);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_InputRecordStream(int lRealHandle, IntPtr pBUffer, int nBufferSize, int isVideo, int isKey, double timestamp);
        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_StopRecordStream(int lRealHandle);


        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_AddInviteAudioStream(ref int lUserId);//add 20130219 by zfuwen



        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_RemoveInviteAudioStream(int lUserId);//add 20130219 by zfuwen


        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SetEncodeAudioCallBack(int lUserId, fEncodeAudioCallBack fnCallBack);//add 20130219 by zfuwen


        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SerialStart(int lUserId, fSerialDataCallBack cbSDCallBack,IntPtr pUser);


        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SerialSend(int lUserId, int lChannel, IntPtr pSendBuf);


        [DllImport("NetSDKDLL.dll")]
        public static extern int IP_NET_DVR_SerialStop(int lUserId);

    }
}
