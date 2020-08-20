using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XMLExportDC
{

    /// <summary>
    /// Builds an XSLT file that can transfrom XML file to CSV.
    /// </summary>
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

        /// <summary>
        /// Creates the object and sets the column names
        /// </summary>
        /// <param name="columns"></param>
        public XSLConstructor(string[] columns)
        {
            Columns = columns;
        }

        /// <summary>
        /// Creates the object and sets the column names
        /// </summary>
        /// <param name="columns"></param>
        public XSLConstructor(IEnumerable<string> columns)
        {
            Columns = columns.ToArray();
        }

        /// <summary>
        /// Reads the special XSL template file.
        /// </summary>
        /// <param name="path">The input stream for the embedded file</param>
        public void ReadXSLTemplate(Stream fileStream)
        {
            using (StreamReader sr = new StreamReader(fileStream))
            {
                XSLTemplate = sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Builds the template with the given columns
        /// </summary>
        public void BuildTemplate()
        {
            if (XSLTemplate != null)
            {
                BuildColumnDefinition(XSLTemplate);
                BuildRowDefinition(XSLTemplate);
            }
        }

        /// <summary>
        /// Inserts column definitions into the template.
        /// </summary>
        /// <param name="fileString"></param>
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

        /// <summary>
        /// Inserts row definitions into the template.
        /// </summary>
        /// <param name="fileString"></param>
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

        /// <summary>
        /// Saves the created template to a path.
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            if (XSLTemplate != null)
            {
                File.WriteAllText(path, XSLTemplate);
            }
        }
    }
}
