//using Microsoft.AspNetCore.SignalR;
//using System.Threading.Tasks;

//namespace WebQuanLyNhaKhoa.Hubs
//{
//    public class ChatHub : Hub
//    {
//        public async Task SendMessage(string user, string message)
//        {
//            await Clients.All.SendAsync("ReceiveMessage", user, message);
//        }
//    }
//}

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace WebQuanLyNhaKhoa.Hubs;
public class ChatHub : Hub
{
    private static Dictionary<string, string> userConnections = new Dictionary<string, string>();

    public override Task OnConnectedAsync()
    {
        string connectionId = Context.ConnectionId;
        // Bạn có thể lưu trữ thêm thông tin người dùng nếu cần
        userConnections[connectionId] = Context.User?.Identity?.Name ?? "Anonymous";
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        string connectionId = Context.ConnectionId;
        userConnections.Remove(connectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public static string GetConnectionIdByUser(string userName)
    {
        return userConnections.FirstOrDefault(x => x.Value == userName).Key;
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.Caller.SendAsync("ReceiveMessage", user, message);
    }

    public static void AddUserConnection(string questionId, string connectionId) 
    { 
        if (!userConnections.ContainsKey(questionId)) 
        { 
            userConnections.Add(questionId, connectionId); 
        } 
    }
    public static string GetConnectionId(string questionId)
    {
        return userConnections.ContainsKey(questionId) ? userConnections[questionId] : null;
    }
}

