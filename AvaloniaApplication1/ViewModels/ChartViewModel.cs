using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.ComponentModel;

namespace AvaloniaSample
{
    public class ViewModel
    {   
        public ISeries[] Series { get; set; }
            = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = new double[] { 1.0,2.9,3.9,4.9,5.9 },
                    //Fill = null
                }
            };
    }
}
