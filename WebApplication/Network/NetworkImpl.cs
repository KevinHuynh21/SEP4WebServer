using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Sockets;
    using System.Text;
    using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols;
using WebApplication.Data;
using Enum = WebApplication.Data.Messages;

namespace WebApplication.Network
    {
        public class NetworkImpl
        {
            SqlCommand command;
            SqlDataReader dataReader;
            public NetworkImpl()
            {
                GetCurrentData(1, 1);
            }

            
            public string GetCurrentData(int userID, int greenhouseID)
            {
                string connectionString;
                SqlConnection rds;
                
                connectionString = @"Data Source=growbro.cdkppreaz70m.us-east-2.rds.amazonaws.com;Initial Catalog=GrowBroDWH;User ID=admin;Password=adminadmin";

                rds = new SqlConnection(connectionString);
                rds.Open();
                Console.WriteLine("Connection Open");
                
                string sql, Output = "";
                
                sql = "select * from edwh.FactManagement where U_ID = @U_ID  and DH_ID = @DH_ID";

                command = new SqlCommand(sql, rds);
                command.Parameters.AddWithValue("@U_ID", userID);
                command.Parameters.AddWithValue("@DH_ID", greenhouseID);
                dataReader = command.ExecuteReader();
                ApiCurrentDataPackage apiCurrentDataPackage;
                double temperatur = 0;
                double fughtighed = 0;
                double co2 = 0;
                while (dataReader.Read())
                {
                    temperatur = dataReader.GetDouble(5);
                    fughtighed = dataReader.GetDouble(6);
                    co2 = dataReader.GetDouble(7);
                    Output = Output + dataReader.GetValue(5) + " - " + dataReader.GetValue(6)+ " - " + dataReader.GetValue(7) + "\n";
                }
                Console.WriteLine(Output);
                
                dataReader.Close();
                command.Dispose();
                rds.Close();
                
                
            }

            public async Task<Message> getGreenhouses(int userID)
           {
           //    string id = userID + "";

           //    string jsonString = JsonSerializer.Serialize(new Message
           //    {
           //        command = "GETGREENHOUSES",
           //        json = id
           //    });
           //    byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
           //    stream.Write(bytes,0,bytes.Length);
           //    
           //    byte[] bytesResponse = new byte[1024 * 1024];
           //    
           //    int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

           //    string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
           //    Message message = JsonSerializer.Deserialize<Message>(response);
              return new Message();
           }

           public async Task<Message> getUser(String username, String password)
           {
           //    string account = username + ":" + password;

           //    string jsonString = JsonSerializer.Serialize(new Message
           //    {
           //        command = "GETUSER",
           //        json = account
           //    });
           //    byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
           //    stream.Write(bytes,0,bytes.Length);
           //    
           //    byte[] bytesResponse = new byte[1024 * 1024];
           //    
           //    int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

           //    string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
           //    Message message = JsonSerializer.Deserialize<Message>(response);
               return new Message();
           }

           public async Task<Message> getGreenhouseByID(int userId, int greenHouseID)
           {
           //    string ids = userId + ":" + greenHouseID;
           //    string jsonString = JsonSerializer.Serialize(new Message
           //    {
           //        command = "GETGREENHOUSEBYID",
           //        json = ids
           //    });
           //    byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
           //    stream.Write(bytes,0,bytes.Length);
           //    
           //    byte[] bytesResponse = new byte[1024 * 1024];
           //    
           //    int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

           //    string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
           //    Message message = JsonSerializer.Deserialize<Message>(response);
               return new Message();
           }

           public async Task<Message> getAverageData(int userId, int greenHouseID)
           {
           //    string ids = userId + ":" + greenHouseID;
           //    string jsonString = JsonSerializer.Serialize(new Message
           //    {
           //        command = "GETAVERAGEDATA",
           //        json = ids
           //    });
           //    byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
           //    stream.Write(bytes,0,bytes.Length);
           //    
           //    byte[] bytesResponse = new byte[1024 * 1024];
           //    
           //    int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

           //    string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
           //    String[] split = response.Split("json\":\"");
           //    String test = split[1].Replace("\\", "");
           //    Char[] chars = test.ToCharArray();
           //    chars[chars.Length - 2] = ' ';
           //    chars[chars.Length - 1] = ' ';
           //    test = new string(chars);
           //    String test2 = test.Replace(" ", "");
           //    ApiCurrentDataPackage apiCurrentDataPackage =
           //        JsonSerializer.Deserialize<ApiCurrentDataPackage>(test2);
           //    double CO2 = 0;
           //    double Temperature = 0;
           //    double Humidity = 0;
           //    int count = 0;
           //    for (int i = 0; i < apiCurrentDataPackage.data.Count; i++)
           //    {
           //        if (apiCurrentDataPackage.data[i].type == DataType.CO2)
           //        {
           //            CO2 += apiCurrentDataPackage.data[i].data;
           //            count++;
           //        }
           //        else if (apiCurrentDataPackage.data[i].type == DataType.TEMPERATURE)
           //        {
           //            Temperature += apiCurrentDataPackage.data[i].data;
           //           
           //        }
           //        else if (apiCurrentDataPackage.data[i].type == DataType.HUMIDITY)
           //        {
           //            Humidity += apiCurrentDataPackage.data[i].data;
           //        }
           //    }
           //    Console.WriteLine("Count: "+ count + " CO2: " + CO2 + " Temperature: "+ Temperature + " Humidity: "+ Humidity);
           //    CO2 = CO2 / count;
           //    Temperature = Temperature / count;
           //    Humidity = Humidity / count;
           //    DataContainer CO2Container = new DataContainer(CO2, DataType.CO2);
           //    DataContainer temperatureContainer = new DataContainer(Temperature, DataType.TEMPERATURE);
           //    DataContainer HumidityContainer = new DataContainer(Humidity, DataType.HUMIDITY);
           //    List<DataContainer> dataContainers = new List<DataContainer>();
           //    dataContainers.Add(CO2Container);
           //    dataContainers.Add(temperatureContainer);
           //    dataContainers.Add(HumidityContainer);
           //    ApiCurrentDataPackage apiCurrent = new ApiCurrentDataPackage(dataContainers,apiCurrentDataPackage.lastDataPoint);
           //    Console.WriteLine(apiCurrent.data.ToString());
           //    String current = JsonSerializer.Serialize(apiCurrent);
           //    Message message = new Message();
           //    message.command = "SUCCES";
           //    message.json = current;
           //    Console.WriteLine(message.json);
           return new Message();
           }

           public async Task waterNow(int userId, int greenHouseID)
           {
           //    string ids = userId + ":" + greenHouseID;
           //    string jsonString = JsonSerializer.Serialize(new Message
           //    {
           //        command = "WATERNOW",
           //        json = ids
           //    });
           //    byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
           //    stream.Write(bytes,0,bytes.Length);
           }

           public async Task openWindow(int userId, int greenHouseID)
           {
           //    string ids = userId + ":" + greenHouseID;
           //    string jsonString = JsonSerializer.Serialize(new Message
           //    {
           //        command = "OPENWINDOW",
           //        json = ids
           //    });
           //    byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
           //    stream.Write(bytes,0,bytes.Length);
           }

           public async Task<Message> addGreenHouse(Greenhouse greenhouse)
           {
           //    string serializedString = JsonSerializer.Serialize(greenhouse);
           //    string jsonString = JsonSerializer.Serialize(new Message
           //    {
           //        command = "ADDGREENHOUSE",
           //        json = serializedString
           //    });
           //    byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
           //    stream.Write(bytes,0,bytes.Length);
           //         
           //    byte[] bytesResponse = new byte[1024 * 1024];
           //    
           //    int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

           //    string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
           //    Message message = JsonSerializer.Deserialize<Message>(response);
              return new Message();
           }

           public async Task<Message> addPlant(Plant plant)
           {
           //    string serializedString = JsonSerializer.Serialize(plant);
           //    string jsonString = JsonSerializer.Serialize(new Message
           //    {
           //        command = "ADDPLANT",
           //        json = serializedString
           //    });
           //    byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
           //    stream.Write(bytes,0,bytes.Length);
           //         
           //    byte[] bytesResponse = new byte[1024 * 1024];
           //    
           //    int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

           //    string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
           //    Message message = JsonSerializer.Deserialize<Message>(response);
           return new Message();
           }

           public async Task<Message> addUser(User user)
           {
           //    string serializedString = JsonSerializer.Serialize(user);
           //    string jsonString = JsonSerializer.Serialize(new Message
           //    {
           //        command = "ADDUSER",
           //        json = serializedString
           //    });
           //    byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
           //    stream.Write(bytes,0,bytes.Length);
           //         
           //    byte[] bytesResponse = new byte[1024 * 1024];
           //    
           //    int bytesRead = stream.Read(bytesResponse, 0, bytesResponse.Length);

           //    string response = Encoding.ASCII.GetString(bytesResponse, 0, bytesRead);
           //    Message message = JsonSerializer.Deserialize<Message>(response);
               return new Message();
           }
           //
        }
    }
