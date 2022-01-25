using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace MVVM.Base.Converter
{
    public abstract class MultiConverterBase<TMultiConverter> : MarkupExtension, IMultiValueConverter
        where TMultiConverter: class, new()
    {
        private static TMultiConverter _provideValue;

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _provideValue ??= new TMultiConverter();
        }
    }
}
