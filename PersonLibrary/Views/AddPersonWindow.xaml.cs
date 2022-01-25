using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PersonLibrary.Views
{
    /// <summary>
    /// Логика взаимодействия для AddPersonWindow.xaml
    /// </summary>
    public partial class AddPersonWindow : Window
    {
        public AddPersonWindow()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public static readonly DependencyProperty PersonTextProperty = DependencyProperty.Register(
            nameof(PersonText),
            typeof(string),
            typeof(AddPersonWindow),
            new PropertyMetadata("", (d, e) => 
            {
                if (!(d is AddPersonWindow userControl))
                {
                    throw new ArgumentException($"Parameter \"{d}\") should be of type \"{typeof(AddPersonWindow).FullName}\"");
                }

                if (!(e.NewValue is string valueString))
                {
                    throw new ArgumentException($"Parameter \"{e.NewValue}\") should be of type \"{typeof(string).FullName}\"");
                }

                if (valueString.Length > 50)
                {
                    userControl.PersonText = valueString.Substring(0, 50);
                }
                else if (valueString.Length == 0)
                {
                    // если пустая строка
                    userControl.PersonTextColor = Brushes.Red;
                    return;
                }

                // проверить валидность данных (не введена цифра)
                foreach (char ch in valueString)
                {
                    if (int.TryParse(ch.ToString(), out int res))
                    {
                        userControl.PersonTextColor = Brushes.Red;
                        return;
                    }
                }

                userControl.PersonTextColor = Brushes.Black;
                
            }));

        public static readonly DependencyProperty PersonTextProperty_2 = DependencyProperty.Register(
            nameof(PersonText_2),
            typeof(string),
            typeof(AddPersonWindow),
            new PropertyMetadata("", (d, e) =>
            {
                if (!(d is AddPersonWindow userControl))
                {
                    throw new ArgumentException($"Parameter \"{d}\") should be of type \"{typeof(AddPersonWindow).FullName}\"");
                }

                if (!(e.NewValue is string valueString))
                {
                    throw new ArgumentException($"Parameter \"{e.NewValue}\") should be of type \"{typeof(string).FullName}\"");
                }

                if (valueString.Length > 50)
                {
                    userControl.PersonText_2 = valueString.Substring(0, 50);
                }
                else if (valueString.Length == 0)
                {
                    // если пустая строка
                    userControl.PersonTextColor_2 = Brushes.Red;
                    return;
                }

                // проверить валидность данных (не введена цифра)
                foreach (char ch in valueString)
                {
                    if (int.TryParse(ch.ToString(), out int res))
                    {
                        userControl.PersonTextColor_2 = Brushes.Red;
                        return;
                    }
                }

                userControl.PersonTextColor_2 = Brushes.Black;

            }));

        public static readonly DependencyProperty PersonTextProperty_3 = DependencyProperty.Register(
            nameof(PersonText_3),
            typeof(string),
            typeof(AddPersonWindow),
            new PropertyMetadata("", (d, e) =>
            {
                if (!(d is AddPersonWindow userControl))
                {
                    throw new ArgumentException($"Parameter \"{d}\") should be of type \"{typeof(AddPersonWindow).FullName}\"");
                }

                if (!(e.NewValue is string valueString))
                {
                    throw new ArgumentException($"Parameter \"{e.NewValue}\") should be of type \"{typeof(string).FullName}\"");
                }

                if (valueString.Length > 16)
                {
                    userControl.PersonText_3 = valueString.Substring(0, 16);
                }
                else if (valueString.Length == 0)
                {
                    // если пустая строка
                    userControl.PersonTextColor_3 = Brushes.Red;
                    return;
                }

                //неправильный номер
                if (!IsPhoneNumber(valueString))
                {
                    userControl.PersonTextColor_3 = Brushes.Red;
                    return;
                }

                userControl.PersonTextColor_3 = Brushes.Black;

            }));

        public static readonly DependencyProperty PersonTextColorProperty = DependencyProperty.Register(
            nameof(PersonTextColor),
            typeof(Brush),
            typeof(AddPersonWindow),
            new PropertyMetadata(Brushes.Red));

        public static readonly DependencyProperty PersonTextColorProperty_2 = DependencyProperty.Register(
            nameof(PersonTextColor_2),
            typeof(Brush),
            typeof(AddPersonWindow),
            new PropertyMetadata(Brushes.Red));

        public static readonly DependencyProperty PersonTextColorProperty_3 = DependencyProperty.Register(
            nameof(PersonTextColor_3),
            typeof(Brush),
            typeof(AddPersonWindow),
            new PropertyMetadata(Brushes.Red));

        public static readonly DependencyProperty AddTextColorProperty = DependencyProperty.Register(
            nameof(AddTextColor),
            typeof(Brush),
            typeof(AddPersonWindow),
            new PropertyMetadata(Brushes.Green));

        #endregion

        #region CLR Properties

        public string PersonText
        {
            get => (string)GetValue(PersonTextProperty);
            set => SetValue(PersonTextProperty, value);
        }

        public Brush PersonTextColor
        {
            get => (Brush)GetValue(PersonTextColorProperty);
            set => SetValue(PersonTextColorProperty, value);
        }

        public string PersonText_2
        {
            get => (string)GetValue(PersonTextProperty_2);
            set => SetValue(PersonTextProperty_2, value);
        }

        public Brush PersonTextColor_2
        {
            get => (Brush)GetValue(PersonTextColorProperty_2);
            set => SetValue(PersonTextColorProperty_2, value);
        }

        public string PersonText_3
        {
            get => (string)GetValue(PersonTextProperty_3);
            set => SetValue(PersonTextProperty_3, value);
        }

        public Brush PersonTextColor_3
        {
            get => (Brush)GetValue(PersonTextColorProperty_3);
            set => SetValue(PersonTextColorProperty_3, value);
        }

        public Brush AddTextColor
        {
            get => (Brush)GetValue(AddTextColorProperty);
            set => SetValue(AddTextColorProperty, value);
        }

        #endregion


        #region Methods

        public static bool IsPhoneNumber(string number)
        {
            if (number.Length == 0)
            {
                return false;
            }

            int start = 0;
            if (number[start] == '+' && number.Length == 16)
            {
                ++start;
                if (number[start] >= '0' && number[start] <= '9'
                    && number[start + 1] == '-')
                {
                    start += 2;
                }
                else
                {
                    return false;
                }

                // пройтись 2 раза по 3 до -
                for (int i = 0; i < 2; ++i)
                {
                    int iter = 0;
                    while (iter != 3)
                    {
                        if (int.TryParse(number[start].ToString(), out _))
                        {
                            ++iter;
                            ++start;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (number[start] == '-')
                    {
                        ++start;
                    }
                    else
                    {
                        return false;
                    }
                }

                // пройтись 2 раза по 2 до -
                for (int i = 0; i < 2; ++i)
                {
                    int iter = 0;
                    while (iter != 2)
                    {
                        if (int.TryParse(number[start].ToString(), out _))
                        {
                            ++iter;
                            ++start;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    //может быть конец
                    if (start == number.Length)
                    {
                        return true;
                    }

                    if (number[start] == '-')
                    {
                        ++start;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        //двигать окно
        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        //Добавить челика
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        #endregion
    }
}