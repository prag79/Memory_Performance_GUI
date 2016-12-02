using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace DIMMDemoGUI
{
    public partial class DIMMDemoGui : Form
    {
        public DIMMDemoGui()
        {
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(chartPanel_MouseWheel);
            //this.chart13.MouseClick += new MouseEventHandler(chart13_SelectionRangeChanged);
            string reportPath = @".\Reports";
            System.IO.DirectoryInfo dir = new DirectoryInfo(reportPath);
            if (System.IO.Directory.Exists(reportPath))
            {
                foreach (FileInfo file in dir.GetFiles()) file.Delete();
            }
            else
            {
                System.IO.Directory.CreateDirectory(reportPath);
            }

            initSimParameters();
        }

        private void chartPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            chartPanel.Focus();
        }
        private void ioSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ioSize = IOSizeBox.Text;
            blkSize = IOSizeBox.Text;

            if(ioSize == "512")
            {
                numCommands = "1024";
            }
            else
            {
                numCommands = "256";
            }
        }

        private void initSimParameters()
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
            runSimButton.Enabled = true;
            enableSeqLBA = "100";
            wrkloadFile = ".\\Workload.txt";
            removeFiles = " 1";
            validBlockSize.Clear();
            validQueueDepth.Clear();
            cmdType = "100";
            enableWrkld = "0";
            IOSizeBox.SelectedIndex = 0;
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

        string multiSimParam;
        int numSlot;
        int cwNum;
        int queueSize;
        int cwCnt;
        int timeScaleVar;
        string depthOfQueue;
        int onfiClock;
        int mode;
        string coreMode;
        bool enMultiSim = true;//Only for Demo
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
        int mAvgIoSize;

        List<string> validBlockSize = new List<string>();
        List<string> validIOSize = new List<string>();
        List<string> validQueueDepth = new List<string>();
        List<double> avgIOPS = new List<double>();
        List<double> maxLatency = new List<double>();
        List<double> minLatency = new List<double>();
        List<double> avgLatency = new List<double>();
        List<int> slotNum = new List<int>();
        List<double> latency = new List<double>();

        #region SIM_BUTTON_CALLBACK
        private void resetButtons()
        {
            progressBar1.Value = 0;
            progressLabel1.Text = "Progress...";
            progressBar2.Value = 0;
            progressLabel2.Text = "Progress...";
            loadGraphButton.Enabled = false;
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

         private void disableLoadGraphButton()
        {
            loadGraphButton.Enabled = false;
        }

         private int getQueueSize()
        {
            return int.Parse(numChannel) * cwNum * int.Parse(queueFactor);
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
            wrkloadFile = " " + wrkloadFile.Trim();
            mode = 0;
            coreMode = " " + mode.ToString().Trim();
            
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
         
        }

         private void resetSimButtonProgressBar(int min, int max)
        {
            progressBar1.Minimum = min;
            progressBar1.Maximum = max;
            progressBar1.Value = 0;
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
                enableSeqLBA = " 100";
                var = numCommands + ioSize + blkSize + emCacheSize + pageSize + numBanks + numDie + numChannel
                    + numPages + queueD[queueIndex] + credit + readTime + programTime + onfiClk + queueFactor + ddrSpeed + enableSeqLBA + cmdType + enableLogs
                    + enMultiSim + removeFiles + coreMode + cmdTransferTime + numCores + enWrkld + wrkloadFile + pollWaitTime;
            }
            Process process = new Process();

            try
            {
                process.StartInfo.FileName = @".\TeraSMemoryController.exe";
                process.StartInfo.Arguments = var; // Put your arguments here
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                progressLabel1.Text = "Running...";
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

        private void setSimButtonProgressBar(int param)
        {
            progressLabel1.Text = "Complete...";
            progressBar1.Value = param;
        }

          private void multiSimCall()
        {
            cwNum = getCwNum();
            disableLoadGraphButton();
            queueSize = getQueueSize();

            setParameterString();
            string enMultiSim = " 1";
          
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
                // validMultiSimParam = "1";
                for (int queueIndex = 0; queueIndex < queueD.Count(); queueIndex++)
                {
                    int qDepthUpperLimit = 256;

                    if ((int.Parse(queueD[queueIndex].Trim()) <= qDepthUpperLimit))
                    {
                        validQueueDepth.Add(queueD[queueIndex].Trim());

                        validParam = validQueueDepth[queueIndex].ToString();
                        // validMultiSimParam = validQueueDepth[queueIndex].ToString();
                        startQDMultiSim(enMultiSim, queueD, queueIndex);
                    }
                    else
                    {
                        setSimButtonProgressBar(queueD.Count());
                    }
                }
                setSimButtonProgressBar(queueD.Count());
            }
            enableLoadGraphButton();
            
        }

         private void enableLoadGraphButton()
        {
            loadGraphButton.Enabled = true;
        }
        private void runSimButton_Click(object sender, EventArgs e)
        {
            resetButtons();
            
          
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
               multiSimCall();
                
            }
            loadGraphButton.Enabled = true;
        }

        #endregion

        #region LATENCY_VS_QD_CHART_CALLBACK
        private void initLatencyVsQDChartAreaParam()
        {
            latencyVsQDChart.ChartAreas[0].AxisX.IntervalAutoMode = 0;
            latencyVsQDChart.ChartAreas[0].AxisX.Minimum = 1;
            latencyVsQDChart.ChartAreas[0].AxisX.Maximum = 512;
            latencyVsQDChart.ChartAreas[0].AxisX.Title = "QD";
            latencyVsQDChart.ChartAreas[0].AxisY.Title = "Latency (microseconds)";
        }

        private void setLatencyVsQDChartOffset(ref int startOffset, ref int endOffset, int queueIndex)
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

        private void setLatencyVsQDChartAnnotation()
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

        /** latencyCalculation
    * Calculates min/max/avg latency of commands 
    * @param bool multiSim
    * @return void
    **/
        private void latencyCalculation()
        {
            try
            {
                maxLatency.Clear();
                minLatency.Clear();
                avgLatency.Clear();

               string fileName = "";

               foreach (string qd in validQueueDepth)
               {
                   if (enableWrkld == "1")
                   {
                       fileName = ".\\Reports\\latency_TB_report_" + "iosize_" + wrkloadBS.Trim() + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + qd + ".log";
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

            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: latency Report", e.Message), e);
            }

        }
        private void LatencyVsQD()
        {

            latencyVsQDChart.Series.Clear();
            latencyVsQDChart.Series.Add("0");
            initLatencyVsQDChartAreaParam();

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
                        for (int queueIndex = 0; queueIndex < validQueueDepth.Count(); queueIndex++)
                        {
                                    setLatencyVsQDChartOffset(ref startOffset, ref endOffset, queueIndex);

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
                           
                        setLatencyVsQDChartAnnotation();
                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: LatencyVsQD", e.Message), e);
            }

        }

#endregion

        #region IOPS_VS_QD_CHART_CALLBACK
        /** iopsCalculation
       * Calculates average iops of the memory controller 
       * @param bool multiSim
       * @return void
       **/
        private void iopsCalculation()
        {
            try
            {
                avgIOPS.Clear();
                string fileName = "";

                foreach (string qd in validQueueDepth)
                {
                    fileName = ".\\Reports\\IOPS_utilization_" + "iosize_" + validIOSize[0] + "_" + "blksize_" + blkSize.Trim() + "_" + "qd_" + qd + ".log";
                    
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

            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: IOPS Report", e.Message), e);
            }
        }
        

        private void initIopsVsQDChartParam()
        {
            iopsVsQDChart.ChartAreas[0].AxisX.IntervalAutoMode = 0;
            iopsVsQDChart.ChartAreas[0].AxisX.Minimum = 1;
            iopsVsQDChart.ChartAreas[0].AxisX.Maximum = 512;
            iopsVsQDChart.ChartAreas[0].AxisX.Title = "QD";
            iopsVsQDChart.ChartAreas[0].AxisY.Title = "IOPS (millions)";
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

        private void setIopsVsQDAnnotation()
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
               
        private void IOPSVsQD()
        {

            iopsVsQDChart.Series.Clear();
            iopsVsQDChart.Series.Add("0");
            initIopsVsQDChartParam();

            int startOffset = 0;
            int endOffset = 60;

            for (double x = iopsVsQDChart.ChartAreas[0].AxisX.Minimum; x < iopsVsQDChart.ChartAreas[0].AxisX.Maximum; x *= 2)
            {
                CustomLabel qdLabel = new CustomLabel(startOffset, endOffset, x.ToString(), 0, LabelMarkStyle.None);
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
                                setIopsVsQDAnnotation();

                        writer.Close();
                    }

                    file.Close();
                }
            }
            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: IOPSVsQD", e.Message), e);
            }
        }

        private void initLatencyVsIOPSChartParam()
        {
            latencyVsIOPswithQDChart.Series.Clear();
            latencyVsIOPswithQDChart.Series.Add("0");
            latencyVsIOPswithQDChart.Series[0].IsVisibleInLegend = false;
            latencyVsIOPswithQDChart.ChartAreas[0].AxisX.Interval = 1;
            latencyVsIOPswithQDChart.ChartAreas[0].AxisX.IntervalOffset = 1;
            latencyVsIOPswithQDChart.ChartAreas[0].AxisX.Minimum = 0;
            latencyVsIOPswithQDChart.ChartAreas[0].AxisY.Title = "Latency (microseconds)";
            latencyVsIOPswithQDChart.ChartAreas[0].AxisX.Title = "IOPS (millions)";
        }

#endregion

        #region LATENCY_VS_QD_WITH_CHART_CALLBACK
        private void setLatencyVsIOPsWithQDAnnotation(int iopsIndex, TextAnnotation[] qd, int qIndex)
        {
            qd[qIndex].Name = "qd_iops_" + qIndex.ToString() + validQueueDepth[iopsIndex].ToString();
            qd[qIndex].Text = string.Concat("QD " + validQueueDepth[iopsIndex].ToString());
            qd[qIndex].ForeColor = Color.Black;
            qd[qIndex].Font = new Font("Arial", 10, FontStyle.Bold);
            qd[qIndex].LineWidth = 1;
            latencyVsIOPswithQDChart.Annotations.Add(qd[qIndex]);
            latencyVsIOPswithQDChart.Annotations[qIndex].AxisX = latencyVsIOPswithQDChart.ChartAreas[0].AxisX;
            latencyVsIOPswithQDChart.Annotations[qIndex].AxisY = latencyVsIOPswithQDChart.ChartAreas[0].AxisY;
            latencyVsIOPswithQDChart.Annotations[qIndex].AnchorDataPoint = latencyVsIOPswithQDChart.Series[0].Points[qIndex];
        }
        private void LatencyVsIOPSwithQD()
        {
            initLatencyVsIOPSChartParam();
            try
            {
                using (FileStream file = new FileStream(@".\Reports\LatencyVsIOPSwithQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOPS(Millions)" + "," + "Latency(us)" + "," + "QD");
                        /* Multiple Sim */

                        string chartType;

                        int iopsIndex = 0;
                        TextAnnotation[] qd = { new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
                                             new TextAnnotation(), new TextAnnotation(), new TextAnnotation(),
                                          new TextAnnotation(), new TextAnnotation(), new TextAnnotation()
                                          };



                        latencyVsIOPswithQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        latencyVsIOPswithQDChart.Series[0].Color = Color.Blue;
                        latencyVsIOPswithQDChart.Series[0].BorderWidth = 3;
                        //chart14.Series[0].Name = "IOPS";
                        latencyVsIOPswithQDChart.Annotations.Clear();
                        latencyVsIOPswithQDChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(avgIOPS.Max());

                        for (int qIndex = 0; qIndex < validQueueDepth.Count(); qIndex++)
                        {
                            latencyVsIOPswithQDChart.Series[0].Points.AddXY(avgIOPS[iopsIndex], avgLatency[qIndex]);
                            writer.WriteLine(avgIOPS[iopsIndex] + "," + avgLatency[qIndex] + "," + validQueueDepth[iopsIndex]);
                            latencyVsIOPswithQDChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                            latencyVsIOPswithQDChart.Series[0].MarkerSize = 10;
                            latencyVsIOPswithQDChart.Series[0].MarkerColor = Color.Green;

                            setLatencyVsIOPsWithQDAnnotation(iopsIndex, qd, qIndex);
                            iopsIndex++;
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

        #endregion

        private void resetLoadGraphProgressBar()
        {
            progressBar2.Maximum = 5;
            progressBar2.Minimum = 0;
            progressBar2.Value = 0;
        }

        private void setSimButtonProgressBar2()
        {
            progressLabel2.Text = "Complete...";
            progressBar2.Value = 5;
        }
        private void loadGraphButton_Click(object sender, EventArgs e)
        {
            runSimButton.Enabled = false;
            resetLoadGraphProgressBar();
            latencyCalculation();
            iopsCalculation();
            LatencyVsQD();
            IOPSVsQD();
            LatencyVsIOPSwithQD();
            setSimButtonProgressBar2();
            loadGraphButton.Enabled = false;
            runSimButton.Enabled = true;
        }

        
    }
}
