﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Timers;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string path_data= "C:\\IDIM";
        string path_ch1 = "C:\\IDIM\\channel_1.txt";
        string path_ch2 = "C:\\IDIM\\channel2.txt";
        string path_ch1_others = "C:\\IDIM\\channel1_others_1.txt";
        string path_ch2_others = "C:\\IDIM\\channel2_others.txt";
        int chang_1 = 395;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            String[] input = SerialPort.GetPortNames();
            comboBox13.Items.AddRange(input);          
            comboBox9.Items.AddRange(input);
            COM1.DataReceived += new SerialDataReceivedEventHandler(COM1_DataReceived);    
            COM2.DataReceived += new SerialDataReceivedEventHandler(COM2_DataReceived);
            // 背景自动执行
            /*
            System.Timers.Timer pTimer = new System.Timers.Timer(5000);//每隔5秒执行一次，没用winfrom自带的
            pTimer.Elapsed += pTimer_Elapsed;//委托，要执行的方法
            pTimer.AutoReset = true;//获取该定时器自动执行
            pTimer.Enabled = true;//这个一定要写，要不然定时器不会执行的
            Control.CheckForIllegalCrossThreadCalls = false;//这个不太懂，有待研究
            */
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //                      
              button12_Click(null, null);
              button2_Click(null, null);
            //生产必要的文件//

            if (!System.IO.Directory.Exists(path_data))
            {
                System.IO.Directory.CreateDirectory(path_data);//不存在就创建目录 
            }

            if (!File.Exists(@path_ch2))
            {
                File.Create(@path_ch2).Close(); //创建该文件}
            }
            if (!File.Exists(path_ch2_others))
            {
                File.Create(path_ch2_others).Close(); //创建该文件}
            }

            if (!File.Exists(@path_ch1))
            {
                File.Create(@path_ch1).Close(); //创建该文件}
            }

            if (!File.Exists(path_ch1_others))
            {
                File.Create(path_ch1_others).Close(); //创建该文件}
            }

        }
        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        // automatic running
        /*
        private void pTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
          //  button9_Click(null, null);
         //   button5_Click(null, null);
        }
        */
        // Serial communication options
        private void button2_Click(object sender, EventArgs e)
        {
             try
             {
                 COM1.PortName = comboBox13.Text;
                 COM1.BaudRate = Convert.ToInt32(comboBox1.Text);
                 COM1.DataBits = Convert.ToInt32(comboBox2.Text);
                 COM1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBox3.Text);
                 COM1.Parity = (Parity)Enum.Parse(typeof(Parity), comboBox4.Text);
                 COM1.Open();
                 progressBar1.Value = 100;
             }
             catch (Exception err)
             {
                 MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
        }       
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                COM2.PortName = comboBox9.Text;
                COM2.BaudRate = 9600;
                COM2.DataBits = 8;
                COM2.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
                COM2.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
                COM2.Open();
                progressBar6.Value = 100;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        //关闭seriel通信//
        private void button3_Click(object sender, EventArgs e)
        {
            if (COM1.IsOpen)
            {
                COM1.Close();
                progressBar1.Value = 0;
            }
        }     
        private void button11_Click(object sender, EventArgs e)
        {
            if (COM2.IsOpen)
            {
                COM2.Close();
                progressBar6.Value = 0;
            }
        }
        //Monitoring 
        private void COM1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(100);  // important
            string from_bs_1_t = COM1.ReadExisting();
            showdata(from_bs_1_t);
        }
        private void COM2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(100);  // important
           string from_bs_2_t = COM2.ReadExisting();         
           showdata_2(from_bs_2_t);
        }
        private void showdata(string from_bs_1_t)
        {

       //     FileStream fss = new FileStream(path_ch1_others, FileMode.Append, FileAccess.Write);
       //     StreamWriter sww = new StreamWriter(fss);
       //     FileStream fso = new FileStream(path_ch1, FileMode.Append, FileAccess.Write);
         //   StreamWriter swo = new StreamWriter(fso);
            int changdu = from_bs_1_t.Length;
        //   textBox1.Text += changdu;
        //    textBox1.Text += "\r\n";
            DateTime dt = DateTime.Now;  //
            int y = 0; int yue = 0;
            int d = 0; int h = 0;
            int n = 0;
            y = dt.Year;      //
            yue = dt.Month;     //
            d = dt.Day;       //
            h = dt.Hour;      //
            n = dt.Minute;    // tring logging;
            int[,] on_ff_warning = new int[2,34];
            textBox1.Text += from_bs_1_t;
            textBox1.Text += "\r\n";
            //   textBox1.Text += "-";
            //   textBox1.Text += changdu;
            //     textBox1.Text += "\r\n";
            //     if (from_bs_1_t.Contains("data")&&(changdu>=chang_1)&&(from_bs_1_t.Contains("~")))
            if (from_bs_1_t.StartsWith("data") && (changdu >= chang_1) && (from_bs_1_t.EndsWith("~")))
                {              
                int tou = from_bs_1_t.IndexOf("data");
                int wei = from_bs_1_t.IndexOf("~");
                from_bs_1_t = from_bs_1_t.Substring(tou, wei);
                from_bs_1_t = System.Text.RegularExpressions.Regex.Replace(from_bs_1_t, "[data\r\n]", "");
                textBox1.Text += from_bs_1_t;
                textBox1.Text += "\r\n";
                using (FileStream fso = new FileStream(path_ch1, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    using (StreamWriter swo = new StreamWriter(fso))
                    {
                        swo.Write(y);   //
                        swo.Write("-");
                        swo.Write(yue);
                        swo.Write("-");
                        swo.Write(d);
                        swo.Write("-");
                        swo.Write(h);
                        swo.Write(':');
                        swo.Write(n);
                        swo.Write('-');
                        swo.Write(from_bs_1_t);
                        swo.Write("\r\n");
                        swo.Close();
                    }
                       
                  fso.Close();
                }                                                  
                string[] zhongjian = from_bs_1_t.Split('/');
                string warnning="";
                string off_on = "";
                textBox9.Text = "";
                textBox6.Text = "";
                for (int i=2;i<zhongjian.Length;i++)
                {
                    string[] zhongjian_2 = zhongjian[i].Split(',');
                    for (int a=0;a<zhongjian_2.Length;a++)
                    {
                        on_ff_warning[i - 2, a] = Convert.ToInt32(zhongjian_2[a]);
                        if (i==2)
                        {
                            if (on_ff_warning[i - 2, a] == 1)//都是闭合状态
                            {
                                off_on+= a + 1 + ";";
                            }
                        }
                        if (i == 3)
                        {
                            if (on_ff_warning[i - 2, a] == 0)//没有接收到信号
                            {
                                warnning += a + 1 + ";";
                            }
                        }

                    }
                   
                }
                textBox9.Text = warnning + "No signal!";

                textBox6.Text = off_on + "_off!";
            }
            else
            {
                using (FileStream fss = new FileStream(path_ch1_others, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sww = new StreamWriter(fss))
                    {
                        string writing = y+"-"+yue+ "-"+d+ "-"+ h+ ":"+n+ "-"+from_bs_1_t;                     
                        sww.WriteLine(writing);
                        sww.Close();
                    }
                    fss.Close();
                }
                   
 

            }
            if (textBox1.Text.Length>3000)
            {
                textBox1.Clear();
            }
            /*
            sww.Close();
            fss.Close();
            sww.Dispose();
            fss.Dispose();            
            swo.Close();
            fso.Close();
            swo.Dispose();
            fso.Dispose();
            */
            GC.Collect();
        }
        private void showdata_2(string from_bs_2_t)
        {
                       
            int changdu = from_bs_2_t.Length;
         //     textBox2.Text += changdu;
         //     textBox2.Text += "\r\n";
            DateTime dt = DateTime.Now;  //
            int y = 0; int yue = 0;
            int d = 0; int h = 0;
            int n = 0;
            y = dt.Year;      //
            yue = dt.Month;     //
            d = dt.Day;       //
            h = dt.Hour;      //
            n = dt.Minute;    // tring logging;
            int[,] on_ff_warning = new int[2, 34];
            textBox2.Text += from_bs_2_t;
            textBox2.Text += "\r\n";
            if (from_bs_2_t.StartsWith("data") && (changdu >= chang_1) && (from_bs_2_t.EndsWith("~")))
            {
                int tou = from_bs_2_t.IndexOf("data");
                int wei = from_bs_2_t.IndexOf("~");
                from_bs_2_t = from_bs_2_t.Substring(tou, wei);
                from_bs_2_t = System.Text.RegularExpressions.Regex.Replace(from_bs_2_t, "[data\r\n]", "");
                textBox2.Text += from_bs_2_t;
                using (FileStream fso = new FileStream(path_ch2, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter swo = new StreamWriter(fso))
                    {
                        swo.Write(y);   //
                        swo.Write("-");
                        swo.Write(yue);
                        swo.Write("-");
                        swo.Write(d);
                        swo.Write("-");
                        swo.Write(h);
                        swo.Write(':');
                        swo.Write(n);
                        swo.Write('-');
                        swo.WriteLine(from_bs_2_t);
                        swo.Close();
                    }
                    fso.Close();
                }
                    
                string[] zhongjian = from_bs_2_t.Split('/');
                string warnning = "";
                string off_on = "";
                textBox10.Text = "";
                textBox4.Text = "";
                for (int i = 2; i < zhongjian.Length; i++)
                {
                    string[] zhongjian_2 = zhongjian[i].Split(',');
                    for (int a = 0; a < zhongjian_2.Length; a++)
                    {
                        on_ff_warning[i - 2, a] = Convert.ToInt32(zhongjian_2[a]);
                        if (i == 2)
                        {
                            if (on_ff_warning[i - 2, a] == 1)//都是闭合状态
                            {
                                off_on += a + 1 + ";";
                            }
                        }
                        if (i == 3)
                        {
                            if (on_ff_warning[i - 2, a] == 0)//没有接收到信号
                            {
                                warnning += a + 1 + ";";
                            }
                        }

                    }

                }
                textBox10.Text = warnning + "No signal!";
                textBox4.Text = off_on + "_off!";
            }
            else
            {
                using (FileStream fss = new FileStream(path_ch2_others, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sww = new StreamWriter(fss))
                    {
                        string writing = y + "-" + yue + "-" + d + "-" + h + ":" + n + "-" + from_bs_2_t;
                        sww.WriteLine(writing);
                        sww.Close();
                    }
                    fss.Close();
                }
                             
            }
            if (textBox2.Text.Length > 3000)
            {
                textBox2.Clear();
            }
            /*
            sww.Close();
            fss.Close();
            sww.Dispose();
            fss.Dispose();
            swo.Close();
            fso.Close();
            swo.Dispose();
            fso.Dispose();
            */
            GC.Collect();
        }

        // control 
        private void control1_Click(object sender, EventArgs e)
        {
            if (COM1.IsOpen)
            {
                string output;              
                output = textBox3.Text;
                COM1.Write(output);
                textBox3.Text = "";            
                COM1.DiscardOutBuffer();
                order_record(output);            
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (COM2.IsOpen)
            {
                string output;
                output = textBox8.Text;
                COM2.Write(output);
                textBox8.Text = "";
                COM2.DiscardOutBuffer();
                order_record_2(output);
            }
        }
        // kill all
        private void kill_all_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process tt = System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id);
            tt.Kill();
        }
        // record
        private void order_record(string record )
        {
            DateTime dt = DateTime.Now;  //
            int y = 0; int yue = 0;
            int d = 0; int h = 0;
            int n = 0;
            y = dt.Year;      //
            yue = dt.Month;     //
            d = dt.Day;       //
            h = dt.Hour;      //
            n = dt.Minute;
            textBox5.Text +=  y + "-"+ yue + "-" + d+ "-"+h+":"+n+ "~"+record+"\r\n";
            if (textBox5.Text.Length>500)
            {
                textBox5.Text = "";
            }
        }
        private void order_record_2(string record)
        {
            DateTime dt = DateTime.Now;  //
            int y = 0; int yue = 0;
            int d = 0; int h = 0;
            int n = 0;
            y = dt.Year;      //
            yue = dt.Month;     //
            d = dt.Day;       //
            h = dt.Hour;      //
            n = dt.Minute;
            textBox7.Text += y + "-" + yue + "-" + d + "-" + h + ":" + n + "~" + record + "\r\n";
            if (textBox7.Text.Length > 500)
            {
                textBox7.Text = "";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.SelectionStart = textBox2.Text.Length;
            textBox2.ScrollToCaret();
        }


    }

}
