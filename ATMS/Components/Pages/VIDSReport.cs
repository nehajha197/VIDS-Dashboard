 using ClosedXML.Excel;
using Microsoft.JSInterop;
using MudBlazor;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using static ATMS.DTOs.DTO;
namespace ATMS.Components.Pages
{
    public partial class VIDSReport
    {
        private VIDSReportViewModel ReportData { get; set; } = new();
        private bool IsLoading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            IsLoading = true;
            try
            {
                await LoadAllData();
                StateHasChanged();
                Snackbar.Add("Report data loaded successfully", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error loading data: {ex.Message}", Severity.Error);
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private async Task LoadAllData()
        {
            var tasks = new List<Task>
        {
            LoadAverageVehicleCount(),
            LoadVehicleClass(),
            LoadDeviceStatus(),
            LoadViolatedVehicle(),
            LoadClassCategory(),
            LoadLastTransaction(),
            LoadPassingVehicles(),
            LoadViolationCategories(),
            LoadTrafficCounts(),
            LoadHeaderData()
        };

            await Task.WhenAll(tasks);

            ReportData.ReportDate = DateTime.Now;
            ReportData.ReportGeneratedBy = "System";
            ReportData.ReportTitle = "VIDS Comprehensive Report";
        }

        private async Task LoadHeaderData()
        {
            try
            {
                var data = await VidsDataService.GetActiveProjectHeaderAsync();
                ReportData.Header = data ?? new MainHeaderDto();
            }
            catch { ReportData.Header = new MainHeaderDto(); }
        }

        private async Task LoadAverageVehicleCount()
        {
            try
            {
                var data = await VidsDataService.GetAverageVehicleCountAsync();
                ReportData.AverageVehicleCount = data?.ToList() ?? new List<AverageVehicleCountDto>();
            }
            catch { ReportData.AverageVehicleCount = new List<AverageVehicleCountDto>(); }
        }

        private async Task LoadVehicleClass()
        {
            try
            {
                var data = await VidsDataService.GetVehicleClassAsync();
                ReportData.VehicleClass = data ?? new VehicleClassDto();
            }
            catch { ReportData.VehicleClass = new VehicleClassDto(); }
        }

        private async Task LoadDeviceStatus()
        {
            try
            {
                var data = await VidsDataService.GetDeviceStatusAsync();
                ReportData.DeviceStatus = data ?? new DeviceStatusDto();
            }
            catch { ReportData.DeviceStatus = new DeviceStatusDto(); }
        }

        private async Task LoadViolatedVehicle()
        {
            try
            {
                var data = await VidsDataService.GetViolatedVehicleAsync();
                ReportData.ViolatedVehicle = data ?? new ViolatedVehicleDto();
            }
            catch { ReportData.ViolatedVehicle = new ViolatedVehicleDto(); }
        }

        private async Task LoadClassCategory()
        {
            try
            {
                var data = await VidsDataService.GetClassCategoryAsync();
                ReportData.ClassCategory = data?.ToList() ?? new List<ClassCategoryDto>();
            }
            catch { ReportData.ClassCategory = new List<ClassCategoryDto>(); }
        }

        private async Task LoadLastTransaction()
        {
            try
            {
                var data = await VidsDataService.GetLastTransactionAsync();
                ReportData.LastTransaction = data ?? new LastTransactionDto();
            }
            catch { ReportData.LastTransaction = new LastTransactionDto(); }
        }

        private async Task LoadPassingVehicles()
        {
            try
            {
                var data = await VidsDataService.GetPassingVehiclesAsync();
                ReportData.PassingVehicles = data?.ToList() ?? new List<PassingVehicleDto>();
            }
            catch { ReportData.PassingVehicles = new List<PassingVehicleDto>(); }
        }

        private async Task LoadViolationCategories()
        {
            try
            {
                var data = await VidsDataService.GetViolationCategoryAsync();
                ReportData.ViolationCategories = data?.ToList() ?? new List<ViolationCategoryDto>();
            }
            catch { ReportData.ViolationCategories = new List<ViolationCategoryDto>(); }
        }

        private async Task LoadTrafficCounts()
        {
            try
            {
                var data = await VidsDataService.GetTrafficCountAsync();
                ReportData.TrafficCounts = data?.ToList() ?? new List<TrafficCountDto>();
            }
            catch { ReportData.TrafficCounts = new List<TrafficCountDto>(); }
        }

        private async Task GeneratePDF()
        {
            try
            {
                IsLoading = true;
                Snackbar.Add("Generating PDF report...", Severity.Info);

                // Set QuestPDF License
                QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

                // Generate the PDF
                var pdfBytes = GenerateSummaryReportPDF();

                // Convert to Base64 and download
                var base64String = Convert.ToBase64String(pdfBytes);

                // Call JavaScript to download
                await JSRuntime.InvokeVoidAsync("window.downloadFile",
                    $"VIDS_Summary_Report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf",
                    base64String);

                Snackbar.Add("PDF generated successfully", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error generating PDF: {ex.Message}", Severity.Error);
                Console.WriteLine($"PDF Generation Error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        private byte[] GenerateSummaryReportPDF()
        {
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    // Header
                    page.Header()
                        .PaddingBottom(15)
                        .BorderBottom(3)
                        .BorderColor(QuestPDF.Helpers.Colors.Blue.Darken2)
                        .Column(col =>
                        {
                            col.Item().AlignCenter().Text("VIDS SUMMARY REPORT")
                                .FontSize(18).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                        });

                    // Content
                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        // Report Metadata 
                        column.Item().PaddingBottom(13)
                         .PaddingVertical(15)
                         .PaddingLeft(15)
                         .PaddingRight(15)
                            .Border(1)
                            .BorderColor(QuestPDF.Helpers.Colors.Black)
                            .Column(meta =>
                            {
                                meta.Spacing(2);

                                meta.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingLeft(7).Text(text =>
                                    {
                                        text.Span("From Date: ").Bold();
                                        text.Span(ReportData.ReportDate.ToString("dd-MM-yyyy 00:00:00"));
                                    });
                                    row.RelativeItem().Text(text =>
                                    {
                                        text.Span("To Date: ").Bold();
                                        text.Span(ReportData.ReportDate.ToString("dd-MM-yyyy 23:59:00"));
                                    });
                                });

                                meta.Item().Row(row =>
                                {
                                    // Get distinct locations
                                    string locations = "All Locations";
                                    if (ReportData.TrafficCounts != null && ReportData.TrafficCounts.Any())
                                    {
                                        var locList = ReportData.TrafficCounts.Select(x => x.TrafficCount).Distinct().ToList();
                                        locations = string.Join(", ", locList);
                                    }

                                    row.RelativeItem().PaddingLeft(7).Text(text =>
                                    {
                                        text.Span("Location: ").Bold();
                                        text.Span(locations);
                                    });
                                    row.RelativeItem().Text(text =>
                                    {
                                        text.Span("Vehicle Class: ").Bold();
                                        text.Span("Auto, Bike, Car, LCV, Truck, Bus, Tractor, Pickup, Others");
                                    });
                                });
                            });

                        // Summary Table
                        column.Item().PaddingTop(10).Table(table =>
                        {
                            var vehicleClassColumns = new (string Label, Func<ClassCategoryDto, int> Selector)[]
                            {
                        ("Auto", c => c.Auto),
                        ("Bike", c => c.Bike),
                        ("Car", c => c.Car),
                        ("LCV", c => c.LCV),
                        ("Truck", c => c.Truck),
                        ("Bus", c => c.Bus),
                        ("Tractor", c => c.Tractor),
                        ("Pickup", c => c.Pickup),
                        ("Others", c => c.Others),
                            };

                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(70);
                                foreach (var col in vehicleClassColumns)
                                {
                                    columns.RelativeColumn();
                                }
                                columns.ConstantColumn(50);
                            });

                            // Table Header
                            table.Header(header =>
                            {
                                header.Cell().Border(1).BorderColor(QuestPDF.Helpers.Colors.Black)
                                    .Background(QuestPDF.Helpers.Colors.Blue.Lighten2)
                                    .Padding(6).Text("Location")
                                    .FontColor(QuestPDF.Helpers.Colors.Black).Bold().FontSize(10);

                                foreach (var col in vehicleClassColumns)
                                {
                                    header.Cell().Border(1).BorderColor(QuestPDF.Helpers.Colors.Black)
                                        .Background(QuestPDF.Helpers.Colors.Blue.Lighten2)
                                        .Padding(6)
                                        .AlignCenter()
                                        .Text(col.Label)
                                        .FontColor(QuestPDF.Helpers.Colors.Black).Bold().FontSize(10);
                                }

                                header.Cell().Border(1).BorderColor(QuestPDF.Helpers.Colors.Black)
                                    .Background(QuestPDF.Helpers.Colors.Blue.Lighten2)
                                    .Padding(6)
                                    .AlignCenter()
                                    .Text("Total")
                                    .FontColor(QuestPDF.Helpers.Colors.Black).Bold().FontSize(10);
                            });

                            // Table Body
                            if (ReportData.ClassCategory != null && ReportData.ClassCategory.Any())
                            {
                                bool rowColor = false;
                                var grandTotals = new int[vehicleClassColumns.Length];
                                int grandTotal = 0;

                                foreach (var row in ReportData.ClassCategory)
                                {
                                    rowColor = !rowColor;
                                    var bodyBackground = QuestPDF.Helpers.Colors.Blue.Lighten4;

                                    table.Cell().Border(1).BorderColor(QuestPDF.Helpers.Colors.Black)
                                        .Background(bodyBackground)
                                        .Padding(6).Text(row.Location ?? "-")
                                        .FontColor(QuestPDF.Helpers.Colors.Black).Bold();

                                    int rowTotal = 0;
                                    for (int i = 0; i < vehicleClassColumns.Length; i++)
                                    {
                                        var value = vehicleClassColumns[i].Selector(row);
                                        rowTotal += value;
                                        grandTotals[i] += value;

                                        table.Cell().Border(1).BorderColor(QuestPDF.Helpers.Colors.Black)
                                            .Background(bodyBackground)
                                            .Padding(6)
                                            .AlignCenter()
                                            .Text(value.ToString())
                                            .FontSize(10).Bold()
                                            .FontColor(QuestPDF.Helpers.Colors.Black);
                                    }

                                    grandTotal += rowTotal;

                                    table.Cell().Border(1).BorderColor(QuestPDF.Helpers.Colors.Black)
                                        .Background(bodyBackground)
                                        .Padding(6)
                                        .AlignCenter()
                                        .Text(rowTotal.ToString())
                                        .FontSize(10).Bold()
                                        .FontColor(QuestPDF.Helpers.Colors.Black);
                                }

                                // Grand Total Row
                                table.Cell().Border(1).BorderColor(QuestPDF.Helpers.Colors.Black)
                                    .Background(QuestPDF.Helpers.Colors.Blue.Lighten2)
                                    .Padding(8).Text("GRAND TOTAL")
                                    .FontColor(QuestPDF.Helpers.Colors.Black).Bold().FontSize(9);

                                foreach (var _ in vehicleClassColumns)
                                {
                                    table.Cell().Border(1).BorderColor(QuestPDF.Helpers.Colors.Black)
                                        .Background(QuestPDF.Helpers.Colors.Blue.Lighten2)
                                        .Padding(8)
                                        .AlignCenter()
                                        .Text("");
                                }

                                table.Cell().Border(1).BorderColor(QuestPDF.Helpers.Colors.Black)
                                    .Background(QuestPDF.Helpers.Colors.Blue.Lighten2)
                                    .Padding(8)
                                    .AlignCenter()
                                    .Text(grandTotal.ToString())
                                    .FontColor(QuestPDF.Helpers.Colors.Black).Bold().FontSize(10);
                            }
                        });
                    });

                    // Footer
                    page.Footer()
                        .PaddingTop(10)
                        .BorderTop(1)
                        .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1)
                        .Row(row =>
                        {
                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Generated: ").FontSize(8).FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);
                                text.Span(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"))
                                    .FontSize(8).FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);

                                text.Span("    Generated By: ").FontSize(8).FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);
                                text.Span("Administrator")
                                    .FontSize(8).FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);
                            });

                            row.RelativeItem().AlignRight().Text(text =>
                            {
                                text.Span("Page ").FontSize(8).FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);
                                text.CurrentPageNumber().FontSize(8).FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);
                            });
                        });
                });
            });

            return document.GeneratePdf();
        }

        private void ClearFilters() => Snackbar.Add("Filters cleared", Severity.Info);
        private void ViewAllData() => Snackbar.Add("Loading all data...", Severity.Info);
        private async Task ExportToExcel()
        {
            try
            {
                IsLoading = true;
                Snackbar.Add("Generating Excel report...", Severity.Info);

                var excelBytes = GenerateSummaryReportExcel();
                var base64String = Convert.ToBase64String(excelBytes);

                await JSRuntime.InvokeVoidAsync("window.downloadExcelFile",
                    $"VIDS_Summary_Report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx",
                    base64String);

                Snackbar.Add("Excel file generated successfully", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error generating Excel: {ex.Message}", Severity.Error);
                Console.WriteLine($"Excel Generation Error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private byte[] GenerateSummaryReportExcel()
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("VIDS Summary Report");

            // Title
            ws.Cell(1, 1).Value = "VIDS SUMMARY REPORT";
            ws.Range(1, 1, 1, 11).Merge();
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 16;
            ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#42a5f5");
            ws.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Metadata
            ws.Cell(3, 1).Value = "From Date:";
            ws.Cell(3, 2).Value = ReportData.ReportDate.ToString("dd-MM-yyyy 00:00:00");
            ws.Cell(3, 4).Value = "To Date:";
            ws.Cell(3, 5).Value = ReportData.ReportDate.ToString("dd-MM-yyyy 23:59:00");

            string locations = "All Locations";
            if (ReportData.TrafficCounts != null && ReportData.TrafficCounts.Any())
            {
                locations = string.Join(", ", ReportData.TrafficCounts.Select(x => x.TrafficCount).Distinct());
            }

            ws.Cell(4, 1).Value = "Location:";
            ws.Cell(4, 2).Value = locations;
            ws.Cell(4, 4).Value = "Vehicle Class:";
            ws.Cell(4, 5).Value = "Auto, Bike, Car, LCV, Truck, Bus, Tractor, Pickup, Others";

            ws.Range(3, 1, 4, 5).Style.Font.Bold = false;
            ws.Range(3, 1, 4, 1).Style.Font.Bold = true;
            ws.Range(4, 1, 4, 1).Style.Font.Bold = true;
            ws.Range(3, 4, 3, 4).Style.Font.Bold = true;
            ws.Range(4, 4, 4, 4).Style.Font.Bold = true;

            // Table header
            int headerRow = 6;
            var vehicleClassColumns = new (string Label, Func<ClassCategoryDto, int> Selector)[]
            {
 ("Auto",    c => c.Auto),
 ("Bike",    c => c.Bike),
 ("Car",     c => c.Car),
 ("LCV",     c => c.LCV),
 ("Truck",   c => c.Truck),
 ("Bus",     c => c.Bus),
 ("Tractor", c => c.Tractor),
 ("Pickup",  c => c.Pickup),
 ("Others",  c => c.Others),
            };

            ws.Cell(headerRow, 1).Value = "Location";
            for (int i = 0; i < vehicleClassColumns.Length; i++)
                ws.Cell(headerRow, i + 2).Value = vehicleClassColumns[i].Label;
            ws.Cell(headerRow, vehicleClassColumns.Length + 2).Value = "Total";

            var headerRange = ws.Range(headerRow, 1, headerRow, vehicleClassColumns.Length + 2);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontColor = XLColor.Black;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#90caf9");
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Table body
            int row = headerRow + 1;
            var grandTotals = new int[vehicleClassColumns.Length];
            int grandTotal = 0;

            if (ReportData.ClassCategory != null && ReportData.ClassCategory.Any())
            {
                foreach (var item in ReportData.ClassCategory)
                {
                    ws.Cell(row, 1).Value = item.Location ?? "-";
                    ws.Cell(row, 1).Style.Font.Bold = true;

                    int rowTotal = 0;
                    for (int i = 0; i < vehicleClassColumns.Length; i++)
                    {
                        var value = vehicleClassColumns[i].Selector(item);
                        rowTotal += value;
                        grandTotals[i] += value;
                        ws.Cell(row, i + 2).Value = value;
                        ws.Cell(row, i + 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }

                    grandTotal += rowTotal;
                    ws.Cell(row, vehicleClassColumns.Length + 2).Value = rowTotal;
                    ws.Cell(row, vehicleClassColumns.Length + 2).Style.Font.Bold = true;
                    ws.Cell(row, vehicleClassColumns.Length + 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    row++;
                }

                // Grand total row
                ws.Cell(row, 1).Value = "GRAND TOTAL";
                var grandRow = ws.Range(row, 1, row, vehicleClassColumns.Length + 2);
                grandRow.Style.Font.Bold = true;
                grandRow.Style.Font.FontColor = XLColor.Black;
                grandRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#64b5f6");
                ws.Cell(row, vehicleClassColumns.Length + 2).Value = grandTotal;
                ws.Cell(row, vehicleClassColumns.Length + 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++; // move past grand total row
            }


            // Footer: Generated / Generated By
            int footerRow = row + 2; // leave a blank row for spacing
            ws.Cell(footerRow, 1).Value = "Generated: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            ws.Cell(footerRow, 1).Style.Font.Italic = false;
            ws.Cell(footerRow, 1).Style.Font.FontSize = 9;
            ws.Cell(footerRow, 1).Style.Font.FontColor = XLColor.Gray;

            ws.Cell(footerRow, 4).Value = "Generated By: Administrator";
            ws.Cell(footerRow, 4).Style.Font.Italic = false;
            ws.Cell(footerRow, 4).Style.Font.FontSize = 9;
            ws.Cell(footerRow, 4).Style.Font.FontColor = XLColor.Gray;

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        private void GoToDashboard() => NavigationManager.NavigateTo("/dashboard");
    }
}

