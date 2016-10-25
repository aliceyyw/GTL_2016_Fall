using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTLutils;

namespace Instrument
{
    public class MDFDispenMessage
    {
        public String Barcode;//条码号
        public String Stackcode;//堆码号
        public String Petricode;//培养皿号
		
        public MDFDispenMessage()
        {
            Barcode = Stackcode = Petricode = "";
        }
    }

    public class AutoDispenVirtualDevice : BaseVirtualDevice
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
        public string MDF_BarCode;
        public string MDF_Cmd;

        /// <summary>
        /// Others
        /// </summary>
        private List<MDFDispenMessage> DispenMessages = new List<MDFDispenMessage>();

        private bool needRefreshMessages = false;
        private Object RefreshObject = new Object();

        //private System.Timers.Timer samTimer = null;
        //private System.Timers.Timer dispenTimer = null;

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

        public void sendMDFCodesReport(String WhichStack, String WhichDish, String BarCode)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ReportType", "MDF");
            ht.Add("MDF_WhichStack", WhichStack);
            ht.Add("MDF_WhichDish", WhichDish);
            ht.Add("MDF_BarCode", BarCode);
            SendModBusMsg(ModbusMessage.MessageType.REPORT, ht);
        }

        public void sendMDFCurrencyReport(String[] currency)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ReportType", "MDF_Current");
            String[] s = { "MDF_Current1", "MDF_Current2", "MDF_Current3", "MDF_Current4" };
            for (int i = 0; i < s.Length; i++)
            {
                ht.Add(s[i], currency[i]);
            }
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

        public override void decodeResponseMessage(ModbusMessage msg)
        {
            this.sendOKResponse();
        }

        public override void decodeReportMessage(ModbusMessage msg)//解码报告消息
        {
            String reportType = (String)msg.Data["ReportType"];
            if ("MDF_Current".Equals(reportType))
            {
                ArrayList list = new ArrayList();

                list.Add(this.Code.Substring(0, 8)); //Device_Id
                list.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //CurrentTime
                if (msg.Data.ContainsKey("CREATER_ID")) // CREATER_ID, can be null
                    list.Add(msg.Data["CREATER_ID"]);
                else 
                    list.Add("NULL");
                if (msg.Data.ContainsKey("MDF_Current1"))
                {
                    MDF_Current1 = double.Parse((String)msg.Data["MDF_Current1"]);
                }
                if (msg.Data.ContainsKey("MDF_Current2"))
                {
                    MDF_Current2 = double.Parse((String)msg.Data["MDF_Current2"]);
                }
                if (msg.Data.ContainsKey("MDF_Current3"))
                {
                    MDF_Current3 = double.Parse((String)msg.Data["MDF_Current3"]);
                }
                if (msg.Data.ContainsKey("MDF_Current4"))
                {
                    MDF_Current4 = double.Parse((String)msg.Data["MDF_Current4"]);
                }
                list.Add(MDF_Current1.ToString());
                list.Add(MDF_Current2.ToString());
                list.Add(MDF_Current3.ToString());
                list.Add(MDF_Current4.ToString());
                if (msg.Data.ContainsKey("Device_Time")) 
                    list.Add(msg.Data["Device_Time"]);
                else 
                    list.Add("NULL");  //devicetime
                if (msg.Data.ContainsKey("TASK_ID")) 
                    list.Add(msg.Data["TASK_ID"]);
                else 
                    list.Add("NULL");  //task id
                if (msg.Data.ContainsKey("FLOW_ID")) 
                    list.Add(msg.Data["FLOW_ID"]);
                else 
                    list.Add("NULL");  //flow id
                if (msg.Data.ContainsKey("CREATE_DATE")) 
                    list.Add(msg.Data["CREATE_DATE"]);
                else 
                    list.Add("NULL");  //creater_id
                Database.insertTable(reportType, list);
                Console.WriteLine(Database.insertTable(reportType, list));
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
                if (msg.Data.ContainsKey("MDF_BarCode"))
                    list.Add(msg.Data["MDF_BarCode"]);
                else
                    list.Add("platebarcode");  //platebarcode
                if (msg.Data.ContainsKey("MDF_BarCode"))
                    list.Add(msg.Data["MDF_BarCode"]);
                else
                    list.Add("sourcebarcode");  //sourcebarcode
                if (msg.Data.ContainsKey("MDF_VolsperDish"))
                    list.Add(msg.Data["MDF_VolsperDish"]);
                else 
                    list.Add("2.0");  //volume
                list.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//devicetime
                if (msg.Data.ContainsKey("CREATE_DATE"))
                    list.Add(msg.Data["CREATE_DATE"]);
                else
                    list.Add("NULL");  //creater_id
                Database.insertTable("MDF_Volume", list);
                Console.WriteLine(Database.insertTable("MDF_Volume", list));
            }
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