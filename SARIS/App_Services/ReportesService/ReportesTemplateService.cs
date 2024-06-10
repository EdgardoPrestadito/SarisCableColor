using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using OrionCoreCableColor.App_Helper;
using OrionCoreCableColor.Controllers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace OrionCoreCableColor.App_Services.ReportesService
{
    public class ReportesTemplateService
    {

        public Table Linea(int columnas)
        {
            var tabla = new Table(columnas).SetWidth(UnitValue.CreatePercentValue(100)).SetHorizontalAlignment(HorizontalAlignment.CENTER);
            var linea = new Cell(1, columnas).SetBorder(Border.NO_BORDER);
            tabla.AddCell(linea.SetBorderBottom(new SolidBorder(Color.ConvertRgbToCmyk(new DeviceRgb(System.Drawing.Color.Gray)), 1f, 0.5f)));
            return tabla;
        }

        

        //public Stream GenerarFactura(int fiIDTransaccion)
        //{
        //    using (var contexto = new CoreFinancieroEntities2())
        //    {
        //        var ms = new MemoryStream();
        //        var pw = new PdfWriter(ms);
        //        var pdfDocument = new PdfDocument(pw);
        //        var doc = new Document(pdfDocument, PageSize.LETTER);

        //        doc.SetMargins(20, 20, 5, 20);
        //        doc.SetProperty(Property.LEADING, new Leading(Leading.MULTIPLIED, 1f));
        //        PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        //        doc.SetFontSize(12f).SetFont(font);
        //        string pathImageNovanetLogo = System.IO.Path.Combine(MemoryLoadManager.URL, @"Content\img\LogoPrestadito.png");
        //        var prestaditoLogo = new Image(ImageDataFactory.Create(pathImageNovanetLogo));

        //        string pathImageBackGround = System.IO.Path.Combine(MemoryLoadManager.URL, @"Content\img\HOJAS_NOVANET.png");
        //        var backGround = new Image(ImageDataFactory.Create(pathImageBackGround));

        //        pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, new BackgroundImageHandler(backGround));
        //        var modelDb = contexto.sp_Facturacion_ConsultarFactura_NovaNet(fiIDTransaccion, null)?.FirstOrDefault() ?? null;

        //        if (modelDb != null) 
        //        {
        //            var tblInformacionEmpresa = new Table(UnitValue.CreatePercentArray(new float[] { 25f, 45f })).SetWidth(UnitValue.CreatePercentValue(100)).SetHorizontalAlignment(HorizontalAlignment.LEFT);
        //            var cellLogo = new Cell(6, 1).Add(prestaditoLogo.SetAutoScale(true).SetBorder(Border.NO_BORDER)).SetBorder(Border.NO_BORDER);
        //            tblInformacionEmpresa.AddCell(cellLogo);

        //            var cellInfoEmpresa = new Cell(1, 1).Add(new Paragraph(new Text($"Prestadito S.A de C.V.").SetBold()).SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f)).SetBorder(Border.NO_BORDER);
        //            tblInformacionEmpresa.AddCell(cellInfoEmpresa);

        //            cellInfoEmpresa = new Cell(1, 1).Add(new Paragraph(new Text(modelDb.fcDireccionAgencia).SetBold()).SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f)).SetBorder(Border.NO_BORDER);
        //            tblInformacionEmpresa.AddCell(cellInfoEmpresa);

        //            cellInfoEmpresa = new Cell(1, 1).Add(new Paragraph(new Text("SAN PEDRO SULA").SetBold()).SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f)).SetBorder(Border.NO_BORDER);
        //            tblInformacionEmpresa.AddCell(cellInfoEmpresa);

        //            cellInfoEmpresa = new Cell(1, 1).Add(new Paragraph(new Text("Tel.(+504)2504-6682").SetBold()).SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f)).SetBorder(Border.NO_BORDER);
        //            tblInformacionEmpresa.AddCell(cellInfoEmpresa);

        //            cellInfoEmpresa = new Cell(1, 1).Add(new Paragraph(new Text("contabilidad@miprestadito.com").SetBold()).SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f)).SetBorder(Border.NO_BORDER);
        //            tblInformacionEmpresa.AddCell(cellInfoEmpresa);

        //            cellInfoEmpresa = new Cell(1, 1).Add(new Paragraph(new Text("Domicilio Fiscal: San Pedro Sula").SetBold()).SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f)).SetBorder(Border.NO_BORDER);
        //            tblInformacionEmpresa.AddCell(cellInfoEmpresa);

        //            doc.Add(tblInformacionEmpresa);

        //            doc.Add(new Paragraph(new Text($"RTN. 05019016811399").SetBold()));
        //            doc.Add(new Paragraph(new Text($"Factura: {modelDb.fcFactura}").SetBold()));
        //            doc.Add(new Paragraph(new Text($"CAI: {modelDb.fcCAI}")).SetBold());

        //            doc.Add(new Paragraph());

        //            var tablaInfoCliente = new Table(4).SetWidth(UnitValue.CreatePercentValue(100)).SetHorizontalAlignment(HorizontalAlignment.LEFT);
        //            var celdaDescripcion = new Cell(1, 1).Add(new Paragraph(new Text("Cliente:")).SetBold()).SetBorder(Border.NO_BORDER);
        //            var celdaValor = new Cell(1, 1).Add(new Paragraph(new Text(modelDb.fcNombreCliente))).SetBorder(Border.NO_BORDER);

        //            tablaInfoCliente.AddCell(celdaDescripcion).AddCell(celdaValor);
                    
        //            celdaDescripcion = new Cell(1, 1).Add(new Paragraph(new Text("Fecha:")).SetBold()).SetBorder(Border.NO_BORDER);
        //            celdaValor = new Cell(1, 1).Add(new Paragraph(new Text(modelDb.fdFechaFactura.ToString("dd/MM/yyyy")))).SetBorder(Border.NO_BORDER);
        //            tablaInfoCliente.AddCell(celdaDescripcion).AddCell(celdaValor);



        //            celdaDescripcion = new Cell(1, 1).Add(new Paragraph(new Text("RTN:")).SetBold()).SetBorder(Border.NO_BORDER);
        //            celdaValor = new Cell(1, 3).Add(new Paragraph(new Text(modelDb.fcRTN))).SetBorder(Border.NO_BORDER);
        //            tablaInfoCliente.AddCell(celdaDescripcion).AddCell(celdaValor);
                    


        //            celdaDescripcion = new Cell(1, 1).Add(new Paragraph(new Text("Direccion:")).SetBold()).SetBorder(Border.NO_BORDER);
        //            celdaValor = new Cell(1, 3).Add(new Paragraph(new Text(""))).SetBorder(Border.NO_BORDER);
        //            tablaInfoCliente.AddCell(celdaDescripcion).AddCell(celdaValor);
                    

        //            celdaDescripcion = new Cell(1, 1).Add(new Paragraph(new Text("Fecha Limite de Emisión:")).SetBold()).SetBorder(Border.NO_BORDER);
        //            celdaValor = new Cell(1, 3).Add(new Paragraph(new Text(modelDb.fdFechaRangoFinalFacturacion.ToString("dd/MM/yyyy")))).SetBorder(Border.NO_BORDER);
        //            tablaInfoCliente.AddCell(celdaDescripcion).AddCell(celdaValor);
                    

        //            celdaDescripcion = new Cell(1, 1).Add(new Paragraph(new Text("Rango de Autorización:")).SetBold()).SetBorder(Border.NO_BORDER);
        //            celdaValor = new Cell(1, 3).Add(new Paragraph(new Text($"{modelDb.fcRangoInicialFacturacion} al {modelDb.fcRangoFinalFacturacion}"))).SetBorder(Border.NO_BORDER);
        //            tablaInfoCliente.AddCell(celdaDescripcion).AddCell(celdaValor);
                   

        //            celdaDescripcion = new Cell(1, 1).Add(new Paragraph(new Text("No. Declaracion:")).SetBold()).SetBorder(Border.NO_BORDER);
        //            celdaValor = new Cell(1, 3).Add(new Paragraph(new Text($""))).SetBorder(Border.NO_BORDER);
        //            tablaInfoCliente.AddCell(celdaDescripcion).AddCell(celdaValor);
                    


        //            doc.Add(tablaInfoCliente);
        //            doc.Add(new Paragraph(""));
        //            var tablaProducto = new Table(UnitValue.CreatePercentArray(new float[] { 10f, 60f, 15f, 15f })).UseAllAvailableWidth().SetWidth(UnitValue.CreatePercentValue(100)).SetHorizontalAlignment(HorizontalAlignment.LEFT);
        //            tablaProducto.AddHeaderCell(new Paragraph("CANT.").SetBold().SetTextAlignment(TextAlignment.CENTER));
        //            tablaProducto.AddHeaderCell(new Paragraph("DESCRIPCION").SetBold());
        //            tablaProducto.AddHeaderCell(new Paragraph("VALOR").SetBold().SetTextAlignment(TextAlignment.RIGHT));
        //            tablaProducto.AddHeaderCell(new Paragraph("TOTAL").SetBold().SetTextAlignment(TextAlignment.RIGHT));

        //            var celdaProdCantidad = new Cell(1, 1).Add(new Paragraph("1").SetTextAlignment(TextAlignment.CENTER));
        //            var celdaProdDescripcion = new Cell(1, 1).Add(new Paragraph(modelDb.fcProductoDescripcion));
        //            var celdaProdValor = new Cell(1, 1).Add(new Paragraph(new Text($"{modelDb.fcSimboloMoneda} {Convert.ToDecimal(modelDb.fnTotal):n}")).SetTextAlignment(TextAlignment.RIGHT));
        //            var celdaProdTotal = new Cell(1, 1).Add(new Paragraph(new Text($"{modelDb.fcSimboloMoneda} {Convert.ToDecimal(modelDb.fnTotal):n}")).SetTextAlignment(TextAlignment.RIGHT));
        //            tablaProducto.AddCell(celdaProdCantidad).AddCell(celdaProdDescripcion).AddCell(celdaProdValor).AddCell(celdaProdTotal);
                    


        //            celdaProdCantidad = new Cell(1, 1);
        //            celdaProdDescripcion = new Cell(1, 1).Add(new Paragraph("Sub Total").SetBold().SetTextAlignment(TextAlignment.RIGHT));
        //            celdaProdValor = new Cell(1, 1);
        //            celdaProdTotal = new Cell(1, 1).Add(new Paragraph(new Text($"{modelDb.fcSimboloMoneda} {Convert.ToDecimal(modelDb.fnSubtotal).ToString("n")}")).SetTextAlignment(TextAlignment.RIGHT));
        //            tablaProducto.AddCell(celdaProdCantidad).AddCell(celdaProdDescripcion).AddCell(celdaProdValor).AddCell(celdaProdTotal);


        //            celdaProdCantidad = new Cell(1, 1);
        //            celdaProdDescripcion = new Cell(1, 1).Add(new Paragraph("Descuento").SetBold().SetTextAlignment(TextAlignment.RIGHT));
        //            celdaProdValor = new Cell(1, 1);
        //            celdaProdTotal = new Cell(1, 1).Add(new Paragraph(new Text($"{modelDb.fcSimboloMoneda} 0.00")).SetTextAlignment(TextAlignment.RIGHT));
        //            tablaProducto.AddCell(celdaProdCantidad).AddCell(celdaProdDescripcion).AddCell(celdaProdValor).AddCell(celdaProdTotal);

        //            celdaProdCantidad = new Cell(1, 1);
        //            celdaProdDescripcion = new Cell(1, 1).Add(new Paragraph("Exento").SetBold().SetTextAlignment(TextAlignment.RIGHT));
        //            celdaProdValor = new Cell(1, 1);
        //            celdaProdTotal = new Cell(1, 1).Add(new Paragraph(new Text($"{modelDb.fcSimboloMoneda} {Convert.ToDecimal(modelDb.fnImporteExento).ToString("n")}")).SetTextAlignment(TextAlignment.RIGHT));
        //            tablaProducto.AddCell(celdaProdCantidad).AddCell(celdaProdDescripcion).AddCell(celdaProdValor).AddCell(celdaProdTotal);


        //            celdaProdCantidad = new Cell(1, 1);
        //            celdaProdDescripcion = new Cell(1, 1).Add(new Paragraph("Exonerado").SetBold().SetTextAlignment(TextAlignment.RIGHT));
        //            celdaProdValor = new Cell(1, 1);
        //            celdaProdTotal = new Cell(1, 1).Add(new Paragraph(new Text($"{modelDb.fcSimboloMoneda} {Convert.ToDecimal(modelDb.fnImporteExonerado).ToString("n")}")).SetTextAlignment(TextAlignment.RIGHT));
        //            tablaProducto.AddCell(celdaProdCantidad).AddCell(celdaProdDescripcion).AddCell(celdaProdValor).AddCell(celdaProdTotal);


        //            celdaProdCantidad = new Cell(1, 1);
        //            celdaProdDescripcion = new Cell(1, 1).Add(new Paragraph("Gravado 15%").SetBold().SetTextAlignment(TextAlignment.RIGHT));
        //            celdaProdValor = new Cell(1, 1);
        //            celdaProdTotal = new Cell(1, 1).Add(new Paragraph(new Text($"{modelDb.fcSimboloMoneda} {Convert.ToDecimal(modelDb.fnImporteGravado).ToString("n")}")).SetTextAlignment(TextAlignment.RIGHT));
        //            tablaProducto.AddCell(celdaProdCantidad).AddCell(celdaProdDescripcion).AddCell(celdaProdValor).AddCell(celdaProdTotal);

        //            celdaProdCantidad = new Cell(1, 1);
        //            celdaProdDescripcion = new Cell(1, 1).Add(new Paragraph("ISV 15%").SetBold().SetTextAlignment(TextAlignment.RIGHT));
        //            celdaProdValor = new Cell(1, 1);
        //            celdaProdTotal = new Cell(1, 1).Add(new Paragraph(new Text($"{modelDb.fcSimboloMoneda} {Convert.ToDecimal(modelDb.fnImpuestos).ToString("n")}")).SetTextAlignment(TextAlignment.RIGHT));
        //            tablaProducto.AddCell(celdaProdCantidad).AddCell(celdaProdDescripcion).AddCell(celdaProdValor).AddCell(celdaProdTotal);

        //            celdaProdCantidad = new Cell(1, 1);
        //            celdaProdDescripcion = new Cell(1, 1).Add(new Paragraph("TOTAL").SetBold().SetTextAlignment(TextAlignment.RIGHT));
        //            celdaProdValor = new Cell(1, 1);
        //            celdaProdTotal = new Cell(1, 1).Add(new Paragraph(new Text($"{modelDb.fcSimboloMoneda} {Convert.ToDecimal(modelDb.fnTotal).ToString("n")}")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
        //            tablaProducto.AddCell(celdaProdCantidad).AddCell(celdaProdDescripcion).AddCell(celdaProdValor).AddCell(celdaProdTotal);


        //            doc.Add(tablaProducto);
                    
        //            doc.Add(new Paragraph($"MONTO EN LETRAS: {modelDb.fcValorenLetras} CTVS. EXACTOS\n\n\n\n\n").SetFontSize(9f).SetBold());

        //            var tblFirmas = new Table(UnitValue.CreatePercentArray(new float[] { 15f, 17f, 2f, 18f, 14f, 2f, 16f, 16f })).SetWidth(UnitValue.CreatePercentValue(100)).SetHorizontalAlignment(HorizontalAlignment.LEFT);
        //            tblFirmas.AddCell(new Cell(1, 1).Add(new Paragraph("No. Orden Exenta:").SetBold().SetTextAlignment(TextAlignment.JUSTIFIED)).SetBorder(Border.NO_BORDER).SetFontSize(9));
        //            tblFirmas.AddCell(new Cell(1, 1).Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1f)));
        //            tblFirmas.AddCell(new Cell(1, 1).SetBorder(Border.NO_BORDER));


        //            tblFirmas.AddCell(new Cell(1, 1).Add(new Paragraph("Constancia Exonerada:").SetBold().SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER).SetFontSize(8));
        //            tblFirmas.AddCell(new Cell(1, 1).Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1f)));
        //            tblFirmas.AddCell(new Cell(1, 1).SetBorder(Border.NO_BORDER));

        //            tblFirmas.AddCell(new Cell(1, 1).Add(new Paragraph("No. Registro SAG:").SetBold().SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER).SetFontSize(9));
        //            tblFirmas.AddCell(new Cell(1, 1).Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1f)));

        //            tblFirmas.AddCell(new Cell(1, 8).Add(new Paragraph("Original: Cliente").SetBold().SetFontSize(9)).SetBorder(Border.NO_BORDER));
        //            tblFirmas.AddCell(new Cell(1, 8).Add(new Paragraph("Copia: Obligado Tributario").SetBold().SetFontSize(9)).SetBorder(Border.NO_BORDER));
        //            doc.Add(tblFirmas);
        //            doc.Add(new Paragraph("\n\n\n\n"));

        //            var tblLineaFirmaYSello = new Table(new float[] { 20f, 60f, 20f }).SetWidth(UnitValue.CreatePercentValue(100)).SetHorizontalAlignment(HorizontalAlignment.LEFT);
        //            tblLineaFirmaYSello.AddCell(new Cell(1, 1).SetBorder(Border.NO_BORDER));
        //            tblLineaFirmaYSello.AddCell(new Cell(1, 1).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1f)));
        //            tblLineaFirmaYSello.AddCell(new Cell(1, 1).SetBorder(Border.NO_BORDER));
        //            doc.Add(tblLineaFirmaYSello);
        //            doc.Add(new Paragraph("Firma y Sello\n").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));
        //            doc.Add(new Paragraph("LA FACTURA ES BENEFICIO DE TODOS, EXIJALA").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8));

        //        }
        //        doc.Close();
        //        var bytesStream = ms.ToArray();
        //        ms = new MemoryStream();
        //        ms.Write(bytesStream, 0, bytesStream.Length);
        //        ms.Position = 0;
        //        return ms;
        //    }
        //}


        
    }



}