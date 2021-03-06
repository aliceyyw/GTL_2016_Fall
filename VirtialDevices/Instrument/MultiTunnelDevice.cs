﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using DeviceUtils;

namespace Instrument
{


    public class MultiTunnelDeviceMessageCreator
    {

        public static String createOKResponse()
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("Result", "OK");
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.RESPONSE), creator.getDataBytes());
        }

        public static String createPlateReport(bool YouKongBan, String TiaoMaHao)
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

    }

    public class MultiTunnelDevice : BaseVirtualDevice
    {
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
        public int MMA_TestMode = 0;  //光学模式OM，MC单色仪模式，Flc荧光接插件模式
        public int MMA_TestType = 0; //终点0/动态1
        public int MMA_LightType; //光波类型
        public int MMA_WaveLength; //光波波长
        public int MMA_OrificeType = 0; //孔板类型，0 96孔板/1 48孔板
        public int MMA_MeasureArea; //检测区域
        public int MMA_Time; //时间
        public int MMA_IntegralTime;  //积分时间
        public static int MMA_TestRowIndex = 8; //检测行数
        public static int MMA_TestColumnIndex = 12;  //检测列数
        public float MMA_WaveLengthUp;  //波长上限
        public float MMA_WaveLengthDown;  //波长下限
        public int MMA_MeasureTime;  //处理时间

        //仪器发给上位机
        //加样仪
        public int MMA_RestTip;  //剩余吸头数
        public int[] MMA_PlateFlag = new int[10];//有无放孔板*10
        public float[] MMA_PlateTemp = new float[10]{0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f};//度*10
        //酶标仪
        public float[] MMA_ODValue = new float[96];  //OD值*96
        public float[] MMA_FluCount = new float[96];  //荧光检测参数*96
        public float[] MMA_CheCount = new float[96];  //化学发光检测参数*96
        public float MMA_Wave;  //波长范围 
        public float MMA_CurrentTemp; //当前温度
        public bool MMA_PlateDetect = false;  //有无放孔板 true表示有 false表示无
        public string MMA_InBarCode = "";
       public string MMA_OutBarCode = "";
        public bool MMA_SendBarCodeFlag = true; //是否需要发送条码
        public float[][] MMA_DetectValues = null; //当前检测参数

        private System.Timers.Timer MMA_Timer = null;

        public static int stringToDetectMethod(String mode)
        {
            if ("OD".Equals(mode)) return 0; 
            if ("Flu".Equals(mode)) return 1;
            if ("Che".Equals(mode)) return 2;
            return 0;
        }

        public static String detectMethodToString(int m)
        {
            switch (m)
            {
                case 0:   //吸光
                    return "OD";
                case 1:  //荧光
                    return "Flu";
                case 2:  //化学发光
                    return "Che";
            }
            return "OD";
        }


        public void setDetectValues(float[][] v)
        {
            if (v.Length != MMA_TestRowIndex) return;
            for (int i = 0; i < v.Length; i++)
            {
                if (v[i].Length != MMA_TestColumnIndex) return;
            }
            MMA_DetectValues = new float[MMA_TestRowIndex][];
            for (int i = 0; i < MMA_TestRowIndex; i++)
            {
                MMA_DetectValues[i] = new float[MMA_TestColumnIndex];
            }
            for (int i = 0; i < MMA_TestRowIndex; i++)
            {
                for (int j = 0; j < MMA_TestColumnIndex; j++)
                {
                    MMA_DetectValues[i][j] = v[i][j];
                }
            }
        }


        public void stateSendReport()
        {
            Hashtable ht = new Hashtable();
            ht.Add("ReportType", "State");
            ht.Add("MMA_RestTip", MMA_RestTip.ToString());  //剩余吸头
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                str.Append(MMA_PlateFlag[i].ToString());    // 有无放孔板，长度为10的字符串
            }
            ht.Add("MMA_PlateFlag", str.ToString());
            for (int j = 0; j < 10; j++)
            {
                ht.Add("PlateTemp" + j.ToString(), MMA_PlateTemp[j].ToString());  //当前温度，10个浮点数，index0~9
            }
            SendModBusMsg(ModbusMessage.MessageType.REPORT, ht);
        }

        public void valueSendReport(string mode)  //mode 为  OD  FLU  CHE 选一
        {
            Hashtable ht = new Hashtable();
            mode = mode.ToUpper();
            switch (mode)
            {
                case "OD":
                    ht.Add("ReportType", "ODValue");
                    for (int i = 0; i < 96; i++)
                    {
                        ht.Add("OD"+i.ToString(), MMA_ODValue[i].ToString());   // OD0 ~OD95, i为下标
                    }
                        break;
                case "FLU":
                        ht.Add("ReportType", "FluValue");
                        for (int j = 0; j < 96; j++)
                        {
                            ht.Add("FLU" + j.ToString(), MMA_FluCount[j].ToString());   // FLU0 ~FLU95, J为下标
                        }
                        break;
                case "CHE":
                        ht.Add("ReportType", "CheValue");
                        for (int k = 0; k < 96; k++)                        
                        {
                            ht.Add("CHE" + k.ToString(), MMA_CheCount[k].ToString()); //CHE0~CHE95,k为下标
                        }
                        break;
            }
            SendModBusMsg(ModbusMessage.MessageType.REPORT, ht);
        }

        public void parameterSendReport()
        {
            Hashtable ht = new Hashtable();
            ht.Add("ReportType", "Parameter");
            ht.Add("MMA_Wave", MMA_Wave.ToString());    //波长范围
            ht.Add("MMA_CurrentTemp", MMA_CurrentTemp.ToString()); //当前温度
            ht.Add("MMA_PlateDetect", (MMA_PlateDetect ? "1" : "0"));  //true=1 false=0
            SendModBusMsg(ModbusMessage.MessageType.REPORT, ht);
            
        }

        public void barcodeSendReport()
        {
            Hashtable ht = new Hashtable();
            ht.Add("ReportType", "Barcode");
            ht.Add("MMA_SendBarCodeFlag", (MMA_SendBarCodeFlag ? "1" : "0"));  //是否需要发送条码，true1 false0
            if (MMA_SendBarCodeFlag)
            {
                ht.Add("MMA_InBarCode", MMA_InBarCode);
                ht.Add("MMA_OutBarCode", MMA_OutBarCode);
            }
            SendModBusMsg(ModbusMessage.MessageType.REPORT, ht);
        }

        private void chuLiTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MMA_Timer.Stop();
            if (MMA_PlateDetect)
            {
                int b = 0;
                String s;
                for (int i = 0; i < MMA_TestRowIndex; i++)
                {
                    s = MultiTunnelDeviceMessageCreator.createJianCeZhiReport(b, MMA_DetectValues[i]);
                    SendMsg(s);
                    b += MMA_TestColumnIndex;
                }
                //s = MultiTunnelDeviceMessageCreator.createKongBanReport(youKongBan,currentTiaoMaHao);
                //SendMsg(s);
                MMA_PlateDetect = false;
                MMA_currentBarCode = null;
            }
            MMA_Timer = null;
        }

        private void startTimer()
        {
            if (MMA_PlateDetect) MMA_currentBarCode = BarCodeGenerator.generateBarCode();
            String s = MultiTunnelDeviceMessageCreator.createPlateReport(MMA_PlateDetect, MMA_currentBarCode);
            SendMsg(s);
            if (MMA_PlateDetect)
            {
                if (MMA_Timer != null) MMA_Timer.Stop();
                MMA_Timer = new System.Timers.Timer();
                MMA_Timer.Interval = MMA_MeasureTime * 1000;
                MMA_Timer.Elapsed += new System.Timers.ElapsedEventHandler(chuLiTimer_Elapsed);
                MMA_Timer.Start();
            }
        }

        public void setSingleDetectValue(int i, int j, float v)
        {
            if (i < 0 || i >= MMA_TestRowIndex) return;
            if (j < 0 || j >= MMA_TestColumnIndex) return;
            if (MMA_DetectValues == null) return;
            MMA_DetectValues[i][j] = v;
        }

        public float[][] getDetectValues()
        {
            return MMA_DetectValues;
        }

        public float getSingeDetectValue(int i, int j)
        {
            if (i < 0 || i >= MMA_TestRowIndex) return 0;
            if (j < 0 || j >= MMA_TestColumnIndex) return 0;
            if (MMA_DetectValues == null) return 0;
            return MMA_DetectValues[i][j];
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

        //private void decodeCmdMessage(ModbusMessage msg)
        //{
        //    String cmd = (String)msg.Data["Cmd"];
        //    if ("Start".Equals(cmd))
        //    {
        //        startTimer();
        //    }
        //    if ("Next".Equals(cmd))
        //    {
        //        MMA_PlateDetect = true;
        //        startTimer();
        //    }
        //}

        public override void decodeSetMessage(ModbusMessage msg)
        {
            String setType = (String)msg.Data["SetType"];
            if ("Mode".Equals(setType))
            {
                String mode = (String)msg.Data["Mode"];
                MMA_TestMode = stringToDetectMethod(mode);
            }
            //加样仪
            if ("SampleAddingDevice".Equals(setType))
            {
                this.MMA_TipIdx = Convert.ToInt32((String)msg.Data["MMA_TipIdx"]);
                this.MMA_TargetIdx = (String)msg.Data["MMA_TargetIdx"];
                this.MMA_ContainerType = Convert.ToInt32((String)msg.Data["MMA_ContainerType"]);
                this.MMA_Volume = Convert.ToInt32((String)msg.Data["MMA_Volume"]);
                this.MMA_SampleIdx = (String)msg.Data["MMA_SampleIdx"];
                this.MMA_SampleType = Convert.ToInt32((String)msg.Data["MMA_SampleType"]);
                this.MMA_HeatFlag = Convert.ToInt32((String)msg.Data["MMA_HeatFlag"]);
                this.MMA_Temp = Convert.ToInt32((String)msg.Data["MMA_Temp"]);
                this.MMA_VibrateFlag = Convert.ToInt32((String)msg.Data["MMA_VibrateFlag"]);
                this.MMA_VibrateTime = float.Parse((String)msg.Data["MMA_VibrateTime"]);
            }
            //酶标仪
            if ("Eliasa".Equals(setType))
            {
                this.MMA_TestMethod = Convert.ToInt32((String)msg.Data["MMA_TestMethod"]);
                this.MMA_TestMode = Convert.ToInt32((String)msg.Data["MMA_TestMode"]);
                this.MMA_TestType = Convert.ToInt32((String)msg.Data["MMA_TestType"]);
                this.MMA_LightType = Convert.ToInt32((String)msg.Data["MMA_LightType"]);
                this.MMA_WaveLength = Convert.ToInt32((String)msg.Data["MMA_WaveLength"]);
                this.MMA_OrificeType = Convert.ToInt32((String)msg.Data["MMA_OrificeType"]);
                this.MMA_MeasureArea = Convert.ToInt32((String)msg.Data["MMA_MeasureArea"]);
                this.MMA_Time = Convert.ToInt32((String)msg.Data["MMA_Time"]);
                this.MMA_IntegralTime = Convert.ToInt32((String)msg.Data["MMA_IntegralTime"]);
                MultiTunnelDevice.MMA_TestRowIndex = Convert.ToInt32((String)msg.Data["MMA_TestRowIndex"]);
                MultiTunnelDevice.MMA_TestColumnIndex = Convert.ToInt32((String)msg.Data["MMA_TestColumnIndex"]);
                this.MMA_WaveLengthUp = float.Parse((String)msg.Data["MMA_WaveLengthUp"]);
                this.MMA_WaveLengthDown = float.Parse((String)msg.Data["MMA_WaveLengthDown"]);
                this.MMA_MeasureTime = Convert.ToInt32((String)msg.Data["MMA_MeasureTime"]);
            }
        }

       

    }
}
