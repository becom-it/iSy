using Becom.EDI.PersonalDataExchange.Model;
using Becom.EDI.PersonalDataExchange.Model.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Becom.EDI.PersonalDataExchange.Extensions;
using System.Collections.Generic;
using Becom.EDI.PersonalDataExchange.Model.Config;

namespace Becom.EDI.PersonalDataExchange.Services
{
    public interface IZeiterfassungsService
    {
        /// <summary>
        /// Es wird der Personalstammsatz zurückgegeben
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <returns>Personalstammsatz</returns>
        Task<EmployeeInfo> GetEmployeeInfo(CompanyEnum betrieb, int employeeId);

        /// <summary>
        /// Es wird der Personalstammsatz zurückgegeben
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <returns>Personalstammsatz</returns>
        Task<EmployeeInfo> GetEmployeeInfo(int betrieb, int employeeId);

        /// <summary>
        /// Es wird eine Liste der Mitarbeiter ausgegeben
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <returns></returns>
        Task<List<EmployeeBaseInfo>> GetEmployeeList(CompanyEnum betrieb);

        /// <summary>
        /// Es wird eine Liste der Mitarbeiter ausgegeben
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <returns></returns>
        Task<List<EmployeeBaseInfo>> GetEmployeeList(int betrieb);

        /// <summary>
        /// Zeiterfassungsdetails in ausgewähltem Zeitraum
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <param name="From">Datum von</param>
        /// <param name="To">Datum bis</param>
        /// <returns>Zeiterfassungsdetails</returns>
        Task<List<EmployeeTimeDetail>> GetEmployeeTimeDetails(CompanyEnum betrieb, int employeeId, DateTime From, DateTime To);

        /// <summary>
        /// Zeiterfassungsdetails in ausgewähltem Zeitraum
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <param name="From">Datum von</param>
        /// <param name="To">Datum bis</param>
        /// <returns>Zeiterfassungsdetails</returns>
        Task<List<EmployeeTimeDetail>> GetEmployeeTimeDetails(int betrieb, int employeeId, DateTime From, DateTime To);

        /// <summary>
        /// Aktueller Anwesenheitsstatusstatus einer Person. Information kommt live aus Zeiterfassungssystem
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <returns>Anwesenheitsstatusstatus</returns>
        Task<EmployeePresenceStatus> GetEmployeePresenceStatus(CompanyEnum betrieb, int employeeId);

        /// <summary>
        /// Aktueller Anwesenheitsstatusstatus einer Person. Information kommt live aus Zeiterfassungssystem
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <returns>Anwesenheitsstatusstatus</returns>
        Task<EmployeePresenceStatus> GetEmployeePresenceStatus(int betrieb, int employeeId);

        /// <summary>
        /// Gibt eine Liste mit allen Statusänderungen an einem Tag zurück
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <param name="ForDate">Abgfrage Datum</param>
        /// <returns>Liste mit allen Statusänderungen an einem Tag</returns>
        Task<List<EmployeeCheckIn>> GetEmployeeCheckIns(CompanyEnum betrieb, int employeeId, DateTime ForDate);

        /// <summary>
        /// Gibt eine Liste mit allen Statusänderungen an einem Tag zurück
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <param name="ForDate">Abgfrage Datum</param>
        /// <returns>Liste mit allen Statusänderungen an einem Tag</returns>
        Task<List<EmployeeCheckIn>> GetEmployeeCheckIns(int betrieb, int employeeId, DateTime ForDate);

        /// <summary>
        /// Gibt eine Liste der Abwesenheitskennzeichen zurück (Tabellenart ZEKZ aus der Tabelle DTG0LF)
        /// </summary>
        /// <returns>Liste der Abwesenheitskennzeichen</returns>
        Task<List<ZeiterfassungsCustomizing>> GetZeiterfassungsCustomizing();
    }

    public class ZeiterfassungsService : IZeiterfassungsService
    {
        private readonly ILogger<ZeiterfassungsService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PersonalDataExchangeConfig _config;
        private readonly IIBMiSQLApi _sqlApi;

        public ZeiterfassungsService(ILogger<ZeiterfassungsService> logger, IHttpClientFactory httpClientFactory, PersonalDataExchangeConfig config, IIBMiSQLApi sqlApi)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _config = config;
            _sqlApi = sqlApi;
        }

        #region EmployeeInfo
        /// <summary>
        /// Es wird der Personalstammsatz zurückgegeben
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <returns>Personalstammsatz</returns>
        public async Task<EmployeeInfo> GetEmployeeInfo(CompanyEnum betrieb, int employeeId) => await GetEmployeeInfo((int)betrieb, employeeId);

        /// <summary>
        /// Es wird der Personalstammsatz zurückgegeben
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <returns>Personalstammsatz</returns>
        public async Task<EmployeeInfo> GetEmployeeInfo(int betrieb, int employeeId)
        {
            try
            {
                _logger.LogInformation($"Loading employee info from webservice with company code {(int)betrieb} and for employeeId {employeeId}...");
                var xml = _config.EmployeeInfoRequest
                    .Replace("company", betrieb.ToString())
                    .Replace("employeeid", employeeId.ToString());
                _logger.LogDebug($"Posting request xml: {xml}");

                var result = await callEndpoint(xml);
                _logger.LogDebug($"Received: {result}");

                _logger.LogInformation("Mapping result into EmployeeInfo object...");
                XDocument doc = XDocument.Parse(result);
                return doc.Descendants().Where(x => x.Name.LocalName == "getPersonal")
                    .Select(x => new EmployeeInfo
                    {
                        Company = x.ToCompany("pnbtrm"),
                        EmployeeId = x.ToEmployeeId("pnpern"),
                        FirstName = x.Element(x.Name.Namespace + "pnvnam").Value.Trim(),
                        LastName = x.Element(x.Name.Namespace + "pnname").Value.Trim(),
                        ManagerDisciplinary = x.ToInt("pnvgsd"),
                        ManagerProfessional = x.ToInt("pnvgsf"),
                        EntryDate = x.ToDateShort("pneind"),
                        WorktimeSaldo = x.ToMinutesTimeSpan("pnazss"),
                        FreeTimeOption = x.ToHourTimeSpan("pnfzop"),
                        HolidayAvailiable = x.ToInt("pnurlg"),
                        HolidayUsed = x.ToInt("pnurlv")
                    }).FirstOrDefault();
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling GetEmployeeInfo (getPersonal): {ex.Message}", ex);
            }
        }
        #endregion

        #region EmployeeList
        /// <summary>
        /// Es wird eine Liste der Mitarbeiter ausgegeben
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <returns></returns>
        public async Task<List<EmployeeBaseInfo>> GetEmployeeList(CompanyEnum betrieb) => await GetEmployeeList((int)betrieb);

        /// <summary>
        /// Es wird eine Liste der Mitarbeiter ausgegeben
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <returns></returns>
        public async Task<List<EmployeeBaseInfo>> GetEmployeeList(int betrieb)
        {
            try
            {
                _logger.LogInformation($"Loading employees from webservice with company code {(int)betrieb}...");
                var xml = _config.EmployeeListRequest
                    .Replace("company", betrieb.ToString());
                _logger.LogDebug($"Posting request xml: {xml}");

                var result = await callEndpoint(xml);
                _logger.LogDebug($"Received: {result}");

                _logger.LogInformation("Mapping result into EmployeeInfo list...");
                XDocument doc = XDocument.Parse(result);
                return doc.Descendants().Where(x => x.Name.LocalName == "personalList")
                    .Select(x => new EmployeeBaseInfo
                    {
                        Company = x.ToCompany("pnbtrm"),
                        EmployeeId = x.ToEmployeeId("pnpern"),
                        FirstName = x.Element(x.Name.Namespace + "pnvnam").Value.Trim(),
                        LastName = x.Element(x.Name.Namespace + "pnname").Value.Trim(),
                    }).ToList();
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling GetEmployeeList (getPersonalList): {ex.Message}", ex);
            }
        }
        #endregion

        #region EmployeeTimeDetails
        /// <summary>
        /// Zeiterfassungsdetails in ausgewähltem Zeitraum
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <param name="From">Datum von</param>
        /// <param name="To">Datum bis</param>
        /// <returns>Zeiterfassungsdetails</returns>
        public async Task<List<EmployeeTimeDetail>> GetEmployeeTimeDetails(CompanyEnum betrieb, int employeeId, DateTime From, DateTime To) => await GetEmployeeTimeDetails((int)betrieb, employeeId, From, To);

        /// <summary>
        /// Zeiterfassungsdetails in ausgewähltem Zeitraum
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <param name="From">Datum von</param>
        /// <param name="To">Datum bis</param>
        /// <returns>Zeiterfassungsdetails</returns>
        public async Task<List<EmployeeTimeDetail>> GetEmployeeTimeDetails(int betrieb, int employeeId, DateTime From, DateTime To)
        {
            try
            {
                _logger.LogInformation($"Loading employees time details from webservice with company code {(int)betrieb} and employeeId {employeeId} between {From.ToShortDateString()} and {To.ToShortDateString()}...");
                var xml = _config.EmployeeTimeDetailsRequest
                .Replace("company", (betrieb).ToString())
                .Replace("employeeid", employeeId.ToString())
                .Replace("fromdate", From.FromDate())
                .Replace("todate", To.FromDate());
                _logger.LogDebug($"Posting request xml: {xml}");

                var result = await callEndpoint(xml);
                _logger.LogDebug($"Received: {result}");

                _logger.LogInformation("Mapping result into EmployeeTimeDetail list...");
                XDocument doc = XDocument.Parse(result);
                return doc.Descendants().Where(x => x.Name.LocalName == "zeiterfassungList")
                    .Select(x => new EmployeeTimeDetail
                    {
                        Company = x.ToCompany("pnbtrm"),
                        EmployeeId = x.ToEmployeeId("pnpern"),
                        AbsentKey1 = x.Element(x.Name.Namespace + "zeaht1").Value.Trim(),
                        AbsentKey2 = x.Element(x.Name.Namespace + "zeaht2").Value.Trim(),
                        GrossWorktime = x.ToMinutesTimeSpan("zeazbr"),
                        GrossWorktimeDifference = x.ToMinutesTimeSpan("zeazdb"),
                        WorktimeOffSite = x.ToMinutesTimeSpan("zeazdi"),
                        NetWorktimeDifference = x.ToMinutesTimeSpan("zeazdn"),
                        NetWorktime = x.ToMinutesTimeSpan("zeazna"),
                        GrossWorktimeBalance = x.ToMinutesTimeSpan("zeazsb"),
                        TargetWorktime = x.ToMinutesTimeSpan("zeazso"),
                        WorktimeBalanceSum = x.ToMinutesTimeSpan("zeazss"),
                        WorkOvertime = x.ToMinutesTimeSpan("zeazue"),
                        PresenceDate = x.ToDate("zedate"),
                        PresenceDate2 = x.ToDate2("zedat2"),
                        FactoryDate = (int)x.Element(x.Name.Namespace + "zefkal"),
                        TimeType = (int)x.Element(x.Name.Namespace + "zesart")
                    }).ToList();
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling GetEmployeeTimeDetails (getZeiterfassung): {ex.Message}", ex);
            }
        }
        #endregion

        #region EmployeePresenceStatus
        /// <summary>
        /// Aktueller Anwesenheitsstatusstatus einer Person. Information kommt live aus Zeiterfassungssystem
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <returns>Anwesenheitsstatusstatus</returns>
        public async Task<EmployeePresenceStatus> GetEmployeePresenceStatus(CompanyEnum betrieb, int employeeId) => await GetEmployeePresenceStatus((int)betrieb, employeeId);

        public async Task<EmployeePresenceStatus> GetEmployeePresenceStatus(int betrieb, int employeeId)
        {
            try
            {
                _logger.LogInformation($"Loading employees presence status from webservice with company code {(int)betrieb} and employeeId {employeeId}...");
                var xml = _config.EmployeePresenceStatusRequest
                .Replace("company", (betrieb).ToString())
                .Replace("employeeid", employeeId.ToString());
                _logger.LogDebug($"Posting request xml: {xml}");

                var result = await callEndpoint(xml);
                _logger.LogDebug($"Received: {result}");

                _logger.LogInformation("Mapping result into EmployeePresenceStatus object...");
                XDocument doc = XDocument.Parse(result);
                return doc.Descendants().Where(x => x.Name.LocalName == "getPersonalStatus")
                    .Select(x => new EmployeePresenceStatus
                    {
                        Company = x.ToCompany("pnbtrm"),
                        EmployeeId = x.ToEmployeeId("pnpern"),
                        CurrentDate = x.ToDate2("z1date"),
                        Type = x.Element(x.Name.Namespace + "z1stat").Value == "AN" ? PresenceType.AN : PresenceType.AB
                    }).FirstOrDefault();
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling GetEmployeePresenceStatus (getPersonalStatus): {ex.Message}", ex);
            }
        }
        #endregion

        #region EmployeeCheckIns
        /// <summary>
        /// Gibt eine Liste mit allen Statusänderungen an einem Tag zurück
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <param name="ForDate">Abgfrage Datum</param>
        /// <returns>Liste mit allen Statusänderungen an einem Tag</returns>
        public async Task<List<EmployeeCheckIn>> GetEmployeeCheckIns(CompanyEnum betrieb, int employeeId, DateTime ForDate) => await GetEmployeeCheckIns((int)betrieb, employeeId, ForDate);

        /// <summary>
        /// Gibt eine Liste mit allen Statusänderungen an einem Tag zurück
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        /// <param name="employeeId">Personalnummer</param>
        /// <param name="ForDate">Abgfrage Datum</param>
        /// <returns>Liste mit allen Statusänderungen an einem Tag</returns>
        public async Task<List<EmployeeCheckIn>> GetEmployeeCheckIns(int betrieb, int employeeId, DateTime ForDate)
        {
            try
            {
                _logger.LogInformation($"Loading employees checkins from webservice with company code {(int)betrieb} and employeeId {employeeId} for {ForDate.ToShortDateString()}...");
                var xml = _config.EmployeeCheckInsRequest
                .Replace("company", (betrieb).ToString())
                .Replace("employeeid", employeeId.ToString())
                .Replace("fordate", ForDate.FromDate());
                _logger.LogDebug($"Posting request xml: {xml}");

                var result = await callEndpoint(xml);
                _logger.LogDebug($"Received: {result}");

                _logger.LogInformation("Mapping result into EmployeeCheckIn list...");
                XDocument doc = XDocument.Parse(result);
                return doc.Descendants().Where(x => x.Name.LocalName == "personalStautsList")
                    .Select(x => new EmployeeCheckIn
                    {
                        Company = x.ToCompany("pnbtrm"),
                        EmployeeId = x.ToEmployeeId("pnpern"),
                        AskedForDate = x.ToDate2("z1date"),
                        Type = x.Element(x.Name.Namespace + "z1stat").Value == "AN" ? PresenceType.AN : PresenceType.AB,
                        CheckinTime = x.ToTime("z1time", x.ToDate2("z1date")),
                        AbsenceType = x.Element(x.Name.Namespace + "z1aht1").Value
                    }).ToList();
            }
            catch(ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling GetEmployeeCheckIns (getPersonalStatusList): {ex.Message}", ex);
            }
        }
        #endregion

        #region Customizing
        public async Task<List<ZeiterfassungsCustomizing>> GetZeiterfassungsCustomizing()
        {
            try
            {
                var res = await _sqlApi.CallSqlService<ZeiterfassungsCustomizing>(_config.ZeiterfassungsCustomizingQuery);
                var grouped = res.GroupBy(x => x.AbscenceKey).Select(x => x.FirstOrDefault()).ToList();
                return grouped;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading the Zeiterfassungs customizing: {ex.Message}", ex);
            }
        }
        #endregion

        private async Task<string> callEndpoint(string xml)
        {
            string resultString;
            try
            {
                var httpContent = new StringContent(xml, Encoding.UTF8, "text/xml");

                _logger.LogDebug("Creating http client...");

                var client = _httpClientFactory.CreateClient("edi");


                _logger.LogDebug($"Calling endpoint...");
                var result = await client.PostAsync("", httpContent);
                if (result.IsSuccessStatusCode)
                {
                    resultString = await result.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"Endpoint HTTP error {result.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling endpoint: {ex.Message}", ex);
            }

            //Prüfem auf Fehler
            checkForError(resultString);

            return resultString;
        }

        private static void checkForError(string content)
        {
            XDocument doc = XDocument.Parse(content);
            var errors = doc.Descendants().Where(x => x.Name.LocalName == "errors")
                .Select(x => new ErrorResponse
                {
                    ErrorCode = x.ToInt("erco"),
                    ErrorText = x.Element(x.Name.Namespace + "ertx").Value.Trim()
                });
            if(errors.Any())
            {
                throw new ArgumentException($"Error {errors.First().ErrorText}", new PersonalDataExchangeException(errors.First()));
            }
        }
    }
}
