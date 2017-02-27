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
    public partial class MultiTunnelDeviceForm : Form
    {
        public ControlForm FatherForm;
        public MultiTunnelVirtualDevice DeviceInfo;

        private DataTable dt;

        public MultiTunnelDeviceForm()
        {
            InitializeComponent();
        }

        public void MultiTunnelDevice_cmdEvent()
        {
            //currentCmdTextBox.Text = DeviceInfo.Glb_Cmd;
        }

        private void MultiTunnelDeviceForm_Load(object sender, EventArgs e)
        {
            FatherForm.Enabled = false;
            MMA_TestMethod.SelectedIndex = 0;
            dt = new DataTable();
            DataColumn dc;
            for (int i = 1; i <= MultiTunnelVirtualDevice.MMA_TestColumnIndex; i++)
            {
                dc = new DataColumn(i.ToString());
                dt.Columns.Add(dc);
            }
            DataRow dr;
            for (int i = 1; i <= MultiTunnelVirtualDevice.MMA_TestRowIndex; i++)
            {
                dr = dt.NewRow();
                for (int j = 0; j < MultiTunnelVirtualDevice.MMA_TestColumnIndex; j++)
                {
                    dr[i] = "";
                }
                dt.Rows.Add(dr);
            }
            dataGridView.DataSource = dt;
            foreach (DataGridViewColumn dgc in dataGridView.Columns)
            {
                dgc.Width = 44;
                dgc.Resizable = DataGridViewTriState.False;
            }
            timer1.Start();
        }

        private void MultiTunnelDeviceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FatherForm.Enabled = true;
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            int index = MMA_TestMethod.SelectedIndex;
            if (index == 0) DeviceInfo.MMA_TestMethod = 0;
            if (index == 1) DeviceInfo.MMA_TestMethod = 1;
            if (index == 2) DeviceInfo.MMA_TestMethod = 2;
            DeviceInfo.send_moshi();
            DeviceInfo.send_cmd("Start");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            float[][] v = DeviceInfo.getDetectValues();
            String TiaoMaHao = DeviceInfo.getTiaoMaHao();
            if (v == null || TiaoMaHao == null)
            {
                timer1.Start();
                return;
            }
            DataRow dr;
            for (int i = 0; i < MultiTunnelVirtualDevice.MMA_TestRowIndex; i++)
            {
                dr = dt.Rows[i];
                for (int j = 0; j < MultiTunnelVirtualDevice.MMA_TestColumnIndex; j++)
                {
                    dr[j] = v[i][j];
                }
            }
            dataGridView.DataSource = dt;
            dataGridView.Update();
            timer1.Start();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void send_cmd(String cmd)
        {
            DeviceInfo.SendModBusMsg(ModbusMessage.MessageType.CMD, "Cmd", cmd);
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

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        //设置酶标仪信息
        private void button2_Click(object sender, EventArgs e)
        {
            DeviceInfo.MMA_TestMethod = Convert.ToInt32(MMA_TestMethod.SelectedIndex);
            DeviceInfo.MMA_TestMode = Convert.ToInt32(MMA_TestMode.SelectedIndex);
            DeviceInfo.MMA_TestType = Convert.ToInt32(MMA_TestType.SelectedIndex);
            DeviceInfo.MMA_LightType = Convert.ToInt32(MMA_LightType.SelectedIndex);
            DeviceInfo.MMA_WaveLength = Convert.ToInt32(MMA_WaveLength.Text);
            DeviceInfo.MMA_OrificeType = Convert.ToInt32(MMA_OrificeType.SelectedIndex);
            DeviceInfo.MMA_MeasureArea = Convert.ToInt32(MMA_MeasureArea.Text);
            DeviceInfo.MMA_Time = Convert.ToInt32(MMA_Time.Text);
            DeviceInfo.MMA_IntegralTime = Convert.ToInt32(MMA_IntegralTime.Text);
            MultiTunnelVirtualDevice.MMA_TestRowIndex = Convert.ToInt32(MMA_TestRowIndex.Text);
            MultiTunnelVirtualDevice.MMA_TestColumnIndex = Convert.ToInt32(MMA_TestColumnIndex.Text);
            DeviceInfo.MMA_WaveLengthUp = Convert.ToInt32(MMA_WaveLengthUp.Text);
            DeviceInfo.MMA_WaveLengthDown = Convert.ToInt32(MMA_WaveLengthDown.Text);
            DeviceInfo.MMA_MeasureTime = Convert.ToInt32(MMA_MeasureTime.Text);
            //发送酶标仪信息
            DeviceInfo.sendEliasa();
        }

        //设置加样仪信息
        private void button1_Click(object sender, EventArgs e)
        {
            DeviceInfo.MMA_TipIdx = Convert.ToInt32(MMA_TipIdx.Text);
            DeviceInfo.MMA_TargetIdx = MMA_TargetIdx.Text.ToString();
            DeviceInfo.MMA_ContainerType = Convert.ToInt32(MMA_ContainerType.SelectedIndex);
            DeviceInfo.MMA_Volume = Convert.ToInt32(MMA_Volume.Text);
            DeviceInfo.MMA_SampleIdx = MMA_SampleIdx.Text.ToString();
            DeviceInfo.MMA_SampleType = Convert.ToInt32(MMA_SampleType.SelectedIndex);
            DeviceInfo.MMA_HeatFlag = Convert.ToInt32(MMA_HeatFlag.SelectedIndex);
            DeviceInfo.MMA_Temp = Convert.ToInt32(MMA_Temp.Text);
            DeviceInfo.MMA_VibrateFlag = Convert.ToInt32(MMA_VibrateFlag.SelectedIndex);
            DeviceInfo.MMA_VibrateTime = Convert.ToInt32(MMA_VibrateTime.Text);
            //发送加样仪信息
            DeviceInfo.sendSampleAddingDevice();
        }
    }
}
