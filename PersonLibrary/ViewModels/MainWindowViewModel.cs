using MVVM.Base.Command;
using MVVM.Base.ViewModel;
using PersonLibrary.Converters;
using PersonLibrary.Models;
using PersonLibrary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace PersonLibrary.ViewModels
{
    #region Data

    //for List
    public class Info : ViewModelBase
    {
        private string _name;
        private string _surname;
        private string _telephone;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        public string Telephone
        {
            get => _telephone;
            set
            {
                _telephone = value;
                OnPropertyChanged(nameof(Telephone));
            }
        }
    }

    //for Tree
    public class ListTree
    {
        public ListTree(string name, string surname, string telephone, ListTree left = null, ListTree right = null)
        {
            TreeInfo = new Info { Name = name, Surname = surname, Telephone = telephone };
            Left = left;
            Right = right;
        }

        public ListTree Left;
        public ListTree Right;
        public Info TreeInfo;
    }

    #endregion

    public sealed class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private ICommand _addPersonCommand;
        private ICommand _showAddPersonCommand;
        private ICommand _deletePersonCommand;
        private ICommand _searchPersonCommand;
        private ICommand _redactorPersonCommand;
        private ICommand _searchPersonClickCommand;
        private ListTree _listsearch;
        private ObservableCollection<Info> _list;
        private string _nameString;
        private string _surnameString;
        private bool _deleteButton = false;
        private bool _isReadOnly = true;
        private bool _textVisible = false;
        private int _selectedIndex = -1;

        private static MainModel myModel;

        #endregion

        #region Properties

        public ICommand AddPersonCommand => _addPersonCommand ??= new RelayCommand(_ => AddPerson(), parameter => CanAddPerson(parameter));
        public ICommand ShowAddPersonCommand => _showAddPersonCommand ??= new RelayCommand(_ => ShowAddPerson());
        public ICommand DeletePersonCommand => _deletePersonCommand ??= new RelayCommand(_ => DeletePerson());
        public ICommand SearchPersonCommand => _searchPersonCommand ??= new RelayCommand(_ => SearchPerson());
        public ICommand RedactorPersonCommand => _redactorPersonCommand ??= new RelayCommand(_ => RedactorPerson());
        public ICommand SearchPersonClickCommand => _searchPersonClickCommand ??= new RelayCommand(_ => SearchPersonClick());

        public ObservableCollection<Info> List => _list;

        public bool DeleteButton
        {
            get => _deleteButton;
            set
            {
                _deleteButton = value;
                OnPropertyChanged(nameof(DeleteButton));
            }
        }

        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        public bool TextVisible
        {
            get => _textVisible;
            set
            {
                _textVisible = value;
                OnPropertyChanged(nameof(TextVisible));
            }
        }

        public string NameString
        {
            get => _nameString;
            set
            {
                _nameString = value;
                OnPropertyChanged(nameof(NameString));
            }
        }

        public string SurNameString
        {
            get => _surnameString;
            set
            {
                _surnameString = value;
                OnPropertyChanged(nameof(SurNameString));
            }
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        #endregion


        #region Constructors

        public MainWindowViewModel()
        {
            _list = new ObservableCollection<Info>();
            _nameString = "";
            _surnameString = "";

            //добавим данные из xml
            AddFromXml();
        }

        static MainWindowViewModel()
        {
            myModel = new MainModel();
        }

        #endregion

        #region Methods

        private void AddFromXml()
        {
            var doc = myModel.Document;
            string name = "", surname = "", telephone = "";

            XmlElement elem = doc.DocumentElement;
            if (elem != null)
            {
                //обходим все корневые узлы
                foreach (XmlElement node in elem)
                {
                    //обход дочерних
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "Name")
                        {
                            name = childNode.InnerText;
                        }
                        else if (childNode.Name == "Surname")
                        {
                            surname = childNode.InnerText;
                        }
                        else if (childNode.Name == "Telephone")
                        {
                            telephone = childNode.InnerText;
                        }
                    }

                    AddInList(name, surname, telephone);
                }
            }
        }

        //позволяет вносить изменения в таблицу, также доступна кнопка удалить выбранного человека
        private void RedactorPerson()
        {
            if (IsReadOnly && !DeleteButton)
            {
                IsReadOnly = false;
                DeleteButton = true;
            }
            else
            {
                IsReadOnly = true;
                DeleteButton = false;
            }
        }

        private void SearchPerson()
        {
            TextVisible = !TextVisible;
        }

        //если человек нашелся, то он выделяется, дальше можно спокойно его удалить
        private void SearchPersonClick()
        {

            if (PersonExist(NameString, SurNameString, out int index))
            {
                SelectedIndex = index;
                MessageBox.Show("Пользователь найден", "Инфо", MessageBoxButton.OK);
            }
            else
            {
                SelectedIndex = index;
                MessageBox.Show("Такого пользователя не существует", "Инфо", MessageBoxButton.OK);
            }

            SurNameString = "";
            NameString = "";
        }

        private bool PersonExist(string name, string surname, out int index)
        {
            index = -1;
            int start = 0;
            int end = List.Count - 1;

            while (start <= end)
            {
                int mid = (start + end) / 2;
                if (string.Compare(name, List[mid].Name) < 0)
                {
                    end = mid - 1;
                }
                else if (string.Compare(name, List[mid].Name) > 0)
                {
                    start = mid + 1;
                }
                else //==0
                {
                    //ищем 1-ого из одинаковых
                    index = mid - 1;
                    while (index != -1 && List[index].Name == List[mid].Name)
                    {
                        --index;
                    }

                    ++index;
                    //ищем подходящего
                    while (index != List.Count && List[index].Name == name)
                    {
                        if (List[index].Surname == surname)
                        {
                            return true;
                        }
                        ++index;
                    }

                    return false;
                }
            }

            return false;
        }

        private void ShowAddPerson()
        {
            var personWindow = new AddPersonWindow();
            if (personWindow.ShowDialog() == true)
            {
                if (!PersonExist(personWindow.PersonText, personWindow.PersonText_2, out _))
                {
                    AddInList(personWindow.PersonText, personWindow.PersonText_2, personWindow.PersonText_3);
                    MessageBox.Show("Пользователь успешно добавлен", "Инфо", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Такой пользователь уже существует", "Инфо", MessageBoxButton.OK);
                }
                        
            }
        }

        private void DeletePerson()
        {
            if (SelectedIndex >= 0)
            {
                List.RemoveAt(SelectedIndex);
            }
        }

        private void AddPerson()
        {
        }

        private bool CanAddPerson(object parameter)
        {
            if (parameter is null)
            {
                return false;
            }
            else
            {
                var temp = parameter as BoolItems;

                return temp.PersonTextColor1
                    && temp.PersonTextColor2
                    && temp.PersonTextColor3;
            }
        }

        //поддержка списка отсортированным
        private void AddInList(string name, string surname, string telephone)
        {
            if (List.Count == 0)
            {
                List.Add(new Info { Name = name, Surname = surname, Telephone = telephone });
                return;
            }

            int start = 0;
            int end = List.Count - 1;

            //проверяем начало и конец
            if (string.Compare(name, List[start].Name) <= 0)
            {
                List.Insert(start, new Info { Name = name, Surname = surname, Telephone = telephone });
                return;
            }
            else if (string.Compare(name, List[end].Name) >= 0)
            {
                List.Add(new Info { Name = name, Surname = surname, Telephone = telephone });
                return;
            }

            while (start <= end)
            {         
                if (string.Compare(name, List[start].Name) <= 0 && string.Compare(name, List[start + 1].Name) >= 0)
                {
                    List.Insert(start + 1, new Info { Name = name, Surname = surname, Telephone = telephone });
                    return;
                }
                else if (string.Compare(name, List[end].Name) <= 0 && string.Compare(name, List[end - 1].Name) >= 0)
                {
                    List.Insert(end, new Info { Name = name, Surname = surname, Telephone = telephone });
                    return;
                }
                else
                {
                    ++start;
                    --end;
                }
            }
        }

        #endregion
    }
}
