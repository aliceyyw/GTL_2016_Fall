using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTLutils;

namespace Instrument
{
    
    public class MicroStorageVirtualDevice : BaseVirtualDevice
    {
        private const int ModuleNum=8; //8个模块
        public int MMR_CurentSelectedIndex;   
        public bool[] MMR_ValidModule ={false,false,false,false,false,false,false,false};  //初始全为false
        /* 数据字典中出现的8种属性
         * 数组长度和模块数量一致
         */
        public float[] MMR_PH = new float[ModuleNum];
        public float[] MMR_Temperature = new float[ModuleNum];
        public float[] MMR_Flow = new float[ModuleNum];
        public float[] MMR_DO = new float[ModuleNum];
        public float[] MMR_Rspeed = new float[ModuleNum];
        public float[] MMR_Pressure = new float[ModuleNum];
        public float[] MMR_TailOxygen = new float[ModuleNum];
        public float[] MMR_TailCarbon = new float[ModuleNum];

        // 仪器发给中控的是report
        private void decodeReportMessage(ModbusMessage msg)
        {

        }

       

       

       

    }
}
