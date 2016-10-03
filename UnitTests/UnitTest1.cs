using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.IO;

namespace LINQToXML
{
    [TestClass]
    public class UnitTests
    {
        WorkWithXml workWithXml;
        [TestInitialize]
        public void Initialize()
        {
            workWithXml = new WorkWithXml();
            workWithXml.StartReading();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            workWithXml = null;
        }

        [TestMethod]
        public void TesetCustomersWithSumOfOrdersMoreThan()
        {
            workWithXml.CustomersWithSumOfOrdersMoreThan(1000);
        }
        [TestMethod]
        public void TesetGroupByCountry()
        {
            workWithXml.GroupByCountry();
        }
        [TestMethod]
        public void TesetTotalOfOrderMoreThan()
        {
            workWithXml.TotalOfOrderMoreThan(1000);
        }
        [TestMethod]
        public void TesetFirstDateOfOrder()
        {
            workWithXml.FirstDateOfOrder();
        }
        [TestMethod]
        public void TesetOrderCustomersByYearByMonthBySum()
        {
            workWithXml.OrderCustomersByYearByMonthBySum();
        }
        [TestMethod]
        public void TesetFindEmptyFields()
        {
            workWithXml.FindEmptyFields();
        }
        [TestMethod]
        public void TesetGetAverageSumPerCity()
        {
            workWithXml.AverageSumPerCity();
            workWithXml.IntensivePerCity();
        }
        [TestMethod]
        public void TesetActivityOfCustomerByMonth()
        {
            workWithXml.ActivityOfCustomerByMonth();
            workWithXml.ActivityOfCustomerByYear();
            workWithXml.ActivityOfCustomerByMonthPerYears();
        }
    }

}


