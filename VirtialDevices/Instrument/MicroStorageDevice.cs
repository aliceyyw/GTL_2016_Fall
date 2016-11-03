using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeviceUtils;

namespace Instrument
{
    public class MicroStorageDevice : BaseVirtualDevice
    {


        private const int ModuleNum = 8; //8个模块
        public int MMR_CurentSelectedIndex;
        
        public bool[] MMR_ValidModule = { false, false, false, false, false, false, false, false };  //初始全为false
        /* 数据字典中出现的8种属性
        * 数组长度和模块数量一致， 本来赋了一些随机的初值
        */
        public float[] MMR_PH = new float[ModuleNum];
        public float[] MMR_Temperature = new float[ModuleNum];
        public float[] MMR_Flow = new float[ModuleNum];
        public float[] MMR_DO = new float[ModuleNum];
        public float[] MMR_Rspeed = new float[ModuleNum];
        public float[] MMR_Pressure = new float[ModuleNum];
        public float[] MMR_TailOxygen = new float[ModuleNum];
        public float[] MMR_TailCarbon = new float[ModuleNum];

        public string MMR_Barcode;
        //如果仪器收到了中控将某一模块valid的命令
        //就会回复一条respond，然后包括了自己的 PH DO和温度
        //中控发给仪器的是set类型消息
       

        private void decodeSetMessage(ModbusMessage msg)
        {
            String setType = (String)msg.Data["SetType"];
            /* setting消息
             * 接收的是中控端发来的一些属性，根据模块号给自己的属性赋值
             */
            
            /*
             * Start消息，就是中控那边关于某个模块是否valid的一个设置信息
             * respond自己的某三个属性用在这里很奇怪
             */

            /*
              * Stopt消息，就是中控那边关于某个模块invalid的一个设置信息
              * 如果是关掉模块，就不需要respond
              */
           
        }
       

       

       
    }
}
