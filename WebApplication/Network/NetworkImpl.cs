using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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
                client = new TcpClient();
                string ip = "3.14.79.103";
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 6969);
                client.Connect(ipEndPoint);
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
                String[] split = response.Split("json\":\"");
                String test = split[1].Replace("\\", "");
                Char[] chars = test.ToCharArray();
                chars[chars.Length - 2] = ' ';
                chars[chars.Length - 1] = ' ';
                test = new string(chars);
                String test2 = test.Replace(" ", "");
                ApiCurrentDataPackage apiCurrentDataPackage =
                    JsonSerializer.Deserialize<ApiCurrentDataPackage>(test2);
                double CO2 = 0;
                double Temperature = 0;
                double Humidity = 0;
                int count = 0;
                for (int i = 0; i < apiCurrentDataPackage.data.Count; i++)
                {
                    if (apiCurrentDataPackage.data[i].type == DataType.CO2)
                    {
                        CO2 += apiCurrentDataPackage.data[i].data;
                        count++;
                    }
                    else if (apiCurrentDataPackage.data[i].type == DataType.TEMPERATURE)
                    {
                        Temperature += apiCurrentDataPackage.data[i].data;
                       
                    }
                    else if (apiCurrentDataPackage.data[i].type == DataType.HUMIDITY)
                    {
                        Humidity += apiCurrentDataPackage.data[i].data;
                    }
                }
                Console.WriteLine("Count: "+ count + " CO2: " + CO2 + " Temperature: "+ Temperature + " Humidity: "+ Humidity);
                CO2 = CO2 / count;
                Temperature = Temperature / count;
                Humidity = Humidity / count;
                DataContainer CO2Container = new DataContainer(CO2, DataType.CO2);
                DataContainer temperatureContainer = new DataContainer(Temperature, DataType.TEMPERATURE);
                DataContainer HumidityContainer = new DataContainer(Humidity, DataType.HUMIDITY);
                List<DataContainer> dataContainers = new List<DataContainer>();
                dataContainers.Add(CO2Container);
                dataContainers.Add(temperatureContainer);
                dataContainers.Add(HumidityContainer);
                ApiCurrentDataPackage apiCurrent = new ApiCurrentDataPackage(dataContainers,apiCurrentDataPackage.lastDataPoint);
                Console.WriteLine(apiCurrent.data.ToString());
                String current = JsonSerializer.Serialize(apiCurrent);
                Message message = new Message();
                message.command = "SUCCES";
                message.json = current;
                Console.WriteLine(message.json);
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
