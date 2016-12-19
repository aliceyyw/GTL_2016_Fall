using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using DeviceUtils;

namespace Instrument
{

    public class CloneSelectionDeviceMessageCreator
    {

        public static String createOKResponse()
        {
            ModbusMessageDataCreator creator = new ModbusMessageDataCreator();
            creator.addKeyPair("Result", "OK");
            return ModbusMessageHelper.createModbusMessage(ModbusMessage.messageTypeToByte(ModbusMessage.MessageType.RESPONSE), creator.getDataBytes());
        }

    }

    public class CloneSelectionDevice : BaseVirtualDevice
    {
        ///上位机发向仪器
        //平皿和孔板选择
        public int SCP_LightType = 0;
        public int SCP_DishType = 0;
        public int SCP_PlateType = 0;
        public int SCP_ProbeMethod = 0;
        public int SCP_NeedleFlag = 0;
        public int SCP_NeedleNum = 0;
        public int SCP_DishNeedleFlag = 0;
        public int SCP_CloneNum = 96;
        public int SCP_PlateFlag = 0;
        public int SCP_SpaceFlag = 0;

        //过程设置
        public int SCP_PickStopTime = 1;
        public int SCP_InoStopTime = 1;
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

        public static int SCP_TestRowNum = 1;

        //仪器发向上位机
        public int SCP_Dish;
        public string SCP_DishCode;
        public int SCP_Plate1;
        public int SCP_Plate2;
        public int SCP_Plate3;
        public int SCP_Plate4;
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
        public string SCP_Pic;

        /*//色彩处理
        public int SCP_Gamma = 0;
        public int SCP_Contrast = 0;
        public int SCP_ColEnhance = 0;
        public int SCP_Saturate= 0;
        //亮度控制
        public int SCP_Exposure= 0;
        public int SCP_Target= 0;
        public int SCP_ExpoTime= 0;
        public int SCP_Gain= 0;
        //白平衡
        public int SCP_Red= 0;
        public int SCP_Green= 0;
        public int SCP_Blue = 0;
        //大小
        public string SCP_Pixel = "";
        public int SCP_FrameRate= 0;
        public int SCP_PowerFrequency = 0;
        public int SCP_ParaSet= 0;
        public int SCP_Flip= 0;
        public int SCP_Horizontal= 0;
        public int SCP_GreyLevel= 0;
        public int SCP_Scale= 0;*/

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


        public void imageSendReport(string filename)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ReportType", "READ_IMAGE");
            ht.Add("SCP_Pic", filename);
            SendModBusMsg(ModbusMessage.MessageType.REPORT, ht);
        }




        public override void decodeSetMessage(ModbusMessage msg)
        {
            String setType = (String)msg.Data["SetType"];
            if ("KongBanXuanZe".Equals(setType))
            {
                this.SCP_PickStopTime = Convert.ToInt32((String)msg.Data["SCP_PickStopTime"]);
                this.SCP_ShockCount = Convert.ToInt32((String)msg.Data["SCP_ShockCount"]);
                this.SCP_InoStopTime = Convert.ToInt32((String)msg.Data["SCP_InoStopTime"]);
                this.SCP_LightType = Convert.ToInt32((String)msg.Data["SCP_LightType"]);
                this.SCP_DishType = Convert.ToInt32((String)msg.Data["SCP_DishType"]);
                this.SCP_PlateType = Convert.ToInt32((String)msg.Data["SCP_PlateType"]);
                this.SCP_ProbeMethod = Convert.ToInt32((String)msg.Data["SCP_ProbeMethod"]);
                this.SCP_CloneNum = Convert.ToInt32((String)msg.Data["SCP_CloneNum"]);
            }
            if ("ZhouChangMianJiBi".Equals(setType)) 
            {
                this.SCP_MaxPARate = double.Parse((String)msg.Data["SCP_MaxPARate"]);
                this.SCP_MinPARate = double.Parse((String)msg.Data["SCP_MinPARate"]);
            }

            if ("MianJiShaiXuan".Equals(setType))
            {
                this.SCP_SizeMax = double.Parse((String)msg.Data["SCP_SizeMax"]);
                this.SCP_SizeMin = double.Parse((String)msg.Data["SCP_SizeMin"]);
            } 

            if ("ChangDuanJingShaiXuan".Equals(setType))
            {
                this.SCP_MaxLength = double.Parse((String)msg.Data["SCP_MaxLength"]);
                this.SCP_MinLength = double.Parse((String)msg.Data["SCP_MinLength"]);
                this.SCP_MaxShort = double.Parse((String)msg.Data["SCP_MaxShort"]);
                this.SCP_MinShort = double.Parse((String)msg.Data["SCP_MinShort"]);
                this.SCP_MaxRate = double.Parse((String)msg.Data["SCP_MaxRate"]);
                this.SCP_MinRate = double.Parse((String)msg.Data["SCP_MinRate"]);
            } 

            if ("SeDuPingJunZhi".Equals(setType))
            {
                this.SCP_RedMax = Int16.Parse((String)msg.Data["SCP_RedMax"]);
                this.SCP_RedMin = Int16.Parse((String)msg.Data["SCP_RedMin"]);
                this.SCP_GreenMax = Int16.Parse((String)msg.Data["SCP_GreenMax"]);
                this.SCP_GreenMin = Int16.Parse((String)msg.Data["SCP_GreenMin"]);
                this.SCP_BlueMax = Int16.Parse((String)msg.Data["SCP_BlueMax"]);
                this.SCP_BlueMin = Int16.Parse((String)msg.Data["SCP_BlueMin"]);
            }

            if ("MieJunHeQingXi".Equals(setType))
            {
                this.SCP_HeatTime = UInt32.Parse((String)msg.Data["SCP_HeatTime"]);
                this.SCP_FlushTime = UInt32.Parse((String)msg.Data["SCP_FlushTime"]);
                this.SCP_FlushNo = UInt32.Parse((String)msg.Data["SCP_FlushNo"]);
                this.SCP_ExhaustTime = UInt32.Parse((String)msg.Data["SCP_ExhaustTime"]);
                this.SCP_CoolTime = UInt32.Parse((String)msg.Data["SCP_CoolTime"]);
            }

        }
    }

}
