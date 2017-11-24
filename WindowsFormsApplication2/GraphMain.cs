using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    class GraphMain
    {

        const int maxPoints = 500;
        private double _r;
        private double _Pi;
        private double _maxR;
        private List<PointF> _coordinates;
        private Color _color;
        private List<Color> _colors;

        public List<PointF> Coordinates
        {
            get { return _coordinates; }
        }

        public List<Color> Colors
        {
            get { return _colors; }
        }

        public GraphMain()
        {
            _coordinates = new List<PointF>();
            _colors = new List<Color>();
        }

        public GraphMain(double maxX, double maxY)
        {
            _coordinates = new List<PointF>();
            _colors = new List<Color>();
            _maxR = (maxX + maxY);
            _color = Color.Black;
        }

        public void AddPoint(double total, Color currentColor, double different)
        {
            Random randomNumber = new Random();

            //if (total < 0.1)
            total += 0.05;
            if (total > 1)
                total = 1;

            different += 0.1;

            double newR = (_maxR * total + 14 * _r) / 15;
            //double newPi = _Pi + total * total;
            double newPi = _Pi + different * different * 2 * total / 3;

            for (int i = 1; i <= 10; i++)
            {
                _coordinates.Add(new PointF((float)((_r + i * (newR - _r) / 10) * Math.Cos(_Pi + (newPi - _Pi) * i / 10)),
                    (float)((_r + i * (newR - _r) / 10) * Math.Sin(_Pi + (newPi - _Pi) * i / 10))));
                _colors.Add(Color.FromArgb((_color.R * (10 - i) + currentColor.R * (i)) / 10,
                    (_color.G * (10 - i) + currentColor.G * (i)) / 10, (_color.B * (10 - i) + currentColor.B * (i)) / 10));
            }
            _r = newR;
            _Pi = newPi;
            if (_Pi > 2 * Math.PI)
                _Pi -= 2 * Math.PI;

            _color = currentColor;

            if (_coordinates.Count > maxPoints)
            {
                for (int i = 0; i < 10; i++)
                {
                    _coordinates.RemoveAt(0);
                    _colors.RemoveAt(0);
                }
            }
        }
    }
}