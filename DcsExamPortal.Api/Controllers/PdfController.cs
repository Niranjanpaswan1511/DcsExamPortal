using DcsExamPortal.Api.Services;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace DcsExamPortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IPdfService _pdfService;

        public PdfController(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }
        [HttpGet("fee-receipt")]
        public async Task<IActionResult> GenerateFeeReceipt(string orderId)
        {
            var response = await _pdfService.GetDetailsByOrderId(orderId);
            if (!response.Success || response.Data == null)
                return NotFound(response);

            dynamic data = response.Data;

            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);


            document.SetMargins(40, 40, 40, 40);

            var primaryColor = new DeviceRgb(37, 99, 235);
            var secondaryColor = new DeviceRgb(241, 245, 249); 
            var successColor = new DeviceRgb(34, 197, 94);
            var textColor = new DeviceRgb(51, 65, 85); 

            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            var headerTable = new Table(1).UseAllAvailableWidth();
            var headerCell = new Cell()
                .Add(new Paragraph("FEE PAYMENT RECEIPT")
                    .SetFont(boldFont)
                    .SetFontSize(24)
                    .SetFontColor(ColorConstants.WHITE))
                .Add(new Paragraph("Official Payment Confirmation")
                    .SetFont(regularFont)
                    .SetFontSize(11)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetMarginTop(5))
                .SetBackgroundColor(primaryColor)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetPadding(25)
                .SetBorder(Border.NO_BORDER);

            headerTable.AddCell(headerCell);
            document.Add(headerTable);
            document.Add(new Paragraph("\n").SetFontSize(10));

            var statusBadge = new Paragraph("✓ PAYMENT SUCCESSFUL")
                .SetFont(boldFont)
                .SetFontSize(12)
                .SetFontColor(ColorConstants.WHITE)
                .SetBackgroundColor(successColor)
                .SetPadding(8)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetBorderRadius(new BorderRadius(4));

            document.Add(statusBadge);
            document.Add(new Paragraph("\n").SetFontSize(15));


            document.Add(new Paragraph("Student Information")
                .SetFont(boldFont)
                .SetFontSize(14)
                .SetFontColor(primaryColor)
                .SetMarginBottom(10));

            var studentTable = new Table(2).UseAllAvailableWidth();
            studentTable.SetMarginBottom(20);

            AddInfoRow(studentTable, "Name:", data.Name?.ToString() ?? "N/A", boldFont, regularFont, textColor);
            AddInfoRow(studentTable, "Email:", data.Email?.ToString() ?? "N/A", boldFont, regularFont, textColor);

            document.Add(studentTable);


            document.Add(new Paragraph("Payment Details")
                .SetFont(boldFont)
                .SetFontSize(14)
                .SetFontColor(primaryColor)
                .SetMarginBottom(10));

            var paymentTable = new Table(2).UseAllAvailableWidth();
            paymentTable.SetMarginBottom(20);

            AddInfoRow(paymentTable, "Form Title:", data.Title?.ToString() ?? "N/A", boldFont, regularFont, textColor);
            AddInfoRow(paymentTable, "Description:", data.Description?.ToString() ?? "N/A", boldFont, regularFont, textColor);
            AddInfoRow(paymentTable, "Order ID:", data.OrderId?.ToString() ?? "N/A", boldFont, regularFont, textColor);
            AddInfoRow(paymentTable, "Payment ID:", data.PaymentId?.ToString() ?? "N/A", boldFont, regularFont, textColor);

            DateTime paymentDate = data.CreatedAt ?? DateTime.Now;
            AddInfoRow(paymentTable, "Payment Date:", paymentDate.ToString("dd MMM yyyy, hh:mm tt"), boldFont, regularFont, textColor);

            document.Add(paymentTable);

            var amountTable = new Table(1).UseAllAvailableWidth();
            var amountCell = new Cell()
                .Add(new Paragraph("TOTAL AMOUNT PAID")
                    .SetFont(boldFont)
                    .SetFontSize(12)
                    .SetFontColor(textColor)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER))
                .Add(new Paragraph($"₹{data.Fee ?? 0:N2}")
                    .SetFont(boldFont)
                    .SetFontSize(28)
                    .SetFontColor(primaryColor)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetMarginTop(5))
                .SetBackgroundColor(secondaryColor)
                .SetPadding(20)
                .SetBorder(Border.NO_BORDER)
                .SetBorderRadius(new BorderRadius(8));

            amountTable.AddCell(amountCell);
            document.Add(amountTable);
            document.Add(new Paragraph("\n").SetFontSize(20));

            var footerLine = new SolidLine(0.5f);
            footerLine.SetColor(new DeviceRgb(203, 213, 225));
            var lineSeparator = new LineSeparator(footerLine);
            lineSeparator.SetMarginBottom(15);
            document.Add(lineSeparator);

            document.Add(new Paragraph("Thank you for your payment!")
                .SetFont(boldFont)
                .SetFontSize(12)
                .SetFontColor(textColor)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetMarginBottom(5));

            document.Add(new Paragraph("This is a computer-generated receipt and does not require a signature.")
                .SetFont(regularFont)
                .SetFontSize(9)
                .SetFontColor(new DeviceRgb(148, 163, 184))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetMarginBottom(10));

            document.Add(new Paragraph($"Generated on: {DateTime.Now:dd MMM yyyy, hh:mm tt}")
                .SetFont(regularFont)
                .SetFontSize(8)
                .SetFontColor(new DeviceRgb(148, 163, 184))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            document.Close();

            var fileName = $"FeeReceipt_{data.OrderId}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            return File(ms.ToArray(), "application/pdf", fileName);
        }


        private void AddInfoRow(Table table, string label, string value,
            PdfFont boldFont, PdfFont regularFont, DeviceRgb textColor)
        {
            var labelCell = new Cell()
                .Add(new Paragraph(label)
                    .SetFont(boldFont)
                    .SetFontSize(10)
                    .SetFontColor(textColor))
                .SetBorder(Border.NO_BORDER)
                .SetPaddingBottom(8)
                .SetPaddingTop(8)
                .SetBackgroundColor(new DeviceRgb(249, 250, 251));

            var valueCell = new Cell()
                .Add(new Paragraph(value)
                    .SetFont(regularFont)
                    .SetFontSize(10)
                    .SetFontColor(textColor))
                .SetBorder(Border.NO_BORDER)
                .SetPaddingBottom(8)
                .SetPaddingTop(8)
                .SetBackgroundColor(new DeviceRgb(249, 250, 251));

            table.AddCell(labelCell);
            table.AddCell(valueCell);
        }

    }
}
