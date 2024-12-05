using Google.Cloud.Dialogflow.V2;
using Google.Type;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.RegularExpressions;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.Hubs;

namespace WebQuanLyNhaKhoa.Controllers.ApiController
{
    [Route("api/chatbot")]
    public class ChatbotController : Controller
    {
        private const string ProjectId = "chatbotnhakhoa-tron";
        private readonly ApplicationDbContext _context;

        public ChatbotController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("chat")]
        public IActionResult Chat()
        {
            return View();
        }


        [HttpPost("webhook")]
        public IActionResult Webhook([FromBody] UserMessage userMessage)
        {
            if (userMessage == null || string.IsNullOrEmpty(userMessage.Message))
            {
                return BadRequest("Yêu cầu không hợp lệ!");
            }

            

            string credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "Credentials", "chatbotnhakhoa-tron-f83d37937249.json");
            if (!System.IO.File.Exists(credentialPath))
            {
                return BadRequest($"Không tìm thấy tệp credentials tại: {credentialPath}");
            }

            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
            Console.WriteLine("GOOGLE_APPLICATION_CREDENTIALS: " + System.Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"));

            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

            // Lấy IntentName từ Dialogflow
            string intentName = null;
            string dialogflowResponse = GetDialogflowResponse(userMessage.Message, out intentName);

            // Nếu không có phản hồi từ Dialogflow, lưu câu hỏi vào cơ sở dữ liệu
            //if (string.IsNullOrEmpty(dialogflowResponse) || !dialogflowResponse.Equals(userMessage.Message, StringComparison.OrdinalIgnoreCase))
            if (dialogflowResponse == "") 
            {
                var unansweredQuestion = new UnansweredQuestion
                {
                    Question = userMessage.Message,
                    AskedOn = System.DateTime.Now,
                    IsAnswered = false, // Câu hỏi chưa được trả lời
                    //AnsweredByName = null // Không có nhân viên trả lời
                };
                _context.UnansweredQuestions.Add(unansweredQuestion);
                _context.SaveChanges();
                dialogflowResponse = "Câu hỏi của bạn sẽ được nhân viên trả lời sau ít phút.";
                userMessage.QuestionId = unansweredQuestion.Id.ToString(); 
                // Lưu ConnectionId vào từ điển
                ChatHub.AddUserConnection(userMessage.QuestionId, userMessage.ConnectionId);
            }
            string dialogflowResponse2 = null;
            if (intentName == "ibacsi")
            {
                dialogflowResponse = GetDoctorsList(); // Lấy danh sách bác sĩ từ cơ sở dữ liệu
                dialogflowResponse2 = GetDialogflowResponse(userMessage.Message, out intentName);
            }
            if (intentName == "idichvu")
            {
                dialogflowResponse = GetDichVuList(); // Lấy danh sách bác sĩ từ cơ sở dữ liệu

                dialogflowResponse2 = GetDialogflowResponse(userMessage.Message, out intentName);
            }
            if (intentName == "ilienhe")
            {
                dialogflowResponse = GetDialogflowResponse(userMessage.Message, out intentName);
                dialogflowResponse2 = "<a href='https://localhost:7101/Contact/Index'>Bạn có thể xem Thông tin liên hê tại đây</a>";
            }
            if (intentName == "ihuylichsdt")
            {
                if (string.IsNullOrEmpty(userMessage.Message) || !Regex.IsMatch(userMessage.Message, @"^(0[3|5|7|8|9])[0-9]{8}$"))
                {
                    return Ok(new
                    {
                        fulfillmentText = $"Số điện thoại {userMessage.Message} bạn nhập không hợp lệ. Vui lòng nhập lại số điện thoại."
                    });
                }
            }
            return Ok(new { fulfillmentText = dialogflowResponse, fulfillmentText2 = dialogflowResponse2, connectionId = userMessage.ConnectionId, questionId = userMessage.QuestionId });
        }
        private string GetDoctorsList()
        {
            try
            {
                // Truy vấn bảng Doctors để lấy danh sách bác sĩ
                var doctors = _context.NhanViens
                    .Select(d => $"{d.Ten} có {d.KinhNghiem} kinh nghiệm {d.DichVu.TenDichVu}.")
                    .ToList();

                if (!doctors.Any())
                {
                    return "Hiện tại không có bác sĩ nào trong danh sách.";
                }
                var randomDoctors = doctors.OrderBy(d => Guid.NewGuid()).Take(3).ToList();
                // Kết hợp danh sách bác sĩ thành chuỗi phản hồi
                return "Danh sách bác sĩ:\n" + string.Join("\n", randomDoctors);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi truy vấn danh sách bác sĩ: {ex.Message}");
                return "Đã xảy ra lỗi khi lấy danh sách bác sĩ.";
            }
        }
        private string GetDichVuList()
        {
            try
            {
                // Truy vấn bảng Doctors để lấy danh sách bác sĩ
                var dichvus = _context.DichVus
                    .Select(d => $"{d.TenDichVu} - {d.DonGia.ToString("N0")} VNĐ.")
                    .ToList();

                if (!dichvus.Any())
                {
                    return "Hiện tại không có bác sĩ nào trong danh sách.";
                }
                var randomDoctors = dichvus.OrderBy(d => Guid.NewGuid()).Take(4).ToList();
                // Kết hợp danh sách bác sĩ thành chuỗi phản hồi
                return "Danh sách dịch vụ:\n" + string.Join("\n", randomDoctors);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi truy vấn danh sách bác sĩ: {ex.Message}");
                return "Đã xảy ra lỗi khi lấy danh sách bác sĩ.";
            }
        }
        private string GetDialogflowResponse(string query, out string intentName)
        {
            intentName = null; // Biến trả về tên Intent

            try
            {
                var sessionClient = SessionsClient.Create();
                var session = new SessionName(ProjectId, Guid.NewGuid().ToString());
                var queryInput = new QueryInput
                {
                    Text = new TextInput { Text = query, LanguageCode = "vi" }
                };

                var response = sessionClient.DetectIntent(session, queryInput);
                if (response == null || response.QueryResult == null)
                {
                    Console.WriteLine("Response or QueryResult is null");
                    return null;
                }

                // Lấy IntentName từ kết quả của Dialogflow
                intentName = response.QueryResult.Intent?.DisplayName;

                // Log thông tin Intent và FulfillmentText
                Console.WriteLine($"Intent được nhận diện: {intentName}");
                Console.WriteLine($"Fulfillment Text: {response.QueryResult.FulfillmentText}");

                return response.QueryResult.FulfillmentText; // Trả về phản hồi của Dialogflow
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return null;
            }
        }


    }

    public class UserMessage
    {
        public string Message { get; set; }
        public string ConnectionId { get; set; }
        public string QuestionId { get; set; }
    }
}
