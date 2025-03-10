using Microsoft.AspNetCore.Mvc;

using XYZ.Logic.Common.Interfaces;
using XYZ.Web.Common.Attributes;
using XYZ.Web.Common.Base;
using XYZ.Web.Features.Billing.Mappers;
using XYZ.Web.Features.Billing.WebRequests;

namespace XYZ.Web.Features.Billing.Controllers
{
    /// <summary>
    /// Billing and orders related operations.
    /// </summary>
    [ApiController]
    [Route("api/billing")]
    public class BillingController : ApiControllerBase
    {
        /// <summary>
        /// Main entry point for billing logic.
        /// </summary>
        private readonly IBillingLogic _billingLogic;

        /// <summary>
        /// Main billing constructor.
        /// </summary>
        /// <param name="billingLogic">Main billing logic.</param>
        public BillingController(IBillingLogic billingLogic)
        {
            _billingLogic = billingLogic;
        }

        /// <summary>
        /// Processes billing request to create order for user.
        /// </summary>
        /// <param name="request">Web request with billing info.</param>
        /// <returns>Receipt if ok, errors if not ok.</returns>
        [HttpPost("process")]
        [ValidateModel]
        public async Task<IActionResult> ProcessOrder([FromBody] BillingOrderProcessingRequest request)
        {
            var result = await _billingLogic.ProcessOrderAsync(request.ToOrderInfo());
            if (result.OrderValidationResult.ValidationErrors.Any())
                return BadRequest(result);

            if (!string.IsNullOrEmpty(result?.ProblemDetails?.Details) || !string.IsNullOrEmpty(result?.ProblemDetails?.Title))
                return StatusCode(StatusCodes.Status502BadGateway, result);

            return Ok(result);
        }

        /// <summary>
        /// Gets order information associated with user.
        /// </summary>
        /// <param name="request">Order information web request.</param>
        /// <returns>Order information if ok, not found if not ok.</returns>
        [HttpGet("get-order-info")]
        [ValidateModel]
        public async Task<IActionResult> GetOrderInfo([FromQuery] GetOrderInfoRequest request)
        {
            var order = await _billingLogic.GetOrderInfoAsync(request.UserId, request.OrderNumber);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        /// <summary>
        /// Gets receipt information associated with order.
        /// </summary>
        /// <param name="request">Receipt info associated with order.</param>
        /// <returns>Receipt information if ok, not found if not ok.</returns>
        [HttpGet("get-receipt-info")]
        [ValidateModel]
        public async Task<IActionResult> GetReceiptInfo([FromQuery] GetReceiptInfoRequest request)
        {
            var receipt = await _billingLogic.GetReceiptAsync(request.ReceiptId);
            if (receipt == null)
                return NotFound();

            return Ok(receipt);
        }
    }
}
