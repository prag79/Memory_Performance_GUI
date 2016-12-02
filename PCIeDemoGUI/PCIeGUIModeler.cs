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
using System.Timers;

namespace PCIeDemoGUI
{
    public partial class PCIeModeler : Form
    {
        public PCIeModeler()
        {
            InitializeComponent();
            initSimParam();
            IOSizeBox.SelectedIndex = 0;
        }

        string pollWaitTime;
        string numChannel;
        string ioSize;
        string pcieSpeed;
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
        string depthOfQueue;
        int onfiClock;
        string coreMode;
        string wrkloadFile;
        string enableWrkld;
        string removeFiles;
        string cmdTransferTime;
        string validParam;
        string cntrlQueueDepth;
        Process pPCIeNVM;

        List<string> validBlockSize = new List<string>();
        List<string> validIOSize = new List<string>();
        List<string> validQueueDepth = new List<string>();
        List<int> validQDepth = new List<int>();
        List<string> sqDepth = new List<string>();
        List<double> maxLatency = new List<double>();
        List<double> minLatency = new List<double>();
        List<double> avgLatency = new List<double>();
        List<double> avgIOPS = new List<double>();
        List<int> slotNum = new List<int>();
        List<double> latency = new List<double>();


        private void initSimParam()
        {

            pollWaitTime = "5";
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
          
            depthOfQueue = queueDepth;
            coreMode = "0";
            
            cmdTransferTime = "10";
           
            cmdPct = "100";
            cntrlQueueDepth = "32";
            rdBuffSize = "64";
            wrBuffSize = "64";
            wrkloadFile = " ./Workload.txt";
            enableWrkld = "0";
            removeFiles = " 1";
            validBlockSize.Clear();
            validQueueDepth.Clear();

            cmdType = "2";
            onfiClk = "0";
            enableSeqLBA = "100";
            numSlot = 32;
            cwNum = 8;
            queueSize = 128;
            cwCnt = 16;
            onfiClock = 0;
          
            removeFiles = " 1";
            validParam = "";
          

        }

        #region RUN_SIMULATION_BUTTON_CALLBACK
        private string setPcieSpeed()
        {
            string speed = " 3939";
            
            speed = " " + speed.Trim();
            return speed;
            
        }

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
            //panel1.Enabled = false;
            //label28.Visible = false;
        }

        private int getQueueSize()
        {
            return int.Parse(numChannel) * cwNum * int.Parse(queueFactor);
        }

        private void setParameterString()
        {
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
            credit = " " + credit.Trim();
            readTime = " " + readTime.Trim();
            programTime = " " + programTime.Trim();
            queueFactor = " " + queueFactor.Trim();
            cmdTransferTime = " " + cmdTransferTime.Trim();
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
                   
                string enWrkld = " " + enableWrkld.Trim();
                coreMode = " 0";
                var = numCommands + cntrlQueueDepth + queueFactor + ioSize + emCacheSize + numChannel + pageSize + readTime + programTime + credit + onfiClk
                      + numBanks + numDie + numPages + enMultiSim + coreMode + cmdTransferTime + ioSize + pcieSpeed + sqDepth[queueIndex] + sqDepth[queueIndex] + cmdType + pollWaitTime + queueD[queueIndex] + enWrkld + wrkloadFile
                + enableSeqLBA + cmdPct + wrBuffSize + rdBuffSize + removeFiles + enableLogs;
           
           
            pPCIeNVM = new Process();

            try
            {
                pPCIeNVM.StartInfo.FileName = @".\TeraSPCIeController.exe";
                pPCIeNVM.StartInfo.Arguments = var; // Put your arguments here
                pPCIeNVM.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                pPCIeNVM.StartInfo.UseShellExecute = true;
                pPCIeNVM.Start();
                progressLabel1.Text = "Running...";
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

        private void setSimButtonProgressBar(int param)
        {
            progressLabel1.Text = "Complete...";
            progressBar1.Value = param;
        }

        private void enableLoadGraphButton()
        {
            loadGraphButton.Enabled = true;
        }

        private void multiSimCall()
        {
            cwNum = getCwNum();

            if (cwNum != 0)
            {
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
                    for (int queueIndex = 0; queueIndex < queueD.Count(); queueIndex++)
                    {
                        int qDepthUpperLimit = 256;
                       
                        if ((int.Parse(queueD[queueIndex].Trim()) <= qDepthUpperLimit))
                        {
                            validQueueDepth.Add(queueD[queueIndex].Trim());
                            validParam = validQueueDepth[queueIndex].ToString();
                            validQueueDepth[queueIndex].ToString();
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
        }
              
        #endregion

        #region LOAD_GRAPH_BUTTON_CALLBACK

        private void resetLoadGraphProgressBar()
        {
            progressBar2.Maximum = 5;
            progressBar2.Minimum = 0;
            progressBar2.Value = 0;
        }

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

            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: latency Report", e.Message), e);
            }

        }

        private void iopsCalculation()
        {
            try
            {
                avgIOPS.Clear();
                string fileName = "";
                
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

            catch (IOException e)
            {
                throw new Exception(String.Format("An error occurred while executing the data import: IOPS Report", e.Message), e);
            }
        }

        private void IOPSVsQD()
        {
                   
            iopsVsQDChart.Series.Clear();
            iopsVsQDChart.Series.Add("0");
            iopsVsQDChart.Series[0].IsVisibleInLegend = false;
            iopsVsQDChart.ChartAreas[0].AxisX.IntervalAutoMode = 0;
            iopsVsQDChart.ChartAreas[0].AxisX.Minimum = 1;
            iopsVsQDChart.ChartAreas[0].AxisX.Maximum = 512;
            iopsVsQDChart.ChartAreas[0].AxisX.Title = "QD";
            iopsVsQDChart.ChartAreas[0].AxisY.Title = "IOPS (millions)";

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
                using (FileStream file = new FileStream(@".\Reports_PCIe\IOPSVsQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("QD" + "," + "IOPS(Millions)");
                        
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
                                    iopsVsQDChart.Series[0].Color = Color.Blue;
                                    iopsVsQDChart.Series[0].Points.AddXY(offset, avgIOPS[queueDIndex]);
                                    writer.WriteLine(validQueueDepth[queueDIndex] + "," + avgIOPS[queueDIndex]);
                                    iopsVsQDChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                                    iopsVsQDChart.Series[0].BorderWidth = 3;
                                    iopsVsQDChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                                    iopsVsQDChart.Series[0].MarkerSize = 10;
                                    iopsVsQDChart.Series[0].MarkerColor = Color.Green;
                                }
                                iopsVsQDChart.Annotations.Clear();
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
                                iopsVsQDChart.Annotations.Add(iosize);
                                iopsVsQDChart.Annotations[0].AxisX = iopsVsQDChart.ChartAreas[0].AxisX;
                                iopsVsQDChart.Annotations[0].AxisY = iopsVsQDChart.ChartAreas[0].AxisY;
                                iopsVsQDChart.Annotations[0].AnchorDataPoint = iopsVsQDChart.Series[0].Points[0];
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

        private void LatencyVsQD()
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
                        myLine.Text = string.Concat("IOSize " + ioSize);

                        myLine.ForeColor = Color.Black;
                        myLine.Font = new Font("Arial", 10, FontStyle.Bold); ;
                        myLine.LineWidth = 1;
                        latencyVsQDChart.Annotations.Add(myLine);
                        latencyVsQDChart.Annotations[0].AxisX = latencyVsQDChart.ChartAreas[0].AxisX;
                        latencyVsQDChart.Annotations[0].AxisY = latencyVsQDChart.ChartAreas[0].AxisY;
                        latencyVsQDChart.Annotations[0].AnchorDataPoint = latencyVsQDChart.Series[0].Points[0];
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

        private void LatencyVsIOPSwithQD()
        {
            latencyVsIopsVsQDChart.Series.Clear();

            try
            {
                using (FileStream file = new FileStream(@".\Reports_PCIe\LatencyVsIOPSwithQD.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine("IOPS(Millions)" + "," + "Latency(us)" + "," + "QD");
                       
                           
                            
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

        #endregion

        private void runSimButton_Click_1(object sender, EventArgs e)
        {
            resetButtons();
            pcieSpeed = setPcieSpeed();
            pollWaitTime = " " + pollWaitTime.Trim();
            if (int.Parse(ioSize) < int.Parse(emCacheSize))
            {
                MessageBox.Show("IO Size is less than CW Size, setting both to CW Size");
                ioSize = emCacheSize;
            }
            else
            {
                ioSize = IOSizeBox.Text;
            }

            if (int.Parse(emCacheSize) < int.Parse(pageSize))
            {
                MessageBox.Show("ERROR: Page Size is more than Code Word Size: ABORT SIMULATION!!");
            }
            else
            {
                multiSimCall();
            }
            loadGraphButton.Enabled = true;
        }

        private void IOSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ioSize = IOSizeBox.Text;

            if(ioSize == "512")
            {
                numCommands = "1024";
            }
            else
            {
                numCommands = "256";
            }
        }

        private void loadGraphButton_Click(object sender, EventArgs e)
        {
            resetLoadGraphProgressBar();
            progressLabel1.Text = "Loading...";

            latencyCalculation();
            iopsCalculation();
            IOPSVsQD();
            LatencyVsQD();
            LatencyVsIOPSwithQD();

            progressBar2.Value = 5;
            progressLabel1.Text = "Complete...";

            chartPanel.AutoScroll = true;
        }

        private void crossbarLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {

            this.crossbarLinkLabel.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("http://www.crossbar-inc.com");
        }
    }
}
