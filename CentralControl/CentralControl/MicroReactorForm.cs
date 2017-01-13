using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTLutils;
using Instrument;

namespace CentralControl
{
    public partial class MicroReactorForm : Form
    {
        public MicroStorageVirtualDevice mrDevice;
        int CurrentSelected;

        public ControlForm FatherForm;
        public bool IsSocket;

        public MicroReactorForm()
        {
            mrDevice = new MicroStorageVirtualDevice();
            CurrentSelected = 1;
            InitializeComponent();
        }

        public void TurnOn()  //该模块可用，‘开始’不能按，只能按‘停止’ 设定模块Validation， 发送Set
        {
            StartBtn.Enabled = false;
            StopBtn.Enabled = true;
            mrDevice.MMR_ValidModule[CurrentSelected - 1] = 1;
            mrDevice.validSet(CurrentSelected);
           

        }
        public void TurnOff() //模块不可用，‘开始’能按，‘停止’不能按
        {
            StartBtn.Enabled = true;
            StopBtn.Enabled = false;
            mrDevice.MMR_ValidModule[CurrentSelected - 1] = 0;
            mrDevice.invalidSet(CurrentSelected);
          
        }
        //Global_cmd 具体实现的触发事件，将事件添加给委托的语句在ControlForm中。
        public void MicroReactorDevice_cmdEvent()
        {
            currentCmdTextBox.Text = mrDevice.Glb_Cmd;
        }

        private void comboBox1_textChanged(object sender, EventArgs e)
        {
            CurrentSelected = ModuleParseInt(this.comboBox1.Text);
            switchModule();
            
        }

        private void MicroReactorForm_Load(object sender, EventArgs e)
        {
            initModule();            
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            refresh();
            //CurrentSelected = ModuleParseInt(this.comboBox1.Text);
            //showData(CurrentSelected);
            timer1.Start();
        }

        //private void showData(int curSelectModule)   //对当前视图的模块数据进行实时监听
        //{
        //    int mIndex = curSelectModule - 1;
        //    if (mrDevice.MMR_ValidModule[mIndex] == 1)
        //    {
        //        Validlabel.Text = "有反应器";
        //    }
        //    //功能： 根据当前选中的模块，如果模块valid，则显示中控从仪器接收的那些数据，并把按钮2变成停止
        //    //目前只有 温度，PH和DO
        //    //如果是invalid，就显示0 ，然后设成开始
          
        //}

        private void initModule()
        {
            refresh();
        }
        private void switchModule()
        {
            refresh();
        }
        private void refresh()  //当前视图的模块发生切换时的读操作
        {
            CurrentSelected = ModuleParseInt(comboBox1.Text);
            if (mrDevice.MMR_ValidModule[CurrentSelected - 1] == 1)
            {
                StartBtn.Enabled = false;
                StopBtn.Enabled = true;
                Validlabel.Text = "有反应器";
            }
            else
            {
                StartBtn.Enabled = true;
                StopBtn.Enabled = false;
                Validlabel.Text = "无反应器";
            }
            int mIndex = CurrentSelected - 1;            
            PHtxtb.Text = mrDevice.MMR_PH[mIndex].ToString();
            Pressuretxtb.Text = mrDevice.MMR_Pressure[mIndex].ToString();
            Temperaturetxtb.Text = mrDevice.MMR_Temperature[mIndex].ToString();
            DOtxtb.Text = mrDevice.MMR_DO[mIndex].ToString();
            Rspeedtxtb.Text = mrDevice.MMR_Rspeed[mIndex].ToString();
            TailOxygentxtb.Text = mrDevice.MMR_TailOxygen[mIndex].ToString();
            Flowtxtb.Text = mrDevice.MMR_Flow[mIndex].ToString();
            TailCarbontxtb.Text = mrDevice.MMR_TailCarbon[mIndex].ToString();
            SampleTimetxtb.Text = mrDevice.MMR_Sample_Time[mIndex].ToString();

            //根据当前选中的模块，显示中控——>仪器的属性，从自己的变量里读
            //如果该模块valid， 就变成停止
            //如果该模块invalid，就是开始
           
        }



        private void start_Click(object sender, EventArgs e)
        {
            /*一个会在开始和停止之间切换的按钮，应该是标志当前模块是否为开始状态
             * 发了一条SET类型的start给仪器，仪器收到后就会把该模块置为valid，然后回一条response
             顺便把本地的也改成true
             * 反之亦然
             */
            
           
        }


        private int ModuleParseInt(String text)
        {
            if (text.Equals("模块1")) return 1;
            if (text.Equals("模块2")) return 2;
            if (text.Equals("模块3")) return 3;
            if (text.Equals("模块4")) return 4;
            if (text.Equals("模块5")) return 5;
            if (text.Equals("模块6")) return 6;
            if (text.Equals("模块7")) return 7;
            if (text.Equals("模块8")) return 8;
            return 0;
        }


       


        //global_cmd 设置，可改写对面的事件方法，在特定信息里实例化
        private void send_cmd(String cmd)
        {
            mrDevice.SendModBusMsg(ModbusMessage.MessageType.CMD, "Cmd", cmd);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            send_cmd("Reset");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            send_cmd("Start");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            send_cmd("Stop");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            send_cmd("Auto");
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            TurnOn();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            TurnOff();
        }

        private void Validlabel_Click(object sender, EventArgs e)
        {

        }

        private void SetBtn_Click(object sender, EventArgs e)
        {
            int mIndex = CurrentSelected - 1;
            mrDevice.MMR_PH[mIndex] = float.Parse(PHtxtb.Text);
            mrDevice.MMR_Pressure[mIndex] = float.Parse(Pressuretxtb.Text);
            mrDevice.MMR_Temperature[mIndex] = float.Parse(Temperaturetxtb.Text);
            mrDevice.MMR_Rspeed[mIndex] = float.Parse(Rspeedtxtb.Text);
            mrDevice.MMR_Flow[mIndex] = float.Parse(Flowtxtb.Text);
            mrDevice.MMR_Sample_Time[mIndex] = int.Parse(SampleTimetxtb.Text);
            mrDevice.sendPropertySet(CurrentSelected);
        }
    }
}
