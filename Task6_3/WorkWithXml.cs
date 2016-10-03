using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LINQToXML
{
    public class WorkWithXml
    {
        string FileName = $@"{Environment.CurrentDirectory}\Customers.xml";
        XDocument Document;
        public void StartReading()
        {
            Document = XDocument.Load(FileName);
        }
        public void CustomersWithSumOfOrdersMoreThan(int n)
        {

            var a = Document
                .Elements("customers")
                .Elements("customer")
                .Select(x => new
                {
                    name = x.Element("name").Value,
                    sum = x.Elements("orders").Elements("order").Elements("total")
                .Select(y => double.Parse(y.Value)).Sum(),
                })
                .Where(x => x.sum > n)
                .ToList();
            using (StreamWriter file = new StreamWriter($@"{Environment.CurrentDirectory}\CustomersWithSumOfOrdersMoreThan{n}.txt"))
            {
                foreach (var f in a)
                {
                    file.WriteLine(f.name);
                }
            }
        }
        public void GroupByCountry()
        {
            var a = Document
                .Elements("customers")
                .Elements("customer")
                .Select(x => new
                {
                    name = x.Element("name").Value,
                    country = x.Element("country").Value,
                })
                 .GroupBy(x => x.country)
                 .ToList();

            using (StreamWriter file = new StreamWriter($@"{Environment.CurrentDirectory}\CustomersGroupedByCountry.txt"))
            {
                foreach (var f in a)
                {
                    file.WriteLine(f.Key);
                    foreach (var name in f)
                    {
                        file.WriteLine(name.name);
                    }
                }
            }
        }
        public void TotalOfOrderMoreThan(int n)
        {
            var a = Document
                .Elements("customers")
                .Elements("customer")
                .Select(x => new
                {
                    name = x.Element("name").Value,
                    totalOfOrder = x.Elements("orders").Elements("order").Any(y => Double.Parse(y.Element("total").Value) > n),
                })
               .ToList();
            using (StreamWriter file = new StreamWriter($@"{Environment.CurrentDirectory}\CustomersWithTotalInOrderMoreThan{n}.txt"))
            {
                foreach (var f in a)
                {
                    file.WriteLine(f.name);
                    file.WriteLine(f.totalOfOrder);
                }
            }
        }
        public void FirstDateOfOrder()
        {
            var a = Document
                .Elements("customers")
                .Elements("customer")
                .Select(x => new
                {
                    name = x.Element("name").Value,
                    firstDateOfOrder = x.Elements("orders").Elements("order").Elements("orderdate").Min(y => y.Value)
                })
                .ToList();
            using (StreamWriter file = new StreamWriter($@"{Environment.CurrentDirectory}\CustomersFirstDateOfOrder.txt"))
            {
                foreach (var f in a)
                {
                    file.WriteLine(f.name);
                    file.WriteLine(f.firstDateOfOrder);
                }
            }
        }
        public void OrderCustomersByYearByMonthBySum()
        {
            var a = Document
                .Elements("customers")
                .Elements("customer")
                .Select(x => new
                {
                    name = x.Element("name").Value,
                    firstDateOfOrder = x.Elements("orders").Elements("order").Elements("orderdate").Min(y => y.Value),
                    sum = x.Elements("orders").Elements("order").Elements("total")
                .Select(y => double.Parse(y.Value.Replace(".", ","))).Sum(),
                })
                .OrderBy(x => Convert.ToDateTime(x.firstDateOfOrder).Year)
                .ThenBy(x => Convert.ToDateTime(x.firstDateOfOrder).Month)
                .ThenByDescending(x => x.sum)
                .ToList();
            using (StreamWriter file = new StreamWriter($@"{Environment.CurrentDirectory}\OrderedCustomersByYearByMonthBySum.txt"))
            {
                foreach (var f in a)
                {
                    file.WriteLine(f.name);
                }
            }
        }
        public void FindEmptyFields()
        {
            int i;
            var a = Document
                .Elements("customers")
                .Elements("customer")
                .Select(x => new
                {
                    name = x.Element("name").Value,
                    postalcode = x.Element("postalcode")?.Value,
                    region = x.Element("region")?.Value,
                    phone = x.Element("phone")?.Value,
                })
                .Where(x => !Int32.TryParse(x.postalcode, out i))
                .Where(x => x.phone.Contains("("))
                .Where(x => x.region == null)
                .ToList();
            using (StreamWriter file = new StreamWriter($@"{Environment.CurrentDirectory}\CustomersWithEptyFields.txt"))
            {
                foreach (var f in a)
                {
                    file.WriteLine(f.name);
                }
            }
        }
        public void AverageSumPerCity()
        {
            var a = Document
                .Elements("customers")
                .Elements("customer")
                .GroupBy(x => x.Element("city").Value)
                .ToDictionary(k => k.Key, k => k
                .Sum(orders => orders.Element("orders").Elements("order")
                .Sum(order => Decimal.Parse(order.Element("total").Value))) / k
                .Sum(order => order.Element("orders").Elements("order").Count()));
            using (StreamWriter file = new StreamWriter($@"{Environment.CurrentDirectory}\AverageSumOfCity.txt"))
            {
                foreach (var f in a)
                {
                    file.WriteLine(f.Key);
                    file.WriteLine(f.Value);
                }
            }
        }
        public void IntensivePerCity()
        {
            var a = Document
               .Elements("customers")
               .Elements("customer")
               .GroupBy(x => x.Element("city").Value)
               .ToDictionary(k => k.Key, k => k
               .Sum(orders => orders.Element("orders").Elements("order").Count()) / k
               .Count());
            using (StreamWriter file = new StreamWriter($@"{Environment.CurrentDirectory}\IntensivePerCity.txt"))
            {
                foreach (var f in a)
                {
                    file.WriteLine(f.Key);
                    file.WriteLine(f.Value);
                }
            }
        }

        public void ActivityOfCustomerByMonth()
        {
            var a = Document
                .Elements("customers")
                .Elements("customer")
                .Elements("orders")
                .Elements("order")
                 .Select(x => new
                 {
                     name = x.Element("name")?.Value,
                     orderCount = x.Descendants("orderdate").Count(),
                     orderdate = x.Element("orderdate")?.Value,
                 })
                .GroupBy(x => Convert.ToDateTime(x.orderdate).Month)
                .ToDictionary(k => k.Key, k => k
               .Sum(orders => orders.orderCount));
            using (StreamWriter file = new StreamWriter($@"{Environment.CurrentDirectory}\ActivityOfCustomerByMonth.txt"))
            {
                foreach (var f in a)
                {
                    file.WriteLine($"Month: {f.Key}");
                    file.WriteLine($"Quantity of orders: {f.Value}");
                }
            }
        }
        public void ActivityOfCustomerByYear()
        {
            var a = Document
                .Elements("customers")
                .Elements("customer")
                .Elements("orders")
                .Elements("order")
                 .Select(x => new
                 {
                     name = x.Element("name")?.Value,
                     orderCount = x.Descendants("orderdate").Count(),
                     orderdate = x.Element("orderdate")?.Value,
                 })
                .GroupBy(x => Convert.ToDateTime(x.orderdate).Year)
                .ToDictionary(k => k.Key, k => k
               .Sum(orders => orders.orderCount));
            using (StreamWriter file = new StreamWriter($@"{Environment.CurrentDirectory}\ActivityOfCustomerByYear.txt"))
            {
                foreach (var f in a)
                {
                    file.WriteLine($"Year: {f.Key}");
                    file.WriteLine($"Quantity of orders: {f.Value}");
                }
            }
        }
        public void ActivityOfCustomerByMonthPerYears()
        {
            var a = Document
              .Elements("customers")
                .Elements("customer")
                .Elements("orders")
                .Elements("order")
                 .Select(x => new
                 {
                     name = x.Element("name")?.Value,
                     orderCount = x.Descendants("orderdate").Count(),
                     orderdate = x.Element("orderdate")?.Value,
                 })
                .GroupBy(x => Convert.ToDateTime(x.orderdate).Date.ToString("yyyy-MM"))
                .ToDictionary(k => k.Key, k => k
               .Sum(orders => orders.orderCount));
            using (StreamWriter file = new StreamWriter($@"{Environment.CurrentDirectory}\ActivityOfCustomerByMonthPerYears.txt"))
            {
                foreach (var f in a)
                {
                    file.WriteLine($"Year and Month: {f.Key}");
                    file.WriteLine($"Quantity of orders: {f.Value}");
                }
            }
        }
    }
}
