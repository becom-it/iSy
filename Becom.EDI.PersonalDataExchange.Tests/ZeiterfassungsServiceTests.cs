using Becom.EDI.PersonalDataExchange.Model;
using Becom.EDI.PersonalDataExchange.Model.Enums;
using Becom.EDI.PersonalDataExchange.Services;
using Becom.EDI.PersonalDataExchange.Tests.Helpers;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Becom.EDI.PersonalDataExchange.Tests
{
    public class ZeiterfassungsServiceTests
    {
        #region GetEmployeeInfoTest
        [Fact]
        public async Task GetEmployeeInfoTestEnum()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeInfoResponse());


            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var info = await service.GetEmployeeInfo(CompanyEnum.Austria, 5555);
            
            info.Should().BeOfType<EmployeeInfo>();
            
            info.FirstName.Should().Be("Michael");
            info.LastName.Should().Be("Prattinger");
            info.ManagerDisciplinary.Should().Be(5022);
            info.ManagerProfessional.Should().Be(5022);
            info.EntryDate.Should().Be(new System.DateTime(2018, 4, 3));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeInfoRequestContent(1, 5555))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeInfoTest()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeInfoResponse());


            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var info = await service.GetEmployeeInfo(1, 5555);

            info.Should().BeOfType<EmployeeInfo>();

            info.FirstName.Should().Be("Michael");
            info.LastName.Should().Be("Prattinger");
            info.ManagerDisciplinary.Should().Be(5022);
            info.ManagerProfessional.Should().Be(5022);
            info.EntryDate.Should().Be(new System.DateTime(2018, 4, 3));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeInfoRequestContent(1, 5555))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeInfo_Error_Wrong_Company_Test()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeInfoErrorCompanyResponse());


            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetEmployeeInfo(789, 5555));
                     
            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeInfoRequestContent(789, 5555))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeInfo_Error_No_Data_Test()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeInfoErrorNoDataResponse());


            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetEmployeeInfo(CompanyEnum.Austria, 1234));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeInfoRequestContent(1, 1234))),
                ItExpr.IsAny<CancellationToken>());
        }
        #endregion

        #region GetEmployeeList
        [Fact]
        public async Task GetEmployeeListEnumTest()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeListResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var list = await service.GetEmployeeList(CompanyEnum.Austria);
            
            list.Should().BeOfType<List<EmployeeBaseInfo>>();
            list.Count.Should().Be(4);
            
            list.First().FirstName.Should().Be("Martin");
            list.First().LastName.Should().Be("Auer");
            list.First().EmployeeId.Should().Be(5120);
            
            list.Last().FirstName.Should().Be("Manfred");
            list.Last().LastName.Should().Be("Augustin");
            list.Last().EmployeeId.Should().Be(1425);

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeListRequestContent(1))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeListTest()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeListResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var list = await service.GetEmployeeList(1);

            list.Should().BeOfType<List<EmployeeBaseInfo>>();
            list.Count.Should().Be(4);

            list.First().FirstName.Should().Be("Martin");
            list.First().LastName.Should().Be("Auer");
            list.First().EmployeeId.Should().Be(5120);

            list.Last().FirstName.Should().Be("Manfred");
            list.Last().LastName.Should().Be("Augustin");
            list.Last().EmployeeId.Should().Be(1425);

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeListRequestContent(1))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeList_Error_Wrong_Company_Test()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeListErrorCompanyResponse());


            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetEmployeeList(789));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeListRequestContent(789))),
                ItExpr.IsAny<CancellationToken>());
        }
        #endregion

        #region GetEmployeeTimeDetails
        [Fact]
        public async Task GetEmployeeTimeDetailsEnumTest()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeTimeDetailsResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var list = await service.GetEmployeeTimeDetails(CompanyEnum.Austria, 5555, new DateTime(2020, 10, 1), new DateTime(2020, 10, 30));

            list.Should().BeOfType<List<EmployeeTimeDetail>>();
            list.Count.Should().Be(30);

            list.First().GrossWorktime.Should().Be(TimeSpan.FromMinutes(519));
            list.First().TargetWorktime.Should().Be(TimeSpan.FromMinutes(492));
            list.First().NetWorktime.Should().Be(TimeSpan.FromMinutes(519));
            list.First().NetWorktimeDifference.Should().Be(TimeSpan.FromMinutes(27));

            list.Last().GrossWorktime.Should().Be(TimeSpan.FromMinutes(362));
            list.Last().TargetWorktime.Should().Be(TimeSpan.FromMinutes(342));
            list.Last().NetWorktime.Should().Be(TimeSpan.FromMinutes(362));
            list.Last().NetWorktimeDifference.Should().Be(TimeSpan.FromMinutes(20));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeTimeDetailsRequestContent(1, 5555, "1102020", "30102020"))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeTimeDetailsTest()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeTimeDetailsResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var list = await service.GetEmployeeTimeDetails(1, 5555, new DateTime(2020, 10, 1), new DateTime(2020, 10, 30));

            list.Should().BeOfType<List<EmployeeTimeDetail>>();
            list.Count.Should().Be(30);

            list.First().GrossWorktime.Should().Be(TimeSpan.FromMinutes(519));
            list.First().TargetWorktime.Should().Be(TimeSpan.FromMinutes(492));
            list.First().NetWorktime.Should().Be(TimeSpan.FromMinutes(519));
            list.First().NetWorktimeDifference.Should().Be(TimeSpan.FromMinutes(27));

            list.Last().GrossWorktime.Should().Be(TimeSpan.FromMinutes(362));
            list.Last().TargetWorktime.Should().Be(TimeSpan.FromMinutes(342));
            list.Last().NetWorktime.Should().Be(TimeSpan.FromMinutes(362));
            list.Last().NetWorktimeDifference.Should().Be(TimeSpan.FromMinutes(20));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeTimeDetailsRequestContent(1, 5555, "1102020", "30102020"))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeTimeDetails_Error_Wrong_Company_Test()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeTimeDetailsErrorCompanyResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetEmployeeTimeDetails(789, 5555, new DateTime(2020, 10, 1), new DateTime(2020, 10, 30)));
            
            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeTimeDetailsRequestContent(789, 5555, "1102020", "30102020"))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeTimeDetails_Error_NoData_Test()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeTimeDetailsErrorNoDataResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetEmployeeTimeDetails(1, 5555, new DateTime(2015, 10, 1), new DateTime(2015, 10, 30)));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeTimeDetailsRequestContent(1, 5555, "1102015", "30102015"))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeTimeDetails_Error_Wrong_Employee_Test()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeTimeDetailsErrorWrongEmployeeResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetEmployeeTimeDetails(1, 1234, new DateTime(2020, 10, 1), new DateTime(2020, 10, 30)));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeTimeDetailsRequestContent(1, 1234, "1102020", "30102020"))),
                ItExpr.IsAny<CancellationToken>());
        }
        #endregion

        #region GetEmployePresenceStatus
        [Fact]
        public async Task GetEmployeePresenceStatusEnumTest()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeePresenceStatusResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var status = await service.GetEmployeePresenceStatus(CompanyEnum.Austria, 5555);

            status.Should().BeOfType<EmployeePresenceStatus>();
            status.EmployeeId.Should().Be(5555);
            status.CurrentDate.Should().Be(new DateTime(2020, 11, 11));
            status.Type.Should().Be(PresenceType.AB);

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeePresenceStatusRequestContent(1, 5555))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeePresenceStatusTest()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeePresenceStatusResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var status = await service.GetEmployeePresenceStatus(1, 5555);

            status.Should().BeOfType<EmployeePresenceStatus>();
            status.EmployeeId.Should().Be(5555);
            status.CurrentDate.Should().Be(new DateTime(2020, 11, 11));
            status.Type.Should().Be(PresenceType.AB);

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeePresenceStatusRequestContent(1, 5555))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeePresenceStatus_Error_Wrong_Company_Test()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeePresenceStatusErrorCompanyResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetEmployeePresenceStatus(789, 5555));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeePresenceStatusRequestContent(789, 5555))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeePresenceStatus_NoData_Test()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeePresenceStatusErrorNoDataResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetEmployeePresenceStatus(1, 1234));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeePresenceStatusRequestContent(1, 1234))),
                ItExpr.IsAny<CancellationToken>());
        }
        #endregion

        #region GetEmployeeCheckIns
        [Fact]
        public async Task GetEmployeeCheckInsEnumTest()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeCheckInsResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var list = await service.GetEmployeeCheckIns(CompanyEnum.Austria, 5555, new DateTime(2020, 11, 5));
            list.Count.Should().Be(6);

            list.First().CheckinTime.Should().Be(new DateTime(2020, 11, 5, 6, 30, 0));
            list.First().Type.Should().Be(PresenceType.AN);

            list[2].CheckinTime.Should().Be(new DateTime(2020, 11, 5, 7, 41, 0));
            list[2].Type.Should().Be(PresenceType.AN);

            list.Last().CheckinTime.Should().Be(new DateTime(2020, 11, 5, 16, 0, 0));
            list.Last().Type.Should().Be(PresenceType.AB);

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeCheckInsRequestContent(1, 5555, "5112020"))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeCheckInsTest()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeCheckInsResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var list = await service.GetEmployeeCheckIns(1, 5555, new DateTime(2020, 11, 5));
            list.Count.Should().Be(6);

            list.First().CheckinTime.Should().Be(new DateTime(2020, 11, 5, 6, 30, 0));
            list.First().Type.Should().Be(PresenceType.AN);

            list[2].CheckinTime.Should().Be(new DateTime(2020, 11, 5, 7, 41, 0));
            list[2].Type.Should().Be(PresenceType.AN);

            list.Last().CheckinTime.Should().Be(new DateTime(2020, 11, 5, 16, 0, 0));
            list.Last().Type.Should().Be(PresenceType.AB);

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeCheckInsRequestContent(1, 5555, "5112020"))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeCheckIns_Error_Wrong_Company_Test()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeCheckInsErrorCompanyResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetEmployeeCheckIns(789, 5555, new DateTime(2020, 11, 5)));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeCheckInsRequestContent(789, 5555, "5112020"))),
                ItExpr.IsAny<CancellationToken>());
        }      

        [Fact]
        public async Task GetEmployeeCheckIns_Error_NoData_Test()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeCheckInsErrorNoDataResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetEmployeeCheckIns(1, 5555, new DateTime(2015, 11, 5)));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeCheckInsRequestContent(1, 5555, "5112015"))),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetEmployeeCheckIns_Error_Wrong_Employee_Test()
        {
            //Arrange
            var mocks = MockHelpers.GetMocks(ResponseContents.GetEmployeeCheckInsErrorWrongEmployeeResponse());

            //Act
            var service = new ZeiterfassungsService(mocks.logger, mocks.mockFactory.Object, mocks.config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetEmployeeCheckIns(1, 1234, new DateTime(2020, 11, 5)));

            mocks.mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.CheckRequest(mocks.config.Endpoint, RequestContents.GetEmployeeCheckInsRequestContent(1, 1234, "5112020"))),
                ItExpr.IsAny<CancellationToken>());
        }
        #endregion
    }
}
