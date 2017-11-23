using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    class Calculation
    {
        public static void setFFtNum(int[] FFTnum)
        {
            Random RandomNum = new Random();
            FFTnum[0] = (RandomNum.Next() % 5) + 1;
            for (int j = 1; j < 50; j++)
            {
                if (FFTnum[j - 1] == 5)
                    FFTnum[j] = 1;
                else
                {
                    FFTnum[j] = FFTnum[j - 1] + 1;
                }
            }
            //for (int i = 0; i < 50; i++)
            //{
            //    if (FFTnum[i] == 0)
            //        FFTnum[i] = 1;
            //}
        }

        public static void ChangeColor(ref double red,
            ref double green, ref double blue, double total,
            ref double _red, ref double _green, ref double _blue)
        {
            green = red * blue * total * 4;

            red = red * total * 2;
            if (red > 1)
                red = 1;
            blue = blue * total * 2;
            if (blue > 1)
                blue = 1;

            if ((red > blue) && (green > blue))
            {
                green = green * (1 + total);
                red = red * (1 + total);
                blue = blue / (1 + total);
            }
            if ((blue > red) && (green > red))
            {
                green = green * (1 + total);
                blue = blue * (1 + total);
                red = red / (1 + total);
            }
            if ((blue > green) && (red > green))
            {
                red = red * (1 + total);
                blue = blue * (1 + total);
                green = green / (1 + total);
            }

            blue = (2.2 - total) * blue * (1 - red + green);

            if (blue < 0)
                blue = 0;

            if (green > 1)
                green = 1;
            if (red > 1)
                red = 1;
            if (blue > 1)
                blue = 1;

            _red = (red * 255);
            _green = (green * 255);
            _blue = (blue * 255);

            if (_red > 255)
                _red = 255;

            if (_green > 255)
                _green = 255;

            if (_blue > 255)
                _blue = 255;

            _blue = 255 - _red;
        }
    }
}
