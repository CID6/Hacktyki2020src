#XMLExport

XMLExport is a fast and flexible solution for converting your XML files into CSV files. No more broken online converters!


#Features

-Quickly transform your XML data
-Convert you XML files into CSV files
-Build XSL transformations for future conversions
-Works on Windows, Linux and macOS

#Requirements

.NETCore 3.1 required. Download from https://dotnet.microsoft.com/download

#Usage

Look how easy it is to use:
dotnet .\XMLExportDC.dll E:\XMLData\ProductionResults.xml Car ProductionYear VIN Model
Usage: dotnet program input_path element_name \{columns} \[-o \[output_xml_path]] \[-c \[output_csv_path]] \[-x \[output_xsl_path]]