﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Timers;
using DeviceUtils;

namespace Instrument
{
    public class MDFDispenMessage
    {
        public String Barcode;
        public String Stackcode;
        public String Petricode;

        public MDFDispenMessage()
        {
            Barcode = Stackcode = Petricode = "";
        }
    }


    public class AutoDispenDevice : BaseVirtualDevice
    {
        /// <summary>
        /// MDF parameters
        /// </summary>
        public int MDF_NumsperStack = 0;
        public double MDF_VolsperDish = 0;
        public double MDF_Current1;
        public double MDF_Current2;
        public double MDF_Current3;
        public double MDF_Current4;
        public int MDF_RunningError;
        public int MDF_DispenTime;
        public int MDF_CurSamTime;
        public int MDF_WhichStack = 1;
        public int MDF_WhichDish = 1;
        public string MDF_InBarCode;
        public string MDF_OutBarCode;
        public string MDF_Cmd;

        /// <summary>
        /// Others
        /// </summary>
        private List<MDFDispenMessage> DispenMessages = new List<MDFDispenMessage>();

        private bool needRefreshMessages = false;
        private Object RefreshObject = new Object();
        private object KeyObject = new object();

        private System.Timers.Timer samTimer = null;
        private System.Timers.Timer dispenTimer = null;

        private int MDF_RemianedDish = 0; //控制剩余的培养皿数量

        public void sendCmd(String cmd)
        {
            SendModBusMsg(ModbusMessage.MessageType.CMD, "Cmd", cmd);
        }

        public void sendMDFSetNumAndVol(String Num, String Vol)
        {
            Hashtable ht = new Hashtable();
            ht.Add("SetType", "MDF_NumAndVol");
            ht.Add("MDF_NumsperStack", Num);
            ht.Add("MDF_VolsperDish", Vol);
            SendModBusMsg(ModbusMessage.MessageType.SET, ht);
        }

        public void sendOKResponse()
        {
            SendModBusMsg(ModbusMessage.MessageType.RESPONSE, "Result", "OK");
        }

        public void sendMDFCodesReport(String WhichStack, String WhichDish, String InBarCode, String OutBarCode)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ReportType", "MDF");
            ht.Add("MDF_WhichStack", WhichStack);
            ht.Add("MDF_WhichDish", WhichDish);
            ht.Add("MDF_InBarCode", InBarCode);
            ht.Add("MDF_OutBarCode", OutBarCode);
            SendModBusMsg(ModbusMessage.MessageType.REPORT, ht);
        }

        public void sendMDFVolumeReport(String WhichStack, String WhichDish, String BarCode)  //MDF_Volume
        {
            Hashtable ht = new Hashtable();
            String devicetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ht.Add("ReportType", "MDF_Volume");
            ht.Add("MDF_WhichStack", WhichStack);
            ht.Add("MDF_WhichDish", WhichDish);
            ht.Add("MDF_InBarCode", BarCode);
            ht.Add("MDF_OutBarCode", BarCode);
            ht.Add("Device_Time", devicetime);
            SendModBusMsg(ModbusMessage.MessageType.REPORT, ht);
        }
        public void sendMDFCurrencyReport(String[] currency)
        {
            Hashtable ht = new Hashtable();
            String devicetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ht.Add("ReportType", "MDF_Current");
            String[] s = { "MDF_Current1", "MDF_Current2", "MDF_Current3", "MDF_Current4" };
            for (int i = 0; i < s.Length; i++)
            {
                ht.Add(s[i], currency[i]);
            }
            ht.Add("Device_Time", devicetime);
            SendModBusMsg(ModbusMessage.MessageType.REPORT, ht);
        }

        public bool NeedRefreshMessages
        {
            get
            {
                lock (RefreshObject)
                {
                    bool res = needRefreshMessages;
                    needRefreshMessages = false;
                    return res;
                }
            }
        }
        public List<MDFDispenMessage> getDispenMessages()
        {
            List<MDFDispenMessage> res = new List<MDFDispenMessage>();
            lock (DispenMessages)
            {
                foreach (MDFDispenMessage xinXin in DispenMessages)
                {
                    res.Add(xinXin);
                }
            }
            return res;
        }

        public int getLeft()
        {
            lock (KeyObject)
            {
                if ((MDF_NumsperStack - MDF_RemianedDish) >= 0)
                    return MDF_NumsperStack - MDF_RemianedDish;  //中控端按下开始按钮后，剩余培养皿数量依次递减
                else
                    return 0;
            }
        }

        private void increaseDispenStatus()
        {
            lock (KeyObject)
            {
                MDF_WhichDish++;
                if (MDF_WhichDish > 36)
                {
                    MDF_WhichStack++;
                    MDF_WhichDish = 1;
                }
                if (MDF_WhichStack > MDF_NumsperStack)  //total
                {
                    MDF_WhichStack = 1;
                    dispenTimer.Stop();
                }
                MDF_RemianedDish += Convert.ToInt32(MDF_VolsperDish);  //控制培养皿数量递减
            }
        }

        private void sendDispenMessage()
        {
            String Stackcode = "";
            String Petricode = "";
            String Barcode = "";

            lock (KeyObject)
            {
                Petricode = MDF_WhichDish.ToString();
                Stackcode = MDF_WhichStack.ToString();
            }
            Barcode = BarCodeGenerator.generateBarCode();
            increaseDispenStatus();
            String msg;
            MDF_InBarCode = Barcode;
            MDF_OutBarCode = Barcode;
            this.sendMDFCodesReport(Stackcode, Petricode, MDF_InBarCode, MDF_OutBarCode);
            this.sendMDFVolumeReport(Stackcode, Petricode, Barcode);  //插入数据库操作
        }

        private void DispenTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            String msg = "";
            sendDispenMessage();
        }

        private void samTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            String[] currencies = new String[] { MDF_Current1.ToString(), MDF_Current2.ToString(), MDF_Current3.ToString(), MDF_Current4.ToString() };
            this.sendMDFCurrencyReport(currencies);

        }
        public void startTimers()
        {

            if (dispenTimer != null) dispenTimer.Stop();
            if (samTimer != null) samTimer.Stop();
            dispenTimer = new System.Timers.Timer();
            dispenTimer.Interval = MDF_DispenTime * 1000;
            dispenTimer.Elapsed += new System.Timers.ElapsedEventHandler(DispenTimer_Elapsed);

            samTimer = new System.Timers.Timer();
            samTimer.Interval = MDF_CurSamTime * 1000;
            samTimer.Elapsed += new System.Timers.ElapsedEventHandler(samTimer_Elapsed);
            samTimer.Start();
        }

        public override void decodeResponseMessage(ModbusMessage msg)
        {
            //this.sendOKResponse();
        }

        public override void decodeReportMessage(ModbusMessage msg)//解码报告消息
        {
            String reportType = (String)msg.Data["ReportType"];
            if ("MDF_Current".Equals(reportType))
            {

                MDF_Current1 = double.Parse((String)msg.Data["MDF_Current1"]);
                MDF_Current2 = double.Parse((String)msg.Data["MDF_Current2"]);
                MDF_Current3 = double.Parse((String)msg.Data["MDF_Current3"]);
                MDF_Current4 = double.Parse((String)msg.Data["MDF_Current4"]);
            }
            if ("MDF".Equals(reportType))
            {
                String Stackcode = (String)msg.Data["MDF_WhichStack"];
                String Petricode = (String)msg.Data["MDF_WhichDish"];
                String Barcode = (String)msg.Data["MDF_BarCode"];
                MDFDispenMessage xinXi = new MDFDispenMessage();
                xinXi.Stackcode = Stackcode;
                xinXi.Petricode = Petricode;
                xinXi.Barcode = Barcode;
                lock (DispenMessages)
                {
                    DispenMessages.Add(xinXi);
                }
                lock (RefreshObject)
                {
                    needRefreshMessages = true;
                }
            }
        }

        public override void decodeCmdMessage(ModbusMessage msg)
        {
            String cmd = (String)msg.Data["Cmd"];
            if ("Start".Equals(cmd))
            {
                dispenTimer.Start();
                this.MDF_Cmd = "Start";
            }
            if ("Reset".Equals(cmd))
            {
                //重置所有
                MDF_WhichDish = 1;
                MDF_WhichStack = 1;
                MDF_RemianedDish = 0;
                this.MDF_Cmd = "Reset";
            }
            if ("Stop".Equals(cmd))
            {
                dispenTimer.Stop();
                this.MDF_Cmd = "Stop";
            }
            if ("Auto".Equals(cmd))
            {
                //先复位
                MDF_WhichDish = 1;
                MDF_WhichStack = 1;
                MDF_RemianedDish = 0;
                //再开始
                dispenTimer.Start();
                this.MDF_Cmd = "Auto";
            }

            this.sendOKResponse();
        }

        public override void decodeSetMessage(ModbusMessage msg)
        {
            String setType = (String)msg.Data["SetType"];
            if ("MDF_NumAndVol".Equals(setType))
            {
                this.MDF_NumsperStack = Int32.Parse((String)msg.Data["MDF_NumsperStack"]);
                this.MDF_VolsperDish = double.Parse((String)msg.Data["MDF_VolsperDish"]);
            }
        }

    }
}

