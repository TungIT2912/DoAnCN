using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.ServicesPay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExcute(IQueryCollection collections);
    }
}
