using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using DeviceUtils;

namespace Instrument
{
    public class MicroStorageDevice : BaseVirtualDevice
    {


        private const int ModuleNum = 8; //8个模块
        
        
        /*中控——>仪器*/
        public int[] MMR_ValidModule = { 0,0,0,0,0,0,0,0};  //初始全为false
        public int[] MMR_Sample_Time = new int[ModuleNum];
        
        /* 
        * 仪器——>中控 且 中控——>仪器
        */
        public float[] MMR_PH = new float[ModuleNum];
        public float[] MMR_Temperature = new float[ModuleNum];
        public float[] MMR_Flow = new float[ModuleNum];
        public float[] MMR_Rspeed = new float[ModuleNum];
        public float[] MMR_Pressure = new float[ModuleNum];
        /*
         * 仪器——>中控
         */
        public float[] MMR_DO = new float[ModuleNum];
        public float[] MMR_TailOxygen = new float[ModuleNum];
        public float[] MMR_TailCarbon = new float[ModuleNum];
        public string MMR_Barcode;

        //如果仪器收到了中控将某一模块valid的命令
        //就会回复一条respond，然后包括了自己的 PH DO和温度
              

     
        /*
         * 仪器——>中控 
         * ReportType=MMR_Report
         * current = 1,2,3...8
         * */
        public void propertySendReport(int current)
        {
            int mIndex = current - 1;
            Hashtable ht = new Hashtable();
            ht.Add("ReportType", "MMR_Report");
            ht.Add("ModuleIndex", mIndex.ToString());
            ht.Add("MMR_PH", MMR_PH[mIndex].ToString());
            ht.Add("MMR_Temperature", MMR_Temperature[mIndex].ToString());
            ht.Add("MMR_Flow", MMR_Flow[mIndex].ToString());
            ht.Add("MMR_Rspeed", MMR_Rspeed[mIndex].ToString());
            ht.Add("MMR_Pressure", MMR_Pressure[mIndex].ToString());
            ht.Add("MMR_DO", MMR_DO[mIndex].ToString());
            ht.Add("MMR_TailOxygen", MMR_TailOxygen[mIndex].ToString());
            ht.Add("MMR_TailCarbon", MMR_TailCarbon[mIndex].ToString());
            SendModBusMsg(ModbusMessage.MessageType.REPORT, ht);

        }
        public void sendPropertyReport(int cur)
        {

        }

        public override void decodeSetMessage(ModbusMessage msg)
        {
            String setType = (String)msg.Data["SetType"];
            if ("MMR_Set".Equals(setType))
            {
                int mIndex = int.Parse((string)msg.Data["ModuleIndex"]);
                MMR_PH[mIndex] = float.Parse((string)msg.Data["MMR_PH"]);
                MMR_Temperature[mIndex] = float.Parse((string)msg.Data["MMR_Temperature"]);
                MMR_Flow[mIndex] = float.Parse((string)msg.Data["MMR_Flow"]);
                MMR_Rspeed[mIndex] = float.Parse((string)msg.Data["MMR_Rspeed"]);
                MMR_Pressure[mIndex] = float.Parse((string)msg.Data["MMR_Pressure"]);
                MMR_Sample_Time[mIndex] = int.Parse((string)msg.Data["MMR_Sample_Time"]);
            }
            if ("Module_Valid".Equals(setType))
            {
                int mIndex = int.Parse((string)msg.Data["ModuleIndex"]);
                MMR_ValidModule[mIndex] = 1;
            }
            if ("Module_Invalid".Equals(setType))
            {
                int mIndex = int.Parse((string)msg.Data["ModuleIndex"]);
                MMR_ValidModule[mIndex] = 0;
            }
        }
         
         
         
        
       
         
       
    }
}
