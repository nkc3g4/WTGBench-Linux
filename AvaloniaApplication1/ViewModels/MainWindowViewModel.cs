using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiveChartsCore.Defaults;
using System.Collections.ObjectModel;
using Tmds.Linux;
using static Tmds.Linux.LibC;
using System.Text;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace AvaloniaApplication1.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public string Greeting => "Welcome to Avalonia!";
        public string SpeedSeqRead { get => speedSeqRead; set { speedSeqRead = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SpeedSeqRead))); } }
        public string SpeedSeqWrite { get => speedSeqWrite; set { speedSeqWrite = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SpeedSeqWrite))); } }

        public string Speed4kRead { get => speed4kRead; set { speed4kRead = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Speed4kRead))); } }
        public string Speed4kWrite { get => speed4kWrite; set { speed4kWrite = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Speed4kWrite))); } }



        string buttonText = "Start!";

        public string ButtonText
        {
            get => buttonText;
            set
            {
                buttonText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            }
        }

        public string PathInput { get; set; }
        private int progressValue = 0;
        private string speedSeqRead = "0";
        private string speedSeqWrite = "0";
        private string speed4kRead = "0";
        private string speed4kWrite = "0";

        public int ProgressValue { get => progressValue; set { progressValue = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProgressValue))); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindowViewModel()
        {
            Series = new ObservableCollection<ISeries>
        {
            new LineSeries<ObservablePoint>
            {
                Name = "4k Write",
                Values = write4kPointValues,
                Fill = null,
                GeometrySize = 0,

            },new LineSeries<ObservablePoint>
            {
                Name = "4k Read",
                Values = read4kPointValues,
                Fill = null,
                GeometrySize = 0,
            }
        };
        }
        public void ButtonClicked()
        {

            //TestFunc(PathInput);
            //Series[0].Values.
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonClicked)));
            //CreateDummyFile(PathInput, 10240);
            Task.Factory.StartNew(() =>
            {
                FastBenchmark(PathInput, null);
            });
            //Debug.WriteLine(PathInput);
        }
        private void GenerateRandomArray(byte[] rnd_array)
        {
            Random random = new Random();
            for (int i = 0; i < rnd_array.Length; i++)
            {
                rnd_array[i] = (byte)random.Next(255);
            }
        }
        private void FastBenchmark(string diskRootPath, object ctobj)
        {
            //chart1.Invoke(new Action(() =>
            //{
            //    chart1.Series[0].Points.Clear();
            //}));
            double adj4k = 0;
            double speedWriteSeq = 0;
            Task t1 = Task.Factory.StartNew(() =>
            {
                speedWriteSeq = WriteSeq(diskRootPath + "test.bin", ctobj);
                SpeedSeqWrite = speedWriteSeq.ToString("f4");
            });
            t1.Wait();
            Task t2 = Task.Factory.StartNew(() =>
            {
                double speedReadeq = ReadSeq(diskRootPath + "test.bin", ctobj);
                SpeedSeqRead = speedReadeq.ToString("f4");
            });
            t2.Wait();

            double speedWrite4k = Write4k(diskRootPath + "test.bin", ref adj4k, ctobj);
            Speed4kWrite = speedWrite4k.ToString("f4");

            double speedRead4k = Read4k(diskRootPath + "test.bin", ref adj4k, ctobj);
            Speed4kRead = speedRead4k.ToString("f4");

            File.Delete(diskRootPath + "test.bin");

            double score = adj4k + Math.Log(1 + (speedWriteSeq / 1000));
            int lv = 0;
            if (score > 30)
            {
                lv = 5;
            }
            else if (score > 10)
            {
                lv = 4;
            }
            else if (score > 0.8)
            {
                lv = 3;
            }
            else if (score > 0.3)
            {
                lv = 2;
            }
            else
            {
                lv = 1;
            }

            string ln = "Error";
            //Color lc = Color.Yellow;
            //Image L1 = Properties.Resources.grey;
            //Image L2 = Properties.Resources.grey;
            //Image L3 = Properties.Resources.grey;
            //Image L4 = Properties.Resources.grey;
            //if (lv == 1)
            //{
            //    ln = "Steel";
            //    lc = Color.SteelBlue;

            //}
            //else if (lv == 2)
            //{
            //    ln = "Bronze";
            //    lc = Color.Crimson;
            //    L1 = Properties.Resources.orange;
            //}
            //else if (lv == 3)
            //{
            //    ln = "Silver";
            //    lc = Color.Silver;
            //    L1 = Properties.Resources.orange;
            //    L2 = Properties.Resources.orange;
            //}
            //else if (lv == 4)
            //{
            //    ln = "Gold";
            //    lc = Color.Gold;
            //    L1 = Properties.Resources.orange;
            //    L2 = Properties.Resources.orange;
            //    L3 = Properties.Resources.orange;
            //}
            //else if (lv == 5)
            //{
            //    ln = "Platinum";
            //    lc = Color.White;
            //    L1 = Properties.Resources.orange;
            //    L2 = Properties.Resources.orange;
            //    L3 = Properties.Resources.orange;
            //    L4 = Properties.Resources.orange;
            //}
            //labelLevel.Invoke(new Action(() =>
            //{
            //    labelLevel.Text = ln;
            //    labelLevel.ForeColor = lc;
            //    labelLevel.Visible = true;
            //}));

            //pictureBoxL1.Invoke(new Action(() =>
            //{
            //    pictureBoxL1.Image = L1;
            //    pictureBoxL1.Visible = true;
            //}));
            //pictureBoxL2.Invoke(new Action(() =>
            //{
            //    pictureBoxL2.Image = L2;
            //    pictureBoxL2.Visible = true;
            //}));
            //pictureBoxL3.Invoke(new Action(() =>
            //{
            //    pictureBoxL3.Image = L3;
            //    pictureBoxL3.Visible = true;
            //}));
            //pictureBoxL4.Invoke(new Action(() =>
            //{
            //    pictureBoxL4.Image = L4;
            //    pictureBoxL4.Visible = true;
            //}));

        }
        private double WriteSeq(string path, object ctobj)
        {

            //CancellationToken token = (CancellationToken)ctobj;

            FileInfo fileInfo = new FileInfo(path);
            fileInfo.Delete();

            Thread.Sleep(1000);

            List<double> seqPoints = new List<double>();

            using (FileStream fileStream = new FileStream(path, new FileStreamOptions() { Options = FileOptions.WriteThrough, Access = FileAccess.Write, Mode = FileMode.Create }))
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int seqSize = 67108864;
                byte[] seqBuffer = new byte[seqSize];
                long testDuration = 10 * 1000;
                GenerateRandomArray(seqBuffer);

                for (long dataLength = 0L; dataLength < 10737418240; dataLength += seqSize)
                {

                    //if (token.IsCancellationRequested)
                    //{
                    //    break;
                    //}
                    fileStream.Position = dataLength;

                    long preTime = sw.ElapsedMilliseconds;
                    fileStream.Write(seqBuffer, 0, seqSize);
                    fileStream.Flush();
                    long curTime = sw.ElapsedMilliseconds;
                    seqPoints.Add((seqSize / (1024.0 * 1024)) / ((curTime - preTime) / 1000.0));
                    int val = (int)((sw.ElapsedMilliseconds / (double)testDuration) * 100);
                    ProgressValue = val <= 100 ? val : 100;
                    if ((dataLength / seqSize) % 100 == 0)
                    {
                        GenerateRandomArray(seqBuffer);
                    }

                    //if (sw.ElapsedMilliseconds > testDuration)
                    //    break;
                }
                sw.Stop();

            }

            return seqPoints.Average();
        }


        private unsafe double ReadSeq(string path, object ctobj)
        {
            Thread.Sleep(3000);
            Stopwatch sw = new Stopwatch();

            int seqSize = 524288;
            //byte[] seqBuffer = new byte[seqSize];
            List<double> seqPoints = new List<double>();
            long testDuration = 10 * 1000;
            long dataLength = 0L;

            var bytes = Encoding.UTF8.GetBytes(path);

            //CancellationToken token = (CancellationToken)ctobj;
            fixed (byte* bytesPath = bytes)
            {

                int fd = open(bytesPath, O_RDONLY | O_DIRECT);
                //byte* seqBuffer = stackalloc byte[seqSize];
                byte* seqBuffer = (byte*)NativeMemory.AlignedAlloc((nuint)seqSize, 512);
                sw.Start();
                for (; dataLength < 10737418240; dataLength += seqSize)
                {
                    ssize_t readRes = read(fd, seqBuffer, seqSize);
                    //ButtonText = fd.ToString();

                    lseek(fd, dataLength, SEEK_SET);

                    int val = (int)((sw.ElapsedMilliseconds / (double)testDuration) * 100);
                    ProgressValue = val <= 100 ? val : 100;
                    if (sw.ElapsedMilliseconds > testDuration)
                        break;
                }
                sw.Stop();
                close(fd);

            }

            return (dataLength / (1024.0 * 1024)) / ((sw.ElapsedMilliseconds) / 1000.0);
        }

        private double Write4k(string path, ref double adjustResult, object ctsobj)
        {

            //CancellationToken token = (CancellationToken)ctsobj;


            Random random = new Random();
            byte[] buffer = new byte[4096];
            GenerateRandomArray(buffer);

            long dataLength = 536866816L;
            FileStream fileStream = new FileStream(path, new FileStreamOptions() { Options = FileOptions.WriteThrough, Access = FileAccess.Write, Mode = FileMode.Open, BufferSize = 0 });
            Thread.Sleep(5000);
            //progressBar1.Invoke(new Action(() =>
            //{
            //    progressBar1.Style = ProgressBarStyle.Blocks;
            //}));
            List<double> testPoints = new List<double>();
            Stopwatch temp_timer = new Stopwatch();
            temp_timer.Start();
            long previousTime = 0L;
            long prenum = 0L;
            // int loopTimes = 30;
            int LoopTime4k = 30;
            for (long num = 0L; testPoints.Count < LoopTime4k; num += 1L)
            {
                //if (token.IsCancellationRequested)
                //{
                //    break;
                //}
                long num2 = random.Next((int)(dataLength / 4096 + 1));

                long curTime = temp_timer.ElapsedMilliseconds;
                if (curTime - previousTime > 500)
                {
                    double curSpeed = ((num - prenum) * 4096.0 / 1024 / 1024) / ((curTime - previousTime) / 1000.0);
                    write4kPointValues.Add(new ObservablePoint(testPoints.Count / 2.0, curSpeed));
                    testPoints.Add(curSpeed);
                    prenum = num;
                    //progressBar1.Invoke(new Action(() =>
                    //{
                    //    progressBar1.Value = (int)(testPoints.Count / (double)LoopTime4k * 100.0);
                    //}));

                    previousTime = temp_timer.ElapsedMilliseconds;
                }
                fileStream.Position = num2 * 4096;
                fileStream.Write(buffer, 0, 4096);
                fileStream.Flush();
            }

            fileStream.Close();
            temp_timer.Stop();
            double avg = testPoints.Average();
            testPoints.AddRange(testPoints.GetRange(LoopTime4k / 2, LoopTime4k / 2));

            int cnt = testPoints.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (testPoints[i] < avg * 0.5)
                {
                    testPoints.Add(testPoints[i]);
                }
            }
            adjustResult = testPoints.Average();
            return avg;
        }
        private unsafe double Read4k(string path, ref double adjustResult, object ctsobj)
        {

            //CancellationToken token = (CancellationToken)ctsobj;


            Random random = new Random();
            byte[] buffer = new byte[4096];
            //GenerateRandomArray(buffer);

            long dataLength = 536866816L;
            //Create 
            //CreateDummyFile(path, dataLength);
            Thread.Sleep(5000);
            //progressBar1.Invoke(new Action(() =>
            //{
            //    progressBar1.Style = ProgressBarStyle.Blocks;
            //}));
            List<double> testPoints = new List<double>();
            Stopwatch sw = new Stopwatch();
            long previousTime = 0L;
            long prenum = 0L;
            // int loopTimes = 30;
            int LoopTime4k = 30;



            var bytes = Encoding.UTF8.GetBytes(path);

            //CancellationToken token = (CancellationToken)ctobj;
            fixed (byte* bytesPath = bytes)
            {

                int fd = open(bytesPath, O_RDONLY | O_DIRECT);
                //byte* seqBuffer = stackalloc byte[4096 + 512];
                //byte* aligned_buf = (byte*)(((uint)seqBuffer + 512 - 1) / 512 *512);
                byte* aligned_buf = (byte*)NativeMemory.AlignedAlloc(4096, 512);
                sw.Start();
                for (long num = 0L; testPoints.Count < LoopTime4k; num += 1L)
                {

                    long randomPos = random.Next((int)(dataLength / 4096 + 1));

                    long curTime = sw.ElapsedMilliseconds;
                    if (curTime - previousTime > 500)
                    {
                        double curSpeed = ((num - prenum) * 4096.0 / 1024 / 1024) / ((curTime - previousTime) / 1000.0);
                        read4kPointValues.Add(new ObservablePoint(testPoints.Count / 2.0, curSpeed));
                        testPoints.Add(curSpeed);
                        prenum = num;

                        previousTime = sw.ElapsedMilliseconds;
                    }
                    lseek(fd, randomPos * 4096, SEEK_SET);
                    ssize_t readRes = read(fd, aligned_buf, 4096);
                    //ButtonText = readRes.ToString();
                }
                sw.Stop();
                close(fd);

            }

            double avg = testPoints.Average();
            testPoints.AddRange(testPoints.GetRange(LoopTime4k / 2, LoopTime4k / 2));

            int cnt = testPoints.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (testPoints[i] < avg * 0.5)
                {
                    testPoints.Add(testPoints[i]);
                }
            }
            adjustResult = testPoints.Average();
            return avg;
        }

        private readonly ObservableCollection<ObservablePoint> write4kPointValues = new ObservableCollection<ObservablePoint>();
        private readonly ObservableCollection<ObservablePoint> read4kPointValues = new ObservableCollection<ObservablePoint>();

        public ObservableCollection<ISeries> Series { get; set; }


        unsafe void TestFunc(string path)
        {
            var bytes = Encoding.UTF8.GetBytes(path + "/test.bin");

            fixed (byte* buffer = bytes)
            {
                byte[] seqBuffer = new byte[16777216];
                fixed (byte* buf2 = seqBuffer)
                {
                    int fd = open(buffer, O_RDONLY);
                    read(fd, buf2, 1048576);
                    close(fd);
                }


                //write(STDOUT_FILENO, buffer, bytes.Length);
            }
            ButtonText = "FIN";
        }

    }
}
