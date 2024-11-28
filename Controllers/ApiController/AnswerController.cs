using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.Hubs;

namespace WebQuanLyNhaKhoa.Controllers.ApiConrtroller
{
    [Route("api/answer")]
    public class AnswerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AnswerController(ApplicationDbContext context, IHubContext<ChatHub> chatHubContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _chatHubContext = chatHubContext;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        // Lấy tất cả các câu hỏi chưa trả lời
        [HttpGet("unanswered")]
        public IActionResult GetUnansweredQuestions()
        {
            var unansweredQuestions = _context.UnansweredQuestions
                                               .Where(q => !q.IsAnswered)
                                               .ToList();
            return Ok(unansweredQuestions);
        }
        [HttpGet("post")] 
        public IActionResult AnswerPost(int questionId) 
        { 
            ViewBag.QuestionId = questionId; 
            return View(); 
        }
        // Nhân viên trả lời câu hỏi
        [HttpPost("answer")]
        public async Task<IActionResult> AnswerQuestionAsync([FromBody] UnansweredQuestionDTO answerRequest)
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value; 
            if (employeeId == null) 
            { 
                return Unauthorized("Không thể xác thực nhân viên!"); 
            }
            var question = _context.UnansweredQuestions.FirstOrDefault(q => q.Id == answerRequest.questionId);

            if (question == null)
            {
                return Json(new { message = "Câu hỏi không tồn tại!" });
            }

            if (question.IsAnswered)
            {
                return Json(new { message = "Câu hỏi đã được trả lời!" });
            }

            var employee = _context.NhanViens.FirstOrDefault(e => e.MaNv.ToString() == employeeId);

            if (employee == null)
            {
                return Json(new { message = "Nhân viên không tồn tại!" });
            }

            question.IsAnswered = true;
            question.MaNv = employee.MaNv;
            question.Answer = answerRequest.Answer;
            _context.SaveChanges();

            //// Gửi thông báo real-time tới người dùng cụ thể qua SignalR
            //var connectionId = _httpContextAccessor.HttpContext.Connection.Id; 
            //await _chatHubContext.Clients.Group(connectionId).SendAsync("ReceiveMessage", employee.Ten, answerRequest.Answer);

            //await _chatHubContext.Clients.All.SendAsync("ReceiveMessage", employee.Ten, answerRequest.Answer);

            // Lấy ConnectionId của người dùng từ ChatHub
            var connectionId = ChatHub.GetConnectionId(answerRequest.questionId.ToString());
            if (string.IsNullOrEmpty(connectionId))
            {
                return Json(new { message = "Không tìm thấy ConnectionId của người dùng!" });
            }

            // Gửi thông báo real-time tới người dùng cụ thể qua SignalR
           
            await _chatHubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", employee.Ten, answerRequest.Answer);
            return Json(new { message = "Câu hỏi đã được trả lời thành công!" });
        }

        public class UnansweredQuestionDTO
        {
            public int questionId { get; set; }
            public string Question { get; set; }
            
            public string? Answer { get; set; }

            public DateTime AskedOn { get; set; }

            public bool IsAnswered { get; set; } 

            public int? MaNv { get; set; }

            public string? ConnectionId { get; set; }
            
        }

    }
}
