using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Xml;

namespace PersonLibrary.Models
{
    internal class MainModel
    {

        #region Fields

        private XmlDocument _document;

        #endregion

        public MainModel()
        {
            _document = new XmlDocument();
            _document.Load(@"A:\Проекты\PersonLibrary\PersonLibrary\XMLFile1.xml");
        }

        public XmlDocument Document => _document;

        #region Methods



        #endregion

    }
}
