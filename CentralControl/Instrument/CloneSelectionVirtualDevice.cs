using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTLutils;

namespace Instrument
{
    public class CloneSelectionDeviceMessageCreator
    {
        //孔板选择
        public static String createSetKongBanXuanZe(String arg1, String arg2, String arg3, String arg4, String arg5, String arg6, String arg7, String arg8)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("SetType", "KongBanXuanZe");
            creator.addKeyPair("SCP_PickStopTime", arg1);
            creator.addKeyPair("SCP_ShockCount", arg2);
            creator.addKeyPair("SCP_InoStopTime", arg3);
            creator.addKeyPair("SCP_LightType", arg4);
            creator.addKeyPair("SCP_DishType", arg5);
            creator.addKeyPair("SCP_PlateType", arg6);
            creator.addKeyPair("SCP_ProbeMethod", arg7);
            creator.addKeyPair("SCP_CloneNum", arg8);
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.SET), creator.getDataBytes());
        }
       // 周长面积比
        public static String createSetLowAndUpp(String Lower, String Upper)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("SetType", "ZhouChangMianJiBi");
            creator.addKeyPair("SCP_MaxPARate", Upper);
            creator.addKeyPair("SCP_MinPARate", Lower);
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.SET), creator.getDataBytes());
        }

        public static String createSetMianJiLowAndUpp(String Lower, String Upper)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("SetType", "MianJiShaiXuan");
            creator.addKeyPair("SCP_SizeMax", Upper);
            creator.addKeyPair("SCP_SizeMin", Lower);
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.SET), creator.getDataBytes());
        }

        public static String createSetChangDuanJing(String arg1, String arg2, String arg3, String arg4, String arg5, String arg6)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("SetType", "ChangDuanJingShaiXuan");
            creator.addKeyPair("SCP_MaxLength", arg1);
            creator.addKeyPair("SCP_MinLength", arg2);
            creator.addKeyPair("SCP_MaxShort", arg3);
            creator.addKeyPair("SCP_MinShort", arg4);
            creator.addKeyPair("SCP_MaxRate", arg5);
            creator.addKeyPair("SCP_MinRate", arg6);
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.SET), creator.getDataBytes());
        }

        public static String createSetColor(String arg1, String arg2, String arg3, String arg4, String arg5, String arg6)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("SetType", "SeDuPingJunZhi");
            creator.addKeyPair("SCP_RedMax", arg1);
            creator.addKeyPair("SCP_RedMin", arg2);
            creator.addKeyPair("SCP_GreenMax", arg3);
            creator.addKeyPair("SCP_GreenMin", arg4);
            creator.addKeyPair("SCP_BlueMax", arg5);
            creator.addKeyPair("SCP_BlueMin", arg6);
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.SET), creator.getDataBytes());
        }
        public static String createSetMieJun(String arg1, String arg2, String arg3, String arg4, String arg5)
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("SetType", "MieJunHeQingXi");
            creator.addKeyPair("SCP_HeatTime", arg1);
            creator.addKeyPair("SCP_FlushNo", arg2);
            creator.addKeyPair("SCP_CoolTime", arg3);
            creator.addKeyPair("SCP_FlushTime", arg4);
            creator.addKeyPair("SCP_ExhaustTime", arg5);
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.SET), creator.getDataBytes());
        }
        
    }
    public class CloneSelectionVirtualDevice : BaseVirtualDevice
    {
        //上位机发向仪器
        //平皿和孔板选择
        public int SCP_LightType = 0;
        public int SCP_DishType = 1;
        public int SCP_PlateType = 2;
        public int SCP_ProbeMethod = 0;
        public int SCP_NeedleFlag = 0;
        public int SCP_NeedleNum = 0;
        public int SCP_DishNeedleFlag = 0;
        public int SCP_CloneNum = 96;
        public int SCP_PlateFlag = 0;
        public int SCP_SpaceFlag = 0;

        //过程设置
        public float SCP_PickStopTime = 1;
        public float SCP_InoStopTime = 1;
        public int SCP_ShockCount = 10;

        //灭菌与清洗
        public UInt32 SCP_HeatTime = 10;
        public UInt32 SCP_CoolTime = 0;
        public UInt32 SCP_ExhaustTime = 0;
        public UInt32 SCP_FlushTime = 0;
        public UInt32 SCP_FlushNo = 0;

        //筛选过程
        public int SCP_CircleLoc = 0;
        public int SCP_X = 403;
        public int SCP_Y = 281;
        public int SCP_Radius = 245;
        public int SCP_MatrixLoc = 1;
        public int SCP_CenterX = 400;
        public int SCP_CenterY = 300;
        public int SCP_Length = 320;
        public int SCP_Width = 220;
        /*public int SCP_Calibrate = 0;
        public int SCP_OriginPoint = 0;
        public int SCP_ControlPoint = 0;*/

        //筛选条件
        public int SCP_ColorFlag = 1;
        public int SCP_RedMin = 0;
        public int SCP_RedMax = 255;
        public int SCP_GreenMin = 0;
        public int SCP_GreenMax = 255;
        public int SCP_BlueMin = 0;
        public int SCP_BlueMax = 255;
        public int SCP_AreaFilter = 1;
        public double SCP_SizeMin = 100;
        public double SCP_SizeMax = 1000;
        public int SCP_LengthFilter = 1;
        public double SCP_MinLength = 5;
        public double SCP_MaxLength = 20;
        public double SCP_MinShort = 5;
        public double SCP_MaxShort = 15;
        public double SCP_MinRate = 1.0;
        public double SCP_MaxRate = 1.8;
        public int SCP_PARate = 1;
        public double SCP_MinPARate = 5.6;
        public double SCP_MaxPARate = 9.8;

        //仪器发向上位机
        public int SCP_Dish;
        public string SCP_DishCode;
        //public int SCP_Plate1;
        //public int SCP_Plate2;
        //public int SCP_Plate3;
        //public int SCP_Plate4;
        //public int SCP_Plate5;
        //public int SCP_Plate6;
        //public int SCP_Plate7;
        //public int SCP_Plate8;
        public int[] SCP_Plate= new int[8];
        public string SCP_Plate1Code;
        public string SCP_Plate2Code;
        public string SCP_Plate3Code;
        public string SCP_Plate4Code;
        public string SCP_HoleX;
        public string SCP_HoleY;
        public float SCP_PointX;
        public float SCP_PointY;
        public int SCP_CentroidX;
        public int SCP_CentroidY;
        public int SCP_Area;
        public int SCP_Perimeter;
        public int SCP_MajorDiameter;
        public int SCP_MinorDiameter;
        public float SCP_MajToMinAxisRatio;
        public int SCP_ColorR;
        public int SCP_ColorG;
        public int SCP_ColorB;
        public string SCP_Pic="null";
        public string SCP_Data = "null";

        //上层Form需要用到这些函数，所以暂时保留，但底层类操作中不需要这些函数
        public UInt32 getJiaReShiJian() { return this.SCP_HeatTime; }
        public UInt32 getQingXiShiJian() { return this.SCP_FlushTime; }
        public UInt32 getQingXiCiShu() { return this.SCP_FlushNo; }
        public UInt32 getChouQiShiJian() { return this.SCP_ExhaustTime; }
        public UInt32 getLengQueShiJian() { return this.SCP_CoolTime; }
        public double getZhouChangMianJiBi_Max() { return this.SCP_MaxPARate; }
        public double getZhouChangMianJiBi_Min() { return this.SCP_MinPARate; }
        public double getMianJi_Max() { return this.SCP_SizeMax; }
        public double getMianJi_Min() { return this.SCP_SizeMin; }
        public double getChangJing_Max() { return this.SCP_MaxLength; }
        public double getChangJing_Min() { return this.SCP_MinLength; }
        public double getDuanJing_Max() { return this.SCP_MaxShort; }
        public double getDuanJing_Min() { return this.SCP_MinShort; }
        public double getBiZhi_Max() { return this.SCP_MaxRate; }
        public double getBiZhi_Min() { return this.SCP_MinRate; }

        public void setZhouChangMianJiBi_Max(string max) {  SCP_MaxPARate = double.Parse(max); }
        public void setZhouChangMianJiBi_Min(string min) {  SCP_MinPARate = double.Parse(min); }

        public override void decodeReportMessage(ModbusMessage msg)//解码报告消息
        {
            String reportType = (String)msg.Data["ReportType"];
            if ("READ_IMAGE".Equals(reportType))
            {
                SCP_Pic = (string)msg.Data["SCP_Pic"];
            }
            if ("READ_DATA".Equals(reportType))
            {
                SCP_Data = (string)msg.Data["SCP_Data"];
            }
            if ("READ_DATA".Equals(reportType))
            {
                SCP_Data = (string)msg.Data["SCP_Data"];
            }
            if ("isDishAndPlate".Equals(reportType))
            {
                SCP_Dish = int.Parse((string)msg.Data["SCP_Dish"]);
                SCP_DishCode = (string)msg.Data["SCP_DishCode"];
                string[] key = new string[8];
                for (int i = 0; i < 8; i++)
            {
                key[i] = "SCP_Plate" + (i+1).ToString();
            }

                for (int j = 0; j < 8; j++)
                {
                    SCP_Plate[j] = int.Parse((string)msg.Data[key[j]]);
                }
              
            }
            
        }
       
    }
}
