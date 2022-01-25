using MVVM.Base.Converter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace PersonLibrary.Converters
{
    public class BoolItems
    {
        public bool PersonTextColor1 { get; set; }
        public bool PersonTextColor2 { get; set; }
        public bool PersonTextColor3 { get; set; }

        /*
        public string PersonText1 { get; set; }
        public string PersonText2 { get; set; }
        public string PersonText3 { get; set; }
        */
    }

    public sealed class BrushToBoolConverter : MultiConverterBase<BrushToBoolConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3)
            {
                throw new ArgumentException($"Length is not 3");
            }

            if (!(values[0] is Brush)
                && !(values[1] is Brush)
                && !(values[2] is Brush))
            {
                throw new ArgumentException($"Type is not {typeof(Brush).FullName}", nameof(values));
            }

            return new BoolItems
            {
                PersonTextColor1 = (Brush)values[0] == Brushes.Black,
                PersonTextColor2 = (Brush)values[1] == Brushes.Black,
                PersonTextColor3 = (Brush)values[2] == Brushes.Black
            };
        }
    }
}
