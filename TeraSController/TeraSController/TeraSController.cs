using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DIMMGUI
{
    public partial class TeraSController : Form
    {
        
        public TeraSController()
            
        {
            InitializeComponent();
            initControlSettings();
            initSimParam();

            this.MouseWheel += new MouseEventHandler(chartPanel_MouseWheel);
            //this.chart13.MouseClick += new MouseEventHandler(chart13_SelectionRangeChanged);
            string reportPath = @".\Reports";
            System.IO.DirectoryInfo dir = new DirectoryInfo(reportPath);
            if(System.IO.Directory.Exists(reportPath))
            {
                foreach (FileInfo file in dir.GetFiles()) file.Delete();
            }
            else
            {
                System.IO.Directory.CreateDirectory(reportPath);
            }

            

            InitializeToolTip();

            
            hideButtons();

        }

        private void initSimParam()
        {
            if (FlashDemo)
            {
                pollWaitTime = "100";
                numChannel = "16";
                ddrSpeed = "1600";
                credit = "5";
                onfiSpeed = "400";
                interfaceType = "DDR";
                programTime = "2000";
                readTime = "1000";
                pageSize = "256";
                numPages = "1024";
                numBanks = "8";
                numDie = "2";
                enableLogs= "0";
                emCacheSize = "512";
                queueDepth = "8";
                queueFactor = "4";
                timeScale = "us";
                depthOfQueue = queueDepth;
                coreMode = "0";
                numCores = "1";
                cmdTransferTime = "10";
                enMultiSim = true;
                multiSimParam = "Queue Depth";
                enMultiSim = true;
                runSimButton.Enabled = true;
                panel4.Enabled = true;
                if (workloadTypeBox.Text == "Load Workload File")
                {
                    hostIOSizeBox.Enabled = false;
                }
                else
                {
                    hostIOSizeBox.Enabled = true;
                }
                blockSizeBox.Enabled = true;
                hostQueueDepthBox.Enabled = false;
                removeFiles = " 1";
                validBlockSize.Clear();
                validQueueDepth.Clear();

            }
            else
            {
                pollWaitTime = pollingStatusTextBox.Text;
                numChannel = numChannelBox.Text;
                ioSize = hostIOSizeBox.Text;
                blkSize = blockSizeBox.Text;
                ddrSpeed = ddrSpeedBox.Text;
                credit = outStandingCmdPerChBox.Text;
                onfiSpeed = onfiSpeedBox.Text;
                interfaceType = ifTypeBox.Text;
                programTime = programTimeBox.Text;
                readTime = readTimeBox.Text;
                pageSize = pageSizeBox.Text;
                numPages = numPagesBox.Text;
                numBanks = numBanksBox.Text;
                numDie = numDiePerChanBox.Text;
                enableLogs = checkBox2.Checked.ToString();
                emCacheSize = cwSizeBox.Text;
                queueDepth = hostQueueDepthBox.Text;
                numCommands = numCmdsBox.Text;
                queueFactor = queueFactorBox.Text;
                timeScale = comboBox22.Text;
                depthOfQueue = queueDepth;
                coreMode = threadSelectBox.Checked.ToString();
                numCores = numCoresBox.Text;
                cmdTransferTime = cmdOHTextBox.Text;
                enMultiSim = false;
                //zoomScale = int.Parse(comboBox35.Text);
                chartPanel.ScrollControlIntoView(iopsVsIOSizeChart);
            }
            cmdType = "100";
            onfiClk = "0";
            enableSeqLBA = "100";
            numSlot = 32;
            cwNum = 8;
            queueSize = 128;
            cwCnt = 16;
            timeScaleVar = 0;
            onfiClock = 0;
            mode = 0;
           
            wrkloadBS = " ";
            wrkloadFile = " ../../../../../Workload/default.txt ";
            enableWrkld = "0";
            mCoreMode = "Single CPU";
            ioSizeVariation = false;
            removeFiles = " 1";
            validParam = "";
            configFile = "../../../../../Configuration/configuration.txt ";
        }
        string pollWaitTime;
        string numChannel; 
        string ioSize;
        string blkSize;
        string ddrSpeed;
        string credit;
        string onfiSpeed;
        string interfaceType;
        string programTime;
        string readTime;
        string pageSize;
        string numPages;
        string numBanks;
        string numDie;
        string enableLogs;
        string enableSeqLBA;
        string emCacheSize;
        string queueDepth;
        string numCommands;
        string queueFactor;
        string timeScale;
        string cmdType;
        string onfiClk;

        int numSlot;
        int cwNum;
        int queueSize;
        int cwCnt;
        int timeScaleVar;
        string depthOfQueue;
        int onfiClock;
        int mode ;
        string coreMode;
        bool enMultiSim = true;//Only for Demo
        int zoomScale;
        string wrkloadBS;
        string wrkloadFile;
        string enableWrkld;
        string mCoreMode;
        string numCores;
        bool ioSizeVariation ;
        string removeFiles;
        string cmdTransferTime ;
        string validParam ;
        string configFile;
        int mAvgIoSize;

        private void initControlSettings()
        {
            List<string> twos = new List<string>();
            int item = 1;
            int max = 256;
            twos.Add(item.ToString());
            while ((item = 2 * item) <= max)
            {
                twos.Add(item.ToString());
            }

            loadGraphButton.Enabled = false;
            if (FlashDemo)
            {
                iopsVsIOSizeChart.Enabled = false;
                iopsVsIOSizeChart.Visible = false;

                latencyVsIOSizeChart.Enabled = false;
                latencyVsIOSizeChart.Visible = false;

                latencyVsIOPS_vIOSize.Enabled = false;
                latencyVsIOPS_vIOSize.Visible = false;
                iopsVsQDChart.Visible = true;
                latencyVsQDChart.Visible = true;
                latencyVsIOPs_vQDChart.Visible = true;
                label46.Visible = false;
                label46.Enabled = false;
                cmdCountVsIOSizeChart.Enabled = false;
                cmdCountVsIOSizeChart.Visible = false;
                cmdCountVsLBAChart.Enabled = false;
                cmdCountVsLBAChart.Visible = false;
                label32.Enabled = false;
                label32.Visible = false;
                comboBox25.Enabled = false;
                comboBox25.Visible = false;
                comboBox28.Enabled = false;
                comboBox28.Visible = false;
                comboBox36.Enabled = false;
                comboBox36.Visible = false;
                queueFactorBox.Enabled = false;
                queueFactorBox.Visible = false;
                label19.Enabled = false;
                label19.Visible = false;
                ddr_onfiBusUtilChart.Enabled = false;
                ddr_onfiBusUtilChart.Visible = false;
                bankUtilChart.Enabled = false;
                bankUtilChart.Visible = false;
                channelUtilChart.Enabled = false;
                channelUtilChart.Visible = false;
                label28.Visible = false;
                label28.Enabled = false;
                comboBox37.SelectedIndex = 0;
                int yLocation = 0;
                this.iopsVsQDChart.Location = new Point(1, yLocation);
                this.latencyVsQDChart.Location = new Point(1, yLocation + 292);
                this.latencyVsIOPs_vQDChart.Location = new Point(1, yLocation + 584);
                this.chartPanel.ScrollControlIntoView(iopsVsQDChart);

            }
            else
            {
                hostQueueDepthBox.Items.AddRange(twos.ToArray());
                hostQueueDepthBox.AutoCompleteCustomSource.AddRange(twos.ToArray());
                hostIOSizeBox.SelectedIndex = 3;
                workloadTypeBox.SelectedIndex = 1;
                cwSizeBox.SelectedIndex = 1;
                pageSizeBox.SelectedIndex = 0;
                numChannelBox.SelectedIndex = 3;
                numDiePerChanBox.SelectedIndex = 1;
                numBanksBox.SelectedIndex = 1;
                queueFactorBox.SelectedIndex = 3;
                hostQueueDepthBox.SelectedIndex = 0;
                numPagesBox.SelectedIndex = 0;
                readTimeBox.SelectedIndex = 0;
                programTimeBox.SelectedIndex = 0;
                outStandingCmdPerChBox.SelectedIndex = 0;
                outStandingCmdPerChBox.SelectedIndex = 0;
                ifTypeBox.SelectedIndex = 1;
                comboBox22.SelectedIndex = 0;
                ddrSpeedBox.SelectedIndex = 1;
                comboBox24.SelectedIndex = 0;
                numCoresBox.SelectedIndex = 0;
                simulationTypeBox.SelectedIndex = 2;
                simTypeBox.SelectedIndex = 0;
                blockSizeBox.SelectedIndex = 3;
                comboBox35.SelectedIndex = 0;
                button4.Text = "Load Config...";
                // chart6.Enabled = false;
                label32.Visible = false;
                label28.Visible = false;
                comboBox28.Visible = false;
                comboBox28.Enabled = false;
                comboBox25.Enabled = false;
                radioButton1.Checked = true;
                panel4.Enabled = false;
                numCoresBox.Enabled = false;
                //this.chartPanel.AutoScroll = true;
                this.chartPanel.ScrollControlIntoView(iopsVsIOSizeChart);
            }
            linkLabel1.Size = new Size(170, 20);

            linkLabel1.AutoSize = false;
            linkLabel1.Font = new Font(FontFamily.GenericSansSerif, 12);
        }

        private void InitializeToolTip()
        {
            System.Windows.Forms.ToolTip TextTip1 = new System.Windows.Forms.ToolTip();
            TextTip1.SetToolTip(this.numCmdsBox, "Number of commands to be sent to the memory");

            System.Windows.Forms.ToolTip ComboTip23 = new System.Windows.Forms.ToolTip();
            ComboTip23.SetToolTip(this.workloadTypeBox, "Type of commands: 0:read Only,100: write Only or mix of both");

            System.Windows.Forms.ToolTip checkTip1 = new System.Windows.Forms.ToolTip();
            checkTip1.SetToolTip(this.checkBox1, "Enable sequential or random LBA generation");

            System.Windows.Forms.ToolTip comboTip2 = new System.Windows.Forms.ToolTip();
            comboTip2.SetToolTip(this.hostIOSizeBox, "IO Size of data(in bytes) sent from or received at the host");

            System.Windows.Forms.ToolTip comboTip1 = new System.Windows.Forms.ToolTip();
            comboTip1.SetToolTip(this.cwSizeBox, "Code word size of data(in bytes) sent from or received at the memory at a time");

            System.Windows.Forms.ToolTip comboTip16 = new System.Windows.Forms.ToolTip();
            comboTip16.SetToolTip(this.pageSizeBox, "Page Size (in bytes) per bank in memory");

            System.Windows.Forms.ToolTip comboTip4 = new System.Windows.Forms.ToolTip();
            comboTip4.SetToolTip(this.numChannelBox, "Number of ONFI channels ");

            System.Windows.Forms.ToolTip comboTip6 = new System.Windows.Forms.ToolTip();
            comboTip6.SetToolTip(this.numDiePerChanBox, "Number of memory die per channel");

            System.Windows.Forms.ToolTip comboTip14 = new System.Windows.Forms.ToolTip();
            comboTip14.SetToolTip(this.numBanksBox, "Number of banks per die");

            System.Windows.Forms.ToolTip comboTip20 = new System.Windows.Forms.ToolTip();
            comboTip20.SetToolTip(this.queueFactorBox, "Factor to determine maximum command queue size; Queue Size is calculated as number of channels X number of logical banks X queue factor");

            System.Windows.Forms.ToolTip comboTip19 = new System.Windows.Forms.ToolTip();
            comboTip19.SetToolTip(this.hostQueueDepthBox, "Host command queue depth");

            System.Windows.Forms.ToolTip comboTip15 = new System.Windows.Forms.ToolTip();
            comboTip15.SetToolTip(this.numPagesBox, "Number of pages (in bytes) per bank in memory");

            System.Windows.Forms.ToolTip comboTip17 = new System.Windows.Forms.ToolTip();
            comboTip17.SetToolTip(this.readTimeBox, "Read access time per bank in memory");

            System.Windows.Forms.ToolTip comboTip18 = new System.Windows.Forms.ToolTip();
            comboTip18.SetToolTip(this.programTimeBox, "Write access time per bank in memory");

            System.Windows.Forms.ToolTip comboTip11 = new System.Windows.Forms.ToolTip();
            comboTip18.SetToolTip(this.outStandingCmdPerChBox, "Maximum read credit for each ONFI channel");

            System.Windows.Forms.ToolTip comboTip13 = new System.Windows.Forms.ToolTip();
            comboTip13.SetToolTip(this.ddrSpeedBox, "DDR clock frequency");

            System.Windows.Forms.ToolTip textTip2 = new System.Windows.Forms.ToolTip();
            textTip2.SetToolTip(this.onfiSpeedBox, "ONFI clock frequency");

            System.Windows.Forms.ToolTip comboTip21 = new System.Windows.Forms.ToolTip();
            comboTip21.SetToolTip(this.ifTypeBox, "ONFI interface Type");

            System.Windows.Forms.ToolTip comboTip22 = new System.Windows.Forms.ToolTip();
            comboTip22.SetToolTip(this.comboBox22, "X axis time Scale ( in us or ms) for the plots");

            System.Windows.Forms.ToolTip checkTip2 = new System.Windows.Forms.ToolTip();
            checkTip2.SetToolTip(this.checkBox2, "Enable simulation log dump");

            System.Windows.Forms.ToolTip checkTip3 = new System.Windows.Forms.ToolTip();
            checkTip3.SetToolTip(this.checkBox3, "Check this box to enable multi sim environment");

            System.Windows.Forms.ToolTip comboTip24 = new System.Windows.Forms.ToolTip();
            comboTip24.SetToolTip(this.comboBox24, "Select the parameter to vary in multi sim");

            System.Windows.Forms.ToolTip buttonTip3 = new System.Windows.Forms.ToolTip();
            buttonTip3.SetToolTip(this.button3, "Click this button to run multiple simulations");

            System.Windows.Forms.ToolTip progressBarTip1 = new System.Windows.Forms.ToolTip();
            progressBarTip1.SetToolTip(this.progressBar1, "Simulation Progress Bar..wait for complete status");

            System.Windows.Forms.ToolTip buttonTip1 = new System.Windows.Forms.ToolTip();
            buttonTip1.SetToolTip(this.loadGraphButton, "click to load the charts after running simulation ");

            System.Windows.Forms.ToolTip buttonTip2 = new System.Windows.Forms.ToolTip();
            buttonTip2.SetToolTip(this.runSimButton, "click to run simulation");

            System.Windows.Forms.ToolTip comboTip30 = new System.Windows.Forms.ToolTip();
            comboTip30.SetToolTip(this.simulationTypeBox, "Select to change configuration parameter settings");

            System.Windows.Forms.ToolTip comboTip33 = new System.Windows.Forms.ToolTip();
            comboTip33.SetToolTip(this.simTypeBox, "Select to change simulation types");

            System.Windows.Forms.ToolTip textTip8 = new System.Windows.Forms.ToolTip();
            textTip8.SetToolTip(this.pollingStatusTextBox, "Completion Queue polling interval");

            System.Windows.Forms.ToolTip checkTip5 = new System.Windows.Forms.ToolTip();
            checkTip5.SetToolTip(this.threadSelectBox, "check to select multiple cores");

            System.Windows.Forms.ToolTip comboTip27 = new System.Windows.Forms.ToolTip();
            comboTip27.SetToolTip(this.numCoresBox, "Select number of cores");

            System.Windows.Forms.ToolTip comboTip34 = new System.Windows.Forms.ToolTip();
            comboTip34.SetToolTip(this.blockSizeBox, "Block Size configuration");
        }
        
        List<long> chanTransCount = new List<long>();
        
        List<string> IOSize = new List<string>();
        string validMultiSimParam = "1";
        double latencyMaxima = 0;
        double latencyMinima = 0;
        double latencyAverage = 0;
        double iopsAverage = 0;
        List<double> maxLatency = new List<double>();
        List<double> minLatency = new List<double>();
        List<double> avgLatency = new List<double>();
        List<int> slotNum = new List<int>();
        List<double> latency = new List<double>();
        List<double> avgIOPS = new List<double>();

        //private void chart13_SelectionRangeChanged(object sender, MouseEventArgs e)
        //{
        //    double startX, endX ;
        //    startX = e.X;
        //    endX = startX + 2;
        //    //if (chart1.ChartAreas[0].CursorX.SelectionStart > chart1.ChartAreas[0].CursorX.SelectionEnd)
        //    //{
        //    //    startX = chart1.ChartAreas[0].CursorX.SelectionEnd;
        //    //    endX = chart1.ChartAreas[0].CursorX.SelectionStart;
        //    //}
        //    //else
        //    //{
        //    //    startX = chart1.ChartAreas[0].CursorX.SelectionStart;
        //    //    endX = chart1.ChartAreas[0].CursorX.SelectionEnd;
        //    //}
        //    //if (chart1.ChartAreas[0].CursorY.SelectionStart > chart1.ChartAreas[0].CursorY.SelectionEnd)
        //    //{
        //    //    endY = chart1.ChartAreas[0].CursorY.SelectionStart;
        //    //    startY = chart1.ChartAreas[0].CursorY.SelectionEnd;
        //    //}
        //    //else
        //    //{
        //    //    startY = chart1.ChartAreas[0].CursorY.SelectionStart;
        //    //    endY = chart1.ChartAreas[0].CursorY.SelectionEnd;
        //    //}

        //    if (startX == endX)
        //    {
        //        return;
        //    }

        //    chart13.ChartAreas[0].AxisX.ScaleView.Zoom(startX, (endX - startX), DateTimeIntervalType.Auto, true);
            
        //}
        /** latencyCalculation
	    * Calculates min/max/avg latency of commands 
	    * @param bool multiSim
	    * @return void
	    **/
        private void latencyCalculation(bool multiSim)
        {
            try
            {
                if (multiSim)
                {
                    maxLatency.Clear();
                    minLatency.Clear();
                    avgLatency.Clear();
                                    
                    string fileName = "";
                    string chartType;
                    if(FlashDemo)
                    {
                        chartType = MULTISIM_QD;

                    }
                    else
                    {
                        chartType = simTypeBox.Text;
                    }
                    if (chartType == "Multi Sim based on Block Size")
                    {
                        int qIndex = 0;
                        foreach (string blk in validBlockSize)
                        {
                            if (enableWrkld == "1")
                            {
                                fileName = ".\\Reports\\latency_TB_report_" + "iosize_" + blk /*wrkloadBS.Trim()*/ + "_" + "blksize_" + blk + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                            }
                            else
                            {
                                fileName = ".\\Reports\\latency_TB_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blk + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                            }
                            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader reader = new StreamReader(file))
                                {
                                    latency.Clear();
                                    slotNum.Clear();
                                    while (reader.Peek() != -1)
                                    {
                                        string line = reader.ReadLine();
                                        string[] tokens = line.Split(' ');

                                        slotNum.Add(int.Parse(tokens[0]));
                                        latency.Add(double.Parse(tokens[1]));

                                    }
                                    reader.Close();
                                    maxLatency.Add(latency.Max());
                                    minLatency.Add(latency.Min());
                                    avgLatency.Add(latency.Average());
                                    qIndex++;        
                                }
                            }
                        }
                    }//if
                    else if (chartType == "Multi Sim based on QD")
                    //else if(radioButton3.Checked == true)
                    {
                        foreach (string qd in validQueueDepth)
                        {
                            if (enableWrkld == "1")
                            {
                                fileName = ".\\Reports\\latency_TB_report_" +  "iosize_" +  wrkloadBS.Trim() + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + qd + ".log";
                            }
                            else
                            {
                                fileName = ".\\Reports\\latency_TB_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + qd + ".log";
                            }
                            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader reader = new StreamReader(file))
                                {
                                    latency.Clear();
                                    slotNum.Clear();
                                    while (reader.Peek() != -1)
                                    {
                                        string line = reader.ReadLine();
                                        string[] tokens = line.Split(' ');

                                        slotNum.Add(int.Parse(tokens[0]));
                                        latency.Add(double.Parse(tokens[1]));

                                    }
                                    reader.Close();
                                    
                                    maxLatency.Add(latency.Max());
                                    minLatency.Add(latency.Min());

                                    avgLatency.Add(latency.Average());
                                   
                                }
                            }
                        }
                    }
                    else
                    {
                        int qIndex = 0;
                        foreach (string io in validIOSize)
                        {
                            fileName = ".\\Reports\\latency_TB_report_" + "iosize_" + io + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";

                            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader reader = new StreamReader(file))
                                {
                                    latency.Clear();
                                    slotNum.Clear();
                                    while (reader.Peek() != -1)
                                    {
                                        string line = reader.ReadLine();
                                        string[] tokens = line.Split(' ');

                                        slotNum.Add(int.Parse(tokens[0]));
                                        latency.Add(double.Parse(tokens[1]));

                                    }
                                    reader.Close();
                                    maxLatency.Add(latency.Max());
                                    minLatency.Add(latency.Min());
                                    avgLatency.Add(latency.Average());
                                    qIndex++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (FileStream file = new FileStream(@".\Reports\latency_TB_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(file))
                        {

                            latency.Clear();
                            slotNum.Clear();
                            while (reader.Peek() != -1)
                            {
                                string line = reader.ReadLine();
                                string[] tokens = line.Split(' ');

                                slotNum.Add(int.Parse(tokens[0]));
                                latency.Add(double.Parse(tokens[1]));

                            }
                            reader.Close();

                            latencyMaxima = 0;
                            latencyMinima = 0;
                            latencyAverage = 0;
                            latencyMaxima = latency.Max();
                            latencyMinima = latency.Min();
                            latencyAverage = latency.Average();
                            
                        }
                        file.Close();
                    }
                }
            }

            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: latency Report", e.Message), e);
            }

        }

        /** iopsCalculation
       * Calculates average iops of the memory controller 
       * @param bool multiSim
       * @return void
       **/
        private void iopsCalculation(bool multiSim)
        {
            try
            {
                if (multiSim)
                {
                    avgIOPS.Clear();
                    string fileName = "";
                    string chartType;
                    if (FlashDemo)
                    {
                        chartType = MULTISIM_QD;

                    }
                    else
                    {
                        chartType = simTypeBox.Text;
                    }
                    if (chartType == "Multi Sim based on Block Size")
                    {
                        int qIndex = 0;
                        foreach (string io in validBlockSize)
                        {
                            if (enableWrkld == "1")
                            {
                                fileName = ".\\Reports\\IOPS_utilization_" + "iosize_" + io/*wrkloadBS.Trim()*/ + "_" + "blksize_" + io +"_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                            }
                            else
                            {
                                fileName = ".\\Reports\\IOPS_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + io + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                            }
                            qIndex++;
                            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader reader = new StreamReader(file))
                                {
                                    while (reader.Peek() != -1)
                                    {
                                        string line = reader.ReadLine();
                                        string[] tokens = line.Split(' ');

                                        double time = double.Parse(tokens[0]);
                                        avgIOPS.Add(double.Parse(tokens[1]));

                                    }
                                    reader.Close();
                                }
                                file.Close();
                            }
                        }
                    }

                    //else if (radioButton3.Checked)
                    else if (chartType == "Multi Sim based on QD")
                    {
                        foreach (string qd in validQueueDepth)
                        {
                            if (enableWrkld == "1")
                            {
                                fileName = ".\\Reports\\IOPS_utilization_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + qd + ".log";
                            }
                            else
                            {
                                fileName = ".\\Reports\\IOPS_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + qd + ".log";
                            }
                            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader reader = new StreamReader(file))
                                {
                                    while (reader.Peek() != -1)
                                    {
                                        string line = reader.ReadLine();
                                        string[] tokens = line.Split(' ');

                                        double time = double.Parse(tokens[0]);
                                        avgIOPS.Add(double.Parse(tokens[1]));

                                    }
                                    reader.Close();
                                }
                                file.Close();
                            }
                        }
                    }
                    else
                    {
                        int qIndex = 0;
                        foreach (string io in validIOSize)
                        {
                            fileName = ".\\Reports\\IOPS_utilization_" + "iosize_" + io + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                            qIndex++;
                            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader reader = new StreamReader(file))
                                {
                                    while (reader.Peek() != -1)
                                    {
                                        string line = reader.ReadLine();
                                        string[] tokens = line.Split(' ');

                                        double time = double.Parse(tokens[0]);
                                        avgIOPS.Add(double.Parse(tokens[1]));

                                    }
                                    reader.Close();
                                }
                                file.Close();
                            }
                        }
                    }

                    
                }
                else
                {
                    using (FileStream file = new FileStream(@".\Reports\IOPS_utilization.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(file))
                        {
                            while (reader.Peek() != -1)
                            {
                                string line = reader.ReadLine();
                                string[] tokens = line.Split(' ');

                                double time = double.Parse(tokens[0]);
                                iopsAverage = double.Parse(tokens[1]);
                            }
                            reader.Close();
                        }
                        file.Close();
                    }
                }
              }

            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: IOPS Report", e.Message), e);
            }
        }


        /** bankUtilizationData
        * Calculates utilization of each banks on each channel and
        * display it on the graph
        * @return void
        **/
        private void bankUtilizationData()
        {
           

            try
            {
                using (FileStream file = new FileStream(@".\Reports\channel_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(file))
                    {

                        List<int> channel = new List<int>();
                        List<int> bank = new List<int>();
                        List<long> transCount = new List<long>();
                        chanTransCount.Clear();
                        int bankSkipGap = int.Parse(emCacheSize) / int.Parse(pageSize);
                        while (reader.Peek() != -1)
                        {
                            string line = reader.ReadLine();
                            string[] tokens = line.Split(' ');

                            bank.Add(int.Parse(tokens[0]));
                            channel.Add(int.Parse(tokens[1]));
                            transCount.Add(Int64.Parse(tokens[2]));
                        }
                        reader.Close();
                        List<long> transactionCount = new List<long>();

                        int bankCount = (int.Parse(numBanks) * int.Parse(numDie));
                        for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
                        {
                            long tempVal = 0;
                            for (int bankIndex = 0; bankIndex < bankCount; bankIndex += bankSkipGap )
                            {
                                tempVal += transCount[bankIndex + chanIndex * (int.Parse(numBanks) * int.Parse(numDie))];

                            }
                            chanTransCount.Add(tempVal);
                        }

                        for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
                        {
                            for (int bankIndex = 0; bankIndex < bankCount; bankIndex += bankSkipGap)
                            {
                                transactionCount.Add(transCount[chanIndex * bankCount + bankIndex]);
                            }

                        }
                        if (bankCount > 127 && bankCount <= 256)
                            bankUtilChart.Width = 4096;
                        else if (bankCount > 63 && bankCount <= 127)
                            bankUtilChart.Width = 2048;
                        else
                            bankUtilChart.Width = 1082;

                        //bankUtilChart.Update();
                        //chartPanel.HorizontalScroll.Enabled = true;
                        if(bankUtilChart.Series.Count !=0)
                        bankUtilChart.Series[0].Points.Clear();
                        bankUtilChart.ChartAreas[0].AxisX.Title = "Logical Banks";
                        
                        bankUtilChart.ChartAreas[0].AxisY.Title = "No. Of References (CodeWord)";
                        bankUtilChart.ChartAreas[0].AxisY.Minimum = 0;
                        bankUtilChart.ChartAreas[0].AxisY.Maximum = transCount.Max();
                        //bankUtilChart.ChartAreas[0].AxisX.Maximum = bankCount / bankSkipGap + 1;

                        if(transCount.Max() > 100)
                        {
                            bankUtilChart.ChartAreas[0].AxisY.Interval = 10;
                            bankUtilChart.ChartAreas[0].AxisY.IntervalOffset = 10;
                         }
                        if (bankUtilChart.Series.Count != 0)
                        bankUtilChart.Series.Clear();
                        if (bankUtilChart.Legends.Count != 0)
                        bankUtilChart.Legends.Clear();
                        bankUtilChart.Legends.Add("Channels");
                        bankUtilChart.Legends["Channels"].Title = "Channels";
                        
                        for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
                        {
                            bankUtilChart.Series.Add(chanIndex.ToString());
                            bankUtilChart.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 5";
                        }

                        int bankLabel = 0;
                        int logicBankIndex = 1;
                        for (int cwBankIndex = 0; cwBankIndex < bankCount/ bankSkipGap; cwBankIndex++)
                        {
                           
                            bankUtilChart.ChartAreas[0].AxisX.CustomLabels.Add(logicBankIndex - 1.5, logicBankIndex + 1.5, bankLabel.ToString());
                            for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
                            {
                                bankUtilChart.Series[chanIndex.ToString()].Points.AddY(transCount.ElementAt(chanIndex * bankCount + bankSkipGap * cwBankIndex));
                            }
                            logicBankIndex++;
                            bankLabel++;
                        }
                    }
                    file.Close();
                }
                
            }

            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: Channel Report", e.Message), e);
            }

        }

        /** IOPSVsIOSize
        * Plots average IOPS versus IOSize graph
        * @param bool multiSim
        * @return void
        **/
        private void IOPSVsBlockSize(bool multiSim)
        {
            iopsVsIOSizeChart.ChartAreas[0].AxisX.Title = "Block Size";
            iopsVsIOSizeChart.Series.Clear();
            //chart7.Titles.Clear();
            //chart7.Titles.Add("IOPS Vs Block Size");
            iopsVsIOSizeChart.Titles[0].Text = "IOPS Vs Block Size";
            try
            {
                using (FileStream file = new FileStream(@".\Reports\IopsVsBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("BlockSize" + "," + "IOPS(Millions)");
                        if (multiSim)
                        {
                            
                            if (simTypeBox.Text == MULTISIM_BS)
                            {
                                int iopsIndex = 0;
                                
                                iopsVsIOSizeChart.Series.Add("0");
                                
                                iopsVsIOSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                iopsVsIOSizeChart.Series[0].Color = Color.Blue;
                                iopsVsIOSizeChart.Series[0].BorderWidth = 3;
                                iopsVsIOSizeChart.Series[0].Name = "IOPS";
                                iopsVsIOSizeChart.Annotations.Clear();
                                iopsVsIOSizeChart.Series[0].LegendText = string.Concat("QD " + validQDepth[validQDepth.Count() -1].ToString());
                             
                                for (int ioIndex = 0; ioIndex < validBlockSize.Count(); ioIndex++)
                                {
                                    iopsVsIOSizeChart.Series[0].Points.AddXY(int.Parse(validBlockSize[ioIndex]), avgIOPS[iopsIndex]);
                                    writer.WriteLine(validBlockSize[ioIndex] + "," + avgIOPS[iopsIndex]);
                                    iopsVsIOSizeChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    iopsVsIOSizeChart.Series[0].MarkerSize = 10;
                                    iopsVsIOSizeChart.Series[0].MarkerColor = Color.Green;
                                    iopsIndex++;
                                }
                                
                            }
                        }
                        else
                        {
                            iopsVsIOSizeChart.Series.Add("0");
                            if (enableWrkld == "1")
                            {
                                iopsVsIOSizeChart.Series[0].Points.AddXY(int.Parse(blkSize), iopsAverage);
                                writer.WriteLine(blkSize + "," + iopsAverage);
                            }
                            else
                            {
                                iopsVsIOSizeChart.Series[0].Points.AddXY(int.Parse(blkSize), iopsAverage);
                                writer.WriteLine(blkSize + "," + iopsAverage);
                            }
                            iopsVsIOSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            iopsVsIOSizeChart.Series[0].Name = "IOPS";
                            iopsVsIOSizeChart.Annotations.Clear();
                            iopsVsIOSizeChart.Series[0].LegendText = string.Concat("QD " + depthOfQueue);
                           
                        }
                        writer.Close();
                    }
                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: IOPSVsBlockSize", e.Message), e);
            }
        }

        private void IOPSVsIOSize(bool multiSim)
        {
            iopsVsIOSizeChart.ChartAreas[0].AxisX.Title = "IO Size";
            iopsVsIOSizeChart.Series.Clear();
          
            iopsVsIOSizeChart.Titles[0].Text = "IOPS Vs IO Size";
            
            try
            {
                using (FileStream file = new FileStream(@".\Reports\IopsVsIOSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOSize" + "," + "IOPS(Millions)");
                        if (multiSim)
                        {

                            if (simTypeBox.Text == MULTISIM_IO)
                            {
                                int iopsIndex = 0;

                                iopsVsIOSizeChart.Series.Add("0");

                                iopsVsIOSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                iopsVsIOSizeChart.Series[0].Color = Color.Blue;
                                iopsVsIOSizeChart.Series[0].BorderWidth = 3;
                                iopsVsIOSizeChart.Series[0].Name = "IOPS";
                                iopsVsIOSizeChart.Annotations.Clear();
                                iopsVsIOSizeChart.Series[0].LegendText = string.Concat("QD " + validQDepth[validQDepth.Count() - 1].ToString());

                                for (int ioIndex = 0; ioIndex < validIOSize.Count(); ioIndex++)
                                {
                                    iopsVsIOSizeChart.Series[0].Points.AddXY(int.Parse(validIOSize[ioIndex]), avgIOPS[iopsIndex]);
                                    writer.WriteLine(validIOSize[ioIndex] + "," + avgIOPS[iopsIndex]);
                                    iopsVsIOSizeChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    iopsVsIOSizeChart.Series[0].MarkerSize = 10;
                                    iopsVsIOSizeChart.Series[0].MarkerColor = Color.Green;
                                    iopsIndex++;
                                }

                            }
                        }
                            /*Single Simulation */
                        else
                        {
                            iopsVsIOSizeChart.Series.Add("0");
                            if (enableWrkld == "1")
                            {
                                iopsVsIOSizeChart.Series[0].Points.AddXY(mAvgIoSize, iopsAverage);
                                writer.WriteLine(mAvgIoSize + "," + iopsAverage);
                            }
                            else
                            {
                                iopsVsIOSizeChart.Series[0].Points.AddXY(int.Parse(ioSize), iopsAverage);
                                writer.WriteLine(ioSize + "," + iopsAverage);
                            }
                            iopsVsIOSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            iopsVsIOSizeChart.Series[0].Name = "IOPS";
                            iopsVsIOSizeChart.Annotations.Clear();
                            iopsVsIOSizeChart.Series[0].LegendText = string.Concat("QD " + depthOfQueue);

                            setChart7Annotation();

                        }
                        writer.Close();
                    }
                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: IOPSVsBlockSize", e.Message), e);
            }
        }

        private void setChart7Annotation()
        {
            iopsVsIOSizeChart.Annotations.Clear();
            TextAnnotation iosize = new TextAnnotation();
            iosize.Name = "iosize";
            if (enableWrkld == "1")
            {
                iosize.Text = string.Concat("IOSize " + mAvgIoSize);
            }
            else
            {
                iosize.Text = string.Concat("IOSize " + ioSize);
            }
            iosize.ForeColor = Color.Black;
            iosize.Font = new Font("Arial", 10, FontStyle.Bold); ;
            iosize.LineWidth = 1;
            iopsVsIOSizeChart.Annotations.Add(iosize);
            iopsVsIOSizeChart.Annotations[0].AxisX = iopsVsIOSizeChart.ChartAreas[0].AxisX;
            iopsVsIOSizeChart.Annotations[0].AxisY = iopsVsIOSizeChart.ChartAreas[0].AxisY;
            iopsVsIOSizeChart.Annotations[0].AnchorDataPoint = iopsVsIOSizeChart.Series[0].Points[0];
        }
        /** LatencyVsIOSize
        * Plots average latency versus IOSize graph
        * @param bool multiSim
        * @return void
        **/
        private void LatencyVsBlockSize(bool multiSim)
        {
            latencyVsIOSizeChart.ChartAreas[0].AxisX.Title = "Block Size";
            latencyVsIOSizeChart.Series.Clear();
            //chart8.Titles.Clear();
            //chart8.Titles.Add("Latency Vs Block Size");
            latencyVsIOSizeChart.Titles[0].Text = "Latency Vs Block Size";
            try
            {
                using (FileStream file = new FileStream(@".\Reports\LatencyVsBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("BlockSize" + "," + "Latency(us)");
                        if (multiSim)
                        {
                            if (simTypeBox.Text == "Multi Sim based on Block Size")
                            {
                                latencyVsIOSizeChart.Annotations.Clear();
                                
                                int avgLatencyIndex = 0;
                                latencyVsIOSizeChart.Series.Add("0");

                                latencyVsIOSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                latencyVsIOSizeChart.Series[0].BorderWidth = 3;
                                latencyVsIOSizeChart.Series[0].Color = Color.Blue;
                                latencyVsIOSizeChart.Series[0].Name = "Latency";
                                string legends = string.Concat("QD " + validQDepth[validQDepth.Count() - 1].ToString());
                                latencyVsIOSizeChart.Series[0].LegendText = legends;
                              
                                for (int ioIndex = 0; ioIndex < validBlockSize.Count(); ioIndex++)
                                {

                                    latencyVsIOSizeChart.Series[0].Points.AddXY(int.Parse(validBlockSize[ioIndex]), avgLatency[avgLatencyIndex]);
                                    writer.WriteLine(validBlockSize[ioIndex] +"," + avgLatency[avgLatencyIndex]);
                                    latencyVsIOSizeChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    latencyVsIOSizeChart.Series[0].MarkerSize = 10;
                                    latencyVsIOSizeChart.Series[0].MarkerColor = Color.Green;

                                    avgLatencyIndex++;
                                }

                            }
                        }
                        else
                        {
                            latencyVsIOSizeChart.Series.Add("0");
                            if (enableWrkld == "1")
                            {
                                latencyVsIOSizeChart.Series[0].Points.AddXY(int.Parse(blkSize), latencyAverage);
                                writer.WriteLine(blkSize + "," + latencyAverage);
                            }
                            else
                            {
                                latencyVsIOSizeChart.Series[0].Points.AddXY(int.Parse(blkSize), latencyAverage);
                                writer.WriteLine(blkSize + "," + latencyAverage);
                            }
                            latencyVsIOSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            latencyVsIOSizeChart.Series[0].Name = "Latency";
                            latencyVsIOSizeChart.Series[0].LegendText = string.Concat("QD " + depthOfQueue);
                            latencyVsIOSizeChart.Annotations.Clear();
                          
                        }
                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
            }

        }

        private void LatencyVsIOSize(bool multiSim)
        {
            latencyVsIOSizeChart.ChartAreas[0].AxisX.Title = "IO Size";
            latencyVsIOSizeChart.Series.Clear();
            latencyVsIOSizeChart.Titles[0].Text = "Latency Vs IO Size";
            try
            {
                using (FileStream file = new FileStream(@".\Reports\LatencyVsIOSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOSize" + "," + "Latency(us)");
                        /* Multi Sim */
                        if (multiSim)
                        {
                            if (simTypeBox.Text == MULTISIM_IO)
                            {
                                latencyVsIOSizeChart.Annotations.Clear();

                                int avgLatencyIndex = 0;
                                latencyVsIOSizeChart.Series.Add("0");

                                latencyVsIOSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                latencyVsIOSizeChart.Series[0].BorderWidth = 3;
                                latencyVsIOSizeChart.Series[0].Color = Color.Blue;
                                latencyVsIOSizeChart.Series[0].Name = "Latency";
                                string legends = string.Concat("QD " + validQDepth[validQDepth.Count() - 1].ToString());
                                latencyVsIOSizeChart.Series[0].LegendText = legends;

                                for (int ioIndex = 0; ioIndex < validIOSize.Count(); ioIndex++)
                                {

                                    latencyVsIOSizeChart.Series[0].Points.AddXY(int.Parse(validIOSize[ioIndex]), avgLatency[avgLatencyIndex]);
                                    writer.WriteLine(validIOSize[ioIndex] + "," + avgLatency[avgLatencyIndex]);
                                    latencyVsIOSizeChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    latencyVsIOSizeChart.Series[0].MarkerSize = 10;
                                    latencyVsIOSizeChart.Series[0].MarkerColor = Color.Green;

                                    avgLatencyIndex++;
                                }

                            }
                        }
                            /*Single Sim */
                        else
                        {
                            latencyVsIOSizeChart.Series.Add("0");
                            if (enableWrkld == "1")
                            {
                                latencyVsIOSizeChart.Series[0].Points.AddXY(mAvgIoSize, latencyAverage);
                                writer.WriteLine(mAvgIoSize + "," + latencyAverage);
                            }
                            else
                            {
                                latencyVsIOSizeChart.Series[0].Points.AddXY(int.Parse(ioSize), latencyAverage);
                                writer.WriteLine(ioSize + "," + latencyAverage);
                            }
                            latencyVsIOSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            latencyVsIOSizeChart.Series[0].Name = "Latency";
                            latencyVsIOSizeChart.Series[0].LegendText = string.Concat("QD " + depthOfQueue);

                            setChart8Annotation();
                        }
                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
            }

        }

        private void LatencyVsQD(bool multiSim)
        {


            latencyVsQDChart.Series.Clear();
            latencyVsQDChart.Series.Add("0");
            initChart10CAParam();

            int startOffset = 0;
            int endOffset = 60;

            for (double x = latencyVsQDChart.ChartAreas[0].AxisX.Minimum; x < latencyVsQDChart.ChartAreas[0].AxisX.Maximum; x *= 2)
            {
                CustomLabel qdLabel = new CustomLabel(startOffset, endOffset, x.ToString(), 0, LabelMarkStyle.None);
                latencyVsQDChart.ChartAreas[0].AxisX.CustomLabels.Add(qdLabel);
                startOffset = endOffset;
                endOffset += 60;
            }

            try
            {
                using (FileStream file = new FileStream(@".\Reports\LatencyVsQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("QD" + "," + "Latency(us)");
                        /*Multiple Simulation */
                        if (multiSim)
                        {
                            string chartType;
                            if (FlashDemo)
                            {
                                chartType = MULTISIM_QD;

                            }
                            else
                            {
                                chartType = simTypeBox.Text;
                            }
                            if (chartType == MULTISIM_QD)
                            {

                                for (int queueIndex = 0; queueIndex < validQueueDepth.Count(); queueIndex++)
                                {
                                    setChart10Offset(ref startOffset, ref endOffset, queueIndex);

                                    int offset = (startOffset + endOffset) / 2;
                                    latencyVsQDChart.Series[0].Color = Color.Blue;
                                    latencyVsQDChart.Series[0].Points.AddXY(offset, avgLatency[queueIndex]);
                                    writer.WriteLine(validQueueDepth[queueIndex] + "," + avgLatency[queueIndex]);
                                    latencyVsQDChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    latencyVsQDChart.Series[0].MarkerSize = 10;
                                    latencyVsQDChart.Series[0].MarkerColor = Color.Green;
                                }

                                latencyVsQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                latencyVsQDChart.Series[0].BorderWidth = 3;
                                latencyVsQDChart.Series[0].IsVisibleInLegend = false;
                                //chart10.Series[0].Name = "Latency(us)";

                                setChart10Annotation();
                            }
                        }
                        /*Single Simulation */
                        else
                        {
                            setChart10SMOffset(ref startOffset, ref endOffset);

                            int offset = (startOffset + endOffset) / 2;

                            latencyVsQDChart.Series[0].Color = Color.Blue;
                            latencyVsQDChart.Series[0].Points.AddXY(offset, latencyAverage);
                            writer.WriteLine(int.Parse(depthOfQueue) + "," + latencyAverage);
                            latencyVsQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            latencyVsQDChart.Series[0].Name = "Latency(us)";
                            setChart10Annotation();

                        }
                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
            }

        }
        private void setChart8Annotation()
        {
            latencyVsIOSizeChart.Annotations.Clear();
            TextAnnotation iosize = new TextAnnotation();
            iosize.Name = "iosize";
            if (enableWrkld == "1")
            {
                iosize.Text = string.Concat("IOSize " + mAvgIoSize);
            }
            else
            {
                iosize.Text = string.Concat("IOSize " + ioSize);
            }
            iosize.ForeColor = Color.Black;
            iosize.Font = new Font("Arial", 10, FontStyle.Bold); ;
            iosize.LineWidth = 1;
            latencyVsIOSizeChart.Annotations.Add(iosize);
            latencyVsIOSizeChart.Annotations[0].AxisX = latencyVsIOSizeChart.ChartAreas[0].AxisX;
            latencyVsIOSizeChart.Annotations[0].AxisY = latencyVsIOSizeChart.ChartAreas[0].AxisY;
            latencyVsIOSizeChart.Annotations[0].AnchorDataPoint = latencyVsIOSizeChart.Series[0].Points[0];
        }

        private void IOPSVsQD(bool multiSim)
        {
            
            iopsVsQDChart.Series.Clear();
            iopsVsQDChart.Series.Add("0");
            initChart9CAParam();
             
            int startOffset = 0;
            int endOffset = 60;
         
            for (double x = iopsVsQDChart.ChartAreas[0].AxisX.Minimum; x < iopsVsQDChart.ChartAreas[0].AxisX.Maximum; x*=2 )
            {
                CustomLabel qdLabel = new CustomLabel(startOffset, endOffset, x.ToString() , 0, LabelMarkStyle.None);
                iopsVsQDChart.ChartAreas[0].AxisX.CustomLabels.Add(qdLabel);
                startOffset = endOffset;
                endOffset += 60;
            }

            try
            {
                using (FileStream file = new FileStream(@".\Reports\IOPSVsQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("QD" + "," + "IOPS(Millions)");
                        if (multiSim)
                        {
                            //if (radioButton3.Checked)
                            string chartType;
                            if (FlashDemo)
                            {
                                chartType = MULTISIM_QD;

                            }
                            else
                            {
                                chartType = simTypeBox.Text;
                            }
                            if (chartType == "Multi Sim based on QD")
                            {
                                for (int queueDIndex = 0; queueDIndex < validQueueDepth.Count(); queueDIndex++)
                                {
                                    setPlotOffset(ref startOffset, ref endOffset, queueDIndex);

                                    int offset = (startOffset + endOffset) / 2;
                                    iopsVsQDChart.Series[0].Color = Color.Blue;

                                    if (int.Parse(queueDepth) > numSlot)
                                    {
                                        iopsVsQDChart.Series[0].Points.AddXY(offset, avgIOPS[queueDIndex]);
                                        writer.WriteLine(validQueueDepth[queueDIndex] + "," + avgIOPS[queueDIndex]);
                                    }
                                    else
                                    {
                                        iopsVsQDChart.Series[0].Points.AddXY(offset, avgIOPS[queueDIndex]);
                                        writer.WriteLine(validQueueDepth[queueDIndex] + "," + avgIOPS[queueDIndex]);
                                    }
                                    iopsVsQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                    iopsVsQDChart.Series[0].BorderWidth = 3;
                                    //chart9.Series[0].Name = "IOPS(M)";
                                    iopsVsQDChart.Series[0].IsVisibleInLegend = false;
                                    iopsVsQDChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    iopsVsQDChart.Series[0].MarkerSize = 10;
                                    iopsVsQDChart.Series[0].MarkerColor = Color.Green;
                                }
                                setChart9Annotation();

                            }

                        }
                            /*Single Simulation */
                        else
                        {
                            setSMPlotOffset(ref startOffset, ref endOffset);

                            int offset = (startOffset + endOffset) / 2;


                            iopsVsQDChart.Series[0].Color = Color.Blue;

                            if (int.Parse(queueDepth) > numSlot)
                            {
                                iopsVsQDChart.Series[0].Points.AddXY(offset, iopsAverage);
                                writer.WriteLine(depthOfQueue + "," + iopsAverage);
                            }
                            else
                            {
                                iopsVsQDChart.Series[0].Points.AddXY(offset, iopsAverage);
                                writer.WriteLine(depthOfQueue + "," + iopsAverage);
                            }
                            iopsVsQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            iopsVsQDChart.Series[0].Name = "IOPS(M)";
                            setChart9Annotation();

                        }

                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
            }
        }

        private void initChart9CAParam()
        {
            iopsVsQDChart.ChartAreas[0].AxisX.IntervalAutoMode = 0;
            iopsVsQDChart.ChartAreas[0].AxisX.Minimum = 1;
            iopsVsQDChart.ChartAreas[0].AxisX.Maximum = 512;
            iopsVsQDChart.ChartAreas[0].AxisX.Title = "QD";
            iopsVsQDChart.ChartAreas[0].AxisY.Title = "IOPS (millions)";
        }

        private void setSMPlotOffset(ref int startOffset, ref int endOffset)
        {
            switch (int.Parse(depthOfQueue))
            {
                case 1:
                    {
                        startOffset = 0;
                        endOffset = 60;
                        break;
                    }
                case 2:
                    {
                        startOffset = 60;
                        endOffset = 120;
                        break;
                    }
                case 4:
                    {
                        startOffset = 120;
                        endOffset = 180;
                        break;
                    }
                case 8:
                    {
                        startOffset = 180;
                        endOffset = 240;
                        break;
                    }
                case 16:
                    {
                        startOffset = 240;
                        endOffset = 300;
                        break;
                    }
                case 32:
                    {
                        startOffset = 300;
                        endOffset = 360;
                        break;
                    }
                case 64:
                    {
                        startOffset = 360;
                        endOffset = 420;
                        break;
                    }
                case 128:
                    {
                        startOffset = 420;
                        endOffset = 480;
                        break;
                    }
                case 256:
                    {
                        startOffset = 480;
                        endOffset = 540;
                        break;
                    }
            }
        }

        private void setChart9Annotation()
        {
            iopsVsQDChart.Annotations.Clear();
            TextAnnotation iosize = new TextAnnotation();
            iosize.Name = "iosize";
            if (enableWrkld == "1")
            {
                iosize.Text = string.Concat("IOSize " + mAvgIoSize);
            }
            else
            {
                iosize.Text = string.Concat("IOSize " + ioSize);
            }
            iosize.ForeColor = Color.Black;
            iosize.Font = new Font("Arial", 10, FontStyle.Bold); ;
            iosize.LineWidth = 1;
            iopsVsQDChart.Annotations.Add(iosize);
            iopsVsQDChart.Annotations[0].AxisX = iopsVsQDChart.ChartAreas[0].AxisX;
            iopsVsQDChart.Annotations[0].AxisY = iopsVsQDChart.ChartAreas[0].AxisY;
            iopsVsQDChart.Annotations[0].AnchorDataPoint = iopsVsQDChart.Series[0].Points[0];
        }

        private void setPlotOffset(ref int startOffset, ref int endOffset, int queueDIndex)
        {
            switch (int.Parse(validQueueDepth[queueDIndex]))
            {
                case 1:
                    {
                        startOffset = 0;
                        endOffset = 60;
                        break;
                    }
                case 2:
                    {
                        startOffset = 60;
                        endOffset = 120;
                        break;
                    }
                case 4:
                    {
                        startOffset = 120;
                        endOffset = 180;
                        break;
                    }
                case 8:
                    {
                        startOffset = 180;
                        endOffset = 240;
                        break;
                    }
                case 16:
                    {
                        startOffset = 240;
                        endOffset = 300;
                        break;
                    }
                case 32:
                    {
                        startOffset = 300;
                        endOffset = 360;
                        break;
                    }
                case 64:
                    {
                        startOffset = 360;
                        endOffset = 420;
                        break;
                    }
                case 128:
                    {
                        startOffset = 420;
                        endOffset = 480;
                        break;
                    }
                case 256:
                    {
                        startOffset = 480;
                        endOffset = 540;
                        break;
                    }
            }
        }

       

        private void setChart10Annotation()
        {
            latencyVsQDChart.Annotations.Clear();
            TextAnnotation myLine = new TextAnnotation();
            myLine.Name = "myLine";
            if (enableWrkld == "1")
            {
                myLine.Text = string.Concat("IOSize " + mAvgIoSize);
            }
            else
            {
                myLine.Text = string.Concat("IOSize " + ioSize);
            }
            myLine.ForeColor = Color.Black;
            myLine.Font = new Font("Arial", 10, FontStyle.Bold); ;
            myLine.LineWidth = 1;
            latencyVsQDChart.Annotations.Add(myLine);
            latencyVsQDChart.Annotations[0].AxisX = latencyVsQDChart.ChartAreas[0].AxisX;
            latencyVsQDChart.Annotations[0].AxisY = latencyVsQDChart.ChartAreas[0].AxisY;
            latencyVsQDChart.Annotations[0].AnchorDataPoint = latencyVsQDChart.Series[0].Points[0];
        }

        private void setChart10SMOffset(ref int startOffset, ref int endOffset)
        {
            switch (int.Parse(depthOfQueue))
            {
                case 1:
                    {
                        startOffset = 0;
                        endOffset = 60;
                        break;
                    }
                case 2:
                    {
                        startOffset = 60;
                        endOffset = 120;
                        break;
                    }
                case 4:
                    {
                        startOffset = 120;
                        endOffset = 180;
                        break;
                    }
                case 8:
                    {
                        startOffset = 180;
                        endOffset = 240;
                        break;
                    }
                case 16:
                    {
                        startOffset = 240;
                        endOffset = 300;
                        break;
                    }
                case 32:
                    {
                        startOffset = 300;
                        endOffset = 360;
                        break;
                    }
                case 64:
                    {
                        startOffset = 360;
                        endOffset = 420;
                        break;
                    }
                case 128:
                    {
                        startOffset = 420;
                        endOffset = 480;
                        break;
                    }
                case 256:
                    {
                        startOffset = 480;
                        endOffset = 540;
                        break;
                    }
            }
        }

        private void setChart10Offset(ref int startOffset, ref int endOffset, int queueIndex)
        {
            switch (int.Parse(validQueueDepth[queueIndex]))
            {
                case 1:
                    {
                        startOffset = 0;
                        endOffset = 60;
                        break;
                    }
                case 2:
                    {
                        startOffset = 60;
                        endOffset = 120;
                        break;
                    }
                case 4:
                    {
                        startOffset = 120;
                        endOffset = 180;
                        break;
                    }
                case 8:
                    {
                        startOffset = 180;
                        endOffset = 240;
                        break;
                    }
                case 16:
                    {
                        startOffset = 240;
                        endOffset = 300;
                        break;
                    }
                case 32:
                    {
                        startOffset = 300;
                        endOffset = 360;
                        break;
                    }
                case 64:
                    {
                        startOffset = 360;
                        endOffset = 420;
                        break;
                    }
                case 128:
                    {
                        startOffset = 420;
                        endOffset = 480;
                        break;
                    }
                case 256:
                    {
                        startOffset = 480;
                        endOffset = 540;
                        break;
                    }
            }
        }

        private void initChart10CAParam()
        {
            latencyVsQDChart.ChartAreas[0].AxisX.IntervalAutoMode = 0;
            latencyVsQDChart.ChartAreas[0].AxisX.Minimum = 1;
            latencyVsQDChart.ChartAreas[0].AxisX.Maximum = 512;
            latencyVsQDChart.ChartAreas[0].AxisX.Title = "QD";
            latencyVsQDChart.ChartAreas[0].AxisY.Title = "Latency (microseconds)";
        }

        private void LatencyVsIOPSwithQD(bool multiSim)
        {
            initChart14Param();
            try
            {
                using (FileStream file = new FileStream(@".\Reports\LatencyVsIOPSwithQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOPS(Millions)" + "," + "Latency(us)" + "," + "QD");
                        /* Multiple Sim */
                        if (multiSim)
                        {
                            string chartType;
                            if (FlashDemo)
                            {
                                chartType = MULTISIM_QD;

                            }
                            else
                            {
                                chartType = simTypeBox.Text;
                            }
                            if (chartType == MULTISIM_QD)
                            {
                                int iopsIndex = 0;
                                TextAnnotation[] qd = { new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
                                             new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
                                          new TextAnnotation(), new TextAnnotation(), new TextAnnotation()
                                          };


                                
                                latencyVsIOPs_vQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                latencyVsIOPs_vQDChart.Series[0].Color = Color.Blue;
                                latencyVsIOPs_vQDChart.Series[0].BorderWidth = 3;
                                //chart14.Series[0].Name = "IOPS";
                                latencyVsIOPs_vQDChart.Annotations.Clear();
                                latencyVsIOPs_vQDChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(avgIOPS.Max());

                                for (int qIndex = 0; qIndex < validQueueDepth.Count(); qIndex++)
                                {
                                    latencyVsIOPs_vQDChart.Series[0].Points.AddXY(avgIOPS[iopsIndex], avgLatency[qIndex]);
                                    writer.WriteLine(avgIOPS[iopsIndex] + "," + avgLatency[qIndex] + "," + validQueueDepth[iopsIndex]);
                                    latencyVsIOPs_vQDChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    latencyVsIOPs_vQDChart.Series[0].MarkerSize = 10;
                                    latencyVsIOPs_vQDChart.Series[0].MarkerColor = Color.Green;

                                    setChart14MSAnnotation(iopsIndex, qd, qIndex);
                                    iopsIndex++;
                                }
                            }
                        }
                            /* Single Sim */
                        else
                        {
                            latencyVsIOPs_vQDChart.Series[0].Points.AddXY(Math.Round(iopsAverage, 2), latencyAverage);
                            writer.WriteLine(Math.Round(iopsAverage, 2) + "," + latencyAverage + "," + depthOfQueue);
                            latencyVsIOPs_vQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            latencyVsIOPs_vQDChart.Series[0].Name = "IOPS";

                            setChart14SMAnnotation();

                        }
                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
            }
        }

        private void initChart14Param()
        {
            latencyVsIOPs_vQDChart.Series.Clear();
            latencyVsIOPs_vQDChart.Series.Add("0");
            latencyVsIOPs_vQDChart.Series[0].IsVisibleInLegend = false;
            latencyVsIOPs_vQDChart.ChartAreas[0].AxisX.Interval = 1;
            latencyVsIOPs_vQDChart.ChartAreas[0].AxisX.IntervalOffset = 1;
            latencyVsIOPs_vQDChart.ChartAreas[0].AxisX.Minimum = 0;
            latencyVsIOPs_vQDChart.ChartAreas[0].AxisY.Title = "Latency (microseconds)";
            latencyVsIOPs_vQDChart.ChartAreas[0].AxisX.Title = "IOPS (millions)";
        }

        private void setChart14SMAnnotation()
        {
            latencyVsIOPs_vQDChart.Annotations.Clear();
            TextAnnotation qd = new TextAnnotation();
            qd.Name = "qd_iops";
            qd.Text = string.Concat("QD " + depthOfQueue);
            qd.ForeColor = Color.Black;
            qd.Font = new Font("Arial", 10, FontStyle.Bold);
            qd.LineWidth = 1;
            latencyVsIOPs_vQDChart.Annotations.Add(qd);
            latencyVsIOPs_vQDChart.Annotations[0].AxisX = latencyVsIOPs_vQDChart.ChartAreas[0].AxisX;
            latencyVsIOPs_vQDChart.Annotations[0].AxisY = latencyVsIOPs_vQDChart.ChartAreas[0].AxisY;
            latencyVsIOPs_vQDChart.Annotations[0].AnchorDataPoint = latencyVsIOPs_vQDChart.Series[0].Points[0];
        }

        private void setChart14MSAnnotation(int iopsIndex, TextAnnotation[] qd, int qIndex)
        {
            qd[qIndex].Name = "qd_iops_" + qIndex.ToString() + validQueueDepth[iopsIndex].ToString();
            qd[qIndex].Text = string.Concat("QD " + validQueueDepth[iopsIndex].ToString());
            qd[qIndex].ForeColor = Color.Black;
            qd[qIndex].Font = new Font("Arial", 10, FontStyle.Bold);
            qd[qIndex].LineWidth = 1;
            latencyVsIOPs_vQDChart.Annotations.Add(qd[qIndex]);
            latencyVsIOPs_vQDChart.Annotations[qIndex].AxisX = latencyVsIOPs_vQDChart.ChartAreas[0].AxisX;
            latencyVsIOPs_vQDChart.Annotations[qIndex].AxisY = latencyVsIOPs_vQDChart.ChartAreas[0].AxisY;
            latencyVsIOPs_vQDChart.Annotations[qIndex].AnchorDataPoint = latencyVsIOPs_vQDChart.Series[0].Points[qIndex];
        }

        private void LatencyVsIOPSwithBlockSize(bool multiSim)
        {
            
            latencyVsIOPS_vIOSize.Series.Clear();
            //chart4.Titles.Clear();
            //chart4.Titles.Add("Latency Vs IOPS(with varying Block Size)");
            latencyVsIOPS_vIOSize.Titles[0].Text = "Latency Vs IOPS ( with varying Block Size )";
            try
            {
                using (FileStream file = new FileStream(@".\Reports\LatencyVsIOPSwithBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOPS(Millions)" + "," + "Latency(us)" + "," + "BlockSize");
                        if (multiSim)
                        {
                            if (simTypeBox.Text == "Multi Sim based on Block Size")
                            {
                                int iopsIndex = 0;
                                TextAnnotation[] qd = { new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
                                             new TextAnnotation(), new TextAnnotation()};

                                latencyVsIOPS_vIOSize.Series.Add("0");
                                latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.Interval = 0.5;
                                latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.IntervalOffset = 0.5;
                                latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.Minimum = 0;
                                latencyVsIOPS_vIOSize.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                latencyVsIOPS_vIOSize.Series[0].Color = Color.Blue;
                                latencyVsIOPS_vIOSize.Series[0].BorderWidth = 3;
                                latencyVsIOPS_vIOSize.Series[0].Name = "IOPS";
                                latencyVsIOPS_vIOSize.Annotations.Clear();


                                for (int qIndex = 0; qIndex < validBlockSize.Count(); qIndex++)
                                {
                                    latencyVsIOPS_vIOSize.Series[0].Points.AddXY(Math.Round(avgIOPS[iopsIndex], 2), avgLatency[qIndex]);
                                    writer.WriteLine(Math.Round(avgIOPS[iopsIndex], 2) + "," + avgLatency[qIndex] + "," + validBlockSize[iopsIndex]);
                                    latencyVsIOPS_vIOSize.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    latencyVsIOPS_vIOSize.Series[0].MarkerSize = 10;
                                    latencyVsIOPS_vIOSize.Series[0].MarkerColor = Color.Green;
                                    qd[qIndex].Name = "qd_iops_" + qIndex.ToString() + validBlockSize[iopsIndex].ToString();
                                    qd[qIndex].Text = string.Concat("BlockSize " + validBlockSize[iopsIndex].ToString());
                                    qd[qIndex].ForeColor = Color.Black;
                                    qd[qIndex].Font = new Font("Arial", 10, FontStyle.Bold); 
                                    qd[qIndex].LineWidth = 1;
                                    latencyVsIOPS_vIOSize.Annotations.Add(qd[qIndex]);
                                    latencyVsIOPS_vIOSize.Annotations[qIndex].AxisX = latencyVsIOPS_vIOSize.ChartAreas[0].AxisX;
                                    latencyVsIOPS_vIOSize.Annotations[qIndex].AxisY = latencyVsIOPS_vIOSize.ChartAreas[0].AxisY;
                                    latencyVsIOPS_vIOSize.Annotations[qIndex].AnchorDataPoint = latencyVsIOPS_vIOSize.Series[0].Points[qIndex];
                                    iopsIndex++;
                                }
                         
                            }
                        }
                        else
                        {
                            latencyVsIOPS_vIOSize.Series.Add("0");
                            latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.Interval = 0.5;
                            latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.IntervalOffset = 0.5;
                            latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.Minimum = 0;
                            latencyVsIOPS_vIOSize.Series[0].Points.AddXY(iopsAverage, latencyAverage);
                            if (enableWrkld == "1")
                            {
                                writer.WriteLine(iopsAverage + "," + latencyAverage + "," + wrkloadBS);
                            }
                            else
                            {
                                writer.WriteLine(iopsAverage + "," + latencyAverage + "," + blkSize);
                            }
                            latencyVsIOPS_vIOSize.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            latencyVsIOPS_vIOSize.Series[0].Name = "IOPS";
                            latencyVsIOPS_vIOSize.Annotations.Clear();
                            TextAnnotation qd = new TextAnnotation();
                            qd.Name = "qd_iops";
                            if (enableWrkld == "1")
                            {
                                qd.Text = string.Concat("BlockSize " + blkSize);
                            }
                            else
                            {
                                qd.Text = string.Concat("BlockSize " + blkSize);
                            }
                            qd.ForeColor = Color.Black;
                            qd.Font = new Font("Arial", 10, FontStyle.Bold); 
                            qd.LineWidth = 1;
                            latencyVsIOPS_vIOSize.Annotations.Add(qd);
                            latencyVsIOPS_vIOSize.Annotations[0].AxisX = latencyVsIOPS_vIOSize.ChartAreas[0].AxisX;
                            latencyVsIOPS_vIOSize.Annotations[0].AxisY = latencyVsIOPS_vIOSize.ChartAreas[0].AxisY;
                            latencyVsIOPS_vIOSize.Annotations[0].AnchorDataPoint = latencyVsIOPS_vIOSize.Series[0].Points[0];

                        }
                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
            }

        }

        private void LatencyVsIOPSwithIOSize(bool multiSim)
        {

            latencyVsIOPS_vIOSize.Series.Clear();
            //chart4.Titles.Clear();
            //chart4.Titles.Add("Latency Vs IOPS(with varying IO Size)");
            latencyVsIOPS_vIOSize.Titles[0].Text = "Latency Vs IOPS ( with varying IO Size )";
            try
            {
                using (FileStream file = new FileStream(@".\Reports\LatencyVsIOPSwithIOSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOPS(Millions)" + "," + "Latency(us)" + "," + "IOSize");
                        if (multiSim)
                        {
                            if (simTypeBox.Text == "Multi Sim based on IO Size")
                            {
                                int iopsIndex = 0;
                                TextAnnotation[] qd = { new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
                                             new TextAnnotation(), new TextAnnotation()};

                                latencyVsIOPS_vIOSize.Series.Add("0");
                                latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.Interval = 0.5;
                                latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.IntervalOffset = 0.5;
                                latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.Minimum = 0;
                                latencyVsIOPS_vIOSize.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                latencyVsIOPS_vIOSize.Series[0].Color = Color.Blue;
                                latencyVsIOPS_vIOSize.Series[0].BorderWidth = 3;
                                latencyVsIOPS_vIOSize.Series[0].Name = "IOPS";
                                latencyVsIOPS_vIOSize.Annotations.Clear();


                                for (int qIndex = 0; qIndex < validIOSize.Count(); qIndex++)
                                {
                                    latencyVsIOPS_vIOSize.Series[0].Points.AddXY(Math.Round(avgIOPS[iopsIndex], 2), avgLatency[qIndex]);
                                    writer.WriteLine(Math.Round(avgIOPS[iopsIndex], 2) + "," + avgLatency[qIndex] + "," + validIOSize[iopsIndex]);
                                    latencyVsIOPS_vIOSize.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    latencyVsIOPS_vIOSize.Series[0].MarkerSize = 10;
                                    latencyVsIOPS_vIOSize.Series[0].MarkerColor = Color.Green;
                                    qd[qIndex].Name = "qd_iops_" + qIndex.ToString() + validIOSize[iopsIndex].ToString();
                                    qd[qIndex].Text = string.Concat("IOSize " + validIOSize[iopsIndex].ToString());
                                    qd[qIndex].ForeColor = Color.Black;
                                    qd[qIndex].Font = new Font("Arial", 10, FontStyle.Bold); 
                                    qd[qIndex].LineWidth = 1;
                                    latencyVsIOPS_vIOSize.Annotations.Add(qd[qIndex]);
                                    latencyVsIOPS_vIOSize.Annotations[qIndex].AxisX = latencyVsIOPS_vIOSize.ChartAreas[0].AxisX;
                                    latencyVsIOPS_vIOSize.Annotations[qIndex].AxisY = latencyVsIOPS_vIOSize.ChartAreas[0].AxisY;
                                    latencyVsIOPS_vIOSize.Annotations[qIndex].AnchorDataPoint = latencyVsIOPS_vIOSize.Series[0].Points[qIndex];
                                    iopsIndex++;
                                }

                            }
                        }
                        else
                        {
                            latencyVsIOPS_vIOSize.Series.Add("0");
                            latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.Interval = 0.5;
                            latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.IntervalOffset = 0.5;
                            latencyVsIOPS_vIOSize.ChartAreas[0].AxisX.Minimum = 0;
                            latencyVsIOPS_vIOSize.Series[0].Points.AddXY(iopsAverage, latencyAverage);
                            if (enableWrkld == "1")
                            {
                                writer.WriteLine(iopsAverage + "," + latencyAverage + "," + mAvgIoSize);
                            }
                            else
                            {
                                writer.WriteLine(iopsAverage + "," + latencyAverage + "," + ioSize);
                            }
                            latencyVsIOPS_vIOSize.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            latencyVsIOPS_vIOSize.Series[0].Name = "IOPS";
                            latencyVsIOPS_vIOSize.Annotations.Clear();
                            TextAnnotation qd = new TextAnnotation();
                            qd.Name = "qd_iops";
                            if (enableWrkld == "1")
                            {
                                qd.Text = string.Concat("IOSize " + mAvgIoSize);
                            }
                            else
                            {
                                qd.Text = string.Concat("IOSize " + ioSize);
                            }
                            qd.ForeColor = Color.Black;
                            qd.Font = new Font("Arial", 10, FontStyle.Bold); 
                            qd.LineWidth = 1;
                            latencyVsIOPS_vIOSize.Annotations.Add(qd);
                            latencyVsIOPS_vIOSize.Annotations[0].AxisX = latencyVsIOPS_vIOSize.ChartAreas[0].AxisX;
                            latencyVsIOPS_vIOSize.Annotations[0].AxisY = latencyVsIOPS_vIOSize.ChartAreas[0].AxisY;
                            latencyVsIOPS_vIOSize.Annotations[0].AnchorDataPoint = latencyVsIOPS_vIOSize.Series[0].Points[0];

                        }
                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
            }

        }

        const string MULTISIM_BS = "Multi Sim based on Block Size";

        private void CommandVsBlockSize(bool multiSim)
        {
            setupChart11();
            try
            {
                using (FileStream file = new FileStream(@".\Reports\CmdVsBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("BlockSize" + "," + "Number of Commands");
                        if (multiSim)
                        {
                            switch(simTypeBox.Text)
                            {
                                case MULTISIM_BS:
                                    plotblkSize_msBS(writer);
                                    break;
                                case MULTISIM_QD:
                                    {
                                        plotblkSize_msQD(writer);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            //chart11.ChartAreas[0].AxisX.CustomLabels.Add(ioIndex - 1.0, ioIndex + 1.0);
                            if (enableWrkld == "1")
                            {
                                cmdCountVsIOSizeChart.Series[0].Points.AddXY(int.Parse(blkSize), Int64.Parse(numCommands));
                                writer.WriteLine(int.Parse(wrkloadBS) + "," + Int64.Parse(numCommands));
                            }
                            else
                            {
                                cmdCountVsIOSizeChart.Series[0].Points.AddXY(int.Parse(blkSize), Int64.Parse(numCommands));
                                writer.WriteLine(int.Parse(ioSize) + "," + Int64.Parse(numCommands));
                            }
                        }
                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: CommandVsBlocksize", e.Message), e);
            }
        }

        private void calculateCmdCountPerIoSize(StreamReader reader, Hashtable hashtable)
    {
       
        hashIoSize(reader, hashtable);
    }

        private void hashIoSize(StreamReader reader, Hashtable hashtable)
        {
            int index = 0;
            int hashIndex = 0;
            int value;
            ulong sumIoSize = 0;

            mIoSize.Clear();
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                string[] tokens = line.Split(' ');
                mIoSize.Add(int.Parse(tokens[1]));
                hashIndex = mIoSize.ElementAt(index);
                sumIoSize += Convert.ToUInt64(hashIndex);
                if (hashtable.ContainsKey(hashIndex))
                {
                    value = (int)hashtable[hashIndex];
                    value++;
                    hashtable[hashIndex] = value;
                }
                else
                {
                    hashtable.Add(hashIndex, 1);
                }
                index++;
            }
            
            /*Find Average IoSize */
            decimal avgIoSize;
            avgIoSize = (decimal)((decimal)sumIoSize / (decimal)mIoSize.Count());
            int multiplier = (int)Math.Ceiling((decimal)(avgIoSize / (decimal)512));
            mAvgIoSize = multiplier * 512;
            reader.Close();
        }
        List<int> mIoSize  = new List<int>();

        const string MULTISIM_IO ="Multi Sim based on IO Size";
        const string MULTISIM_QD = "Multi Sim based on QD";

        private void CommandVsIOSize(bool multiSim)
        {

            setupChart11();
            try
            {
                using (FileStream file = new FileStream(@".\Reports\CmdVsIOSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOSize" + "," + "Number of Commands");
                        if (multiSim)
                        {
                            switch(simTypeBox.Text)
                            {
                                case MULTISIM_IO:
                                    plotIoSize_msIO(writer);
                                        break;
                                case MULTISIM_QD:
                                        plotIoSize_msQD(writer);
                                    break;

                            }//switch
                           
                        }
                        else
                        {
                            plotIoSize_SingleSim(writer);
                        }
                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: CommandVs IOSize", e.Message), e);
            }
        }

        private void setupChart11()
        {
            cmdCountVsIOSizeChart.Series.Clear();
            cmdCountVsIOSizeChart.Series.Add("Number Of Commands");
            cmdCountVsIOSizeChart.Titles[0].Text = "Command Count Vs IO Size";
            cmdCountVsIOSizeChart.ChartAreas[0].AxisX.Title = "IO Size";
            cmdCountVsIOSizeChart.ChartAreas[0].AxisY.Title = "Number Of Commands";
            cmdCountVsIOSizeChart.ChartAreas[0].AxisX.Minimum = 0;
            cmdCountVsIOSizeChart.ChartAreas[0].AxisX.Maximum = 8704;
            cmdCountVsIOSizeChart.ChartAreas[0].AxisX.Interval = 1024;
            cmdCountVsIOSizeChart.ChartAreas[0].AxisY.Minimum = 0;
            //chart11.ChartAreas[0].AxisY.Maximum = Int64.Parse(numCommands) + 1;

            cmdCountVsIOSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            cmdCountVsIOSizeChart.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 20";
            cmdCountVsIOSizeChart.Series[0].Color = Color.Blue;
            cmdCountVsIOSizeChart.Series[0].Name = "Number Of Commands";
        }

        private void plotIoSize_SingleSim(StreamWriter writer)
        {
            if (enableWrkld == "1")
            {
                cmdCountVsIOSizeChart.Titles[0].Text = "Command Count Vs IO Size";
                cmdCountVsIOSizeChart.ChartAreas[0].AxisX.Title = "IO Size";
                Hashtable hashtable = new Hashtable();
                using (FileStream file1 = new FileStream(@".\Reports\iosize_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(file1))
                    {
                        calculateCmdCountPerIoSize(reader, hashtable);
                    }
                    file1.Close();
                }
                ICollection key = hashtable.Keys;
                foreach (Int32 k in key)
                {
                    cmdCountVsIOSizeChart.Series[0].Points.AddXY(k, hashtable[k]);

                }

            }
            else
            {
                cmdCountVsIOSizeChart.Series[0].Points.AddXY(int.Parse(ioSize), Int64.Parse(numCommands));
                writer.WriteLine(int.Parse(ioSize) + "," + Int64.Parse(numCommands));
            }
        }

        private void plotIoSize_msBS(StreamWriter writer)
        {
            if (enableWrkld == "1")
            {

                cmdCountVsIOSizeChart.Titles[0].Text = "Command Count Vs IO Size";
                cmdCountVsIOSizeChart.ChartAreas[0].AxisX.Title = "IO Size";
                Hashtable hashtable = new Hashtable();
                int qIndex = 0;
                switch(int.Parse(mValidIoParam))
                {
                    case 512: qIndex = 4; break;
                    case 1024: qIndex = 3; break;
                    case 2048: qIndex = 2; break;
                    case 4096: qIndex = 1; break;
                    default: qIndex = 0; break;
                }
                using (FileStream file1 = new FileStream(@".\Reports\iosize_report_iosize_" + mValidIoParam + "_blksize_" + mValidIoParam + "_qd_" + validQDepth[qIndex] + ".log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(file1))
                    {
                        calculateCmdCountPerIoSize(reader, hashtable);
                    }
                    file1.Close();
                }
                ICollection key = hashtable.Keys;
                foreach (Int32 k in key)
                {
                    cmdCountVsIOSizeChart.Series[0].Points.AddXY(k, hashtable[k]);
                }
            }
            else
            {
                cmdCountVsIOSizeChart.Series[0].Points.AddXY(int.Parse(ioSize), Int64.Parse(numCommands));
                writer.WriteLine(int.Parse(ioSize) + "," + Int64.Parse(numCommands));
            }
        }
        private void plotblkSize_msBS(StreamWriter writer)
        {
            cmdCountVsIOSizeChart.Titles[0].Text = "Command Count Vs Block Size";
            cmdCountVsIOSizeChart.ChartAreas[0].AxisX.Title = "Block Size";
            if (enableWrkld == "1")
            {
                Hashtable hashtable = new Hashtable();
                int qIndex = 0;
                switch (int.Parse(mValidIoParam))
                {
                    case 512: qIndex = 4; break;
                    case 1024: qIndex = 3; break;
                    case 2048: qIndex = 2; break;
                    case 4096: qIndex = 1; break;
                    default: qIndex = 0; break;
                }
                using (FileStream file1 = new FileStream(@".\Reports\iosize_report_iosize_" + mValidIoParam + "_blksize_" + mValidIoParam + "_qd_" + validQDepth[qIndex] + ".log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(file1))
                    {
                        calculateCmdCountPerIoSize(reader, hashtable);
                    }
                    file1.Close();
                }
                ICollection key = hashtable.Keys;
                foreach (Int32 k in key)
                {
                    cmdCountVsIOSizeChart.Series[0].Points.AddXY(k, hashtable[k]);
                }
            }
            else
            {
                cmdCountVsIOSizeChart.Series[0].Points.AddXY(int.Parse(mValidIoParam), Int64.Parse(numCommands));
                writer.WriteLine(int.Parse(mValidIoParam) + "," + Int64.Parse(numCommands));
            }
        }
        private void plotIoSize_msQD(StreamWriter writer)
        {
            if (enableWrkld == "1")
            {
                cmdCountVsIOSizeChart.Titles[0].Text = "Command Count Vs IO Size";
                cmdCountVsIOSizeChart.ChartAreas[0].AxisX.Title = "IO Size";
                Hashtable hashtable = new Hashtable();

                using (FileStream file1 = new FileStream(@".\Reports\iosize_report_iosize_" + blkSize.Trim() + "_blksize_" + blkSize.Trim() + "_qd_" + validQueueDepth[0] + ".log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(file1))
                    {
                        calculateCmdCountPerIoSize(reader, hashtable);
                    }
                    file1.Close();
                }
                ICollection key = hashtable.Keys;
                foreach (Int32 k in key)
                {
                    cmdCountVsIOSizeChart.Series[0].Points.AddXY(k, hashtable[k]);
                }
            }
            else
            {
                cmdCountVsIOSizeChart.Series[0].Points.AddXY(int.Parse(ioSize), Int64.Parse(numCommands));
                writer.WriteLine(int.Parse(ioSize) + "," + Int64.Parse(numCommands));
            }
        }

        private void plotblkSize_msQD(StreamWriter writer)
        {
            if (enableWrkld == "1")
            {
                cmdCountVsIOSizeChart.Titles[0].Text = "Command Count Vs Block Size";
                cmdCountVsIOSizeChart.ChartAreas[0].AxisX.Title = "Block Size";
                Hashtable hashtable = new Hashtable();

                using (FileStream file1 = new FileStream(@".\Reports\iosize_report_iosize_" + blkSize.Trim() + "_blksize_" + blkSize.Trim() + "_qd_" + validQueueDepth[0] + ".log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(file1))
                    {
                        calculateCmdCountPerIoSize(reader, hashtable);
                    }
                    file1.Close();
                }
                ICollection key = hashtable.Keys;
                foreach (Int32 k in key)
                {
                    cmdCountVsIOSizeChart.Series[0].Points.AddXY(k, hashtable[k]);
                }
            }
            else
            {
                cmdCountVsIOSizeChart.Series[0].Points.AddXY(int.Parse(blkSize), Int64.Parse(numCommands));
                writer.WriteLine(int.Parse(blkSize) + "," + Int64.Parse(numCommands));
            }
        }
        private void plotIoSize_msIO(StreamWriter writer)
        {
            //foreach (string io in validIOSize)
            //{
                cmdCountVsIOSizeChart.Series[0].Points.AddXY(int.Parse(mValidIoParam), Int64.Parse(numCommands));
                writer.WriteLine(int.Parse(mValidIoParam) + "," + Int64.Parse(numCommands));
            //}
        }

      
        private void plotCmdVsLBA(string fileName)
        {
            try
            {
                using (FileStream file = new FileStream(@".\Reports\CmdVsLBA.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("LBA" + "," + "Number of Commands");
                        try
                        {
                            using (FileStream file2 = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader reader = new StreamReader(file2))
                                {

                                    List<int> lba = new List<int>();
                                    List<double> time = new List<double>();
                                    List<string> range = new List<string>();

                                    Hashtable hashtable = new Hashtable();

                                    while (reader.Peek() != -1)
                                    {
                                        string line = reader.ReadLine();
                                        string[] tokens = line.Split(' ');

                                        time.Add(double.Parse(tokens[0]));
                                        lba.Add(int.Parse(tokens[1]));
                                        writer.WriteLine(int.Parse(tokens[1]) + "," + 1);
                                    }

                                    reader.Close();

                                    int count = 0;
                                    int binRange = 0;
                                    int lbaMax = lba.Max();

                                    if (lba.Max() < 1024)
                                    {
                                        binRange = 1;
                                        cmdCountVsLBAChart.Series.Add(new Series());
                                        int hashIndex = 0;
                                        int value = 0;
                                        for (int index = 0; index < lba.Count(); index++)
                                        {
                                            hashIndex = lba.ElementAt(index);
                                            if (hashtable.ContainsKey(hashIndex))
                                            {
                                                value = (int)hashtable[hashIndex];
                                                value++;
                                                hashtable[hashIndex] = value;
                                            }
                                            else
                                            {
                                                hashtable.Add(hashIndex, 1);

                                            }
                                        }
                                        cmdCountVsLBAChart.Series.Clear();
                                        cmdCountVsLBAChart.Series.Add(new Series());
                                        cmdCountVsLBAChart.Series[0].IsVisibleInLegend = false;
                                        cmdCountVsLBAChart.ChartAreas[0].AxisX.Title = "LBA";
                                        cmdCountVsLBAChart.ChartAreas[0].AxisY.Title = "Number Of Commands";
                                        cmdCountVsLBAChart.ChartAreas[0].AxisY.Minimum = 0;
                                        cmdCountVsLBAChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                                        cmdCountVsLBAChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                                        cmdCountVsLBAChart.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 5";
                                        ICollection key = hashtable.Keys;
                                        cmdCountVsLBAChart.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                                        cmdCountVsLBAChart.ChartAreas[0].AxisX.CustomLabels.Clear();
                                        foreach (Int32 k in key)
                                        {
                                            cmdCountVsLBAChart.Series[0].Points.AddXY(k, hashtable[k]);
                                           // writer.WriteLine(k + "," + hashtable[k]);
                                        }
                                    }
                                    else
                                    {
                                        binRange = 1024;
                                        count = lba.Max() / binRange;
                                        if (lba.Max() % binRange != 0)
                                        {
                                            count++;
                                        }
                                        for (int index = 0; index < count; index++)
                                        {
                                            hashtable.Add(binRange * (index + 1), 0);

                                        }
                                        double hashIndex = 0;
                                        int value = 0;
                                        for (int lbaIndex = 0; lbaIndex < lba.Count(); lbaIndex++)
                                        {
                                            hashIndex = lba.ElementAt(lbaIndex) / binRange;
                                            hashIndex = Math.Floor(hashIndex);
                                            if (hashIndex == 0)
                                            {
                                                value = (int)hashtable[binRange];
                                                value++;
                                                hashtable[binRange] = value;
                                            }
                                            else
                                            {
                                                value = (int)hashtable[binRange * (int)(hashIndex)];
                                                value++;
                                                hashtable[binRange * (int)(hashIndex)] = value;
                                            }
                                        }

                                        cmdCountVsLBAChart.Update();
                                        cmdCountVsLBAChart.Series.Clear();
                                        cmdCountVsLBAChart.ChartAreas[0].AxisX.Title = "LBA";
                                        cmdCountVsLBAChart.ChartAreas[0].AxisY.Title = "Number Of Commands";
                                        cmdCountVsLBAChart.ChartAreas[0].AxisX.Minimum = 0;
                                        cmdCountVsLBAChart.ChartAreas[0].AxisY.Minimum = 0;
                                        cmdCountVsLBAChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                                        cmdCountVsLBAChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                                        int rangeIndex = 0;

                                        ICollection key = hashtable.Keys;

                                        rangeIndex = 0;
                                        Int32 upperIndex = 0;
                                        Int32 lowerIndex = 0;
                                        cmdCountVsLBAChart.ChartAreas[0].AxisX.CustomLabels.Clear();
                                        range.Clear();

                                        foreach (Int32 k in key)
                                        {
                                            if ((int)hashtable[k] != 0)
                                            {
                                                if (k <= binRange)
                                                {
                                                    lowerIndex = 0;
                                                    upperIndex = 1;
                                                }
                                                else
                                                {
                                                    lowerIndex = k / binRange;
                                                    upperIndex = (k + binRange) / binRange;
                                                }
                                                range.Add(String.Concat(lowerIndex.ToString() + "K" + "-" + upperIndex.ToString() + "K"));
                                                cmdCountVsLBAChart.ChartAreas[0].AxisX.CustomLabels.Add(k / binRange - 1, k / binRange + 1, range[rangeIndex]);
                                                cmdCountVsLBAChart.Series.Add(range[rangeIndex]);
                                                cmdCountVsLBAChart.Series[range[rangeIndex]].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                                                cmdCountVsLBAChart.Series[range[rangeIndex]].Color = Color.Green;
                                                cmdCountVsLBAChart.Series[range[rangeIndex]].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                                                cmdCountVsLBAChart.Series[range[rangeIndex]].Points.AddXY(k / binRange, hashtable[k]);
                                             //   writer.WriteLine(k / binRange + "," + hashtable[k]);
                                                cmdCountVsLBAChart.Series[range[rangeIndex]].IsVisibleInLegend = false;
                                                cmdCountVsLBAChart.Series[range[rangeIndex]].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 20";
                                                rangeIndex++;
                                            }
                                        }
                                    }
                                    file2.Close();
                                }
                            }
                        }

                        catch (IOException e)
                        {
                            throw new Exception(String.Format("An error ocurred while executing the data import: Transaction Report", e.Message), e);
                        }
                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
            }
        }

        private void CommandVsLBA(bool multiSim)
        {
            
            string fileName= "";
            try
            {
                if (multiSim)
                {
                    string param = validParam.Trim();
                    if (simTypeBox.Text == "Multi Sim based on Block Size")
                    {
                        int qIndex = 0;
                        switch(param)
                        {
                            case "512":
                            qIndex = validQDepth.Count() - 1;
                            break;
                            case "1024":
                            qIndex = validQDepth.Count() - 2;
                            break;
                            case "2048":
                            {
                                qIndex = validQDepth.Count() - 3;
                                break;
                            }
                            case "4096":
                            {
                                qIndex = validQDepth.Count() - 4;
                                break;
                            }
                            case "8192":
                            {
                                qIndex = validQDepth.Count() - 5;
                                break;
                            }
                        }
                        if (enableWrkld == "1")
                        {
                            fileName = ".\\Reports\\lba_report_" + "iosize_" + param + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                        }
                        else
                        {
                            fileName = ".\\Reports\\lba_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                        }
                    }
                    else if (simTypeBox.Text == "Multi Sim based on QD")
                    {
                        if (enableWrkld == "1")
                        {
                            fileName = ".\\Reports\\lba_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.Trim() +"_" + "qd_" + param + ".log";
                        }
                        else
                        {
                            fileName = ".\\Reports\\lba_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + param + ".log";
                        }
                    }
                    else
                    {
                        int qIndex = 0;
                        switch (param)
                        {
                            case "512":
                                qIndex = validQDepth.Count() - 1;
                                break;
                            case "1024":
                                qIndex = validQDepth.Count() - 2;
                                break;
                            case "2048":
                                {
                                    qIndex = validQDepth.Count() - 3;
                                    break;
                                }
                            case "4096":
                                {
                                    qIndex = validQDepth.Count() - 4;
                                    break;
                                }
                            case "8192":
                                {
                                    qIndex = validQDepth.Count() - 5;
                                    break;
                                }
                        }
                        fileName = ".\\Reports\\lba_report_" + "iosize_" + param + "_" + "blksize_" + validBlockSize[0] + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";

                    }
                   
                }
                else
                {
                    fileName = ".\\Reports\\lba_report.log";
                    
                }
                plotCmdVsLBA(fileName);
            }

            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: LBA Report", e.Message), e);
            }
        }

        private void channelUtilizationData()
        {
            channelUtilChart.Update();
            if (channelUtilChart.Series.Count != 0)
                channelUtilChart.Series.Clear(); 
            channelUtilChart.Series.Add(new Series());
            channelUtilChart.Series[0].IsVisibleInLegend = false;
            channelUtilChart.ChartAreas[0].AxisX.Title = "Channels";
            channelUtilChart.ChartAreas[0].AxisY.Title = "No. Of References (CodeWord)";
            channelUtilChart.ChartAreas[0].AxisY.Maximum = chanTransCount.Max();
            int chanLabel = 0;
            for (int chanIndex = 1; chanIndex < (int.Parse(numChannel) + 1); chanIndex++)
            {
                chanLabel = chanIndex - 1;
                channelUtilChart.ChartAreas[0].AxisX.CustomLabels.Add(chanIndex - 1.0, chanIndex + 1.0, chanLabel.ToString());
                channelUtilChart.Series[0].Points.AddY(chanTransCount.ElementAt((chanIndex -1)));
                channelUtilChart.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 20";
            }
        }

        //private void shortQueueUtilizationData()
        //{
        //    try
        //    {

        //        using (FileStream file = new FileStream(@".\Reports\shortQueue_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //        {
        //            using (StreamReader reader = new StreamReader(file))
        //            {
        //                if (reader.Peek() != -1)
        //                {

        //                    List<int> channel = new List<int>();
        //                    List<double> time = new List<double>();
        //                    List<Int16> queueSize = new List<Int16>();

        //                    while (reader.Peek() != -1)
        //                    {
        //                        string line = reader.ReadLine();
        //                        string[] tokens = line.Split(' ');

        //                        time.Add(double.Parse(tokens[0]));
        //                        channel.Add(int.Parse(tokens[1]));
        //                        queueSize.Add(Int16.Parse(tokens[2]));
        //                    }

        //                    reader.Close();
        //                    if (queueSize.Count != 0)
        //                    {
        //                        int last = time.Count - 1;
        //                        int first = 0;
        //                        chart3.Update();

        //                        if (timeScale.ToString() == "us")
        //                        {
        //                            timeScaleVar = 1000;
        //                            chart3.ChartAreas[0].AxisX.Minimum = (time[first] / timeScaleVar);
        //                            chart3.ChartAreas[0].AxisX.Maximum = (time[last] / timeScaleVar);
        //                            chart3.ChartAreas[0].AxisX.Title = "Time (us)";
        //                        }
        //                        else if (timeScale.ToString() == "ms")
        //                        {
        //                            timeScaleVar = 1000000;
        //                            chart3.ChartAreas[0].AxisX.Minimum = (time[first] / timeScaleVar);
        //                            chart3.ChartAreas[0].AxisX.Maximum = (time[last] / timeScaleVar);
        //                            chart3.ChartAreas[0].AxisX.Title = "Time (ms)";
        //                        }
        //                        chart3.ChartAreas[0].AxisY.Title = "Queue Size";
        //                        chart3.ChartAreas[0].AxisY.Maximum = cwNum;
        //                        if (chart3.Series.Count != 0)
        //                            chart3.Series.Clear();

        //                        if (chart3.Legends.Count != 0)
        //                            chart3.Legends.Clear();
        //                        chart3.Legends.Add("Channels");
        //                        chart3.Legends["Channels"].Title = "Channels";

        //                        for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
        //                        {
        //                            chart3.Series.Add(chanIndex.ToString());
        //                            chart3.Series[chanIndex.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
        //                        }

        //                        for (int index = 0; index < time.Count; index++)
        //                        {
        //                            chart3.Series[channel.ElementAt(index).ToString()].Points.AddXY(time[index] / timeScaleVar, queueSize.ElementAt(index));
        //                        }
        //                    }
        //                }

        //            }
        //            file.Close();
        //        }
                    
        //    }

        //    catch (IOException e)
        //    {   
        //        throw new Exception(String.Format("An error occurred while executing the data import: shortQueue Report", e.Message), e);
        //    }
        //}

        //private void longQueueUtilizationData()
        //{

        //    try
        //    {
        //        using (FileStream file = new FileStream(@".\Reports\longQueue_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //        {
        //            using (StreamReader reader = new StreamReader(file))
        //            {

        //                List<int> channel = new List<int>();
        //                List<double> time = new List<double>();
        //                List<Int16> queueSize = new List<Int16>();

        //                if (reader.Peek() != -1)
        //                {
        //                   while (reader.Peek() != -1)
        //                    {
        //                        string line = reader.ReadLine();
        //                        string[] tokens = line.Split(' ');

        //                        time.Add(double.Parse(tokens[0]));
        //                        channel.Add(int.Parse(tokens[1]));
        //                        queueSize.Add(Int16.Parse(tokens[2]));
        //                    }

        //                    reader.Close();
        //                    int last = time.Count - 1;
        //                    int first = 0;
        //                    chart5.Update();

        //                    if (timeScale.ToString() == "us")
        //                    {
        //                        timeScaleVar = 1000;
        //                        chart5.ChartAreas[0].AxisX.Minimum = (time[first] / timeScaleVar);
        //                        chart5.ChartAreas[0].AxisX.Maximum = (time[last] / timeScaleVar);
        //                        chart5.ChartAreas[0].AxisX.Title = "Time (us)";
        //                    }
        //                    else if (timeScale.ToString() == "ms")
        //                    {
        //                        timeScaleVar = 1000000;
        //                        chart5.ChartAreas[0].AxisX.Minimum = (time[first] / timeScaleVar);
        //                        chart5.ChartAreas[0].AxisX.Maximum = (time[last] / timeScaleVar);
        //                        chart5.ChartAreas[0].AxisX.Title = "Time (ms)";
        //                    }
        //                    chart5.ChartAreas[0].AxisY.Title = "Queue Size";
        //                    chart5.ChartAreas[0].AxisY.Maximum = cwNum;// double.Parse(queueDepth);

        //                    if(chart5.Series.Count != 0)
        //                    chart5.Series.Clear();

        //                    if (chart5.Legends.Count != 0)
        //                    chart5.Legends.Clear();
        //                    chart5.Legends.Add("Channels");
        //                    chart5.Legends["Channels"].Title = "Channels";

        //                    for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
        //                    {
        //                        chart5.Series.Add(chanIndex.ToString());
        //                        chart5.Series[chanIndex.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
        //                    }

        //                    for (int index = 0; index < time.Count; index++)
        //                    {
        //                        chart5.Series[channel.ElementAt(index).ToString()].Points.AddXY(time[index] / timeScaleVar, queueSize.ElementAt(index));
        //                    }

        //                }
        //            }
        //            file.Close();
        //        }
        //    }

        //    catch (IOException e)
        //    {
        //        throw new Exception(String.Format("An error occurred while executing the data import: Long Queue Utilization Report", e.Message), e);
        //    }
        //}
        double simTime = 0;
        private void plotDDR(string fileName)
        {
            
            if (ddr_onfiBusUtilChart.Series.Count != 0)
                ddr_onfiBusUtilChart.Series.Clear();
            List<double> busUtil = new List<double>();
            List<double> time = new List<double>();
             try
                {
                    using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        parseDDRBusUtilStream(busUtil, time, file);
                            List<double> busUtilPercent = new List<double>();
                            double avgBusUtil = 0;
                            double effectiveBusUtil = 0;
                            double totalBusTime = 0;
                            double effectiveBusTime = 0;
                            double commandTime = (double)8000/ (double)(2* int.Parse(ddrSpeed));// (double)(2 * COMMAND_BL * 1000) / (double)(2 * mDdrSpeed);
                            foreach (double t in busUtil)
                            {
                                totalBusTime += t;
                                if (t > commandTime)
                                {
                                    effectiveBusTime += t;
                                }
                            }
                            avgBusUtil = (double)((double)totalBusTime / time.Max()) * 100;
                            effectiveBusUtil = (double)((double)effectiveBusTime / time.Max()) *100;

                            
                            
                            ddr_onfiBusUtilChart.Series.Add(new Series("DATA"));
                            ddr_onfiBusUtilChart.Series[0].IsVisibleInLegend = false;
                            ddr_onfiBusUtilChart.Series[0].ChartType = SeriesChartType.FastPoint;
                            ddr_onfiBusUtilChart.Series[0].Color = Color.Brown;

                            ddr_onfiBusUtilChart.Series.Add(new Series("CMD"));
                            ddr_onfiBusUtilChart.Series[1].IsVisibleInLegend = false;
                            ddr_onfiBusUtilChart.Series[1].ChartType = SeriesChartType.FastPoint;
                            ddr_onfiBusUtilChart.Series[1].Color = Color.Gray;   
                            
                            
                            setDDRChartLegend(avgBusUtil, effectiveBusUtil);
                            
                                                     
                            
                           int chartSize = (int)(time.Max() + busUtil.ElementAt(busUtil.Count() -1));
                           int numCmd = int.Parse(numCommands.Trim());
                           if (numCmd > 3)
                           {
                          
                               chartSize /= 2;
                               if(chartSize < 1068)
                               {
                                   chartSize = 1068;
                               }
                               ddr_onfiBusUtilChart.Width = chartSize;
                        
                           }
                           else
                           {
                               ddr_onfiBusUtilChart.Width = 1068;
                           }
                           ddr_onfiBusUtilChart.ChartAreas[0].Position.Y = 10;
                           ddr_onfiBusUtilChart.ChartAreas[0].Position.Height = 80;
                           ddr_onfiBusUtilChart.ChartAreas[0].Position.X = 1;
                           ddr_onfiBusUtilChart.ChartAreas[0].Position.Width = 99;
                                                      
                           ddr_onfiBusUtilChart.ChartAreas[0].AxisY.LabelStyle.IsEndLabelVisible = false;
                           ddr_onfiBusUtilChart.ChartAreas[0].AxisY.CustomLabels.Clear();
                           ddr_onfiBusUtilChart.ChartAreas[0].AxisY.CustomLabels.Add(int.Parse(numChannel) + 39, int.Parse(numChannel) + 42, "DDR");
                           //chart13.ChartAreas[0].AxisX.IntervalAutoMode = false;
                           ddr_onfiBusUtilChart.ChartAreas[0].AxisX.Interval = 500;


                           lockChart13Position();
                            
                            ddr_onfiBusUtilChart.ChartAreas[0].AxisY.Minimum = 0;
                            ddr_onfiBusUtilChart.ChartAreas[0].AxisX.Minimum = 0;
                            ddr_onfiBusUtilChart.ChartAreas[0].AxisX.Maximum = time.Max() + busUtil.ElementAt(busUtil.Count() - 1);
                            simTime = time.Max();
                                                        
                            for (int timeIndex = 0; timeIndex < time.Count(); timeIndex++)
                            {
                                double timeAxis = time.ElementAt(timeIndex);
                                
                                for (int busIndex = 0; busIndex < busUtil.ElementAt(timeIndex); busIndex++)
                                {
                                    if (busUtil.ElementAt(timeIndex) > 5)
                                    {
                                        ddr_onfiBusUtilChart.Series["DATA"].Points.AddXY(timeAxis, int.Parse(numChannel) + 40);
                                    }
                                    else
                                    {
                                        ddr_onfiBusUtilChart.Series["CMD"].Points.AddXY(timeAxis, int.Parse(numChannel) + 40);
                                    }
                                    timeAxis++;
                                }
                                
                            }
                               
                        //}
                        file.Close();
                    }
                }

                catch (IOException e)
                {
                    throw new Exception(String.Format("An error occurred while executing the data import: DDR Utilization Report", e.Message), e);
                }
        }

        private void lockChart13Position()
        {
            ChartArea ca = ddr_onfiBusUtilChart.ChartAreas[0];
            ElementPosition cap = ca.Position;
            ElementPosition ipp = ca.InnerPlotPosition;
            ipp.Width = 70;
            ipp.Height = 80;

            int ippX = 80;//inner plot X axis pixel position                          
            int caX = 60;//chartArea X axis pixel position
            //convert pixel to percentage
            int capWidth = getChartAreaWidthPercent(cap);
            float newIppX = getXAxisLocation(ippX, capWidth);
            float newCaX = getXAxisLocation(caX, ddr_onfiBusUtilChart.ClientSize.Width);
            ipp.X = newIppX;
            ca.Position = new ElementPosition(newCaX, cap.Y, cap.Width, cap.Height);
        }

        private static float getXAxisLocation(int numerator, int denominator)
        {
            return (float)(numerator * 100) / denominator;
        }

        private int getChartAreaWidthPercent(ElementPosition cap)
        {
            return (int)((cap.Width * (float)ddr_onfiBusUtilChart.ClientSize.Width) / 100f);
        }

        private void setDDRChartLegend(double avgBusUtil, double effectiveBusUtil)
        {
            ddr_onfiBusUtilChart.Legends.Clear();
            ddr_onfiBusUtilChart.Legends.Add(new Legend());
            ddr_onfiBusUtilChart.Legends[0].CustomItems.Add(new LegendItem());
            ddr_onfiBusUtilChart.Legends[0].CustomItems[0].Cells.Add(new LegendCell("  "));
            ddr_onfiBusUtilChart.Legends[0].CustomItems[0].Cells.Add(new LegendCell("Actual"));
            ddr_onfiBusUtilChart.Legends[0].CustomItems[0].Cells.Add(new LegendCell("Effective"));
            ddr_onfiBusUtilChart.Legends[0].CustomItems.Add(new LegendItem());
            ddr_onfiBusUtilChart.Legends[0].CustomItems[1].Cells.Add(new LegendCell("DDR"));
            ddr_onfiBusUtilChart.Legends[0].CustomItems[1].Cells[0].Font = new Font("Arial", 8, System.Drawing.FontStyle.Bold);
            ddr_onfiBusUtilChart.Legends[0].CustomItems[1].Cells.Add(new LegendCell(Math.Round(avgBusUtil, 2).ToString()));
            ddr_onfiBusUtilChart.Legends[0].CustomItems[1].Cells.Add(new LegendCell(Math.Round(effectiveBusUtil, 2).ToString()));
            ddr_onfiBusUtilChart.Legends[0].CustomItems.Add(new LegendItem());
            ddr_onfiBusUtilChart.Legends[0].CustomItems[2].Cells.Add(new LegendCell(""));
        }

        private static void parseDDRBusUtilStream(List<double> busUtil, List<double> time, FileStream file)
        {
            try
            {
                using (StreamReader reader = new StreamReader(file))
                {

                    while (reader.Peek() != -1)
                    {
                        string line = reader.ReadLine();
                        string[] tokens = line.Split(' ');

                        time.Add(double.Parse(tokens[0]));
                        busUtil.Add(double.Parse(tokens[1]));
                    }
                    reader.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the stream reading: DDR Utilization Report", e.Message), e);
            }
        }

        private void DDRBusUtilizationData(bool multiSim)
        {
            string fileName = "";
            if (multiSim)
            {
                
               // chart13.Series.Clear();
                string param = validMultiSimParam.Trim();
                if (simTypeBox.Text == "Multi Sim based on Block Size")
                {
                    int qIndex = 0;
                    switch (param)
                    {
                        case "512":
                            qIndex = validQDepth.Count() - 1;
                            break;
                        case "1024":
                            qIndex = validQDepth.Count() - 2;
                            break;
                        case "2048":
                            {
                                qIndex = validQDepth.Count() - 3;
                                break;
                            }
                        case "4096":
                            {
                                qIndex = validQDepth.Count() - 4;
                                break;
                            }
                        case "8192":
                            {
                                qIndex = validQDepth.Count() - 5;
                                break;
                            }
                    }
                    if (enableWrkld == "1")
                    {
                        fileName = ".\\Reports\\DDR_bus_utilization_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + param.Trim() + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                    }
                    else
                    {
                        fileName = ".\\Reports\\DDR_bus_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + param.Trim() + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                    }        
                }
                else if (simTypeBox.Text == "Multi Sim based on QD")
                {
                    if (enableWrkld == "1")
                    {
                        fileName = ".\\Reports\\DDR_bus_utilization_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.ToString().Trim() + "_" + "qd_" + param.Trim() + ".log";
                    }
                    else
                    {
                        fileName = ".\\Reports\\DDR_bus_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.ToString().Trim() + "_" + "qd_" + param.Trim() + ".log";
                    }
                }
                else
                {
                    int qIndex = 0;
                    switch (param)
                    {
                        case "512":
                            qIndex = validQDepth.Count() - 1;
                            break;
                        case "1024":
                            qIndex = validQDepth.Count() - 2;
                            break;
                        case "2048":
                            {
                                qIndex = validQDepth.Count() - 3;
                                break;
                            }
                        case "4096":
                            {
                                qIndex = validQDepth.Count() - 4;
                                break;
                            }
                        case "8192":
                            {
                                qIndex = validQDepth.Count() - 5;
                                break;
                            }
                    }
                    fileName = ".\\Reports\\DDR_bus_utilization_" + "iosize_" + param + "_" + "blksize_" + validBlockSize[0] + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                }
            }
            else
            {
                fileName = ".\\Reports\\DDR_bus_utilization.log";
            }
            plotDDR(fileName);    
        }

        private void plotONFI(string fileName1, string fileName2)
        {
            try
                {
                    List<double> startTime = new List<double>();
                    List<int> channelNum = new List<int>();
                    List<double> endTime = new List<double>();
                    List<double>[] busUtilPerChannel = new List<double>[int.Parse(numChannel)];
                    List<double>[] timePerChannel = new List<double>[int.Parse(numChannel)];
                   // Hashtable timeWindowValue = new Hashtable();

                    Hashtable totalTime = new Hashtable();
                    using (FileStream file1 = new FileStream(fileName1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader1 = new StreamReader(file1))
                        {
                            while (reader1.Peek() != -1)
                            {
                                string line = reader1.ReadLine();
                                string[] tokens = line.Split(' ');
                                channelNum.Add(int.Parse(tokens[0]));
                                startTime.Add(double.Parse(tokens[1]));
                                endTime.Add(double.Parse(tokens[2]));
                            }
                            reader1.Close();
                            for (int chanIndex = 0; chanIndex < channelNum.Count(); chanIndex++)
                            {
                                busUtilPerChannel[channelNum[chanIndex]] = new List<double>();
                                timePerChannel[channelNum[chanIndex]] = new List<double>();
                            }

                        }
                        file1.Close();
                    }

                    using (FileStream file = new FileStream(fileName2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {


                        using (StreamReader reader = new StreamReader(file))
                        {
                            List<double> busUtil = new List<double>();
                            List<double> time = new List<double>();
                            List<int> channel = new List<int>();
                            List<double> avgONFIUtil = new List<double>();
                            List<double> effectiveUtil = new List<double>();
                            while (reader.Peek() != -1)
                            {
                                string line = reader.ReadLine();            
                                string[] tokens = line.Split(' ');

                                time.Add(double.Parse(tokens[0]));
                                channel.Add(int.Parse(tokens[1]));
                                busUtil.Add(double.Parse(tokens[2]));
                            }

                            reader.Close();
                            for (int chanIndex = 0; chanIndex < channel.Count(); chanIndex++)
                            {
                                busUtilPerChannel[channel[chanIndex]].Add(busUtil[chanIndex]);
                                timePerChannel[channel[chanIndex]].Add(time[chanIndex]);
                            }
                            for (int chanIndex = 0; chanIndex < channelNum.Count(); chanIndex++)
                            {
                               
                                if ((double)busUtilPerChannel[channelNum[chanIndex]].Count() != 0)
                                {
                                    double maxTime = endTime[channelNum[chanIndex]];
                                    double minTime = startTime[channelNum[chanIndex]];
                                    totalTime.Add(channelNum[chanIndex], maxTime);
                                    
                                    double totalBusTime =0;
                                    double effectiveBusTime = 0;
                                    foreach (double t in busUtilPerChannel[channelNum[chanIndex]])
                                    {
                                        totalBusTime += t;
                                        if (t == busUtilPerChannel[channelNum[chanIndex]].Max())
                                        {
                                            effectiveBusTime += t;
                                        }
                                    }
                                    if ((double)totalTime[channelNum[chanIndex]] != 0)
                                    {
                                        avgONFIUtil.Add((totalBusTime / endTime.Max()) * 100);//(double)totalTime[channelNum[chanIndex]]) * 100);
                                        effectiveUtil.Add(((double)effectiveBusTime / endTime.Max()) * 100);//(double)totalTime[channelNum[chanIndex]]) * 100);
                                    }
                                    else
                                    {
                                        avgONFIUtil.Add(0);
                                        effectiveUtil.Add(0);
                                    }

                                }
                                else
                                {
                                    avgONFIUtil.Add(0);
                                    effectiveUtil.Add(0);
                                }
                            }
                          
                            //chart13.ChartAreas[0].AxisY.CustomLabels.Add(channelNum.Count() + 3, channelNum.Count() + 4, "ONFI");
                          //  chart13.ChartAreas[0].InnerPlotPosition.Width = true;
                            ddr_onfiBusUtilChart.SuspendLayout();
                            for (int chanIndex = 0; chanIndex < channelNum.Count(); chanIndex++)
                            {
                                ddr_onfiBusUtilChart.Series.Add(new Series());
                            }
                            int lowLimit = 2;
                            int upLimit = 4;
                            for (int chanIndex = 0; chanIndex < channelNum.Count(); chanIndex++)
                            {
                               
                                ddr_onfiBusUtilChart.Legends[0].CustomItems.Add(new LegendItem());
                                ddr_onfiBusUtilChart.Legends[0].CustomItems[chanIndex + 3].Cells.Add(new LegendCell("ch " + channelNum[chanIndex].ToString()));
                                ddr_onfiBusUtilChart.Legends[0].CustomItems[chanIndex + 3].Cells.Add(new LegendCell(Math.Round(avgONFIUtil[channelNum[chanIndex]], 2).ToString()));
                                ddr_onfiBusUtilChart.Legends[0].CustomItems[chanIndex + 3].Cells.Add(new LegendCell(Math.Round(effectiveUtil[channelNum[chanIndex]], 2).ToString()));
                                ddr_onfiBusUtilChart.ChartAreas[0].AxisY.CustomLabels.Add(lowLimit , upLimit, "ch " + channelNum[chanIndex].ToString());
                                lowLimit = upLimit;
                                upLimit = lowLimit + 3 ;
                                ddr_onfiBusUtilChart.Series[chanIndex + 2].IsVisibleInLegend = false;
                               
                                if (busUtilPerChannel[channelNum[chanIndex]].Count() != 0)
                                {
                                   // chart13.Series.Add(new Series());
                                    
                                    ddr_onfiBusUtilChart.Series[chanIndex + 2].ChartType = SeriesChartType.FastPoint;

                                    for (int timeIndex = 0; timeIndex < timePerChannel[channelNum[chanIndex]].Count(); timeIndex++)
                                    {
                                        double timeAxis = timePerChannel[channelNum[chanIndex]].ElementAt(timeIndex);

                                        for (int busIndex = 0; busIndex < busUtilPerChannel[channelNum[chanIndex]].ElementAt(timeIndex); busIndex++)
                                        {
                                            ddr_onfiBusUtilChart.Series[chanIndex + 2].Points.AddXY(timeAxis, channelNum[chanIndex]*3 + 3);
                                            timeAxis++;
                                        }
                                    }

                                }//if
                                file.Close();
                            }//for
                            ddr_onfiBusUtilChart.ResumeLayout();

                        }//using
                    }
                    //chart13.Series.Clear();
                }//end try
                catch (IOException e)
                {
                    throw new Exception(String.Format("An error occurred while executing the data import: ONFI Utilization Report", e.Message), e);
                }
        }
        
        private void ONFIBusUtilizationData(bool multiSim)
        {
            string fileName1 = "";
            string fileName2 = "";
            if (multiSim)
            {
                
                string param = validMultiSimParam.Trim();
                if (simTypeBox.Text == "Multi Sim based on Block Size")
                {
                    
                    int qIndex = 0;
                    switch (param)
                    {
                        case "512":
                            qIndex = validQDepth.Count() - 1;
                            break;
                        case "1024":
                            qIndex = validQDepth.Count() - 2;
                            break;
                        case "2048":
                            {
                                qIndex = validQDepth.Count() - 3;
                                break;
                            }
                        case "4096":
                            {
                                qIndex = validQDepth.Count() - 4;
                                break;
                            }
                        case "8192":
                            {
                                qIndex = validQDepth.Count() - 5;
                                break;
                            }
                    }
                    param = validMultiSimParam.Trim();
                    if (enableWrkld == "1")
                    {
                        fileName1 = ".\\Reports\\onfi_chan_activity_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                        fileName2 = ".\\Reports\\onfi_chan_util_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                    }
                    else
                    {
                        fileName1 = ".\\Reports\\onfi_chan_activity_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                        fileName2 = ".\\Reports\\onfi_chan_util_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                    }
                }
                else if (simTypeBox.Text == "Multi Sim based on QD")
                {
                    //int qdIndex = 0;
                    param = validMultiSimParam.Trim();
                    if (enableWrkld == "1")
                    {
                        fileName1 = ".\\Reports\\onfi_chan_activity_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + param + ".log";
                        fileName2 = ".\\Reports\\onfi_chan_util_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + param + ".log";
                    }
                    else
                    {
                        fileName1 = ".\\Reports\\onfi_chan_activity_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + param + ".log";
                        fileName2 = ".\\Reports\\onfi_chan_util_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + param + ".log";
                    }
                  
                }
                else
                {
                    int qIndex = 0;
                    switch (param)
                    {
                        case "512":
                            qIndex = validQDepth.Count() - 1;
                            break;
                        case "1024":
                            qIndex = validQDepth.Count() - 2;
                            break;
                        case "2048":
                            {
                                qIndex = validQDepth.Count() - 3;
                                break;
                            }
                        case "4096":
                            {
                                qIndex = validQDepth.Count() - 4;
                                break;
                            }
                        case "8192":
                            {
                                qIndex = validQDepth.Count() - 5;
                                break;
                            }
                    }
                    param = validMultiSimParam.Trim();
                    fileName1 = ".\\Reports\\onfi_chan_activity_report_" + "iosize_" + param + "_" + "blksize_" + validBlockSize[0] + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                    fileName2 = ".\\Reports\\onfi_chan_util_report_" + "iosize_" + param + "_" + "blksize_" + validBlockSize[0] + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                    
                }
            }
            else
            {
                fileName1 = ".\\Reports\\onfi_chan_activity_report.log";
                fileName2 = ".\\Reports\\onfi_chan_util_report.log";
               
            }
            plotONFI(fileName1, fileName2);
        }//end function

      
        private void cwSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(cwSizeBox.Text)) return;
            emCacheSize = cwSizeBox.Text;

            pageSizeBox.Items.Clear();
            switch(int.Parse(emCacheSize))
            {
                
                case 128:
                    {
                        pageSizeBox.Items.Add("64");
                        pageSizeBox.Items.Add("128");
                        pageSizeBox.SelectedIndex = 0;
                        break;
                    }
                case 256:
                    {
                        pageSizeBox.Items.Add("64");
                        pageSizeBox.Items.Add("128");
                        pageSizeBox.Items.Add("256");
                        pageSizeBox.SelectedIndex = 1;
                        break;
                    }
                case 512:
                    {
                        pageSizeBox.Items.Add("64");
                        pageSizeBox.Items.Add("128");
                        pageSizeBox.Items.Add("256");
                        pageSizeBox.Items.Add("512");
                        pageSizeBox.SelectedIndex = 1;
                        break;
                    }
                case 1024:
                    {
                        pageSizeBox.Items.Add("64");
                        pageSizeBox.Items.Add("128");
                        pageSizeBox.Items.Add("256");
                        pageSizeBox.Items.Add("512");
                        pageSizeBox.Items.Add("1024");
                        pageSizeBox.SelectedIndex = 1;
                        break;
                    }
                case 2048:
                    {
                        pageSizeBox.Items.Add("64");
                        pageSizeBox.Items.Add("128");
                        pageSizeBox.Items.Add("256");
                        pageSizeBox.Items.Add("512");
                        pageSizeBox.Items.Add("1024");
                        pageSizeBox.Items.Add("2048");
                        pageSizeBox.SelectedIndex = 1;
                        break;
                    }
                case 4096:
                    {
                        pageSizeBox.Items.Add("64");
                        pageSizeBox.Items.Add("128");
                        pageSizeBox.Items.Add("256");
                        pageSizeBox.Items.Add("512");
                        pageSizeBox.Items.Add("1024");
                        pageSizeBox.Items.Add("2048");
                        pageSizeBox.Items.Add("4096");
                        pageSizeBox.SelectedIndex = 1;
                        break;
                    }
                default:
                    {
                        pageSizeBox.Items.Add("64");
                        pageSizeBox.Items.Add("128");
                        pageSizeBox.Items.Add("256");
                        pageSizeBox.Items.Add("512");
                        pageSizeBox.Items.Add("1024");
                        pageSizeBox.Items.Add("2048");
                        pageSizeBox.Items.Add("4096");
                        pageSizeBox.Items.Add("8192");
                        pageSizeBox.SelectedIndex = 1;
                        break;
                    }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        bool FlashDemo = false;
        private void loadGraphButton_Click(object sender, EventArgs e)
        {
            resetLoadGraphProgressBar();
            graphLoadProgressLabel.Text = "Loading...";
            resetDefaultCharts();
            
            if (enMultiSim)
            {
                resetMultiSimCharts();

                if(FlashDemo)
                {
                    iopsVsIOSizeChart.Enabled = false;
                    iopsVsIOSizeChart.Visible = false;

                    latencyVsIOSizeChart.Enabled = false;
                    latencyVsIOSizeChart.Visible = false;

                    latencyVsIOPS_vIOSize.Enabled = false;
                    latencyVsIOPS_vIOSize.Visible = false;
                    iopsVsQDChart.Visible = true;
                    latencyVsQDChart.Visible = true;
                    latencyVsIOPs_vQDChart.Visible = true;
                    label46.Visible = false;
                    label46.Enabled = false;
                    cmdCountVsIOSizeChart.Enabled = false;
                    

                    comboBox36.Visible = false;
                    comboBox36.Enabled = false;

                    this.chartPanel.ScrollControlIntoView(iopsVsQDChart);
                    this.chartPanel.AutoScroll = true;
                    int yLocation = 0;
                    this.iopsVsQDChart.Location = new Point(3, yLocation);

                    this.latencyVsQDChart.Location = new Point(3, yLocation + 292);
                    this.latencyVsIOPs_vQDChart.Location = new Point(3, yLocation + 584);
                   
                }
                else if (isMultiSimBlockSize())
                {
                    disableIOPSVsQDChart();
                    disableLatencyVsQDChart();
                    disableLatVsIOPS_QD();

                    enableIOPSVsBlkSize();
                    enableLatVsBlkSize();
                    enableLatVsIOPS_BlkSize();
                    int yLocation = 0;
                    initMultiSimBlkSizeParam(yLocation);
                      
                }
                else if (isMultiSimQD())
                {
                    iopsVsIOSizeChart.Enabled = false;
                    iopsVsIOSizeChart.Visible = false;
                    
                    latencyVsIOSizeChart.Enabled = false;
                    latencyVsIOSizeChart.Visible = false;

                    latencyVsIOPS_vIOSize.Enabled = false;
                    latencyVsIOPS_vIOSize.Visible = false;
                    iopsVsQDChart.Visible = true;
                    latencyVsQDChart.Visible = true;
                    latencyVsIOPs_vQDChart.Visible = true;
                    label46.Visible = false;
                    label46.Enabled = false;
                    comboBox36.Visible = false;
                    comboBox36.Enabled = false;

                    int y1 = latencyVsQDChart.Location.Y;
                    this.chartPanel.ScrollControlIntoView(iopsVsQDChart);
                    chartPanel.AutoScroll = true;
                    int yLocation = 0;
                    this.iopsVsQDChart.Location = new Point(3, yLocation);
                    
                    this.latencyVsQDChart.Location = new Point(3, yLocation + 292);
                    this.latencyVsIOPs_vQDChart.Location = new Point(3, yLocation + 584);
                    this.cmdCountVsIOSizeChart.Location = new System.Drawing.Point(3, yLocation + 876);
                    this.label46.Location = new Point(870, yLocation + 876);
                    this.comboBox36.Location = new System.Drawing.Point(930, yLocation + 876);
                    label32.Enabled = true;
                    label32.Visible = true;
                    this.label32.Text = "Queue Depth";
                    this.label32.Location = new Point(820, 1168);
                    comboBox25.Enabled = true;
                    comboBox25.Visible = true;
                    comboBox28.Enabled = true;
                    comboBox28.Visible = true;
                    this.comboBox28.Location = new System.Drawing.Point(896, 1168);
                    this.cmdCountVsLBAChart.Location = new System.Drawing.Point(3, yLocation + 1168);
                
                    if (isDisableBusUtilChartSelected())
                    {
                        disableBusUtilizationChart();
                        disableBusUtilChartViewSelection();
                    }
                    else
                    {
                        enableBusUtilizationChart();
                        enableBusUtilChartViewSelection();
                        this.label28.Location = new System.Drawing.Point(820, 1460);
                        this.comboBox25.Location = new System.Drawing.Point(896, 1460);
                        this.ddr_onfiBusUtilChart.Location = new System.Drawing.Point(3, yLocation + 1460);
                    }
           
                }
                else
                {
                    iopsVsQDChart.Enabled = false;
                    iopsVsQDChart.Visible = false;

                    latencyVsQDChart.Enabled = false;
                    latencyVsQDChart.Visible = false;

                    latencyVsIOPs_vQDChart.Enabled = false;
                    latencyVsIOPs_vQDChart.Visible = false;

                    iopsVsIOSizeChart.Enabled = true;
                    iopsVsIOSizeChart.Visible = true;

                    latencyVsIOSizeChart.Enabled = true;
                    latencyVsIOSizeChart.Visible = true;

                    latencyVsIOPS_vIOSize.Enabled = true;
                    latencyVsIOPS_vIOSize.Visible = true;
                    this.chartPanel.ScrollControlIntoView(iopsVsIOSizeChart);
                    chartPanel.AutoScroll = true;
                    int yLocation = 0;
                    this.iopsVsIOSizeChart.Location = new Point(3, yLocation);
                    this.latencyVsIOSizeChart.Location = new Point(3, yLocation + 292);
                    this.latencyVsIOPS_vIOSize.Location = new Point(3, yLocation + 584);
                    this.cmdCountVsIOSizeChart.Location = new System.Drawing.Point(3, yLocation + 876);
                    this.label46.Location = new Point(870, yLocation + 876);
                    this.label46.Text = "IOSize";
                    this.comboBox36.Location = new System.Drawing.Point(930, yLocation + 876);
                    label32.Enabled = true;
                    label32.Visible = true;
                    this.label32.Location = new Point(840, 1168);
                    this.label32.Text = "IOSize";
                    this.label28.Text = "IOSize";
                    comboBox25.Enabled = true;
                    comboBox25.Visible = true;
                    comboBox28.Enabled = true;
                    comboBox28.Visible = true;
                    this.comboBox28.Location = new System.Drawing.Point(896, 1168);
                    this.cmdCountVsLBAChart.Location = new System.Drawing.Point(3, yLocation + 1168);

                    if (disableBusUtilBox.Checked)
                    {
                        this.ddr_onfiBusUtilChart.Enabled = false;
                        this.ddr_onfiBusUtilChart.Visible = false;
                        this.comboBox25.Visible = false;
                        this.label28.Visible = false;
                    }
                    else
                    {
                        this.ddr_onfiBusUtilChart.Enabled = true;
                        this.ddr_onfiBusUtilChart.Visible = true;
                        this.comboBox25.Visible = true;
                        this.label28.Visible = true;
                        this.label28.Location = new System.Drawing.Point(840, 1460);
                        this.comboBox25.Location = new System.Drawing.Point(896, 1460);
                        this.ddr_onfiBusUtilChart.Location = new System.Drawing.Point(3, yLocation + 1460);
                    }
                      
                }
                
            }
            else//single sim
            {
               
                chartPanel.AutoScroll = true;
                this.chartPanel.ScrollControlIntoView(iopsVsIOSizeChart);
                int yLocation = 0;
                this.iopsVsIOSizeChart.Location = new Point(1, yLocation); //IOPS Vs IOSize
                this.latencyVsIOSizeChart.Location = new Point(1, yLocation + 292);//Latency Vs IOSize
                this.iopsVsQDChart.Location = new Point(1, yLocation + 584);//IOPS Vs QD
                this.latencyVsQDChart.Location = new Point(1, yLocation + 876);//Latency Vs QD
                this.cmdCountVsIOSizeChart.Location = new Point(1, yLocation + 1168); //CommandVs IOSize
                label46.Visible = false;
                comboBox36.Visible = false;
                label46.Enabled = false;
                comboBox36.Enabled = false;
                this.label46.Location = new Point(920, yLocation + 1168);
                this.comboBox36.Location = new System.Drawing.Point(930, yLocation + 1168);
                this.comboBox28.Location = new Point(760, yLocation + 1460);
                this.cmdCountVsLBAChart.Location = new Point(1, yLocation + 1460);//Cmd Vs LBA
           
                if (disableBusUtilBox.Checked)
                {
                    this.label28.Visible = false;
                    this.comboBox25.Visible = false;
                    this.ddr_onfiBusUtilChart.Enabled = false;
                    this.ddr_onfiBusUtilChart.Visible = false;
                    
                    this.bankUtilChart.Location = new Point(1, yLocation + 1752);
                    this.channelUtilChart.Location = new Point(1, yLocation + 2044);
                   
                }
                else
                {
                    this.ddr_onfiBusUtilChart.Enabled = true;
                    this.ddr_onfiBusUtilChart.Visible = true;
            
                    this.ddr_onfiBusUtilChart.Location = new Point(1, yLocation + 1752);//DDR/ONFI
                    this.bankUtilChart.Location = new Point(1, yLocation + 2104);//bank Util
                    this.channelUtilChart.Location = new Point(1, yLocation + 2396);//channel Util
                    
                }
                label32.Enabled = false;
                label32.Visible = false;
                comboBox28.Enabled = false;
                comboBox28.Visible = false;
                bankUtilChart.Enabled = true;
                bankUtilChart.Visible = true;

                channelUtilChart.Enabled = true;
                channelUtilChart.Visible = true;

                iopsVsQDChart.Enabled = true;
                iopsVsQDChart.Visible = true;

                latencyVsQDChart.Enabled = true;
                latencyVsQDChart.Visible = true;

                latencyVsIOPs_vQDChart.Enabled = false;
                latencyVsIOPs_vQDChart.Visible = false;

                iopsVsIOSizeChart.Enabled = false;
                iopsVsIOSizeChart.Visible = false;

                latencyVsIOSizeChart.Enabled = true;
                latencyVsIOSizeChart.Visible = true;

                latencyVsIOPS_vIOSize.Enabled = false;
                latencyVsIOPS_vIOSize.Visible = false;

                cmdCountVsIOSizeChart.Enabled = true;
                cmdCountVsIOSizeChart.Visible = true;

                cmdCountVsLBAChart.Enabled = true;
                cmdCountVsLBAChart.Visible = true;

                iopsVsIOSizeChart.Enabled = true;
                iopsVsIOSizeChart.Visible = true;
                latencyVsIOSizeChart.Enabled = true;
                latencyVsIOSizeChart.Visible = true;

                
                comboBox25.Enabled = false;
                comboBox25.Visible = false;
                chartPanel.ResumeLayout();
                bankUtilizationData();
                channelUtilizationData();

            }
            
            latencyCalculation(enMultiSim);
            iopsCalculation(enMultiSim);

            if (FlashDemo)
            {
                IOPSVsQD(enMultiSim);
                LatencyVsQD(enMultiSim);
                LatencyVsIOPSwithQD(enMultiSim);
              
            }
            else
            {
                if (simTypeBox.Text == "Multi Sim based on Block Size")
                {
                    CommandVsBlockSize(enMultiSim);
                    IOPSVsBlockSize(enMultiSim);
                    LatencyVsBlockSize(enMultiSim);
                    LatencyVsIOPSwithBlockSize(enMultiSim);
                }
                else
                {
                    CommandVsIOSize(enMultiSim);
                    IOPSVsIOSize(enMultiSim);
                    LatencyVsIOSize(enMultiSim);
                    LatencyVsIOPSwithIOSize(enMultiSim);
                }
                IOPSVsQD(enMultiSim);
                LatencyVsQD(enMultiSim);
                LatencyVsIOPSwithQD(enMultiSim);
                CommandVsLBA(enMultiSim);
                if (!disableBusUtilBox.Checked)
                {

                    DDRBusUtilizationData(enMultiSim);
                    ONFIBusUtilizationData(enMultiSim);


                }
                if (!enMultiSim)
                {
                    resetLatencyIOPSTextBox();
                    setLatencyIOPSTextBox();
                }
            }
            progressBar2.Value = 5;
            graphLoadProgressLabel.Text = "Complete...";
            
            
        }

        private void enableBusUtilChartViewSelection()
        {
            this.comboBox25.Visible = true;
            this.label28.Visible = true;
        }

        private void enableBusUtilizationChart()
        {
            this.ddr_onfiBusUtilChart.Enabled = true;
            this.ddr_onfiBusUtilChart.Visible = true;
        }

        private void disableBusUtilChartViewSelection()
        {
            this.comboBox25.Visible = false;
            this.label28.Visible = false;
        }

        private bool isMultiSimQD()
        {
            return simTypeBox.Text == "Multi Sim based on QD";
        }

        private void setLatencyIOPSTextBox()
        {
            this.minLatencyBox.Enabled = true;
            this.minLatencyBox.Text = latencyMinima.ToString();
            this.avgLatencyBox.Text = latencyAverage.ToString();
            this.avgLatencyBox.Enabled = true;
            this.maxLatencyBox.Text = latencyMaxima.ToString();
            this.maxLatencyBox.Enabled = true;
            this.textBox7.Text = iopsAverage.ToString();
            this.textBox7.Enabled = true;
        }

        private void resetLatencyIOPSTextBox()
        {
            this.minLatencyBox.Text = "";
            this.avgLatencyBox.Text = "";
            this.maxLatencyBox.Text = "";
            this.textBox7.Text = "";
        }

        private void initMultiSimBlkSizeParam(int yLocation)
        {
            this.chartPanel.ScrollControlIntoView(iopsVsIOSizeChart);
            chartPanel.AutoScroll = true;
            this.iopsVsIOSizeChart.Location = new Point(3, yLocation);
            this.latencyVsIOSizeChart.Location = new Point(3, yLocation + 292);
            this.latencyVsIOPS_vIOSize.Location = new Point(3, yLocation + 584);
            this.cmdCountVsIOSizeChart.Location = new System.Drawing.Point(3, yLocation + 876);
            label46.Location = new System.Drawing.Point(870, yLocation + 876);
            comboBox36.Location = new System.Drawing.Point(930, yLocation + 876);
            label32.Enabled = true;
            label32.Visible = true;
            comboBox25.Enabled = true;
            comboBox25.Visible = true;
            comboBox28.Enabled = true;
            comboBox28.Visible = true;
            label46.Visible = true;
            label46.Enabled = true;
            label46.Text = "BlockSize";
            comboBox36.Enabled = true;
            comboBox36.Visible = true; 
            this.label32.Location = new Point(840, yLocation + 1168);
            this.label32.Text = "BlockSize";
            this.label28.Text = "BlockSize";
            this.comboBox28.Location = new System.Drawing.Point(896, yLocation + 1168);
            this.cmdCountVsLBAChart.Location = new System.Drawing.Point(3, yLocation + 1168);

            if (isDisableBusUtilChartSelected())
            {
                disableBusUtilizationChart();
                this.comboBox25.Visible = false;
                this.label28.Visible = false;
            }
            else
            {
                this.ddr_onfiBusUtilChart.Enabled = true;
                this.ddr_onfiBusUtilChart.Visible = true;
                this.comboBox25.Visible = true;
                this.label28.Visible = true;
                this.label28.Location = new System.Drawing.Point(840, 1460);
                this.comboBox25.Location = new System.Drawing.Point(896, 1460);
                this.ddr_onfiBusUtilChart.Location = new System.Drawing.Point(3, yLocation + 1460);
            }
        }

        private void disableBusUtilizationChart()
        {
            this.ddr_onfiBusUtilChart.Enabled = false;
            this.ddr_onfiBusUtilChart.Visible = false;
        }

        private bool isDisableBusUtilChartSelected()
        {
            return disableBusUtilBox.Checked;
        }

        private void enableLatVsIOPS_BlkSize()
        {
            latencyVsIOPS_vIOSize.Enabled = true;
            latencyVsIOPS_vIOSize.Visible = true;
        }

        private void enableLatVsBlkSize()
        {
            latencyVsIOSizeChart.Enabled = true;
            latencyVsIOSizeChart.Visible = true;
        }

        private void enableIOPSVsBlkSize()
        {
            iopsVsIOSizeChart.Enabled = true;
            iopsVsIOSizeChart.Visible = true;
        }

        private void disableLatVsIOPS_QD()
        {
            latencyVsIOPs_vQDChart.Enabled = false;
            latencyVsIOPs_vQDChart.Visible = false;
        }

        private bool isMultiSimBlockSize()
        {
            
            return simTypeBox.Text == "Multi Sim based on Block Size";
        }

        private void disableLatencyVsQDChart()
        {
            latencyVsQDChart.Enabled = false;
            latencyVsQDChart.Visible = false;
        }

        private void disableIOPSVsQDChart()
        {
            iopsVsQDChart.Enabled = false;
            iopsVsQDChart.Visible = false;
        }

        private void resetMultiSimCharts()
        {
            this.minLatencyBox.Text = "";
            this.avgLatencyBox.Text = "";
            this.maxLatencyBox.Text = "";
            this.textBox7.Text = "";
            this.minLatencyBox.Enabled = false;
            this.avgLatencyBox.Enabled = false;
            this.maxLatencyBox.Enabled = false;
            this.textBox7.Enabled = false;

            ddr_onfiBusUtilChart.Series.Clear();
            bankUtilChart.Series.Clear();
            channelUtilChart.Series.Clear();
            //chart3.Series.Clear();
            //chart5.Series.Clear();
            bankUtilChart.Enabled = false;
            bankUtilChart.Visible = false;

            channelUtilChart.Enabled = false;
            channelUtilChart.Visible = false;

            //chart3.Enabled = false;
            //chart3.Visible = false;

            //chart5.Enabled = false;
            //chart5.Visible = false;
        }

        private void resetDefaultCharts()
        {
            //chart3.Series.Clear();
            iopsVsIOSizeChart.Annotations.Clear();
            latencyVsIOSizeChart.Annotations.Clear();
            latencyVsIOPs_vQDChart.Annotations.Clear();
            latencyVsIOPS_vIOSize.Annotations.Clear();
            cmdCountVsIOSizeChart.Annotations.Clear();
            iopsVsQDChart.Annotations.Clear();
            latencyVsQDChart.Annotations.Clear();
            cmdCountVsIOSizeChart.Series.Clear();
            ddr_onfiBusUtilChart.Series.Clear();
        }

        private void resetLoadGraphProgressBar()
        {
            progressBar2.Maximum = 5;
            progressBar2.Minimum = 0;
            progressBar2.Value = 0;
        }

      
        List<string> validBlockSize = new List<string>();
        List<string> validIOSize = new List<string>();
        List<string> validQueueDepth = new List<string>();
        List<int> validQDepth = new List<int>();

        private void initParams()
        {
            pollWaitTime = pollingStatusTextBox.Text;
            numChannel = numChannelBox.Text;
            ioSize = hostIOSizeBox.Text;
            blkSize = blockSizeBox.Text;
            ddrSpeed = ddrSpeedBox.Text;
            credit = outStandingCmdPerChBox.Text;
            onfiSpeed = onfiSpeedBox.Text;
            interfaceType = ifTypeBox.Text;
            programTime = programTimeBox.Text;
            readTime = readTimeBox.Text;
            pageSize = pageSizeBox.Text;
            numPages = numPagesBox.Text;
            numBanks = numBanksBox.Text;
            numDie = numDiePerChanBox.Text;
            enableLogs = checkBox2.Checked.ToString();
            emCacheSize = cwSizeBox.Text;
            queueDepth = hostQueueDepthBox.Text;
            numCommands = numCmdsBox.Text;
            queueFactor = queueFactorBox.Text;
            timeScale = comboBox22.Text;
            //depthOfQueue = queueDepth;
            coreMode = threadSelectBox.Checked.ToString();
            numCores = numCoresBox.Text;
            cmdTransferTime = cmdOHTextBox.Text;
            zoomScale = int.Parse(comboBox35.Text);
             
             cmdType = "100";
             numSlot = 32;
             cwNum = 8;
             queueSize = 128;
             cwCnt = 16;
             timeScaleVar = 0;
             onfiClock = 0;
             mode = 0;
             //enMultiSim = false;
            // mCoreMode = "Single CPU";
             ioSizeVariation = false;
             removeFiles = " 1";
            // validParam = "";
        
        }

        private void numChannelBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(numChannelBox.Text)) return;
            numChannel = numChannelBox.Text;
        }

        private void hostIOSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(hostIOSizeBox.Text)) return;
            ioSize = hostIOSizeBox.Text;
            cwSizeBox.Items.Clear();
            switch (int.Parse(ioSize))
            {
                case 512:
                    {
                        cwSizeBox.Items.Add("128");
                        cwSizeBox.Items.Add("256");
                        cwSizeBox.Items.Add("512");
                        cwSizeBox.SelectedIndex = 2;
                        break;
                    }
                case 1024:
                    {
                        cwSizeBox.Items.Add("128");
                        cwSizeBox.Items.Add("256");
                        cwSizeBox.Items.Add("512");
                        cwSizeBox.Items.Add("1024");
                        cwSizeBox.SelectedIndex = 2;
                        break;
                    }
                case 2048:
                    {
                        cwSizeBox.Items.Add("128");
                        cwSizeBox.Items.Add("256");
                        cwSizeBox.Items.Add("512");
                        cwSizeBox.Items.Add("1024");
                        cwSizeBox.Items.Add("2048");
                        cwSizeBox.SelectedIndex = 2;
                        break;
                    }
                case 4096:
                    {
                        cwSizeBox.Items.Add("128");
                        cwSizeBox.Items.Add("256");
                        cwSizeBox.Items.Add("512");
                        cwSizeBox.Items.Add("1024");
                        cwSizeBox.Items.Add("2048");
                        // cwSizeBox.Items.Add("4096");
                        cwSizeBox.SelectedIndex = 2;
                        break;
                    }
                case 8192:
                    {
                        cwSizeBox.Items.Add("128");
                        cwSizeBox.Items.Add("256");
                        cwSizeBox.Items.Add("512");
                        cwSizeBox.Items.Add("1024");
                        cwSizeBox.Items.Add("2048");
                        //cwSizeBox.Items.Add("4096");
                        //cwSizeBox.Items.Add("8192");
                        cwSizeBox.SelectedIndex = 2;
                        break;

                    }
                default:
                    {
                        cwSizeBox.Items.Add("128");
                        cwSizeBox.Items.Add("256");
                        cwSizeBox.Items.Add("512");
                        cwSizeBox.Items.Add("1024");
                        cwSizeBox.Items.Add("2048");
                        // cwSizeBox.Items.Add("4096");
                        // cwSizeBox.Items.Add("8192");
                        cwSizeBox.SelectedIndex = 2;
                        break;

                    }
            }
        }

        private void ddrSpeedBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(ddrSpeedBox.Text)) return;
            ddrSpeed = ddrSpeedBox.Text;
        }

        private void outstandingCmdPerChBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(outStandingCmdPerChBox.Text)) return;
            credit = outStandingCmdPerChBox.Text;
        }

        private void onfiSpeedBox_TextChanged(object sender, EventArgs e)
        {
            resetButtons();
            onfiSpeed = "800";
            onfiSpeed = onfiSpeedBox.Text;
        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(comboBox12.Text)) return;
            interfaceType = comboBox12.Text;
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(comboBox9.Text)) return;
            programTime = comboBox9.Text;
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(comboBox8.Text)) return;
            readTime = comboBox8.Text;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(comboBox3.Text)) return;
            pageSize = comboBox3.Text;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(comboBox7.Text)) return;
            numPages = comboBox7.Text;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(comboBox5.Text)) return;
            numBanks = comboBox5.Text;
        }

        private void numDiePerChanBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(numDiePerChanBox.Text)) return;
            numDie = numDiePerChanBox.Text;
        }
        
        private int getCwNum()
        {
            int cwNum;
            cwNum = (int.Parse(numDie) * int.Parse(numBanks) * int.Parse(pageSize)) / int.Parse(emCacheSize);
            if (cwNum == 0)
            {
                MessageBox.Show("ERROR: Number of Logical Banks is Zero: ABORT SIMULATION!!");

            }
            return cwNum;
        }

        private int getQueueSize()
        {
            return int.Parse(numChannel) * cwNum * int.Parse(queueFactor);
        }
        private int getCwCnt()
        {
            return int.Parse(blkSize) / int.Parse(emCacheSize);
        }
        private int getNumSlot()
        {
            int slot = queueSize / cwCnt;
            if (slot == 0)
            {
                MessageBox.Show("ERROR: Max Number Of Slots is Zero: ABORT SIMULATION!!");

            }
            return slot;
        }

        private int getQueueDepth()
        {
            int qDepth = int.Parse(queueDepth);
            if (qDepth > numSlot)
            {
                qDepth = numSlot;
            }

            if (enableWrkld != "1")
            {
                if (qDepth > int.Parse(numCommands))
                {
                    if (int.Parse(numCommands) < numSlot)
                    {
                        qDepth = int.Parse(numCommands);
                    }
                }
            }
            if (mCoreMode == "Multi Core")
            {
                qDepth = int.Parse(numCores.Trim());
            }
            return qDepth;
        }
        private void setParameterString()
        {
            blkSize = " " + blkSize.Trim();
            ioSize = " " + ioSize.Trim();
            emCacheSize = " " + emCacheSize.Trim();
            pageSize = " " + pageSize.Trim();
            numPages = " " + numPages.Trim();
            numBanks = " " + numBanks.Trim();
            readTime = " " + readTime.Trim();
            programTime = " " + programTime.Trim();
            enableLogs = " " + enableLogs.Trim();
            numDie = " " + numDie.Trim();
            numChannel = " " + numChannel.Trim();
            depthOfQueue = " " + depthOfQueue.Trim();
            credit = " " + credit.Trim();
            readTime = " " + readTime.Trim();
            programTime = " " + programTime.Trim();
            queueFactor = " " + queueFactor.Trim();
            ddrSpeed = " " + ddrSpeed.Trim();
            cmdTransferTime = " " + cmdTransferTime.Trim();
            numCores = " " + numCores.Trim();
            wrkloadBS = " " + blkSize.Trim();
            pollWaitTime = " " + pollWaitTime.Trim();
            if (enableWrkld == "1" && (simulationTypeBox.SelectedIndex != 0))
            {
                wrkloadFile = workloadFileBox.Text;
                wrkloadFile = " " + ".\\Workload\\" + wrkloadFile.Trim();
            }
            else
            {
                wrkloadFile = " " + wrkloadFile.Trim();
            }

            if (mCoreMode.Trim() == "Single CPU")
            {
                mode = 0;
                coreMode = " " + mode.ToString().Trim();
            }
            else if (mCoreMode.Trim() == "Multi Core")
            {
                mode = 1;
                //depthOfQueue = "1";
                depthOfQueue = " " + depthOfQueue.Trim();
                coreMode = " " + mode.ToString().Trim();
            }

            enableSeqLBA = " " + enableSeqLBA.Trim();
            cmdType = " " + cmdType.Trim();

            int speed = 0;
            if (interfaceType == "DDR")
            {
                Int32.TryParse(onfiSpeed, out speed);
                speed = 2 * speed;
                onfiClk = speed.ToString();
                onfiClock = speed;
            }
            else
            {
                onfiClock = int.Parse(onfiSpeed);
                onfiClk = onfiSpeed;
            }
            onfiClk = " " + onfiClk.Trim();
           
        }

        private void resetSimButtonProgressBar(int min, int max)
        {
            progressBar1.Minimum = min;
            progressBar1.Maximum = max;
            progressBar1.Value = 0;
        }

        private void singleSimCall()
        {
            cwNum = getCwNum();
            disableLoadGraphButton();
            queueSize = getQueueSize();
            cwCnt = getCwCnt();
            numSlot = getNumSlot();
            depthOfQueue = getQueueDepth().ToString();
           
            string enableMultiSim = " 0";
            setParameterString();
            resetSimButtonProgressBar(0, 5);
            startSingleSim(enableMultiSim);
            setSimButtonProgressBar(5);
            enableGUIParamSelect();
           
            if (enableWrkld == "1")
            {
                initParams();
            }
        }

        private void setSimButtonProgressBar(int param)
        {
            simProgressLabel.Text = "Complete...";
            progressBar1.Value = param;
        }

        private void startSingleSim(string enableMultiSim)
        {
            string var = "";
            string removeFiles = " 1";
            if (enableWrkld == "1")
            {
                string enWrkld = " " + enableWrkld.Trim();
                var = numCommands + wrkloadBS + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
                + numPages + depthOfQueue + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
                + enableMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
            }
            else
            {
                string enWrkld = " " + enableWrkld.Trim();
                var = numCommands + ioSize + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
                    + numPages + depthOfQueue + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
                    + enableMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
            }
            Process process = new Process();
            try
            {
                process.StartInfo.FileName = @".\TeraSMemoryController.exe";
                process.StartInfo.Arguments = var; // Put your arguments here
                process.Start();
            }
            finally
            {
                if (process != null)
                {
                    simProgressLabel.Text = "Running...";
                    process.WaitForExit();
                    process.Close();
                    

                }
            }
        }
        string multiSimParam = "";

        private void disableLoadGraphButton()
        {
            loadGraphButton.Enabled = false;
            panel1.Enabled = false;
            label28.Visible = false;
        }

        private void multiSimCall()
        {
            cwNum = getCwNum();
            disableLoadGraphButton();
            queueSize = getQueueSize();

            selectMultiSimOption();
            setParameterString();
            string enMultiSim = " 1";
            comboBox25.Visible = false;
            comboBox28.Visible = false;

                if (multiSimParam == "BlockSize")
                {
                    List<string> blockSize = new List<string>();
                    if (int.Parse(emCacheSize) > 512)
                    {
                        MessageBox.Show(" CW Size is more than 512, setting CWSize to 512");
                        cwSizeBox.SelectedIndex = 1;
                        emCacheSize = " 512";

                    }
                    initBlkSizeParam(blockSize);
                   
                    for (int blockIndex = 0; blockIndex < blockSize.Count(); blockIndex++)
                    {
                        if (int.Parse(blockSize[blockIndex].Trim()) >= int.Parse(emCacheSize))
                        {

                            cwCnt = int.Parse(blockSize[blockIndex]) / int.Parse(emCacheSize);
                            numSlot = getNumSlot();
                            validBlockSize.Add(blockSize[blockIndex].Trim());

                            comboBox25.Items.Add(blockSize[blockIndex].Trim());
                            comboBox28.Items.Add(blockSize[blockIndex].Trim());
                            comboBox36.Items.Add(blockSize[blockIndex].Trim());
                            comboBox28.SelectedIndex = blockIndex;
                            comboBox25.SelectedIndex = blockIndex;
                            comboBox36.SelectedIndex = blockIndex;
                            validParam = validBlockSize[blockIndex].ToString();
                            validMultiSimParam = validBlockSize[blockIndex].ToString();
                         
                            string finalQDepth = " " + getQueueDepth().ToString();
                            validQDepth.Add(int.Parse(finalQDepth));

                            startBlkSizeMultiSim(enMultiSim, blockSize, blockIndex, finalQDepth);
                        }
                        else
                        {
                            progressBar1.Value = blockSize.Count();
                            simProgressLabel.Text = "Complete...";

                        }
                    }
                    progressBar1.Value = blockSize.Count();
                    simProgressLabel.Text = "Complete...";

                }
                else if (multiSimParam == "Queue Depth")
                {
                    List<string> queueD = new List<string>();
                    cwCnt = getCwCnt();
                    numSlot = getNumSlot();

                    initQDParam(queueD);
                    resetSimButtonProgressBar(0, queueD.Count);
                    
                    if (int.Parse(ioSize) < int.Parse(emCacheSize))
                    {
                        MessageBox.Show("ERROR: Code Word Size is more than IO Size: ABORT SIMULATION!!");
                    }
                    else
                    {
                        validMultiSimParam = "1";
                        for (int queueIndex = 0; queueIndex < queueD.Count(); queueIndex++)
                        {
                            int qDepthUpperLimit = 256;
                            
                            if ((int.Parse(queueD[queueIndex].Trim()) <= qDepthUpperLimit))
                            {
                                validQueueDepth.Add(queueD[queueIndex].Trim());
                                if (!FlashDemo)
                                {
                                    comboBox25.Items.Add(validQueueDepth[queueIndex].ToString());
                                    comboBox28.Items.Add(validQueueDepth[queueIndex].ToString());
                                    comboBox28.SelectedIndex = queueIndex;
                                    comboBox25.SelectedIndex = queueIndex;
                                }
                                validParam = validQueueDepth[queueIndex].ToString();
                                validMultiSimParam = validQueueDepth[queueIndex].ToString();
                                startQDMultiSim(enMultiSim, queueD, queueIndex);
                            }
                            else
                            {
                                setSimButtonProgressBar(queueD.Count());
                            }
                        }
                        setSimButtonProgressBar(queueD.Count());
                    }
                }//else if
                else
                {
                    if (int.Parse(emCacheSize) > 512)
                    {
                        MessageBox.Show(" CW Size is more than 512, setting CWSize to 512");
                        cwSizeBox.SelectedIndex = 1;
                        emCacheSize = " 512";

                    }
                    List<string> IOSize = new List<string>();
                    initIOSizeParam(IOSize);
                    resetSimButtonProgressBar(0, IOSize.Count());
                    cwCnt = getCwCnt();
                    numSlot = getNumSlot();
                    string finalQDepth = " " + getQueueDepth().ToString();
                    for (int ioSizeIndex = 0; ioSizeIndex < IOSize.Count(); ioSizeIndex++)
                    {
                        if (int.Parse(IOSize[ioSizeIndex].Trim()) >= int.Parse(emCacheSize))
                        {
                            validIOSize.Add(IOSize[ioSizeIndex].Trim());
                            comboBox25.Items.Add(IOSize[ioSizeIndex].Trim());
                            comboBox28.Items.Add(IOSize[ioSizeIndex].Trim());
                            comboBox36.Items.Add(IOSize[ioSizeIndex].Trim());
                            comboBox28.SelectedIndex = ioSizeIndex;
                            comboBox25.SelectedIndex = ioSizeIndex;
                            comboBox36.SelectedIndex = ioSizeIndex;
                            validParam = validIOSize[ioSizeIndex].ToString();
                            validMultiSimParam = validIOSize[ioSizeIndex].ToString();
                            
                            validQDepth.Add(int.Parse(finalQDepth.Trim()));

                            startIOSizeMultiSim(enMultiSim, IOSize, ioSizeIndex, finalQDepth);
                        }
                        else
                        {
                            setSimButtonProgressBar(IOSize.Count());
                        }
                    }
                    setSimButtonProgressBar(IOSize.Count());
                }
                if (!FlashDemo)
                {
                    comboBox25.Visible = true;
                    comboBox28.Visible = true;
                }
            enableLoadGraphButton();
            enableGUIParamSelect();
            if (enableWrkld == "1")
            {
                initParams();
            }
        }

        private void enableLoadGraphButton()
        {
            loadGraphButton.Enabled = true;
        }

        private void initIOSizeParam(List<string> IOSize)
        {
            validQDepth.Clear();
            validBlockSize.Clear();
            validIOSize.Clear();

            validMultiSimParam = "4096";
            comboBox25.Items.Clear();
            comboBox28.Items.Clear();
            comboBox36.Items.Clear();
            button3.Enabled = false;
            IOSize.Add(" 8192");
            IOSize.Add(" 4096");
            IOSize.Add(" 2048");
            IOSize.Add(" 1024");
            IOSize.Add(" 512");
            validBlockSize.Add(blkSize.Trim());
            removeFiles = " 1";
        }

        private void initQDParam(List<string> queueD)
        {
            validQueueDepth.Clear();
            validIOSize.Clear();

            queueD.Add(" 1");
            queueD.Add(" 2");
            queueD.Add(" 4");
            queueD.Add(" 8");
            queueD.Add(" 16");
            queueD.Add(" 32");
            queueD.Add(" 64");
            queueD.Add(" 128");
            queueD.Add(" 256");
            removeFiles = " 1";
            validIOSize.Add(ioSize.Trim());
            comboBox25.Items.Clear();
            comboBox28.Items.Clear();
        }

        private void startIOSizeMultiSim(string enMultiSim, List<string> IOSize, int ioSizeIndex, string finalQDepth)
        {
            string var = "";
            if (enableWrkld == "1")
            {
                string enWrkld = " " + enableWrkld.Trim();
                var = numCommands + IOSize[ioSizeIndex] + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
                + numPages + finalQDepth + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
                + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile;

            }
            else
            {
                string enWrkld = " " + enableWrkld.Trim();
                // wrkloadFile = " wrkload.txt";
                var = numCommands + IOSize[ioSizeIndex] + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
                    + numPages + finalQDepth + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
                    + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
            }
            Process process = new Process();

            try
            {
                process.StartInfo.FileName = @"..\..\..\..\..\Solution\Release\TeraSMemoryController.exe";
                process.StartInfo.Arguments = var; // Put your arguments here
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                simProgressLabel.Text = "Running...";
                process.WaitForExit();
                double percentage = (double)((double)(ioSizeIndex + 1) / IOSize.Count()) * 100;
                progressBar1.Value = ioSizeIndex + 1;
                removeFiles = " 0";

            }
            finally
            {
                if (process != null)
                {
                    process.Close();
                }

            }
        }

        private void startQDMultiSim(string enMultiSim, List<string> queueD, int queueIndex)
        {
            string var = "";
            if (enableWrkld == "1")
            {
                string enWrkld = " " + enableWrkld.Trim();
                wrkloadBS = " " + blkSize.Trim();
                var = numCommands + wrkloadBS + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
                + numPages + queueD[queueIndex] + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
                + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
            }
            else
            {
                string enWrkld = " " + enableWrkld.Trim();

                if(FlashDemo)
                {
                    enableSeqLBA = " 1";
                }
                var = numCommands + ioSize + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
                    + numPages + queueD[queueIndex] + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
                    + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
            }
            Process process = new Process();

            try
            {
                if (FlashDemo)
                {
                    process.StartInfo.FileName = @".\TeraSMemoryController.exe";
                }
                else
                {
                    process.StartInfo.FileName = @".\TeraSMemoryController.exe";
                }
                process.StartInfo.Arguments = var; // Put your arguments here
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                simProgressLabel.Text = "Running...";
                process.WaitForExit();
                double percentage = (double)((double)(queueIndex + 1) / queueD.Count()) * 100;
                progressBar1.Value = queueIndex + 1;
                removeFiles = " 0";
            }
            finally
            {
                if (process != null)
                {
                    process.Close();
                }

            }
        }

        private void startBlkSizeMultiSim(string enMultiSim, List<string> blockSize, int blockIndex, string finalQDepth)
        {
            string var = "";
            if (enableWrkld == "1")
            {
                string enWrkld = " " + enableWrkld.Trim();
                wrkloadBS = " " + blockSize[blockIndex];//wrkloadBS.Trim();
                var = numCommands + wrkloadBS + blockSize[blockIndex] + emCacheSize + pageSize + numBanks + numDie + numChannel
                + numPages + finalQDepth + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
                + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;

            }
            else
            {
                string enWrkld = " " + enableWrkld.Trim();
                // wrkloadFile = " wrkload.txt";
                var = numCommands + ioSize + blockSize[blockIndex] + emCacheSize + pageSize + numBanks + numDie + numChannel
                    + numPages + finalQDepth + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
                    + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
            }
            Process process = new Process();

            try
            {
                process.StartInfo.FileName = @"..\..\..\..\..\Solution\Release\TeraSMemoryController.exe";

                process.StartInfo.Arguments = var; // Put your arguments here
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                simProgressLabel.Text = "Running...";
                process.WaitForExit();
                double percentage = (double)((double)(blockIndex + 1) / blockSize.Count()) * 100;
                progressBar1.Value = blockIndex + 1;
                removeFiles = " 0";

            }
            finally
            {
                if (process != null)
                {
                    process.Close();
                }

            }
        }

        private void initBlkSizeParam(List<string> blockSize)
        {
            validQDepth.Clear();
            validIOSize.Clear();
            validBlockSize.Clear();

            validMultiSimParam = "4096";
            comboBox25.Items.Clear();
            comboBox28.Items.Clear();
            comboBox36.Items.Clear();
            validBlockSize.Clear();
            button3.Enabled = false;
            blockSize.Add(" 8192");
            blockSize.Add(" 4096");
            blockSize.Add(" 2048");
            blockSize.Add(" 1024");
            blockSize.Add(" 512");
            validIOSize.Add(ioSize.Trim());
            removeFiles = " 1";
            progressBar1.Maximum = blockSize.Count();
            progressBar1.Value = 0;
        }

        private void selectMultiSimOption()
        {
            if (multiSimParam == "Queue Depth")
            {
                ioSizeVariation = false;
                label28.Text = "Queue Depth";
            }
            else if (multiSimParam == "BlockSize")
            {
                ioSizeVariation = true;
                label28.Text = "BlockSize";
            }
            else if (multiSimParam == "IOSize")
            {
                ioSizeVariation = true;
                label28.Text = "IOSize";
            }
        }

        private void runSimButton_Click(object sender, EventArgs e)
        {
            resetButtons();
            
            if(int.Parse(ioSize) < int.Parse(emCacheSize))
            {
                MessageBox.Show("IO Size is less than CW Size, setting both to CW Size");
                ioSize = emCacheSize;
            }
            else
            {
                if(!FlashDemo)
                {
                    ioSize = hostIOSizeBox.Text;    
                }
                
            }
            if (int.Parse(blkSize) < int.Parse(emCacheSize))
            {
                MessageBox.Show("ERROR: Code Word Size is more than Block Size: ABORT SIMULATION!!");
            }
            else if (int.Parse(emCacheSize) < int.Parse(pageSize))
            {
                MessageBox.Show("ERROR: Page Size is more than Code Word Size: ABORT SIMULATION!!");
            }
            else
            {
                if (enMultiSim)
                {
                    multiSimCall();
                }
                else
                {
                    singleSimCall();
                }
            }
            loadGraphButton.Enabled = true;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
          
        }
     
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void numBanksBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(numBanksBox.Text)) return;
            numBanks = numBanksBox.Text;
           
        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(numPagesBox.Text)) return;
          
            if (numPagesBox.Text == "16 KB")
                numPages = "16384";
            else if (numPagesBox.Text == "32 KB")
                numPages = "32768";
            else if (numPagesBox.Text == "64 KB")
                numPages = "65536";
            else if (numPagesBox.Text == "128 KB")
                numPages = "131072";
            else if (numPagesBox.Text == "256 KB")
                numPages = "262144";
            else
                numPages = numPagesBox.Text;
        }

        private void pageSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(pageSizeBox.Text)) return;
            pageSize = pageSizeBox.Text;
            
        }

        private void readTimeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(readTimeBox.Text)) return;
            readTime = readTimeBox.Text;
        }

        private void comboBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(programTimeBox.Text)) return;
            programTime = programTimeBox.Text;
        }

        private void hostQueueDepthBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            queueDepth = hostQueueDepthBox.Text;
        }

        private void comboBox20_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ifTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(ifTypeBox.Text)) return;
            interfaceType = ifTypeBox.Text;
        }

        private void comboBox20_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void queueFactorBox_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            resetButtons();
             if (string.IsNullOrWhiteSpace(queueFactorBox.Text)) return;
             queueFactor = queueFactorBox.Text;

        }

        private void numCmdsBox_TextChanged_1(object sender, EventArgs e)
        {
            resetButtons();
            numCommands = "8";
            numCommands = numCmdsBox.Text;
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void TeraSController_Load(object sender, EventArgs e)
        {

        }

        private void comboBox22_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(comboBox22.Text)) return;
            timeScale = comboBox22.Text;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void workloadTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(workloadTypeBox.Text)) return;
           
            simTypeBox.Items.Clear();
            simTypeBox.Items.Add("Single Sim");
            if (!threadSelectBox.Checked)
            {
                simTypeBox.Items.Add("Multi Sim based on QD");
            }
            simTypeBox.Items.Add("Multi Sim based on Block Size");
            if(workloadTypeBox.Text == "Load Workload File")
            {
                string[] dirs = Directory.GetFiles(@".\Workload\", "*.txt");
                workloadFileBox.Items.Clear();
                foreach (string dir in dirs)
                {
                    string file = dir.Remove(0, 11);// - "..\..\..\..\..\Workload\";
                    workloadFileBox.Enabled = true;
                    workloadFileBox.Items.Add(file);
                }
                workloadFileBox.SelectedIndex = 0;
                enableWrkld = "1";
                hostIOSizeBox.Enabled = false;
                cwSizeBox.Enabled = false;
                numCmdsBox.Enabled = false;
                cmdType = "100";
                enableSeqLBA = "100";
                
                
            }
            else if(workloadTypeBox.Text == "Sequential 100% Read")
            {
                cmdType = "100";
                enableSeqLBA = "100";
                workloadFileBox.Enabled = false;
                enableWrkld = "0";
                hostIOSizeBox.Enabled = true;
                cwSizeBox.Enabled = true;
                numCmdsBox.Enabled = true;
                simTypeBox.Items.Add("Multi Sim based on IO Size");
               
            }
            else if(workloadTypeBox.Text == "Sequential 100% Write")
            {
                cmdType = "0";
                enableSeqLBA = "100";
                workloadFileBox.Enabled = false;
                enableWrkld = "0";
                hostIOSizeBox.Enabled = true;
                cwSizeBox.Enabled = true;
                numCmdsBox.Enabled = true;
                simTypeBox.Items.Add("Multi Sim based on IO Size");
            }
            else if (workloadTypeBox.Text == "Random 100% Read")
            {
                cmdType = "100";
                enableSeqLBA = "0";
                workloadFileBox.Enabled = false;
                enableWrkld = "0";
                hostIOSizeBox.Enabled = true;
                cwSizeBox.Enabled = true;
                numCmdsBox.Enabled = true;
                simTypeBox.Items.Add("Multi Sim based on IO Size");

            }
            else if (workloadTypeBox.Text == "Random 100% Write")
            {
                cmdType = "0";
                enableSeqLBA = "0";
                workloadFileBox.Enabled = false;
                enableWrkld = "0";
                hostIOSizeBox.Enabled = true;
                cwSizeBox.Enabled = true;
                numCmdsBox.Enabled = true;
                simTypeBox.Items.Add("Multi Sim based on IO Size");

            }
            else if (workloadTypeBox.Text == "Random 30/70 R/W")
            {
                cmdType = "30";
                enableSeqLBA = "0";
                workloadFileBox.Enabled = false;
                enableWrkld = "0";
                hostIOSizeBox.Enabled = true;
                cwSizeBox.Enabled = true;
                numCmdsBox.Enabled = true;
                simTypeBox.Items.Add("Multi Sim based on IO Size");

            }
            else if (workloadTypeBox.Text == "Random 70/30 R/W")
            {
                cmdType = "70";
                enableSeqLBA = "0";
                workloadFileBox.Enabled = false;
                enableWrkld = "0";
                hostIOSizeBox.Enabled = true;
                cwSizeBox.Enabled = true;
                numCmdsBox.Enabled = true;
                simTypeBox.Items.Add("Multi Sim based on IO Size");

            }
            else if (workloadTypeBox.Text == "Random 50/50 R/W")
            {
                cmdType = "50";
                enableSeqLBA = "0";
                workloadFileBox.Enabled = false;
                enableWrkld = "0";
                hostIOSizeBox.Enabled = true;
                cwSizeBox.Enabled = true;
                numCmdsBox.Enabled = true;
                simTypeBox.Items.Add("Multi Sim based on IO Size");

            }
           
            simTypeBox.SelectedIndex = 0;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (checkBox1.Checked == true)
            {
                enableSeqLBA = "1";
            }
            else if (checkBox1.Checked == false)
            {
                enableSeqLBA = "0";
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (checkBox2.Checked == true)
            {
                enableLogs = "1";
            }
            else if (checkBox2.Checked == false)
            {
                enableLogs = "0";
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
           
           
        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {
            

        }

        //private void button3_Click(object sender, EventArgs e)
        //{
           
        //}

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (checkBox3.Checked == true)
            {
                runSimButton.Enabled = true;
                button3.Enabled = true;
                panel4.Enabled = true;
                comboBox25.Enabled = true;
                comboBox25.Visible = true;
                comboBox28.Enabled = true;
                comboBox28.Visible = true;
                enMultiSim = true;
                simProgressLabel.Text = "Progress";
                removeFiles = " 1";
                validBlockSize.Clear();
                validQueueDepth.Clear();
                validIOSize.Clear();
                //label24.Visible = true;
                label28.Visible = true;
            }
            else
            {
                button3.Enabled = false;
                comboBox25.Enabled = false;
                comboBox25.Visible = false;
                comboBox28.Enabled = false;
                comboBox28.Visible = false;
                runSimButton.Enabled = true;
                panel4.Enabled = false;
                enMultiSim = false;
                progressBar1.Value = 0;
                simProgressLabel.Text = "Progress";
                //label24.Visible = false;
                label28.Visible = false;
                removeFiles = " 0";
            }
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            
        }

        private void radioButton3_CheckedChanged_1(object sender, EventArgs e)
        {
           
        }
     
    
        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox24_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
        private void comboBox25_SelectedIndexChanged(object sender, EventArgs e)
        {
            //resetButtons();
            validMultiSimParam = comboBox25.Text;
        }
 
        private void comboBox26_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(comboBox26.Text)) return;
            mCoreMode = comboBox26.Text;
            if(mCoreMode == "Multi Core")
            {
                numCoresBox.Enabled = true;
                hostQueueDepthBox.Enabled = false;
                comboBox24.Items.Clear();
                comboBox24.Items.Add("IOSize");
                comboBox24.SelectedIndex = 0;
            }
            else
            {
                numCoresBox.Enabled = false;
                hostQueueDepthBox.Enabled = true;
                comboBox24.Items.Clear();
                comboBox24.Items.Add("IOSize");
                comboBox24.Items.Add("Queue Depth");
                comboBox24.SelectedIndex = 0;
            }
            //if(comboBox26.Text.Equals("Single Core"))
            //{
            //    mode = false;
            //}
            //else
            //{
            //    mode = true;
            //}
        }

        private void numCoresBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            numCores = numCoresBox.Text;
        }

        
        private void cmdOHTextBox_TextChanged_2(object sender, EventArgs e)
        {
            resetButtons();
            cmdTransferTime = "10";
            cmdTransferTime = cmdOHTextBox.Text;
        }

        private void chart6_Click(object sender, EventArgs e)
        {

        }

       
        private void comboBox28_SelectedIndexChanged(object sender, EventArgs e)
        {
            //resetButtons();
            validParam = comboBox28.Text;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            string[] dirs = Directory.GetFiles(@"..\..\..\..\..\Workload\", "*.txt");
            if (checkBox4.Checked == true)
            {
                foreach (string dir in dirs)
                {
                    string file = dir.Remove(0, 24);// - "..\..\..\..\..\Workload\";
                    comboBox29.Items.Add(file);
                }
                comboBox29.Enabled = true;
                cwSizeBox.Enabled = false;
                pageSizeBox.Enabled = false;
                hostIOSizeBox.Enabled = false;
                //checkBox3.Enabled = false;
                numCmdsBox.Enabled = false;
                workloadTypeBox.Enabled = false;
                //comboBox24.Enabled = false;
                checkBox1.Enabled = false;
                //button3.Enabled = false;
                //comboBox26.Enabled = false;
                enableWrkld = "1";
            }
            else
            {
                if(comboBox29.Items.Count!=0)
                {
                    comboBox29.Items.Clear();
                }
                comboBox29.Enabled = false;
                cwSizeBox.Enabled = true;
                pageSizeBox.Enabled = true;
                hostIOSizeBox.Enabled = true;
                checkBox3.Enabled = true;
                numCmdsBox.Enabled = true;
                workloadTypeBox.Enabled = true;
                checkBox1.Enabled = true;
                
                //comboBox26.Enabled = true;
                enableWrkld = "0";
            }
        }

        private void comboBox29_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            wrkloadFile =  comboBox29.Text;
            wrkloadFile = " "+ "..\\..\\..\\..\\..\\Workload\\" + wrkloadFile.Trim();// String.Concat("..\\..\\..\\..\\..\\Workload\\", wrkloadFile);
            string fileName = "..\\..\\..\\..\\..\\Workload\\" + comboBox29.Text.Trim();
           
           
            wrkloadBS = blkSize.ToString();
           
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void disableGUIParamSelect()
        {
            panel1.Enabled = false;
            checkBox4.Enabled = false;
            checkBox3.Enabled = false;
            comboBox29.Enabled = false;
            numCoresBox.Enabled = false;
            comboBox26.Enabled = false;
            checkBox2.Enabled = false;
        }

        private void enableGUIParamSelect()
        {
            
            checkBox4.Enabled = true;
            checkBox3.Enabled = true;
            comboBox29.Enabled = true;
            //comboBox27.Enabled = true;
            comboBox26.Enabled = true;
            checkBox2.Enabled = true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            disableGUIParamSelect();
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            List<string> configLine = new List<string>();
            openFileDialog1.InitialDirectory = "..\\..\\..\\..\\..\\Configuration\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            List<string> config = new List<string>();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (StreamReader reader = new StreamReader(myStream))
                        {
                            while(reader.Peek() != -1)
                            {
                                string line = reader.ReadLine();
                                if(line.StartsWith("//"))
                                {
                                    continue;
                                }
                                else
                                {
                                    config.Add(line);
                                }
                            }
                            // Insert code to read the stream here.
                            reader.Close();
                        }
                        numCommands = config[0];
                        cmdType = config[1];
                        enableSeqLBA = config[2];
                        ioSize = config[3];
                        emCacheSize = config[4];
                        pageSize = config[5];
                        numChannel = config[6];
                        numDie = config[7];
                        numBanks = config[8];
                        queueFactor = config[9];
                        queueDepth = config[10];
                        numPages = config[11];
                        readTime = config[12];
                        programTime = config[13];
                        cmdTransferTime = config[14];
                        credit = config[15];
                        ddrSpeed = config[16];
                        onfiSpeed = config[17];
                        interfaceType = config[18];
                        enableLogs = config[19];
                        if(config[20] == "True")
                        {
                            enMultiSim = true;
                        }
                        else
                        {
                            enMultiSim = false;
                        }
                        if(enMultiSim)
                        {
                            button3.Enabled = true;
                            checkBox3.Enabled = true;
                            checkBox3.Checked = true;
                            runSimButton.Enabled = true;
                        }
                        else
                        {
                            button3.Enabled = false;
                            runSimButton.Enabled = true;
                        }
                        ioSizeVariation = bool.Parse(config[21]);
                        coreMode = config[22];
                        numCores = config[23];
                        enableWrkld = config[24];
                        wrkloadFile = config[25];
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
                myStream.Close();
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "..\\..\\..\\..\\..\\Configuration\\";
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            string path = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog1.FileName;
                //File.Create(path);
                using (FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("//Number of Commands");
                        writer.WriteLine(numCommands);
                        writer.WriteLine("//% Writes");
                        writer.WriteLine(cmdType);
                        writer.WriteLine("//SequentialLBA");
                        writer.WriteLine(enableSeqLBA);
                        writer.WriteLine("//Block Size");
                        writer.WriteLine(ioSize);
                        writer.WriteLine("//Code Word Size");
                        writer.WriteLine(emCacheSize);
                        writer.WriteLine("//Page Size");
                        writer.WriteLine(pageSize);
                        writer.WriteLine("//Number Of Channels");
                        writer.WriteLine(numChannel);
                        writer.WriteLine("//Number Of Die Per Channel");
                        writer.WriteLine(numDie);
                        writer.WriteLine("//Number of Banks Per Device");
                        writer.WriteLine(numBanks);
                        writer.WriteLine("//Queue Factor");
                        writer.WriteLine(queueFactor);
                        writer.WriteLine("//Queue Depth");
                        writer.WriteLine(queueDepth);
                        writer.WriteLine("//Number of Pages Per Bank");
                        writer.WriteLine(numPages);
                        writer.WriteLine("//Read Time");
                        writer.WriteLine(readTime);
                        writer.WriteLine("//Program Time");
                        writer.WriteLine(programTime);
                        writer.WriteLine("//ONFI Cmd Transfer Time");
                        writer.WriteLine(cmdTransferTime);
                        writer.WriteLine("//Credit");
                        writer.WriteLine(credit);
                        writer.WriteLine("//DDR Speed");
                        writer.WriteLine(ddrSpeed);
                        writer.WriteLine("//ONFI Speed");
                        writer.WriteLine(onfiSpeed);
                        writer.WriteLine("//Interface Type");
                        writer.WriteLine(interfaceType);
                        writer.WriteLine("//Enable logs");
                        writer.WriteLine(enableLogs);
                        writer.WriteLine("//Enable Multi Sim");
                        writer.WriteLine(enMultiSim);
                        writer.WriteLine("//IOSize Variation(true = IOSize, false = Queue Depth)");
                        writer.WriteLine(ioSizeVariation);
                        writer.WriteLine("//Core Mode (0 = Single CPU, 1= Multi Core)");
                        writer.WriteLine(coreMode);
                        writer.WriteLine("//Number Of Cores");
                        writer.WriteLine(numCores);
                        writer.WriteLine("//Enable Workload");
                        writer.WriteLine(enableWrkld);
                        writer.WriteLine("//Workload File");
                        writer.WriteLine(wrkloadFile);
                        
                        writer.Close();
                    }
                    file.Close();
                }
                
            }
            
        }

        List<string> config = new List<string>();
        private void simulationTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            progressBar1.Value = 0;
            simProgressLabel.Text = "Progress";
            progressBar2.Value = 0;
            graphLoadProgressLabel.Text = "Progress";
            if (simulationTypeBox.SelectedIndex == 0)
            {
                string[] dirs = Directory.GetFiles(@".\Configuration\", "*.txt");
                configFileBox.Items.Clear();
                foreach (string dir in dirs)
                {
                    string file = dir.Remove(0, 16);
                    configFileBox.Items.Add(file);
                }
                configFileBox.Enabled = true;
                configFileBox.SelectedIndex = 0;
                numCmdsBox.Enabled = false;
                panel6.Enabled = false;
                panel7.Enabled = false;
                panel9.Enabled = false;
                workloadTypeBox.Enabled = false;
                simTypeBox.Enabled = false;
                //comboBox32.Items.Clear();
                workloadFileBox.Enabled = false;
                enableWrkld = "0";
            }
            else if (simulationTypeBox.SelectedIndex == 1)
            {
                if (configFileBox.Items.Count != 0)
                {
                    configFileBox.Items.Clear();
                }
                configFileBox.Items.Add(configFile);
                configFileBox.Enabled = false;
                numCmdsBox.Enabled = true;
                panel6.Enabled = true;
                panel7.Enabled = true;
                panel9.Enabled = true;
                workloadTypeBox.Enabled = true;
                simTypeBox.Enabled = true;
                
                string cwd = parseConfigPath();
                SaveFileDialog saveFileDialog1 = saveDialogProperty(cwd);
              
                string path = "";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = saveFileDialog1.FileName;
                    //File.Create(path);
                    using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (StreamWriter writer = new StreamWriter(file))
                        {
                            saveFile(writer);
                            writer.Close();
                        }
                        file.Close();
                    }

                }
            }
            else
            {
                if (workloadTypeBox.Text == "Load Workload File")
                {
                    numCmdsBox.Enabled = false;
                    workloadFileBox.Enabled = true;
                    enableWrkld = "1";
                }
                else
                {
                    numCmdsBox.Enabled = true;
                    workloadFileBox.Enabled = false;
                    enableWrkld = "0";
                }
                panel6.Enabled = true;
                panel7.Enabled = true;
                panel9.Enabled = true;
                workloadTypeBox.Enabled = true;
                simTypeBox.Enabled = true;
                configFileBox.Enabled = false;
            }
        }

        private static SaveFileDialog saveDialogProperty(string cwd)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = cwd;
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            return saveFileDialog1;
        }

        private static string parseConfigPath()
        {
            string cwd = Directory.GetCurrentDirectory();
            cwd = cwd.Replace("GUI\\TeraSController\\TeraSController\\bin\\Release", "");
            cwd = cwd + "Configuration\\";
            return cwd;
        }

        private void saveFile(StreamWriter writer)
        {
            writer.WriteLine("[Simulation Parameters]");
            writer.WriteLine(";Number of Commands");
            writer.WriteLine("num_cmd= " + numCommands);
            writer.WriteLine(";Input Type");
            writer.WriteLine("input_type= " + workloadTypeBox.Text);
            writer.WriteLine(";Sim Type");
            writer.WriteLine("sim_type= " + simTypeBox.Text);
            writer.WriteLine(";Taffic Pattern");
            string pattern;
            if (enableSeqLBA == "100")
                pattern = "Sequential";
            else
                pattern = "Random";

            writer.WriteLine("pattern= " + pattern);
            //writer.WriteLine(";Workload");
            //writer.WriteLine(enableWrkld);
            writer.WriteLine(";Workload File");
            string fileName = wrkloadFile.Remove(0, 25);
            writer.WriteLine("workload_file= " + fileName);
            writer.WriteLine(";Enable logs");
            string isLogEn;
            if (enableLogs == "1")
                isLogEn = "true";
            else
                isLogEn = "false";
            writer.WriteLine("en_logs= " + isLogEn);
            writer.WriteLine(" ");
            writer.WriteLine("[Host Parameters]");
            writer.WriteLine(";IO Size");
            writer.WriteLine("io_size= " + ioSize);
            writer.WriteLine(";Queue Depth");
            writer.WriteLine("queue_depth= " + queueDepth);
            writer.WriteLine(";DDR Speed");
            writer.WriteLine("DDR Speed= " + ddrSpeed);
            writer.WriteLine(";Single Thread/Core");
            if (coreMode == "0")
                writer.WriteLine("MultiThread= " + "No");
            else
                writer.WriteLine("MultiThread= " + "Yes");
            writer.WriteLine(";Number Of Cores");
            writer.WriteLine("num_cores= " + numCores);

            writer.WriteLine(" ");
            writer.WriteLine("[Controller Parameters]");
            writer.WriteLine(";Block Size");
            writer.WriteLine("blk_size= " + blkSize);
            writer.WriteLine(";Code Word Size");
            writer.WriteLine("cw_size= " + emCacheSize);
            writer.WriteLine(";Number Of Channels");
            writer.WriteLine("num_chan= " + numChannel);
            writer.WriteLine(";Number Of Die Per Channel");
            writer.WriteLine("num_die= " + numDie);
            writer.WriteLine(";Queue Factor");
            writer.WriteLine("queue_factor= " + queueFactor);
            writer.WriteLine(";Credit");
            writer.WriteLine("credit= " + credit);

            writer.WriteLine(" ");
            writer.WriteLine("[Device Parameters]");
            writer.WriteLine(";Number of Banks Per Device");
            writer.WriteLine("num_banks= " + numBanks);
            writer.WriteLine(";Page Size");
            writer.WriteLine("page_size= " + pageSize);
            writer.WriteLine(";Number of Pages Per Bank");
            writer.WriteLine("num_pages= " + numPages);
            writer.WriteLine(";Read Time");
            writer.WriteLine("read_time= " + readTime);
            writer.WriteLine(";Program Time");
            writer.WriteLine("program_time= " + programTime);
            writer.WriteLine(";ONFI Cmd Transfer Time");
            writer.WriteLine("cmd_transfer_time= " + cmdTransferTime);
            writer.WriteLine(";ONFI Speed");
            writer.WriteLine("onfi_speed= " + onfiSpeed);
            writer.WriteLine(";Interface Type");
            writer.WriteLine("interface_type= " + interfaceType);

        }

        private void configFileBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            configFileBox.BackColor = Color.DarkGray;
            config.Clear();
            string fullPath = "..\\..\\..\\..\\..\\Configuration\\" + configFileBox.Text;
            try
            {
                using (FileStream file = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        while (reader.Peek() != -1)
                        {
                            string line = reader.ReadLine();
                            if (line.StartsWith(";") || line.StartsWith("[") || line.StartsWith(" "))
                            {
                                continue;
                            }
                            else
                            {
                                string[] tokens = line.Split('=');
                                config.Add(tokens[1]);
                            }
                        }
                        // Insert code to read the stream here.
                        reader.Close();
                    }
                    //Simulation Parameters
                    numCommands = config[0].Trim();
                    if (config[1] == "Load Workload File")
                    {
                        cmdType = "100";
                        enableSeqLBA = "0";
                    }
                    else if (config[1].Trim() == "Random 100% Read")
                    {
                        cmdType = "100";
                        enableSeqLBA = "0";
                    }
                    else if (config[1].Trim() == "Sequential 100% Read")
                    {
                        cmdType = "100";
                        enableSeqLBA = "100";
                    }
                    else if (config[1].Trim() == "Sequential 100% Write")
                    {
                        cmdType = "0";
                        enableSeqLBA = "100";
                    }
                    else if (config[1].Trim() == "Random 100% Write")
                    {
                        cmdType = "0";
                        enableSeqLBA = "0";
                    }
                    else if (config[1].Trim() == "Random 30/70 R/W")
                    {
                        cmdType = "30";
                        enableSeqLBA = "0";
                    }
                    else if (config[1].Trim() == "Random 70/30 R/W")
                    {
                        cmdType = "70";
                        enableSeqLBA = "0";
                    }
                    else if (config[1].Trim() == "Random 50/50 R/W")
                    {
                        cmdType = "50";
                        enableSeqLBA = "0";
                    }
                    simTypeBox.Text = config[2].Trim();
                    //if(config[3] =="Seq")
                    wrkloadFile = "../../../../../Workload/" + config[4].Trim();
                    if (config[5].Trim() == "false")
                        enableLogs = "0";
                    else
                        enableLogs = "1";

                    ioSize = config[6].Trim();
                    queueDepth = config[7].Trim();
                    ddrSpeed = config[8].Trim();
                    if (config[9].Trim() == "No")
                        coreMode = "0";
                    else
                        coreMode = "1";
                    numCores = config[10].Trim();

                    //Controller Parameters
                    blkSize = config[11].Trim();
                    emCacheSize = config[12].Trim();
                    numChannel = config[13].Trim();
                    numDie = config[14].Trim();
                    queueFactor = config[15].Trim();
                    credit = config[16].Trim();

                    numBanks = config[17].Trim();
                    pageSize = config[18].Trim();
                    numPages = config[19].Trim();
                    readTime = config[20].Trim();
                    programTime = config[21].Trim();
                    cmdTransferTime = config[22].Trim();
                    onfiSpeed = config[23].Trim();
                    interfaceType = config[24].Trim();
                    runSimButton.Enabled = true;
                    if (enMultiSim)
                    {
                       // button3.Enabled = true;
                        checkBox3.Enabled = true;
                        checkBox3.Checked = true;
                        //button2.Enabled = false;
                    }
                    else
                    {
                        button3.Enabled = false;
                        runSimButton.Enabled = true;
                    }
                    //ioSizeVariation = bool.Parse(config[21]);
                    
                    
                    //enableWrkld = config[24];
                    //wrkloadFile = config[25];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
                      
        }

        private void workloadFileBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            wrkloadFile = workloadFileBox.Text;
            wrkloadFile = " " + "../../../../../Workload/" + wrkloadFile.Trim();// String.Concat("..\\..\\..\\..\\..\\Workload\\", wrkloadFile);
            string fileName = "..\\..\\..\\..\\..\\Workload\\" + workloadFileBox.Text.Trim();
            // string cmdType = "";
            //string lba = "";
            //string cwCnt = "";
           
            //using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //{
            //    parseWorkLoadFile(ref lba, ref cwCnt, file);
            //}
            //int blockSize = int.Parse(cwCnt) * 512;
            wrkloadBS = blkSize;
        }

        private void parseWorkLoadFile(ref string lba, ref string cwCnt, FileStream file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                bool firstParse = true;
                int count = 0;
                string line = "";
                while (reader.Peek() != -1)
                {
                    if (firstParse)
                    {
                        line = reader.ReadLine();
                        string[] tokens = line.Split(' ');

                        cmdType = tokens[0];
                        lba = tokens[1];
                        cwCnt = tokens[2];
                        firstParse = false;
                        count++;
                    }
                    else
                    {
                        line = reader.ReadLine();
                        count++;
                    }

                }
                numCommands = count.ToString();
                reader.Close();
            }
        }

        private void blockSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            blkSize = blockSizeBox.Text;
        }

        private void simTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if(simTypeBox.Text == "Single Sim")
            {
                enMultiSim = false;
                if (workloadTypeBox.Text == "Load Workload File")
                {
                    hostIOSizeBox.Enabled = false;
                }
                else
                {
                    hostIOSizeBox.Enabled = true;
                }
                button3.Enabled = false;
                comboBox25.Enabled = false;
                comboBox25.Visible = false;
                comboBox28.Enabled = false;
                comboBox28.Visible = false;
                blockSizeBox.Enabled = true;
                if (!threadSelectBox.Checked)
                {
                    hostQueueDepthBox.Enabled = true;
                }
                runSimButton.Enabled = true;
                panel4.Enabled = false;
                label28.Visible = false;
                removeFiles = " 0";
            }
            else if(simTypeBox.Text == "Multi Sim based on Block Size")
            {
                multiSimParam = "BlockSize";
                if (workloadTypeBox.Text == "Load Workload File")
                {
                    hostIOSizeBox.Enabled = false;
                }
                else
                {
                    hostIOSizeBox.Enabled = true;
                }
                enMultiSim = true;
                runSimButton.Enabled = true;
                button3.Enabled = true;
                panel4.Enabled = true;
               
                blockSizeBox.Enabled = false;
                if (!threadSelectBox.Checked)
                {
                    hostQueueDepthBox.Enabled = true;
                }
                progressBar1.Value = 0;
                simProgressLabel.Text = "Progress";
                removeFiles = " 1";
                validBlockSize.Clear();
                validQueueDepth.Clear();
                               
            }
            else if(simTypeBox.Text == "Multi Sim based on QD")
            {
                multiSimParam = "Queue Depth";
                enMultiSim = true;
                runSimButton.Enabled = true;
                panel4.Enabled = true;
                if (workloadTypeBox.Text == "Load Workload File")
                {
                    hostIOSizeBox.Enabled = false;
                }
                else
                {
                    hostIOSizeBox.Enabled = true;
                }
                blockSizeBox.Enabled = true;
                hostQueueDepthBox.Enabled = false;
                removeFiles = " 1";
                validBlockSize.Clear();
                validQueueDepth.Clear();
                
            }
            else if (simTypeBox.Text == "Multi Sim based on IO Size")
            {
                multiSimParam = "IOSize";
                enMultiSim = true;
                runSimButton.Enabled = true;
                ///button3.Enabled = true;
                panel4.Enabled = true;
               
                blockSizeBox.Enabled = true;
                if (!threadSelectBox.Checked)
                {
                    hostQueueDepthBox.Enabled = true;
                }
                hostIOSizeBox.Enabled = false;
                removeFiles = " 1";
                validIOSize.Clear();
                validQueueDepth.Clear();
                
            }
        }

        private void threadSelectBox_CheckedChanged(object sender, EventArgs e)
        {
            if (threadSelectBox.Checked == true)
            {
                mCoreMode = "Multi Core";
                numCoresBox.Enabled = true;
                hostQueueDepthBox.Enabled = false;
                simTypeBox.Items.Clear();
                simTypeBox.Items.Add("Single Sim");
                simTypeBox.Items.Add("Multi Sim based on Block Size");
                if (!(workloadTypeBox.Text == "Load Workload File"))
                {
                    simTypeBox.Items.Add("Multi Sim based on IO Size");
                }
                simTypeBox.SelectedIndex = 0;
            }
            else
            {
                mCoreMode = "Single CPU";
                numCoresBox.Enabled = false;
                hostQueueDepthBox.Enabled = true;
                simTypeBox.Items.Clear();
                simTypeBox.Items.Add("Single Sim");
                simTypeBox.Items.Add("Multi Sim based on QD");
                simTypeBox.Items.Add("Multi Sim based on Block Size");
                if (!(workloadTypeBox.Text == "Load Workload File"))
                {
                    simTypeBox.Items.Add("Multi Sim based on IO Size");
                }
                simTypeBox.SelectedIndex = 0;

            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("http://www.crossbar-inc.com");
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void pollingStatusTextBox_TextChanged(object sender, EventArgs e)
        {
            resetButtons();
            pollWaitTime = "100";
            pollWaitTime = pollingStatusTextBox.Text;
        }
     
        private void resetButtons()
        {
            progressBar1.Value = 0;
            simProgressLabel.Text = "Progress...";
            progressBar2.Value = 0;
            graphLoadProgressLabel.Text = "Progress...";
            loadGraphButton.Enabled = false;
        }

        private void showButtons()
        {
            queueFactorBox.Enabled = true;
            queueFactorBox.Visible = true;
            label19.Visible = true;
        }

        private void hideButtons()
        {
            queueFactorBox.Enabled = false;
            queueFactorBox.Visible = false;
            label19.Visible = false;
        }
       
        private void comboBox35_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chart13_Click(object sender, EventArgs e)
        {

        }

        bool isButtonClicked = true;
        private void button6_Click(object sender, EventArgs e)
        {
            if(isButtonClicked)
            {
                showButtons();
                isButtonClicked = false;
            }
            else
            {
                hideButtons();
                isButtonClicked = true;
            }
        }
        private void chartPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            chartPanel.Focus();
        }
        bool initFlag = true;
        int lastZoomScale = 1;
        private void comboBox35_SelectedIndexChanged_1(object sender, EventArgs e)
        {
           // if (comboBox35.Text.Trim() == "1") return;
            if(initFlag)
            {
                initFlag = false;
                return;
            }
            zoomScale = int.Parse(comboBox35.Text.Trim());
            ddr_onfiBusUtilChart.SuspendLayout();
            if(lastZoomScale < zoomScale)
            {
            ddr_onfiBusUtilChart.Width *= zoomScale;
            }
            else if(lastZoomScale > zoomScale)
            {
                ddr_onfiBusUtilChart.Width = (ddr_onfiBusUtilChart.Width /lastZoomScale) * zoomScale;
            }
            lastZoomScale = zoomScale;
            lockChart13Position();
            ddr_onfiBusUtilChart.ResumeLayout();
        }

        string mValidIoParam = "512";
        private void comboBox36_SelectedIndexChanged(object sender, EventArgs e)
        {
            mValidIoParam = comboBox36.Text;
            //if (comboBox33.Text == MULTISIM_BS)
            //    CommandVsBlockSize(enMultiSim);
            //else if (comboBox33.Text == MULTISIM_IO)
            //    CommandVsIOSize(enMultiSim);
        }

        private void comboBox37_SelectedIndexChanged(object sender, EventArgs e)
        {
            ioSize = comboBox37.Text;
            if (ioSize.Trim() == "512")
            {
                numCommands = "1024";
                blkSize = "512";
            }
            else if (ioSize.Trim() == "4096")
            {
                numCommands = "256";
                blkSize = "4096";
            }
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        //private void Panel5_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    panel5.Focus();
        //}
        }

   
      
}
