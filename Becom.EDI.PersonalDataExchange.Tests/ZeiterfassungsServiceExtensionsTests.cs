using Moq;
using System;
using System.Xml.Linq;
using Xunit;
using Becom.EDI.PersonalDataExchange.Extensions;
using FluentAssertions;
using Becom.EDI.PersonalDataExchange.Model.Enums;

namespace Becom.EDI.PersonalDataExchange.Tests
{
    public class ZeiterfassungsServiceExtensionsTests
    {
        [Fact]
        public void ToDateShort()
        {
            var ele = new XElement("top");
            ele.Add(new XElement("test", "010418"));

            var res = ele.ToDateShort("test");

            res.Should().Be(new DateTime(2018, 4, 1));
        }

        [Fact]
        public void ToDate()
        {
            var ele = new XElement("top");
            ele.Add(new XElement("test", "20180401"));

            var res = ele.ToDate("test");

            res.Should().Be(new DateTime(2018, 4, 1));
        }

        [Fact]
        public void ToDate2_1()
        {
            var ele = new XElement("top");
            ele.Add(new XElement("test", "1042018"));

            var res = ele.ToDate2("test");

            res.Should().Be(new DateTime(2018, 4, 1));
        }

        [Fact]
        public void ToDate2_2()
        {
            var ele = new XElement("top");
            ele.Add(new XElement("test", "22042018"));

            var res = ele.ToDate2("test");

            res.Should().Be(new DateTime(2018, 4, 22));
        }

        [Fact]
        public void FromDate()
        {
            var d = new DateTime(2018, 4, 22);

            var res = d.FromDate();

            res.Should().Be("22042018");
        }
        [Fact]
        public void ToInt()
        {
            var ele = new XElement("top");
            ele.Add(new XElement("test", "2"));

            var res = ele.ToInt("test");

            res.Should().Be(2);
        }

        [Fact]
        public void ToCompany()
        {
            var ele = new XElement("top");
            ele.Add(new XElement("test", "001"));

            var res = ele.ToCompany("test");

            res.Should().Be(CompanyEnum.Austria);
        }

        [Fact]
        public void ToEmployeeId()
        {
            var ele = new XElement("top");
            ele.Add(new XElement("test", "5555"));

            var res = ele.ToCompany("test");

            res.Should().Be(5555);
        }

        [Fact]
        public void ToMinutesTimeSpan()
        {
            var ele = new XElement("top");
            ele.Add(new XElement("test", "155.0"));

            var res = ele.ToMinutesTimeSpan("test");

            res.Should().Be(TimeSpan.FromMinutes(155));
        }

        [Fact]
        public void ToMinutesTimeSpan2()
        {
            var ele = new XElement("top");
            ele.Add(new XElement("test", "155.3"));

            var res = ele.ToMinutesTimeSpan("test");

            res.Should().Be(TimeSpan.FromMinutes(155.3));
        }

        [Fact]
        public void ToHourTimeSpan()
        {
            var ele = new XElement("top");
            ele.Add(new XElement("test", "88"));

            var res = ele.ToHourTimeSpan("test");

            res.Should().Be(TimeSpan.FromHours(88));
        }

        [Fact]
        public void ToTime()
        {
            var ele = new XElement("top");
            ele.Add(new XElement("test", "11:45"));

            var thed = new DateTime(2020, 11, 8);
            var res = ele.ToTime("test", thed);

            res.Should().Be(new DateTime(2020, 11, 8, 11, 45, 0));
        }

    }
}
