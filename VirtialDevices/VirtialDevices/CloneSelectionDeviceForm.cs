using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DeviceUtils;
using Instrument;

namespace VirtialDevices
{
    public partial class CloneSelectionDeviceForm : Form
    {

        public DeviceForm FatherForm;
        public bool IsSocket;
        public CloneSelectionDevice DeviceInfo;

        public CloneSelectionDeviceForm()
        {
            InitializeComponent();
        }

        private void CloneSelectionDeviceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FatherForm.Enabled = true;
        }

        public void CloneSelectionDevice_cmdEvent()
        {
            currentCmdTextBox.Text = DeviceInfo.Glb_Cmd;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.jiaReTextBox.Text = DeviceInfo.getJiaReShiJian().ToString();
            this.qingXiShiJianTextBox.Text = DeviceInfo.getQingXiShiJian().ToString();
            this.qingXiCiShuTextBox.Text = DeviceInfo.getQingXiCiShu().ToString();
            this.lengQueTextBox.Text = DeviceInfo.getLengQueShiJian().ToString();
            this.chouQiTextBox.Text = DeviceInfo.getChouQiShiJian().ToString();
            this.biZhiUpperTextBox.Text = DeviceInfo.getZhouChangMianJiBi_Max().ToString();
            this.biZhiLowerTextBox.Text = DeviceInfo.getZhouChangMianJiBi_Min().ToString();
            this.areaUpperTextBox.Text = DeviceInfo.getMianJi_Max().ToString();
            this.areaLowerTextBox.Text = DeviceInfo.getMianJi_Min().ToString();
            this.changJingUpperTextBox.Text = DeviceInfo.getChangJing_Max().ToString();
            this.changJingLowerTextBox.Text = DeviceInfo.getChangJing_Min().ToString();
            this.duanJingUpperTextBox.Text = DeviceInfo.getDuanJing_Max().ToString();
            this.duanJingLowerTextBox.Text = DeviceInfo.getDuanJing_Min().ToString();
            this.jingBiZhiUpperTextBox.Text = DeviceInfo.getBiZhi_Max().ToString();
            this.jingBiZhiLowerTextBox.Text = DeviceInfo.getBiZhi_Min().ToString();

            this.colorRUpperTextBox.Text = DeviceInfo.SCP_RedMax.ToString();
            this.colorRLowerTextBox.Text = DeviceInfo.SCP_RedMin.ToString();
            this.colorGUpperTextBox.Text = DeviceInfo.SCP_GreenMax.ToString();
            this.colorGLowerTextBox.Text = DeviceInfo.SCP_GreenMin.ToString();
            this.colorBUpperTextBox.Text = DeviceInfo.SCP_BlueMax.ToString();
            this.colorBLowerTextBox.Text = DeviceInfo.SCP_BlueMin.ToString();

            this.tiaoXuanTingLiuShiJianTextBox.Text = DeviceInfo.SCP_PickStopTime.ToString();
            this.jieZhongZhenDongTextBox.Text = DeviceInfo.SCP_ShockCount.ToString();
            this.jieZhongTingLiuShiJianTextBox.Text = DeviceInfo.SCP_InoStopTime.ToString();

            this.LightcomboBox.SelectedIndex = DeviceInfo.SCP_LightType;
            this.PingminTypecomboBox.SelectedIndex = DeviceInfo.SCP_DishType;
            this.KongbanTypecomboBox1.SelectedIndex = DeviceInfo.SCP_PlateType;
            this.GongzuoFanngshicomboBox1.SelectedIndex = DeviceInfo.SCP_ProbeMethod;
            this.tiaoXuanZongShuTextBox.Text = DeviceInfo.SCP_CloneNum.ToString();
        }

        private void PicButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdPic = new OpenFileDialog();
            ofdPic.Filter = "JPG(*.JPG;*.JPEG);gif文件(*.GIF)|*.jpg;*.jpeg;*.gif";
            ofdPic.FilterIndex = 1;
            ofdPic.RestoreDirectory = true;
            ofdPic.FileName = "";
            if (ofdPic.ShowDialog() == DialogResult.OK)
            {
                string sPicPaht = ofdPic.FileName.ToString();
                FileInfo fiPicInfo = new FileInfo(sPicPaht);
                long lPicLong = fiPicInfo.Length / 1024;
                string sPicName = fiPicInfo.Name;
                string sPicDirectory = fiPicInfo.Directory.ToString();
                string sPicDirectoryPath = fiPicInfo.DirectoryName;
                Bitmap bmPic = new Bitmap(sPicPaht);
                if (lPicLong > 400)
                {
                    MessageBox.Show("此文件大小" + lPicLong + "K；已超最大限制！");
                }
                else
                {
                    Point ptLoction = new Point(bmPic.Size);
                    if (ptLoction.X > this.junLuoPictureBox.Size.Width || ptLoction.Y > junLuoPictureBox.Size.Height)
                    {
                        junLuoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        junLuoPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    }
                }
                junLuoPictureBox.LoadAsync(sPicPaht);
                this.shiBieHouPictureBox.LoadAsync(sPicPaht);
            }
        }

        private void shengchengButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String fileName = saveFileDialog1.FileName;
                this.textBox1.Text = fileName;

                int JianCeLieShu = int.Parse((String)this.textBox2.Text);

                try
                {
                    float inc = 1;
                    float start = 1;
                    float[][] v;
                    v = new float[CloneSelectionDevice.SCP_TestRowNum][];
                    for (int i = 0; i < CloneSelectionDevice.SCP_TestRowNum; i++)
                    {
                        v[i] = new float[JianCeLieShu];
                    }
                    for (int i = 0; i < CloneSelectionDevice.SCP_TestRowNum; i++)
                    {
                        for (int j = 0; j < JianCeLieShu; j++)
                        {
                            v[i][j] = start;
                            start += inc;
                        }
                    }
                    CloneSelectFileHelper.setJianCeShuJu(fileName, v, JianCeLieShu);
                }
                catch (Exception ex)
                {

                }
             }
        }

        private void daoRuButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String fileName = openFileDialog1.FileName;
                float[][] v = CloneSelectFileHelper.getJianCeShuJu(fileName, int.Parse(textBox2.Text));
                if (v != null)
                {
                    //DeviceInfo.setDetectValues(v);
                    for (int i = 0; i < int.Parse(textBox2.Text); i++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        for (int j = 0; j < CloneSelectionDevice.SCP_TestRowNum; j++)
                        {
                            lvi.SubItems.Add(v[j][i].ToString()); 
                        }
                        this.listView1.Items.Add(lvi); 
                    }
                }

                listView1.EndUpdate();
            }
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

        private void confirmButton_Click(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void jiaReTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void changJingLowerTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void colorGTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void colorRTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void colorBTextBox_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
