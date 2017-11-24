using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using GL = OpenTK.Graphics.OpenGL.GL;
using BeginMode = OpenTK.Graphics.OpenGL.BeginMode;
using ClearBufferMask = OpenTK.Graphics.OpenGL.ClearBufferMask;
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;
using MatrixMode = OpenTK.Graphics.OpenGL.MatrixMode;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;
using TextureTarget = OpenTK.Graphics.OpenGL.TextureTarget;
using NAudio;
using NAudio.Wave;
//using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        double TR = 0.8; //прозрачность
        //Переменные для работы с буфером
        private static int _fftLength = 1024; // NAudio fft wants powers of two!
        private IWaveIn _waveIn;
        private SampleAggregator _sampleAggregator = new SampleAggregator(_fftLength);
        private IWavePlayer _waveOutDevice;
        private AudioFileReader _audioFileReader;
        // Массив со спектром
        private List<double> _FFTData = new List<double>(_fftLength);
        private List<double> _FFTDataOld = new List<double>(_fftLength);
        // Доступность массива
        private bool _FFTDataAvailable = false;
        //Доступен ли OpenGl
        private bool _glLoaded = false;
        //Прорисован ли waveForm
        private bool _drawed = false;
        //waveform 
        private List<double> _waveformPoints = new List<double>();
        private List<double> _waveformPointsMiliseconds = new List<double>();
        private List<double> _currentWaveFormList;
        private int _currentwaveFormDecade = 0;
        //время тика
        static private int _timeTick = 30;
        //Таймеры
        private System.Timers.Timer _waveformTmr; //waveform timer
        private System.Timers.Timer _FFTTimer;
        //Цвет музыки
        private double _red = 128;
        private double _blue = 128;
        private double _green = 128;
        private double _total = 0;
        private double _fonStrong = 0;
        private double _hue = 0;
        private double _different = 0;
        //Тип заполенения
        PolygonMode mode = PolygonMode.Fill;
        //Круговой график
        private GraphMain Visual1;
        //private GraphMain Visual2;
        //Текстуры для графиков
        Bitmap[] bmpTex = new Bitmap[5];
        //Фоновый цвет музыки
        private Color _soundColor = Color.AliceBlue;
        // порядковые номера определенных участков спектра при прорисовке
        private int[] _FFTnum = new int[50];
        //Сила спектра в определенный момент времени
        private double[] _FFTSpectr = new double[5];
        // Набор цветов для музыкальных кругов
        Color firstColorSpectr = Color.Black; //внутренний цвет
        private double _speed = 1.0;
        private static Color[] secondColorSpectr =
            {Color.Red, Color.Purple, Color.Yellow, Color.Lime, Color.DarkBlue };
        //радиус определенного спектра на музыкальном круге
        double[] spectrRadius = new double[5];
        //Делитель силы спектра
        private double _fftPart = 3;
        //конструктор формы
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Начало обратоки данных из буфера
            _sampleAggregator.FftCalculated += new EventHandler<FftEventArgs>(FftCalculated);
            _sampleAggregator.PerformFFT = true;
            _waveIn = new WasapiLoopbackCapture();
            _waveIn.DataAvailable += OnDataAvailable;
            _waveIn.StartRecording();
            //Создание таймеров
            _waveformTmr = new System.Timers.Timer();
            _waveformTmr.Interval = _timeTick;
            _waveformTmr.Elapsed += WaveformTick;
            _FFTTimer = new System.Timers.Timer();
            _FFTTimer.Interval = _timeTick;
            _FFTTimer.Elapsed += FFTTimerTick;
            _FFTTimer.Start();
            //настройка OpenGl
            SetupViewport();
            GL.ClearColor(1f, 1f, 1f, 1f); // цвет фона
            GL.Enable(EnableCap.DepthTest);
            //задать позиции графиков FFT
            Calculation.setFFtNum(_FFTnum);
            //Загрузка текстур для графика FFT
            for (int i = 0; i < 5; i++)
                bmpTex[i] = new Bitmap((i + 1) + ".bmp");
            //Задание маленького начального радиуса
            for (int i = 0; i < 5; i++)
                spectrRadius[i] = 1;
            // заполение коллекции
            for (int i = 0; i < 1024; i++)
                _FFTDataOld.Add(0);
        }

        private void WaveformTick(object sender, ElapsedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                label1.Text = _audioFileReader.CurrentTime.TotalMilliseconds.ToString();

                if (_currentWaveFormList.Count > 0)
                    textBox1.Text = (_currentWaveFormList[10]).ToString();

                if (_currentwaveFormDecade * 10000 < _audioFileReader.CurrentTime.TotalMilliseconds)
                {
                    _currentwaveFormDecade++;
                    DataGraph.HandleData(ref _currentWaveFormList, _waveformPointsMiliseconds, currentWaveFormBox.Width, (_currentwaveFormDecade - 1) * 10000, 10000);
                }

                timeBox.Refresh();
                currentWaveFormBox.Refresh();
            });
        }

        private void FFTTimerTick(object sender, ElapsedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                double red = 0;
                double blue = 0;
                double green = 0;
                double total = 0;
                double different = 0;

                if (_FFTDataAvailable)
                {
                    // double _hue = 0.2
                    double saturation = 0, value = 0;
                    //Просчет спектров для графика
                    if (_FFTDataOld.Capacity > 1020)
                        for (int i = 0; i < 1020; i++)
                        {
                            different += 100 * Math.Abs(_FFTData[i] - _FFTDataOld[i]);
                            //_FFTDataOld[i] = _FFTData[i];
                        }

                    if (different > 50)
                        different = 50;

                    different /= 50;

                    _different = _different * 3 / 4 + different / 4;

                    _FFTDataOld.Clear();

                    for (int i = 0; i < 1024; i++)
                        _FFTDataOld.Add(_FFTData[i]);

                    for (int i = 0; i < 5; i++)
                        _FFTSpectr[i] = 0;

                    //for (int i = 0; i < 40; i++)
                    //    _FFTSpectr[0] += _FFTData[i];
                    //for (int i = 40; i < 100; i++)
                    //    _FFTSpectr[1] += _FFTData[i];
                    //for (int i = 100; i < 175; i++)
                    //    _FFTSpectr[2] += _FFTData[i];
                    //for (int i = 175; i < 300; i++)
                    //    _FFTSpectr[3] += _FFTData[i];
                    //for (int i = 300; i < 1024; i++)
                    //    _FFTSpectr[4] += _FFTData[i];
                    for (int i = 0; i < 30; i++)
                        _FFTSpectr[0] += _FFTData[i];
                    for (int i = 30; i < 100; i++)
                        _FFTSpectr[1] += _FFTData[i];
                    for (int i = 100; i < 400; i++)
                        _FFTSpectr[2] += _FFTData[i];
                    for (int i = 400; i < 900; i++)
                        _FFTSpectr[3] += _FFTData[i];
                    for (int i = 850; i < 960; i++)
                        _FFTSpectr[4] += _FFTData[i];


                    double total2 = 0;
                    for (int i = 0; i < 5; i++)
                        total2 += _FFTSpectr[i];

                    for (int i = 0; i < 5; i++)
                        _FFTSpectr[i] = _FFTSpectr[i] / _fftPart;

                    //_hue -= _FFTSpectr[0] + _FFTSpectr[1];
                    //_hue += _FFTSpectr[2] + _FFTSpectr[3];

                    if (_hue > 1)
                        _hue--;
                    if (_hue < 0)
                        _hue++;

                    //Конец просчета спектров для графика
                    //просчет цвета
                    for (int i = 0; i < 300; i++)
                    {
                        total += _FFTData[i];
                        red += _FFTData[i];
                    }
                    for (int i = 301; i < 1024; i++)
                    {
                        green += _FFTData[i];
                        total += _FFTData[i];
                    }
                    red = red / total;
                    blue = 1 - red;

                    _total = total * 100 / (double)(numericUpDown1.Value * 2 / 3) * 450 / (double)(numericUpDown1.Value);

                    _hue += 0.01 * _total;

                    _red = HSVtoRGB(_hue, _total, _total * 0.5 + 0.5).R;
                    _blue = HSVtoRGB(_hue, _total, _total * 0.5 + 0.5).B;
                    _green = HSVtoRGB(_hue, _total, _total * 0.5 + 0.5).G;

                    Visual1.AddPoint(_total, _soundColor, _different, _speed);
                    total = total * 100 / (double)(numericUpDown1.Value);
                    _soundColor = Color.FromArgb((byte)((_red + 3 * _soundColor.R) / 4),
                        (byte)((_green + 3 * _soundColor.G) / 4), (byte)((_blue + 3 * _soundColor.B) / 4));

                    //конец просчета цвета
                    //Вывод информации на текстбок
                    //richTextBox1.Text = "Red" + (int)(_red) + "\n\r" +
                    //"Green" + (int)(_green) + "\n\r" +
                    //"Blue" + (int)(_blue) + "\n\r" +
                    //"Total" + (int)(_total * 100) + "\n\r" +
                    //"Defferent" + (int)(_different * 100) + "\n\r";



                    //richTextBox1.Text = //"Red" + (int) (_red) + "\n\r" +
                    //                    //"Green" + (int) (_green) + "\n\r" +
                    //                    //"Blue" + (int) (_blue) + "\n\r" +
                    //                    // "Total" + (int) (_total * 100) + "\n\r" +
                    //                    // "Defferent" + (int) (_different * 100) + "\n\r" +
                    //                    "0   " + (int) (_FFTSpectr[0] * 100) + "\n\r" +
                    //                    "1   " + (int) (_FFTSpectr[1] * 100) + "\n\r" +
                    //                    "2   " + (int) (_FFTSpectr[2] * 100) + "\n\r" +
                    //                    "3   " + (int) (_FFTSpectr[3] * 100) + "\n\r" +
                    //                    "4   " + (int) (_FFTSpectr[4] * 100) + "\n\r";


                }
                //Просчет новой точке на музыкальном круге
                FFTBox.Refresh();
                glControl1.Refresh();
            });
        }

        private void StartVisualization(object sender, EventArgs e)
        {
            WaveChannel32 wave = new WaveChannel32(new Mp3FileReader("test.mp3"));
            // WaveChannel32 wave = new WaveChannel32(new Mp3FileReader("C:\\Users\\Kapi\\Desktop\\test2.mp3"));
            int sampleSize = 1024;
            var bufferSize = 16384 * sampleSize;
            bufferSize = 1024;
            var buffer = new byte[bufferSize];
            int read = 0;

            while (wave.Position < wave.Length)
            {
                read = wave.Read(buffer, 0, bufferSize);
                for (int i = 0; i < read / sampleSize; i++)
                {
                    var point = BitConverter.ToSingle(buffer, i * sampleSize);
                    _waveformPoints.Add(point);
                }
            }



            _waveOutDevice = new WaveOut();
            _audioFileReader = new AudioFileReader("test.mp3");
            //_audioFileReader = new AudioFileReader("C:\\Users\\Kapi\\Desktop\\test2.mp3");

            for (int i = 0; i < _audioFileReader.TotalTime.TotalMilliseconds; i++)
            {
                _waveformPointsMiliseconds.Add(_waveformPoints[(int)((double)i / (_audioFileReader.TotalTime.TotalMilliseconds) * _waveformPoints.Count)]);
            }

            _waveOutDevice.Init(_audioFileReader);
            _waveOutDevice.Play();
            _waveformTmr.Start();
            label2.Text = _audioFileReader.TotalTime.TotalMilliseconds.ToString();
            waveFormBox.Refresh();

        }

        private void WaveFormBoxPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black);

            if (_waveformPoints.Count == 0)
                return;

            if (!_drawed)
            {
                for (int i = 0; i < _waveformPoints.Count - 1; i++)
                    g.DrawLine(p, (float)i / 5, (float)_waveformPoints[i] * 100 + 125, ((float)((i + 1) / 5)),
                        (float)_waveformPoints[i + 1] * 100 + 125);

                _drawed = true;
            }
        }

        private void TimeBoxPaint(object sender, PaintEventArgs e)
        {
            if (_waveformPoints.Count == 0)
                return;
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black);
            p.Color = Color.Red;
            g.DrawLine(p,
                _waveformPoints.Count * ((float)_audioFileReader.CurrentTime.TotalMilliseconds /
                (float)_audioFileReader.TotalTime.TotalMilliseconds) / 5, 0,
              _waveformPoints.Count * ((float)_audioFileReader.CurrentTime.TotalMilliseconds /
              (float)_audioFileReader.TotalTime.TotalMilliseconds) / 5, 50);
        }

        private void currentWaveFormBoxPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black);

            if (_currentWaveFormList.Count == 0)
                return;

            for (int i = 0; i < _currentWaveFormList.Count - 1; i++)
                g.DrawLine(p, (float)i, -(float)_currentWaveFormList[i] * 5 + 125, (float)i + 1,
                    -(float)_currentWaveFormList[i + 1] * 5 + 125);

            g.DrawLine(p, 0, 125, currentWaveFormBox.Width, 125);

            p.Color = Color.Red;
            g.DrawLine(p, (float)((double)((int)_audioFileReader.CurrentTime.TotalMilliseconds % 10000) / 10000 * currentWaveFormBox.Width), 0,
                (float)((double)((int)_audioFileReader.CurrentTime.TotalMilliseconds % 10000) / 10000 * currentWaveFormBox.Width), currentWaveFormBox.Height);
        }

        class FFTPaint
        {
            private Thread thread;
            class DataFFT
            {
                public PaintEventArgs e;
                public int Width;
                public List<double> _FFTData;
            }
            public FFTPaint(PaintEventArgs e, int Width, List<double> _FFTData)
            {
                thread = new Thread(this.paint);
                thread.Name = "paint";
                DataFFT data = new DataFFT();
                data.e = e;
                data.Width = Width;
                data._FFTData = _FFTData;
                //thread.Start(num);//передача параметра в поток
                thread.Start(data);//передача параметра в поток
            }

            void paint(object data)
            {

                Graphics g = ((DataFFT)data).e.Graphics;
                Pen p = new Pen(Color.Black);

                for (int i = 0; i < (((DataFFT)data)._FFTData.Count - 1) / 2; i++)
                    g.DrawLine(p,
                        ((float)i) / (((DataFFT)data)._FFTData.Count - 1) * 2 * ((DataFFT)data).Width,
                        -(float)Math.Sqrt(Math.Abs(((DataFFT)data)._FFTData[i])) * 250 + 125,
                        ((float)i + 1) / (((DataFFT)data)._FFTData.Count - 1) * 2 * ((DataFFT)data).Width,
                        -(float)Math.Sqrt(Math.Abs(((DataFFT)data)._FFTData[i + 1])) * 250 + 125);

                p.Color = Color.Red;
                g.DrawLine(p, 0, 325, ((DataFFT)data).Width, 325);
            }

        }

        public delegate void MyDelegate(PaintEventArgs e);

        public void FFTBoxPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black);

            for (int i = 0; i < (_FFTData.Count - 1) / 2; i++)
                g.DrawLine(p,
                    ((float)i) / (_FFTData.Count - 1) * 2 * FFTBox.Width,
                    -(float)Math.Sqrt(Math.Abs(_FFTData[i])) * 250 + 125,
                    ((float)i + 1) / (_FFTData.Count - 1) * 2 * FFTBox.Width,
                    -(float)Math.Sqrt(Math.Abs(_FFTData[i + 1])) * 250 + 125);

            p.Color = Color.Red;
            g.DrawLine(p, 0, 325, FFTBox.Width, 325);
        }



        private void GLLoad(object sender, EventArgs e)
        {
            _glLoaded = true;
            GL.ClearColor(Color.White);
            SetupViewport();
            _currentWaveFormList = new List<double>();
        }

        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;

            Visual1 = new GraphMain(w, h);
            //Visual2 = new GraphMain(w, h);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-w, w, -h, h, -100, 100);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.Viewport(0, 0, w, h);
        }

        private void GlPaint(object sender, PaintEventArgs e)
        {
            if (!_glLoaded)
                return;
            // Color fonColor = Color.FromArgb(_soundColor.R, _soundColor.B, 0);
            _fonStrong = ((_fonStrong * 3) + _total) / 4;
            if (_fonStrong > 1)
                _fonStrong = 1;

            Color fonColor = Color.FromArgb((int)(255 * _fonStrong),
               (int)(255 * _fonStrong), (int)(255 * _fonStrong * 0.2));
            GL.ClearColor(fonColor);
            // GL.ClearColor(1f, 1f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            //Прорисoвка круга
            if (Visual1.Colors.Count >= 10)
            {
                Color spin = new Color();
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Color3(Visual1.Colors[0]);
                GL.Color4(Visual1.Colors[0].R, Visual1.Colors[0].G, Visual1.Colors[0].B, (double)0.5);
                GL.Color4(Color.FromArgb(Visual1.Colors[0].A*0, Visual1.Colors[0].R, Visual1.Colors[0].G, Visual1.Colors[0].B));
                GL.Vertex3(-0, -0, -1);
                GL.Vertex3(Visual1.Coordinates[0].X, Visual1.Coordinates[0].Y, -1);
                for (int i = 0; i < Visual1.Coordinates.Count; i++)
                {
                    if ((i % 4 == 0) && (i > 0))
                    {
                        GL.End();

                        GL.Begin(PrimitiveType.TriangleFan);
                        GL.Vertex3(-0, -0, (float)((double)i / Visual1.Coordinates.Count / 2 - 1));
                        for (int j = i - 3; j < i; j++)
                        {
                            GL.Vertex3(-Visual1.Coordinates[j - 1].X, -Visual1.Coordinates[j - 1].Y,
                                (float)((double)j / Visual1.Coordinates.Count / 2 - 1));
                        }
                        GL.End();

                        GL.Begin(PrimitiveType.TriangleFan);
                        GL.Vertex3(-0, -0, (float)((double)i / Visual1.Coordinates.Count / 2 - 1));
                        GL.Vertex3(Visual1.Coordinates[i - 1].X, Visual1.Coordinates[i - 1].Y,
                            (float)((double)i / Visual1.Coordinates.Count / 2 - 1));
                    }

                    GL.Color3(Visual1.Colors[i]);
                    GL.Color4(Visual1.Colors[i].R, Visual1.Colors[i].G, Visual1.Colors[i].B, (double)0.5);
                    GL.Color4(Color.FromArgb((int)((double)i/Visual1.Coordinates.Count*TR * 255), Visual1.Colors[i].R, Visual1.Colors[i].G, Visual1.Colors[i].B));

                    GL.Vertex3(Visual1.Coordinates[i].X, Visual1.Coordinates[i].Y,
                        (float)((double)i / Visual1.Coordinates.Count / 2 - 1));
                }
                GL.End();
            }

            //////конец прорисовки круга

            float[] diffuse = new float[3] { 0.5f, 0.5f, 0.5f };
            GL.Light(LightName.Light0, LightParameter.Diffuse, diffuse);

            float[] ambient = new float[3] { 4, 4, 4 };
            GL.Light(LightName.Light0, LightParameter.Ambient, ambient);

            float[] lightPos = new float[4] { -glControl1.Width, 0.0f, 10.0f, 1.0f };
            GL.Light(LightName.Light0, LightParameter.Position, lightPos);

            float[] specular = new float[4] { 10, 10, 10, 1};
            GL.Light(LightName.Light0, LightParameter.Specular, specular);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);

            //звуковой спектр снизу в виде диаграмм
            Random rand = new Random();
            for (int color = 0; color < 5; color++)
            {
                LoadTexture(bmpTex[color]);
                for (int j = 0; j < 50; j++)
                {
                    if (_FFTnum[j] - 1 != color)
                        continue;
                    GL.Enable(EnableCap.Texture2D);

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                        (int)TextureMinFilter.Nearest);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                        (int)TextureMagFilter.Linear);

                    GL.PolygonMode(MaterialFace.FrontAndBack, mode);
                    GL.Begin(PrimitiveType.QuadStrip);
                    double w = glControl1.Width / 25;

                    float k = 0.5f;
                    double r = 0.5;
                    int n = 20;
                    for (int i = 0; i <= n; ++i)
                    {
                        double a = Math.PI / n * i;
                        double x = r * Math.Cos(a);
                        double z = r * Math.Sin(a);
                        GL.TexCoord2(0, 1);
                        GL.Vertex3(x * w + w * j + (w / 2) - glControl1.Width,
                            -glControl1.Height * (1 - _FFTSpectr[_FFTnum[j] - 1]), z);
                        GL.TexCoord2(1, 0);
                        GL.Vertex3(x * w + w * j + (w / 2) - glControl1.Width, -glControl1.Height, z);

                        if (i > 0)
                            GL.Normal3(-(x - r * Math.Cos(a - 1)), (x - r * Math.Cos(a - 1)), 0);
                    }
                    GL.End();
                }
            }

            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);

            //myThread t1 = new myThread("Thread 1", glControl1.Width, glControl1.Height, firstColorSpectr, spectrRadius,
            //    _FFTSpectr, glControl1);
            //myThread t2 = new myThread("Thread 1", -glControl1.Width, glControl1.Height, firstColorSpectr, spectrRadius,
            //    _FFTSpectr, glControl1);
            //myThread t3 = new myThread("Thread 1", glControl1.Width, -glControl1.Height, firstColorSpectr, spectrRadius,
            //    _FFTSpectr, glControl1);
            //myThread t4 = new myThread("Thread 1", -glControl1.Width, -glControl1.Height, firstColorSpectr, spectrRadius,
            //    _FFTSpectr, glControl1);



            MusicCirsle(glControl1.Width, glControl1.Height, Math.PI, 3 * Math.PI / 2);
            MusicCirsle(-glControl1.Width, glControl1.Height, 3 * Math.PI / 2, 2 * Math.PI);
            MusicCirsle(glControl1.Width, -glControl1.Height, Math.PI / 2, 2 * Math.PI);
            MusicCirsle(-glControl1.Width, -glControl1.Height, 0, Math.PI / 2);

            //GL.Disable(EnableCap.Lighting);



            GL.Disable(EnableCap.Blend);

            GL.Flush();
            //GL.Finish();

            glControl1.SwapBuffers();
        }

        class myThread
        {
            Thread thread;


            class tData
            {
                public double x;
                public double y;
                public Color firstColorSpectr;
                public double[] spectrRadius;
                public double[] _FFTSpectr;
                public OpenTK.GLControl glControl1;
            }

            public myThread(string name, double x, double y, Color firstColorSpectr, double[] spectrRadius, double[] _FFTSpectr, OpenTK.GLControl glControl1) //Конструктор получает имя функции и номер до кторого ведется счет
            {

                thread = new Thread(this.MusicCirsle);
                thread.Name = name;
                tData data = new tData();
                data.x = x;
                data.y = y;
                data.firstColorSpectr = firstColorSpectr;
                data.spectrRadius = spectrRadius;
                data._FFTSpectr = _FFTSpectr;
                data.glControl1 = glControl1;
                //thread.Start(num);//передача параметра в поток
                thread.Start(data);//передача параметра в поток
            }

            void func(object num)//Функция потока, передаем параметр
            {
                for (int i = 0; i < (int)num; i++)
                {
                    Console.WriteLine(Thread.CurrentThread.Name + " выводит " + i);
                    Thread.Sleep(0);
                }
                Console.WriteLine(Thread.CurrentThread.Name + " завершился");
            }
            void abc(object num)//Функция потока, передаем параметр
            {
                for (int i = 0; i < (int)num; i++)
                {
                    Console.WriteLine(Thread.CurrentThread.Name + " выводит " + i);
                    Thread.Sleep(0);
                }
                Console.WriteLine(Thread.CurrentThread.Name + " завершился");
            }

            void MusicCirsle(object data)
            {
                for (int j = 0; j < 5; j++)
                {
                    GL.Begin(PrimitiveType.TriangleFan);
                    //GL.Color3(firstColorSpectr);
                    GL.Color4(((tData)data).firstColorSpectr.R, ((tData)data).firstColorSpectr.G, ((tData)data).firstColorSpectr.B, 0.8);

                    // spectrRadius[j] *= 3;
                    if (j == 0)
                        ((tData)data).spectrRadius[j] += ((tData)data)._FFTSpectr[j] * ((tData)data).glControl1.Width / 3;
                    else
                        ((tData)data).spectrRadius[j] += ((tData)data)._FFTSpectr[j] * ((tData)data).glControl1.Width / Math.Sqrt(j + 10) + ((tData)data).spectrRadius[j - 1];
                    ((tData)data).spectrRadius[j] /= 2;

                    GL.Vertex3(((tData)data).x, ((tData)data).y, (double)(1 - 0.1 + j / 100));
                    for (double i = 0; i < 2 * Math.PI; i += 0.01)
                    {
                        //GL.Color3(secondColorSpectr[j]);
                        GL.Color4(secondColorSpectr[j].R, secondColorSpectr[j].G, secondColorSpectr[j].B, 0.8);
                        GL.Vertex3(((tData)data).spectrRadius[j] * Math.Cos(i) + ((tData)data).x,
                            ((tData)data).spectrRadius[j] * Math.Sin(i) + ((tData)data).y, (double)(1 - 0.1 - j / 100));
                        GL.Vertex3(((tData)data).spectrRadius[j] * Math.Cos(i + 0.01) + ((tData)data).x,
                            ((tData)data).spectrRadius[j] * Math.Sin(i + 0.01) + ((tData)data).y, (double)(1 - 0.1 - j / 100));
                    }
                    GL.End();
                }
                Console.WriteLine("asdf");

                GL.Begin(PrimitiveType.Triangles);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(100, 0, 0);
                GL.Vertex3(0, 100, 0);

                GL.End();


            }


        }

        private void MusicCirsle(double x, double y, double begin, double end)
        {
            for (int j = 0; j < 5; j++)
            {
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Color4(firstColorSpectr.R, firstColorSpectr.G, firstColorSpectr.B, 0.8);

                if (j == 0)
                    spectrRadius[j] += _FFTSpectr[j] * glControl1.Width / 3;
                else
                    spectrRadius[j] += _FFTSpectr[j] * glControl1.Width / Math.Sqrt(j + 10) + spectrRadius[j - 1];
                spectrRadius[j] /= 2;

                GL.Vertex3(x, y, (double)(1 - 0.1));
                for (double i = begin; i < end; i += 0.05)
                {
                    GL.Color4(secondColorSpectr[j].R, secondColorSpectr[j].G, secondColorSpectr[j].B, 0.8);
                    GL.Vertex3(spectrRadius[j] * Math.Cos(i) + x,
                        spectrRadius[j] * Math.Sin(i) + y, (double)(1 - 0.1));
                    GL.Vertex3(spectrRadius[j] * Math.Cos(i + 0.01) + x,
                        spectrRadius[j] * Math.Sin(i + 0.01) + y, (double)(1 - 0.1));
                }
                GL.End();
            }
        }

        private void GLResize(object sender, EventArgs e)
        {
            if (!_glLoaded)
                return;

            SetupViewport();
            glControl1.Invalidate();
        }

        void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<WaveInEventArgs>(OnDataAvailable), sender, e);
            }
            else
            {
                byte[] buffer = e.Buffer;
                int bytesRecorded = e.BytesRecorded;
                int bufferIncrement = _waveIn.WaveFormat.BlockAlign;

                for (int index = 0; index < bytesRecorded; index += bufferIncrement)
                {
                    float sample32 = BitConverter.ToSingle(buffer, index);
                    _sampleAggregator.Add(sample32);
                }
            }
        }

        void FftCalculated(object sender, FftEventArgs e)
        {
            _FFTDataAvailable = false;
            _FFTData.Clear();
            for (int i = 0; i < e.Result.Length; i++)
                _FFTData.Add(Math.Sqrt(e.Result[i].X * e.Result[i].X + e.Result[i].Y * e.Result[i].Y));
            _FFTDataAvailable = true;
        }

        public Color HSVtoRGB(double hue, double saturation, double value)
        {
            while (hue > 1f) { hue -= 1f; }
            while (hue < 0f) { hue += 1f; }
            while (saturation > 1f) { saturation -= 1f; }
            while (saturation < 0f) { saturation += 1f; }
            while (value > 1f) { value -= 1f; }
            while (value < 0f) { value += 1f; }
            if (hue > 0.999f) { hue = 0.999f; }
            if (hue < 0.001f) { hue = 0.001f; }
            if (saturation > 0.999f) { saturation = 0.999f; }
            if (saturation < 0.001f) { return Color.FromArgb((int)value * 255, (int)value * 255, (int)value * 255); }
            if (value > 0.999f) { value = 0.999f; }
            if (value < 0.001f) { value = 0.001f; }

            double h6 = hue * 6f;
            if (h6 == 6f) { h6 = 0f; }
            int ihue = (int)(h6);
            double p = value * (1f - saturation);
            double q = value * (1f - (saturation * (h6 - (float)ihue)));
            double t = value * (1f - (saturation * (1f - (h6 - (float)ihue))));

            p *= 255;
            value *= 255;
            t *= 255;
            q *= 255;
            switch (ihue)
            {
                case 0:
                    return Color.FromArgb((int)value, (int)t, (int)p);
                case 1:
                    return Color.FromArgb((int)q, (int)value, (int)p);
                case 2:
                    return Color.FromArgb((int)p, (int)value, (int)t);
                case 3:
                    return Color.FromArgb((int)p, (int)q, (int)value);
                case 4:
                    return Color.FromArgb((int)t, (int)p, (int)value);
                default:
                    return Color.FromArgb((int)value, (int)p, (int)q);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _waveformTmr.Close();
            _FFTTimer.Close();
            _waveformTmr.Dispose();
            _FFTTimer.Dispose();

            System.Diagnostics.Process.GetCurrentProcess().Kill();

            Application.ExitThread();
            Application.Exit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        public static void LoadTexture(Bitmap bmp)
        {
            BitmapData data = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0,
            PixelInternalFormat.Rgba, data.Width, data.Height, 0,
            OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
            PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
        }

        private void TransparencyChange(object sender, EventArgs e)
        {
            TR = (double)(numericUpDown2.Value) / 100;
        }

        private void SizeChange(object sender, EventArgs e)
        {
            _fftPart = 180.0 / (double)numericUpDown3.Value;
        }

        private void CountChange(object sender, EventArgs e)
        {
            Visual1.Change((int)numericUpDown4.Value);
        }

        private void SpeedChange(object sender, EventArgs e)
        {
            _speed = (double)numericUpDown5.Value/5;
            Console.WriteLine(_speed);
        }
    }
}