using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DotvSocketServer.domain;
using DotvSocketServer.handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DotvSocketServer
{
    public class Startup
    {

        private List<MessageHandler> handerlers;
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void initHandlers()
        {
            handerlers = new List<MessageHandler>();
            handerlers.Add(new AuthHandler());
            handerlers.Add(new ChatHandler());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            initHandlers();
            
            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/chat")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await ChatSocketHandler(context, webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });
        }
        
        public async Task ChatSocketHandler(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                string json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Message message = JsonSerializer.Deserialize<Message>(json);
                await ProcessMessage(message, json, webSocket);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private async Task SendMessageToSocket(String data, WebSocket webSocket)
        {
            var encoded = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public MessageHandler GetMessageHandler(int msgType)
        {
            foreach(MessageHandler handler in handerlers)
            {
                if (handler.HasMessageType(msgType))
                {
                    return handler;
                }
            }
            return null;
        }
        
        private async Task ProcessMessage(Message message, String json, WebSocket webSocket)
        {
            MessageHandler handler = GetMessageHandler(message.MessageType);
            if (handler is null)
                return;
            
            MessageResponse response = handler.processMessage(message, json);
            
            if (response.IsGlobal) {
                //TODO: send to all rooms
            }else if (response.SendToRoom) {
                //TODO: send to room specified by the id from the message
            }else if (response.SendToUser) {
                await SendMessageToSocket(JsonSerializer.Serialize(response), webSocket);
            }
            
        }

        private async Task<AuthResponse> ProcessAuthRequest(AuthRequest authRequest)
        {
            AuthResponse response = new AuthResponse();
            response.Valid = true;
            //TODO: actually make sure the user is valid by checking the token
            return response;
        }
        
    }
}