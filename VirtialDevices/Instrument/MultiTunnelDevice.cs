using System;
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
        public enum MMA_TestMethod { OD, Flu, Che };  //检测方式
        private MMA_TestMethod MMA_TestMode = MMA_TestMethod.OD;  //检测模式
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
        private bool[] MMA_PlateFlag;//有无放孔板*10
        private float[] MMA_PlateTemp; //当前温度*10
        //酶标仪
        private float[] MMA_ODValue;  //OD值*96
        private float[] MMA_FluCount;  //荧光检测参数*96
        private float[] MMA_CheCount;  //化学发光检测参数*96
        private float MMA_Wave;  //波长范围 
        private float MDF_CurrentTemp; //当前温度
        private bool MMA_PlateDetect = false;  //有无放孔板 true表示有 false表示无
        private string MMA_InBarCode = "";
        private string MMA_OutBarCode = "";
        private bool MMA_SendBarCodeFlag = true; //是否需要发送条码
        private float[][] MMA_DetectValues = null; //当前检测参数

        private System.Timers.Timer MMA_Timer = null;

        public static MMA_TestMethod stringToJianCeMoShi(String mode)
        {
            if ("OD".Equals(mode)) return MMA_TestMethod.OD;
            if ("YG".Equals(mode)) return MMA_TestMethod.Flu;
            if ("HXFG".Equals(mode)) return MMA_TestMethod.Che;
            return MMA_TestMethod.OD;
        }

        public static String jianCeMoShiToString(MMA_TestMethod m)
        {
            switch (m)
            {
                case MMA_TestMethod.OD:
                    return "OD";
                case MMA_TestMethod.Flu:
                    return "YG";
                case MMA_TestMethod.Che:
                    return "HXFG";
            }
            return "OD";
        }

        public MMA_TestMethod MoShi
        {
            get
            {
                return this.MMA_TestMode;
            }
            set
            {
                this.MMA_TestMode = value;
            }
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
            String s = MultiTunnelDeviceMessageCreator.createKongBanReport(MMA_PlateDetect, MMA_currentBarCode);
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

        private void decodeSetMessage(ModbusMessage msg)
        {
            String setType = (String)msg.Data["SetType"];
            if ("Mode".Equals(setType))
            {
                String mode = (String)msg.Data["Mode"];
                MMA_TestMode = stringToJianCeMoShi(mode);
            }
        }

       

    }
}
