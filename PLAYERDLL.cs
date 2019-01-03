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




    //消息（LIB）/事件(COM) 代码
    public enum EventCode
    {
        RECVDATATIMEOUT = 1			//收取流时，如果10秒内都没有数据，则会发送此消息
        ,
        FOCUSCHANGE				//播放器焦点改变时发生
       ,
        STARTPLAY					//开始播放
       ,
        ENTERWAITFORBUFFER			//进入缓冲
       ,
        SETUP_VIDEO_PARAM_OK		//SetupDecoder返回成功
       ,
        SETUP_AUDIO_PARAM_OK		//SetupDecoder返回成功
       ,
        SNAPSHORT_FINISH			//抓图结束
       ,
        BINDPORTERROR				//邦定端口失败
       ,
        VSS_STOPPLAY				//播放VSS文件结束
       ,
        VSS_STARTPLAY				//开始播放
       ,
        VSS_PLAYNEXT				//播放下一个文件
       ,
        VSS_SETUPERROR				//调用setup返回失败
       ,
        CREATE_THREAD_ERROR		//创建线程失败
       ,
        LOC_STOPPLAY				//停止本地文件播放
       ,
        LOC_OPENFILEERROR			//本地文件播放时打开文件失败
       ,
        LOC_PLAYERROR				//播放失败退出
       ,
        LOC_PAUSEPLAY				//暂停播放
       ,
        LOC_OPENFILE_ERROR			//打开本地文件失败
       ,
        RECV_RETUN_ERROR			//RTPRECV读取数据返回少于16个字节
       ,
        FIRSTPLAY					//第一次播放
       ,
        RECORDEND
       ,
        CAPTUREPICEND
       , PLAYTIMECHANGE
    };



    public enum PLAYDLL_ERROR_CODE
    {
        ERR_PLY_AUDIOPARAM_ERROR = -999991,
        ERR_PLY_VIDEOPARAM_ERROR,
        ERR_PLY_VIDEOCHANNELID_ERROR,
        ERR_PLY_NOT_DECODER_MODE,
        ERR_PLY_DECODERTHREAD_NOTSTART,
        ERR_PLY_SOUND_OFF_SKIPBUFFER,
        ERR_PLY_NOAUDIOON_ERROR,
        ERR_PLAY_NOTPLAYMODE_ERROR,
        ERR_PLY_DISPLAY_OFF_ERROR,
        ERR_PLAY_FILETYPE_ERROR,
        ERR_PLAY_AVIFILE_ERROR,
        ERR_PLAY_NOTPLAYMODE,
        ERR_PLAY_CONTROL_PARAM_ERROR,
        ERR_PLAY_CONTROLTYPE_ERROR,
        ERR_PLAYER_ISPLAYING_FILE,
        ERR_PLAYER_OPENFILEERROR,
        ERR_PLAY_STOPPLAYFIRST,
        ERR_PLAY_BUFFER_ISFULL,
        ERR_PLAY_NOT_PLAYLOCFILE_MODE,
        ERR_PLAY_NOTFIND_VIDEO_ERROR,
        ERR_PLAY_NOTREPLAY_MODE_ERROR,
        ERR_PLAY_NOPLAYING_ERROR,
        ERR_PLAY_OUT_OFF_MEMORY,
        ERR_PLAY_INITDSOUND_FAIL,
        ERR_PLAY_PARAM_ERROR,
        ERR_POINTER_ISNULL,
    };


    ///播放动作1正常，2停止，3快进，4慢放，5帧进，6定点播放
    public enum PLAYFILE_ACTION
    {
        PLAYER_ACTION_PLAY = 1,
        PLAYER_ACTION_STOP,
        PLAYER_ACTION_FAST,
        PLAYER_ACTION_SLOW,
        PLAYER_ACTION_FRAMESKIP,
        PLAYER_ACTION_SEEK,
        PLAYER_ACTION_PAUSE,
        PLAYER_ACTION_RESUME,
        PLAYER_ACTION_CAPIMG = 10,
        PLAYER_ACTION_CHANGE_SOUND,
        PLAYER_ACTION_RECV_DECODEPARAM,
        PLAYER_ACTION_NOSKIPFRAME_FAST,
    };



    public struct FRAME_INFO
    {
        int nWidth;
        int nHeight;
        int nStamp;
        int nType;
        int nFrameRate;
        int bIsVideo; 
        [MarshalAs(UnmanagedType.ByValArray,ArraySubType=UnmanagedType.I4, SizeConst = 4)]
        int[] nLinseSize;
    };





    public delegate int fDecCallBackFunction(int nPort, IntPtr pBuf, int nSize, ref FRAME_INFO pFrameInfo, IntPtr pUser, int nReserved2);
    public delegate int fStatusEventCallBack(int nPort, Int32 nStateCode, IntPtr pResponse, IntPtr pUser);
    public delegate int fDisplayFinishCallBack(int lType/*=0x10000001*/, IntPtr lPara1/*==HDC*/, int lPara2/*==NULL*/);//add 20130217 play file by zfuwen



    public class PLAYERDLL
    {
        public PLAYERDLL()
        {

        }

        /// <summary>
        /// 调用其它接口之前，应该先调用此接口，进行初始化，结束时，应该调用IP_TPS_ReleaseAll进行释放
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_Init();
        /// <summary>
        /// 打开流
        /// </summary>
        /// <param name="nPort">视频播放标记，由用户定义，建议从1开始编号</param>
        /// <param name="pParam">解码参数，由netsdkdll中的事件EVENT_RECVVIDEOPARAM和EVENT_RECVAUDIOPARAM取得</param>
        /// <param name="pSize">参数长度</param>
        /// <param name="isAudioParam">是否为音频</param>
        /// <param name="nMaxBufFrameCount">缓冲帧最多帧数，注意不是内存大小</param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_OpenStream(int nPort, IntPtr pParam, int pSize, int isAudioParam, int nMaxBufFrameCount);
        /// <summary>
        /// 在指定hWnd上进行播放，如果hWnd==Inprt.Zero则使用解码方式进行解码回放
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_Play(int nPort, IntPtr hWnd);
        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_PlaySound(int nPort);
        /// <summary>
        /// 传入音频数据
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="pBuf"></param>
        /// <param name="nSize"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_InputAudioData(int nPort, IntPtr pBuf, int nSize, int timestamp);
        /// <summary>
        /// 传入视频数据
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="pBuf"></param>
        /// <param name="nSize"></param>
        /// <param name="isKey"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_InputVideoData(int nPort, IntPtr pBuf, int nSize, int isKey, int timestamp);
        /// <summary>
        /// 抓图，传入目录
        /// </summary>
        /// <param name="Port"></param>
        /// <param name="sDirName"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_CatchPic(int Port, string sDirName);
        /// <summary>
        /// 指定文件名进行抓图
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="sFileName"></param>
        /// <param name="isJpg"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_CatchPicByFileName(int nPort, string sFileName, int isJpg);
        /// <summary>
        /// 停止所有声音
        /// </summary>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_StopSound();
        /// <summary>
        /// 停止播放指定视频
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_Stop(int nPort);
        /// <summary>
        /// 关闭解码器，注意，调用此接口只是关闭解码器，并不会释放相关资源，如果要直接释放相关资源则要调用IP_TPS_DeleteStream
        /// 一般在关闭当前流重新播放新流时，可以使用此接口
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_CloseStream(int nPort);
        /// <summary>
        /// 关闭解码器并释放资源
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_DeleteStream(int nPort);
        /// <summary>
        /// 关闭所有解码器，注意只是关闭解码器，并不释放相关资源
        /// </summary>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_CloseAll();
        /// <summary>
        /// 释放所有资源，此函数，一般只有退出应用程序进程时才调用。
        /// </summary>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_ReleaseAll();
        /// <summary>
        /// 设置解码数据回调函数
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="func"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetDecCallBack(int nPort, fDecCallBackFunction func, IntPtr pUser);
        /// <summary>
        /// 取得当前版本号
        /// </summary>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_GetVersion();
        /// <summary>
        /// 设置状态回调函数
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="func"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetStatusEventCallBack(int nPort, fStatusEventCallBack func, IntPtr pUser);

        /// <summary>
        /// 播放本地文件
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="hWnd"></param>
        /// <param name="filename"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_PlayLocFile(int nPort, IntPtr hWnd, string filename, int fileType);
        /// <summary>
        /// 停止播放本地文件
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_StopPlayLocFile(int nPort);
        /// <summary>
        /// 取当当前播放时间，仅用于回放指定文件时
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="nRetTime"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_GetPlayTime(int nPort, ref int nRetTime);
        /// <summary>
        /// 取得当前文件所有时间，用于回放指定文件时
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="nRetTime"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_GetFileTime(int nPort, ref int nRetTime);
        /// <summary>
        /// 回放文件控制，一般控制快进慢放，暂停等。
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="nAvtionType"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_ControlPlay(int nPort, int nAvtionType, int param);


        /// <summary>
        /// 传入鼠标动作，以处理，注意不要传双击事件
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="nMsgType"></param>
        /// <param name="wp"></param>
        /// <param name="lp"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_InputMouseEvent(int nPort, int nMsgType, Int32 wp, Int32 lp);
        /// <summary>
        /// 设置缩放类型
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetZoomRectOn(int nPort, int nType);
        /// <summary>
        /// 取当前状态，是否启用电子缩放
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_GetZoomRectStatus(int nPort);
        /// <summary>
        /// 切换满屏显示状态
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="bIsFullFill"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetFullFillStatus(int nPort, int bIsFullFill);
        /// <summary>
        /// 获取当前满屏显示状态
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_GetFullFillStatus(int nPort);
        /// <summary>
        /// 设置视频是否播放
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="bIsOn"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetVideoOn(int nPort, int bIsOn);
        /// <summary>
        /// 切换视频，不建议使用此函数。
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SwitchVideo(int from, int to);

        /// <summary>
        /// 调节亮度和对比度
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="nBrightness"></param>
        /// <param name="nContrast"></param>
        /// <param name="bIsEnable"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetContrast(int nPort, int nBrightness, int nContrast, int bIsEnable);
        /// <summary>
        /// 调节显示效果gamma值
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="nGammaValue"></param>
        /// <param name="bIsEnable"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetGamma(int nPort, int nGammaValue, int bIsEnable);



        /// <summary>
        /// 设置显示区域
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="hWnd"></param>
        /// <param name="rcArea"></param>
        /// <returns></returns>
        //[DllImport("DllPlayer.dll")]
        //public static extern int IP_TPS_PlayByArea(int nPort, IntPtr hWnd, RECT rcArea);


        /// <summary>
        /// 设置缓存大小
        /// </summary>
        /// <param name="nPort">通道号</param>
        /// <param name="minMinSecond">缓存最小值，当缓存数据小于此时，播放速度将会减慢，默认为200，以取得更优的流畅性，减少缓冲次数</param>
        /// <param name="maxMaxSecond">缓存最大值，当缓存数据大于此时时，播放速度将会加快，以取得更好的时延，默认500</param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetBufferTick(int nPort, int minMinSecond, int maxMaxSecond);//add 2013-03-21

        /// <summary>
        /// 让播放重新刷新一次图像
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_RefreshSurface(int nPort);//redraw image  add 20130217 by zfuwen

        /// <summary>
        /// 设置音频声音
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="nVolume"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetVolume(int nPort, int nVolume);//add 20130217 play file by zfuwen

        /// <summary>
        /// 取得音频声音
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="pVolume"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_GetVolume(int nPort, ref int pVolume);//add 20130217 play file by zfuwen

        /// <summary>
        /// 设置是否反向播放，此接口暂时不能使用，留作扩展接口
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetPlayDirection(int nPort, int nType);//add 20130217 play file by zfuwen

        /// <summary>
        /// 显示完一帧后，都会调用此回调函数，让用户可以自行进行一些自定义操作
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="fnCallBack"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetDisplayFinishCallBack(int nPort, fDisplayFinishCallBack fnCallBack);//add 20130217 play file by zfuwen

        /// <summary>
        /// 播放声音，调用此方法将不停止正在播放的声音，即允许二路音频同时播放
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_PlaySoundShare(int nPort);//add 20130217 play file by zfuwen,play audio more than one stram
        /// <summary>
        /// 停止 播放声音，调用此方法将只停止指定通道的声音
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_StopSoundShare(int nPort);//add 20130217 play file by zfuwen,stop play one audio stream

        /// <summary>
        /// 打开要回放的文件,配合IP_TPS_StartPlayFile使用
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_OpenFile(ref int nPort, StringBuilder filename);//add 20130217 test file is ok?

        /// <summary>
        /// 开始播放本地文件，配合IP_TPS_OpenFile使用z
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_StartPlayFile(int nPort, IntPtr hWnd);//add 20130217 play file

        /// <summary>
        /// 调节播放结束后的消息通知接受名柄和消息类型
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="hWnd"></param>
        /// <param name="lMsg"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetFileEndMsgWnd(int nPort, int hWnd, int lMsg);//add 20130217 play file

        /// <summary>
        /// 设置回放模式
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="bIsPlayBack"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetPlayBackMode(int nPort, int bIsPlayBack);




        /// <summary>
        /// 设置亮度对比度，饱和度和色调
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="lBrightness"></param>
        /// <param name="lContrast"></param>
        /// <param name="lSaturation"></param>
        /// <param name="lHue"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetColor(int nPort, int lBrightness, int lContrast, int lSaturation, int lHue);//add 20130217 play file by zfuwen

        /// <summary>
        /// 获取亮度对比度，饱和度和色调
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="plBrightness"></param>
        /// <param name="plContrast"></param>
        /// <param name="plSaturation"></param>
        /// <param name="plHue"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_GetColor(int nPort, ref int plBrightness, ref int plContrast, ref  int plSaturation, ref  int plHue);//add 20130217 play file by zfuwen


        /// <summary>
        /// 取当指定通道红缓存了多少帧
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="pRetCount"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_GetBufferCount(int nPort, ref int pRetCount);
        /// <summary>
        /// 清除指定通道的所有缓存
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_ClearBuffer(int nPort);
        /// <summary>
        /// 判断指定缓存的缓存是否已经满了
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="bAutoPlayMaxFrame"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_BufferIsFull(int nPort, int bAutoPlayMaxFrame);//add by zfuwen 20130301
        /// <summary>
        /// 设置要显示的标题
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="TitleMsg"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="bNeedShow"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetShowTitle(int nPort, StringBuilder TitleMsg, int x, int y, int bNeedShow);
        /// <summary>
        /// 设置日志保存目录，此函数仅用于调试
        /// </summary>
        /// <param name="bLogEnable"></param>
        /// <param name="strLogDir"></param>
        /// <param name="bAutoDel"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetLogToFile(int bLogEnable, StringBuilder strLogDir, int bAutoDel);

        /// <summary>
        /// 设置告警区域显示状态
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetMotionDetectConfigOn(int nPort, int value);
        /// <summary>
        /// 设置告警方区域布局及选择状态
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="xBlocks"></param>
        /// <param name="yBlocks"></param>
        /// <param name="pConfigString"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_SetMotionDetectConfig(int nPort, int xBlocks, int yBlocks, StringBuilder pConfigString);
        /// <summary>
        /// 获取当前告警区别的选择状态
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="pConfigString"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_GetMotionDetectConfigString(int nPort, StringBuilder pConfigString);

        /// <summary>
        /// 初始化音频播放器，注意，只播放音频时才使用此方式
        /// </summary>
        /// <param name="plPort"></param>
        /// <param name="audiotype"></param>
        /// <param name="samplerate"></param>
        /// <param name="bitspersample"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_CreateAuidoStreamPlayer(ref int plPort, int audiotype, int samplerate, int bitspersample, int channels);//add by zfuwen 20130221

        /// <summary>
        /// 仅有音频播放时，传入参数数据
        /// </summary>
        /// <param name="lPort"></param>
        /// <param name="pBuffer"></param>
        /// <param name="buflen"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_InputAuidoStreamPlayer(int lPort, IntPtr pBuffer, int buflen);//add by zfuwen 20130221

        /// <summary>
        /// 释放音频播放器
        /// </summary>
        /// <param name="lPort"></param>
        /// <returns></returns>
        [DllImport("DllPlayer.dll")]
        public static extern int IP_TPS_DestroyAuidoStreamPlayer(int lPort);//add by zfuwen 20130221

        /// <summary>
        /// DLL的版本至少应该大于等于此值，才能使用这里定义的所有功能
        /// </summary>
        public static string strNeedDllVersion = "V1.6.0.1";
    }
}

