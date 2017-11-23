using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    class DataGraph
    {
        static public void HandleData(ref List<double> graphPoints, List<double> data, int width, int startPoint, int countOfPoints)
        {
            graphPoints = new List<double>();

            double pointByStep = (double)countOfPoints/width;
            int currentPoint = 0;
            int countStepPoints = 0;

            for (int i = 1; i < width; i++)
            {
                double newPoint = 0;

                if (data.Count< startPoint + pointByStep * i + 2)
                    break;

                for (int j = currentPoint; j < pointByStep*i; j++)
                {
                    newPoint += Math.Abs(data[startPoint + j]);
                    currentPoint++;
                }
                graphPoints.Add(newPoint); 
            }
        }
    }
}
