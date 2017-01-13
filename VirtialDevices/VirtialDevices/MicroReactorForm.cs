using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Instrument;
using DeviceUtils;

namespace VirtialDevices
{
    public partial class MicroReactorForm : Form
    {
        public DeviceForm FatherForm;
        public MicroStorageDevice mrDevice;
        public bool IsSocket;

        int CurrentSelected;

        public MicroReactorForm()
        {
            mrDevice = new MicroStorageDevice();
            CurrentSelected = 1;
            InitializeComponent();
        }
        private void mrDeviceForm_load(object sender, EventArgs e)
        {
            refresh();
            timer1.Start();
        }
        public void MicroReactorDevice_cmdEvent()
        {
            currentCmdTextBox.Text = mrDevice.Glb_Cmd;
        }


        private void comboBox1_textChanged(object sender, EventArgs e)
        {
            CurrentSelected = parseInt(this.comboBox1.Text);
            refresh();
        }

        private int parseInt(String text)
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
        private void refresh()
        {
            // 根据当前选中的模块，将中控设定仪器的一些数据显示在自己的textbox上。
            //然后判断仪器的这个模块是不是valid，如果valid就显示自己的值，不是就显示0
           

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            CurrentSelected = parseInt(this.comboBox1.Text);
            refresh();
            timer1.Start();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();

        }

            


        //仪器到中控好像是不需要cmd的，暂且保留
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

        private void MicroReactorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
             FatherForm.Enabled = true;
        }

        /*
         * 仪器——>中控 8个属性
         * 当前模块1,2,3...8
         */

        private void SendBtn_Click(object sender, EventArgs e)
        {
            int mIndex = CurrentSelected - 1;
            mrDevice.MMR_PH[mIndex] = float.Parse(PHtxtb.Text);
            mrDevice.MMR_Flow[mIndex] = float.Parse(Flowtxtb.Text);
            mrDevice.MMR_Temperature[mIndex] = float.Parse(Temperaturetxtb.Text);
            mrDevice.MMR_Pressure[mIndex] = float.Parse(Pressuretxtb.Text);
            mrDevice.MMR_DO[mIndex] = float.Parse(DOtxtb.Text);
            mrDevice.MMR_TailOxygen[mIndex] = float.Parse(TailOxygentxtb.Text);
            mrDevice.MMR_TailCarbon[mIndex] = float.Parse(TailCarbontxtb.Text);
            mrDevice.MMR_Rspeed[mIndex] = float.Parse(Rspeedtxtb.Text);
            mrDevice.propertySendReport(CurrentSelected);
           
        }

        private void insertDBbtn_Click(object sender, EventArgs e)
        {
            //插入数据库的操作
        }

    }
}
