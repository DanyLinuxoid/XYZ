using Microsoft.AspNetCore.Mvc;

using XYZ.Logic.Common.Interfaces;
using XYZ.Web.Common.Attributes;
using XYZ.Web.Common.Base;
using XYZ.Web.Features.Billing.Mappers;
using XYZ.Web.Features.Billing.WebRequests;

namespace XYZ.Web.Features.Billing.Controllers
{
    [ApiController]
    [Route("api/billing")]
    public class BillingController : ApiControllerBase
    {
        private readonly IBillingLogic _billingService;

        public BillingController(IBillingLogic billingService)
        {
            _billingService = billingService;
        }

        [HttpPost("process")]
        [ValidateModel]
        public async Task<IActionResult> ProcessOrder([FromBody] BillingOrderProcessingRequest request)
        {
            var result = await _billingService.ProcessOrderAsync(request.ToOrderInfo());
            if (result.OrderValidationResult.ValidationErrors.Any())
                return BadRequest(result);

            if (!string.IsNullOrEmpty(result?.ProblemDetails?.Details) || !string.IsNullOrEmpty(result?.ProblemDetails?.Title))
                return StatusCode(StatusCodes.Status502BadGateway, result);

            return Ok(result);
        }

        [HttpGet("get-order-info")]
        [ValidateModel]
        public async Task<IActionResult> GetOrderInfo([FromQuery] GetOrderInfoRequest request)
        {
            var order = await _billingService.GetOrderInfoAsync(request.UserId, request.OrderNumber);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("get-receipt-info")]
        [ValidateModel]
        public async Task<IActionResult> GetReceiptInfo([FromQuery] GetReceiptInfoRequest request)
        {
            var receipt = await _billingService.GetReceiptAsync(request.ReceiptId);
            if (receipt == null)
                return NotFound();

            return Ok(receipt);
        }
    }
}
