﻿using System;
using System.Net.Sockets;
    using System.Text;
    using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
    using WebApplication.Data;
using Enum = WebApplication.Data.Enum;

namespace WebApplication.Network
    {
        public class NetworkImpl
        {
            private NetworkStream stream;
            private TcpClient client;
            public NetworkImpl()
            {
                client = new TcpClient("127.0.0.1", 6969);
                stream = client.GetStream();
            }

            public async Task<Message> GetCurrentData(int userID, int greenhouseID)
            {
                string whatever = userID + ":" + greenhouseID;
                //string jsonString = JsonSerializer.Serialize(whatever);
                Message message = new Message(Enum.GETCURRENTDATA, whatever);
                string jsonStringFinal = JsonSerializer.Serialize(message);
                byte[] bytes = Encoding.ASCII.GetBytes(jsonStringFinal);
                stream.Write(bytes, 0, bytes.Length);

                byte[] bytesResponse = new byte[1024 * 1024];
                stream.Read(bytesResponse, 0, bytesResponse.Length);
                string answer = Encoding.ASCII.GetString(bytesResponse);
                Message deserialize = JsonSerializer.Deserialize<Message>(answer);
                
                return deserialize;

            }
        
        }
    }
