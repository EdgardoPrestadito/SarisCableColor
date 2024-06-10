using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using OrionCoreCableColor.App_Helper;
using OrionCoreCableColor.App_Services.EmailService;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace OrionCoreCableColor.App_Services.ContratoService
{
    public class ContratoTemplateService
    {
        
        private SendEmailService _emailService;


        public ContratoTemplateService()
        {
            var ConnectionString = ConfigurationManager.ConnectionStrings["OrionConnections"].ConnectionString; // DataCrypt.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString);
            

            _emailService = new SendEmailService();

        }


        public string pathToOpen { get; set; }
        public string pathToSave { get; set; }

        public Dictionary<string, string> DictionaryList = new Dictionary<string, string>();



        public byte[] ResizeImage(byte[] data, float Twidth, float Theight)
        {
            using (var ms = new MemoryStream(data))
            {
                var image = System.Drawing.Image.FromStream(ms);

                //var ratioX = 150/Twidth; //(double)150 / image.Width;
                //var ratioY = 50/Theight; //(double)50 / image.Height;

                //var ratio = Math.Min(ratioX, ratioY);

                //var width = (int)(image.Width * ratio);
                //var height = (int)(image.Height * ratio);

                //var newImage = new Bitmap(width, height);
                var newImage = new Bitmap((int)Twidth, (int)Theight);
                Graphics.FromImage(newImage).DrawImage(image, 0, 0, Twidth, Theight);

                Bitmap bmp = new Bitmap(newImage);

                ImageConverter converter = new ImageConverter();

                data = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

                return Convert.FromBase64String(Convert.ToBase64String(data));
            }
        }


        


        


        


        public string ParseVariablesInContratoTemplate(string contratoTemplate, Dictionary<string, string> variableValues)
        {

            var codeStartDelimiter = "{";
            var codeEndDelimiter = "}";

            var escapeCharacters = new[] { ".", "$", "{", "}", "[", "]", "^", "(", ")", "|", "*", "+", "?", @"\" };

            var delStartReg = $"{codeStartDelimiter}";
            var delEndReg = $"{codeEndDelimiter}";

            if (escapeCharacters.Contains(delStartReg)) delStartReg = $"\\{delStartReg}";
            if (escapeCharacters.Contains(delEndReg)) delEndReg = $"\\{delEndReg}";

            var regexp = new Regex(delStartReg + "[^" + delStartReg + delEndReg + "]*" + delEndReg);

            var matches = regexp.Matches(contratoTemplate);

            foreach (Match match in matches)
            {
                if (!match.Value.Contains("BLACK") && !match.Value.Contains("TITULO"))
                {
                    var code = match.Value.Replace(codeStartDelimiter, "");
                    code = code.Replace(codeEndDelimiter, "");

                    var codeResult = variableValues[code];


                    contratoTemplate = contratoTemplate.Replace(match.Value, codeResult);
                }


            }

            return contratoTemplate;

        }


        public Paragraph SetTextNegritas(string contratoTemplate)
        {

            var parrafo = new Paragraph("").SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED);
            var codeStartDelimiter = "{";
            var codeEndDelimiter = "}";

            var escapeCharacters = new[] { ".", "$", "{", "}", "[", "]", "^", "(", ")", "|", "*", "+", "?", @"\" };

            var delStartReg = $"{codeStartDelimiter}";
            var delEndReg = $"{codeEndDelimiter}";

            if (escapeCharacters.Contains(delStartReg)) delStartReg = $"\\{delStartReg}";
            if (escapeCharacters.Contains(delEndReg)) delEndReg = $"\\{delEndReg}";

            var regexp = new Regex(delStartReg + "[^" + delStartReg + delEndReg + "]*" + delEndReg);
            var indice = 0;
            var matches = regexp.Matches(contratoTemplate).Cast<Match>().Select(x => x.Value).Where(x => x.Contains("BLACK")).ToList();

            foreach (var match in matches)
            {


                var ultimoIndex = contratoTemplate.IndexOf(match);
                var textoNormal = contratoTemplate.Substring(indice, ultimoIndex);
                var textoCompleto = textoNormal + match;
                parrafo.Add(new Text(textoNormal));
                parrafo.Add(new Text(match.Replace("BLACK", "").Replace("{","").Replace("}","")).SetBold());
                //indice = ultimoIndex;
                contratoTemplate = contratoTemplate.Replace(textoCompleto, "");

            }

            parrafo.Add(new Text(contratoTemplate));

            return parrafo;
        }


        public Table SetTituloContrato(string titulo)
        {

            
            
            var TablaTitulo = new Table(UnitValue.CreatePercentArray(new float[] { 30f, 40f, 30f })).SetWidth(UnitValue.CreatePercentValue(100)).SetHorizontalAlignment(HorizontalAlignment.CENTER);

            var imagenPrestadito = new iText.Layout.Element.Image(ImageDataFactory.Create(System.IO.Path.Combine(MemoryLoadManager.URL, @"Content\img\LogoPrestadito.png")));
            var imagenNovanet = new iText.Layout.Element.Image(ImageDataFactory.Create(System.IO.Path.Combine(MemoryLoadManager.URL, @"Content\img\NOVANETLOGO.png")));



            var logoPrestadito = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(imagenPrestadito.Scale(0.8f, 1f)).SetBorder(Border.NO_BORDER);
            var TituloContrato = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(titulo).SetBold().SetFontSize(10)).SetBorder(Border.NO_BORDER);
            var logoNovanet = new Cell(1, 1).SetTextAlignment(TextAlignment.RIGHT).Add(imagenNovanet.Scale(0.035f, 0.05f).SetHorizontalAlignment(HorizontalAlignment.RIGHT)).SetBorder(Border.NO_BORDER);

            TablaTitulo.AddCell(logoPrestadito).AddCell(TituloContrato.SetHorizontalAlignment(HorizontalAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE)).AddCell(logoNovanet);

            

            return TablaTitulo;
        }

        public Table SetTituloPagare(string titulo)
        {



            var TablaTitulo = new Table(UnitValue.CreatePercentArray(new float[] { 30f, 40f, 30f })).SetWidth(UnitValue.CreatePercentValue(100)).SetHorizontalAlignment(HorizontalAlignment.CENTER);

            var imagenPrestadito = new iText.Layout.Element.Image(ImageDataFactory.Create(System.IO.Path.Combine(MemoryLoadManager.URL, @"Content\img\LogoPrestadito.png")));
            var imagenNovanet = new iText.Layout.Element.Image(ImageDataFactory.Create(System.IO.Path.Combine(MemoryLoadManager.URL, @"Content\img\NOVANETLOGO.png")));



            var logoPrestadito = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(imagenPrestadito.Scale(0.8f, 1f)).SetBorder(Border.NO_BORDER);
            var TituloContrato = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(titulo).SetBold().SetFontSize(10)).SetBorder(Border.NO_BORDER);
            var logoNovanet = new Cell(1, 1).SetTextAlignment(TextAlignment.RIGHT).Add(imagenNovanet.Scale(0.035f, 0.05f).SetHorizontalAlignment(HorizontalAlignment.RIGHT)).SetBorder(Border.NO_BORDER);

            TablaTitulo.AddCell(logoPrestadito).AddCell(TituloContrato.SetHorizontalAlignment(HorizontalAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE)).AddCell(logoNovanet);



            return TablaTitulo;
        }

        public Table setFirmasContrato(string firmaNovanet, string firmaCliente)
        {
            firmaNovanet = firmaNovanet.Replace("data:image/png;base64,", "");
            firmaCliente = firmaCliente.Replace("data:image/png;base64,", "");

            

            var TablaFirmas = new Table(UnitValue.CreatePercentArray(new float[] { 47f, 6f, 47f })).SetWidth(UnitValue.CreatePercentValue(100)).SetHorizontalAlignment(HorizontalAlignment.CENTER);
            var firmaNovanetArray = Convert.FromBase64String(firmaNovanet);
            var firmaClienteArray = Convert.FromBase64String(firmaCliente);


            var dataNovanet = ImageDataFactory.Create(ResizeImage(firmaNovanetArray, 200f, 100f));
            var dataCliente = ImageDataFactory.Create(ResizeImage(firmaClienteArray, 200f, 100f));

            var imagenFirmaNovanet = new iText.Layout.Element.Image(dataNovanet);
            var imagenFirmaCliente = new iText.Layout.Element.Image(dataCliente);

            

            
            
            
            var cellFirmaNovanet = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(imagenFirmaNovanet).SetBorder(Border.NO_BORDER);
            var cellFirmaCliente = new Cell(1, 1).SetTextAlignment(TextAlignment.RIGHT).Add(imagenFirmaCliente).SetBorder(Border.NO_BORDER);




            TablaFirmas
                .AddCell(cellFirmaCliente.SetBorderBottom(new SolidBorder(1f)))
                .AddCell(new Cell().SetBorder(Border.NO_BORDER))
                .AddCell(cellFirmaNovanet.SetBorderBottom(new SolidBorder(1f)));

            /////TEXTO
            TablaFirmas
                .AddCell(new Cell().Add(new Paragraph("EL CLIENTE").SetTextAlignment(TextAlignment.CENTER)).SetBorder(Border.NO_BORDER))
                .AddCell(new Cell().SetBorder(Border.NO_BORDER))
                .AddCell(new Cell().Add(new Paragraph("NOVANET").SetTextAlignment(TextAlignment.CENTER)).SetBorder(Border.NO_BORDER));

            

            return TablaFirmas;

        }



        public Table setFirmasPagare(string firmaCliente)
        {
            
            firmaCliente = firmaCliente.Replace("data:image/png;base64,", "");
            var TablaFirmas = new Table(UnitValue.CreatePercentArray(new float[] { 10f, 80f, 10f })).SetWidth(UnitValue.CreatePercentValue(100)).SetHorizontalAlignment(HorizontalAlignment.CENTER);
            var firmaClienteArray = Convert.FromBase64String(firmaCliente);
            var dataCliente = ImageDataFactory.Create(ResizeImage(firmaClienteArray, 300f, 150f));
            var imagenFirmaCliente = new iText.Layout.Element.Image(dataCliente);
            var cellFirmaCliente = new Cell(1, 1).SetTextAlignment(TextAlignment.RIGHT).Add(imagenFirmaCliente).SetBorder(Border.NO_BORDER);
            TablaFirmas
                .AddCell(new Cell().SetBorder(Border.NO_BORDER))
                .AddCell(cellFirmaCliente.SetBorderBottom(new SolidBorder(1f)))
                .AddCell(new Cell().SetBorder(Border.NO_BORDER));
   
            TablaFirmas
                .AddCell(new Cell().SetBorder(Border.NO_BORDER))
                .AddCell(new Cell().Add(new Paragraph("Firma del cliente").SetTextAlignment(TextAlignment.CENTER)).SetBorder(Border.NO_BORDER))
                .AddCell(new Cell().SetBorder(Border.NO_BORDER));



            return TablaFirmas;

        }


       


        

    }
}