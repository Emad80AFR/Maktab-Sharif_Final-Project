﻿using System.Globalization;
using System.Security.Cryptography;
using AM._Application.Contracts.Account;
using BP._Query.Contracts.Order;
using BP._Query.Contracts.Product;
using FrameWork.Application;
using FrameWork.Application.Authentication;
using FrameWork.Application.ZarinPal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using SM._Application.Contracts.Order;
using SM._Application.Contracts.Order.DTO_s;
using WebHost.Services;

namespace WebHost.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        public Cart Cart;
        public const string CookieName = "cart-items";
        private readonly IAuthHelper _authHelper;
        private readonly ICartService _cartService;
        private readonly IProductQuery _productQuery;
        private readonly ICalculateWage _calculateWage;
        private readonly IZarinPalFactory _zarinPalFactory;
        private readonly IOrderApplication _orderApplication;
        private readonly IAccountApplication _accountApplication;
        private readonly ICartCalculatorService _cartCalculatorService;

        public CheckoutModel(ICartCalculatorService cartCalculatorService, ICartService cartService,
            IProductQuery productQuery, IOrderApplication orderApplication, IZarinPalFactory zarinPalFactory,
            IAuthHelper authHelper, ICalculateWage calculateWage, IAccountApplication accountApplication)
        {
            Cart = new Cart();
            _authHelper = authHelper;
            _cartService = cartService;
            _productQuery = productQuery;
            _calculateWage = calculateWage;
            _zarinPalFactory = zarinPalFactory;
            _orderApplication = orderApplication;
            _accountApplication = accountApplication;
            _cartCalculatorService = cartCalculatorService;
        }

        public async Task OnGet(CancellationToken cancellationToken)
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            foreach (var item in cartItems)
            {
                item.CalculateTotalItemPrice();
                var sellerInfo =await _accountApplication.GetFinancialInfo(item.SellerId, cancellationToken);
                _calculateWage.CalculateWageAsync(item,sellerInfo,cancellationToken);
            }

            Cart = await _cartCalculatorService.ComputeCart(cartItems,cancellationToken);
            _cartService.Set(Cart);
        }

        public async Task<IActionResult> OnPostPay(int paymentMethod,CancellationToken cancellationToken)
        {
            var cart = _cartService.Get();
            cart.SetPaymentMethod(paymentMethod);

            var result = await _productQuery.CheckInventoryStatus(cart.Items,cancellationToken);
            if (result.Any(x => !x.IsInStock))
                return RedirectToPage("/Cart");

            var orderId = await _orderApplication.PlaceOrder(cart, cancellationToken);
            if (paymentMethod == 1)
            {
                //var paymentResponse = _zarinPalFactory.CreatePaymentRequest(
                //    cart.PayAmount.ToString(CultureInfo.InvariantCulture), "", "",
                //    "خرید از درگاه بازارچه پلاس", orderId);

                //return Redirect(
                //    $"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentResponse.Authority}");

                var onlinePaymentResult = new PaymentResult();
                var issueTrackingNo = await _orderApplication.PaymentSucceeded(orderId, CodeGenerator.RandomRefId(), cancellationToken);
                Response.Cookies.Delete("cart-items");
                onlinePaymentResult = onlinePaymentResult.Succeeded("پرداخت با موفقیت انجام شد.", issueTrackingNo);
                return RedirectToPage("/PaymentResult", onlinePaymentResult);
            }

            Response.Cookies.Delete("cart-items");
            var paymentResult = new PaymentResult();
            return RedirectToPage("/PaymentResult",
                paymentResult.Succeeded(
                    "سفارش شما با موفقیت ثبت شد. پس از تماس کارشناسان ما و پرداخت وجه، سفارش ارسال خواهد شد.", null));
        }

        //public async Task<IActionResult> OnGetCallBack([FromQuery] string authority, [FromQuery] string status,
        //    [FromQuery] long oId,CancellationToken cancellationToken)
        //{
        //    var orderAmount =await _orderApplication.GetAmountBy(oId,cancellationToken);
        //    var verificationResponse =
        //        _zarinPalFactory.CreateVerificationRequest(authority,
        //            orderAmount.ToString(CultureInfo.InvariantCulture));

        //    var result = new PaymentResult();
        //    if (status == "OK" && verificationResponse.Status >= 100)
        //    {
        //        var issueTrackingNo = await _orderApplication.PaymentSucceeded(oId, verificationResponse.RefID,cancellationToken);
        //        Response.Cookies.Delete("cart-items");
        //        result = result.Succeeded("پرداخت با موفقیت انجام شد.", issueTrackingNo);
        //        return RedirectToPage("/PaymentResult", result);
        //    }

        //    result = result.Failed(
        //        "پرداخت با موفقیت انجام نشد. درصورت کسر وجه از حساب، مبلغ تا 24 ساعت دیگر به حساب شما بازگردانده خواهد شد.");
        //    return RedirectToPage("/PaymentResult", result);
        //}
    }
}