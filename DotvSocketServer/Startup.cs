using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DotvSocketServer.domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DotvSocketServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
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

            //app.UseRouting();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            //});
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
                //get the message Type
                //based on the message type do different things
               
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
        
        private async Task ProcessMessage(Message message, String json, WebSocket webSocket)
        {
            switch (message.MessageType)
            {
                case 0: //auth message
                    var authRequest = JsonSerializer.Deserialize<AuthRequest>(json);
                    var authResponse = await ProcessAuthRequest(authRequest);
                    //TODO: if this user is valid, add them to valid users
                    //if (authResponse.Valid)
                    //{
                    //    addToValidUsers();
                    //}
                    await SendMessageToSocket(JsonSerializer.Serialize(authResponse), webSocket);
                    break;
                
                case 1: //chat message
                    break;
                    
                case 2: //heartbeat 
                    break;
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