using System;
using System.Collections.Generic;
using System.Net.Sockets;
    using System.Text;
    using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Hosting;
    using WebApplication.Data;
using Enum = WebApplication.Data.Messages;

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
                //Message message = new Message(Enum.GETCURRENTDATA, whatever);
                string jsonStringFinal = JsonSerializer.Serialize(new Message
                {
                    command = "GETCURRENTDATA",
                    json = whatever
                });
                byte[] bytes = Encoding.ASCII.GetBytes(jsonStringFinal);
                stream.Write(bytes, 0, bytes.Length);

                byte[] bytesResponse = new byte[1024 * 1024];
                
                int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

                string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
                Message message2 = JsonSerializer.Deserialize<Message>(response);
                Console.WriteLine(message2.command);
                return message2;

            }

            public async Task<Message> getGreenhouses(int userID)
            {
                string id = userID + "";

                string jsonString = JsonSerializer.Serialize(new Message
                {
                    command = "GETGREENHOUSES",
                    json = id
                });
                byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                stream.Write(bytes,0,bytes.Length);
                
                byte[] bytesResponse = new byte[1024 * 1024];
                
                int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

                string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
                Message message = JsonSerializer.Deserialize<Message>(response);
                return message;
            }

            public async Task<Message> getUser(String username, String password)
            {
                string account = username + ":" + password;

                string jsonString = JsonSerializer.Serialize(new Message
                {
                    command = "GETUSER",
                    json = account
                });
                byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                stream.Write(bytes,0,bytes.Length);
                
                byte[] bytesResponse = new byte[1024 * 1024];
                
                int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

                string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
                Message message = JsonSerializer.Deserialize<Message>(response);
                return message;
            }

            public async Task<Message> getGreenhouseByID(int userId, int greenHouseID)
            {
                string ids = userId + ":" + greenHouseID;
                string jsonString = JsonSerializer.Serialize(new Message
                {
                    command = "GETGREENHOUSEBYID",
                    json = ids
                });
                byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                stream.Write(bytes,0,bytes.Length);
                
                byte[] bytesResponse = new byte[1024 * 1024];
                
                int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

                string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
                Message message = JsonSerializer.Deserialize<Message>(response);
                return message;
            }

            public async Task<Message> getAverageData(int userId, int greenHouseID)
            {
                string ids = userId + ":" + greenHouseID;
                string jsonString = JsonSerializer.Serialize(new Message
                {
                    command = "GETAVERAGEDATA",
                    json = ids
                });
                byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                stream.Write(bytes,0,bytes.Length);
                
                byte[] bytesResponse = new byte[1024 * 1024];
                
                int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

                string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
                Message message = JsonSerializer.Deserialize<Message>(response);
                return message;
            }

            public async Task waterNow(int userId, int greenHouseID)
            {
                string ids = userId + ":" + greenHouseID;
                string jsonString = JsonSerializer.Serialize(new Message
                {
                    command = "WATERNOW",
                    json = ids
                });
                byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                stream.Write(bytes,0,bytes.Length);
            }

            public async Task openWindow(int userId, int greenHouseID)
            {
                string ids = userId + ":" + greenHouseID;
                string jsonString = JsonSerializer.Serialize(new Message
                {
                    command = "OPENWINDOW",
                    json = ids
                });
                byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                stream.Write(bytes,0,bytes.Length);
            }

            public async Task<Message> addGreenHouse(Greenhouse greenhouse)
            {
                string serializedString = JsonSerializer.Serialize(greenhouse);
                string jsonString = JsonSerializer.Serialize(new Message
                {
                    command = "ADDGREENHOUSE",
                    json = serializedString
                });
                byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                stream.Write(bytes,0,bytes.Length);
                     
                byte[] bytesResponse = new byte[1024 * 1024];
                
                int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

                string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
                Message message = JsonSerializer.Deserialize<Message>(response);
                return message;
            }

            public async Task<Message> addPlant(Plant plant)
            {
                string serializedString = JsonSerializer.Serialize(plant);
                string jsonString = JsonSerializer.Serialize(new Message
                {
                    command = "ADDPLANT",
                    json = serializedString
                });
                byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                stream.Write(bytes,0,bytes.Length);
                     
                byte[] bytesResponse = new byte[1024 * 1024];
                
                int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

                string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
                Message message = JsonSerializer.Deserialize<Message>(response);
                return message;
            }

            public async Task<Message> addUser(User user)
            {
                string serializedString = JsonSerializer.Serialize(user);
                string jsonString = JsonSerializer.Serialize(new Message
                {
                    command = "ADDUSER",
                    json = serializedString
                });
                byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                stream.Write(bytes,0,bytes.Length);
                     
                byte[] bytesResponse = new byte[1024 * 1024];
                
                int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

                string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
                Message message = JsonSerializer.Deserialize<Message>(response);
                return message;
            }
        
        }
    }
