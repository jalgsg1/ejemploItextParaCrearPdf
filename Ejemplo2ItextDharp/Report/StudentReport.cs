using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Ejemplo2ItextDharp.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Mvc;


namespace Ejemplo2ItextDharp.Report
{
    public class StudentReport
    {
        
        #region Declaration
        private int _totalColumn = 3;
        private Document _document;
        private Font _fontStyle;
        private PdfPTable _pdfPTable = new PdfPTable(3);
        private PdfPCell _pdfPCell;
        private MemoryStream _memoryStream = new MemoryStream();
        private List<Student> _students = new List<Student>();
        #endregion

        

        //using (FileStream output = new FileStream(@"test-results\content\dynamicTable.pdf", FileMode.Create, FileAccess.Write))

        public byte[] PrepareReport(List<Student> students)
        {
            _students = students;

            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 40f, 20f);
            _pdfPTable.WidthPercentage = 100;
            _pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            this.Set_fontStyle("Helvetica", 8f);

            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();

            float[] pwidths = new float[] {40f, 150f, 100f };
            /*
            List<float> pwidths = new List<float>();
            pwidths.Add(40f);
            pwidths.Add(150f);
            pwidths.Add(100f);
            */
            _pdfPTable.SetWidths( pwidths );

            this.ReportHeader("Título", "descripcion");
            this.ReportBody();
            _pdfPTable.HeaderRows = 2;
            _document.Add(_pdfPTable);
            _document.Close();

            return _memoryStream.ToArray();
        }

        //"Helvetica, 8f, 1,"
        private void Set_fontStyle(String pFontName, float pFontSize)
        {
            _fontStyle = FontFactory.GetFont(pFontName, pFontSize, 1);
        }

        private void ReportHeader(String ptittle, String subTitle)
        {
            createTittleCell(ptittle, "Helvetica", 11f, BaseColor.WHITE);
            //_pdfPTable.CompleteRow();

            createTittleCell(subTitle, "Helvetica", 9f, BaseColor.WHITE);
            _pdfPTable.CompleteRow();

        }

        #region function to createCenteredCell / title
        /*
         * Create a centered cell that looks like the title.
         * myPhrase -> simple string
         * pFontName -> Font type, it could be "Tahoma", "Arial", "Helvetica", etc.
         * pFontSize -> The size of the letters in pixels.
         */
        private PdfPCell createTittleCell(String myPhrase, String pFontName, float pfontSize, iTextSharp.text.BaseColor pBackGColor)
        {
            this.Set_fontStyle(pFontName, pfontSize);
            this._pdfPCell = new PdfPCell(new Phrase(myPhrase, _fontStyle));
            this._pdfPCell.Colspan = _totalColumn;
            this._pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //Border is to made the cell visible
            this._pdfPCell.Border = 0;
            this._pdfPCell.BackgroundColor = pBackGColor;
            this._pdfPCell.ExtraParagraphSpace = 0;

            _pdfPTable.AddCell(this._pdfPCell);
            
            return this._pdfPCell;
        }
        #endregion

        

        private void ReportBody()
        {
            #region Table header
            
            this.createCell("Serial number", "Helvetica", 8f, BaseColor.LIGHT_GRAY);

            this.createCell("Name", "Helvetica", 8f, BaseColor.LIGHT_GRAY);

            this.createCell("Roll", "Helvetica", 8f, BaseColor.LIGHT_GRAY);
            
            #endregion


            #region Table Body

            int serialNumber = 1;
            foreach (Student student in _students)
            {
                this.createCell(serialNumber++.ToString(), "Helvetica", 8f, BaseColor.WHITE);

                this.createCell(student.Name, "Helvetica", 8f, BaseColor.WHITE);

                this.createCell(student.Roll, "Helvetica", 8f, BaseColor.WHITE);
                
            }

            #endregion
        }

        #region Function to create table cells
        /*
         * Create a centered cell and insert it on the table.
         * 
         * myPhrase -> simple string
         * pFontName -> Font type, it could be "Tahoma", "Arial", "Helvetica", etc.
         * pFontSize -> The size of the letters in pixels.
         * pBackGColor -> The cell background color. Ex: BaseColor.WHITE, BaseColor.LIGHT_GRAY, etc.
         */
        private PdfPCell createCell(String myPhrase, String pFontName, float pfontSize, 
            iTextSharp.text.BaseColor pBackGColor)
        {
            this.Set_fontStyle(pFontName, pfontSize);
            this._pdfPCell = new PdfPCell(new Phrase(myPhrase, _fontStyle));
            this._pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            this._pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            this._pdfPCell.BackgroundColor = pBackGColor;
            _pdfPTable.AddCell(this._pdfPCell);

            return this._pdfPCell;
        }
        #endregion

        private void insertCellInTable(PdfPCell p_Cell, PdfPTable p_pdfPTable)
        {
            p_pdfPTable.AddCell(p_Cell);

        }

    }
}