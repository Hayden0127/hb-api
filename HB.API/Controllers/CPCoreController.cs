
using HB.Model;
using HB.Service;
using HB.SmartSD.Integrator;
using HB.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Strateq.Core.API.Controllers;
using Strateq.Core.API.Exceptions;
using Strateq.Core.Model;
using Strateq.Core.Service;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace HB.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CPCoreController : STQControllerBase
    {
        #region
        private readonly ISystemLogService _logger;
        private readonly ICPMonitoringService _cpMonitoringService;
        private readonly ICPTransactionService _cpTransactionService;
        private readonly ServiceHelper _smartSDServiceHelper;
        private string CPConnectorUrl;
        #endregion

        #region Ctor
        public CPCoreController(
            ISystemLogService logger,
            ICPMonitoringService cpMonitoringService, 
            ICPTransactionService cpTransactionService)
        {
            _logger = logger;
            _cpMonitoringService = cpMonitoringService;
            _cpTransactionService = cpTransactionService;
            _smartSDServiceHelper = new ServiceHelper(logger);
            CPConnectorUrl = Environment.GetEnvironmentVariable("CPCONNECTOR_API_URL") ?? string.Empty;
        }
        #endregion

        #region Methods

        [HttpGet]
        [AllowAnonymous]
        [Route("HeartBeat/{websocketId}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> HeartBeat([FromRoute] string websocketId)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                logger.Append($"HeartBeat Request WebSocket {websocketId}");
                HeartBeatConf response = await _cpMonitoringService.UpdateCPHeartBeatAsync(websocketId);
                await _logger.LogInformation(logger.ToString());

                return Ok(response);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("BootNotification")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> BootNotification(BootNotificationModel request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"BootNotification Timestamp {timestamp}");
                await _logger.LogInformation(logger.ToString());
                await _logger.LogInformation($"BootNotificationRequest: {JsonSerializer.Serialize(request)}");
                ResponseModelBase response = await _cpMonitoringService.BootNotification(request);

                return Ok(response);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.NotFound };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.Invalid };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }


        [HttpPost]
        [AllowAnonymous]
        [Route("StatusNotification")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> StatusNotification([FromBody, SwaggerRequestBody("Request body", Required = true)] UpdateCPStatusRequestModel request)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                logger.Append($"UpdateCPStatusAsync Request Timestamp {request.Timestamp}");
                logger.Append($"UpdateCPStatusAsync SerialNumber - {request.StatusNotification.VendorId}");
                await _cpMonitoringService.UpdateCPStatusAsync(request.StatusNotification);
                await _logger.LogInformation(logger.ToString());
                return Ok();
            }
            catch (SmartSDException ex)
            {
                await _logger.LogError("Error - ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            catch (Exception ex)
            {
                await _logger.LogError("Error - ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            finally
            {

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("StartTransaction")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> StartTransaction(StartTransactionRequestDetail request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                logger.Append($"StartTransaction Request Timestamp {request.timestamp}");
                ResponseModelBase response = await _cpTransactionService.StartTransaction(request);
                await _logger.LogInformation(logger.ToString());

                return Ok(response);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("StopTransaction")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> StopTransaction(StopTransactionReq request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                logger.Append($"StopTransaction Request Timestamp {request.timestamp}");
                ResponseModelBase response = await _cpTransactionService.StopTransaction(request);
                await _logger.LogInformation(logger.ToString());

                return Ok(response);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.NotFound };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.Invalid };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }



        [HttpGet]
        [AllowAnonymous]
        [Route("RemoteReboot/{type}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> RemoteReboot([FromRoute] int type)
        {
            StringBuilder logger = new StringBuilder();

            try
            {

                var url = new Uri(CPConnectorUrl);
                var exitEvent = new ManualResetEvent(false);

                var assignType = string.Empty;
                switch (type)
                {
                    case 0:
                        assignType = SystemData.ResetType.Hard;
                        break;
                    case 1:
                        assignType = SystemData.ResetType.Soft;
                        break;                
                }

                ResetReq newRequest = new ResetReq()
                {
                    type = assignType
                };

                Object[] request = new Object[4];
                request[0] = SystemData.MessageType.Call;
                request[1] = Guid.NewGuid().ToString();
                request[2] = SystemData.CPAction.Reset;
                request[3] = newRequest;

                string jsonRequest = JsonSerializer.Serialize(request);

                using (var client = new WebsocketClient(url))
                {
                    await client.Start();
                    await client.SendInstant(jsonRequest);
                }

                return Ok(jsonRequest);

            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }



        [HttpPost]
        [AllowAnonymous]
        [Route("RemoteStartTransaction")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> RemoteStartTransaction(RemoteStartTransactionReq model)
        {
            StringBuilder logger = new StringBuilder();

            try
            {

                var url = new Uri(CPConnectorUrl);
                var exitEvent = new ManualResetEvent(false);

                Object[] request = new Object[4];
                request[0] = SystemData.MessageType.Call;
                request[1] = Guid.NewGuid().ToString();
                request[2] = SystemData.CPAction.RemoteStartTransaction;
                request[3] = model;

                string jsonRequest = JsonSerializer.Serialize(request);

                using (var client = new WebsocketClient(url))
                {
                    await client.Start();
                    await client.SendInstant(jsonRequest);
                }

                return Ok(jsonRequest);

            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("RemoteStopTransaction")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> RemoteStopTransaction(RemoteStopTransactionRequestModel model)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var url = new Uri(CPConnectorUrl);
                var exitEvent = new ManualResetEvent(false);

                var transactionId = _cpTransactionService.GetActiveTransactionIdByConnectorId(model);
                RemoteStopTransactionReq newRequest = new RemoteStopTransactionReq()
                {
                    transactionid = transactionId,
                };

                Object[] request = new Object[4];
                request[0] = SystemData.MessageType.Call;
                request[1] = Guid.NewGuid().ToString();
                request[2] = SystemData.CPAction.RemoteStopTransaction;
                request[3] = newRequest;

                string jsonRequest = JsonSerializer.Serialize(request);

                using (var client = new WebsocketClient(url))
                {
                    await client.Start();
                    await client.SendInstant(jsonRequest);
                }

                return Ok(jsonRequest);

            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("UnlockConnector/{id}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> UnlockConnector([FromRoute] int id)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var url = new Uri(CPConnectorUrl);
                var exitEvent = new ManualResetEvent(false);

                UnlockConnectorReq reqData = new UnlockConnectorReq
                {
                    connectorId = id
                };

                Object[] request = new Object[4];
                request[0] = SystemData.MessageType.Call;
                request[1] = Guid.NewGuid().ToString();
                request[2] = SystemData.CPAction.UnlockConnector;
                request[3] = reqData;

                string jsonRequest = JsonSerializer.Serialize(request);

                using (var client = new WebsocketClient(url))
                {
                    await client.Start();
                    await client.SendInstant(jsonRequest);
                }

                return Ok(jsonRequest);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ClearCache")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> ClearCache()
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var url = new Uri(CPConnectorUrl);
                var exitEvent = new ManualResetEvent(false);

                Object[] request = new Object[4];
                request[0] = SystemData.MessageType.Call;
                request[1] = Guid.NewGuid().ToString();
                request[2] = SystemData.CPAction.ClearCache;
                request[3] = String.Empty;

                string jsonRequest = JsonSerializer.Serialize(request);

                using (var client = new WebsocketClient(url))
                {
                    await client.Start();
                    await client.SendInstant(jsonRequest);
                }

                return Ok(jsonRequest);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("MeterValues")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> MeterValues(MeterValuesRequestModel request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"MeterValues Request Timestamp {timestamp}");
                await _logger.LogInformation($"MeterValueRequest: {JsonSerializer.Serialize(request)}");
                ResponseModelBase response = await _cpTransactionService.InsertMeterValuesRecords(request);
                await _logger.LogInformation(logger.ToString());

                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }


        [HttpPost]
        [AllowAnonymous]
        [Route("TriggerMessage")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> TriggerMessage(TriggerMessageReq model)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var url = new Uri(CPConnectorUrl);
                var exitEvent = new ManualResetEvent(false);

                Object[] request = new Object[4];
                request[0] = SystemData.MessageType.Call;
                request[1] = Guid.NewGuid().ToString();
                request[2] = SystemData.CPAction.TriggerMessage;
                request[3] = model;

                string jsonRequest = JsonSerializer.Serialize(request);

                using (var client = new WebsocketClient(url))
                {
                    await client.Start();
                    await client.SendInstant(jsonRequest);
                }

                return Ok(jsonRequest);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("ChangeAvailability")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> ChangeAvailability(ChangeAvailabilityReq model)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var url = new Uri(CPConnectorUrl);
                var exitEvent = new ManualResetEvent(false);

                Object[] request = new Object[4];
                request[0] = SystemData.MessageType.Call;
                request[1] = Guid.NewGuid().ToString();
                request[2] = SystemData.CPAction.ChangeAvailability;
                request[3] = model;

                string jsonRequest = JsonSerializer.Serialize(request);

                using (var client = new WebsocketClient(url))
                {
                    await client.Start();
                    await client.SendInstant(jsonRequest);
                }

                return Ok(jsonRequest);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SendLocalList")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> SendLocalList(SendLocalListReq requestModel)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var url = new Uri(CPConnectorUrl);
                var exitEvent = new ManualResetEvent(false);

                Object[] request = new Object[4];
                request[0] = SystemData.MessageType.Call;
                request[1] = Guid.NewGuid().ToString();
                request[2] = SystemData.CPAction.SendLocalList;
                request[3] = requestModel;

                string jsonRequest = JsonSerializer.Serialize(request);

                using (var client = new WebsocketClient(url))
                {
                    await client.Start();
                    await client.SendInstant(jsonRequest);
                }

                return Ok(jsonRequest);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("GetLocalListVersion")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetLocalListVersion(GetLocalListVersionRequestModel req)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                GetLocalListVersionConf response = new GetLocalListVersionConf();
                if (req.MessageType == SystemData.MessageType.Call)
                {
                    var url = new Uri(CPConnectorUrl);
                    var exitEvent = new ManualResetEvent(false);

                    Object[] request = new Object[4];
                    request[0] = SystemData.MessageType.Call;
                    request[1] = Guid.NewGuid().ToString();
                    request[2] = SystemData.CPAction.GetLocalListVersion;

                    string jsonRequest = JsonSerializer.Serialize(request);

                    using (var client = new WebsocketClient(url))
                    {
                        await client.Start();
                        await client.SendInstant(jsonRequest);
                    }
                }

                if(req.MessageType == SystemData.MessageType.CallResult)
                {
                    response.listVersion = req.ListVersion;
                    //add to cp details

                }
                return Ok(response);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UpdateFirmware")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> UpdateFirmware(UpdateFirmwareReq model)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var url = new Uri(CPConnectorUrl);
                var exitEvent = new ManualResetEvent(false);

                Object[] request = new Object[4];
                request[0] = SystemData.MessageType.Call;
                request[1] = Guid.NewGuid().ToString();
                request[2] = SystemData.CPAction.UpdateFirmware;
                request[3] = model;

                string jsonRequest = JsonSerializer.Serialize(request);

                using (var client = new WebsocketClient(url))
                {
                    await client.Start();
                    await client.SendInstant(jsonRequest);
                }

                return Ok(jsonRequest);

            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("FirmwareStatusNotification")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> FirmwareStatusNotification(FirmwareStatusNotificationModel model)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var url = new Uri(CPConnectorUrl);
                var exitEvent = new ManualResetEvent(false); 
                ResponseModelBase response = await _cpMonitoringService.FirmwareStatusNotification(model);
                return Ok(model);

            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }
        #endregion

    }
}
