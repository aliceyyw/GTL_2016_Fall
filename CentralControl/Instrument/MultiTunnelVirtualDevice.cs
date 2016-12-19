using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTLutils;

namespace Instrument
{
    public class MultiTunnelDeviceMessageCreator
    {

        public static String createCmd(String cmd)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("Cmd", cmd);
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.CMD), creator.getDataBytes());
        }

        public static String createSetMode(String s)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("SetType", "Mode");
            creator.addKeyPair("Mode", s);
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.SET), creator.getDataBytes());
        }

        public static String createOKResponse()
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("Result", "OK");
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.RESPONSE), creator.getDataBytes());
        }

        public static String createKongBanReport(bool YouKongBan, String TiaoMaHao)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("ReportType", "YouKongBan");
            String f = "";
            if (YouKongBan) f = "1";
            else f = "0";
            creator.addKeyPair("Flag", f);
            if (YouKongBan)
            {
                creator.addKeyPair("TiaoMaHao", TiaoMaHao);
            }
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.REPORT), creator.getDataBytes());
        }

        public static String createJianCeZhiReport(int b, float[] v)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("ReportType", "Value");
            for (int i = 0; i < v.Length; i++)
            {
                creator.addKeyPair("v" + b, v[i].ToString());
                b++;
            }
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.REPORT), creator.getDataBytes());
        }

        //加样仪信息设定
        public static String createSampleAddingDevice(int MMA_TipIdx,string MMA_TargetIdx,int MMA_ContainerType,int MMA_Volume,string MMA_SampleIdx,
            int MMA_SampleType,int MMA_HeatFlag,float MMA_Temp,int MMA_VibrateFlag,float MMA_VibrateTime)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("SetType", "SampleAddingDevice");
            creator.addKeyPair("MMA_TipIdx", MMA_TipIdx.ToString());
            creator.addKeyPair("MMA_TargetIdx", MMA_TargetIdx.ToString());
            creator.addKeyPair("MMA_ContainerType", MMA_ContainerType.ToString());
            creator.addKeyPair("MMA_Volume", MMA_Volume.ToString());
            creator.addKeyPair("MMA_SampleIdx", MMA_SampleIdx.ToString());
            creator.addKeyPair("MMA_SampleType", MMA_SampleType.ToString());
            creator.addKeyPair("MMA_HeatFlag", MMA_HeatFlag.ToString());
            creator.addKeyPair("MMA_Temp", MMA_Temp.ToString());
            creator.addKeyPair("MMA_VibrateFlag", MMA_VibrateFlag.ToString());
            creator.addKeyPair("MMA_VibrateTime", MMA_VibrateTime.ToString());
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.SET), creator.getDataBytes());
        }

        //酶标仪信息设定
        public static String createEliasa(int MMA_TestType,int MMA_LightType,int MMA_WaveLength,int MMA_OrificeType,
               int MMA_MeasureArea,int MMA_Time,int MMA_IntegralTime,int MMA_TestRowIndex,int MMA_TestColumnIndex,
            float MMA_WaveLengthUp,float MMA_WaveLengthDown,int MMA_MeasureTime)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("SetType", "Eliasa");
            creator.addKeyPair("", "");
            creator.addKeyPair("", "");
            creator.addKeyPair("MMA_TestType", MMA_TestType.ToString());
            creator.addKeyPair("MMA_LightType", MMA_LightType.ToString());
            creator.addKeyPair("MMA_WaveLength", MMA_WaveLength.ToString());
            creator.addKeyPair("MMA_OrificeType", MMA_OrificeType.ToString());
            creator.addKeyPair("MMA_MeasureArea", MMA_MeasureArea.ToString());
            creator.addKeyPair("MMA_IntegralTime", MMA_IntegralTime.ToString());
            creator.addKeyPair("MMA_TestRowIndex", MMA_TestRowIndex.ToString());
            creator.addKeyPair("MMA_TestColumnIndex", MMA_TestColumnIndex.ToString());
            creator.addKeyPair("MMA_WaveLengthUp", MMA_WaveLengthUp.ToString());
            creator.addKeyPair("MMA_WaveLengthDown", MMA_WaveLengthDown.ToString());
            creator.addKeyPair("MMA_MeasureTime", MMA_MeasureTime.ToString());
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.SET), creator.getDataBytes());
        }
    }

    public class MultiTunnelVirtualDevice : BaseVirtualDevice
    {
        private float[][] MMA_CurrentValues = null;
        private float[][] MMA_PreValues = null;

        //上位机发给仪器
        private String MMA_currentBarCode = null;
        private String MMA_preBarCode = null;
        //加样仪
        private int MMA_TipIdx = 0; //吸头索引
        private string MMA_TargetIdx = "1-5"; //值范围是0--27，数据格式1,2或1-5等，用“，”或“-”区分，“-”表示连续。
        private int MMA_ContainerType = 0; //容器类型
        private int MMA_Volume = 0; //加样体积
        private string MMA_SampleIdx = "";//加样孔板的索引，96孔板，8孔一组，10个孔板，共120组,范围0-119
        private int MMA_SampleType = 0; //标准样品0/测量样品1/空白样品2
        private int MMA_HeatFlag = 0; //0不需要加热/1需要加热
        private float MMA_Temp;
        private int MMA_VibrateFlag;//振动标志
        private float MMA_VibrateTime; //振动时间
        //酶标仪
        //public enum MMA_TestMethod { OD, Flu, Che };  //检测方式，吸光度检测/y荧光检测/化学发光检测
        public int MMA_TestMethod = 0;  //OD, Flu, Che 检测方式
        private int MMA_TestMode = 0;  //光学模式OM，MC单色仪模式，Flc荧光接插件模式
        private int MMA_TestType = 0; //终点0/动态1
        private int MMA_LightType; //光波类型
        private int MMA_WaveLength; //光波波长
        private int MMA_OrificeType = 0; //孔板类型，0 96孔板/1 48孔板
        private int MMA_MeasureArea; //检测区域
        private int MMA_Time; //时间
        private int MMA_IntegralTime;  //积分时间
        public static int MMA_TestRowIndex = 8; //检测行数
        public static int MMA_TestColumnIndex = 12;  //检测列数
        private float MMA_WaveLengthUp;  //波长上限
        private float MMA_WaveLengthDown;  //波长下限
        private int MMA_MeasureTime;  //处理时间

        //仪器发给上位机
        //加样仪
        private int MMA_RestTip;  //剩余吸头数
        private int[] MMA_PlateFlag;//有无放孔板*10
        private float[] MMA_PlateTemp; //当前温度*10
        //酶标仪
        private float[] MMA_ODValue;  //OD值*96
        private float[] MMA_FluCount;  //荧光检测参数*96
        private float[] MMA_CheCount;  //化学发光检测参数*96
        private float MMA_Wave;  //波长范围 
        private float MMA_CurrentTemp; //当前温度
        private bool MMA_PlateDetect = false;  //有无放孔板 true表示有 false表示无
        private string MMA_InBarCode = "";
        private string MMA_OutBarCode = "";
        private bool MMA_SendBarCodeFlag = true; //是否需要发送条码
        private float[][] MMA_DetectValues = null; //当前检测参数

        public static int stringToJianCeMoShi(String mode)
        {
            if ("OD".Equals(mode)) return 0;
            if ("Flu".Equals(mode)) return 1;
            if ("Che".Equals(mode)) return 2;
            return 0;
        }

        public static String TestMethodToString(int m)
        {
            switch (m)
            {
                case 0:
                    return "OD";
                case 1:
                    return "Flu";
                case 2:
                    return "Che";
            }
            return "OD";
        }

        public void send_TestMethod()
        {
            String msg = MultiTunnelDeviceMessageCreator.createSetMode(TestMethodToString(MMA_TestMethod));
            SendMsg(msg);
        }

        public String getTiaoMaHao()
        {
            return MMA_preBarCode;
        }

        public float[][] getDetectValues()
        {
            float[][] res = null;
            if (MMA_PreValues == null) return res;
            lock (MMA_PreValues)
            {
                res = new float[MultiTunnelVirtualDevice.MMA_TestRowIndex][];
                for (int i = 0; i < MultiTunnelVirtualDevice.MMA_TestRowIndex; i++)
                {
                    res[i] = new float[MultiTunnelVirtualDevice.MMA_TestColumnIndex];
                    for (int j = 0; j < MultiTunnelVirtualDevice.MMA_TestColumnIndex; j++)
                    {
                        res[i][j] = MMA_PreValues[i][j];
                    }
                }
            }
            return res;
        }

        public float getSingeDetectValue(int i, int j)
        {
            if (i < 0 || i >= MMA_TestRowIndex) return 0;
            if (j < 0 || j >= MMA_TestColumnIndex) return 0;
            if (MMA_PreValues == null) return 0;
            lock (MMA_PreValues)
            {
                return MMA_PreValues[i][j];
            }
        }


        public bool YouKongBan
        {
            get
            {
                return this.MMA_PlateDetect;
            }
            set
            {
                this.MMA_PlateDetect = value;
            }
        }


        public int ChuLiShiJian
        {
            get
            {
                return this.MMA_MeasureTime;
            }
            set
            {
                this.MMA_MeasureTime = value;
            }
        }


        public float DangQiangWenWu
        {
            get
            {
                return this.MMA_Temp;
            }
            set
            {
                this.MMA_Temp = value;
            }
        }


        public float BoChangXiaXian
        {
            get
            {
                return this.MMA_WaveLengthUp;
            }
            set
            {
                this.MMA_WaveLengthUp = value;
            }
        }


        public float BoChangShangXian
        {
            get
            {
                return this.MMA_WaveLengthDown;
            }
            set
            {
                this.MMA_WaveLengthDown = value;
            }
        }

        //public override void decodeCmdMessage(ModbusMessage msg)
        //{
        //    String cmd = (String)msg.Data["Cmd"];

        //}

        public override void decodeSetMessage(ModbusMessage msg)
        {
            String setType = (String)msg.Data["SetType"];

        }

        public void send_cmd(String cmd)
        {
            String msg = MultiTunnelDeviceMessageCreator.createCmd(cmd);
            SendMsg(msg);
        }

        public override void decodeReportMessage(ModbusMessage msg)
        {
            String reportType = (String)msg.Data["ReportType"];

            if ("State".Equals(reportType))
            {
                MMA_RestTip = int.Parse((string)msg.Data["MMA_RestTip"]);
                string str = (string)msg.Data["MMA_PlateFlag"];
                for (int i = 0; i < 10; i++)
                {
                    MMA_PlateFlag[i] = str[i];
                }
                for (int j = 0; j < 10; j++)
                {
                    MMA_PlateTemp[j] = float.Parse((string)msg.Data["PlateTemp"] + j.ToString());
                }
            }
            if ("ODValue".Equals(reportType))
            {
                for(int i=0;i<96;i++){
                    MMA_ODValue[i]=float.Parse((string)msg.Data["OD"+i.ToString()]); // OD0 ~OD95, i为下标
                }
            }
            if("FluValue".Equals(reportType))
            {
                for(int i=0;i<96;i++){
                    MMA_FluCount[i]=float.Parse((string)msg.Data["FLU"+i.ToString()]); // FLU0 ~FLU95, i为下标
                }
            }
            if("CheValue".Equals(reportType))
            {
                 for(int i=0;i<96;i++){
                      MMA_CheCount[i]=float.Parse((string)msg.Data["CHE"+i.ToString()]);// CHE0 ~CHE95, i为下标
                 }
            }
            if("Parameter".Equals(reportType))
            {
                MMA_Wave = float.Parse((string)msg.Data["MMA_Wave"]);
                MMA_CurrentTemp = float.Parse((string)msg.Data["MMA_CurrentTemp"]);
                int platedetect = int.Parse((string)msg.Data["MMA_PlateDetect"]);
                if(platedetect==0) MMA_PlateDetect=false;
                else MMA_PlateDetect=true;

            }
            if("BarCode".Equals(reportType)){
                int ifbar = int.Parse((string)msg.Data["MMA_SendBarCodeFlag"]);
                if(ifbar==0) MMA_SendBarCodeFlag=false;
                else{
                    MMA_SendBarCodeFlag=true;
                    MMA_InBarCode = (string)msg.Data["MMA_InBarCode"];
                    MMA_OutBarCode = (string)msg.Data["MMA_OutBarCode"];
                }
            }
           
                
                if ("YouKongBan".Equals(reportType))
            {
                int f = int.Parse((String)msg.Data["Flag"]);
                if (f > 0) MMA_PlateDetect = true;
                else MMA_PlateDetect = false;
                if (MMA_PlateDetect)
                {
                    MMA_currentBarCode = (String)msg.Data["TiaoMaHao"];
                }
                else
                {
                    send_cmd("Next");
                }
            }
            if ("Value".Equals(reportType))
            {
                String key;
                bool hasFinish = false;
                if (MMA_CurrentValues == null)
                {
                    MMA_CurrentValues = new float[MultiTunnelVirtualDevice.MMA_TestRowIndex][];
                    for (int i = 0; i < MultiTunnelVirtualDevice.MMA_TestRowIndex; i++)
                    {
                        MMA_CurrentValues[i] = new float[MultiTunnelVirtualDevice.MMA_TestColumnIndex];
                    }
                }
                lock (MMA_CurrentValues)
                {
                    foreach (Object ob in msg.Data.Keys)
                    {
                        key = (String)ob;
                        if (key.StartsWith("v"))
                        {
                            int index = int.Parse(key.Substring(1, key.Length - 1));
                            int i = index / MultiTunnelVirtualDevice.MMA_TestColumnIndex;
                            int j = index % MultiTunnelVirtualDevice.MMA_TestColumnIndex;
                            if (index == MultiTunnelVirtualDevice.MMA_TestColumnIndex * MultiTunnelVirtualDevice.MMA_TestRowIndex - 1) hasFinish = true;
                            float v = float.Parse((String)msg.Data[key]);
                            MMA_CurrentValues[i][j] = v;
                        }
                    }
                }
                if (hasFinish)
                {
                    if (MMA_PreValues == null)
                    {
                        MMA_PreValues = MMA_CurrentValues;
                        MMA_preBarCode = MMA_currentBarCode;
                    }
                    else
                    {
                        lock (MMA_PreValues)
                        {
                            MMA_PreValues = MMA_CurrentValues;
                            MMA_preBarCode = MMA_currentBarCode;
                        }
                    }
                    lock (MMA_CurrentValues)
                    {
                        MMA_CurrentValues = null;
                        MMA_currentBarCode = null;
                    }
                }
                if ("MMA_CHEMLIGHT".Equals(reportType))
                {
                    //插入数据库准备
                    ArrayList list = new ArrayList();
                    list.Add(this.Code.Substring(0, 8)); //Device_Id
                    list.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //CurrentTime
                    if (msg.Data.ContainsKey("CREATER_ID")) // CREATER_ID, can be null
                        list.Add(msg.Data["CREATER_ID"]);
                    else
                        list.Add("NULL");
                    if (msg.Data.ContainsKey("TASK_ID"))
                        list.Add(msg.Data["TASK_ID"]);
                    else
                        list.Add("NULL");  //task id
                    if (msg.Data.ContainsKey("FLOW_ID"))
                        list.Add(msg.Data["FLOW_ID"]);
                    else
                        list.Add("NULL");  //flow id
                    if (msg.Data.Contains("MMA_currentBarCode"))
                    {
                        MMA_currentBarCode = msg.Data["MMA_currentBarCode"].ToString();
                        list.Add(MMA_currentBarCode);
                    }
                    else list.Add("123456789");  //MMA_currentBarCode
                    if (msg.Data.Contains("MMA_X"))
                    {
                        list.Add(msg.Data["MMA_X"].ToString());
                    }
                    else list.Add("2.0");  //x坐标
                    if (msg.Data.ContainsKey("MMA_Y"))
                        list.Add(msg.Data["MMA_Y"]);
                    else
                        list.Add("2.0");  //y坐标
                    if (msg.Data.ContainsKey("MMA_ChemLight"))
                        list.Add(msg.Data["MMA_ChemLight"]);
                    else
                        list.Add("2.0");  //chemlight
                    if (msg.Data.Contains("Device_Time")) 
                        list.Add(msg.Data["Device_Time"]);
                    else 
                        list.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));  //devicetime
                    if (msg.Data.ContainsKey("MMA_Temp"))
                    {
                        MMA_Temp = float.Parse(msg.Data["MMA_Temp"].ToString());
                        list.Add(MMA_Temp);
                    }
                    else
                        list.Add("2.0");  //温度
                    if (msg.Data.ContainsKey("CREATE_DATE"))
                        list.Add(msg.Data["CREATE_DATE"]);
                    else
                        list.Add("NULL");  //creater_date
                    Database.insertTable("MMA_CHEMLIGHT", list);
                }
                if ("MMA_LUMIN".Equals(reportType))
                {
                    //插入数据库准备
                    ArrayList list = new ArrayList();
                    list.Add(this.Code.Substring(0, 8)); //Device_Id
                    list.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //CurrentTime
                    if (msg.Data.ContainsKey("CREATER_ID")) // CREATER_ID, can be null
                        list.Add(msg.Data["CREATER_ID"]);
                    else
                        list.Add("NULL");
                    if (msg.Data.ContainsKey("TASK_ID"))
                        list.Add(msg.Data["TASK_ID"]);
                    else
                        list.Add("NULL");  //task id
                    if (msg.Data.ContainsKey("FLOW_ID"))
                        list.Add(msg.Data["FLOW_ID"]);
                    else
                        list.Add("NULL");  //flow id
                    if (msg.Data.Contains("MMA_currentBarCode"))
                    {
                        MMA_currentBarCode = msg.Data["MMA_currentBarCode"].ToString();
                        list.Add(MMA_currentBarCode);
                    }
                    else list.Add("123456789");  //MMA_currentBarCode
                    if (msg.Data.Contains("MMA_X"))
                    {
                        list.Add(msg.Data["MMA_X"].ToString());
                    }
                    else list.Add("2.0");  //x坐标
                    if (msg.Data.ContainsKey("MMA_Y"))
                        list.Add(msg.Data["MMA_Y"]);
                    else
                        list.Add("2.0");  //y坐标
                    if (msg.Data.ContainsKey("MMA_Lumin"))
                        list.Add(msg.Data["MMA_Lumin"]);
                    else
                        list.Add("2.0");  //lumin
                    if (msg.Data.Contains("Device_Time"))
                        list.Add(msg.Data["Device_Time"]);
                    else
                        list.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));  //devicetime
                    if (msg.Data.ContainsKey("MMA_Temp"))
                    {
                        MMA_Temp = float.Parse(msg.Data["MMA_Temp"].ToString());
                        list.Add(MMA_Temp);
                    }
                    else
                        list.Add("2.0");  //温度
                    if (msg.Data.ContainsKey("CREATE_DATE"))
                        list.Add(msg.Data["CREATE_DATE"]);
                    else
                        list.Add("NULL");  //creater_date
                    Database.insertTable("MMA_LUMIN", list);
                }
                if ("MMA_OD".Equals(reportType))
                {
                    //插入数据库准备
                    ArrayList list = new ArrayList();
                    list.Add(this.Code.Substring(0, 8)); //Device_Id
                    list.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //CurrentTime
                    if (msg.Data.ContainsKey("CREATER_ID")) // CREATER_ID, can be null
                        list.Add(msg.Data["CREATER_ID"]);
                    else
                        list.Add("NULL");
                    if (msg.Data.ContainsKey("TASK_ID"))
                        list.Add(msg.Data["TASK_ID"]);
                    else
                        list.Add("NULL");  //task id
                    if (msg.Data.ContainsKey("FLOW_ID"))
                        list.Add(msg.Data["FLOW_ID"]);
                    else
                        list.Add("NULL");  //flow id
                    if (msg.Data.Contains("MMA_currentBarCode"))
                    {
                        MMA_currentBarCode = msg.Data["MMA_currentBarCode"].ToString();
                        list.Add(MMA_currentBarCode);
                    }
                    else list.Add("123456789");  //MMA_currentBarCode
                    if (msg.Data.Contains("MMA_X"))
                    {
                        list.Add(msg.Data["MMA_X"].ToString());
                    }
                    else list.Add("2.0");  //x坐标
                    if (msg.Data.ContainsKey("MMA_Y"))
                        list.Add(msg.Data["MMA_Y"]);
                    else
                        list.Add("2.0");  //y坐标
                    if (msg.Data.ContainsKey("MMA_OD"))
                        list.Add(msg.Data["MMA_OD"]);
                    else
                        list.Add("2.0");  //od
                    if (msg.Data.Contains("Device_Time"))
                        list.Add(msg.Data["Device_Time"]);
                    else
                        list.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));  //devicetime
                    if (msg.Data.ContainsKey("MMA_Temp"))
                    {
                        MMA_Temp = float.Parse(msg.Data["MMA_Temp"].ToString());
                        list.Add(MMA_Temp);
                    }
                    else
                        list.Add("2.0");  //温度
                    if (msg.Data.ContainsKey("CREATE_DATE"))
                        list.Add(msg.Data["CREATE_DATE"]);
                    else
                        list.Add("NULL");  //creater_date
                    Database.insertTable("MMA_OD", list);
                }
            }
        }
    }
}
