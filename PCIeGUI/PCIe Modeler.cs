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
using System.Timers;
namespace PCIeGUI
{
    public partial class PCIeControllerGUI : Form
    {
        public PCIeControllerGUI()
        {
            InitializeComponent();
            initControlSettings();
            initSimParam();
            
            this.MouseWheel += new MouseEventHandler(chartPanel_MouseWheel);
            //this.MouseWheel += new MouseEventHandler(parameterPanel_EventHandler);
            //this.chart13.MouseClick += new MouseEventHandler(chart13_SelectionRangeChanged);
            string reportPath = @".\Reports_PCIe";
            System.IO.DirectoryInfo dir = new DirectoryInfo(reportPath);
            if (System.IO.Directory.Exists(reportPath))
            {
                foreach (FileInfo file in dir.GetFiles()) file.Delete();
            }
            else
            {
                System.IO.Directory.CreateDirectory(reportPath);
            }



            InitializeToolTip();

            showButtons();
            //hideButtons();
        }

        private void initSimParam()
        {
            if (FlashDemo)
            {
                pollWaitTime = "100";
                numChannel = "16";
                credit = "5";
                onfiSpeed = "400";
                interfaceType = "DDR";
                programTime = "2000";
                readTime = "1000";
                pageSize = "256";
                numPages = "1024";
                numBanks = "8";
                numDie = "2";
                enableLogs = "0";
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
                button2.Enabled = true;
                cntrlQueueDepth = "32";
                selectQueueDepthBox.Items.Clear();
                if (workLoadTypeBox.Text == "Load Workload File")
                {
                    hostIOSizeBox.Enabled = false;
                }
                else
                {
                    hostIOSizeBox.Enabled = true;
                }
                blockSizeBox.Enabled = true;
                HostQDepthBox.Enabled = false;
                removeFiles = " 1";
                validBlockSize.Clear();
                validQueueDepth.Clear();

            }
            else
            {
                pollWaitTime = pollingStatusBox.Text;
                numChannel = numChannelBox.Text;
                ioSize = hostIOSizeBox.Text;
                blkSize = blockSizeBox.Text;
                //ddrSpeed = comboBox13.Text;
                credit = outstandingCmdBox.Text;
                onfiSpeed = onfiSpeedBox.Text;
                interfaceType = onfiInterfaceTypeBox.Text;
                programTime = programTimeBox.Text;
                readTime = readTimeBox.Text;
                pageSize = pageSizeBox.Text;
                numPages = numPagesBox.Text;
                numBanks = numBanksBox.Text;
                numDie = numDieBox.Text;
                if(enLogsCheckBox.Checked)
                    enableLogs = "1";
                else
                    enableLogs = "0";
                emCacheSize = cwSizeBox.Text;
                //sqSize = ;
                //cqSize = comboBox5.Text;
                numCommands = numCommandsBox.Text;
                queueFactor = queueFactorBox.Text;
                timeScale = comboBox22.Text;
                depthOfQueue = queueDepth;
                //coreMode = checkBox5.Checked.ToString();
                //numCores = comboBox27.Text;
                cmdTransferTime = cmdOHTextBox.Text;
                zoomScale = int.Parse(comboBox35.Text);
                enMultiSim = false;
                hostQDepth = HostQDepthBox.Text;
            }
                cmdType = "2";
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
                //wrkloadFile = " ../../../../../Workload/default.txt ";
                //enableWrkld = "0";
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
        string pcieSpeed;
        string sqSize;
        string cqSize;
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
        string cmdPct;
        string rdBuffSize;
        string wrBuffSize;

        //string cmdTransferTime;
        int numSlot;
        int cwNum;
        int queueSize;
        int cwCnt;
        int timeScaleVar;
        string depthOfQueue;
        int onfiClock;
        int mode;
        string coreMode;
        bool enMultiSim;
        int zoomScale;
        string wrkloadBS;
        string wrkloadFile;
        string enableWrkld;
        string mCoreMode;
        string numCores;
        bool ioSizeVariation;
        string removeFiles;
        string cmdTransferTime;
        string validParam;
        string configFile;
        string cntrlQueueDepth;
        string hostQDepth;
        bool FlashDemo = false;

        List<long> chanTransCount = new List<long>();
        List<int> slotNum = new List<int>();
        List<double> latency = new List<double>();
        List<string> IOSize = new List<string>();
        string validMultiSimParam = "1";
        double latencyMaxima = 0;
        double latencyMinima = 0;
        double latencyAverage = 0;
        double iopsAverage = 0;
        List<double> maxLatency = new List<double>();
        List<double> minLatency = new List<double>();
        List<double> avgLatency = new List<double>();
        List<double> avgIOPS = new List<double>();
        string multiSimParam = "";
        const string MULTISIM_IO = "Multi Sim based on IO Size";
        const string MULTISIM_QD = "Multi Sim based on QD";

        #region initialization_routine
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

            button1.Enabled = false;

            if (FlashDemo)
            {
                iopsVsIosizeChart.Enabled = false;
                iopsVsIosizeChart.Visible = false;

                cmdCntVslbaChart.Enabled = false;
                cmdCntVslbaChart.Visible = false;

                onfi_PCIeUtilChart.Enabled = false;
                onfi_PCIeUtilChart.Visible = false;
                bankUtilChart.Enabled = false;
                bankUtilChart.Visible = false;
                channelUtilChart.Enabled = false;
                channelUtilChart.Visible = false;

                latencyVsQDChart.Enabled = true;
                latencyVsQDChart.Visible = true;
                iopsVsQdChart.Visible = true;
                latencyVsIopsVsQDChart.Visible = true;
                iopsVsQdChart.Enabled = true;
                latencyVsIopsVsQDChart.Enabled = true;
                label32.Enabled = false;
                label32.Visible = false;
                label28.Enabled = false;
                label28.Visible = false;
                selectIoSizeBox.Enabled = false;
                selectIoSizeBox.Visible = false;
              
                comboBox37.SelectedIndex = 0;
                iopsVsIosizeChart.Enabled = false;
                iopsVsIosizeChart.Visible = false;
                latencyVsIosizeChart.Enabled = false;
                latencyVsIosizeChart.Visible = false;
                latencyVsIopsVsIoSizeChart.Enabled = false;
                latencyVsIopsVsIoSizeChart.Visible = false;
                int yLocation = 0;
                this.iopsVsQdChart.Location = new Point(1, yLocation);
                this.latencyVsQDChart.Location = new Point(1, yLocation + 292);
                this.latencyVsIopsVsQDChart.Location = new Point(1, yLocation + 584);
                this.chartPanel.ScrollControlIntoView(iopsVsQdChart);
                this.chartPanel.AutoScroll = true;

            }
            else
            {
                //comboBox19.Items.AddRange(twos.ToArray());
                //comboBox19.AutoCompleteCustomSource.AddRange(twos.ToArray());
                hostIOSizeBox.SelectedIndex = 0;
                workLoadTypeBox.SelectedIndex = 1;
                cwSizeBox.SelectedIndex = 2;
                pageSizeBox.SelectedIndex = 1;
                numChannelBox.SelectedIndex = 3;
                numDieBox.SelectedIndex = 1;
                numBanksBox.SelectedIndex = 3;
                queueFactorBox.SelectedIndex = 3;
                HostQDepthBox.SelectedIndex = 0;
                numPagesBox.SelectedIndex = 0;
                readTimeBox.SelectedIndex = 0;
                programTimeBox.SelectedIndex = 0;
                outstandingCmdBox.SelectedIndex = 0;
                outstandingCmdBox.SelectedIndex = 0;
                onfiInterfaceTypeBox.SelectedIndex = 1;
                comboBox22.SelectedIndex = 0;
                cqDepthBox.SelectedIndex = 0;
                //PCIe Speed
                PCIeGenBox.SelectedIndex = 0;
                PCIeLaneBox.SelectedIndex = 0;

                inputTypeBox.SelectedIndex = 0;
                simulationTypeBox.SelectedIndex = 0;

                //Controller sector size disabled
                blockSizeBox.Visible = false;
                label35.Visible = false;
                cqDepthBox.Visible = false;
                label11.Visible = false;
                //Controller Queue Depth
                contrlQueueDepthBox.SelectedIndex = 0;

                comboBox35.SelectedIndex = 0;

                label32.Visible = false;
                label28.Visible = false;
               
                selectIoSizeBox.Enabled = false;

                readBufferSizeBox.SelectedIndex = 1;
                writeBufferSizeBox.SelectedIndex = 1;

                crossbarLinkLabel.Size = new Size(170, 20);

                crossbarLinkLabel.AutoSize = false;
                crossbarLinkLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
                this.chartPanel.ScrollControlIntoView(iopsVsIosizeChart);
            }

        }

        private void InitializeToolTip()
        {
            System.Windows.Forms.ToolTip TextTip1 = new System.Windows.Forms.ToolTip();
            TextTip1.SetToolTip(this.numCommandsBox, "Number of commands to be sent to the memory");

            System.Windows.Forms.ToolTip ComboTip23 = new System.Windows.Forms.ToolTip();
            ComboTip23.SetToolTip(this.workLoadTypeBox, "Type of commands: 0:read Only,100: write Only or mix of both");

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
            comboTip6.SetToolTip(this.numDieBox, "Number of memory die per channel");

            System.Windows.Forms.ToolTip comboTip14 = new System.Windows.Forms.ToolTip();
            comboTip14.SetToolTip(this.numBanksBox, "Number of banks per die");

            System.Windows.Forms.ToolTip comboTip20 = new System.Windows.Forms.ToolTip();
            comboTip20.SetToolTip(this.queueFactorBox, "Factor to determine maximum command queue size; Queue Size is calculated as number of channels X number of logical banks X queue factor");

            System.Windows.Forms.ToolTip comboTip19 = new System.Windows.Forms.ToolTip();
            comboTip19.SetToolTip(this.HostQDepthBox, "Submission Queue Depth");

            System.Windows.Forms.ToolTip comboTip15 = new System.Windows.Forms.ToolTip();
            comboTip15.SetToolTip(this.numPagesBox, "Number of pages (in bytes) per bank in memory");

            System.Windows.Forms.ToolTip comboTip17 = new System.Windows.Forms.ToolTip();
            comboTip17.SetToolTip(this.readTimeBox, "Read access time per bank in memory");

            System.Windows.Forms.ToolTip comboTip18 = new System.Windows.Forms.ToolTip();
            comboTip18.SetToolTip(this.programTimeBox, "Write access time per bank in memory");

            System.Windows.Forms.ToolTip comboTip11 = new System.Windows.Forms.ToolTip();
            comboTip18.SetToolTip(this.outstandingCmdBox, "Maximum read credit for each ONFI channel");

            System.Windows.Forms.ToolTip comboTip13 = new System.Windows.Forms.ToolTip();
            comboTip13.SetToolTip(this.PCIeGenBox, "DDR clock frequency");

            System.Windows.Forms.ToolTip textTip2 = new System.Windows.Forms.ToolTip();
            textTip2.SetToolTip(this.onfiSpeedBox, "ONFI clock frequency");

            System.Windows.Forms.ToolTip comboTip21 = new System.Windows.Forms.ToolTip();
            comboTip21.SetToolTip(this.onfiInterfaceTypeBox, "ONFI interface Type");

            System.Windows.Forms.ToolTip comboTip22 = new System.Windows.Forms.ToolTip();
            comboTip22.SetToolTip(this.comboBox22, "X axis time Scale ( in us or ms) for the plots");

            System.Windows.Forms.ToolTip checkTip2 = new System.Windows.Forms.ToolTip();
            checkTip2.SetToolTip(this.enLogsCheckBox, "Enable simulation log dump");

            //System.Windows.Forms.ToolTip checkTip3 = new System.Windows.Forms.ToolTip();
            //checkTip3.SetToolTip(this.checkBox3, "Check this box to enable multi sim environment");

            //System.Windows.Forms.ToolTip comboTip24 = new System.Windows.Forms.ToolTip();
            //comboTip24.SetToolTip(this.comboBox24, "Select the parameter to vary in multi sim");

            //System.Windows.Forms.ToolTip buttonTip3 = new System.Windows.Forms.ToolTip();
            //buttonTip3.SetToolTip(this.button3, "Click this button to run multiple simulations");

            System.Windows.Forms.ToolTip progressBarTip1 = new System.Windows.Forms.ToolTip();
            progressBarTip1.SetToolTip(this.progressBar1, "Simulation Progress Bar..wait for complete status");

            System.Windows.Forms.ToolTip buttonTip1 = new System.Windows.Forms.ToolTip();
            buttonTip1.SetToolTip(this.button1, "click to load the charts after running simulation ");

            System.Windows.Forms.ToolTip buttonTip2 = new System.Windows.Forms.ToolTip();
            buttonTip2.SetToolTip(this.button2, "click to run simulation");

            System.Windows.Forms.ToolTip comboTip30 = new System.Windows.Forms.ToolTip();
            comboTip30.SetToolTip(this.inputTypeBox, "Select to change configuration parameter settings");

            System.Windows.Forms.ToolTip comboTip33 = new System.Windows.Forms.ToolTip();
            comboTip33.SetToolTip(this.simulationTypeBox, "Select to change simulation types");

            System.Windows.Forms.ToolTip textTip8 = new System.Windows.Forms.ToolTip();
            textTip8.SetToolTip(this.pollingStatusBox, "Completion Queue polling interval");

            //System.Windows.Forms.ToolTip checkTip5 = new System.Windows.Forms.ToolTip();
            //checkTip5.SetToolTip(this.checkBox5, "check to select multiple cores");

            //System.Windows.Forms.ToolTip comboTip27 = new System.Windows.Forms.ToolTip();
            //comboTip27.SetToolTip(this.comboBox27, "Select number of cores");

            System.Windows.Forms.ToolTip comboTip34 = new System.Windows.Forms.ToolTip();
            comboTip34.SetToolTip(this.blockSizeBox, "Block Size configuration");
        }
        #endregion
        #region buttons_callbacks
        private void runSimButton_Click(object sender, EventArgs e)
        {
            resetButtons();
            pcieSpeed = setPcieSpeed();
            if (!FlashDemo)
            {
                sqSize = " " + sqSize.Trim();
                cqSize = " " + cqSize.Trim();
            }
            pollWaitTime = " " + pollWaitTime.Trim();
            if (int.Parse(ioSize) < int.Parse(emCacheSize))
            {
                MessageBox.Show("IO Size is less than CW Size, setting both to CW Size");
                ioSize = emCacheSize;
            }
            else
            {
                if (!FlashDemo)
                {
                    ioSize = hostIOSizeBox.Text;
                }
            }
            //if (int.Parse(blkSize) < int.Parse(emCacheSize))
            //{
            //    MessageBox.Show("ERROR: Code Word Size is more than Block Size: ABORT SIMULATION!!");
            //}
            if (int.Parse(emCacheSize) < int.Parse(pageSize))
            {
                MessageBox.Show("ERROR: Page Size is more than Code Word Size: ABORT SIMULATION!!");
            }
            else
            {
               // singleSimCall();
                if (enMultiSim)
                {
                    multiSimCall();
                }
                else
                {
                    singleSimCall();
                }
            }
            button1.Enabled = true;
        }

        private void loadGraph_Click(object sender, EventArgs e)
        {
            resetLoadGraphProgressBar();
            progressLabel1.Text = "Loading...";
            resetDefaultCharts();

            if (enMultiSim)
            {
                resetMultiSimCharts();

                if (FlashDemo)
                {
                    iopsVsIosizeChart.Enabled = false;
                    iopsVsIosizeChart.Visible = false;

                    latencyVsIosizeChart.Enabled = false;
                    latencyVsIosizeChart.Visible = false;

                    latencyVsIopsVsIoSizeChart.Enabled = false;
                    latencyVsIopsVsIoSizeChart.Visible = false;

                    latencyVsQDChart.Enabled = true;
                    latencyVsQDChart.Visible = true;

                    iopsVsQdChart.Enabled = true;
                    iopsVsQdChart.Visible = true;

                    latencyVsIopsVsQDChart.Enabled = true;
                    latencyVsIopsVsQDChart.Visible = true;

                    iopsVsIosizeChart.Enabled = false;
                    iopsVsIosizeChart.Visible = false;

                    cmdCntVslbaChart.Enabled = false;
                    cmdCntVslbaChart.Visible = false;

                    //chart9.Enabled = true;
                    //chart9.Visible = true;

                    bankUtilChart.Enabled = false;
                    bankUtilChart.Visible = false;

                    channelUtilChart.Enabled = false;
                    channelUtilChart.Visible = false;
                    selectQueueDepthBox.Enabled = false;
                    selectQueueDepthBox.Visible = false;

                    this.chartPanel.ScrollControlIntoView(iopsVsQdChart);
                    int yLocation = 0;
                    this.iopsVsQdChart.Location = new Point(3, yLocation);

                    this.latencyVsQDChart.Location = new Point(3, yLocation + 292);
                    this.latencyVsIopsVsQDChart.Location = new Point(3, yLocation + 584);
                    //this.chart9.Location = new Point(3, yLocation + 876);
                    //this.selectQueueDepth.Location = new Point(879, yLocation + 876);
                    selectIoSizeBox.Enabled = false;
                    selectIoSizeBox.Visible = false;
                    
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

                    loadMultiSimWithQDGraph();
                    HostQDepthBox.Enabled = true;

                }//multi sim with QD
                else //multisim with IOSize
                {

                    loadMultiSimWithIOSizeGraph();
                   
                   
                }//multi sim with IOSize

            }//multi sim
            else //single sim
            {
                loadSingleSimGraph();


            }//Single simulation

            latencyCalculation(enMultiSim);
            iopsCalculation(enMultiSim);
            if (FlashDemo)
            {
                IOPSVsQD(enMultiSim);
                LatencyVsQD(enMultiSim);
                LatencyVsIOPSwithQD(enMultiSim);
                //PCIBusUtilizationData(enMultiSim);
                //ONFIBusUtilizationData(enMultiSim);

            }
            else
            {
                if (simulationTypeBox.Text == "Multi Sim based on Block Size")
                {
                    IOPSVsBlockSize(enMultiSim);
                    LatencyVsBlockSize(enMultiSim);
                    CommandVsBlockSize(enMultiSim);
                    LatencyVsIOPSwithBlockSize(enMultiSim);
                }
                else
                {
                    IOPSVsIOSize(enMultiSim);
                    LatencyVsIOSize(enMultiSim);
                    CommandVsIOSize(enMultiSim);
                    LatencyVsIOPSwithIOSize(enMultiSim);
                }
                IOPSVsQD(enMultiSim);
                LatencyVsQD(enMultiSim);
                LatencyVsIOPSwithQD(enMultiSim);
                CommandVsLBA(enMultiSim);

                if (!disableONFI_PCIECheckBox.Checked)
                {

                    PCIBusUtilizationData(enMultiSim);
                    ONFIBusUtilizationData(enMultiSim);

                }
            }
            if (!enMultiSim)
            {
                resetLatencyIOPSTextBox();
                setLatencyIOPSTextBox();
            }
            progressBar2.Value = 5;
            progressLabel1.Text = "Complete...";

            chartPanel.AutoScroll = true;
            
        }

        private void loadSingleSimGraph()
        {
            this.chartPanel.ScrollControlIntoView(iopsVsIosizeChart);

            int yLocation = 0;
            this.iopsVsIosizeChart.Location = new Point(0, 2); //IOPS Vs IOSize
            this.iopsVsIosizeChart.Enabled = true;
            this.iopsVsIosizeChart.Visible = true;

            this.latencyVsIosizeChart.Location = new Point(0, 293);//Latency Vs IOSize
            this.latencyVsIosizeChart.Enabled = true;
            this.latencyVsIosizeChart.Visible = true;

            this.latencyVsIopsVsIoSizeChart.Location = new Point(0, 584);
            this.latencyVsIopsVsIoSizeChart.Enabled = true;
            this.latencyVsIopsVsIoSizeChart.Visible = true;

            this.iopsVsQdChart.Location = new Point(0, 875);//Latency Vs QD
            iopsVsQdChart.Enabled = true;
            iopsVsQdChart.Visible = true;

            this.latencyVsQDChart.Location = new Point(0, 1166);//IOPS Vs QD
            latencyVsQDChart.Enabled = true;
            latencyVsQDChart.Visible = true;

            this.latencyVsIopsVsQDChart.Location = new Point(0, 1457);
            latencyVsIopsVsQDChart.Enabled = true;
            latencyVsIopsVsQDChart.Visible = true;

            cmdCntVsLba_Label.Enabled = false;
            cmdCntVsLba_Label.Visible = false;
            selectQueueDepthBox.Enabled = false;
            selectQueueDepthBox.Visible = false;
            //this.comboBox28.Location = new Point(760, yLocation + 1460);

            //If ONFI/PCI plot disabled
            if (disableONFI_PCIECheckBox.Checked)
            {
                this.label28.Visible = false;
                this.selectIoSizeBox.Visible = false;
                this.onfi_PCIeUtilChart.Enabled = false;
                this.onfi_PCIeUtilChart.Visible = false;
                this.cmdCntVsIOSizeChart.Location = new Point(0, 1748);//Cmd Vs LBA
                cmdCntVsIOSizeChart.Enabled = true;
                cmdCntVsIOSizeChart.Visible = true;

                this.cmdCntVslbaChart.Location = new Point(0, 2039);//Cmd Vs LBA
                cmdCntVslbaChart.Enabled = true;
                cmdCntVslbaChart.Visible = true;

                this.bankUtilChart.Location = new Point(0, 2330);
                bankUtilChart.Enabled = true;
                bankUtilChart.Visible = true;

                this.channelUtilChart.Location = new Point(0, 2621);
                channelUtilChart.Enabled = true;
                channelUtilChart.Visible = true;

            }
            else
            {
                this.onfi_PCIeUtilChart.Enabled = true;
                this.onfi_PCIeUtilChart.Visible = true;
                this.selectIoSizeBox.Visible = false;
                this.onfi_PCIeUtilChart.Location = new Point(0, 1748);//DDR/ONFI


                this.cmdCntVsIOSizeChart.Location = new Point(0, 2139);//Cmd Vs LBA
                cmdCntVsIOSizeChart.Enabled = true;
                cmdCntVsIOSizeChart.Visible = true;

                this.cmdCntVslbaChart.Location = new Point(0, 2430);//Cmd Vs LBA
                cmdCntVslbaChart.Enabled = true;
                cmdCntVslbaChart.Visible = true;

                this.bankUtilChart.Location = new Point(0, 2721);//bank Util
                bankUtilChart.Enabled = true;
                bankUtilChart.Visible = true;

                this.channelUtilChart.Location = new Point(0, 3012);//channel Util
                channelUtilChart.Enabled = true;
                channelUtilChart.Visible = true;


            }
            pci_onfi_label.Visible = false;
            pci_onfi_label.Enabled = false;
            label32.Enabled = false;
            label32.Visible = false;
            //comboBox28.Enabled = false;
            //comboBox28.Visible = false;
            iopsVsIosizeChart.Enabled = true;
            iopsVsIosizeChart.Visible = true;

            latencyVsIosizeChart.Enabled = true;
            latencyVsIosizeChart.Visible = true;

            latencyVsQDChart.Enabled = true;
            latencyVsQDChart.Visible = true;

            iopsVsQdChart.Enabled = true;
            iopsVsQdChart.Visible = true;

            iopsVsIosizeChart.Enabled = true;
            iopsVsIosizeChart.Visible = true;

            selectIoSizeBox.Enabled = false;
            selectIoSizeBox.Visible = false;
            chartPanel.ResumeLayout();
            bankUtilizationData();
            channelUtilizationData();

            //if (cmdType != "Sequential 100% Write" || cmdType != "Random 100% Write")
            //{
            //    shortQueueUtilizationData();
            //}
            //longQueueUtilizationData();
        }

        private void loadMultiSimWithIOSizeGraph()
        {
            latencyVsIopsVsQDChart.Enabled = false;
            latencyVsIopsVsQDChart.Visible = false;

            iopsVsIosizeChart.Enabled = true;
            iopsVsIosizeChart.Visible = true;

            cmdCntVslbaChart.Enabled = true;
            cmdCntVslbaChart.Visible = true;

            latencyVsQDChart.Enabled = false;
            latencyVsQDChart.Visible = false;
            this.chartPanel.ScrollControlIntoView(iopsVsIosizeChart);

            this.iopsVsIosizeChart.Location = new Point(0, 2);
            this.iopsVsIosizeChart.Enabled = true;
            this.iopsVsIosizeChart.Visible = true;

            chartPanel.SuspendLayout();

            this.chartPanel.ScrollControlIntoView(iopsVsIosizeChart);
            chartPanel.ResumeLayout();

            this.latencyVsIosizeChart.Location = new Point(0, 293);
            this.latencyVsIosizeChart.Enabled = true;
            this.latencyVsIosizeChart.Visible = true;

            this.latencyVsIopsVsIoSizeChart.Location = new Point(0, 584);
            this.latencyVsIopsVsIoSizeChart.Enabled = true;
            this.latencyVsIopsVsIoSizeChart.Visible = true;


            if (disableONFI_PCIECheckBox.Checked)
            {
                this.onfi_PCIeUtilChart.Enabled = false;
                this.onfi_PCIeUtilChart.Visible = false;
                this.selectIoSizeBox.Visible = false;
                this.cmdCntVsIOSizeChart.Location = new Point(0, 875);
                this.cmdCntVsIOSizeChart.Enabled = true;
                this.cmdCntVsIOSizeChart.Visible = true;

                cmdCntVsLba_Label.Location = new Point(870, 1166);
                cmdCntVsLba_Label.Visible = true;
                cmdCntVsLba_Label.Enabled = true;
                cmdCntVsLba_Label.Text = "IO Size";

                this.cmdCntVslbaChart.Location = new Point(0, 1166);
                this.cmdCntVslbaChart.Enabled = true;
                this.cmdCntVslbaChart.Visible = true;

                this.selectQueueDepthBox.Location = new Point(920, 1166);
                this.selectQueueDepthBox.Enabled = true;
                this.selectQueueDepthBox.Visible = true;

                //this.bankUtilChart.Location = new Point(0, 1748);
                this.bankUtilChart.Enabled = false;
                this.bankUtilChart.Visible = false;

                //this.channelUtilChart.Location = new Point(0, 2039);
                this.channelUtilChart.Enabled = false;
                this.channelUtilChart.Visible = false;
                this.label28.Visible = false;
                selectIoSizeBox.Enabled = false;
                selectIoSizeBox.Visible = false;
                pci_onfi_label.Visible = false;
                pci_onfi_label.Enabled = false;

            }
            else
            {
                this.onfi_PCIeUtilChart.Enabled = true;
                this.onfi_PCIeUtilChart.Visible = true;
                this.selectIoSizeBox.Visible = true;
                //this.label28.Visible = true;
                //this.label28.Location = new System.Drawing.Point(840, 875);
                pci_onfi_label.Visible = true;
                pci_onfi_label.Enabled = true;
                pci_onfi_label.Text = "IO Size";
                pci_onfi_label.Location = new Point(870, 875);
                this.selectIoSizeBox.Location = new System.Drawing.Point(920, 875);
                this.onfi_PCIeUtilChart.Location = new System.Drawing.Point(0, 875);

                this.cmdCntVsIOSizeChart.Location = new Point(0, 1266);
                this.cmdCntVsIOSizeChart.Enabled = true;
                this.cmdCntVsIOSizeChart.Visible = true;

                this.cmdCntVslbaChart.Location = new Point(0, 1557);
                this.cmdCntVslbaChart.Enabled = true;
                this.cmdCntVslbaChart.Visible = true;

                cmdCntVsLba_Label.Location = new Point(870, 1557);
                cmdCntVsLba_Label.Visible = true;
                cmdCntVsLba_Label.Enabled = true;
                cmdCntVsLba_Label.Text = "IO Size";

                this.selectQueueDepthBox.Location = new Point(920, 1557);
                this.selectQueueDepthBox.Enabled = true;
                this.selectQueueDepthBox.Visible = true;

                this.bankUtilChart.Location = new Point(0, 1848);
                this.bankUtilChart.Enabled = false;
                this.bankUtilChart.Visible = false;

                this.channelUtilChart.Location = new Point(0, 2139);
                this.channelUtilChart.Enabled = false;
                this.channelUtilChart.Visible = false;
                selectIoSizeBox.Enabled = true;
                selectIoSizeBox.Visible = true;


            }
        }

        private void loadMultiSimWithQDGraph()
        {
            

            this.chartPanel.ScrollControlIntoView(iopsVsIosizeChart);

            this.iopsVsQdChart.Location = new Point(0, 2);
            iopsVsQdChart.Enabled = true;
            iopsVsQdChart.Visible = true;

            this.latencyVsQDChart.Location = new Point(0, 293);
            latencyVsQDChart.Enabled = true;
            latencyVsQDChart.Visible = true;

            this.latencyVsIopsVsQDChart.Location = new Point(0, 584);
            latencyVsIopsVsQDChart.Visible = true;
            latencyVsIopsVsQDChart.Enabled = true;


            if (isDisableBusUtilChartSelected())
            {
                this.onfi_PCIeUtilChart.Enabled = false;
                this.onfi_PCIeUtilChart.Visible = false;


                this.cmdCntVsIOSizeChart.Location = new Point(0, 875);
                this.cmdCntVsIOSizeChart.Enabled = true;
                this.cmdCntVsIOSizeChart.Visible = true;

                this.cmdCntVslbaChart.Location = new Point(0, 1166);
                cmdCntVslbaChart.Enabled = true;
                cmdCntVslbaChart.Visible = true;
                selectQueueDepthBox.Location = new Point(920, 1166);
                selectQueueDepthBox.Visible = true;
                selectQueueDepthBox.Enabled = true;

                cmdCntVsLba_Label.Location = new Point(845, 1166);
                cmdCntVsLba_Label.Visible = true;
                cmdCntVsLba_Label.Enabled = true;

                this.bankUtilChart.Location = new Point(0, 1457);
                bankUtilChart.Visible = false;
                bankUtilChart.Enabled = false;

                this.channelUtilChart.Location = new Point(0, 1748);
                channelUtilChart.Visible = false;
                channelUtilChart.Enabled = false;

                selectIoSizeBox.Enabled = false;
                selectIoSizeBox.Visible = false;
            }
            else
            {
                this.onfi_PCIeUtilChart.Enabled = true;
                this.onfi_PCIeUtilChart.Visible = true;
                pci_onfi_label.Location = new Point(845, 875);
                pci_onfi_label.Visible = true;
                pci_onfi_label.Enabled = true;
                pci_onfi_label.Text = "Queue Depth";
                this.selectIoSizeBox.Location = new Point(920, 875);
                selectIoSizeBox.Enabled = true;
                selectIoSizeBox.Visible = true;
                this.onfi_PCIeUtilChart.Location = new Point(0, 875);

                this.cmdCntVsIOSizeChart.Location = new Point(0, 1266);
                this.cmdCntVsIOSizeChart.Enabled = true;
                this.cmdCntVsIOSizeChart.Visible = true;

                this.cmdCntVslbaChart.Location = new Point(0, 1557);
                cmdCntVslbaChart.Enabled = true;
                cmdCntVslbaChart.Visible = true;
                selectQueueDepthBox.Location = new Point(920, 1557);
                selectQueueDepthBox.Visible = true;
                selectQueueDepthBox.Enabled = true;

                cmdCntVsLba_Label.Location = new Point(845, 1557);
                cmdCntVsLba_Label.Visible = true;
                cmdCntVsLba_Label.Enabled = true;
                cmdCntVsLba_Label.Text = "Queue Depth";

                this.bankUtilChart.Location = new Point(0, 1848);
                bankUtilChart.Visible = false;
                bankUtilChart.Enabled = false;

                this.channelUtilChart.Location = new Point(0, 2139);
                channelUtilChart.Visible = false;
                channelUtilChart.Enabled = false;
            }
        }
        #endregion

        #region button_callbacks_helper
        private void resetSimButtonProgressBar(int min, int max)
        {
            progressBar1.Minimum = min;
            progressBar1.Maximum = max;
            progressBar1.Value = 0;
        }

        private void singleSimCall()
        {
            cwNum = getCwNum();
            if (cwNum !=0)
            {
                disableLoadGraphButton();
                //queueSize = getQueueSize();
                cwCnt = getCwCnt();
                numSlot = getNumSlot();
                //depthOfQueue = getQueueDepth().ToString();

                string enableMultiSim = " 0";
                setParameterString();
                resetSimButtonProgressBar(0, 5);
                /* Reset  validIOSize parameter */

                validIOSize.Clear();
                validIOSize.Add(ioSize);
                startSingleSim(enableMultiSim);
                setSimButtonProgressBar(5);
                enableGUIParamSelect();

                if (enableWrkld == "1")
                {
                    initParams();
                }
            }
        }
        
        Process pPCIeNVM;
        
        private void startQDMultiSim(string enMultiSim, List<string> queueD, int queueIndex)
        {
            string var = "";


            if (enableWrkld == "1")
            {
                string enWrkld = " " + enableWrkld.Trim();
                coreMode = " 0";
                var = numCommands + cntrlQueueDepth + queueFactor + ioSize + emCacheSize + numChannel + pageSize + readTime + programTime + credit + onfiClk
                     + numBanks + numDie + numPages + enMultiSim + coreMode + cmdTransferTime + ioSize + pcieSpeed + sqDepth[queueIndex] + sqDepth[queueIndex] + cmdType + pollWaitTime + queueD[queueIndex] + enWrkld + wrkloadFile
               + enableSeqLBA + cmdPct + wrBuffSize + rdBuffSize + removeFiles + enableLogs;
            }
            else
            {
                string enWrkld = " " + enableWrkld.Trim();
                coreMode = " 0";
                var = numCommands + cntrlQueueDepth + queueFactor + ioSize + emCacheSize + numChannel + pageSize + readTime + programTime + credit + onfiClk
                      + numBanks + numDie + numPages + enMultiSim + coreMode + cmdTransferTime + ioSize + pcieSpeed + sqDepth[queueIndex] + sqDepth[queueIndex] + cmdType + pollWaitTime + queueD[queueIndex] + enWrkld + wrkloadFile
                + enableSeqLBA + cmdPct + wrBuffSize + rdBuffSize + removeFiles + enableLogs;
            }
            //if (enableWrkld == "1")
            //{
            //    string enWrkld = " " + enableWrkld.Trim();
            //    wrkloadBS = " " + wrkloadBS.Trim();
            //    var = numCommands + wrkloadBS + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
            //    + numPages + queueD[queueIndex] + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
            //    + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
            //}
            //else
            //{
            //    string enWrkld = " " + enableWrkld.Trim();

            //    var = numCommands + ioSize + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
            //        + numPages + queueD[queueIndex] + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
            //        + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
            //}
            pPCIeNVM = new Process();

            try
            {
                //pPCIeNVM.StartInfo.FileName = @".\TeraSPCIeController.exe";
                //pPCIeNVM.StartInfo.FileName = @".\TeraSPCIeController.exe";
                pPCIeNVM.StartInfo.FileName = @".\TeraSPCIeController.exe";

                pPCIeNVM.StartInfo.Arguments = var; // Put your arguments here
                pPCIeNVM.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                pPCIeNVM.StartInfo.UseShellExecute = true;
                //pPCIeNVM.EnableRaisingEvents = true;
                //pPCIeNVM.Exited += new EventHandler(pPCIeNVM_Exited);
                pPCIeNVM.Start();
                progressLabel.Text = "Running...";
               pPCIeNVM.WaitForExit();
                double percentage = (double)((double)(queueIndex + 1) / queueD.Count()) * 100;
                progressBar1.Value = queueIndex + 1;
                removeFiles = " 0";
            }
            finally
            {
                if (pPCIeNVM != null)
                {
                    pPCIeNVM.Close();
                }

            }
        }

        //void pPCIeNVM_Exited(object sender, EventArgs e)
        //{
        //    pPCIeNVM = null;
        //    //processDone = true;
        //    //progressBar1.Value = queueIndex + 1;
        //    //removeFiles = " 0";
        //}
        bool processDone = true;
        
        private void multiSimCall()
        {
            cwNum = getCwNum();

            if (cwNum != 0)
            {
                disableLoadGraphButton();
                queueSize = getQueueSize();

                selectMultiSimOption();
                setParameterString();
                string enMultiSim = " 1";
                selectIoSizeBox.Visible = false;
                //comboBox28.Visible = false;
                //comboBox28.Enabled = false;
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

                            selectIoSizeBox.Items.Add(blockSize[blockIndex].Trim());
                            //comboBox28.Items.Add(blockSize[blockIndex].Trim());
                            //comboBox28.SelectedIndex = blockIndex;
                            selectIoSizeBox.SelectedIndex = blockIndex;
                            validParam = validBlockSize[blockIndex].ToString();
                            validMultiSimParam = validBlockSize[blockIndex].ToString();

                            string finalQDepth = " " + getQueueDepth().ToString();
                            validQDepth.Add(int.Parse(finalQDepth));

                            //startBlkSizeMultiSim(enMultiSim, blockSize, blockIndex, finalQDepth);
                        }
                        else
                        {
                            progressBar1.Value = blockSize.Count();
                            progressLabel.Text = "Complete...";

                        }
                    }
                    progressBar1.Value = blockSize.Count();
                    progressLabel.Text = "Complete...";

                }
                else if (multiSimParam == "Queue Depth")
                {
                    HostQDepthBox.Enabled = false;
                    List<string> queueD = new List<string>();
                    cwCnt = getCwCnt();
                    numSlot = getNumSlot();
                    // selectIoSizeBox.Items.Clear();
                    initQDParam(queueD);
                    resetSimButtonProgressBar(0, queueD.Count);

                    if (int.Parse(ioSize) < int.Parse(emCacheSize))
                    {
                        MessageBox.Show("ERROR: Code Word Size is more than IO Size: ABORT SIMULATION!!");
                    }
                    else
                    {
                        validMultiSimParam = "1";
                        //System.Timers.Timer timer = new System.Timers.Timer(1000);
                        //timer.Elapsed += new ElapsedEventHandler(timerEvent);
                        //for (int queueIndex = 0; queueIndex < queueD.Count(); queueIndex++)

                        for (int queueIndex = 0; queueIndex < queueD.Count(); queueIndex++)
                        {
                            int qDepthUpperLimit = 256;
                            //if (enableWrkld == "1")
                            //{
                            //    qDepthUpperLimit = 256;
                            //}
                            //else
                            //{
                            //    qDepthUpperLimit = int.Parse(numCommands);
                            //}
                            // if ((int.Parse(queueD[queueIndex].Trim()) <= numSlot) && (int.Parse(queueD[queueIndex].Trim()) <= qDepthUpperLimit))
                            if ((int.Parse(queueD[queueIndex].Trim()) <= qDepthUpperLimit))
                            {
                                validQueueDepth.Add(queueD[queueIndex].Trim());
                                if (!FlashDemo)
                                {
                                    selectIoSizeBox.Items.Add(validQueueDepth[queueIndex].ToString());
                                    selectIoSizeBox.SelectedIndex = queueIndex;
                                    //comboBox28.Items.Add(validQueueDepth[queueIndex].ToString());
                                    //comboBox28.SelectedIndex = queueIndex;

                                }
                                selectQueueDepthBox.Items.Add(validQueueDepth[queueIndex].ToString());
                                selectQueueDepthBox.SelectedIndex = queueIndex;

                                //selectIoSizeBox.Items.Add(validQueueDepth[queueIndex].ToString());
                                //selectIoSizeBox.SelectedIndex = queueIndex;

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
                else// multisim with IOSize
                {
                    if (int.Parse(emCacheSize) > 512)
                    {
                        MessageBox.Show(" CW Size is more than 512, setting CWSize to 512");
                        cwSizeBox.SelectedIndex = 1;
                        emCacheSize = " 512";

                    }
                    List<string> IOSize = new List<string>();
                    selectQueueDepthBox.Items.Clear();
                    initIOSizeParam(IOSize);
                    resetSimButtonProgressBar(0, IOSize.Count());
                    cwCnt = getCwCnt();
                    numSlot = getNumSlot();
                    // string finalQDepth = " " + getQueueDepth().ToString();
                    for (int ioSizeIndex = 0; ioSizeIndex < IOSize.Count(); ioSizeIndex++)
                    {
                        if (int.Parse(IOSize[ioSizeIndex].Trim()) >= int.Parse(emCacheSize))
                        {
                            validIOSize.Add(IOSize[ioSizeIndex].Trim());
                            selectIoSizeBox.Items.Add(IOSize[ioSizeIndex].Trim());
                            selectIoSizeBox.SelectedIndex = ioSizeIndex;

                            selectQueueDepthBox.Items.Add(IOSize[ioSizeIndex].Trim());
                            selectQueueDepthBox.SelectedIndex = ioSizeIndex;

                            validParam = validIOSize[ioSizeIndex].ToString();
                            validMultiSimParam = validIOSize[ioSizeIndex].ToString();

                            validQDepth.Add(int.Parse(hostQDepth.Trim()));

                            startIOSizeMultiSim(enMultiSim, IOSize, ioSizeIndex);
                        }
                        else
                        {
                            setSimButtonProgressBar(IOSize.Count());
                        }
                    }
                    setSimButtonProgressBar(IOSize.Count());
                }

                selectIoSizeBox.Visible = true;

                enableLoadGraphButton();
                enableGUIParamSelect();
                if (enableWrkld == "1")
                {
                    initParams();
                }
            }
        }

        private void startIOSizeMultiSim(string enMultiSim, List<string> IOSize, int ioSizeIndex)
        {
            string var = "";
            coreMode = " 0";
            hostQDepth = " " + hostQDepth.Trim();
            //if (enableWrkld == "1")
            //{
            //    string enWrkld = " " + enableWrkld.Trim();
            //    var = numCommands + IOSize[ioSizeIndex] + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
            //    + numPages + finalQDepth + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
            //    + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile;

            //}
            //else
            //{
            //    string enWrkld = " " + enableWrkld.Trim();
            //    // wrkloadFile = " wrkload.txt";
            //    var = numCommands + IOSize[ioSizeIndex] + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
            //        + numPages + finalQDepth + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
            //        + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
            //}

            if (enableWrkld == "1")
            {
                string enWrkld = " " + enableWrkld.Trim();
                var = numCommands + cntrlQueueDepth + queueFactor + IOSize[ioSizeIndex] + emCacheSize + numChannel + pageSize + readTime + programTime + credit + onfiClk
                        + numBanks + numDie + numPages + enMultiSim + coreMode + cmdTransferTime + IOSize[ioSizeIndex] + pcieSpeed + sqSize + cqSize + cmdType + pollWaitTime + hostQDepth + enWrkld + wrkloadFile
                + enableSeqLBA + cmdPct + wrBuffSize + rdBuffSize + removeFiles + enableLogs;
            }
            else
            {
                string enWrkld = " " + enableWrkld.Trim();
                 wrkloadFile = " wrkload.txt";
                 var = numCommands + cntrlQueueDepth + queueFactor + IOSize[ioSizeIndex] + emCacheSize + numChannel + pageSize + readTime + programTime + credit + onfiClk
                         + numBanks + numDie + numPages + enMultiSim + coreMode + cmdTransferTime + IOSize[ioSizeIndex] + pcieSpeed + sqSize + cqSize + cmdType + pollWaitTime + hostQDepth + enWrkld + wrkloadFile
                 + enableSeqLBA + cmdPct + wrBuffSize + rdBuffSize + removeFiles + enableLogs;
            }
            Process process = new Process();

            try
            {
                //process.StartInfo.FileName = @".\TeraSPCIeController.exe";
                //process.StartInfo.FileName = @".\TeraSPCIeController.exe";
                process.StartInfo.FileName = @".\TeraSPCIeController.exe";
                process.StartInfo.Arguments = var; // Put your arguments here
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                progressLabel.Text = "Running...";
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
        private void setSimButtonProgressBar(int param)
        {
            progressLabel.Text = "Complete...";
            progressBar1.Value = param;
        }

        private void startSingleSim(string enableMultiSim)
        {
            string var = "";
            string removeFiles = " 1";
            //if (enableWrkld == "1")
            //{
            //    string enWrkld = " " + enableWrkld.Trim();
            //    //var = numCommands + wrkloadBS + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
            //    //+ numPages + depthOfQueue + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
            //    //+ enableMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
            //    var= numCommands + cntrlQueueDepth + queueFactor + wrkloadBS + emCacheSize + numChannel + pageSize + readTime + programTime + credit + onfiClk
            //          + numBanks + numDie + numPages + enableMultiSim + coreMode + cmdTransferTime + ioSize + pcieSpeed + sqSize + cqSize + cmdType + pollWaitTime + hostQDepth + enWrkld + wrkloadFile 
            //+ enableSeqLBA + cmdPct + wrBuffSize + rdBuffSize + removeFiles + enableLogs;
            //}
            //else
            //{
                string enWrkld = " " + enableWrkld.Trim();
                hostQDepth = " " + hostQDepth.Trim();
                coreMode = " 0";
                if (enableWrkld == "0")
                {
                    wrkloadFile = " wrkload.txt";
                }
                var = numCommands + cntrlQueueDepth + queueFactor + ioSize + emCacheSize + numChannel + pageSize + readTime + programTime + credit + onfiClk
                      + numBanks + numDie + numPages + enableMultiSim + coreMode + cmdTransferTime + ioSize + pcieSpeed + sqSize + cqSize + cmdType + pollWaitTime + hostQDepth + enWrkld + wrkloadFile 
            + enableSeqLBA + cmdPct + wrBuffSize + rdBuffSize + removeFiles + enableLogs;
               
            //}
            Process process = new Process();
            try
            {
                //process.StartInfo.FileName = @".\TeraSPCIeController.exe";
                //process.StartInfo.FileName = @".\TeraSPCIeController.exe";
                process.StartInfo.FileName = @".\TeraSPCIeController.exe";
                process.StartInfo.Arguments = var; // Put your arguments here
                process.Start();
            }
            finally
            {
                if (process != null)
                {
                    progressLabel.Text = "Running...";
                    process.WaitForExit();
                    process.Close();


                }
            }
        }
        
        private void disableLoadGraphButton()
        {
            button1.Enabled = false;
            //panel1.Enabled = false;
            label28.Visible = false;
        }
        
        private void enableLoadGraphButton()
        {
            button1.Enabled = true;
        }
        #endregion

        private string setPcieSpeed()
        {
            string speed = " 3939";
            if (PCIeGenBox.Text == "Gen3" && PCIeLaneBox.Text == "x4")
            {
                speed = "3939";
                speed = " " + speed.Trim();
                return speed;
            }
            else
                return speed;
        }
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
                    if (FlashDemo)
                    {
                        chartType = MULTISIM_QD;

                    }
                    else
                    {
                        chartType = simulationTypeBox.Text;
                    }

                    if (chartType == "Multi Sim based on Block Size")
                    {
                        int qIndex = 0;
                        foreach (string blk in validBlockSize)
                        {
                            if (enableWrkld == "1")
                            {
                                fileName = ".\\Reports_PCIe\\latency_TB_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blk + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                            }
                            else
                            {
                                fileName = ".\\Reports_PCIe\\latency_TB_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blk + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
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
                                fileName = ".\\Reports_PCIe\\latency_TB_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + qd + ".log";
                            }
                            else
                            {
                                fileName = ".\\Reports_PCIe\\latency_TB_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + qd + ".log";
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
                            fileName = ".\\Reports_PCIe\\latency_TB_report_" + "iosize_" + io + "_" + "blksize_" + io + "_" + "qd_" + hostQDepth.Trim() + ".log";

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
                    using (FileStream file = new FileStream(@".\Reports_PCIe\latency_TB_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                        chartType = simulationTypeBox.Text;
                    }

                    if (chartType == "Multi Sim based on Block Size")
                    {
                        int qIndex = 0;
                        foreach (string io in validBlockSize)
                        {
                            if (enableWrkld == "1")
                            {
                                fileName = ".\\Reports_PCIe\\IOPS_utilization_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + io + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                            }
                            else
                            {
                                fileName = ".\\Reports_PCIe\\IOPS_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + io + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
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
                                fileName = ".\\Reports_PCIe\\IOPS_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + qd + ".log";
                            }
                            else
                            {
                                fileName = ".\\Reports_PCIe\\IOPS_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + qd + ".log";
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
                            fileName = ".\\Reports_PCIe\\IOPS_utilization_" + "iosize_" + io + "_" + "blksize_" + io + "_" + "qd_" + hostQDepth.Trim() + ".log";
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
                    using (FileStream file = new FileStream(@".\Reports_PCIe\IOPS_utilization.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                using (FileStream file = new FileStream(@".\Reports_PCIe\channel_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                            for (int bankIndex = 0; bankIndex < bankCount; bankIndex += bankSkipGap)
                            {
                                tempVal += transCount[bankIndex + chanIndex * (int.Parse(numBanks) * int.Parse(numDie))];

                            }
                            chanTransCount.Add(tempVal);
                        }

                        for (int chanIndex = 0; chanIndex < int.Parse(numChannel.Trim()); chanIndex++)
                        {
                            for (int bankIndex = 0; bankIndex < bankCount; bankIndex += bankSkipGap)
                            {
                                transactionCount.Add(transCount[chanIndex * bankCount + bankIndex]);
                            }

                        }
                        bankUtilChart.Update();
                        if (bankUtilChart.Series.Count != 0)
                            bankUtilChart.Series[0].Points.Clear();
                        bankUtilChart.ChartAreas[0].AxisX.Title = "Logical Banks";

                        bankUtilChart.ChartAreas[0].AxisY.Title = "No. Of References (CodeWord)";
                        bankUtilChart.ChartAreas[0].AxisY.Minimum = 0;
                        bankUtilChart.ChartAreas[0].AxisY.Maximum = transCount.Max();
                        //bankUtilChart.ChartAreas[0].AxisX.Maximum =  bankCount;// / bankSkipGap + 1;

                        if (bankCount > 127 && bankCount <= 256)
                            bankUtilChart.Width = 4096;
                        else if(bankCount > 63 && bankCount <= 127)
                            bankUtilChart.Width = 2048;
                        else
                            bankUtilChart.Width = 1082;
                        if (transCount.Max() > 100)
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
                            bankUtilChart.Series[chanIndex.ToString()].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 15";
                        }

                        int bankLabel = 0;
                        int logicBankIndex = 1;
                        for (int cwBankIndex = 0; cwBankIndex < bankCount / bankSkipGap; cwBankIndex++)
                        {
                           
                            bankUtilChart.ChartAreas[0].AxisX.CustomLabels.Add(logicBankIndex -1.5 , logicBankIndex + 1.5, bankLabel.ToString());
                            for (int chanIndex = 0; chanIndex < int.Parse(numChannel.Trim()); chanIndex++)
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
            iopsVsIosizeChart.ChartAreas[0].AxisX.Title = "Block Size";
            iopsVsIosizeChart.Series.Clear();
            //iopsVsIosizeChart.Titles.Clear();
            //iopsVsIosizeChart.Titles.Add("IOPS Vs Block Size");
            iopsVsIosizeChart.Titles[0].Text = "IOPS Vs Block Size";
            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\IopsVsBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("BlockSize" + "," + "IOPS(Millions)");
                        if (multiSim)
                        {

                            if (simulationTypeBox.Text == "Multi Sim based on Block Size")
                            {
                                int iopsIndex = 0;

                                iopsVsIosizeChart.Series.Add("0");

                                iopsVsIosizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                iopsVsIosizeChart.Series[0].Color = Color.Blue;
                                iopsVsIosizeChart.Series[0].BorderWidth = 3;
                                iopsVsIosizeChart.Series[0].Name = "IOPS";
                                iopsVsIosizeChart.Annotations.Clear();
                                iopsVsIosizeChart.Series[0].LegendText = string.Concat("QD " + validQDepth[validQDepth.Count() - 1].ToString());

                                for (int ioIndex = 0; ioIndex < validBlockSize.Count(); ioIndex++)
                                {
                                    iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(validBlockSize[ioIndex]), avgIOPS[iopsIndex]);
                                    writer.WriteLine(validBlockSize[ioIndex] + "," + avgIOPS[iopsIndex]);
                                    iopsVsIosizeChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    iopsVsIosizeChart.Series[0].MarkerSize = 10;
                                    iopsVsIosizeChart.Series[0].MarkerColor = Color.Green;
                                    iopsIndex++;
                                }

                            }
                        }
                        else
                        {
                            iopsVsIosizeChart.Series.Add("0");
                            if (enableWrkld == "1")
                            {
                                iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(blkSize), iopsAverage);
                                writer.WriteLine(blkSize + "," + iopsAverage);
                            }
                            else
                            {
                                iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(blkSize), iopsAverage);
                                writer.WriteLine(blkSize + "," + iopsAverage);
                            }
                            iopsVsIosizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            iopsVsIosizeChart.Series[0].Name = "IOPS";
                            iopsVsIosizeChart.Annotations.Clear();
                            iopsVsIosizeChart.Series[0].LegendText = string.Concat("QD " + depthOfQueue);

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
            iopsVsIosizeChart.ChartAreas[0].AxisX.Title = "IO Size";
           
            iopsVsIosizeChart.Series.Clear();
            
            //iopsVsIosizeChart.Titles.Clear();
            //iopsVsIosizeChart.Titles.Add("IOPS Vs IO Size");
            iopsVsIosizeChart.Titles[0].Text = "IOPS Vs IO Size";

            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\IopsVsIOSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IO Size" + "," + "IOPS(Millions)");
                        if (multiSim)
                        {

                            if (simulationTypeBox.Text == "Multi Sim based on IO Size")
                            {
                                int iopsIndex = 0;

                                iopsVsIosizeChart.Series.Add("0");

                                iopsVsIosizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                iopsVsIosizeChart.Series[0].Color = Color.Blue;
                                iopsVsIosizeChart.Series[0].BorderWidth = 3;
                                iopsVsIosizeChart.Series[0].Name = "IOPS";
                               
                                iopsVsIosizeChart.Annotations.Clear();
                                iopsVsIosizeChart.Series[0].LegendText = string.Concat("QD " + validQDepth[validQDepth.Count() - 1].ToString());

                                for (int ioIndex = 0; ioIndex < validIOSize.Count(); ioIndex++)
                                {
                                    iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(validIOSize[ioIndex]), avgIOPS[ioIndex]);
                                    writer.WriteLine(validIOSize[ioIndex] + "," + avgIOPS[ioIndex]);
                                    iopsVsIosizeChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    iopsVsIosizeChart.Series[0].MarkerSize = 10;
                                    iopsVsIosizeChart.Series[0].MarkerColor = Color.Green;
                                    iopsIndex++;
                                }

                            }
                        }
                            /*Single Sim */
                        else
                        {
                            iopsVsIosizeChart.Series.Add("0");
                            if (enableWrkld == "1")
                            {
                                //iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(wrkloadBS), iopsAverage);
                                //writer.WriteLine(wrkloadBS + "," + iopsAverage);
                                iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(ioSize), iopsAverage);
                                writer.WriteLine(ioSize + "," + iopsAverage);
                            }
                            else
                            {
                                iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(ioSize), iopsAverage);
                                writer.WriteLine(ioSize + "," + iopsAverage);
                            }
                            iopsVsIosizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            iopsVsIosizeChart.Series[0].Name = "IOPS";
                            iopsVsIosizeChart.Annotations.Clear();
                            iopsVsIosizeChart.Series[0].LegendText = string.Concat("QD " + hostQDepth.Trim());

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
        /** LatencyVsIOSize
        * Plots average latency versus IOSize graph
        * @param bool multiSim
        * @return void
        **/
        private void LatencyVsBlockSize(bool multiSim)
        {
            cmdCntVslbaChart.ChartAreas[0].AxisX.Title = "Block Size";
            cmdCntVslbaChart.Series.Clear();
            //chart8.Titles.Clear();
            //chart8.Titles.Add("Latency Vs Block Size");
            cmdCntVslbaChart.Titles[0].Text = "Latency Vs Block Size";
            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("BlockSize" + "," + "Latency(us)");
                        if (multiSim)
                        {
                            if (simulationTypeBox.Text == "Multi Sim based on Block Size")
                            {
                                cmdCntVslbaChart.Annotations.Clear();

                                int avgLatencyIndex = 0;
                                cmdCntVslbaChart.Series.Add("0");

                                cmdCntVslbaChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                cmdCntVslbaChart.Series[0].BorderWidth = 3;
                                cmdCntVslbaChart.Series[0].Color = Color.Blue;
                                cmdCntVslbaChart.Series[0].Name = "Latency";
                                string legends = string.Concat("QD " + validQDepth[validQDepth.Count() - 1].ToString());
                                cmdCntVslbaChart.Series[0].LegendText = legends;

                                for (int ioIndex = 0; ioIndex < validBlockSize.Count(); ioIndex++)
                                {

                                    cmdCntVslbaChart.Series[0].Points.AddXY(int.Parse(validBlockSize[ioIndex]), avgLatency[avgLatencyIndex]);
                                    writer.WriteLine(validBlockSize[ioIndex] + "," + avgLatency[avgLatencyIndex]);
                                    cmdCntVslbaChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    cmdCntVslbaChart.Series[0].MarkerSize = 10;
                                    cmdCntVslbaChart.Series[0].MarkerColor = Color.Green;

                                    avgLatencyIndex++;
                                }

                            }
                        }
                        else
                        {
                            cmdCntVslbaChart.Series.Add("0");
                            if (enableWrkld == "1")
                            {
                                cmdCntVslbaChart.Series[0].Points.AddXY(int.Parse(blkSize), latencyAverage);
                                writer.WriteLine(blkSize + "," + latencyAverage);
                            }
                            else
                            {
                                cmdCntVslbaChart.Series[0].Points.AddXY(int.Parse(blkSize), latencyAverage);
                                writer.WriteLine(blkSize + "," + latencyAverage);
                            }
                            cmdCntVslbaChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            cmdCntVslbaChart.Series[0].Name = "Latency";
                            cmdCntVslbaChart.Series[0].LegendText = string.Concat("QD " + depthOfQueue);
                            cmdCntVslbaChart.Annotations.Clear();

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
            latencyVsIosizeChart.ChartAreas[0].AxisX.Title = "IO Size";
            latencyVsIosizeChart.Series.Clear();
            //chart8.Titles.Clear();
            //chart8.Titles.Add("Latency Vs IO Size");
            latencyVsIosizeChart.Titles[0].Text = "Latency Vs IO Size";
            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsSectorSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("SectorSize" + "," + "Latency(us)");
                        if (multiSim)
                        {
                            if (simulationTypeBox.Text == "Multi Sim based on IO Size")
                            {
                                latencyVsIosizeChart.Annotations.Clear();

                                int avgLatencyIndex = 0;
                                latencyVsIosizeChart.Series.Add("0");
                               
                                latencyVsIosizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                latencyVsIosizeChart.Series[0].BorderWidth = 3;
                                latencyVsIosizeChart.Series[0].Color = Color.Blue;
                                latencyVsIosizeChart.Series[0].Name = "Latency";
                                string legends = string.Concat("QD " + validQDepth[validQDepth.Count() - 1].ToString());
                                latencyVsIosizeChart.Series[0].LegendText = legends;

                                for (int ioIndex = 0; ioIndex < validIOSize.Count(); ioIndex++)
                                {

                                    latencyVsIosizeChart.Series[0].Points.AddXY(int.Parse(validIOSize[ioIndex]), avgLatency[avgLatencyIndex]);
                                    writer.WriteLine(validIOSize[ioIndex] + "," + avgLatency[avgLatencyIndex]);
                                    latencyVsIosizeChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    latencyVsIosizeChart.Series[0].MarkerSize = 10;
                                    latencyVsIosizeChart.Series[0].MarkerColor = Color.Green;

                                    avgLatencyIndex++;
                                }

                            }
                        }
                        else
                        {
                            latencyVsIosizeChart.Series.Add("0");
                            
                            if (enableWrkld == "1")
                            {
                                //latencyVsIosizeChart.Series[0].Points.AddXY(int.Parse(wrkloadBS), latencyAverage);
                                //writer.WriteLine(wrkloadBS + "," + latencyAverage);
                                latencyVsIosizeChart.Series[0].Points.AddXY(int.Parse(ioSize), latencyAverage);
                                writer.WriteLine(ioSize + "," + latencyAverage);
                            }
                            else
                            {
                                latencyVsIosizeChart.Series[0].Points.AddXY(int.Parse(ioSize), latencyAverage);
                                writer.WriteLine(ioSize + "," + latencyAverage);
                            }
                            latencyVsIosizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            latencyVsIosizeChart.Series[0].Name = "Latency";
                            latencyVsIosizeChart.Series[0].LegendText = string.Concat("QD " + hostQDepth.Trim());
                            latencyVsIosizeChart.Annotations.Clear();

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

        private void IOPSVsQD(bool multiSim)
        {

            //chart9.ChartAreas.Clear();
            iopsVsQdChart.Series.Clear();
            iopsVsQdChart.Series.Add("0");
            iopsVsQdChart.Series[0].IsVisibleInLegend = false;
            //chart9.ChartAreas.Add("0");
            iopsVsQdChart.ChartAreas[0].AxisX.IntervalAutoMode = 0;
            iopsVsQdChart.ChartAreas[0].AxisX.Minimum = 1;
            iopsVsQdChart.ChartAreas[0].AxisX.Maximum = 512;
            iopsVsQdChart.ChartAreas[0].AxisX.Title = "QD";
            iopsVsQdChart.ChartAreas[0].AxisY.Title = "IOPS (millions)";

            int startOffset = 0;
            int endOffset = 60;

            for (double x = iopsVsQdChart.ChartAreas[0].AxisX.Minimum; x < iopsVsQdChart.ChartAreas[0].AxisX.Maximum; x *= 2)
            {
                CustomLabel qdLabel = new CustomLabel(startOffset, endOffset, x.ToString(), 0, LabelMarkStyle.None);
                iopsVsQdChart.ChartAreas[0].AxisX.CustomLabels.Add(qdLabel);
                startOffset = endOffset;
                endOffset += 60;
            }

            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\IOPSVsQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("QD" + "," + "IOPS(Millions)");
                        if (multiSim)
                        {
                            string chartType;
                            if (FlashDemo)
                            {
                                chartType = MULTISIM_QD;

                            }
                            else
                            {
                                chartType = simulationTypeBox.Text;
                            }
                            //if (radioButton3.Checked)
                            if (chartType == "Multi Sim based on QD")
                            {
                                for (int queueDIndex = 0; queueDIndex < validQueueDepth.Count(); queueDIndex++)
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

                                    int offset = (startOffset + endOffset) / 2;
                                    iopsVsQdChart.Series[0].Color = Color.Blue;

                                    //if (int.Parse(queueDepth) > numSlot)
                                    //{
                                    //    iopsVsQdChart.Series[0].Points.AddXY(offset, avgIOPS[queueDIndex]);
                                    //    writer.WriteLine(validQueueDepth[queueDIndex] + "," + avgIOPS[queueDIndex]);
                                    //}
                                    //else
                                    //{
                                        iopsVsQdChart.Series[0].Points.AddXY(offset, avgIOPS[queueDIndex]);
                                        writer.WriteLine(validQueueDepth[queueDIndex] + "," + avgIOPS[queueDIndex]);
                                  //  }
                                    iopsVsQdChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                    iopsVsQdChart.Series[0].BorderWidth = 3;
                                    iopsVsQdChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    iopsVsQdChart.Series[0].MarkerSize = 10;
                                    iopsVsQdChart.Series[0].MarkerColor = Color.Green;
                                }
                                iopsVsQdChart.Annotations.Clear();
                                TextAnnotation iosize = new TextAnnotation();
                                iosize.Name = "iosize";
                                if (enableWrkld == "1")
                                {
                                    iosize.Text = string.Concat("IOSize " + validIOSize[0].Trim());
                                }
                                else
                                {
                                    iosize.Text = string.Concat("IOSize " + ioSize);
                                }
                                iosize.ForeColor = Color.Black;
                                iosize.Font = new Font("Arial", 10, FontStyle.Bold); ;
                                iosize.LineWidth = 1;
                                iopsVsQdChart.Annotations.Add(iosize);
                                iopsVsQdChart.Annotations[0].AxisX = iopsVsQdChart.ChartAreas[0].AxisX;
                                iopsVsQdChart.Annotations[0].AxisY = iopsVsQdChart.ChartAreas[0].AxisY;
                                iopsVsQdChart.Annotations[0].AnchorDataPoint = iopsVsQdChart.Series[0].Points[0];

                            }

                        }
                        else
                        {
                            switch (int.Parse(hostQDepth.Trim()))
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

                            int offset = (startOffset + endOffset) / 2;

                            iopsVsQdChart.Series[0].Points.AddXY(offset, iopsAverage);
                            writer.WriteLine(sqSize + "," + iopsAverage);
                            iopsVsQdChart.Series[0].Color = Color.Blue;

                            //if (int.Parse(queueDepth) > numSlot)
                            //{
                            //    chart5.Series[0].Points.AddXY(offset, iopsAverage);
                            //    writer.WriteLine(depthOfQueue + "," + iopsAverage);
                            //}
                            //else
                            //{
                            //    chart5.Series[0].Points.AddXY(offset, iopsAverage);
                            //    writer.WriteLine(depthOfQueue + "," + iopsAverage);
                            //}
                            iopsVsQdChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            iopsVsQdChart.Annotations.Clear();
                            TextAnnotation iosize = new TextAnnotation();
                            iosize.Name = "iosize";
                            iosize.Text = string.Concat("IOSize " + ioSize);
                            iosize.ForeColor = Color.Black;
                            iosize.Font = new Font("Arial", 10, FontStyle.Bold); ;
                            iosize.LineWidth = 1;
                            iopsVsQdChart.Annotations.Add(iosize);
                            iopsVsQdChart.Annotations[0].AxisX = iopsVsQdChart.ChartAreas[0].AxisX;
                            iopsVsQdChart.Annotations[0].AxisY = iopsVsQdChart.ChartAreas[0].AxisY;
                            iopsVsQdChart.Annotations[0].AnchorDataPoint = iopsVsQdChart.Series[0].Points[0];

                        }

                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: LatencyVsIOsize", e.Message), e);
            }
        }

        private void LatencyVsQD(bool multiSim)
        {

            //chart10.ChartAreas.Clear();
            latencyVsQDChart.Series.Clear();
            latencyVsQDChart.Series.Add("0");
            latencyVsQDChart.Series[0].IsVisibleInLegend = false;
            //chart10.ChartAreas.Add("0");
            latencyVsQDChart.ChartAreas[0].AxisX.IntervalAutoMode = 0;
            latencyVsQDChart.ChartAreas[0].AxisX.Minimum = 1;
            latencyVsQDChart.ChartAreas[0].AxisX.Maximum = 512;
            latencyVsQDChart.ChartAreas[0].AxisX.Title = "QD";
            latencyVsQDChart.ChartAreas[0].AxisY.Title = "Latency (microseconds)";

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
                using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("QD" + "," + "Latency(us)");
                        if (multiSim)
                        {
                            string chartType;
                            if (FlashDemo)
                            {
                                chartType = MULTISIM_QD;

                            }
                            else
                            {
                                chartType = simulationTypeBox.Text;
                            }
                            if (chartType == "Multi Sim based on QD")
                            {

                                for (int queueIndex = 0; queueIndex < validQueueDepth.Count(); queueIndex++)
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
                                
                                latencyVsQDChart.Annotations.Clear();
                                TextAnnotation myLine = new TextAnnotation();
                                myLine.Name = "myLine";
                                if (enableWrkld == "1")
                                {
                                    myLine.Text = string.Concat("IOSize " + validIOSize[0].Trim());
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
                        }
                        else
                        {
                            switch (int.Parse(hostQDepth.Trim()))
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

                            int offset = (startOffset + endOffset) / 2;

                            latencyVsQDChart.Series[0].Color = Color.Blue;
                            latencyVsQDChart.Series[0].Points.AddXY(offset, latencyAverage);
                            writer.WriteLine(int.Parse(sqSize) + "," + latencyAverage);
                            latencyVsQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            latencyVsQDChart.Annotations.Clear();
                            TextAnnotation myLine = new TextAnnotation();
                            myLine.Name = "myLine";
                            myLine.Text = string.Concat("IOSize " + ioSize);
                            myLine.ForeColor = Color.Black;
                            myLine.Font = new Font("Arial", 10, FontStyle.Bold);
                            myLine.LineWidth = 1;
                            latencyVsQDChart.Annotations.Add(myLine);
                            latencyVsQDChart.Annotations[0].AxisX = latencyVsQDChart.ChartAreas[0].AxisX;
                            latencyVsQDChart.Annotations[0].AxisY = latencyVsQDChart.ChartAreas[0].AxisY;
                            latencyVsQDChart.Annotations[0].AnchorDataPoint = latencyVsQDChart.Series[0].Points[0];
                        }
                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: LatencyVsIOsize", e.Message), e);
            }

        }

        private void LatencyVsIOPSwithQD(bool multiSim)
        {
            latencyVsIopsVsQDChart.Series.Clear();

            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsIOPSwithQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOPS(Millions)" + "," + "Latency(us)" + "," + "QD");
                        if (multiSim)
                        {
                            string chartType;
                            if (FlashDemo)
                            {
                                chartType = MULTISIM_QD;

                            }
                            else
                            {
                                chartType = simulationTypeBox.Text;
                            }
                            if (chartType == "Multi Sim based on QD")
                            {
                                int iopsIndex = 0;
                                TextAnnotation[] qd = { new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
                                             new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
                                          new TextAnnotation(), new TextAnnotation(), new TextAnnotation()
                                          };


                                latencyVsIopsVsQDChart.Series.Add("0");
                                latencyVsIopsVsQDChart.ChartAreas[0].AxisX.Interval = 0.5;
                                latencyVsIopsVsQDChart.ChartAreas[0].AxisX.IntervalOffset = 0.5;
                                latencyVsIopsVsQDChart.ChartAreas[0].AxisX.Minimum = 0;
                                latencyVsIopsVsQDChart.ChartAreas[0].AxisX.Title = "IOPS (millions)";
                                latencyVsIopsVsQDChart.ChartAreas[0].AxisY.Title = "Latency (microseconds)";
                                latencyVsIopsVsQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                latencyVsIopsVsQDChart.Series[0].Color = Color.Blue;
                                latencyVsIopsVsQDChart.Series[0].BorderWidth = 3;
                                latencyVsIopsVsQDChart.Series[0].IsVisibleInLegend = false;
                              
                                latencyVsIopsVsQDChart.Annotations.Clear();
                                latencyVsIopsVsQDChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(avgIOPS.Max());

                                for (int qIndex = 0; qIndex < validQueueDepth.Count(); qIndex++)
                                {
                                    latencyVsIopsVsQDChart.Series[0].Points.AddXY(avgIOPS[iopsIndex], avgLatency[qIndex]);
                                    writer.WriteLine(avgIOPS[iopsIndex] + "," + avgLatency[qIndex] + "," + validQueueDepth[iopsIndex]);
                                    latencyVsIopsVsQDChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    latencyVsIopsVsQDChart.Series[0].MarkerSize = 10;
                                    latencyVsIopsVsQDChart.Series[0].MarkerColor = Color.Green;
                                    qd[qIndex].Name = "qd_iops_" + qIndex.ToString() + validQueueDepth[iopsIndex].ToString();
                                    qd[qIndex].Text = string.Concat("QD " + validQueueDepth[iopsIndex].ToString());
                                    qd[qIndex].ForeColor = Color.Black;
                                    qd[qIndex].Font = new Font("Arial", 10, FontStyle.Bold);
                                    qd[qIndex].LineWidth = 1;
                                    latencyVsIopsVsQDChart.Annotations.Add(qd[qIndex]);
                                    latencyVsIopsVsQDChart.Annotations[qIndex].AxisX = latencyVsIopsVsQDChart.ChartAreas[0].AxisX;
                                    latencyVsIopsVsQDChart.Annotations[qIndex].AxisY = latencyVsIopsVsQDChart.ChartAreas[0].AxisY;
                                    latencyVsIopsVsQDChart.Annotations[qIndex].AnchorDataPoint = latencyVsIopsVsQDChart.Series[0].Points[qIndex];
                                    iopsIndex++;
                                }
                            }
                        }
                        else //single sim
                        {
                            latencyVsIopsVsQDChart.Series.Add("0");
                            latencyVsIopsVsQDChart.ChartAreas[0].AxisX.Interval = 0.5;
                            latencyVsIopsVsQDChart.ChartAreas[0].AxisX.IntervalOffset = 0.5;
                            latencyVsIopsVsQDChart.ChartAreas[0].AxisX.Minimum = 0;
                            //chart6.ChartAreas[0].AxisX.IntervalAutoMode = 
                            //chart6.ChartAreas[0].AxisX.Maximum = Math.Ceiling(iopsAverage) + 1;
                            latencyVsIopsVsQDChart.Series[0].Points.AddXY(Math.Round(iopsAverage, 2), latencyAverage);
                            writer.WriteLine(Math.Round(iopsAverage, 2) + "," + latencyAverage + "," + hostQDepth.Trim());
                            latencyVsIopsVsQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            latencyVsIopsVsQDChart.Series[0].IsVisibleInLegend = false;
                            
                            latencyVsIopsVsQDChart.Annotations.Clear();
                            TextAnnotation qd = new TextAnnotation();
                            qd.Name = "qd_iops";
                            qd.Text = string.Concat("QD " + hostQDepth.Trim());
                            qd.ForeColor = Color.Black;
                            qd.Font = new Font("Arial", 10, FontStyle.Bold);
                            qd.LineWidth = 1;
                            latencyVsIopsVsQDChart.Annotations.Add(qd);
                            latencyVsIopsVsQDChart.Annotations[0].AxisX = latencyVsIopsVsQDChart.ChartAreas[0].AxisX;
                            latencyVsIopsVsQDChart.Annotations[0].AxisY = latencyVsIopsVsQDChart.ChartAreas[0].AxisY;
                            latencyVsIopsVsQDChart.Annotations[0].AnchorDataPoint = latencyVsIopsVsQDChart.Series[0].Points[0];

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

        private void LatencyVsIOPSwithBlockSize(bool multiSim)
        {

            latencyVsQDChart.Series.Clear();
            //chart4.Titles.Clear();
            //chart4.Titles.Add("Latency Vs IOPS(with varying Block Size)");
            latencyVsQDChart.Titles[0].Text = "Latency Vs IOPS ( with varying Block Size )";
            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsIOPSwithBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOPS(Millions)" + "," + "Latency(us)" + "," + "BlockSize");
                        if (multiSim)
                        {
                            if (simulationTypeBox.Text == "Multi Sim based on Block Size")
                            {
                                int iopsIndex = 0;
                                TextAnnotation[] qd = { new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
                                             new TextAnnotation(), new TextAnnotation()};

                                latencyVsQDChart.Series.Add("0");
                                latencyVsQDChart.ChartAreas[0].AxisX.Interval = 0.5;
                                latencyVsQDChart.ChartAreas[0].AxisX.IntervalOffset = 0.5;
                                latencyVsQDChart.ChartAreas[0].AxisX.Minimum = 0;
                                latencyVsQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                latencyVsQDChart.Series[0].Color = Color.Blue;
                                latencyVsQDChart.Series[0].BorderWidth = 3;
                                latencyVsQDChart.Series[0].Name = "IOPS";
                                latencyVsQDChart.Annotations.Clear();


                                for (int qIndex = 0; qIndex < validBlockSize.Count(); qIndex++)
                                {
                                    latencyVsQDChart.Series[0].Points.AddXY(Math.Round(avgIOPS[iopsIndex], 2), avgLatency[qIndex]);
                                    writer.WriteLine(Math.Round(avgIOPS[iopsIndex], 2) + "," + avgLatency[qIndex] + "," + validBlockSize[iopsIndex]);
                                    latencyVsQDChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    latencyVsQDChart.Series[0].MarkerSize = 10;
                                    latencyVsQDChart.Series[0].MarkerColor = Color.Green;
                                    qd[qIndex].Name = "qd_iops_" + qIndex.ToString() + validBlockSize[iopsIndex].ToString();
                                    qd[qIndex].Text = string.Concat("BlockSize " + validBlockSize[iopsIndex].ToString());
                                    qd[qIndex].ForeColor = Color.Black;
                                    qd[qIndex].Font = new Font("Arial", 10, FontStyle.Bold);
                                    qd[qIndex].LineWidth = 1;
                                    latencyVsQDChart.Annotations.Add(qd[qIndex]);
                                    latencyVsQDChart.Annotations[qIndex].AxisX = latencyVsQDChart.ChartAreas[0].AxisX;
                                    latencyVsQDChart.Annotations[qIndex].AxisY = latencyVsQDChart.ChartAreas[0].AxisY;
                                    latencyVsQDChart.Annotations[qIndex].AnchorDataPoint = latencyVsQDChart.Series[0].Points[qIndex];
                                    iopsIndex++;
                                }

                            }
                        }
                        else
                        {
                            latencyVsQDChart.Series.Add("0");
                            latencyVsQDChart.ChartAreas[0].AxisX.Interval = 0.5;
                            latencyVsQDChart.ChartAreas[0].AxisX.IntervalOffset = 0.5;
                            latencyVsQDChart.ChartAreas[0].AxisX.Minimum = 0;
                            latencyVsQDChart.Series[0].Points.AddXY(iopsAverage, latencyAverage);
                            if (enableWrkld == "1")
                            {
                                writer.WriteLine(iopsAverage + "," + latencyAverage + "," + wrkloadBS);
                            }
                            else
                            {
                                writer.WriteLine(iopsAverage + "," + latencyAverage + "," + blkSize);
                            }
                            latencyVsQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            latencyVsQDChart.Series[0].Name = "IOPS";
                            latencyVsQDChart.Annotations.Clear();
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
                            latencyVsQDChart.Annotations.Add(qd);
                            latencyVsQDChart.Annotations[0].AxisX = latencyVsQDChart.ChartAreas[0].AxisX;
                            latencyVsQDChart.Annotations[0].AxisY = latencyVsQDChart.ChartAreas[0].AxisY;
                            latencyVsQDChart.Annotations[0].AnchorDataPoint = latencyVsQDChart.Series[0].Points[0];

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

            latencyVsIopsVsIoSizeChart.Series.Clear();
            //chart4.Titles.Clear();
            //chart4.Titles.Add("Latency Vs IOPS(with varying IO Size)");
            latencyVsIopsVsIoSizeChart.Titles[0].Text = "Latency Vs IOPS ( with varying IO Size )";
            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsIOPSwithIOSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOPS(Millions)" + "," + "Latency(us)" + "," + "IOSize");
                        if (multiSim)
                        {
                            if (simulationTypeBox.Text == "Multi Sim based on IO Size")
                            {
                                int iopsIndex = 0;
                                TextAnnotation[] qd = { new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
                                             new TextAnnotation(), new TextAnnotation()};

                                latencyVsIopsVsIoSizeChart.Series.Add("0");
                                latencyVsIopsVsIoSizeChart.ChartAreas[0].AxisX.Interval = 0.5;
                                latencyVsIopsVsIoSizeChart.ChartAreas[0].AxisX.IntervalOffset = 0.5;
                                latencyVsIopsVsIoSizeChart.ChartAreas[0].AxisX.Minimum = 0;
                                latencyVsIopsVsIoSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                latencyVsIopsVsIoSizeChart.Series[0].Color = Color.Blue;
                                latencyVsIopsVsIoSizeChart.Series[0].BorderWidth = 3;
                                latencyVsIopsVsIoSizeChart.Series[0].Name = "IOPS";
                                latencyVsIopsVsIoSizeChart.Annotations.Clear();


                                for (int qIndex = 0; qIndex < validIOSize.Count(); qIndex++)
                                {
                                    latencyVsIopsVsIoSizeChart.Series[0].Points.AddXY(Math.Round(avgIOPS[iopsIndex], 2), avgLatency[qIndex]);
                                    writer.WriteLine(Math.Round(avgIOPS[iopsIndex], 2) + "," + avgLatency[qIndex] + "," + validIOSize[iopsIndex]);
                                    latencyVsIopsVsIoSizeChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    latencyVsIopsVsIoSizeChart.Series[0].MarkerSize = 10;
                                    latencyVsIopsVsIoSizeChart.Series[0].MarkerColor = Color.Green;
                                    qd[qIndex].Name = "qd_iops_" + qIndex.ToString() + validIOSize[iopsIndex].ToString();
                                    qd[qIndex].Text = string.Concat("IOSize " + validIOSize[iopsIndex].ToString());
                                    qd[qIndex].ForeColor = Color.Black;
                                    qd[qIndex].Font = new Font("Arial", 10, FontStyle.Bold);
                                    qd[qIndex].LineWidth = 1;
                                    latencyVsIopsVsIoSizeChart.Annotations.Add(qd[qIndex]);
                                    latencyVsIopsVsIoSizeChart.Annotations[qIndex].AxisX = latencyVsIopsVsIoSizeChart.ChartAreas[0].AxisX;
                                    latencyVsIopsVsIoSizeChart.Annotations[qIndex].AxisY = latencyVsIopsVsIoSizeChart.ChartAreas[0].AxisY;
                                    latencyVsIopsVsIoSizeChart.Annotations[qIndex].AnchorDataPoint = latencyVsIopsVsIoSizeChart.Series[0].Points[qIndex];
                                    iopsIndex++;
                                }

                            }
                        }
                        else
                        {
                            latencyVsIopsVsIoSizeChart.Series.Add("0");
                            latencyVsIopsVsIoSizeChart.ChartAreas[0].AxisX.Interval = 0.5;
                            latencyVsIopsVsIoSizeChart.ChartAreas[0].AxisX.IntervalOffset = 0.5;
                            latencyVsIopsVsIoSizeChart.ChartAreas[0].AxisX.Minimum = 0;
                            latencyVsIopsVsIoSizeChart.Series[0].Points.AddXY(iopsAverage, latencyAverage);
                            if (enableWrkld == "1")
                            {
                                writer.WriteLine(iopsAverage + "," + latencyAverage + "," + wrkloadBS);
                            }
                            else
                            {
                                writer.WriteLine(iopsAverage + "," + latencyAverage + "," + ioSize);
                            }
                            latencyVsIopsVsIoSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                            latencyVsIopsVsIoSizeChart.Series[0].Name = "IOPS";
                            latencyVsIopsVsIoSizeChart.Annotations.Clear();
                            TextAnnotation qd = new TextAnnotation();
                            qd.Name = "qd_iops";
                            if (enableWrkld == "1")
                            {
                                qd.Text = string.Concat("IOSize " + validIOSize[0]);
                            }
                            else
                            {
                                qd.Text = string.Concat("IOSize " + ioSize);
                            }
                            qd.ForeColor = Color.Black;
                            qd.Font = new Font("Arial", 10, FontStyle.Bold);
                            qd.LineWidth = 1;
                            latencyVsIopsVsIoSizeChart.Annotations.Add(qd);
                            latencyVsIopsVsIoSizeChart.Annotations[0].AxisX = latencyVsIopsVsIoSizeChart.ChartAreas[0].AxisX;
                            latencyVsIopsVsIoSizeChart.Annotations[0].AxisY = latencyVsIopsVsIoSizeChart.ChartAreas[0].AxisY;
                            latencyVsIopsVsIoSizeChart.Annotations[0].AnchorDataPoint = latencyVsIopsVsIoSizeChart.Series[0].Points[0];

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

        private void CommandVsBlockSize(bool multiSim)
        {
            iopsVsIosizeChart.Series.Clear();
            iopsVsIosizeChart.Series.Add("Number Of Commands");
            iopsVsIosizeChart.Titles[0].Text = "Command Count Vs Block Size";

            iopsVsIosizeChart.ChartAreas[0].AxisX.Title = "Block Size";
            iopsVsIosizeChart.ChartAreas[0].AxisY.Title = "Number Of Commands";
            iopsVsIosizeChart.ChartAreas[0].AxisX.Minimum = 0;
            iopsVsIosizeChart.ChartAreas[0].AxisX.Maximum = 8704;
            iopsVsIosizeChart.ChartAreas[0].AxisX.Interval = 1024;
            iopsVsIosizeChart.ChartAreas[0].AxisY.Minimum = 0;
            iopsVsIosizeChart.ChartAreas[0].AxisY.Maximum = Int64.Parse(numCommands) + 1;

            iopsVsIosizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            iopsVsIosizeChart.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 20";
            iopsVsIosizeChart.Series[0].Color = Color.Blue;
            iopsVsIosizeChart.Series[0].Name = "Number Of Commands";
            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\CmdVsBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOSize" + "," + "Number of Commands");
                        if (multiSim)
                        {
                            if (simulationTypeBox.Text == "Multi Sim based on Block Size")
                            {
                                foreach (string io in validBlockSize)
                                {
                                    iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(io), Int64.Parse(numCommands));
                                    writer.WriteLine(int.Parse(io) + "," + Int64.Parse(numCommands));
                                }
                            }
                            else if (simulationTypeBox.Text == "Multi Sim based on QD")
                            {
                                if (enableWrkld == "1")
                                {
                                    iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(blkSize), Int64.Parse(numCommands));
                                    writer.WriteLine(int.Parse(wrkloadBS) + "," + Int64.Parse(numCommands));
                                }
                                else
                                {
                                    iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(blkSize), Int64.Parse(numCommands));
                                    writer.WriteLine(int.Parse(ioSize) + "," + Int64.Parse(numCommands));
                                }
                            }
                        }
                        else
                        {
                            //chart11.ChartAreas[0].AxisX.CustomLabels.Add(ioIndex - 1.0, ioIndex + 1.0);
                            if (enableWrkld == "1")
                            {
                                iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(blkSize), Int64.Parse(numCommands));
                                writer.WriteLine(int.Parse(wrkloadBS) + "," + Int64.Parse(numCommands));
                            }
                            else
                            {
                                iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(blkSize), Int64.Parse(numCommands));
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
                throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsBlocksize", e.Message), e);
            }
        }

        private void CommandVsIOSize(bool multiSim)
        {
            //chart11.Update();
            //chart11.ChartAreas.Clear();
            
            cmdCntVsIOSizeChart.Series.Clear();
            //chart11.Titles.Clear();
            cmdCntVsIOSizeChart.Series.Add("Number Of Commands");
            //chart11.Titles.Add("Command Count Vs IO Size");
            cmdCntVsIOSizeChart.Titles[0].Text = "Command Count Vs IO Size";
            //chart11.ChartAreas.Add("0");
            cmdCntVsIOSizeChart.ChartAreas[0].AxisX.Title = "IO Size";
            cmdCntVsIOSizeChart.ChartAreas[0].AxisY.Title = "Number Of Commands";
            cmdCntVsIOSizeChart.ChartAreas[0].AxisX.Minimum = 0;
            cmdCntVsIOSizeChart.ChartAreas[0].AxisX.Maximum = 8704;
            cmdCntVsIOSizeChart.ChartAreas[0].AxisX.Interval = 1024;
            cmdCntVsIOSizeChart.ChartAreas[0].AxisY.Minimum = 0;
           // cmdCntVsIOSizeChart.ChartAreas[0].AxisY.Maximum = Int64.Parse(numCommands) + 1;

            cmdCntVsIOSizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            cmdCntVsIOSizeChart.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 20";
            cmdCntVsIOSizeChart.Series[0].Color = Color.Blue;
            cmdCntVsIOSizeChart.Series[0].Name = "Number Of Commands";
            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\CmdVsIOSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOSize" + "," + "Number of Commands");
                        if (multiSim)
                        {
                            if (simulationTypeBox.Text == "Multi Sim based on IO Size")
                            {
                                long cmdCount = 0;
                                foreach (string io in validIOSize)
                                {
                                    if (enableWrkld == "1")
                                    {
                                        string fileName = @".\Reports_PCIe\num_cmds_report_iosize_" + io + "_" + "blksize_" + io + "_" + "qd_" + hostQDepth.Trim() + ".log";
                                        using (FileStream file1 = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                        {
                                            using (StreamReader reader = new StreamReader(file1))
                                            {

                                                while (reader.Peek() != -1)
                                                {
                                                    string line = reader.ReadLine();
                                                    string[] tokens = line.Split(' ');

                                                    cmdCount = int.Parse(tokens[1]);
                                                }
                                                reader.Close();
                                            }

                                            cmdCntVsIOSizeChart.Series[0].Points.AddXY(int.Parse(io), cmdCount);
                                            writer.WriteLine(int.Parse(io) + "," + cmdCount);
                                        }
                                    }
                                    else
                                    {
                                        cmdCntVsIOSizeChart.Series[0].Points.AddXY(int.Parse(io), Int64.Parse(numCommands));
                                        writer.WriteLine(int.Parse(io) + "," + Int64.Parse(numCommands));
                                    }
                                }
                            }
                            else if (simulationTypeBox.Text == MULTISIM_QD)
                            {
                                long cmdCount = 0;
                                if (enableWrkld == "1")
                                {
                                    
                                    foreach (string qd in validQueueDepth)
                                    {
                                        string fileName = @".\Reports_PCIe\num_cmds_report_iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + qd + ".log";
                                        using (FileStream file1 = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                        {
                                            using (StreamReader reader = new StreamReader(file1))
                                            {


                                                while (reader.Peek() != -1)
                                                {
                                                    string line = reader.ReadLine();
                                                    string[] tokens = line.Split(' ');

                                                    cmdCount = int.Parse(tokens[1]);
                                                }
                                                reader.Close();
                                            }

                                            cmdCntVsIOSizeChart.Series[0].Points.AddXY(int.Parse(validIOSize[0]), cmdCount);
                                            writer.WriteLine(int.Parse(validIOSize[0]) + "," + cmdCount);
                                        }
                                    }
                                }
                                else
                                {
                                    cmdCntVsIOSizeChart.Series[0].Points.AddXY(int.Parse(ioSize), Int64.Parse(numCommands));
                                    writer.WriteLine(int.Parse(ioSize) + "," + Int64.Parse(numCommands));
                                }
                            }
                        }
                        else
                        {
                            //chart11.ChartAreas[0].AxisX.CustomLabels.Add(ioIndex - 1.0, ioIndex + 1.0);
                            if (enableWrkld == "1")
                            {
                                long cmdCount = 0;
                                using (FileStream file1 = new FileStream(@".\Reports_PCIe\num_cmds_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    using (StreamReader reader = new StreamReader(file1))
                                    {


                                        while (reader.Peek() != -1)
                                        {
                                            string line = reader.ReadLine();
                                            string[] tokens = line.Split(' ');

                                            cmdCount = int.Parse(tokens[1]);
                                        }
                                        reader.Close();
                                    }

                                    cmdCntVsIOSizeChart.Series[0].Points.AddXY(int.Parse(ioSize), cmdCount);
                                    writer.WriteLine(int.Parse(ioSize) + "," + cmdCount);
                                }
                            }
                           
                            
                            else
                            {
                                cmdCntVsIOSizeChart.Series[0].Points.AddXY(int.Parse(ioSize), Int64.Parse(numCommands));
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
                throw new Exception(String.Format("An error ocurred while executing the data import: Command Count Vs IoSize", e.Message), e);
            }
        }

        private void plotCmdVsLBA(string fileName)
        {
            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\CmdVsLBA.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
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
                                        cmdCntVslbaChart.Series.Add(new Series());
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
                                        cmdCntVslbaChart.Series.Clear();
                                        cmdCntVslbaChart.Series.Add(new Series());
                                        cmdCntVslbaChart.Series[0].IsVisibleInLegend = false;
                                        cmdCntVslbaChart.ChartAreas[0].AxisX.Title = "LBA";
                                        cmdCntVslbaChart.ChartAreas[0].AxisY.Title = "Number Of Commands";
                                        cmdCntVslbaChart.ChartAreas[0].AxisY.Minimum = 0;
                                        cmdCntVslbaChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                                        cmdCntVslbaChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                                        cmdCntVslbaChart.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 5";
                                        ICollection key = hashtable.Keys;
                                        cmdCntVslbaChart.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                                        cmdCntVslbaChart.ChartAreas[0].AxisX.CustomLabels.Clear();
                                        foreach (Int32 k in key)
                                        {
                                            cmdCntVslbaChart.Series[0].Points.AddXY(k, hashtable[k]);
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

                                        cmdCntVslbaChart.Update();
                                        cmdCntVslbaChart.Series.Clear();
                                        cmdCntVslbaChart.ChartAreas[0].AxisX.Title = "LBA";
                                        cmdCntVslbaChart.ChartAreas[0].AxisY.Title = "Number Of Commands";
                                        cmdCntVslbaChart.ChartAreas[0].AxisX.Minimum = 0;
                                        cmdCntVslbaChart.ChartAreas[0].AxisY.Minimum = 0;
                                        cmdCntVslbaChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                                        cmdCntVslbaChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                                        int rangeIndex = 0;

                                        ICollection key = hashtable.Keys;

                                        rangeIndex = 0;
                                        Int32 upperIndex = 0;
                                        Int32 lowerIndex = 0;
                                        cmdCntVslbaChart.ChartAreas[0].AxisX.CustomLabels.Clear();
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
                                                cmdCntVslbaChart.ChartAreas[0].AxisX.CustomLabels.Add(k / binRange - 1, k / binRange + 1, range[rangeIndex]);
                                                cmdCntVslbaChart.Series.Add(range[rangeIndex]);
                                                cmdCntVslbaChart.Series[range[rangeIndex]].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                                                cmdCntVslbaChart.Series[range[rangeIndex]].Color = Color.Green;
                                                cmdCntVslbaChart.Series[range[rangeIndex]].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                                                cmdCntVslbaChart.Series[range[rangeIndex]].Points.AddXY(k / binRange, hashtable[k]);
                                                //   writer.WriteLine(k / binRange + "," + hashtable[k]);
                                                cmdCntVslbaChart.Series[range[rangeIndex]].IsVisibleInLegend = false;
                                                cmdCntVslbaChart.Series[range[rangeIndex]].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 20";
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

            string fileName = "";
            try
            {
                if (multiSim)
                {
                    string param = validParam.Trim();
                    if (simulationTypeBox.Text == "Multi Sim based on Block Size")
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
                            fileName = ".\\Reports_PCIe\\lba_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                        }
                        else
                        {
                            fileName = ".\\Reports_PCIe\\lba_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                        }
                    }
                    else if (simulationTypeBox.Text == "Multi Sim based on QD")
                    {
                        if (enableWrkld == "1")
                        {
                            fileName = ".\\Reports_PCIe\\lba_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + param + ".log";
                        }
                        else
                        {
                            fileName = ".\\Reports_PCIe\\lba_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + param + ".log";
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
                        fileName = ".\\Reports_PCIe\\lba_report_" + "iosize_" + param + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";

                    }

                }
                else
                {
                    fileName = ".\\Reports_PCIe\\lba_report.log";

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
                channelUtilChart.Series[0].Points.AddY(chanTransCount.ElementAt((chanIndex - 1)));
                channelUtilChart.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 20";
            }
        }

        double simTime = 0;

        #region pci_bus_utilization

        private void plotPCIe(string fileName1, string fileName2)
        {

            if (onfi_PCIeUtilChart.Series.Count != 0)
                onfi_PCIeUtilChart.Series.Clear();
            List<double> busUtil = new List<double>();
            List<double> time = new List<double>();

            List<double> busUtilRx = new List<double>();
            List<double> timeRx = new List<double>();
            try
            {
                using (FileStream file = new FileStream(fileName1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    parsePCIeBusUtilStream(busUtil, time, file);
                    //}
                    file.Close();
                }
                using (FileStream file = new FileStream(fileName2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    parsePCIeBusUtilStream(busUtilRx, timeRx, file);
                    //}
                    file.Close();
                }

                    List<double> busUtilPercent = new List<double>();
                    double avgBusUtilTx;
                    double effectiveBusUtilTx;
                    calculateTxAvgEffBusUtilization(busUtil, time, out avgBusUtilTx, out effectiveBusUtilTx);

                    double avgBusUtilRx;
                    double effectiveBusUtilRx;
                    calculateRxAvgEffBusUtilization(busUtilRx, timeRx, out avgBusUtilRx, out effectiveBusUtilRx);

                    onfi_PCIeUtilChart.Series.Add(new Series("TX_DATA"));
                    onfi_PCIeUtilChart.Series[0].IsVisibleInLegend = false;
                    onfi_PCIeUtilChart.Series[0].ChartType = SeriesChartType.FastPoint;
                    onfi_PCIeUtilChart.Series[0].Color = Color.Brown;

                    //chart9.Series.Add(new Series("TX_CMD"));
                    //chart9.Series[1].IsVisibleInLegend = false;
                    //chart9.Series[1].ChartType = SeriesChartType.FastPoint;
                    //chart9.Series[1].Color = Color.Gray;

                    onfi_PCIeUtilChart.Series.Add(new Series("RX_DATA"));
                    onfi_PCIeUtilChart.Series[1].IsVisibleInLegend = false;
                    onfi_PCIeUtilChart.Series[1].ChartType = SeriesChartType.FastPoint;
                    onfi_PCIeUtilChart.Series[1].Color = Color.Gray;

                    onfi_PCIeUtilChart.Series.Add(new Series("RX_CQ_CMD"));
                    onfi_PCIeUtilChart.Series[2].IsVisibleInLegend = false;
                    onfi_PCIeUtilChart.Series[2].ChartType = SeriesChartType.FastPoint;
                    onfi_PCIeUtilChart.Series[2].Color = Color.Brown;

                    onfi_PCIeUtilChart.Series.Add(new Series("RX_MEM_RD_CMD"));
                    onfi_PCIeUtilChart.Series[3].IsVisibleInLegend = false;
                    onfi_PCIeUtilChart.Series[3].ChartType = SeriesChartType.FastPoint;
                    onfi_PCIeUtilChart.Series[3].Color = Color.Blue;
                    //chart9.Series.Add(new Series("RX_CMD"));
                    //chart9.Series[1].IsVisibleInLegend = false;
                    //chart9.Series[1].ChartType = SeriesChartType.FastPoint;
                    //chart9.Series[1].Color = Color.Gray;

                    onfi_PCIeUtilChart.Legends.Clear();
                    setPCIeTxChartLegend(avgBusUtilTx, effectiveBusUtilTx);
                    setPCIeRxChartLegend(avgBusUtilRx, effectiveBusUtilRx);

                    int chartSize;
                    if (timeRx.Max() > time.Max())
                    {
                        if (busUtilRx.Max() > busUtil.Max())
                        {
                            chartSize = (int)(timeRx.Max() + busUtilRx.ElementAt(busUtilRx.Count() - 1));
                        }
                        else
                        {
                            chartSize = (int)(timeRx.Max() + busUtil.ElementAt(busUtil.Count() - 1));
                        }
                    }
                    else
                    {
                        if (busUtilRx.Max() > busUtil.Max())
                        {
                            chartSize = (int)(time.Max() + busUtilRx.ElementAt(busUtilRx.Count() - 1));
                        }
                        else
                        {
                            chartSize = (int)(time.Max() + busUtil.ElementAt(busUtil.Count() - 1));
                        }

                       
                    }

                    int numCmd = int.Parse(numCommands.Trim());
                    if (numCmd > 3)
                    {

                        chartSize /= 10;
                        if (chartSize < 1082)
                        {
                            chartSize = 1082;
                        }
                        onfi_PCIeUtilChart.Width = chartSize;

                    }
                    else
                    {
                        onfi_PCIeUtilChart.Width = 1082;
                    }
                    onfi_PCIeUtilChart.ChartAreas[0].Position.Y = 10;
                    onfi_PCIeUtilChart.ChartAreas[0].Position.Height = 80;
                    onfi_PCIeUtilChart.ChartAreas[0].Position.X = 1;
                    onfi_PCIeUtilChart.ChartAreas[0].Position.Width = 90;

                    onfi_PCIeUtilChart.ChartAreas[0].AxisY.LabelStyle.IsEndLabelVisible = false;
                    onfi_PCIeUtilChart.ChartAreas[0].AxisY.CustomLabels.Clear();

                    onfi_PCIeUtilChart.ChartAreas[0].AxisY.CustomLabels.Add(int.Parse(numChannel.Trim())*4   + 13, int.Parse(numChannel.Trim()) * 4 + 18, "Pcie Tx");
                    onfi_PCIeUtilChart.ChartAreas[0].AxisY.CustomLabels.Add(int.Parse(numChannel.Trim())* 4 + 7, int.Parse(numChannel.Trim()) * 4 + 12, "Pcie Rx");
                    onfi_PCIeUtilChart.ChartAreas[0].AxisX.Interval = 500;
                    onfi_PCIeUtilChart.ChartAreas[0].AxisX.Title = "Time (ns)";
                    lockChart9Position();

                    onfi_PCIeUtilChart.ChartAreas[0].AxisY.Minimum = 0;
                    onfi_PCIeUtilChart.ChartAreas[0].AxisX.Minimum = 0;

                    if (timeRx.Max() > time.Max())
                    {
                        onfi_PCIeUtilChart.ChartAreas[0].AxisX.Maximum = timeRx.Max() + busUtilRx.ElementAt(busUtilRx.Count() - 1);
                        simTime = timeRx.Max()/10;
                        if (busUtilRx.Max() > busUtil.Max())
                        {
                            onfi_PCIeUtilChart.ChartAreas[0].AxisX.Maximum = (int)(timeRx.Max() + busUtilRx.ElementAt(busUtilRx.Count() - 1));
                        }
                        else
                        {
                            onfi_PCIeUtilChart.ChartAreas[0].AxisX.Maximum = (int)(timeRx.Max() + busUtil.ElementAt(busUtil.Count() - 1));
                        }
                    }
                    else
                    {
                        onfi_PCIeUtilChart.ChartAreas[0].AxisX.Maximum = time.Max() + busUtil.ElementAt(busUtil.Count() - 1);
                        simTime = time.Max()/10;
                        if (busUtilRx.Max() > busUtil.Max())
                        {
                            onfi_PCIeUtilChart.ChartAreas[0].AxisX.Maximum = (int)(time.Max() + busUtilRx.ElementAt(busUtilRx.Count() - 1));
                        }
                        else
                        {
                            onfi_PCIeUtilChart.ChartAreas[0].AxisX.Maximum = (int)(time.Max() + busUtil.ElementAt(busUtil.Count() - 1));
                        }


                    }
                    plotTxBusUtilization("TX_DATA",  busUtil, time, /*int.Parse(numChannel.Trim()) +*/ 14);
                    plotBusUtilization("RX_DATA", "RX_CQ_CMD", "RX_MEM_RD_CMD", busUtilRx, timeRx, /*int.Parse(numChannel.Trim()) +*/ 8);
                   
            }

            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: PCIe Utilization Report", e.Message), e);
            }
        }

        private void plotBusUtilization(string data, string cmd1, string cmd2, List<double> busUtil, List<double> time, int yAxis)
        {
            for (int timeIndex = 0; timeIndex < time.Count(); timeIndex++)
            {
                double timeAxis = time.ElementAt(timeIndex);

                for (int busIndex = 0; busIndex < busUtil.ElementAt(timeIndex); busIndex++)
                {
                    if (busUtil.ElementAt(timeIndex) == 7)
                    {
                        onfi_PCIeUtilChart.Series[cmd1].Points.AddXY(timeAxis, int.Parse(numChannel.Trim()) * 4 + yAxis);
                    }
                    else if(busUtil.ElementAt(timeIndex) == 3)
                    {
                        onfi_PCIeUtilChart.Series[cmd2].Points.AddXY(timeAxis, int.Parse(numChannel.Trim()) * 4 + yAxis);
                    }

                    else
                    {
                        onfi_PCIeUtilChart.Series[data].Points.AddXY(timeAxis, int.Parse(numChannel.Trim()) * 4 + yAxis);
                    }
                  
                    timeAxis++;
                }

            }
        }
        private void plotTxBusUtilization(string data,  List<double> busUtil, List<double> time, int yAxis)
        {
            for (int timeIndex = 0; timeIndex < time.Count(); timeIndex++)
            {
                double timeAxis = time.ElementAt(timeIndex);

                for (int busIndex = 0; busIndex < busUtil.ElementAt(timeIndex); busIndex++)
                {
                    
                        onfi_PCIeUtilChart.Series[data].Points.AddXY(timeAxis, int.Parse(numChannel.Trim()) * 4 + yAxis);
                    
                    timeAxis++;
                }

            }
        }
        #endregion 

        private void calculateTxAvgEffBusUtilization(List<double> busUtil, List<double> time, out double avgBusUtil, out double effectiveBusUtil)
        {
            avgBusUtil = 0;
            effectiveBusUtil = 0;
            double totalBusTime = 0;
            double effectiveBusTime = 0;
            double commandTime = (double)(64 * 1000) / (double)(int.Parse(pcieSpeed));// (double)(2 * COMMAND_BL * 1000) / (double)(2 * mDdrSpeed);
            bool discardCmdTime = false;
            foreach (double t in busUtil)
            {
                totalBusTime += t;
                if (t >= 35)
                {
                    if(discardCmdTime)
                    effectiveBusTime += t;

                    discardCmdTime = true;
                }
            }
            avgBusUtil = (double)((double)totalBusTime / time.Max()) * 100;
            effectiveBusUtil =  (double)((double)effectiveBusTime / time.Max()) * 100;
        }
        private void calculateRxAvgEffBusUtilization(List<double> busUtil, List<double> time, out double avgBusUtil, out double effectiveBusUtil)
        {
            avgBusUtil = 0;
            effectiveBusUtil = 0;
            double totalBusTime = 0;
            double effectiveBusTime = 0;
            double commandTime = (double)(64 * 1000) / (double)(int.Parse(pcieSpeed));// (double)(2 * COMMAND_BL * 1000) / (double)(2 * mDdrSpeed);
            foreach (double t in busUtil)
            {
                totalBusTime += t;
                if (t >= 35)
                {
                    effectiveBusTime += t;
                }
            }
            avgBusUtil = (double)((double)totalBusTime / time.Max()) * 100;
            effectiveBusUtil = (double)((double)effectiveBusTime / time.Max()) * 100;
        }
        private void lockChart9Position()
        {
            ChartArea ca = onfi_PCIeUtilChart.ChartAreas[0];
            ElementPosition cap = ca.Position;
            ElementPosition ipp = ca.InnerPlotPosition;
            ipp.Width = 70;
            ipp.Height = 80;

            int ippX = 80;//inner plot X axis pixel position                          
            int caX = 60;//chartArea X axis pixel position
            //convert pixel to percentage
            int capWidth = getChartAreaWidthPercent(cap);
            float newIppX = getXAxisLocation(ippX, capWidth);
            float newCaX = getXAxisLocation(caX, onfi_PCIeUtilChart.ClientSize.Width);
            ipp.X = newIppX;
            ca.Position = new ElementPosition(newCaX, cap.Y, cap.Width, cap.Height);
        }

        private static float getXAxisLocation(int numerator, int denominator)
        {
            return (float)(numerator * 100) / denominator;
        }

        private int getChartAreaWidthPercent(ElementPosition cap)
        {
            return (int)((cap.Width * (float)onfi_PCIeUtilChart.ClientSize.Width) / 100f);
        }

        private void setPCIeTxChartLegend(double avgBusUtil, double effectiveBusUtil)
        {
            
            onfi_PCIeUtilChart.Legends.Add(new Legend());
          
            onfi_PCIeUtilChart.Legends[0].CustomItems.Add(new LegendItem());
            onfi_PCIeUtilChart.Legends[0].CustomItems[0].Cells.Add(new LegendCell("  "));
          
            onfi_PCIeUtilChart.Legends[0].CustomItems[0].Cells.Add(new LegendCell("Actual"));
            onfi_PCIeUtilChart.Legends[0].CustomItems[0].Cells.Add(new LegendCell("Effective"));
            onfi_PCIeUtilChart.Legends[0].CustomItems.Add(new LegendItem());
            onfi_PCIeUtilChart.Legends[0].CustomItems[1].Cells.Add(new LegendCell("PCIe_Tx"));
            onfi_PCIeUtilChart.Legends[0].CustomItems[1].Cells[0].Font = new Font("Arial", 8, System.Drawing.FontStyle.Bold);
            onfi_PCIeUtilChart.Legends[0].CustomItems[1].Cells.Add(new LegendCell(Math.Round(avgBusUtil, 2).ToString()));
            onfi_PCIeUtilChart.Legends[0].CustomItems[1].Cells.Add(new LegendCell(Math.Round(effectiveBusUtil, 2).ToString()));
            //onfi_PCIeUtilChart.Legends[0].CustomItems.Add(new LegendItem());
            //onfi_PCIeUtilChart.Legends[0].CustomItems[2].Cells.Add(new LegendCell(""));
        }

        private void setPCIeRxChartLegend(double avgBusUtil, double effectiveBusUtil)
        {

           // onfi_PCIeUtilChart.Legends.Add(new Legend());
            //onfi_PCIeUtilChart.Legends[0].CustomItems.Add(new LegendItem());
            //onfi_PCIeUtilChart.Legends[0].CustomItems[3].Cells.Add(new LegendCell("  "));
            //onfi_PCIeUtilChart.Legends[0].CustomItems[3].Cells.Add(new LegendCell("Actual"));
            //onfi_PCIeUtilChart.Legends[0].CustomItems[3].Cells.Add(new LegendCell("Effective"));
            onfi_PCIeUtilChart.Legends[0].CustomItems.Add(new LegendItem());
            onfi_PCIeUtilChart.Legends[0].CustomItems[2].Cells.Add(new LegendCell("PCIe_Rx"));
            onfi_PCIeUtilChart.Legends[0].CustomItems[2].Cells[0].Font = new Font("Arial", 8, System.Drawing.FontStyle.Bold);
            onfi_PCIeUtilChart.Legends[0].CustomItems[2].Cells.Add(new LegendCell(Math.Round(avgBusUtil, 2).ToString()));
            onfi_PCIeUtilChart.Legends[0].CustomItems[2].Cells.Add(new LegendCell(Math.Round(effectiveBusUtil, 2).ToString()));
            onfi_PCIeUtilChart.Legends[0].CustomItems.Add(new LegendItem());
            onfi_PCIeUtilChart.Legends[0].CustomItems[3].Cells.Add(new LegendCell(""));
        }

        private static void parsePCIeBusUtilStream(List<double> busUtil, List<double> time, FileStream file)
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
                throw new Exception(String.Format("An error occurred while executing the stream reading: PCIe Utilization Report", e.Message), e);
            }
        }

        private void PCIBusUtilizationData(bool multiSim)
        {
            string fileName1 = "";
            string fileName2 = "";

            if(multiSim)
            {
                onfi_PCIeUtilChart.Series.Clear();
                string chartType;
                if (FlashDemo)
                {
                    chartType = MULTISIM_QD;

                }
                else
                {
                    chartType = simulationTypeBox.Text;
                }
                string param = validMultiSimParam.Trim();
                
                 if(chartType == "Multi Sim based on QD")
                {
                    if (enableWrkld == "1")
                    {
                        fileName1 = ".\\Reports_PCIe\\TxPci_bus_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + param.Trim() + ".log";
                        fileName2 = ".\\Reports_PCIe\\RxPci_bus_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + param.Trim() + ".log";
                    }
                    else
                    {
                        fileName1 = ".\\Reports_PCIe\\TxPci_bus_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + param.Trim() + ".log";
                        fileName2 = ".\\Reports_PCIe\\RxPci_bus_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + param.Trim() + ".log";
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
                    fileName1 = ".\\Reports_PCIe\\TxPci_bus_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + validQDepth[qIndex] +".log";
                    fileName2 = ".\\Reports_PCIe\\RxPci_bus_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + validQDepth[qIndex] + ".log";
                }
                
            }
            
            else
            {
                fileName1 = ".\\Reports_PCIe\\TxPci_bus_utilization.log";
                fileName2 = ".\\Reports_PCIe\\RxPci_bus_utilization.log";
            }
            plotPCIe(fileName1, fileName2);
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

                                double totalBusTime = 0;
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

                        onfi_PCIeUtilChart.SuspendLayout();
                        int maxChannel = channelNum.Count();
                        for (int chanIndex = 0; chanIndex < channelNum.Count(); chanIndex++)
                        {
                            onfi_PCIeUtilChart.Series.Add(new Series());
                            onfi_PCIeUtilChart.Series[chanIndex +4].IsVisibleInLegend = false;
                        }
                        onfi_PCIeUtilChart.Series.Add(new Series());
                        onfi_PCIeUtilChart.Series[maxChannel + 4].ChartType = SeriesChartType.FastPoint;
                        onfi_PCIeUtilChart.Series[maxChannel + 4].Color = Color.Black;
                         onfi_PCIeUtilChart.Series[maxChannel + 4].IsVisibleInLegend = false;
                        
                        int lowLimit = 2;
                        int upLimit = 4;
                        for (int chanIndex = 0; chanIndex < channelNum.Count(); chanIndex++)
                        {
                            lowLimit = channelNum[chanIndex]*4 + 2;
                            upLimit = lowLimit + 5;
                            setOnfiLegends(channelNum, avgONFIUtil, effectiveUtil, lowLimit, upLimit, chanIndex);
                            onfi_PCIeUtilChart.ChartAreas[0].AxisY.CustomLabels.Add(lowLimit, upLimit, "ch " + channelNum[chanIndex].ToString());
                           
                            onfi_PCIeUtilChart.Series[chanIndex + 2].IsVisibleInLegend = false;

                            if (busUtilPerChannel[channelNum[chanIndex]].Count() != 0)
                            {
                              
                                onfi_PCIeUtilChart.Series[chanIndex + 2].ChartType = SeriesChartType.FastPoint;

                                for (int timeIndex = 0; timeIndex < timePerChannel[channelNum[chanIndex]].Count(); timeIndex++)
                                {
                                    double timeAxis = timePerChannel[channelNum[chanIndex]].ElementAt(timeIndex);

                                    for (int busIndex = 0; busIndex < busUtilPerChannel[channelNum[chanIndex]].ElementAt(timeIndex); busIndex++)
                                    {
                                        if (busUtilPerChannel[channelNum[chanIndex]].ElementAt(timeIndex) <= int.Parse(cmdOHTextBox.Text.Trim()))
                                        {
                                            onfi_PCIeUtilChart.Series[maxChannel + 4].Points.AddXY(timeAxis, channelNum[chanIndex] * 4 + 5);
                                        }
                                        else
                                        {
                                            onfi_PCIeUtilChart.Series[chanIndex + 2].Points.AddXY(timeAxis, channelNum[chanIndex] * 4 + 5);
                                        }
                                        timeAxis++;
                                    }
                                }

                            }//if
                            file.Close();
                        }//for
                        onfi_PCIeUtilChart.ResumeLayout();

                    }//using
                }
                //chart9.Series.Clear();
            }//end try
            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: ONFI Utilization Report", e.Message), e);
            }
        }

        private void setOnfiLegends(List<int> channelNum, List<double> avgONFIUtil, List<double> effectiveUtil, int lowLimit, int upLimit, int chanIndex)
        {
            onfi_PCIeUtilChart.Legends[0].CustomItems.Add(new LegendItem());
            onfi_PCIeUtilChart.Legends[0].CustomItems[chanIndex + 4].Cells.Add(new LegendCell("ch " + channelNum[chanIndex].ToString()));
            onfi_PCIeUtilChart.Legends[0].CustomItems[chanIndex + 4].Cells.Add(new LegendCell(Math.Round(avgONFIUtil[channelNum[chanIndex]], 2).ToString()));
            onfi_PCIeUtilChart.Legends[0].CustomItems[chanIndex + 4].Cells.Add(new LegendCell(Math.Round(effectiveUtil[channelNum[chanIndex]], 2).ToString()));
            
        }

        private void ONFIBusUtilizationData(bool multiSim)
        {
            string fileName1 = "";
            string fileName2 = "";
            if (multiSim)
            {

                string param = validMultiSimParam.Trim();
                 string chartType;
                
                 if (FlashDemo)
                 {
                     chartType = MULTISIM_QD;
                 }
                 else
                 {
                     chartType = simulationTypeBox.Text;
                 }
                if (chartType == "Multi Sim based on Block Size")
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
                        fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                        fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                    }
                    else
                    {
                        fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                        fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                    }
                }
                else if (chartType == "Multi Sim based on QD")
                {
                    //int qdIndex = 0;
                    param = validMultiSimParam.Trim();
                    if (enableWrkld == "1")
                    {
                        fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + param + ".log";
                        fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + param + ".log";
                    }
                    else
                    {
                        fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + param + ".log";
                        fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + validIOSize[0] + "_" + "qd_" + param + ".log";
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
                    fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report_" + "iosize_" + param + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
                    fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report_" + "iosize_" + param + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";

                }
            }
            else
            {
                fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report.log";
                fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report.log";

            }
            plotONFI(fileName1, fileName2);
        }//end function

        //private void chart9_SelectionRangeChanged(object sender, MouseEventArgs e)
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

        //    chart9.ChartAreas[0].AxisX.ScaleView.Zoom(startX, (endX - startX), DateTimeIntervalType.Auto, true);

        //}
        /** latencyCalculation
        * Calculates min/max/avg latency of commands 
        * @param bool multiSim
        * @return void
        **/
       // private void latencyCalculation(bool multiSim)
       // {
       //     try
       //     {
       //         if (multiSim)
       //         {
       //             maxLatency.Clear();
       //             minLatency.Clear();
       //             avgLatency.Clear();

       //             string fileName = "";
       //             if (comboBox33.Text == "Multi Sim based on Block Size")
       //             {
       //                 int qIndex = 0;
       //                 foreach (string blk in validBlockSize)
       //                 {
       //                     if (enableWrkld == "1")
       //                     {
       //                         fileName = ".\\Reports_PCIe\\latency_TB_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blk + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //                     }
       //                     else
       //                     {
       //                         fileName = ".\\Reports_PCIe\\latency_TB_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blk + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //                     }
       //                     using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //                     {
       //                         using (StreamReader reader = new StreamReader(file))
       //                         {
       //                             latency.Clear();
       //                             slotNum.Clear();
       //                             while (reader.Peek() != -1)
       //                             {
       //                                 string line = reader.ReadLine();
       //                                 string[] tokens = line.Split(' ');

       //                                 slotNum.Add(int.Parse(tokens[0]));
       //                                 latency.Add(double.Parse(tokens[1]));

       //                             }
       //                             reader.Close();
       //                             maxLatency.Add(latency.Max());
       //                             minLatency.Add(latency.Min());
       //                             avgLatency.Add(latency.Average());
       //                             qIndex++;
       //                         }
       //                     }
       //                 }
       //             }//if
       //             else if (comboBox33.Text == "Multi Sim based on QD")
       //             //else if(radioButton3.Checked == true)
       //             {
       //                 foreach (string qd in validQueueDepth)
       //                 {
       //                     if (enableWrkld == "1")
       //                     {
       //                         fileName = ".\\Reports_PCIe\\latency_TB_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + qd + ".log";
       //                     }
       //                     else
       //                     {
       //                         fileName = ".\\Reports_PCIe\\latency_TB_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + qd + ".log";
       //                     }
       //                     using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //                     {
       //                         using (StreamReader reader = new StreamReader(file))
       //                         {
       //                             latency.Clear();
       //                             slotNum.Clear();
       //                             while (reader.Peek() != -1)
       //                             {
       //                                 string line = reader.ReadLine();
       //                                 string[] tokens = line.Split(' ');

       //                                 slotNum.Add(int.Parse(tokens[0]));
       //                                 latency.Add(double.Parse(tokens[1]));

       //                             }
       //                             reader.Close();

       //                             maxLatency.Add(latency.Max());
       //                             minLatency.Add(latency.Min());

       //                             avgLatency.Add(latency.Average());

       //                         }
       //                     }
       //                 }
       //             }
       //             else
       //             {
       //                 int qIndex = 0;
       //                 foreach (string io in validIOSize)
       //                 {
       //                     fileName = ".\\Reports_PCIe\\latency_TB_report_" + "iosize_" + io + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";

       //                     using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //                     {
       //                         using (StreamReader reader = new StreamReader(file))
       //                         {
       //                             latency.Clear();
       //                             slotNum.Clear();
       //                             while (reader.Peek() != -1)
       //                             {
       //                                 string line = reader.ReadLine();
       //                                 string[] tokens = line.Split(' ');

       //                                 slotNum.Add(int.Parse(tokens[0]));
       //                                 latency.Add(double.Parse(tokens[1]));

       //                             }
       //                             reader.Close();
       //                             maxLatency.Add(latency.Max());
       //                             minLatency.Add(latency.Min());
       //                             avgLatency.Add(latency.Average());
       //                             qIndex++;
       //                         }
       //                     }
       //                 }
       //             }
       //         }
       //         else
       //         {
       //             using (FileStream file = new FileStream(@".\Reports_PCIe\latency_TB_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //             {
       //                 using (StreamReader reader = new StreamReader(file))
       //                 {

       //                     latency.Clear();
       //                     slotNum.Clear();
       //                     while (reader.Peek() != -1)
       //                     {
       //                         string line = reader.ReadLine();
       //                         string[] tokens = line.Split(' ');

       //                         slotNum.Add(int.Parse(tokens[0]));
       //                         latency.Add(double.Parse(tokens[1]));

       //                     }
       //                     reader.Close();

       //                     latencyMaxima = 0;
       //                     latencyMinima = 0;
       //                     latencyAverage = 0;
       //                     latencyMaxima = latency.Max();
       //                     latencyMinima = latency.Min();
       //                     latencyAverage = latency.Average();

       //                 }
       //                 file.Close();
       //             }
       //         }
       //     }

       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: latency Report", e.Message), e);
       //     }

       // }

       // /** iopsCalculation
       //* Calculates average iops of the memory controller 
       //* @param bool multiSim
       //* @return void
       //**/
       // private void iopsCalculation(bool multiSim)
       // {
       //     try
       //     {
       //         if (multiSim)
       //         {
       //             avgIOPS.Clear();
       //             string fileName = "";

       //             if (comboBox33.Text == "Multi Sim based on Block Size")
       //             {
       //                 int qIndex = 0;
       //                 foreach (string io in validBlockSize)
       //                 {
       //                     if (enableWrkld == "1")
       //                     {
       //                         fileName = ".\\Reports_PCIe\\IOPS_utilization_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + io + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //                     }
       //                     else
       //                     {
       //                         fileName = ".\\Reports_PCIe\\IOPS_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + io + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //                     }
       //                     qIndex++;
       //                     using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //                     {
       //                         using (StreamReader reader = new StreamReader(file))
       //                         {
       //                             while (reader.Peek() != -1)
       //                             {
       //                                 string line = reader.ReadLine();
       //                                 string[] tokens = line.Split(' ');

       //                                 double time = double.Parse(tokens[0]);
       //                                 avgIOPS.Add(double.Parse(tokens[1]));

       //                             }
       //                             reader.Close();
       //                         }
       //                         file.Close();
       //                     }
       //                 }
       //             }

       //             //else if (radioButton3.Checked)
       //             else if (comboBox33.Text == "Multi Sim based on QD")
       //             {
       //                 foreach (string qd in validQueueDepth)
       //                 {
       //                     if (enableWrkld == "1")
       //                     {
       //                         fileName = ".\\Reports_PCIe\\IOPS_utilization_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + qd + ".log";
       //                     }
       //                     else
       //                     {
       //                         fileName = ".\\Reports_PCIe\\IOPS_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + qd + ".log";
       //                     }
       //                     using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //                     {
       //                         using (StreamReader reader = new StreamReader(file))
       //                         {
       //                             while (reader.Peek() != -1)
       //                             {
       //                                 string line = reader.ReadLine();
       //                                 string[] tokens = line.Split(' ');

       //                                 double time = double.Parse(tokens[0]);
       //                                 avgIOPS.Add(double.Parse(tokens[1]));

       //                             }
       //                             reader.Close();
       //                         }
       //                         file.Close();
       //                     }
       //                 }
       //             }
       //             else
       //             {
       //                 int qIndex = 0;
       //                 foreach (string io in validIOSize)
       //                 {
       //                     fileName = ".\\Reports_PCIe\\IOPS_utilization_" + "iosize_" + io + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //                     qIndex++;
       //                     using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //                     {
       //                         using (StreamReader reader = new StreamReader(file))
       //                         {
       //                             while (reader.Peek() != -1)
       //                             {
       //                                 string line = reader.ReadLine();
       //                                 string[] tokens = line.Split(' ');

       //                                 double time = double.Parse(tokens[0]);
       //                                 avgIOPS.Add(double.Parse(tokens[1]));

       //                             }
       //                             reader.Close();
       //                         }
       //                         file.Close();
       //                     }
       //                 }
       //             }


       //         }
       //         else
       //         {
       //             using (FileStream file = new FileStream(@".\Reports_PCIe\IOPS_utilization.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //             {
       //                 using (StreamReader reader = new StreamReader(file))
       //                 {
       //                     while (reader.Peek() != -1)
       //                     {
       //                         string line = reader.ReadLine();
       //                         string[] tokens = line.Split(' ');

       //                         double time = double.Parse(tokens[0]);
       //                         iopsAverage = double.Parse(tokens[1]);
       //                     }
       //                     reader.Close();
       //                 }
       //                 file.Close();
       //             }
       //         }
       //     }

       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error occurred while executing the data import: IOPS Report", e.Message), e);
       //     }
       // }


       // /** bankUtilizationData
       // * Calculates utilization of each banks on each channel and
       // * display it on the graph
       // * @return void
       // **/
       // private void bankUtilizationData()
       // {
       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\channel_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //         {
       //             using (StreamReader reader = new StreamReader(file))
       //             {

       //                 List<int> channel = new List<int>();
       //                 List<int> bank = new List<int>();
       //                 List<long> transCount = new List<long>();
       //                 chanTransCount.Clear();
       //                 int bankSkipGap = int.Parse(emCacheSize) / int.Parse(pageSize);
       //                 while (reader.Peek() != -1)
       //                 {
       //                     string line = reader.ReadLine();
       //                     string[] tokens = line.Split(' ');

       //                     bank.Add(int.Parse(tokens[0]));
       //                     channel.Add(int.Parse(tokens[1]));
       //                     transCount.Add(Int64.Parse(tokens[2]));
       //                 }
       //                 reader.Close();
       //                 List<long> transactionCount = new List<long>();

       //                 int bankCount = (int.Parse(numBanks) * int.Parse(numDie));
       //                 for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
       //                 {
       //                     long tempVal = 0;
       //                     for (int bankIndex = 0; bankIndex < bankCount; bankIndex += bankSkipGap)
       //                     {
       //                         tempVal += transCount[bankIndex + chanIndex * (int.Parse(numBanks) * int.Parse(numDie))];

       //                     }
       //                     chanTransCount.Add(tempVal);
       //                 }

       //                 for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
       //                 {
       //                     for (int bankIndex = 0; bankIndex < bankCount; bankIndex += bankSkipGap)
       //                     {
       //                         transactionCount.Add(transCount[chanIndex * bankCount + bankIndex]);
       //                     }

       //                 }
       //                 chart1.Update();
       //                 if (chart1.Series.Count != 0)
       //                     chart1.Series[0].Points.Clear();
       //                 chart1.ChartAreas[0].AxisX.Title = "Logical Banks";

       //                 chart1.ChartAreas[0].AxisY.Title = "No. Of References (CodeWord)";
       //                 chart1.ChartAreas[0].AxisY.Minimum = 0;
       //                 chart1.ChartAreas[0].AxisY.Maximum = transCount.Max();
       //                 chart1.ChartAreas[0].AxisX.Maximum = bankCount / bankSkipGap + 1;

       //                 if (transCount.Max() > 100)
       //                 {
       //                     chart1.ChartAreas[0].AxisY.Interval = 10;
       //                     chart1.ChartAreas[0].AxisY.IntervalOffset = 10;
       //                 }
       //                 if (chart1.Series.Count != 0)
       //                     chart1.Series.Clear();
       //                 if (chart1.Legends.Count != 0)
       //                     chart1.Legends.Clear();
       //                 chart1.Legends.Add("Channels");
       //                 chart1.Legends["Channels"].Title = "Channels";

       //                 for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
       //                 {
       //                     chart1.Series.Add(chanIndex.ToString());
       //                     chart1.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 5";
       //                 }

       //                 int bankLabel = 0;
       //                 int logicBankIndex = 1;
       //                 for (int cwBankIndex = 0; cwBankIndex < bankCount / bankSkipGap; cwBankIndex++)
       //                 {
       //                     bankLabel = logicBankIndex - 1;// (bankIndex - 1);
       //                     chart1.ChartAreas[0].AxisX.CustomLabels.Add(logicBankIndex - 1.0, logicBankIndex + 1.0, bankLabel.ToString());
       //                     for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
       //                     {
       //                         chart1.Series[chanIndex.ToString()].Points.AddY(transCount.ElementAt(chanIndex * bankCount + bankSkipGap * cwBankIndex));
       //                     }
       //                     logicBankIndex++;
       //                 }
       //             }
       //             file.Close();
       //         }

       //     }

       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error occurred while executing the data import: Channel Report", e.Message), e);
       //     }

       // }

       // /** IOPSVsIOSize
       // * Plots average IOPS versus IOSize graph
       // * @param bool multiSim
       // * @return void
       // **/
       // private void IOPSVsBlockSize(bool multiSim)
       // {
       //     iopsVsIosizeChart.ChartAreas[0].AxisX.Title = "Block Size";
       //     iopsVsIosizeChart.Series.Clear();
       //     //iopsVsIosizeChart.Titles.Clear();
       //     //iopsVsIosizeChart.Titles.Add("IOPS Vs Block Size");
       //     iopsVsIosizeChart.Titles[0].Text = "IOPS Vs Block Size";
       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\IopsVsBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("BlockSize" + "," + "IOPS(Millions)");
       //                 if (multiSim)
       //                 {

       //                     if (comboBox33.Text == "Multi Sim based on Block Size")
       //                     {
       //                         int iopsIndex = 0;

       //                         iopsVsIosizeChart.Series.Add("0");

       //                         iopsVsIosizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
       //                         iopsVsIosizeChart.Series[0].Color = Color.Blue;
       //                         iopsVsIosizeChart.Series[0].BorderWidth = 3;
       //                         iopsVsIosizeChart.Series[0].Name = "IOPS";
       //                         iopsVsIosizeChart.Annotations.Clear();
       //                         iopsVsIosizeChart.Series[0].LegendText = string.Concat("QD " + validQDepth[validQDepth.Count() - 1].ToString());

       //                         for (int ioIndex = 0; ioIndex < validBlockSize.Count(); ioIndex++)
       //                         {
       //                             iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(validBlockSize[ioIndex]), avgIOPS[iopsIndex]);
       //                             writer.WriteLine(validBlockSize[ioIndex] + "," + avgIOPS[iopsIndex]);
       //                             iopsVsIosizeChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
       //                             iopsVsIosizeChart.Series[0].MarkerSize = 10;
       //                             iopsVsIosizeChart.Series[0].MarkerColor = Color.Green;
       //                             iopsIndex++;
       //                         }

       //                     }
       //                 }
       //                 else
       //                 {
       //                     iopsVsIosizeChart.Series.Add("0");
       //                     if (enableWrkld == "1")
       //                     {
       //                         iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(blkSize), iopsAverage);
       //                         writer.WriteLine(blkSize + "," + iopsAverage);
       //                     }
       //                     else
       //                     {
       //                         iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(blkSize), iopsAverage);
       //                         writer.WriteLine(blkSize + "," + iopsAverage);
       //                     }
       //                     iopsVsIosizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
       //                     iopsVsIosizeChart.Series[0].Name = "IOPS";
       //                     iopsVsIosizeChart.Annotations.Clear();
       //                     iopsVsIosizeChart.Series[0].LegendText = string.Concat("QD " + depthOfQueue);

       //                 }
       //                 writer.Close();
       //             }
       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error occurred while executing the data import: IOPSVsBlockSize", e.Message), e);
       //     }
       // }

       // private void IOPSVsIOSize(bool multiSim)
       // {
       //     iopsVsIosizeChart.ChartAreas[0].AxisX.Title = "IO Size";
       //     iopsVsIosizeChart.Series.Clear();
       //     //iopsVsIosizeChart.Titles.Clear();
       //     //iopsVsIosizeChart.Titles.Add("IOPS Vs IO Size");
       //     iopsVsIosizeChart.Titles[0].Text = "IOPS Vs IO Size";

       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\IopsVsIOSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("IOSize" + "," + "IOPS(Millions)");
       //                 if (multiSim)
       //                 {

       //                     if (comboBox33.Text == "Multi Sim based on IO Size")
       //                     {
       //                         int iopsIndex = 0;

       //                         iopsVsIosizeChart.Series.Add("0");

       //                         iopsVsIosizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
       //                         iopsVsIosizeChart.Series[0].Color = Color.Blue;
       //                         iopsVsIosizeChart.Series[0].BorderWidth = 3;
       //                         iopsVsIosizeChart.Series[0].Name = "IOPS";
       //                         iopsVsIosizeChart.Annotations.Clear();
       //                         iopsVsIosizeChart.Series[0].LegendText = string.Concat("QD " + validQDepth[validQDepth.Count() - 1].ToString());

       //                         for (int ioIndex = 0; ioIndex < validIOSize.Count(); ioIndex++)
       //                         {
       //                             iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(validIOSize[ioIndex]), avgIOPS[iopsIndex]);
       //                             writer.WriteLine(validIOSize[ioIndex] + "," + avgIOPS[iopsIndex]);
       //                             iopsVsIosizeChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
       //                             iopsVsIosizeChart.Series[0].MarkerSize = 10;
       //                             iopsVsIosizeChart.Series[0].MarkerColor = Color.Green;
       //                             iopsIndex++;
       //                         }

       //                     }
       //                 }
       //                 else
       //                 {
       //                     iopsVsIosizeChart.Series.Add("0");
       //                     if (enableWrkld == "1")
       //                     {
       //                         iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(wrkloadBS), iopsAverage);
       //                         writer.WriteLine(wrkloadBS + "," + iopsAverage);
       //                     }
       //                     else
       //                     {
       //                         iopsVsIosizeChart.Series[0].Points.AddXY(int.Parse(ioSize), iopsAverage);
       //                         writer.WriteLine(ioSize + "," + iopsAverage);
       //                     }
       //                     iopsVsIosizeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
       //                     iopsVsIosizeChart.Series[0].Name = "IOPS";
       //                     iopsVsIosizeChart.Annotations.Clear();
       //                     iopsVsIosizeChart.Series[0].LegendText = string.Concat("QD " + depthOfQueue);

       //                 }
       //                 writer.Close();
       //             }
       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: IOPSVsBlockSize", e.Message), e);
       //     }
       // }
       // /** LatencyVsIOSize
       // * Plots average latency versus IOSize graph
       // * @param bool multiSim
       // * @return void
       // **/
       // private void LatencyVsBlockSize(bool multiSim)
       // {
       //     chart8.ChartAreas[0].AxisX.Title = "Block Size";
       //     chart8.Series.Clear();
       //     //chart8.Titles.Clear();
       //     //chart8.Titles.Add("Latency Vs Block Size");
       //     chart8.Titles[0].Text = "Latency Vs Block Size";
       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("BlockSize" + "," + "Latency(us)");
       //                 if (multiSim)
       //                 {
       //                     if (comboBox33.Text == "Multi Sim based on Block Size")
       //                     {
       //                         chart8.Annotations.Clear();

       //                         int avgLatencyIndex = 0;
       //                         chart8.Series.Add("0");

       //                         chart8.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
       //                         chart8.Series[0].BorderWidth = 3;
       //                         chart8.Series[0].Color = Color.Blue;
       //                         chart8.Series[0].Name = "Latency";
       //                         string legends = string.Concat("QD " + validQDepth[validQDepth.Count() - 1].ToString());
       //                         chart8.Series[0].LegendText = legends;

       //                         for (int ioIndex = 0; ioIndex < validBlockSize.Count(); ioIndex++)
       //                         {

       //                             chart8.Series[0].Points.AddXY(int.Parse(validBlockSize[ioIndex]), avgLatency[avgLatencyIndex]);
       //                             writer.WriteLine(validBlockSize[ioIndex] + "," + avgLatency[avgLatencyIndex]);
       //                             chart8.Series[0].MarkerStyle = MarkerStyle.Diamond;
       //                             chart8.Series[0].MarkerSize = 10;
       //                             chart8.Series[0].MarkerColor = Color.Green;

       //                             avgLatencyIndex++;
       //                         }

       //                     }
       //                 }
       //                 else
       //                 {
       //                     chart8.Series.Add("0");
       //                     if (enableWrkld == "1")
       //                     {
       //                         chart8.Series[0].Points.AddXY(int.Parse(blkSize), latencyAverage);
       //                         writer.WriteLine(blkSize + "," + latencyAverage);
       //                     }
       //                     else
       //                     {
       //                         chart8.Series[0].Points.AddXY(int.Parse(blkSize), latencyAverage);
       //                         writer.WriteLine(blkSize + "," + latencyAverage);
       //                     }
       //                     chart8.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
       //                     chart8.Series[0].Name = "Latency";
       //                     chart8.Series[0].LegendText = string.Concat("QD " + depthOfQueue);
       //                     chart8.Annotations.Clear();

       //                 }
       //                 writer.Close();
       //             }

       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
       //     }

       // }

       // private void LatencyVsIOSize(bool multiSim)
       // {
       //     chart8.ChartAreas[0].AxisX.Title = "IO Size";
       //     chart8.Series.Clear();
       //     //chart8.Titles.Clear();
       //     //chart8.Titles.Add("Latency Vs IO Size");
       //     chart8.Titles[0].Text = "Latency Vs IO Size";
       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsIOSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("IOSize" + "," + "Latency(us)");
       //                 if (multiSim)
       //                 {
       //                     if (comboBox33.Text == "Multi Sim based on IO Size")
       //                     {
       //                         chart8.Annotations.Clear();

       //                         int avgLatencyIndex = 0;
       //                         chart8.Series.Add("0");

       //                         chart8.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
       //                         chart8.Series[0].BorderWidth = 3;
       //                         chart8.Series[0].Color = Color.Blue;
       //                         chart8.Series[0].Name = "Latency";
       //                         string legends = string.Concat("QD " + validQDepth[validQDepth.Count() - 1].ToString());
       //                         chart8.Series[0].LegendText = legends;

       //                         for (int ioIndex = 0; ioIndex < validIOSize.Count(); ioIndex++)
       //                         {

       //                             chart8.Series[0].Points.AddXY(int.Parse(validIOSize[ioIndex]), avgLatency[avgLatencyIndex]);
       //                             writer.WriteLine(validIOSize[ioIndex] + "," + avgLatency[avgLatencyIndex]);
       //                             chart8.Series[0].MarkerStyle = MarkerStyle.Diamond;
       //                             chart8.Series[0].MarkerSize = 10;
       //                             chart8.Series[0].MarkerColor = Color.Green;

       //                             avgLatencyIndex++;
       //                         }

       //                     }
       //                 }
       //                 else
       //                 {
       //                     chart8.Series.Add("0");
       //                     if (enableWrkld == "1")
       //                     {
       //                         chart8.Series[0].Points.AddXY(int.Parse(wrkloadBS), latencyAverage);
       //                         writer.WriteLine(wrkloadBS + "," + latencyAverage);
       //                     }
       //                     else
       //                     {
       //                         chart8.Series[0].Points.AddXY(int.Parse(ioSize), latencyAverage);
       //                         writer.WriteLine(ioSize + "," + latencyAverage);
       //                     }
       //                     chart8.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
       //                     chart8.Series[0].Name = "Latency";
       //                     chart8.Series[0].LegendText = string.Concat("QD " + depthOfQueue);
       //                     chart8.Annotations.Clear();

       //                 }
       //                 writer.Close();
       //             }

       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
       //     }

       // }

       // private void IOPSVsQD(bool multiSim)
       // {

       //     //chart9.ChartAreas.Clear();
       //     chart9.Series.Clear();
       //     chart9.Series.Add("0");
       //     //chart9.ChartAreas.Add("0");
       //     chart9.ChartAreas[0].AxisX.IntervalAutoMode = 0;
       //     chart9.ChartAreas[0].AxisX.Minimum = 1;
       //     chart9.ChartAreas[0].AxisX.Maximum = 512;
       //     chart9.ChartAreas[0].AxisX.Title = "QD";
       //     chart9.ChartAreas[0].AxisY.Title = "IOPS (Millions)";

       //     int startOffset = 0;
       //     int endOffset = 60;

       //     for (double x = chart9.ChartAreas[0].AxisX.Minimum; x < chart9.ChartAreas[0].AxisX.Maximum; x *= 2)
       //     {
       //         CustomLabel qdLabel = new CustomLabel(startOffset, endOffset, x.ToString(), 0, LabelMarkStyle.None);
       //         chart9.ChartAreas[0].AxisX.CustomLabels.Add(qdLabel);
       //         startOffset = endOffset;
       //         endOffset += 60;
       //     }

       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\IOPSVsQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("QD" + "," + "IOPS(Millions)");
       //                 if (multiSim)
       //                 {
       //                     //if (radioButton3.Checked)
       //                     if (comboBox33.Text == "Multi Sim based on QD")
       //                     {
       //                         for (int queueDIndex = 0; queueDIndex < validQueueDepth.Count(); queueDIndex++)
       //                         {
       //                             switch (int.Parse(validQueueDepth[queueDIndex]))
       //                             {
       //                                 case 1:
       //                                     {
       //                                         startOffset = 0;
       //                                         endOffset = 60;
       //                                         break;
       //                                     }
       //                                 case 2:
       //                                     {
       //                                         startOffset = 60;
       //                                         endOffset = 120;
       //                                         break;
       //                                     }
       //                                 case 4:
       //                                     {
       //                                         startOffset = 120;
       //                                         endOffset = 180;
       //                                         break;
       //                                     }
       //                                 case 8:
       //                                     {
       //                                         startOffset = 180;
       //                                         endOffset = 240;
       //                                         break;
       //                                     }
       //                                 case 16:
       //                                     {
       //                                         startOffset = 240;
       //                                         endOffset = 300;
       //                                         break;
       //                                     }
       //                                 case 32:
       //                                     {
       //                                         startOffset = 300;
       //                                         endOffset = 360;
       //                                         break;
       //                                     }
       //                                 case 64:
       //                                     {
       //                                         startOffset = 360;
       //                                         endOffset = 420;
       //                                         break;
       //                                     }
       //                                 case 128:
       //                                     {
       //                                         startOffset = 420;
       //                                         endOffset = 480;
       //                                         break;
       //                                     }
       //                                 case 256:
       //                                     {
       //                                         startOffset = 480;
       //                                         endOffset = 540;
       //                                         break;
       //                                     }
       //                             }

       //                             int offset = (startOffset + endOffset) / 2;
       //                             chart9.Series[0].Color = Color.Blue;

       //                             if (int.Parse(queueDepth) > numSlot)
       //                             {
       //                                 chart9.Series[0].Points.AddXY(offset, avgIOPS[queueDIndex]);
       //                                 writer.WriteLine(validQueueDepth[queueDIndex] + "," + avgIOPS[queueDIndex]);
       //                             }
       //                             else
       //                             {
       //                                 chart9.Series[0].Points.AddXY(offset, avgIOPS[queueDIndex]);
       //                                 writer.WriteLine(validQueueDepth[queueDIndex] + "," + avgIOPS[queueDIndex]);
       //                             }
       //                             chart9.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
       //                             chart9.Series[0].BorderWidth = 3;
       //                             chart9.Series[0].Name = "IOPS(M)";
       //                             chart9.Series[0].MarkerStyle = MarkerStyle.Diamond;
       //                             chart9.Series[0].MarkerSize = 10;
       //                             chart9.Series[0].MarkerColor = Color.Green;
       //                         }
       //                         chart9.Annotations.Clear();
       //                         TextAnnotation iosize = new TextAnnotation();
       //                         iosize.Name = "iosize";
       //                         if (enableWrkld == "1")
       //                         {
       //                             iosize.Text = string.Concat("IOSize " + wrkloadBS.Trim());
       //                         }
       //                         else
       //                         {
       //                             iosize.Text = string.Concat("IOSize " + ioSize);
       //                         }
       //                         iosize.ForeColor = Color.Black;
       //                         iosize.Font = new Font("Arial", 10, FontStyle.Bold); ;
       //                         iosize.LineWidth = 1;
       //                         chart9.Annotations.Add(iosize);
       //                         chart9.Annotations[0].AxisX = chart9.ChartAreas[0].AxisX;
       //                         chart9.Annotations[0].AxisY = chart9.ChartAreas[0].AxisY;
       //                         chart9.Annotations[0].AnchorDataPoint = chart9.Series[0].Points[0];

       //                     }

       //                 }
       //                 else
       //                 {
       //                     switch (int.Parse(depthOfQueue))
       //                     {
       //                         case 1:
       //                             {
       //                                 startOffset = 0;
       //                                 endOffset = 60;
       //                                 break;
       //                             }
       //                         case 2:
       //                             {
       //                                 startOffset = 60;
       //                                 endOffset = 120;
       //                                 break;
       //                             }
       //                         case 4:
       //                             {
       //                                 startOffset = 120;
       //                                 endOffset = 180;
       //                                 break;
       //                             }
       //                         case 8:
       //                             {
       //                                 startOffset = 180;
       //                                 endOffset = 240;
       //                                 break;
       //                             }
       //                         case 16:
       //                             {
       //                                 startOffset = 240;
       //                                 endOffset = 300;
       //                                 break;
       //                             }
       //                         case 32:
       //                             {
       //                                 startOffset = 300;
       //                                 endOffset = 360;
       //                                 break;
       //                             }
       //                         case 64:
       //                             {
       //                                 startOffset = 360;
       //                                 endOffset = 420;
       //                                 break;
       //                             }
       //                         case 128:
       //                             {
       //                                 startOffset = 420;
       //                                 endOffset = 480;
       //                                 break;
       //                             }
       //                         case 256:
       //                             {
       //                                 startOffset = 480;
       //                                 endOffset = 540;
       //                                 break;
       //                             }
       //                     }

       //                     int offset = (startOffset + endOffset) / 2;


       //                     chart9.Series[0].Color = Color.Blue;

       //                     if (int.Parse(queueDepth) > numSlot)
       //                     {
       //                         chart9.Series[0].Points.AddXY(offset, iopsAverage);
       //                         writer.WriteLine(depthOfQueue + "," + iopsAverage);
       //                     }
       //                     else
       //                     {
       //                         chart9.Series[0].Points.AddXY(offset, iopsAverage);
       //                         writer.WriteLine(depthOfQueue + "," + iopsAverage);
       //                     }
       //                     chart9.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
       //                     chart9.Series[0].Name = "IOPS(M)";
       //                     chart9.Annotations.Clear();
       //                     TextAnnotation iosize = new TextAnnotation();
       //                     iosize.Name = "iosize";
       //                     iosize.Text = string.Concat("IOSize " + ioSize);
       //                     iosize.ForeColor = Color.Black;
       //                     iosize.Font = new Font("Arial", 10, FontStyle.Bold); ;
       //                     iosize.LineWidth = 1;
       //                     chart9.Annotations.Add(iosize);
       //                     chart9.Annotations[0].AxisX = chart9.ChartAreas[0].AxisX;
       //                     chart9.Annotations[0].AxisY = chart9.ChartAreas[0].AxisY;
       //                     chart9.Annotations[0].AnchorDataPoint = chart9.Series[0].Points[0];

       //                 }

       //                 writer.Close();
       //             }

       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
       //     }
       // }

       // private void LatencyVsQD(bool multiSim)
       // {

       //     //chart10.ChartAreas.Clear();
       //     chart10.Series.Clear();
       //     chart10.Series.Add("0");
       //     //chart10.ChartAreas.Add("0");
       //     chart10.ChartAreas[0].AxisX.IntervalAutoMode = 0;
       //     chart10.ChartAreas[0].AxisX.Minimum = 1;
       //     chart10.ChartAreas[0].AxisX.Maximum = 512;
       //     chart10.ChartAreas[0].AxisX.Title = "QD";
       //     chart10.ChartAreas[0].AxisY.Title = "Latency";

       //     int startOffset = 0;
       //     int endOffset = 60;

       //     for (double x = chart10.ChartAreas[0].AxisX.Minimum; x < chart10.ChartAreas[0].AxisX.Maximum; x *= 2)
       //     {
       //         CustomLabel qdLabel = new CustomLabel(startOffset, endOffset, x.ToString(), 0, LabelMarkStyle.None);
       //         chart10.ChartAreas[0].AxisX.CustomLabels.Add(qdLabel);
       //         startOffset = endOffset;
       //         endOffset += 60;
       //     }

       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("QD" + "," + "Latency(us)");
       //                 if (multiSim)
       //                 {
       //                     //if (radioButton3.Checked)
       //                     if (comboBox33.Text == "Multi Sim based on QD")
       //                     {

       //                         for (int queueIndex = 0; queueIndex < validQueueDepth.Count(); queueIndex++)
       //                         {
       //                             switch (int.Parse(validQueueDepth[queueIndex]))
       //                             {
       //                                 case 1:
       //                                     {
       //                                         startOffset = 0;
       //                                         endOffset = 60;
       //                                         break;
       //                                     }
       //                                 case 2:
       //                                     {
       //                                         startOffset = 60;
       //                                         endOffset = 120;
       //                                         break;
       //                                     }
       //                                 case 4:
       //                                     {
       //                                         startOffset = 120;
       //                                         endOffset = 180;
       //                                         break;
       //                                     }
       //                                 case 8:
       //                                     {
       //                                         startOffset = 180;
       //                                         endOffset = 240;
       //                                         break;
       //                                     }
       //                                 case 16:
       //                                     {
       //                                         startOffset = 240;
       //                                         endOffset = 300;
       //                                         break;
       //                                     }
       //                                 case 32:
       //                                     {
       //                                         startOffset = 300;
       //                                         endOffset = 360;
       //                                         break;
       //                                     }
       //                                 case 64:
       //                                     {
       //                                         startOffset = 360;
       //                                         endOffset = 420;
       //                                         break;
       //                                     }
       //                                 case 128:
       //                                     {
       //                                         startOffset = 420;
       //                                         endOffset = 480;
       //                                         break;
       //                                     }
       //                                 case 256:
       //                                     {
       //                                         startOffset = 480;
       //                                         endOffset = 540;
       //                                         break;
       //                                     }
       //                             }

       //                             int offset = (startOffset + endOffset) / 2;
       //                             chart10.Series[0].Color = Color.Blue;
       //                             chart10.Series[0].Points.AddXY(offset, avgLatency[queueIndex]);
       //                             writer.WriteLine(validQueueDepth[queueIndex] + "," + avgLatency[queueIndex]);
       //                             chart10.Series[0].MarkerStyle = MarkerStyle.Diamond;
       //                             chart10.Series[0].MarkerSize = 10;
       //                             chart10.Series[0].MarkerColor = Color.Green;
       //                         }

       //                         chart10.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
       //                         chart10.Series[0].BorderWidth = 3;
       //                         chart10.Series[0].Name = "Latency(us)";
       //                         chart10.Annotations.Clear();
       //                         TextAnnotation myLine = new TextAnnotation();
       //                         myLine.Name = "myLine";
       //                         if (enableWrkld == "1")
       //                         {
       //                             myLine.Text = string.Concat("IOSize " + wrkloadBS.Trim());
       //                         }
       //                         else
       //                         {
       //                             myLine.Text = string.Concat("IOSize " + ioSize);
       //                         }
       //                         myLine.ForeColor = Color.Black;
       //                         myLine.Font = new Font("Arial", 10, FontStyle.Bold); ;
       //                         myLine.LineWidth = 1;
       //                         chart10.Annotations.Add(myLine);
       //                         chart10.Annotations[0].AxisX = chart10.ChartAreas[0].AxisX;
       //                         chart10.Annotations[0].AxisY = chart10.ChartAreas[0].AxisY;
       //                         chart10.Annotations[0].AnchorDataPoint = chart10.Series[0].Points[0];
       //                     }
       //                 }
       //                 else
       //                 {
       //                     switch (int.Parse(depthOfQueue))
       //                     {
       //                         case 1:
       //                             {
       //                                 startOffset = 0;
       //                                 endOffset = 60;
       //                                 break;
       //                             }
       //                         case 2:
       //                             {
       //                                 startOffset = 60;
       //                                 endOffset = 120;
       //                                 break;
       //                             }
       //                         case 4:
       //                             {
       //                                 startOffset = 120;
       //                                 endOffset = 180;
       //                                 break;
       //                             }
       //                         case 8:
       //                             {
       //                                 startOffset = 180;
       //                                 endOffset = 240;
       //                                 break;
       //                             }
       //                         case 16:
       //                             {
       //                                 startOffset = 240;
       //                                 endOffset = 300;
       //                                 break;
       //                             }
       //                         case 32:
       //                             {
       //                                 startOffset = 300;
       //                                 endOffset = 360;
       //                                 break;
       //                             }
       //                         case 64:
       //                             {
       //                                 startOffset = 360;
       //                                 endOffset = 420;
       //                                 break;
       //                             }
       //                         case 128:
       //                             {
       //                                 startOffset = 420;
       //                                 endOffset = 480;
       //                                 break;
       //                             }
       //                         case 256:
       //                             {
       //                                 startOffset = 480;
       //                                 endOffset = 540;
       //                                 break;
       //                             }
       //                     }

       //                     int offset = (startOffset + endOffset) / 2;

       //                     chart10.Series[0].Color = Color.OrangeRed;
       //                     chart10.Series[0].Points.AddXY(offset, latencyAverage);
       //                     writer.WriteLine(int.Parse(depthOfQueue) + "," + latencyAverage);
       //                     chart10.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
       //                     chart10.Series[0].Name = "Latency(us)";
       //                     chart10.Annotations.Clear();
       //                     TextAnnotation myLine = new TextAnnotation();
       //                     myLine.Name = "myLine";
       //                     myLine.Text = string.Concat("IOSize " + ioSize);
       //                     myLine.ForeColor = Color.Black;
       //                     myLine.Font = new Font("Arial", 10, FontStyle.Bold);
       //                     myLine.LineWidth = 1;
       //                     chart10.Annotations.Add(myLine);
       //                     chart10.Annotations[0].AxisX = chart10.ChartAreas[0].AxisX;
       //                     chart10.Annotations[0].AxisY = chart10.ChartAreas[0].AxisY;
       //                     chart10.Annotations[0].AnchorDataPoint = chart10.Series[0].Points[0];
       //                 }
       //                 writer.Close();
       //             }

       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
       //     }

       // }

       // private void LatencyVsIOPSwithQD(bool multiSim)
       // {
       //     chart6.Series.Clear();

       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsIOPSwithQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("IOPS(Millions)" + "," + "Latency(us)" + "," + "QD");
       //                 if (multiSim)
       //                 {
       //                     if (comboBox33.Text == "Multi Sim based on QD")
       //                     {
       //                         int iopsIndex = 0;
       //                         TextAnnotation[] qd = { new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
       //                                      new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
       //                                   new TextAnnotation(), new TextAnnotation(), new TextAnnotation()
       //                                   };


       //                         chart6.Series.Add("0");
       //                         chart6.ChartAreas[0].AxisX.Interval = 0.5;
       //                         chart6.ChartAreas[0].AxisX.IntervalOffset = 0.5;
       //                         chart6.ChartAreas[0].AxisX.Minimum = 0;
       //                         chart6.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
       //                         chart6.Series[0].Color = Color.Blue;
       //                         chart6.Series[0].BorderWidth = 3;
       //                         chart6.Series[0].Name = "IOPS";
       //                         chart6.Annotations.Clear();
       //                         chart6.ChartAreas[0].AxisX.Maximum = Math.Ceiling(avgIOPS.Max());

       //                         for (int qIndex = 0; qIndex < validQueueDepth.Count(); qIndex++)
       //                         {
       //                             chart6.Series[0].Points.AddXY(avgIOPS[iopsIndex], avgLatency[qIndex]);
       //                             writer.WriteLine(avgIOPS[iopsIndex] + "," + avgLatency[qIndex] + "," + validQueueDepth[iopsIndex]);
       //                             chart6.Series[0].MarkerStyle = MarkerStyle.Diamond;
       //                             chart6.Series[0].MarkerSize = 10;
       //                             chart6.Series[0].MarkerColor = Color.Green;
       //                             qd[qIndex].Name = "qd_iops_" + qIndex.ToString() + validQueueDepth[iopsIndex].ToString();
       //                             qd[qIndex].Text = string.Concat("QD " + validQueueDepth[iopsIndex].ToString());
       //                             qd[qIndex].ForeColor = Color.Black;
       //                             qd[qIndex].Font = new Font("Arial", 10, FontStyle.Bold);
       //                             qd[qIndex].LineWidth = 1;
       //                             chart6.Annotations.Add(qd[qIndex]);
       //                             chart6.Annotations[qIndex].AxisX = chart6.ChartAreas[0].AxisX;
       //                             chart6.Annotations[qIndex].AxisY = chart6.ChartAreas[0].AxisY;
       //                             chart6.Annotations[qIndex].AnchorDataPoint = chart6.Series[0].Points[qIndex];
       //                             iopsIndex++;
       //                         }
       //                     }
       //                 }
       //                 else
       //                 {
       //                     chart6.Series.Add("0");
       //                     chart6.ChartAreas[0].AxisX.Interval = 0.5;
       //                     chart6.ChartAreas[0].AxisX.IntervalOffset = 0.5;
       //                     chart6.ChartAreas[0].AxisX.Minimum = 0;
       //                     //chart6.ChartAreas[0].AxisX.IntervalAutoMode = 
       //                     //chart6.ChartAreas[0].AxisX.Maximum = Math.Ceiling(iopsAverage) + 1;
       //                     chart6.Series[0].Points.AddXY(Math.Round(iopsAverage, 2), latencyAverage);
       //                     writer.WriteLine(Math.Round(iopsAverage, 2) + "," + latencyAverage + "," + depthOfQueue);
       //                     chart6.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
       //                     chart6.Series[0].Name = "IOPS";
       //                     chart6.Annotations.Clear();
       //                     TextAnnotation qd = new TextAnnotation();
       //                     qd.Name = "qd_iops";
       //                     qd.Text = string.Concat("QD " + depthOfQueue);
       //                     qd.ForeColor = Color.Black;
       //                     qd.Font = new Font("Arial", 10, FontStyle.Bold);
       //                     qd.LineWidth = 1;
       //                     chart6.Annotations.Add(qd);
       //                     chart6.Annotations[0].AxisX = chart6.ChartAreas[0].AxisX;
       //                     chart6.Annotations[0].AxisY = chart6.ChartAreas[0].AxisY;
       //                     chart6.Annotations[0].AnchorDataPoint = chart6.Series[0].Points[0];

       //                 }
       //                 writer.Close();
       //             }

       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
       //     }
       // }

       // private void LatencyVsIOPSwithBlockSize(bool multiSim)
       // {

       //     chart4.Series.Clear();
       //     //chart4.Titles.Clear();
       //     //chart4.Titles.Add("Latency Vs IOPS(with varying Block Size)");
       //     chart4.Titles[0].Text = "Latency Vs IOPS ( with varying Block Size )";
       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsIOPSwithBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("IOPS(Millions)" + "," + "Latency(us)" + "," + "BlockSize");
       //                 if (multiSim)
       //                 {
       //                     if (comboBox33.Text == "Multi Sim based on Block Size")
       //                     {
       //                         int iopsIndex = 0;
       //                         TextAnnotation[] qd = { new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
       //                                      new TextAnnotation(), new TextAnnotation()};

       //                         chart4.Series.Add("0");
       //                         chart4.ChartAreas[0].AxisX.Interval = 0.5;
       //                         chart4.ChartAreas[0].AxisX.IntervalOffset = 0.5;
       //                         chart4.ChartAreas[0].AxisX.Minimum = 0;
       //                         chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
       //                         chart4.Series[0].Color = Color.Blue;
       //                         chart4.Series[0].BorderWidth = 3;
       //                         chart4.Series[0].Name = "IOPS";
       //                         chart4.Annotations.Clear();


       //                         for (int qIndex = 0; qIndex < validBlockSize.Count(); qIndex++)
       //                         {
       //                             chart4.Series[0].Points.AddXY(Math.Round(avgIOPS[iopsIndex], 2), avgLatency[qIndex]);
       //                             writer.WriteLine(Math.Round(avgIOPS[iopsIndex], 2) + "," + avgLatency[qIndex] + "," + validBlockSize[iopsIndex]);
       //                             chart4.Series[0].MarkerStyle = MarkerStyle.Diamond;
       //                             chart4.Series[0].MarkerSize = 10;
       //                             chart4.Series[0].MarkerColor = Color.Green;
       //                             qd[qIndex].Name = "qd_iops_" + qIndex.ToString() + validBlockSize[iopsIndex].ToString();
       //                             qd[qIndex].Text = string.Concat("BlockSize " + validBlockSize[iopsIndex].ToString());
       //                             qd[qIndex].ForeColor = Color.Black;
       //                             qd[qIndex].Font = new Font("Arial", 10, FontStyle.Bold);
       //                             qd[qIndex].LineWidth = 1;
       //                             chart4.Annotations.Add(qd[qIndex]);
       //                             chart4.Annotations[qIndex].AxisX = chart4.ChartAreas[0].AxisX;
       //                             chart4.Annotations[qIndex].AxisY = chart4.ChartAreas[0].AxisY;
       //                             chart4.Annotations[qIndex].AnchorDataPoint = chart4.Series[0].Points[qIndex];
       //                             iopsIndex++;
       //                         }

       //                     }
       //                 }
       //                 else
       //                 {
       //                     chart4.Series.Add("0");
       //                     chart4.ChartAreas[0].AxisX.Interval = 0.5;
       //                     chart4.ChartAreas[0].AxisX.IntervalOffset = 0.5;
       //                     chart4.ChartAreas[0].AxisX.Minimum = 0;
       //                     chart4.Series[0].Points.AddXY(iopsAverage, latencyAverage);
       //                     if (enableWrkld == "1")
       //                     {
       //                         writer.WriteLine(iopsAverage + "," + latencyAverage + "," + wrkloadBS);
       //                     }
       //                     else
       //                     {
       //                         writer.WriteLine(iopsAverage + "," + latencyAverage + "," + blkSize);
       //                     }
       //                     chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
       //                     chart4.Series[0].Name = "IOPS";
       //                     chart4.Annotations.Clear();
       //                     TextAnnotation qd = new TextAnnotation();
       //                     qd.Name = "qd_iops";
       //                     if (enableWrkld == "1")
       //                     {
       //                         qd.Text = string.Concat("BlockSize " + blkSize);
       //                     }
       //                     else
       //                     {
       //                         qd.Text = string.Concat("BlockSize " + blkSize);
       //                     }
       //                     qd.ForeColor = Color.Black;
       //                     qd.Font = new Font("Arial", 10, FontStyle.Bold);
       //                     qd.LineWidth = 1;
       //                     chart4.Annotations.Add(qd);
       //                     chart4.Annotations[0].AxisX = chart4.ChartAreas[0].AxisX;
       //                     chart4.Annotations[0].AxisY = chart4.ChartAreas[0].AxisY;
       //                     chart4.Annotations[0].AnchorDataPoint = chart4.Series[0].Points[0];

       //                 }
       //                 writer.Close();
       //             }

       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
       //     }

       // }

       // private void LatencyVsIOPSwithIOSize(bool multiSim)
       // {

       //     chart4.Series.Clear();
       //     //chart4.Titles.Clear();
       //     //chart4.Titles.Add("Latency Vs IOPS(with varying IO Size)");
       //     chart4.Titles[0].Text = "Latency Vs IOPS ( with varying IO Size )";
       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsIOPSwithIOSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("IOPS(Millions)" + "," + "Latency(us)" + "," + "IOSize");
       //                 if (multiSim)
       //                 {
       //                     if (comboBox33.Text == "Multi Sim based on IO Size")
       //                     {
       //                         int iopsIndex = 0;
       //                         TextAnnotation[] qd = { new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
       //                                      new TextAnnotation(), new TextAnnotation()};

       //                         chart4.Series.Add("0");
       //                         chart4.ChartAreas[0].AxisX.Interval = 0.5;
       //                         chart4.ChartAreas[0].AxisX.IntervalOffset = 0.5;
       //                         chart4.ChartAreas[0].AxisX.Minimum = 0;
       //                         chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
       //                         chart4.Series[0].Color = Color.Blue;
       //                         chart4.Series[0].BorderWidth = 3;
       //                         chart4.Series[0].Name = "IOPS";
       //                         chart4.Annotations.Clear();


       //                         for (int qIndex = 0; qIndex < validIOSize.Count(); qIndex++)
       //                         {
       //                             chart4.Series[0].Points.AddXY(Math.Round(avgIOPS[iopsIndex], 2), avgLatency[qIndex]);
       //                             writer.WriteLine(Math.Round(avgIOPS[iopsIndex], 2) + "," + avgLatency[qIndex] + "," + validIOSize[iopsIndex]);
       //                             chart4.Series[0].MarkerStyle = MarkerStyle.Diamond;
       //                             chart4.Series[0].MarkerSize = 10;
       //                             chart4.Series[0].MarkerColor = Color.Green;
       //                             qd[qIndex].Name = "qd_iops_" + qIndex.ToString() + validIOSize[iopsIndex].ToString();
       //                             qd[qIndex].Text = string.Concat("IOSize " + validIOSize[iopsIndex].ToString());
       //                             qd[qIndex].ForeColor = Color.Black;
       //                             qd[qIndex].Font = new Font("Arial", 10, FontStyle.Bold);
       //                             qd[qIndex].LineWidth = 1;
       //                             chart4.Annotations.Add(qd[qIndex]);
       //                             chart4.Annotations[qIndex].AxisX = chart4.ChartAreas[0].AxisX;
       //                             chart4.Annotations[qIndex].AxisY = chart4.ChartAreas[0].AxisY;
       //                             chart4.Annotations[qIndex].AnchorDataPoint = chart4.Series[0].Points[qIndex];
       //                             iopsIndex++;
       //                         }

       //                     }
       //                 }
       //                 else
       //                 {
       //                     chart4.Series.Add("0");
       //                     chart4.ChartAreas[0].AxisX.Interval = 0.5;
       //                     chart4.ChartAreas[0].AxisX.IntervalOffset = 0.5;
       //                     chart4.ChartAreas[0].AxisX.Minimum = 0;
       //                     chart4.Series[0].Points.AddXY(iopsAverage, latencyAverage);
       //                     if (enableWrkld == "1")
       //                     {
       //                         writer.WriteLine(iopsAverage + "," + latencyAverage + "," + wrkloadBS);
       //                     }
       //                     else
       //                     {
       //                         writer.WriteLine(iopsAverage + "," + latencyAverage + "," + ioSize);
       //                     }
       //                     chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
       //                     chart4.Series[0].Name = "IOPS";
       //                     chart4.Annotations.Clear();
       //                     TextAnnotation qd = new TextAnnotation();
       //                     qd.Name = "qd_iops";
       //                     if (enableWrkld == "1")
       //                     {
       //                         qd.Text = string.Concat("IOSize " + wrkloadBS);
       //                     }
       //                     else
       //                     {
       //                         qd.Text = string.Concat("IOSize " + ioSize);
       //                     }
       //                     qd.ForeColor = Color.Black;
       //                     qd.Font = new Font("Arial", 10, FontStyle.Bold);
       //                     qd.LineWidth = 1;
       //                     chart4.Annotations.Add(qd);
       //                     chart4.Annotations[0].AxisX = chart4.ChartAreas[0].AxisX;
       //                     chart4.Annotations[0].AxisY = chart4.ChartAreas[0].AxisY;
       //                     chart4.Annotations[0].AnchorDataPoint = chart4.Series[0].Points[0];

       //                 }
       //                 writer.Close();
       //             }

       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
       //     }

       // }

       // private void CommandVsBlockSize(bool multiSim)
       // {
       //     //chart11.Update();
       //     //chart11.ChartAreas.Clear();
       //     chart11.Series.Clear();
       //     //chart11.Titles.Clear();
       //     chart11.Series.Add("Number Of Commands");
       //     //chart11.Titles.Add("Command Count Vs Block Size");
       //     chart11.Titles[0].Text = "Command Count Vs Block Size";

       //     //chart11.ChartAreas.Add("0");
       //     chart11.ChartAreas[0].AxisX.Title = "Block Size";
       //     chart11.ChartAreas[0].AxisY.Title = "Number Of Commands";
       //     chart11.ChartAreas[0].AxisX.Minimum = 0;
       //     chart11.ChartAreas[0].AxisX.Maximum = 8704;
       //     chart11.ChartAreas[0].AxisX.Interval = 1024;
       //     chart11.ChartAreas[0].AxisY.Minimum = 0;
       //     chart11.ChartAreas[0].AxisY.Maximum = Int64.Parse(numCommands) + 1;

       //     chart11.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
       //     chart11.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 20";
       //     chart11.Series[0].Color = Color.Blue;
       //     chart11.Series[0].Name = "Number Of Commands";
       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\CmdVsBlockSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("IOSize" + "," + "Number of Commands");
       //                 if (multiSim)
       //                 {
       //                     if (comboBox33.Text == "Multi Sim based on Block Size")
       //                     {
       //                         foreach (string io in validBlockSize)
       //                         {
       //                             chart11.Series[0].Points.AddXY(int.Parse(io), Int64.Parse(numCommands));
       //                             writer.WriteLine(int.Parse(io) + "," + Int64.Parse(numCommands));
       //                         }
       //                     }
       //                     else if (comboBox33.Text == "Multi Sim based on QD")
       //                     {
       //                         if (enableWrkld == "1")
       //                         {
       //                             chart11.Series[0].Points.AddXY(int.Parse(blkSize), Int64.Parse(numCommands));
       //                             writer.WriteLine(int.Parse(wrkloadBS) + "," + Int64.Parse(numCommands));
       //                         }
       //                         else
       //                         {
       //                             chart11.Series[0].Points.AddXY(int.Parse(blkSize), Int64.Parse(numCommands));
       //                             writer.WriteLine(int.Parse(ioSize) + "," + Int64.Parse(numCommands));
       //                         }
       //                     }
       //                 }
       //                 else
       //                 {
       //                     //chart11.ChartAreas[0].AxisX.CustomLabels.Add(ioIndex - 1.0, ioIndex + 1.0);
       //                     if (enableWrkld == "1")
       //                     {
       //                         chart11.Series[0].Points.AddXY(int.Parse(blkSize), Int64.Parse(numCommands));
       //                         writer.WriteLine(int.Parse(wrkloadBS) + "," + Int64.Parse(numCommands));
       //                     }
       //                     else
       //                     {
       //                         chart11.Series[0].Points.AddXY(int.Parse(blkSize), Int64.Parse(numCommands));
       //                         writer.WriteLine(int.Parse(ioSize) + "," + Int64.Parse(numCommands));
       //                     }
       //                 }
       //                 writer.Close();
       //             }

       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsBlocksize", e.Message), e);
       //     }
       // }

       // private void CommandVsIOSize(bool multiSim)
       // {
       //     //chart11.Update();
       //     //chart11.ChartAreas.Clear();
       //     chart11.Series.Clear();
       //     //chart11.Titles.Clear();
       //     chart11.Series.Add("Number Of Commands");
       //     //chart11.Titles.Add("Command Count Vs IO Size");
       //     chart11.Titles[0].Text = "Command Count Vs IO Size";
       //     //chart11.ChartAreas.Add("0");
       //     chart11.ChartAreas[0].AxisX.Title = "IO Size";
       //     chart11.ChartAreas[0].AxisY.Title = "Number Of Commands";
       //     chart11.ChartAreas[0].AxisX.Minimum = 0;
       //     chart11.ChartAreas[0].AxisX.Maximum = 8704;
       //     chart11.ChartAreas[0].AxisX.Interval = 1024;
       //     chart11.ChartAreas[0].AxisY.Minimum = 0;
       //     chart11.ChartAreas[0].AxisY.Maximum = Int64.Parse(numCommands) + 1;

       //     chart11.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
       //     chart11.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 20";
       //     chart11.Series[0].Color = Color.Blue;
       //     chart11.Series[0].Name = "Number Of Commands";
       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\CmdVsIOSize.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("IOSize" + "," + "Number of Commands");
       //                 if (multiSim)
       //                 {
       //                     if (comboBox33.Text == "Multi Sim based on IO Size")
       //                     {
       //                         foreach (string io in validIOSize)
       //                         {
       //                             chart11.Series[0].Points.AddXY(int.Parse(io), Int64.Parse(numCommands));
       //                             writer.WriteLine(int.Parse(io) + "," + Int64.Parse(numCommands));
       //                         }
       //                     }
       //                     else if (comboBox33.Text == "Multi Sim based on QD")
       //                     {
       //                         if (enableWrkld == "1")
       //                         {
       //                             chart11.Series[0].Points.AddXY(int.Parse(wrkloadBS), Int64.Parse(numCommands));
       //                             writer.WriteLine(int.Parse(wrkloadBS) + "," + Int64.Parse(numCommands));
       //                         }
       //                         else
       //                         {
       //                             chart11.Series[0].Points.AddXY(int.Parse(ioSize), Int64.Parse(numCommands));
       //                             writer.WriteLine(int.Parse(ioSize) + "," + Int64.Parse(numCommands));
       //                         }
       //                     }
       //                 }
       //                 else
       //                 {
       //                     //chart11.ChartAreas[0].AxisX.CustomLabels.Add(ioIndex - 1.0, ioIndex + 1.0);
       //                     if (enableWrkld == "1")
       //                     {
       //                         chart11.Series[0].Points.AddXY(int.Parse(wrkloadBS), Int64.Parse(numCommands));
       //                         writer.WriteLine(int.Parse(wrkloadBS) + "," + Int64.Parse(numCommands));
       //                     }
       //                     else
       //                     {
       //                         chart11.Series[0].Points.AddXY(int.Parse(ioSize), Int64.Parse(numCommands));
       //                         writer.WriteLine(int.Parse(ioSize) + "," + Int64.Parse(numCommands));
       //                     }
       //                 }
       //                 writer.Close();
       //             }

       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsBlocksize", e.Message), e);
       //     }
       // }
       // private void plotCmdVsLBA(string fileName)
       // {
       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\CmdVsLBA.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
       //         {
       //             using (StreamWriter writer = new StreamWriter(file))
       //             {
       //                 writer.WriteLine("LBA" + "," + "Number of Commands");
       //                 try
       //                 {
       //                     using (FileStream file2 = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //                     {
       //                         using (StreamReader reader = new StreamReader(file2))
       //                         {

       //                             List<int> lba = new List<int>();
       //                             List<double> time = new List<double>();
       //                             List<string> range = new List<string>();

       //                             Hashtable hashtable = new Hashtable();

       //                             while (reader.Peek() != -1)
       //                             {
       //                                 string line = reader.ReadLine();
       //                                 string[] tokens = line.Split(' ');

       //                                 time.Add(double.Parse(tokens[0]));
       //                                 lba.Add(int.Parse(tokens[1]));
       //                                 writer.WriteLine(int.Parse(tokens[1]) + "," + 1);
       //                             }

       //                             reader.Close();

       //                             int count = 0;
       //                             int binRange = 0;
       //                             int lbaMax = lba.Max();

       //                             if (lba.Max() < 1024)
       //                             {
       //                                 binRange = 1;
       //                                 chart8.Series.Add(new Series());
       //                                 int hashIndex = 0;
       //                                 int value = 0;
       //                                 for (int index = 0; index < lba.Count(); index++)
       //                                 {
       //                                     hashIndex = lba.ElementAt(index);
       //                                     if (hashtable.ContainsKey(hashIndex))
       //                                     {
       //                                         value = (int)hashtable[hashIndex];
       //                                         value++;
       //                                         hashtable[hashIndex] = value;
       //                                     }
       //                                     else
       //                                     {
       //                                         hashtable.Add(hashIndex, 1);

       //                                     }
       //                                 }
       //                                 chart8.Series.Clear();
       //                                 chart8.Series.Add(new Series());
       //                                 chart8.Series[0].IsVisibleInLegend = false;
       //                                 chart8.ChartAreas[0].AxisX.Title = "LBA";
       //                                 chart8.ChartAreas[0].AxisY.Title = "Number Of Commands";
       //                                 chart8.ChartAreas[0].AxisY.Minimum = 0;
       //                                 chart8.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
       //                                 chart8.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
       //                                 chart8.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 5";
       //                                 ICollection key = hashtable.Keys;
       //                                 chart8.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
       //                                 chart8.ChartAreas[0].AxisX.CustomLabels.Clear();
       //                                 foreach (Int32 k in key)
       //                                 {
       //                                     chart8.Series[0].Points.AddXY(k, hashtable[k]);
       //                                     // writer.WriteLine(k + "," + hashtable[k]);
       //                                 }
       //                             }
       //                             else
       //                             {
       //                                 binRange = 1024;
       //                                 count = lba.Max() / binRange;
       //                                 if (lba.Max() % binRange != 0)
       //                                 {
       //                                     count++;
       //                                 }
       //                                 for (int index = 0; index < count; index++)
       //                                 {
       //                                     hashtable.Add(binRange * (index + 1), 0);

       //                                 }
       //                                 double hashIndex = 0;
       //                                 int value = 0;
       //                                 for (int lbaIndex = 0; lbaIndex < lba.Count(); lbaIndex++)
       //                                 {
       //                                     hashIndex = lba.ElementAt(lbaIndex) / binRange;
       //                                     hashIndex = Math.Floor(hashIndex);
       //                                     if (hashIndex == 0)
       //                                     {
       //                                         value = (int)hashtable[binRange];
       //                                         value++;
       //                                         hashtable[binRange] = value;
       //                                     }
       //                                     else
       //                                     {
       //                                         value = (int)hashtable[binRange * (int)(hashIndex)];
       //                                         value++;
       //                                         hashtable[binRange * (int)(hashIndex)] = value;
       //                                     }
       //                                 }

       //                                 chart8.Update();
       //                                 chart8.Series.Clear();
       //                                 chart8.ChartAreas[0].AxisX.Title = "LBA";
       //                                 chart8.ChartAreas[0].AxisY.Title = "Number Of Commands";
       //                                 chart8.ChartAreas[0].AxisX.Minimum = 0;
       //                                 chart8.ChartAreas[0].AxisY.Minimum = 0;
       //                                 chart8.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
       //                                 chart8.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

       //                                 int rangeIndex = 0;

       //                                 ICollection key = hashtable.Keys;

       //                                 rangeIndex = 0;
       //                                 Int32 upperIndex = 0;
       //                                 Int32 lowerIndex = 0;
       //                                 chart8.ChartAreas[0].AxisX.CustomLabels.Clear();
       //                                 range.Clear();

       //                                 foreach (Int32 k in key)
       //                                 {
       //                                     if ((int)hashtable[k] != 0)
       //                                     {
       //                                         if (k <= binRange)
       //                                         {
       //                                             lowerIndex = 0;
       //                                             upperIndex = 1;
       //                                         }
       //                                         else
       //                                         {
       //                                             lowerIndex = k / binRange;
       //                                             upperIndex = (k + binRange) / binRange;
       //                                         }
       //                                         range.Add(String.Concat(lowerIndex.ToString() + "K" + "-" + upperIndex.ToString() + "K"));
       //                                         chart8.ChartAreas[0].AxisX.CustomLabels.Add(k / binRange - 1, k / binRange + 1, range[rangeIndex]);
       //                                         chart8.Series.Add(range[rangeIndex]);
       //                                         chart8.Series[range[rangeIndex]].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
       //                                         chart8.Series[range[rangeIndex]].Color = Color.Green;
       //                                         chart8.Series[range[rangeIndex]].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
       //                                         chart8.Series[range[rangeIndex]].Points.AddXY(k / binRange, hashtable[k]);
       //                                         //   writer.WriteLine(k / binRange + "," + hashtable[k]);
       //                                         chart8.Series[range[rangeIndex]].IsVisibleInLegend = false;
       //                                         chart8.Series[range[rangeIndex]].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 20";
       //                                         rangeIndex++;
       //                                     }
       //                                 }
       //                             }
       //                             file2.Close();
       //                         }
       //                     }
       //                 }

       //                 catch (IOException e)
       //                 {
       //                     throw new Exception(String.Format("An error ocurred while executing the data import: Transaction Report", e.Message), e);
       //                 }
       //                 writer.Close();
       //             }

       //             file.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error ocurred while executing the data import: LatencyVsIOsize", e.Message), e);
       //     }
       // }

       // private void CommandVsLBA(bool multiSim)
       // {

       //     string fileName = "";
       //     try
       //     {
       //         if (multiSim)
       //         {
       //             string param = validParam.Trim();
       //             if (comboBox33.Text == "Multi Sim based on Block Size")
       //             {
       //                 int qIndex = 0;
       //                 switch (param)
       //                 {
       //                     case "512":
       //                         qIndex = validQDepth.Count() - 1;
       //                         break;
       //                     case "1024":
       //                         qIndex = validQDepth.Count() - 2;
       //                         break;
       //                     case "2048":
       //                         {
       //                             qIndex = validQDepth.Count() - 3;
       //                             break;
       //                         }
       //                     case "4096":
       //                         {
       //                             qIndex = validQDepth.Count() - 4;
       //                             break;
       //                         }
       //                     case "8192":
       //                         {
       //                             qIndex = validQDepth.Count() - 5;
       //                             break;
       //                         }
       //                 }
       //                 if (enableWrkld == "1")
       //                 {
       //                     fileName = ".\\Reports_PCIe\\lba_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //                 }
       //                 else
       //                 {
       //                     fileName = ".\\Reports_PCIe\\lba_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //                 }
       //             }
       //             else if (comboBox33.Text == "Multi Sim based on QD")
       //             {
       //                 if (enableWrkld == "1")
       //                 {
       //                     fileName = ".\\Reports_PCIe\\lba_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + param + ".log";
       //                 }
       //                 else
       //                 {
       //                     fileName = ".\\Reports_PCIe\\lba_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + param + ".log";
       //                 }
       //             }
       //             else
       //             {
       //                 int qIndex = 0;
       //                 switch (param)
       //                 {
       //                     case "512":
       //                         qIndex = validQDepth.Count() - 1;
       //                         break;
       //                     case "1024":
       //                         qIndex = validQDepth.Count() - 2;
       //                         break;
       //                     case "2048":
       //                         {
       //                             qIndex = validQDepth.Count() - 3;
       //                             break;
       //                         }
       //                     case "4096":
       //                         {
       //                             qIndex = validQDepth.Count() - 4;
       //                             break;
       //                         }
       //                     case "8192":
       //                         {
       //                             qIndex = validQDepth.Count() - 5;
       //                             break;
       //                         }
       //                 }
       //                 fileName = ".\\Reports_PCIe\\lba_report_" + "iosize_" + param + "_" + "blksize_" + validBlockSize[0] + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";

       //             }

       //         }
       //         else
       //         {
       //             fileName = ".\\Reports_PCIe\\lba_report.log";

       //         }
       //         plotCmdVsLBA(fileName);
       //     }

       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error occurred while executing the data import: LBA Report", e.Message), e);
       //     }
       // }

       // private void channelUtilizationData()
       // {
       //     chart2.Update();
       //     if (chart2.Series.Count != 0)
       //         chart2.Series.Clear();
       //     chart2.Series.Add(new Series());
       //     chart2.Series[0].IsVisibleInLegend = false;
       //     chart2.ChartAreas[0].AxisX.Title = "Channels";
       //     chart2.ChartAreas[0].AxisY.Title = "No. Of References (CodeWord)";
       //     chart2.ChartAreas[0].AxisY.Maximum = chanTransCount.Max();
       //     int chanLabel = 0;
       //     for (int chanIndex = 1; chanIndex < (int.Parse(numChannel) + 1); chanIndex++)
       //     {
       //         chanLabel = chanIndex - 1;
       //         chart2.ChartAreas[0].AxisX.CustomLabels.Add(chanIndex - 1.0, chanIndex + 1.0, chanLabel.ToString());
       //         chart2.Series[0].Points.AddY(chanTransCount.ElementAt((chanIndex - 1)));
       //         chart2.Series[0].CustomProperties = "DrawingStyle = Cylinder,PixelPointWidth = 20";
       //     }
       // }

       // private void shortQueueUtilizationData()
       // {
       //     try
       //     {

       //         using (FileStream file = new FileStream(@".\Reports_PCIe\shortQueue_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //         {
       //             using (StreamReader reader = new StreamReader(file))
       //             {
       //                 if (reader.Peek() != -1)
       //                 {

       //                     List<int> channel = new List<int>();
       //                     List<double> time = new List<double>();
       //                     List<Int16> queueSize = new List<Int16>();

       //                     while (reader.Peek() != -1)
       //                     {
       //                         string line = reader.ReadLine();
       //                         string[] tokens = line.Split(' ');

       //                         time.Add(double.Parse(tokens[0]));
       //                         channel.Add(int.Parse(tokens[1]));
       //                         queueSize.Add(Int16.Parse(tokens[2]));
       //                     }

       //                     reader.Close();
       //                     if (queueSize.Count != 0)
       //                     {
       //                         int last = time.Count - 1;
       //                         int first = 0;
       //                         chart3.Update();

       //                         if (timeScale.ToString() == "us")
       //                         {
       //                             timeScaleVar = 1000;
       //                             chart3.ChartAreas[0].AxisX.Minimum = (time[first] / timeScaleVar);
       //                             chart3.ChartAreas[0].AxisX.Maximum = (time[last] / timeScaleVar);
       //                             chart3.ChartAreas[0].AxisX.Title = "Time (us)";
       //                         }
       //                         else if (timeScale.ToString() == "ms")
       //                         {
       //                             timeScaleVar = 1000000;
       //                             chart3.ChartAreas[0].AxisX.Minimum = (time[first] / timeScaleVar);
       //                             chart3.ChartAreas[0].AxisX.Maximum = (time[last] / timeScaleVar);
       //                             chart3.ChartAreas[0].AxisX.Title = "Time (ms)";
       //                         }
       //                         chart3.ChartAreas[0].AxisY.Title = "Queue Size";
       //                         chart3.ChartAreas[0].AxisY.Maximum = cwNum;
       //                         if (chart3.Series.Count != 0)
       //                             chart3.Series.Clear();

       //                         if (chart3.Legends.Count != 0)
       //                             chart3.Legends.Clear();
       //                         chart3.Legends.Add("Channels");
       //                         chart3.Legends["Channels"].Title = "Channels";

       //                         for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
       //                         {
       //                             chart3.Series.Add(chanIndex.ToString());
       //                             chart3.Series[chanIndex.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
       //                         }

       //                         for (int index = 0; index < time.Count; index++)
       //                         {
       //                             chart3.Series[channel.ElementAt(index).ToString()].Points.AddXY(time[index] / timeScaleVar, queueSize.ElementAt(index));
       //                         }
       //                     }
       //                 }

       //             }
       //             file.Close();
       //         }

       //     }

       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error occurred while executing the data import: shortQueue Report", e.Message), e);
       //     }
       // }

       // private void longQueueUtilizationData()
       // {

       //     try
       //     {
       //         using (FileStream file = new FileStream(@".\Reports_PCIe\longQueue_report.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //         {
       //             using (StreamReader reader = new StreamReader(file))
       //             {

       //                 List<int> channel = new List<int>();
       //                 List<double> time = new List<double>();
       //                 List<Int16> queueSize = new List<Int16>();

       //                 if (reader.Peek() != -1)
       //                 {
       //                     while (reader.Peek() != -1)
       //                     {
       //                         string line = reader.ReadLine();
       //                         string[] tokens = line.Split(' ');

       //                         time.Add(double.Parse(tokens[0]));
       //                         channel.Add(int.Parse(tokens[1]));
       //                         queueSize.Add(Int16.Parse(tokens[2]));
       //                     }

       //                     reader.Close();
       //                     int last = time.Count - 1;
       //                     int first = 0;
       //                     chart5.Update();

       //                     if (timeScale.ToString() == "us")
       //                     {
       //                         timeScaleVar = 1000;
       //                         chart5.ChartAreas[0].AxisX.Minimum = (time[first] / timeScaleVar);
       //                         chart5.ChartAreas[0].AxisX.Maximum = (time[last] / timeScaleVar);
       //                         chart5.ChartAreas[0].AxisX.Title = "Time (us)";
       //                     }
       //                     else if (timeScale.ToString() == "ms")
       //                     {
       //                         timeScaleVar = 1000000;
       //                         chart5.ChartAreas[0].AxisX.Minimum = (time[first] / timeScaleVar);
       //                         chart5.ChartAreas[0].AxisX.Maximum = (time[last] / timeScaleVar);
       //                         chart5.ChartAreas[0].AxisX.Title = "Time (ms)";
       //                     }
       //                     chart5.ChartAreas[0].AxisY.Title = "Queue Size";
       //                     chart5.ChartAreas[0].AxisY.Maximum = cwNum;// double.Parse(queueDepth);

       //                     if (chart5.Series.Count != 0)
       //                         chart5.Series.Clear();

       //                     if (chart5.Legends.Count != 0)
       //                         chart5.Legends.Clear();
       //                     chart5.Legends.Add("Channels");
       //                     chart5.Legends["Channels"].Title = "Channels";

       //                     for (int chanIndex = 0; chanIndex < int.Parse(numChannel); chanIndex++)
       //                     {
       //                         chart5.Series.Add(chanIndex.ToString());
       //                         chart5.Series[chanIndex.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
       //                     }

       //                     for (int index = 0; index < time.Count; index++)
       //                     {
       //                         chart5.Series[channel.ElementAt(index).ToString()].Points.AddXY(time[index] / timeScaleVar, queueSize.ElementAt(index));
       //                     }

       //                 }
       //             }
       //             file.Close();
       //         }
       //     }

       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error occurred while executing the data import: Long Queue Utilization Report", e.Message), e);
       //     }
       // }
       // double simTime = 0;
       // private void plotDDR(string fileName)
       // {

       //     if (chart9.Series.Count != 0)
       //         chart9.Series.Clear();
       //     List<double> busUtil = new List<double>();
       //     List<double> time = new List<double>();
       //     try
       //     {
       //         using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //         {
       //             parseDDRBusUtilStream(busUtil, time, file);
       //             List<double> busUtilPercent = new List<double>();
       //             double avgBusUtil = 0;
       //             double effectiveBusUtil = 0;
       //             double totalBusTime = 0;
       //             double effectiveBusTime = 0;
       //             double commandTime = (double)8000 / (double)(2 * int.Parse(ddrSpeed));// (double)(2 * COMMAND_BL * 1000) / (double)(2 * mDdrSpeed);
       //             foreach (double t in busUtil)
       //             {
       //                 totalBusTime += t;
       //                 if (t > commandTime)
       //                 {
       //                     effectiveBusTime += t;
       //                 }
       //             }
       //             avgBusUtil = (double)((double)totalBusTime / time.Max()) * 100;
       //             effectiveBusUtil = (double)((double)effectiveBusTime / time.Max()) * 100;



       //             chart9.Series.Add(new Series("DATA"));
       //             chart9.Series[0].IsVisibleInLegend = false;
       //             chart9.Series[0].ChartType = SeriesChartType.FastPoint;
       //             chart9.Series[0].Color = Color.Brown;

       //             chart9.Series.Add(new Series("CMD"));
       //             chart9.Series[1].IsVisibleInLegend = false;
       //             chart9.Series[1].ChartType = SeriesChartType.FastPoint;
       //             chart9.Series[1].Color = Color.Gray;


       //             setDDRChartLegend(avgBusUtil, effectiveBusUtil);



       //             int chartSize = (int)(time.Max() + busUtil.ElementAt(busUtil.Count() - 1));
       //             int numCmd = int.Parse(numCommands.Trim());
       //             if (numCmd > 3)
       //             {

       //                 chartSize /= 2;
       //                 if (chartSize < 1068)
       //                 {
       //                     chartSize = 1068;
       //                 }
       //                 chart9.Width = chartSize;

       //             }
       //             else
       //             {
       //                 chart9.Width = 1068;
       //             }
       //             chart9.ChartAreas[0].Position.Y = 10;
       //             chart9.ChartAreas[0].Position.Height = 80;
       //             chart9.ChartAreas[0].Position.X = 1;
       //             chart9.ChartAreas[0].Position.Width = 99;

       //             chart9.ChartAreas[0].AxisY.LabelStyle.IsEndLabelVisible = false;
       //             chart9.ChartAreas[0].AxisY.CustomLabels.Clear();
       //             chart9.ChartAreas[0].AxisY.CustomLabels.Add(int.Parse(numChannel) + 39, int.Parse(numChannel) + 42, "DDR");
       //             //chart9.ChartAreas[0].AxisX.IntervalAutoMode = false;
       //             //chart9.ChartAreas[0].AxisX.Interval = 1;


       //             lockchart9Position();

       //             chart9.ChartAreas[0].AxisY.Minimum = 0;
       //             chart9.ChartAreas[0].AxisX.Minimum = 0;
       //             chart9.ChartAreas[0].AxisX.Maximum = time.Max() + busUtil.ElementAt(busUtil.Count() - 1);
       //             simTime = time.Max();

       //             for (int timeIndex = 0; timeIndex < time.Count(); timeIndex++)
       //             {
       //                 double timeAxis = time.ElementAt(timeIndex);

       //                 for (int busIndex = 0; busIndex < busUtil.ElementAt(timeIndex); busIndex++)
       //                 {
       //                     if (busUtil.ElementAt(timeIndex) > 5)
       //                     {
       //                         chart9.Series["DATA"].Points.AddXY(timeAxis, int.Parse(numChannel) + 40);
       //                     }
       //                     else
       //                     {
       //                         chart9.Series["CMD"].Points.AddXY(timeAxis, int.Parse(numChannel) + 40);
       //                     }
       //                     timeAxis++;
       //                 }

       //             }

       //             //}
       //             file.Close();
       //         }
       //     }

       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error occurred while executing the data import: DDR Utilization Report", e.Message), e);
       //     }
       // }

       // private void lockchart9Position()
       // {
       //     ChartArea ca = chart9.ChartAreas[0];
       //     ElementPosition cap = ca.Position;
       //     ElementPosition ipp = ca.InnerPlotPosition;
       //     ipp.Width = 70;
       //     ipp.Height = 80;

       //     int ippX = 80;//inner plot X axis pixel position                          
       //     int caX = 60;//chartArea X axis pixel position
       //     //convert pixel to percentage
       //     int capWidth = getChartAreaWidthPercent(cap);
       //     float newIppX = getXAxisLocation(ippX, capWidth);
       //     float newCaX = getXAxisLocation(caX, chart9.ClientSize.Width);
       //     ipp.X = newIppX;
       //     ca.Position = new ElementPosition(newCaX, cap.Y, cap.Width, cap.Height);
       // }

       // private static float getXAxisLocation(int numerator, int denominator)
       // {
       //     return (float)(numerator * 100) / denominator;
       // }

       // private int getChartAreaWidthPercent(ElementPosition cap)
       // {
       //     return (int)((cap.Width * (float)chart9.ClientSize.Width) / 100f);
       // }

       // private void setDDRChartLegend(double avgBusUtil, double effectiveBusUtil)
       // {
       //     chart9.Legends.Clear();
       //     chart9.Legends.Add(new Legend());
       //     chart9.Legends[0].CustomItems.Add(new LegendItem());
       //     chart9.Legends[0].CustomItems[0].Cells.Add(new LegendCell("  "));
       //     chart9.Legends[0].CustomItems[0].Cells.Add(new LegendCell("Actual"));
       //     chart9.Legends[0].CustomItems[0].Cells.Add(new LegendCell("Effective"));
       //     chart9.Legends[0].CustomItems.Add(new LegendItem());
       //     chart9.Legends[0].CustomItems[1].Cells.Add(new LegendCell("DDR"));
       //     chart9.Legends[0].CustomItems[1].Cells[0].Font = new Font("Arial", 8, System.Drawing.FontStyle.Bold);
       //     chart9.Legends[0].CustomItems[1].Cells.Add(new LegendCell(Math.Round(avgBusUtil, 2).ToString()));
       //     chart9.Legends[0].CustomItems[1].Cells.Add(new LegendCell(Math.Round(effectiveBusUtil, 2).ToString()));
       //     chart9.Legends[0].CustomItems.Add(new LegendItem());
       //     chart9.Legends[0].CustomItems[2].Cells.Add(new LegendCell(""));
       // }

       // private static void parseDDRBusUtilStream(List<double> busUtil, List<double> time, FileStream file)
       // {
       //     try
       //     {
       //         using (StreamReader reader = new StreamReader(file))
       //         {

       //             while (reader.Peek() != -1)
       //             {
       //                 string line = reader.ReadLine();
       //                 string[] tokens = line.Split(' ');

       //                 time.Add(double.Parse(tokens[0]));
       //                 busUtil.Add(double.Parse(tokens[1]));
       //             }
       //             reader.Close();
       //         }
       //     }
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error occurred while executing the stream reading: DDR Utilization Report", e.Message), e);
       //     }
       // }

       // private void DDRBusUtilizationData(bool multiSim)
       // {
       //     string fileName = "";
       //     if (multiSim)
       //     {

       //         // chart9.Series.Clear();
       //         string param = validMultiSimParam.Trim();
       //         if (comboBox33.Text == "Multi Sim based on Block Size")
       //         {
       //             int qIndex = 0;
       //             switch (param)
       //             {
       //                 case "512":
       //                     qIndex = validQDepth.Count() - 1;
       //                     break;
       //                 case "1024":
       //                     qIndex = validQDepth.Count() - 2;
       //                     break;
       //                 case "2048":
       //                     {
       //                         qIndex = validQDepth.Count() - 3;
       //                         break;
       //                     }
       //                 case "4096":
       //                     {
       //                         qIndex = validQDepth.Count() - 4;
       //                         break;
       //                     }
       //                 case "8192":
       //                     {
       //                         qIndex = validQDepth.Count() - 5;
       //                         break;
       //                     }
       //             }
       //             if (enableWrkld == "1")
       //             {
       //                 fileName = ".\\Reports_PCIe\\DDR_bus_utilization_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + param.Trim() + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //             }
       //             else
       //             {
       //                 fileName = ".\\Reports_PCIe\\DDR_bus_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + param.Trim() + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //             }
       //         }
       //         else if (comboBox33.Text == "Multi Sim based on QD")
       //         {
       //             if (enableWrkld == "1")
       //             {
       //                 fileName = ".\\Reports_PCIe\\DDR_bus_utilization_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.ToString().Trim() + "_" + "qd_" + param.Trim() + ".log";
       //             }
       //             else
       //             {
       //                 fileName = ".\\Reports_PCIe\\DDR_bus_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.ToString().Trim() + "_" + "qd_" + param.Trim() + ".log";
       //             }
       //         }
       //         else
       //         {
       //             int qIndex = 0;
       //             switch (param)
       //             {
       //                 case "512":
       //                     qIndex = validQDepth.Count() - 1;
       //                     break;
       //                 case "1024":
       //                     qIndex = validQDepth.Count() - 2;
       //                     break;
       //                 case "2048":
       //                     {
       //                         qIndex = validQDepth.Count() - 3;
       //                         break;
       //                     }
       //                 case "4096":
       //                     {
       //                         qIndex = validQDepth.Count() - 4;
       //                         break;
       //                     }
       //                 case "8192":
       //                     {
       //                         qIndex = validQDepth.Count() - 5;
       //                         break;
       //                     }
       //             }
       //             fileName = ".\\Reports_PCIe\\DDR_bus_utilization_" + "iosize_" + param + "_" + "blksize_" + validBlockSize[0] + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //         }
       //     }
       //     else
       //     {
       //         fileName = ".\\Reports_PCIe\\DDR_bus_utilization.log";
       //     }
       //     plotDDR(fileName);
       // }

       // private void plotONFI(string fileName1, string fileName2)
       // {
       //     try
       //     {
       //         List<double> startTime = new List<double>();
       //         List<int> channelNum = new List<int>();
       //         List<double> endTime = new List<double>();
       //         List<double>[] busUtilPerChannel = new List<double>[int.Parse(numChannel)];
       //         List<double>[] timePerChannel = new List<double>[int.Parse(numChannel)];
       //         // Hashtable timeWindowValue = new Hashtable();

       //         Hashtable totalTime = new Hashtable();
       //         using (FileStream file1 = new FileStream(fileName1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //         {
       //             using (StreamReader reader1 = new StreamReader(file1))
       //             {
       //                 while (reader1.Peek() != -1)
       //                 {
       //                     string line = reader1.ReadLine();
       //                     string[] tokens = line.Split(' ');
       //                     channelNum.Add(int.Parse(tokens[0]));
       //                     startTime.Add(double.Parse(tokens[1]));
       //                     endTime.Add(double.Parse(tokens[2]));
       //                 }
       //                 reader1.Close();
       //                 for (int chanIndex = 0; chanIndex < channelNum.Count(); chanIndex++)
       //                 {
       //                     busUtilPerChannel[channelNum[chanIndex]] = new List<double>();
       //                     timePerChannel[channelNum[chanIndex]] = new List<double>();
       //                 }

       //             }
       //             file1.Close();
       //         }

       //         using (FileStream file = new FileStream(fileName2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
       //         {


       //             using (StreamReader reader = new StreamReader(file))
       //             {
       //                 List<double> busUtil = new List<double>();
       //                 List<double> time = new List<double>();
       //                 List<int> channel = new List<int>();
       //                 List<double> avgONFIUtil = new List<double>();
       //                 List<double> effectiveUtil = new List<double>();
       //                 while (reader.Peek() != -1)
       //                 {
       //                     string line = reader.ReadLine();
       //                     string[] tokens = line.Split(' ');

       //                     time.Add(double.Parse(tokens[0]));
       //                     channel.Add(int.Parse(tokens[1]));
       //                     busUtil.Add(double.Parse(tokens[2]));
       //                 }

       //                 reader.Close();
       //                 for (int chanIndex = 0; chanIndex < channel.Count(); chanIndex++)
       //                 {
       //                     busUtilPerChannel[channel[chanIndex]].Add(busUtil[chanIndex]);
       //                     timePerChannel[channel[chanIndex]].Add(time[chanIndex]);
       //                 }
       //                 for (int chanIndex = 0; chanIndex < channelNum.Count(); chanIndex++)
       //                 {

       //                     if ((double)busUtilPerChannel[channelNum[chanIndex]].Count() != 0)
       //                     {
       //                         double maxTime = endTime[channelNum[chanIndex]];
       //                         double minTime = startTime[channelNum[chanIndex]];
       //                         totalTime.Add(channelNum[chanIndex], maxTime);

       //                         double totalBusTime = 0;
       //                         double effectiveBusTime = 0;
       //                         foreach (double t in busUtilPerChannel[channelNum[chanIndex]])
       //                         {
       //                             totalBusTime += t;
       //                             if (t == busUtilPerChannel[channelNum[chanIndex]].Max())
       //                             {
       //                                 effectiveBusTime += t;
       //                             }
       //                         }
       //                         if ((double)totalTime[channelNum[chanIndex]] != 0)
       //                         {
       //                             avgONFIUtil.Add((totalBusTime / endTime.Max()) * 100);//(double)totalTime[channelNum[chanIndex]]) * 100);
       //                             effectiveUtil.Add(((double)effectiveBusTime / endTime.Max()) * 100);//(double)totalTime[channelNum[chanIndex]]) * 100);
       //                         }
       //                         else
       //                         {
       //                             avgONFIUtil.Add(0);
       //                             effectiveUtil.Add(0);
       //                         }

       //                     }
       //                     else
       //                     {
       //                         avgONFIUtil.Add(0);
       //                         effectiveUtil.Add(0);
       //                     }
       //                 }

       //                 //chart9.ChartAreas[0].AxisY.CustomLabels.Add(channelNum.Count() + 3, channelNum.Count() + 4, "ONFI");
       //                 //  chart9.ChartAreas[0].InnerPlotPosition.Width = true;
       //                 chart9.SuspendLayout();
       //                 for (int chanIndex = 0; chanIndex < channelNum.Count(); chanIndex++)
       //                 {
       //                     chart9.Series.Add(new Series());
       //                 }
       //                 int lowLimit = 2;
       //                 int upLimit = 4;
       //                 for (int chanIndex = 0; chanIndex < channelNum.Count(); chanIndex++)
       //                 {

       //                     chart9.Legends[0].CustomItems.Add(new LegendItem());
       //                     chart9.Legends[0].CustomItems[chanIndex + 3].Cells.Add(new LegendCell("ch " + channelNum[chanIndex].ToString()));
       //                     chart9.Legends[0].CustomItems[chanIndex + 3].Cells.Add(new LegendCell(Math.Round(avgONFIUtil[channelNum[chanIndex]], 2).ToString()));
       //                     chart9.Legends[0].CustomItems[chanIndex + 3].Cells.Add(new LegendCell(Math.Round(effectiveUtil[channelNum[chanIndex]], 2).ToString()));
       //                     chart9.ChartAreas[0].AxisY.CustomLabels.Add(lowLimit, upLimit, "ch " + channelNum[chanIndex].ToString());
       //                     lowLimit = upLimit;
       //                     upLimit = lowLimit + 3;
       //                     chart9.Series[chanIndex + 2].IsVisibleInLegend = false;

       //                     if (busUtilPerChannel[channelNum[chanIndex]].Count() != 0)
       //                     {
       //                         // chart9.Series.Add(new Series());

       //                         chart9.Series[chanIndex + 2].ChartType = SeriesChartType.FastPoint;

       //                         for (int timeIndex = 0; timeIndex < timePerChannel[channelNum[chanIndex]].Count(); timeIndex++)
       //                         {
       //                             double timeAxis = timePerChannel[channelNum[chanIndex]].ElementAt(timeIndex);

       //                             for (int busIndex = 0; busIndex < busUtilPerChannel[channelNum[chanIndex]].ElementAt(timeIndex); busIndex++)
       //                             {
       //                                 chart9.Series[chanIndex + 2].Points.AddXY(timeAxis, channelNum[chanIndex] * 3 + 3);
       //                                 timeAxis++;
       //                             }
       //                         }

       //                     }//if
       //                     file.Close();
       //                 }//for
       //                 chart9.ResumeLayout();

       //             }//using
       //         }
       //         //chart9.Series.Clear();
       //     }//end try
       //     catch (IOException e)
       //     {
       //         throw new Exception(String.Format("An error occurred while executing the data import: ONFI Utilization Report", e.Message), e);
       //     }
       // }

       // private void ONFIBusUtilizationData(bool multiSim)
       // {
       //     string fileName1 = "";
       //     string fileName2 = "";
       //     if (multiSim)
       //     {

       //         string param = validMultiSimParam.Trim();
       //         if (comboBox33.Text == "Multi Sim based on Block Size")
       //         {

       //             int qIndex = 0;
       //             switch (param)
       //             {
       //                 case "512":
       //                     qIndex = validQDepth.Count() - 1;
       //                     break;
       //                 case "1024":
       //                     qIndex = validQDepth.Count() - 2;
       //                     break;
       //                 case "2048":
       //                     {
       //                         qIndex = validQDepth.Count() - 3;
       //                         break;
       //                     }
       //                 case "4096":
       //                     {
       //                         qIndex = validQDepth.Count() - 4;
       //                         break;
       //                     }
       //                 case "8192":
       //                     {
       //                         qIndex = validQDepth.Count() - 5;
       //                         break;
       //                     }
       //             }
       //             param = validMultiSimParam.Trim();
       //             if (enableWrkld == "1")
       //             {
       //                 fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //                 fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //             }
       //             else
       //             {
       //                 fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //                 fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + param + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //             }
       //         }
       //         else if (comboBox33.Text == "Multi Sim based on QD")
       //         {
       //             //int qdIndex = 0;
       //             param = validMultiSimParam.Trim();
       //             if (enableWrkld == "1")
       //             {
       //                 fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + param + ".log";
       //                 fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + param + ".log";
       //             }
       //             else
       //             {
       //                 fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + param + ".log";
       //                 fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + param + ".log";
       //             }

       //         }
       //         else
       //         {
       //             int qIndex = 0;
       //             switch (param)
       //             {
       //                 case "512":
       //                     qIndex = validQDepth.Count() - 1;
       //                     break;
       //                 case "1024":
       //                     qIndex = validQDepth.Count() - 2;
       //                     break;
       //                 case "2048":
       //                     {
       //                         qIndex = validQDepth.Count() - 3;
       //                         break;
       //                     }
       //                 case "4096":
       //                     {
       //                         qIndex = validQDepth.Count() - 4;
       //                         break;
       //                     }
       //                 case "8192":
       //                     {
       //                         qIndex = validQDepth.Count() - 5;
       //                         break;
       //                     }
       //             }
       //             param = validMultiSimParam.Trim();
       //             fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report_" + "iosize_" + param + "_" + "blksize_" + validBlockSize[0] + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";
       //             fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report_" + "iosize_" + param + "_" + "blksize_" + validBlockSize[0] + "_" + "qd_" + validQDepth[qIndex].ToString().Trim() + ".log";

       //         }
       //     }
       //     else
       //     {
       //         fileName1 = ".\\Reports_PCIe\\onfi_chan_activity_report.log";
       //         fileName2 = ".\\Reports_PCIe\\onfi_chan_util_report.log";

       //     }
       //     plotONFI(fileName1, fileName2);
       // }//end function
        //private void Form1_Load(object sender, EventArgs e)
        //{

        //}

        private void enableBusUtilChartViewSelection()
        {
            this.selectIoSizeBox.Visible = true;
            this.label28.Visible = false;
        }

        private void enableBusUtilizationChart()
        {
            this.onfi_PCIeUtilChart.Enabled = true;
            this.onfi_PCIeUtilChart.Visible = true;
        }

        private void disableBusUtilChartViewSelection()
        {
            this.selectIoSizeBox.Visible = false;
            //this.label28.Visible = false;
            pci_onfi_label.Visible = false;
            pci_onfi_label.Enabled = false;
        }

        private bool isMultiSimQD()
        {

            return simulationTypeBox.Text == "Multi Sim based on QD";
        }

        private void setLatencyIOPSTextBox()
        {
            this.minLatTextBox.Enabled = true;
            this.minLatTextBox.Text = latencyMinima.ToString();
            this.avgLatTextBox.Text = latencyAverage.ToString();
            this.avgLatTextBox.Enabled = true;
            this.maxLatTextBox.Text = latencyMaxima.ToString();
            this.maxLatTextBox.Enabled = true;
            this.avgIOPsTextBox.Text = iopsAverage.ToString();
            this.avgIOPsTextBox.Enabled = true;
        }

        private void resetLatencyIOPSTextBox()
        {
            this.minLatTextBox.Text = "";
            this.avgLatTextBox.Text = "";
            this.maxLatTextBox.Text = "";
            this.avgIOPsTextBox.Text = "";
        }

        private void initMultiSimBlkSizeParam(int yLocation)
        {
            this.chartPanel.ScrollControlIntoView(iopsVsIosizeChart);
            this.iopsVsIosizeChart.Location = new Point(3, yLocation);
            this.cmdCntVslbaChart.Location = new Point(3, yLocation + 292);
            this.latencyVsQDChart.Location = new Point(3, yLocation + 584);
            this.channelUtilChart.Location = new System.Drawing.Point(3, yLocation + 876);
            label32.Enabled = true;
            label32.Visible = true;
            selectIoSizeBox.Enabled = true;
            selectIoSizeBox.Visible = true;
            //comboBox28.Enabled = true;
            //comboBox28.Visible = true;
            this.label32.Location = new Point(840, yLocation + 1168);
            this.label32.Text = "BlockSize";
            this.label28.Text = "BlockSize";
            //this.comboBox28.Location = new System.Drawing.Point(896, yLocation + 1168);
            this.cmdCntVslbaChart.Location = new System.Drawing.Point(3, yLocation + 1168);

            if (isDisableBusUtilChartSelected())
            {
                disableBusUtilizationChart();
                this.selectIoSizeBox.Visible = false;
                this.label28.Visible = false;
            }
            else
            {
                this.onfi_PCIeUtilChart.Enabled = true;
                this.onfi_PCIeUtilChart.Visible = true;
                this.selectIoSizeBox.Visible = true;
                this.label28.Visible = true;
                this.label28.Location = new System.Drawing.Point(840, 1460);
                this.selectIoSizeBox.Location = new System.Drawing.Point(896, 1460);
                this.onfi_PCIeUtilChart.Location = new System.Drawing.Point(3, yLocation + 1460);
            }
        }

        private void disableBusUtilizationChart()
        {
            
        }

        private bool isDisableBusUtilChartSelected()
        {
            return disableONFI_PCIECheckBox.Checked;
        }

        private void enableLatVsIOPS_BlkSize()
        {
            latencyVsQDChart.Enabled = true;
            latencyVsQDChart.Visible = true;
        }

        private void enableLatVsBlkSize()
        {
            cmdCntVslbaChart.Enabled = true;
            cmdCntVslbaChart.Visible = true;
        }

        private void enableIOPSVsBlkSize()
        {
            iopsVsIosizeChart.Enabled = true;
            iopsVsIosizeChart.Visible = true;
        }

        private void disableLatVsIOPS_QD()
        {
            latencyVsIopsVsQDChart.Enabled = false;
            latencyVsIopsVsQDChart.Visible = false;
        }

        private bool isMultiSimBlockSize()
        {
            return simulationTypeBox.Text == "Multi Sim based on Block Size";
        }

        private void disableLatencyVsQDChart()
        {
            bankUtilChart.Enabled = false;
            bankUtilChart.Visible = false;
        }

        private void disableIOPSVsQDChart()
        {
            onfi_PCIeUtilChart.Enabled = false;
            onfi_PCIeUtilChart.Visible = false;
        }

        private void resetMultiSimCharts()
        {
            this.minLatTextBox.Text = "";
            this.avgLatTextBox.Text = "";
            this.maxLatTextBox.Text = "";
            this.avgIOPsTextBox.Text = "";
            this.minLatTextBox.Enabled = false;
            this.avgLatTextBox.Enabled = false;
            this.maxLatTextBox.Enabled = false;
            this.avgIOPsTextBox.Enabled = false;

            onfi_PCIeUtilChart.Series.Clear();
            iopsVsIosizeChart.Series.Clear();
            latencyVsIosizeChart.Series.Clear();
            latencyVsIopsVsIoSizeChart.Series.Clear();
            iopsVsQdChart.Series.Clear();
            iopsVsIosizeChart.Enabled = false;
            iopsVsIosizeChart.Visible = false;

            latencyVsIosizeChart.Enabled = false;
            latencyVsIosizeChart.Visible = false;

            latencyVsIopsVsIoSizeChart.Enabled = false;
            latencyVsIopsVsIoSizeChart.Visible = false;

            iopsVsQdChart.Enabled = false;
            iopsVsQdChart.Visible = false;
        }

        private void resetDefaultCharts()
        {
            latencyVsIopsVsIoSizeChart.Series.Clear();
            iopsVsIosizeChart.Annotations.Clear();
            cmdCntVslbaChart.Annotations.Clear();
            //chart6.Annotations.Clear();
            latencyVsQDChart.Annotations.Clear();
            channelUtilChart.Annotations.Clear();
            onfi_PCIeUtilChart.Annotations.Clear();
            bankUtilChart.Annotations.Clear();
            channelUtilChart.Series.Clear();
            //chart9.Series.Clear();
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
        List<string> sqDepth = new List<string>();

        private void initParams()
        {
            pollWaitTime = pollingStatusBox.Text;
            numChannel = numChannelBox.Text;
            ioSize = hostIOSizeBox.Text;
            blkSize = blockSizeBox.Text;
            //ddrSpeed = comboBox13.Text;
            credit = outstandingCmdBox.Text;
            onfiSpeed = onfiSpeedBox.Text;
            interfaceType = onfiInterfaceTypeBox.Text;
            programTime = programTimeBox.Text;
            readTime = readTimeBox.Text;
            pageSize = pageSizeBox.Text;
            numPages = numPagesBox.Text;
            numBanks = numBanksBox.Text;
            numDie = numDieBox.Text;
            enableLogs = enLogsCheckBox.Checked.ToString();
            emCacheSize = cwSizeBox.Text;
            //sqSize = HostQDepthBox.Text;
            //cqSize = comboBox5.Text;
            numCommands = numCommandsBox.Text;
            queueFactor = queueFactorBox.Text;
            timeScale = comboBox22.Text;
            //depthOfQueue = queueDepth;
            //coreMode = checkBox5.Checked.ToString();
            //numCores = comboBox27.Text;
            cmdTransferTime = cmdOHTextBox.Text;
            zoomScale = int.Parse(comboBox35.Text);

            cmdType = "1";
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

        //private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    resetButtons();
        //    if (string.IsNullOrWhiteSpace(comboBox7.Text)) return;
        //    numPages = comboBox7.Text;
        //}

        private void cqDepthBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(cqDepthBox.Text)) return;
            //cqSize = comboBox5.Text;
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
            return int.Parse(ioSize) / int.Parse(emCacheSize);
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
            //blkSize = " " + blkSize.Trim();
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
            cntrlQueueDepth = " " + cntrlQueueDepth.Trim();
            pollWaitTime = " " + pollWaitTime.Trim();
            //depthOfQueue = " " + depthOfQueue.Trim();
            credit = " " + credit.Trim();
            readTime = " " + readTime.Trim();
            programTime = " " + programTime.Trim();
            queueFactor = " " + queueFactor.Trim();
           // ddrSpeed = " " + ddrSpeed.Trim();
            cmdTransferTime = " " + cmdTransferTime.Trim();
           // numCores = " " + numCores.Trim();
            //wrkloadBS = " " + wrkloadBS.Trim();
          
            //if (enableWrkld == "1" && (comboBox30.SelectedIndex != 0))
            //{
            //    wrkloadFile = comboBox32.Text;
            //    wrkloadFile = " " + "../../../../../Workload/" + wrkloadFile.Trim();
            //}
            //else
            //{
            //    wrkloadFile = " " + wrkloadFile.Trim();
            //}

            //if (mCoreMode.Trim() == "Single CPU")
            //{
            //    mode = 0;
            //    coreMode = " " + mode.ToString().Trim();
            //}
            //else if (mCoreMode.Trim() == "Multi Core")
            //{
            //    mode = 1;
            //    //depthOfQueue = "1";
            //    depthOfQueue = " " + depthOfQueue.Trim();
            //    coreMode = " " + mode.ToString().Trim();
            //}

            enableSeqLBA = " " + enableSeqLBA.Trim();
            cmdType = " " + cmdType.Trim();
            cmdPct = " " + cmdPct.Trim();
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
            rdBuffSize = " " + rdBuffSize.Trim();
            wrBuffSize = " " + wrBuffSize.Trim();

        }

        private void initIOSizeParam(List<string> IOSize)
        {
            validQDepth.Clear();
            validBlockSize.Clear();
            validIOSize.Clear();

            validMultiSimParam = "4096";
            selectIoSizeBox.Items.Clear();
            //comboBox28.Items.Clear();

           // button3.Enabled = false;
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
            sqDepth.Add(" 2");
            queueD.Add(" 2");
            sqDepth.Add(" 3");
            queueD.Add(" 4");
            sqDepth.Add(" 5");
            queueD.Add(" 8");
            sqDepth.Add(" 9");
            queueD.Add(" 16");
            sqDepth.Add(" 17");
            queueD.Add(" 32");
            sqDepth.Add(" 33");
            queueD.Add(" 64");
            sqDepth.Add(" 65");
            queueD.Add(" 128");
            sqDepth.Add(" 129");
            queueD.Add(" 256");
            sqDepth.Add(" 257");

            removeFiles = " 1";
            validIOSize.Add(ioSize.Trim());
            selectIoSizeBox.Items.Clear();
            //comboBox28.Items.Clear();
        }

        //private void startIOSizeMultiSim(string enMultiSim, List<string> IOSize, int ioSizeIndex, string finalQDepth)
        //{
        //    string var = "";
        //    if (enableWrkld == "1")
        //    {
        //        string enWrkld = " " + enableWrkld.Trim();
        //        var = numCommands + IOSize[ioSizeIndex] + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
        //        + numPages + finalQDepth + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
        //        + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile;

        //    }
        //    else
        //    {
        //        string enWrkld = " " + enableWrkld.Trim();
        //        // wrkloadFile = " wrkload.txt";
        //        var = numCommands + IOSize[ioSizeIndex] + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
        //            + numPages + finalQDepth + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
        //            + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
        //    }
        //    Process process = new Process();

        //    try
        //    {
        //        process.StartInfo.FileName = @"..\..\..\..\..\Solution\Release\TeraSMemoryController.exe";
        //        process.StartInfo.Arguments = var; // Put your arguments here
        //        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //        process.Start();
        //        label27.Text = "Running...";
        //        process.WaitForExit();
        //        double percentage = (double)((double)(ioSizeIndex + 1) / IOSize.Count()) * 100;
        //        progressBar1.Value = ioSizeIndex + 1;
        //        removeFiles = " 0";

        //    }
        //    finally
        //    {
        //        if (process != null)
        //        {
        //            process.Close();
        //        }

        //    }
        //}

      

        //private void startBlkSizeMultiSim(string enMultiSim, List<string> blockSize, int blockIndex, string finalQDepth)
        //{
        //    string var = "";
        //    if (enableWrkld == "1")
        //    {
        //        string enWrkld = " " + enableWrkld.Trim();
        //        wrkloadBS = " " + wrkloadBS.Trim();
        //        var = numCommands + wrkloadBS + blockSize[blockIndex] + emCacheSize + pageSize + numBanks + numDie + numChannel
        //        + numPages + finalQDepth + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
        //        + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;

        //    }
        //    else
        //    {
        //        string enWrkld = " " + enableWrkld.Trim();
        //        // wrkloadFile = " wrkload.txt";
        //        var = numCommands + ioSize + blockSize[blockIndex] + emCacheSize + pageSize + numBanks + numDie + numChannel
        //            + numPages + finalQDepth + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
        //            + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
        //    }
        //    Process process = new Process();

        //    try
        //    {
        //        process.StartInfo.FileName = @"..\..\..\..\..\Solution\Release\TeraSMemoryController.exe";

        //        process.StartInfo.Arguments = var; // Put your arguments here
        //        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //        process.Start();
        //        label27.Text = "Running...";
        //        process.WaitForExit();
        //        double percentage = (double)((double)(blockIndex + 1) / blockSize.Count()) * 100;
        //        progressBar1.Value = blockIndex + 1;
        //        removeFiles = " 0";

        //    }
        //    finally
        //    {
        //        if (process != null)
        //        {
        //            process.Close();
        //        }

        //    }
        //}

        private void initBlkSizeParam(List<string> blockSize)
        {
            validQDepth.Clear();
            validIOSize.Clear();
            validBlockSize.Clear();

            validMultiSimParam = "4096";
            selectIoSizeBox.Items.Clear();
            //comboBox28.Items.Clear();

            validBlockSize.Clear();
            //button3.Enabled = false;
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

        private void onfiInterfaceTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(onfiInterfaceTypeBox.Text)) return;
            interfaceType = onfiInterfaceTypeBox.Text;
        }
        
        private void TeraSController_Load(object sender, EventArgs e)
        {

        }

        private void workLoadTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(workLoadTypeBox.Text)) return;

            simulationTypeBox.Items.Clear();
            simulationTypeBox.Items.Add("Single Sim");
          
            switch(workLoadTypeBox.Text)
            {
                case "Load Workload File": 
                    /* Workload is from the file */
                    findFilesInDir();
                    wrkloadInputBox.SelectedIndex = 0;
                    enableWrkld = "1";
                    hostIOSizeBox.Enabled = false;
                    cwSizeBox.Enabled = false;
                    numCommandsBox.Enabled = false;
                    cmdType = "100";
                    enableSeqLBA = "100";
                    cmdPct = "0";
                    simulationTypeBox.Items.Add("Multi Sim based on IO Size");
                    simulationTypeBox.Items.Add("Multi Sim based on QD");
                    break;
                
                case "Sequential 100% Read":
                    cmdType = "2";
                    enableSeqLBA = "100";
                    cmdPct = "100";
                    wrkloadInputBox.Enabled = false;
                    enableWrkld = "0";
                    hostIOSizeBox.Enabled = true;
                    cwSizeBox.Enabled = true;
                    numCommandsBox.Enabled = true;
                    simulationTypeBox.Items.Add("Multi Sim based on IO Size");
                    simulationTypeBox.Items.Add("Multi Sim based on QD");
                    break;

                case "Sequential 100% Write":
                    cmdType = "1";
                    cmdPct = "0";
                    enableSeqLBA = "100";
                    wrkloadInputBox.Enabled = false;
                    enableWrkld = "0";
                    hostIOSizeBox.Enabled = true;
                    cwSizeBox.Enabled = true;
                    numCommandsBox.Enabled = true;
                    simulationTypeBox.Items.Add("Multi Sim based on IO Size");
                    simulationTypeBox.Items.Add("Multi Sim based on QD");
                    break;

                case "Random 100% Read":
                    cmdType = "2";
                    cmdPct = "100";
                    enableSeqLBA = "0";
                    wrkloadInputBox.Enabled = false;
                    enableWrkld = "0";
                    hostIOSizeBox.Enabled = true;
                    cwSizeBox.Enabled = true;
                    numCommandsBox.Enabled = true;
                    simulationTypeBox.Items.Add("Multi Sim based on IO Size");
                    simulationTypeBox.Items.Add("Multi Sim based on QD");
                    break;

                case "Random 100% Write":
                    cmdType = "1";
                    cmdPct = "0";
                    enableSeqLBA = "0";
                    wrkloadInputBox.Enabled = false;
                    enableWrkld = "0";
                    hostIOSizeBox.Enabled = true;
                    cwSizeBox.Enabled = true;
                    numCommandsBox.Enabled = true;
                    simulationTypeBox.Items.Add("Multi Sim based on IO Size");
                    simulationTypeBox.Items.Add("Multi Sim based on QD");
                    break;

                case "Random 30/70 R/W":
                    cmdType = "1";
                    cmdPct = "30";
                    enableSeqLBA = "0";
                    wrkloadInputBox.Enabled = false;
                    enableWrkld = "0";
                    hostIOSizeBox.Enabled = true;
                    cwSizeBox.Enabled = true;
                    numCommandsBox.Enabled = true;
                    simulationTypeBox.Items.Add("Multi Sim based on IO Size");
                    simulationTypeBox.Items.Add("Multi Sim based on QD");
                    break;

                case "Random 70/30 R/W":
                    cmdType = "70";
                    enableSeqLBA = "0";
                    cmdPct = "70";
                    wrkloadInputBox.Enabled = false;
                    enableWrkld = "0";
                    hostIOSizeBox.Enabled = true;
                    cwSizeBox.Enabled = true;
                    numCommandsBox.Enabled = true;
                    simulationTypeBox.Items.Add("Multi Sim based on IO Size");
                    simulationTypeBox.Items.Add("Multi Sim based on QD");
                    break;

                case "Random 50/50 R/W":
                    cmdType = "50";
                    enableSeqLBA = "0";
                    cmdPct = "50";
                    wrkloadInputBox.Enabled = false;
                    enableWrkld = "0";
                    hostIOSizeBox.Enabled = true;
                    cwSizeBox.Enabled = true;
                    numCommandsBox.Enabled = true;
                    simulationTypeBox.Items.Add("Multi Sim based on IO Size");
                    simulationTypeBox.Items.Add("Multi Sim based on QD");
                    break;

                case "Sequential 50/50 R/W":
                    cmdType = "50";
                    enableSeqLBA = "100";
                    cmdPct = "50";
                    wrkloadInputBox.Enabled = false;
                    enableWrkld = "0";
                    hostIOSizeBox.Enabled = true;
                    cwSizeBox.Enabled = true;
                    numCommandsBox.Enabled = true;
                    simulationTypeBox.Items.Add("Multi Sim based on IO Size");
                    simulationTypeBox.Items.Add("Multi Sim based on QD");
                    break;
                
                default:
                    break;

            }
          
           
            simulationTypeBox.SelectedIndex = 0;
            
        }

        private void findFilesInDir()
        {
           // string[] dirs = Directory.GetFiles(@".\Workload\", "*.txt");
            string[] dirs = Directory.GetFiles(@".\Workload\", "*.txt");
            wrkloadInputBox.Items.Clear();
            foreach (string dir in dirs)
            {
                /*Remove the path from the file name */
                string file = dir.Remove(0, 11);// - "..\..\..\..\..\Workload\";
                wrkloadInputBox.Enabled = true;
                wrkloadInputBox.Items.Add(file);
            }
        }


        private void disableGUIParamSelect()
        {
            
            enLogsCheckBox.Enabled = false;
        }

        private void enableGUIParamSelect()
        {
            enLogsCheckBox.Enabled = true;
        }
        //private void button4_Click(object sender, EventArgs e)
        //{
        //    disableGUIParamSelect();
        //    Stream myStream = null;
        //    OpenFileDialog openFileDialog1 = new OpenFileDialog();
        //    List<string> configLine = new List<string>();
        //    openFileDialog1.InitialDirectory = "..\\..\\..\\..\\..\\Configuration\\";
        //    openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        //    openFileDialog1.FilterIndex = 2;
        //    openFileDialog1.RestoreDirectory = true;
        //    List<string> config = new List<string>();
        //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        try
        //        {
        //            if ((myStream = openFileDialog1.OpenFile()) != null)
        //            {
        //                using (StreamReader reader = new StreamReader(myStream))
        //                {
        //                    while (reader.Peek() != -1)
        //                    {
        //                        string line = reader.ReadLine();
        //                        if (line.StartsWith("//"))
        //                        {
        //                            continue;
        //                        }
        //                        else
        //                        {
        //                            config.Add(line);
        //                        }
        //                    }
        //                    // Insert code to read the stream here.
        //                    reader.Close();
        //                }
        //                numCommands = config[0];
        //                cmdType = config[1];
        //                enableSeqLBA = config[2];
        //                ioSize = config[3];
        //                emCacheSize = config[4];
        //                pageSize = config[5];
        //                numChannel = config[6];
        //                numDie = config[7];
        //                numBanks = config[8];
        //                queueFactor = config[9];
        //                queueDepth = config[10];
        //                numPages = config[11];
        //                readTime = config[12];
        //                programTime = config[13];
        //                cmdTransferTime = config[14];
        //                credit = config[15];
        //                ddrSpeed = config[16];
        //                onfiSpeed = config[17];
        //                interfaceType = config[18];
        //                enableLogs = config[19];
        //                if (config[20] == "True")
        //                {
        //                    enMultiSim = true;
        //                }
        //                else
        //                {
        //                    enMultiSim = false;
        //                }
        //                if (enMultiSim)
        //                {
        //                    button3.Enabled = true;
        //                    checkBox3.Enabled = true;
        //                    checkBox3.Checked = true;
        //                    button2.Enabled = true;
        //                }
        //                else
        //                {
        //                    button3.Enabled = false;
        //                    button2.Enabled = true;
        //                }
        //                ioSizeVariation = bool.Parse(config[21]);
        //                coreMode = config[22];
        //                numCores = config[23];
        //                enableWrkld = config[24];
        //                wrkloadFile = config[25];
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
        //        }
        //        myStream.Close();
        //    }
        //}

        List<string> config = new List<string>();
        
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
            writer.WriteLine("input_type= " + workLoadTypeBox.Text);

            
            writer.WriteLine(";Sim Type");
            writer.WriteLine("sim_type= " + simulationTypeBox.Text);
            writer.WriteLine(";Taffic Pattern");
            string pattern;
            if (enableSeqLBA.Trim() == "100")
                pattern = "Sequential";
            else
                pattern = "Random";

            writer.WriteLine("pattern= " + pattern);
            //writer.WriteLine(";Workload");
            //writer.WriteLine(enableWrkld);
            writer.WriteLine(";Workload File");
            //if (workLoadTypeBox.Text == "Load Workload File")
                writer.WriteLine("workload_file= " + wrkloadInputBox.Text);
            //string fileName = wrkloadFile.Remove(0, 25);
            //writer.WriteLine("workload_file= " + fileName);
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
            writer.WriteLine("queue_depth= " + hostQDepth);
            writer.WriteLine(";PCIe Gen");
            writer.WriteLine("PCIe_Gen= " + PCIeGenBox.Text);
            writer.WriteLine(";PCIe Lanes");
            writer.WriteLine("Num_Lanes= " + PCIeLaneBox.Text);
            writer.WriteLine(";Polling Status");
            writer.WriteLine("polling_status= " + pollWaitTime);
            

            writer.WriteLine(" ");
            writer.WriteLine("[Controller Parameters]");
            //writer.WriteLine(";Block Size");
            //writer.WriteLine("blk_size= " + blkSize);
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
            writer.WriteLine(";Read Buffer Size");
            writer.WriteLine("Read_buffer= " + rdBuffSize);
            writer.WriteLine(";Write Buffer Size");
            writer.WriteLine("Write_buffer= " + wrBuffSize);

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

        //private void panel10_Paint(object sender, PaintEventArgs e)
        //{

        //}

        private void resetButtons()
        {
            progressBar1.Value = 0;
            progressLabel.Text = "Progress...";
            progressBar2.Value = 0;
            progressLabel1.Text = "Progress...";
            button1.Enabled = false;
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

        bool isButtonClicked = true;
       
        private void chartPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            chartPanel.Focus();
        }

        private void parameterPanel_EventHandler(object sender, MouseEventArgs e)
        {
            parameterPanel.Focus();
        }
        bool initFlag = true;
        int lastZoomScale = 1;

        private void cntrlrQueueDepthBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            cntrlQueueDepth = contrlQueueDepthBox.Text;

        }

        private void numCommandsBox_TextChanged(object sender, EventArgs e)
        {
            resetButtons();
            numCommands = "8";
            numCommands = numCommandsBox.Text;
        }

        private void enLogsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (enLogsCheckBox.Checked)
                enableLogs = " 1";
            else
                enableLogs = " 0";
        }

        private void hostQDepthBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            
            hostQDepth = HostQDepthBox.Text;
            int sqDepth = int.Parse(hostQDepth) + 1; 
            sqSize = sqDepth.ToString();
            cqSize = sqDepth.ToString();
            
        }

        private void pollingStatusBox_TextChanged(object sender, EventArgs e)
        {
            resetButtons();
            pollWaitTime = pollingStatusBox.Text;
        }

        private void inputTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            progressBar1.Value = 0;
            progressLabel.Text = "Progress";
            progressBar2.Value = 0;
            progressLabel1.Text = "Progress";

            switch(inputTypeBox.SelectedIndex)
            {
                case 1:
                    string[] dirs = Directory.GetFiles(@".\Configuration\", "*.txt");
                configFileBox.Items.Clear();
                foreach (string dir in dirs)
                {
                    string file = dir.Remove(0, 16);
                    configFileBox.Items.Add(file);
                }
                configFileBox.Enabled = true;
                configFileBox.SelectedIndex = 0;
                numCommandsBox.Enabled = false;
                hostParamPanel.Enabled = false;
                controllerParamPanel.Enabled = false;
                deviceParamPanel.Enabled = false;
                workLoadTypeBox.Enabled = false;
                simulationTypeBox.Enabled = false;
                wrkloadInputBox.Enabled = false;
                //enableWrkld = "0";
                break;

                case 2:
                if (configFileBox.Items.Count != 0)
                {
                    configFileBox.Items.Clear();
                }
                //configFileBox.Items.Add(configFile);
                configFileBox.Enabled = false;
                numCommandsBox.Enabled = true;
                hostParamPanel.Enabled = true;
                controllerParamPanel.Enabled = true;
                deviceParamPanel.Enabled = true;
                //comboBox23.Enabled = true;
                //comboBox33.Enabled = true;

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
                break;

                default:
                workLoadTypeBox.Enabled = true;
                simulationTypeBox.Enabled = true;
                if (workLoadTypeBox.Text == "Load Workload File")
                {
                    numCommandsBox.Enabled = false;
                    wrkloadInputBox.Enabled = true;
                    //comboBox32.Enabled = true;
                    enableWrkld = "1";
                }
                else
                {
                    numCommandsBox.Enabled = true;
                    //comboBox32.Enabled = false;
                    enableWrkld = "0";
                }
                hostParamPanel.Enabled = true;
                controllerParamPanel.Enabled = true;
                deviceParamPanel.Enabled = true;
                configFileBox.Enabled = false;

                    
                break;
            }
            //if (inputTypeBox.SelectedIndex == 1)
            //{
                
            //}
            //else if (comboBox30.SelectedIndex == 1)
            //{
                
            //}
            //else
            //{
            //   
            //}
        }

        private void comboBox37_SelectedIndexChanged(object sender, EventArgs e)
        {
            ioSize = comboBox37.Text;
            blkSize = ioSize;
            if(comboBox37.Text == "512")
            {
                numCommands = "1024";
                
            }
            else if(comboBox37.Text == "4096")
            {
                numCommands = "256";
                
            }

        }

        private void crossbarLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.crossbarLinkLabel.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("http://www.crossbar-inc.com");

        }

        private void simulationTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (simulationTypeBox.Text == "Single Sim")
            {
                enMultiSim = false;
                if (workLoadTypeBox.Text == "Load Workload File")
                {
                    hostIOSizeBox.Enabled = false;
                }
                else
                {
                    hostIOSizeBox.Enabled = true;
                }
                //button3.Enabled = false;
                selectIoSizeBox.Enabled = false;
                selectIoSizeBox.Visible = false;
                //comboBox28.Enabled = false;
                //comboBox28.Visible = false;
                blockSizeBox.Enabled = true;
                //if (!checkBox5.Checked)
                //{
                //    comboBox19.Enabled = true;
                //}
                button2.Enabled = true;
                //panel4.Enabled = false;
                label28.Visible = false;
                removeFiles = " 0";
                HostQDepthBox.Enabled = true;
                hostIOSizeBox.Enabled = true;
            }
            else if (simulationTypeBox.Text == "Multi Sim based on Block Size")
            {
                multiSimParam = "BlockSize";
                if (workLoadTypeBox.Text == "Load Workload File")
                {
                    hostIOSizeBox.Enabled = false;
                }
                else
                {
                    hostIOSizeBox.Enabled = true;
                }
                enMultiSim = true;
                button2.Enabled = true;
                //button3.Enabled = true;
                //panel4.Enabled = true;

                blockSizeBox.Enabled = false;
                //if (!checkBox5.Checked)
                //{
                //    comboBox19.Enabled = true;
                //}
                progressBar1.Value = 0;
                progressLabel.Text = "Progress";
                removeFiles = " 1";
                validBlockSize.Clear();
                validQueueDepth.Clear();

            }
            else if (simulationTypeBox.Text == "Multi Sim based on QD")
            {
                multiSimParam = "Queue Depth";
                enMultiSim = true;
                button2.Enabled = true;
                //panel4.Enabled = true;
                if (workLoadTypeBox.Text == "Load Workload File")
                {
                    hostIOSizeBox.Enabled = false;
                }
                else
                {
                    hostIOSizeBox.Enabled = true;
                }
                blockSizeBox.Enabled = true;
                HostQDepthBox.Enabled = false;
                hostIOSizeBox.Enabled = true;
                removeFiles = " 1";
                validBlockSize.Clear();
                validQueueDepth.Clear();

            }
            else if (simulationTypeBox.Text == "Multi Sim based on IO Size")
            {
                multiSimParam = "IOSize";
                enMultiSim = true;
                button2.Enabled = true;
                ///button3.Enabled = true;
                //panel4.Enabled = true;

                blockSizeBox.Enabled = true;
                //if (!checkBox5.Checked)
                //{
                //    comboBox19.Enabled = true;
                //}
                hostIOSizeBox.Enabled = false;
                HostQDepthBox.Enabled = true;
                removeFiles = " 1";
                validIOSize.Clear();
                validQueueDepth.Clear();

            }
        }

        private void selectQueueDepthBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            validParam = selectQueueDepthBox.Text;
        }

        private void selectIoSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            validMultiSimParam = selectIoSizeBox.Text;
        }

        private void comboBox28_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void numChannelBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(numChannelBox.Text)) return;
            numChannel = numChannelBox.Text;
        }

        private void cwSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
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

        private void onfiSpeedBox_TextChanged(object sender, EventArgs e)
        {
            resetButtons();
            onfiSpeed = "800";
            onfiSpeed = onfiSpeedBox.Text;
        }

        private void numDieBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(numDieBox.Text)) return;
            numDie = numDieBox.Text;
        }

        private void PCIeLaneBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PCIeGenBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void queueFactorBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(queueFactorBox.Text)) return;
            queueFactor = queueFactorBox.Text;
        }

        private void outstandingCmdBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            if (string.IsNullOrWhiteSpace(outstandingCmdBox.Text)) return;
            credit = outstandingCmdBox.Text;
        }

        private void pageSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            pageSize = pageSizeBox.Text;
        }

        private void numBanksBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            numBanks = numBanksBox.Text;
        }

        private void numPagesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            numPages = numPagesBox.Text;
        }

        private void programTimeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            programTime = programTimeBox.Text;
        }

        private void readTimeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            readTime = readTimeBox.Text;
        }

        private void minLatTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmdOHTextBox_TextChanged(object sender, EventArgs e)
        {
            resetButtons();
            cmdTransferTime = "10";
            cmdTransferTime = cmdOHTextBox.Text;
        }

        private void readBufferSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            rdBuffSize = readBufferSizeBox.Text;
        }

        private void writeBufferSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            wrBuffSize = writeBufferSizeBox.Text;
        }

        private void hostIOSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ioSize = hostIOSizeBox.Text;
            cwSizeBox.Items.Clear();
            switch(int.Parse(ioSize))
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
           // cwSizeBox.SelectedIndex = 0;
        
        }

        private void wrkloadInputBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //wrkloadFile = " ../../../../Workload/" + wrkloadInputBox.Text;
            wrkloadFile = wrkloadInputBox.Text;
            //wrkloadFile = " " + "../../../Workload/" + wrkloadFile.Trim();// String.Concat("..\\..\\..\\..\\..\\Workload\\", wrkloadFile);
            wrkloadFile = " " + "./Workload/" + wrkloadFile.Trim();// String.Concat("..\\..\\..\\..\\..\\Workload\\", wrkloadFile);

            //string fileName = "D:/CrossbarWorkSpace/Workload/" + wrkloadInputBox.Text.Trim();
            //string cmdType = "";
            //string lba = "";
            //string cwCnt = "";
            //using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //{
            //    using (StreamReader reader = new StreamReader(file))
            //    {
            //        if (reader.Peek() != -1)
            //        {
            //            string line = reader.ReadLine();
            //            string[] tokens = line.Split(' ');

            //            cmdType = tokens[0];
            //            lba = tokens[1];
            //            cwCnt = tokens[2];

            //        }
            //        reader.Close();
            //    }
            //}
            //int blockSize = int.Parse(cwCnt) * 512;
            //wrkloadBS = blockSize.ToString();
        }

        private void configFileBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetButtons();
            configFileBox.BackColor = Color.DarkGray;
            config.Clear();
            string fullPath = ".\\Configuration\\" + configFileBox.Text;
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
                    if (config[1].Trim() == "Load Workload File")
                    {
                        cmdType = "100";
                        enableSeqLBA = "0";
                        enableWrkld = "1";
                    }
                    else if (config[1].Trim() == "Random 100% Read")
                    {
                        cmdType = "100";
                        enableSeqLBA = "0";
                        enableWrkld = "0";
                    }
                    else if (config[1].Trim() == "Sequential 100% Read")
                    {
                        cmdType = "100";
                        enableSeqLBA = "100";
                        enableWrkld = "0";
                    }
                    else if (config[1].Trim() == "Sequential 100% Write")
                    {
                        cmdType = "0";
                        enableSeqLBA = "100";
                        enableWrkld = "0";
                    }
                    else if (config[1].Trim() == "Random 100% Write")
                    {
                        cmdType = "0";
                        enableSeqLBA = "0";
                        enableWrkld = "0";
                    }
                    else if (config[1].Trim() == "Random 30/70 R/W")
                    {
                        cmdType = "30";
                        enableSeqLBA = "0";
                        enableWrkld = "0";
                    }
                    else if (config[1].Trim() == "Random 70/30 R/W")
                    {
                        cmdType = "70";
                        enableSeqLBA = "0";
                        enableWrkld = "0";
                    }
                    else if (config[1].Trim() == "Random 50/50 R/W")
                    {
                        cmdType = "50";
                        enableSeqLBA = "0";
                        enableWrkld = "0";
                    }
                    else if (config[1].Trim() == "Sequential 50/50 R/W")
                    {
                        cmdType = "50";
                        enableSeqLBA = "100";
                        enableWrkld = "0";
                    }
                    simulationTypeBox.Text = config[2].Trim();

                    //comboBox33.Text = config[2].Trim();
                    //if(config[3] =="Seq")
                    wrkloadFile = " ./Workload/" + config[4].Trim();
                    if (config[5].Trim() == "false")
                        enableLogs = "0";
                    else
                        enableLogs = "1";

                    ioSize = config[6].Trim();
                    hostQDepth = config[7].Trim();
                    PCIeGenBox.Text = config[8].Trim();
                    PCIeLaneBox.Text = config[9].Trim();
                    pollWaitTime = config[10].Trim();
                   
                    //Controller Parameters
                    emCacheSize = config[11].Trim();
                    numChannel = config[12].Trim();
                    numDie = config[13].Trim();
                    queueFactor = config[14].Trim();
                    credit = config[15].Trim();
                    rdBuffSize = config[16].Trim();
                    wrBuffSize = config[17].Trim();

                    numBanks = config[18].Trim();
                    pageSize = config[19].Trim();
                    numPages = config[20].Trim();
                    readTime = config[21].Trim();
                    programTime = config[22].Trim();
                    cmdTransferTime = config[23].Trim();
                    onfiSpeed = config[24].Trim();
                    interfaceType = config[25].Trim();
                    button2.Enabled = true;
                    if (enMultiSim)
                    {
                        // button3.Enabled = true;
                        //checkBox3.Enabled = true;
                       // checkBox3.Checked = true;
                        //button2.Enabled = false;
                    }
                    else
                    {
                        //button3.Enabled = false;
                        //button2.Enabled = true;
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

        private void blockSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
