using Microsoft.AspNetCore.SignalR;

namespace SignalRAngularTest.Hubs
{

    public static class UserList
    {
        public static HashSet<string> ConnectedClients = new HashSet<string>();
    }

    public class ChatHub : Hub
    {
        public Task SendMessageToAll(string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", message);
        }

        public Task SendMessageToUser(string connectionId, string message)
        {
            return Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }


        //Overrideing OnConnected to store Connceted Users
        public override Task OnConnectedAsync()
        {
            UserList.ConnectedClients.Add(Context.ConnectionId);
            Clients.All.SendAsync("UsersConnected", UserList.ConnectedClients.ToList());
            return base.OnConnectedAsync();
        }



        //Overrideing OnDisconnected to remove from Connceted Users store
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            UserList.ConnectedClients.Remove(Context.ConnectionId);
            Clients.All.SendAsync("UsersConnected", UserList.ConnectedClients.ToList());
            return base.OnDisconnectedAsync(exception);
        }

    }
}
