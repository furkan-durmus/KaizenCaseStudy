using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace JsonToText
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = File.ReadAllText("response.json");

            List<InvoiceDetailInfo> invoiceItems = JsonConvert.DeserializeObject<List<InvoiceDetailInfo>>(json);
            List<InvoiceDetailInfo> sortedInvoiceByRow = invoiceItems.OrderBy(i => i.BoundingPoly.Vertices[0].Y).ToList();

            int row = 0;
            int coordinateInfo = 0;
            InvoiceItems invoiceListForRowAssign = new();
            InvoiceItems finalInvoice = new();

            foreach (var invoice in sortedInvoiceByRow)
            {
                InvoiceDetailInfo invoiceDetailInfo = new();

                if (invoice.BoundingPoly.Vertices[0].Y > (coordinateInfo + 15))
                    row++;

                invoiceDetailInfo.Description = invoice.Description;
                invoiceDetailInfo.Row = row;
                invoiceDetailInfo.BoundingPoly = invoice.BoundingPoly;
                coordinateInfo = invoice.BoundingPoly.Vertices[0].Y;
                invoiceListForRowAssign.Items.Add(invoiceDetailInfo);
            }

            for (int i = 0; i < invoiceListForRowAssign.Items.Max(i => i.Row) + 1; i++)
            {
                List<InvoiceDetailInfo> invoiceItemsForLineOrder = invoiceListForRowAssign.Items.Where(x => x.Row == i).OrderBy(x => x.BoundingPoly.Vertices[0].X).ToList();

                foreach (var item in invoiceItemsForLineOrder)
                {
                    finalInvoice.Items.Add(item);
                }
            }

            int lineInfo = 0;

            foreach (var line in finalInvoice.Items)
            {
                if (line.Row > lineInfo)
                {
                    Console.WriteLine();
                    lineInfo = line.Row;
                    Console.Write($"{line.Row}");
                }

                Console.Write($" {line.Description}");
            }
        }
    }

    public class InvoiceDetailInfo
    {
        public int Row { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public BoundingPoly BoundingPoly { get; set; }
    }

    public class InvoiceItems
    {
        public InvoiceItems()
        {
            Items = new List<InvoiceDetailInfo>();
        }

        public List<InvoiceDetailInfo> Items { get; set; }
    }

    public class BoundingPoly
    {
        public List<Coordinate> Vertices { get; set; }
    }

    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
