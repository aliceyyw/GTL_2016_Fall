using System;
using System.IO;
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
    public partial class CloneSelectionDeviceForm : Form
    {
        public ControlForm FatherForm;
        public bool IsSocket;
        public CloneSelectionVirtualDevice CloneSelectionDevice;

        public void CloneSelectionDevice_cmdEvent()
        {
            currentCmdTextBox.Text = CloneSelectionDevice.Glb_Cmd;
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            String Lower = "", Upper = "";
            Lower = this.biZhiLowerTextBox.Text;
            Upper = this.biZhiUpperTextBox.Text;
            if (IsSocket)
            {
                String msg = CloneSelectionDeviceMessageCreator.createSetLowAndUpp(Lower, Upper);
                CloneSelectionDevice.SendMsg(msg);
            }
            else
            {
          
            }
            CloneSelectionDevice.setZhouChangMianJiBi_Max(Upper);
            CloneSelectionDevice.setZhouChangMianJiBi_Min(Lower);
        }

        private void setMianJiButton_Click(object sender, EventArgs e)
        {
            String Lower = "", Upper = "";
            Lower = this.areaLowerTextBox.Text;
            Upper = this.areaUpperTextBox.Text;
            if (IsSocket)
            {
                String msg = CloneSelectionDeviceMessageCreator.createSetMianJiLowAndUpp(Lower, Upper);
                CloneSelectionDevice.SendMsg(msg);
            }
            else
            {

            }
            CloneSelectionDevice.SCP_SizeMax = Convert.ToDouble(Upper);
            CloneSelectionDevice.SCP_SizeMin = Convert.ToDouble(Lower);
        }

        private void setKongBanXuanZeButton_Click(object sender, EventArgs e)
        {
            String tiaoXuanTingLiuShiJian = "", jieZhongZhenDong = "", jieZhongTingLiuShiJian = "", lightType = "", PingminType = "", KongbanType = "", GongzuoFanngshi = "", tiaoXuanZongShu = "";
            tiaoXuanTingLiuShiJian = tiaoXuanTingLiuShiJianTextBox.Text;
            jieZhongZhenDong = jieZhongZhenDongTextBox.Text;
            jieZhongTingLiuShiJian = jieZhongTingLiuShiJianTextBox.Text;
            lightType = LightcomboBox.SelectedIndex.ToString();
            GongzuoFanngshi = GongzuoFanngshicomboBox1.SelectedIndex.ToString();
            KongbanType = KongbanTypecomboBox1.SelectedIndex.ToString();
            PingminType = PingminTypecomboBox.SelectedIndex.ToString();
            tiaoXuanZongShu = tiaoXuanZongShuTextBox.Text;
            if (IsSocket)
            {
                String msg = CloneSelectionDeviceMessageCreator.createSetKongBanXuanZe(tiaoXuanTingLiuShiJian, jieZhongZhenDong, jieZhongTingLiuShiJian, lightType, PingminType, KongbanType, GongzuoFanngshi, tiaoXuanZongShu);
                CloneSelectionDevice.SendMsg(msg);
            }
            else
            {

            }
            CloneSelectionDevice.SCP_PickStopTime = Convert.ToInt32(tiaoXuanTingLiuShiJian);
            CloneSelectionDevice.SCP_ShockCount = Convert.ToInt32(jieZhongZhenDong);
            CloneSelectionDevice.SCP_InoStopTime = Convert.ToInt32(jieZhongTingLiuShiJian);
        }
        private void setMieJunButton_Click(object sender, EventArgs e)
        {
            String arg1 = "", arg2 = "", arg3 = "", arg4 = "", arg5 = "";
            arg1 = this.jiaReTextBox.Text;
            arg2 = this.qingXiCiShuTextBox.Text;
            arg3 = this.lengQueTextBox.Text;
            arg4 = this.qingXiShiJianTextBox.Text;
            arg5 = this.chouQiTextBox.Text;
            if (IsSocket)
            {
                String msg = CloneSelectionDeviceMessageCreator.createSetMieJun(arg1, arg2, arg3, arg4, arg5);
                CloneSelectionDevice.SendMsg(msg);
            }
            else
            {

            }
            CloneSelectionDevice.SCP_HeatTime = Convert.ToUInt32(arg1);
            CloneSelectionDevice.SCP_FlushNo = Convert.ToUInt32(arg2);
            CloneSelectionDevice.SCP_CoolTime = Convert.ToUInt32(arg3);
            CloneSelectionDevice.SCP_FlushTime = Convert.ToUInt32(arg4);
        }

        private void setChangDuanJing_Click(object sender, EventArgs e)
        {
            String changJingLower = "", changJingUpper = "", duanJingLower = "", duanJingUpper = "", biZhiLower = "", biZhiUpper = "";
            changJingLower = this.changJingLowerTextBox.Text;
            changJingUpper = this.changJingUpperTextBox.Text;
            duanJingUpper = this.duanJingUpperTextBox.Text;
            duanJingLower = this.duanJingLowerTextBox.Text;
            biZhiLower = this.jingBiZhiLowerTextBox.Text;
            biZhiUpper = this.jingBiZhiUpperTextBox.Text;
            if (IsSocket)
            {
                String msg = CloneSelectionDeviceMessageCreator.createSetChangDuanJing(changJingUpper, changJingLower, duanJingUpper, duanJingLower, biZhiUpper, biZhiLower);
                CloneSelectionDevice.SendMsg(msg);
            }
            else
            {
                //ChangDuanJingShaiXuan
            }
            CloneSelectionDevice.SCP_MaxLength = Convert.ToDouble(changJingUpper);
            CloneSelectionDevice.SCP_MinLength = Convert.ToDouble(changJingLower);
            CloneSelectionDevice.SCP_MaxShort = Convert.ToDouble(duanJingUpper);
            CloneSelectionDevice.SCP_MinShort = Convert.ToDouble(duanJingLower);
            CloneSelectionDevice.SCP_MaxRate = Convert.ToDouble(biZhiUpper);
            CloneSelectionDevice.SCP_MinRate = Convert.ToDouble(biZhiLower);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String RUpper = "", RLower = "", GUpper = "", GLower = "", BUpper = "", BLower = "";
            RUpper = this.colorRUpperTextBox.Text;
            RLower = this.colorRLowerTextBox.Text;
            GUpper = this.colorGUpperTextBox.Text;
            GLower = this.colorGLowertextBox.Text;
            BUpper = this.colorBUpperTextBox.Text;
            BLower = this.colorBLowertextBox.Text;
            if (IsSocket)
            {
                String msg = CloneSelectionDeviceMessageCreator.createSetColor(RUpper, RLower, GUpper, GLower, BUpper, BLower);
                CloneSelectionDevice.SendMsg(msg);
            }
            else
            {
                //ChangDuanJingShaiXuan
            }
            CloneSelectionDevice.SCP_RedMax = Convert.ToInt32(RUpper);
            CloneSelectionDevice.SCP_RedMin = Convert.ToInt32(RLower);
            CloneSelectionDevice.SCP_GreenMax = Convert.ToInt32(GUpper);
            CloneSelectionDevice.SCP_GreenMax = Convert.ToInt32(GLower);
            CloneSelectionDevice.SCP_BlueMax = Convert.ToInt32(BUpper);
            CloneSelectionDevice.SCP_BlueMin = Convert.ToInt32(BLower);
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

        public CloneSelectionDeviceForm()
        {
            InitializeComponent();
        }



        private void CloneSelectionDeviceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FatherForm.Enabled = true;
        }

     

        private void send_cmd(String cmd)
        {
            CloneSelectionDevice.SendModBusMsg(ModbusMessage.MessageType.CMD, "Cmd", cmd);
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

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            this.currentPictxtb.Text = CloneSelectionDevice.SCP_Pic.ToString();
        }

        private void LightcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void GongzuoFanngshicomboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void KongbanTypecomboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void PingminTypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}