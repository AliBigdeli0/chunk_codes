using System;
using System.Net;
using System.Net.Sockets;

namespace CSPortListen
{
    /// <summary>
    /// listen and send
    /// </summary>
    public partial class TcpListenSlim
    {
        private Socket _client_socket;
        public TcpListenSlim()
        {

        }

        public void ListenStop()
        {
            _client_socket?.Close();
            _client_socket = null;
        }

        public void ListenStart(IPEndPoint client)
        {
            if (_client_socket == null)
                _client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _client_socket.Bind(client);
            _client_socket.Listen(10);
            var state = new ListenState();
            try
            {
                state.ClientSocket = _client_socket.Accept();
                MainForm.SendMessage($">>>>client connected : {(state.ClientSocket.RemoteEndPoint as IPEndPoint).Address}");
                state.UserSocket = _client_socket.Accept();
                MainForm.SendMessage($">>>>user connected : {(state.UserSocket.RemoteEndPoint as IPEndPoint).Address}");

                state.ClientSocket.BeginReceive(state.ClientBuffer, 0, ListenState.Lenght, SocketFlags.None, ClientSocketReceive, state);
                state.UserSocket.BeginReceive(state.UserBuffer, 0, ListenState.Lenght, SocketFlags.None, UserSocketReceive, state);
            }
            catch (Exception ex)
            {
                MainForm.SendMessage($"error in listen start : {ex.Message}");
                state.ClientSocket?.Close();
                state.UserSocket?.Close();
            }
        }

        #region User
        private static void UserSocketSend(IAsyncResult result)
        {
            ListenState state = null;
            try
            {
                state = (ListenState)result.AsyncState;
                state.UserSocket.EndSend(result);
            }
            catch (Exception ex)
            {
                MainForm.SendMessage($"##error in ClientSocketSend: {ex.Message}");
                state.ClientSocket.Close();
                state.UserSocket.Close();
            }
        }
        private static void UserSocketReceive(IAsyncResult result)
        {
            ListenState state = null;
            try
            {
                state = (ListenState)result.AsyncState;
                var br = state.UserSocket.EndReceive(result);
                if (br > 0)
                {
                    try
                    {
                        state.ClientSocket.BeginSend(state.UserBuffer, 0, br, SocketFlags.None, ClientSocketSend, state);
                    }
                    catch (Exception ex)
                    {
                        MainForm.SendMessage($"##error in ClientSocketReceive.Send: {ex.Message}");
                    }
                    state.UserSocket.BeginReceive(state.UserBuffer, 0, ListenState.Lenght, SocketFlags.None, UserSocketReceive, state);
                }
            }
            catch (Exception ex)
            {
                MainForm.SendMessage($"##error in UserSocketReceive: {ex.Message}");
                state.ClientSocket?.Close();
                state.UserSocket?.Close();
            }
        }
        #endregion

        #region Client
        private static void ClientSocketSend(IAsyncResult result)
        {
            ListenState state = null;
            try
            {
                state = (ListenState)result.AsyncState;
                state.ClientSocket.EndSend(result);
            }
            catch (Exception ex)
            {
                MainForm.SendMessage($"##error in ClientSocketSend: {ex.Message}");
                state.ClientSocket.Close();
                state.UserSocket.Close();
            }
        }

        private static void ClientSocketReceive(IAsyncResult result)
        {
            ListenState state = null;
            try
            {
                state = (ListenState)result.AsyncState;
                var br = state.ClientSocket.EndReceive(result);
                if (br > 0)
                {
                    try
                    {
                        state.UserSocket.BeginSend(state.ClientBuffer, 0, br, SocketFlags.None, UserSocketSend, state);
                    }
                    catch (Exception ex)
                    {
                        MainForm.SendMessage($"##error in ClientSocketReceive.Send: {ex.Message}");
                    }
                    state.ClientSocket.BeginReceive(state.ClientBuffer, 0, ListenState.Lenght, SocketFlags.None, ClientSocketReceive, state);
                }
            }
            catch (Exception ex)
            {
                MainForm.SendMessage($"##error in ClientSocketReceive: {ex.Message}");
                state?.ClientSocket.Close();
                state?.UserSocket.Close();
            }
        }
        #endregion
    }
}
