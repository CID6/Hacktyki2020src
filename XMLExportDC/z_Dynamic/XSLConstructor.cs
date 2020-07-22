using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace XMLExportDC
{
    public class XSLConstructor
    {
        public string XSLTemplate { get; set; } = null;
        public string[] Columns { get; set; }

        const string tempColumnDefinition = "%%COLUMN_TEMPLATE%%";
        const string tempRowDefinition = "%%ROW_TEMPLATE%%";
        const string textDefinitionS = "<xsl:text>";
        const string textDefinitionE = "</xsl:text>";
        const string valueOfDefinition = "<xsl:value-of select=\"%%ROW_VALUE%%\"/>";
        const string valueOfPlaceholder = "%%ROW_VALUE%%";
        const string commaDefinition = "<xsl:text>,</xsl:text>";
        const string endl = "\n";

        public XSLConstructor(string[] columns)
        {
            Columns = columns;
        }

        public XSLConstructor(IEnumerable<string> columns)
        {
            Columns = columns.ToArray();
        }

        public void ReadXSLTemplate(string path)
        {
            XSLTemplate = File.ReadAllText(path);
        }

        public void ReadXSLTemplate(Stream fileStream)
        {
            using (StreamReader sr = new StreamReader(fileStream))
            {
                XSLTemplate = sr.ReadToEnd();
            }
        }

        public void BuildTemplate()
        {
            if (XSLTemplate != null)
            {
                BuildColumnDefinition(XSLTemplate);
                BuildRowDefinition(XSLTemplate);
            }
        }

        private void BuildColumnDefinition(string fileString)
        {
            string columnDefinition = "";

            for (int i = 0; i < Columns.Length; i++)
            {
                columnDefinition += textDefinitionS + Columns[i] + textDefinitionE;

                if (i != Columns.Length - 1)
                {
                    columnDefinition += endl + commaDefinition + endl;
                }
            }

            XSLTemplate = fileString.Replace(tempColumnDefinition, columnDefinition);
        }

        private void BuildRowDefinition(string fileString)
        {
            string rowDefinition = "";

            for (int i = 0; i < Columns.Length; i++)
            {
                rowDefinition += valueOfDefinition.Replace(valueOfPlaceholder, Columns[i]);

                if (i != Columns.Length - 1)
                {
                    rowDefinition += endl + commaDefinition + endl;
                }
            }

            XSLTemplate = fileString.Replace(tempRowDefinition, rowDefinition);
        }

        public void Save(string path)
        {
            if (XSLTemplate != null)
            {
                File.WriteAllText(path, XSLTemplate);
            }
        }
    }
}
