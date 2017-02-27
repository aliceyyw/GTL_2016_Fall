using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using GTLutils;

namespace Instrument
{
    
    public class MicroStorageVirtualDevice : BaseVirtualDevice
    {
        private const int ModuleNum=8; //8个模块
        //public int MMR_CurentSelectedIndex;   
        
   
        /*中控——>仪器*/
        public int[] MMR_ValidModule = { 0, 0, 0, 0, 0, 0, 0, 0 };  //初始全为false
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
       
        /*
         * 中控——>仪器
         * SetType = MMR_Set
         * current = 1,2,3...8
         * */
        public void sendPropertySet(int current)
        {
            int mIndex = current - 1;
            Hashtable ht = new Hashtable();
            ht.Add("SetType", "MMR_Set");
            ht.Add("ModuleIndex", mIndex.ToString());
            ht.Add("MMR_PH", MMR_PH[mIndex].ToString());
            ht.Add("MMR_Temperature", MMR_Temperature[mIndex].ToString());
            ht.Add("MMR_Flow", MMR_Flow[mIndex].ToString());
            ht.Add("MMR_Rspeed", MMR_Rspeed[mIndex].ToString());
            ht.Add("MMR_Pressure", MMR_Pressure[mIndex].ToString());
            ht.Add("MMR_Sample_Time", MMR_Sample_Time[mIndex].ToString());
            SendModBusMsg(ModbusMessage.MessageType.SET, ht);
            
        } 
        /*
         * 激活模块 1,2,3...8
         * SetType = Module_Valid
         */
        public void validSet(int current)
        {
            int mIndex = current - 1;
            Hashtable ht = new Hashtable();
            ht.Add("SetType", "Module_Valid");
            ht.Add("ModuleIndex", mIndex.ToString());
            SendModBusMsg(ModbusMessage.MessageType.SET, ht);
        }
        /*
         * 停止模块 1,2,3...8
         * SetType = Module_Invalid
         */
        public void invalidSet(int current)
        {
            int mIndex = current - 1;
            Hashtable ht = new Hashtable();
            ht.Add("SetType", "Module_Invalid");
            ht.Add("ModuleIndex", mIndex.ToString());
            SendModBusMsg(ModbusMessage.MessageType.SET, ht);
        }




        // 接收仪器发来的report信息
        public override void decodeReportMessage(ModbusMessage msg)
        {
            String reportType = (String)msg.Data["ReportType"];
            if ("MMR_Report".Equals(reportType))
            {
                int mIndex = int.Parse((string)msg.Data["ModuleIndex"]);
                MMR_PH[mIndex] = float.Parse((string)msg.Data["MMR_PH"]);
                MMR_Temperature[mIndex] = float.Parse((string)msg.Data["MMR_Temperature"]);
                MMR_Flow[mIndex] = float.Parse((string)msg.Data["MMR_Flow"]);
                MMR_Rspeed[mIndex] = float.Parse((string)msg.Data["MMR_Rspeed"]);
                MMR_Pressure[mIndex] = float.Parse((string)msg.Data["MMR_Pressure"]);
                MMR_DO[mIndex] = float.Parse((string)msg.Data["MMR_DO"]);
                MMR_TailOxygen[mIndex] = float.Parse((string)msg.Data["MMR_TailOxygen"]);
                MMR_TailCarbon[mIndex] = float.Parse((string)msg.Data["MMR_TailCarbon"]);

            }
        }

       

       

       

    }
}
