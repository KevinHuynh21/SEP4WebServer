﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
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
            private string connectionString = @"Data Source=growbro.cdkppreaz70m.us-east-2.rds.amazonaws.com;Initial Catalog=GrowBroDWH;User ID=admin;Password=adminadmin";
            public NetworkImpl()
            {
            }

            
            public string GetCurrentData(int userID, int greenhouseID)
            {
                SqlConnection rds;
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
                DateTime time = new DateTime();
                string id = null;
                while (dataReader.Read())
                {
                    id = dataReader.GetString(4);
                    temperatur = dataReader.GetDouble(5);
                    co2 = dataReader.GetDouble(6);
                    fughtighed = dataReader.GetDouble(7);
                    Output = Output + dataReader.GetValue(4) +  " - " + dataReader.GetValue(5)+ " - " + dataReader.GetValue(6)+ " - " + dataReader.GetValue(7) + "\n";
                }
                                
                Console.WriteLine(Output);
                dataReader.Close();
                command.Dispose();

                sql = "SELECT Date FROM edwh.DimDate WHERE D_ID = @D_ID";
                command = new SqlCommand(sql, rds);
                command.Parameters.AddWithValue("@D_ID", int.Parse(id));
                
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    time = dataReader.GetDateTime(0);
                }

                DataContainer temperaturContainer= new DataContainer(temperatur, DataType.TEMPERATURE);
                DataContainer fugtighedContainer = new DataContainer(fughtighed, DataType.HUMIDITY);
                DataContainer co2Container = new DataContainer(co2, DataType.CO2);

                List<DataContainer> list = new List<DataContainer>();
                list.Add(temperaturContainer);
                list.Add(fugtighedContainer);
                list.Add(co2Container);

                apiCurrentDataPackage = new ApiCurrentDataPackage(list, time);
                string message = JsonSerializer.Serialize(apiCurrentDataPackage);

                Console.WriteLine(apiCurrentDataPackage.lastDataPoint);
                dataReader.Close();
                command.Dispose();
                rds.Close();

                return message;
            }

            public string getGreenhouses(int userID)
           {
               SqlConnection rds;
               rds = new SqlConnection(connectionString);
               rds.Open();
               Console.WriteLine("Connection Open");
               string sql;

               sql = "select * from edwh.DimEjer where UserID = @UserID";
               command = new SqlCommand(sql, rds);
               command.Parameters.AddWithValue("@UserID", userID);
               dataReader = command.ExecuteReader();
               
               int uId = 0;
               while (dataReader.Read())
               {
                   uId = dataReader.GetInt32(0);
               }
               command.Dispose();
               dataReader.Close();

               sql = "select D_ID, DH_ID, Temperatur, CO2,Fugtighed  from edwh.FactManagement where U_ID = @U_ID";
               command = new SqlCommand(sql, rds);
               command.Parameters.AddWithValue("U_ID", uId);
               dataReader = command.ExecuteReader();
               
               List<int> dhID = new List<int>();
               Greenhouse house = null;
               List<Greenhouse> GH = new List<Greenhouse>();

               do
               {
                  while(dataReader.Read())
                   {
                       house = new Greenhouse();
                       SensorData sensTemperatur = new SensorData("Temperature", dataReader.GetDouble(2));
                       SensorData sensCO2 = new SensorData("CO2", dataReader.GetDouble(3));
                       SensorData sensFugtighed = new SensorData("Humidity", dataReader.GetDouble(4));
                       house.sensorData.Add(sensTemperatur);
                       house.sensorData.Add(sensCO2);
                       house.sensorData.Add(sensFugtighed);
                       house.userID = userID;
                       dhID.Add(dataReader.GetInt32(1));
                       GH.Add(house);
                   }
               } while (dataReader.NextResult());

               command.Dispose();
               dataReader.Close();

               for (int i = 0; i < dhID.Count; i++)
               {
                   sql = "select Navn, DrivhusID from edwh.DimDrivhus where DH_ID = @DH_ID";

                   command = new SqlCommand(sql, rds);
                   command.Parameters.AddWithValue("@DH_ID", dhID[i]);

                   dataReader = command.ExecuteReader();

                   string name = null;

                   if (dataReader.Read())
                   {
                       GH[i].Name = dataReader.GetString(0);
                       GH[i].greenHouseID = dataReader.GetInt32(1); 
                   }
                   command.Dispose();
                   dataReader.Close();
                 
               }
               rds.Close();
               string message = JsonSerializer.Serialize(GH[1]);
               return message;
   
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

           public string getGreenhouseByID(int userId, int greenHouseID)
           {
               SqlConnection rds;
                
               

               rds = new SqlConnection(connectionString);
               rds.Open();
               Console.WriteLine("Connection Open");
                
               string sql, Output = "";
               sql = "select * from dbo.Drivhus where DrivhusID = @DrivhusID and UserID = @UserID";

               command = new SqlCommand(sql, rds);

               string message = null;
               return message;
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
