using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSPortForward
{
    public partial class TcpForwardSlim
    {
        private Socket _local_socket = null;
        private Socket _remote_socket = null;
        private IPEndPoint _local = null;
        private IPEndPoint _remote = null;

        public void Start(IPEndPoint local, IPEndPoint remote)
        {
            try
            {
                _local = local;
                _remote = remote;

                var local_state = StartLocal(local, remote);
                var remote_state = StartRemote(local_state.LocalSocket, local, remote);
                local_state.RemoteSocket = remote_state.RemoteSocket;
                StartLocalRecive(local_state);
                StartRemoteRecive(remote_state);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"##local Start: {ex.Message}");
            }
        }

        #region Remote

        private void StartRemoteRecive(ForwardState state)
        {
            _remote_socket.BeginReceive(state.RemoteBuffer, 0, state.RemoteBuffer.Length, 0, RemoteDataRecive, state);
        }

        ForwardState StartRemote(Socket local_socket, IPEndPoint local, IPEndPoint remote)
        {
            if (_remote_socket != null)
            {
                _remote_socket.Close();
                _remote_socket = null;
            }

            if (_remote_socket == null)
                _remote_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Thread.Sleep(100);
            _remote_socket.Connect(remote);
            Debug.WriteLine(">>>>forward remote connected");

            var state = new ForwardState()
            {
                LocalEndPoint = local,
                RemoteEndPoint = remote,
                LocalSocket = local_socket,
                RemoteSocket = _remote_socket
            };
            return state;
        }

        private static void RemoteDataSend(IAsyncResult result)
        {
            var state = (ForwardState)result.AsyncState;
            try
            {
                var br = state.RemoteSocket.EndSend(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"####remote sender error : {ex.Message}");
            }
        }

        private static void RemoteDataRecive(IAsyncResult result)
        {
            var state = (ForwardState)result.AsyncState;
            try
            {
                var br = state.RemoteSocket.EndReceive(result);
                if (br > 0)
                {
                    try
                    {
                        Thread.Sleep(1);
                        Debug.Write($">>>>{state.RemoteEndPoint.Port} => {state.LocalEndPoint.Port}");
                        if (state.LocalSocket != null)
                            state.LocalSocket.BeginSend(state.RemoteBuffer, 0, br, SocketFlags.None, LocalDataSend, state);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"####local send error : {ex.Message}");
                    }
                    state.RemoteBuffer = new byte[state.RemoteBuffer.Length];
                    state.RemoteSocket.BeginReceive(state.RemoteBuffer, 0, state.RemoteBuffer.Length, 0, RemoteDataRecive, state);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"####remote recieve error : {ex.Message}");
            }
        }
        #endregion

        #region Local
        private void StartLocalRecive(ForwardState state)
        {
            _local_socket.BeginReceive(state.LocalBuffer, 0, state.LocalBuffer.Length, 0, LocalDataRecive, state);
        }

        ForwardState StartLocal(IPEndPoint local, IPEndPoint remote)
        {
            if (_local_socket != null)
            {
                _local_socket.Close();
                _local_socket = null;
            }

            if (_local_socket == null)
                _local_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Thread.Sleep(100);
            _local_socket.Connect(local);
            Debug.WriteLine(">>>>start local connected");

            var state = new ForwardState()
            {
                LocalEndPoint = local,
                RemoteEndPoint = remote,
                LocalSocket = _local_socket,
                RemoteSocket = _remote_socket
            };
            return state;
        }

        private static void LocalDataSend(IAsyncResult result)
        {
            var state = (ForwardState)result.AsyncState;
            try
            {
                state.LocalSocket.EndSend(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"####remote sender error : {ex.Message}");
            }
        }

        private static void LocalDataRecive(IAsyncResult result)
        {
            ForwardState state = null;
            try
            {
                state = (ForwardState)result.AsyncState;
                var br = state.LocalSocket.EndReceive(result);
                if (br > 0)
                {
                    try
                    {
                        Thread.Sleep(1);
                        if (state.RemoteSocket != null)
                            state.RemoteSocket.BeginSend(state.LocalBuffer, 0, br, SocketFlags.None, RemoteDataSend, state);
                        Debug.Write($">>>>{state.LocalEndPoint.Port} => {state.RemoteEndPoint.Port}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"####remote send error : {ex.Message}");

                    }
                    state.LocalBuffer = new byte[state.LocalBuffer.Length];
                    state.LocalSocket.BeginReceive(state.LocalBuffer, 0, state.LocalBuffer.Length, 0, LocalDataRecive, state);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"####local recive error : {ex.Message}");

            }
        }
        #endregion
    }
}