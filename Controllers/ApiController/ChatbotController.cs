using Google.Cloud.Dialogflow.V2;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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

            string credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "Credentials", "google-credentials.json");
            if (!System.IO.File.Exists(credentialPath))
            {
                return BadRequest($"Không tìm thấy tệp credentials tại: {credentialPath}");
            }

            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

            string dialogflowResponse = GetDialogflowResponse(userMessage.Message);

            // Nếu không có phản hồi từ Dialogflow, lưu câu hỏi vào cơ sở dữ liệu
            //if (string.IsNullOrEmpty(dialogflowResponse) || !dialogflowResponse.Equals(userMessage.Message, StringComparison.OrdinalIgnoreCase))
            if(dialogflowResponse == "") 
            {
                var unansweredQuestion = new UnansweredQuestion
                {
                    Question = userMessage.Message,
                    AskedOn = DateTime.Now,
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

            return Ok(new { fulfillmentText = dialogflowResponse, connectionId = userMessage.ConnectionId, questionId = userMessage.QuestionId });
        }

        private string GetDialogflowResponse(string query)
        {
            try
            {
                var sessionClient = SessionsClient.Create();
                var session = new SessionName(ProjectId, Guid.NewGuid().ToString());
                var queryInput = new QueryInput
                {
                    Text = new TextInput { Text = query, LanguageCode = "vi" }
                };

                var response = sessionClient.DetectIntent(session, queryInput);
                return response.QueryResult.FulfillmentText;
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
