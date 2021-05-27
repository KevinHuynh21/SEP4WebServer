using System;
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
            SqlConnection rds;
            SqlCommand command;
            SqlDataReader dataReader;
            SqlDataAdapter adapter;
            NetworkStream stream;
            string connectionString;
            TcpClient client;
            public NetworkImpl()
            {
                connectionString = @"Data Source=growbro.cdkppreaz70m.us-east-2.rds.amazonaws.com;Initial Catalog=GrowBroDWH;User ID=admin;Password=adminadmin";
                getUser("Kasper", "21345");
            }

            
            public string GetCurrentData(int userID, int greenhouseID)
            {
                
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
                string id = null;
                while (dataReader.Read())
                {
                    id = dataReader.GetString(1);
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
                DateTime time = new DateTime();
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
               

               sql = "select * from dbo.drivhus where UserID = @User_ID";
               command = new SqlCommand(sql, rds);
               command.Parameters.AddWithValue("User_ID", userID);
               dataReader = command.ExecuteReader();
               
             
               Greenhouse house = null;
               List<Greenhouse> GH = new List<Greenhouse>();
               
                  while(dataReader.Read())
                   {
                       house = new Greenhouse();
                       SensorData sensTemperatur = new SensorData("Temperature", dataReader.GetDouble(5));
                       SensorData sensCO2 = new SensorData("CO2", dataReader.GetDouble(4));
                       SensorData sensFugtighed = new SensorData("Humidity", dataReader.GetDouble(6));
                       house.sensorData.Add(sensTemperatur);
                       house.sensorData.Add(sensCO2);
                       house.sensorData.Add(sensFugtighed);
                       house.userID = userID;
                       house.Name = dataReader.GetString(1);
                       house.greenHouseID = dataReader.GetInt32(0);
                       Console.WriteLine(house.ToString());
                       GH.Add(house);
                   }
         
                   command.Dispose();
                   dataReader.Close();
                   rds.Close();
               string message = JsonSerializer.Serialize(GH);
               return message;
   
           }

           public string getUser(String username, String password)
           {
               SqlConnection rds;
               rds = new SqlConnection(connectionString);
               rds.Open();
               Console.WriteLine("Connection Open");
               string sql;
               

               sql = "select * from dbo.ejer where Username = @Username and Password = @Password";
               command = new SqlCommand(sql, rds);
               command.Parameters.AddWithValue("Username", username);
               command.Parameters.AddWithValue("Password", password);
               dataReader = command.ExecuteReader();
               User user = new User();
               if (dataReader.Read())
               {
                   user.Id = dataReader.GetInt32(0);
                   user.Username = dataReader.GetString(1);
                   user.Password = dataReader.GetString(2);
               }
               command.Dispose();
               dataReader.Close();
               rds.Close();
               String userSerialized = JsonSerializer.Serialize(user);
               return userSerialized;
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
               rds = new SqlConnection(connectionString);
               Console.WriteLine("open");
               rds.Open();
               string statement = "update dbo.Drivhus set WaterNow=1 where DrivhusID=@GH_ID";
               Console.WriteLine("command");
               command = new SqlCommand(statement, rds);
               command.Parameters.AddWithValue("@GH_ID", greenHouseID);
               Console.WriteLine("adapter");
               adapter = new SqlDataAdapter();
               Console.WriteLine("execute");
               adapter.UpdateCommand = command;
               adapter.UpdateCommand.ExecuteNonQuery();
               
               Console.WriteLine("close and dispose");
               command.Dispose();
               rds.Close();
                 /* string ids = userId + ":" + greenHouseID;
                  string jsonString = JsonSerializer.Serialize(new Message
                  {
                      command = "WATERNOW",
                      json = ids
                   });
                   byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                  stream.Write(bytes,0,bytes.Length);*/
           }

           public async Task openWindow(int userId, int greenHouseID,int openOrClose)
           {
               rds = new SqlConnection(connectionString);
               Console.WriteLine("open");
               rds.Open();
               string statement = "update dbo.Drivhus set WindowIsOpen=@WinowCommand where DrivhusID=@GH_ID";
               Console.WriteLine("command");
               command = new SqlCommand(statement, rds);
               command.Parameters.AddWithValue("@GH_ID", greenHouseID);
               command.Parameters.AddWithValue("@WindowCommand", openOrClose);
               Console.WriteLine("adapter");
               adapter = new SqlDataAdapter();
               Console.WriteLine("execute");
               adapter.UpdateCommand = command;
               adapter.UpdateCommand.ExecuteNonQuery();
               
               Console.WriteLine("close and dispose");
               command.Dispose();
               rds.Close();
               
               
             /*  Console.WriteLine("send to java");
               if (openOrClose==1)
               {
                   string ids = userId + ":" + greenHouseID;
                   string jsonString = JsonSerializer.Serialize(new Message
                   {
                       command = "OPENWINDOW",
                       json = ids
                   });
                   byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                   stream.Write(bytes, 0, bytes.Length);
               }
               else if (openOrClose==0)
               {
                   string ids = userId + ":" + greenHouseID;
                   Console.WriteLine("close window");
                   string jsonString = JsonSerializer.Serialize(new Message
                   {
                       command = "CLOSEWINDOW",
                       json = ids
                   });
                   byte[] bytes = Encoding.ASCII.GetBytes(jsonString);
                   stream.Write(bytes, 0, bytes.Length);
               }*/
               
           }

           public async Task<Message> addGreenHouse(Greenhouse greenhouse)
           {
               rds = new SqlConnection(connectionString);
               Console.WriteLine("open");
               rds.Open();
               string statement = "insert into dbo.Drivhus (DrivhusID,Navn,UserID,CO2,Temperatur,Fugtighed,WindowIsOpen,WaterNow) values(@GH_ID,@Name,@U_ID,@CO2,@Temp,@Hum,@WinIOP,@WaterNow)";
               Console.WriteLine("command");
               command = new SqlCommand(statement, rds);
               command.Parameters.AddWithValue("@GH_ID", greenhouse.greenHouseID);
               command.Parameters.AddWithValue("@Name", greenhouse.Name);
               command.Parameters.AddWithValue("@U_ID", greenhouse.userID);
               command.Parameters.AddWithValue("@CO2", 0);
               command.Parameters.AddWithValue("@Temp", 0);
               command.Parameters.AddWithValue("@Hum", 0);
               command.Parameters.AddWithValue("@WinIOP", 0);
               command.Parameters.AddWithValue("@WaterNow", 0);
               Console.WriteLine("adapter");
               adapter = new SqlDataAdapter();
               Console.WriteLine("execute");
               adapter.UpdateCommand = command;
               adapter.UpdateCommand.ExecuteNonQuery();
               
               Console.WriteLine("close and dispose");
               command.Dispose();
               rds.Close();
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
           Message message = new Message();
           message.json = JsonSerializer.Serialize(greenhouse.greenHouseID);
           return message;
           }

           public async Task<Message> addPlant(Plant plant)
           {
               rds = new SqlConnection(connectionString);
               rds.Open();
               string statement = "insert into dbo.Plante (DrivhusID,Navn,TemperaturKrav,CO2Krav,FugtighedsKrav,PlanteScore) values(@GH_ID,@Name,@Temp,@CO2,@Hum,@PlSc)";
               Console.WriteLine("command");
               command = new SqlCommand(statement, rds);
               command.Parameters.AddWithValue("@GH_ID", plant.greenHouseID);
               command.Parameters.AddWithValue("@Name", plant.Name);
               command.Parameters.AddWithValue("@Temp", plant.TemperatureRequirement);
               command.Parameters.AddWithValue("@CO2",plant.CO2Requirement );
               command.Parameters.AddWithValue("@Hum", plant.HumidityRequirement);
               command.Parameters.AddWithValue("@PlSc", plant.PlantScore);
               Console.WriteLine("adapter");
               adapter = new SqlDataAdapter();
               Console.WriteLine("execute");
               adapter.UpdateCommand = command;
               adapter.UpdateCommand.ExecuteNonQuery();
               Console.WriteLine("close and dispose");
               command.Dispose();
               adapter.Dispose();

               statement = "select PlanteID from dbo.Plante where Navn = @Navn AND DrivhusID = @Dh_ID ";
               command = new SqlCommand(statement, rds);
               command.Parameters.AddWithValue("@Navn", plant.Name);
               command.Parameters.AddWithValue("@Dh_ID", plant.greenHouseID);
               dataReader = command.ExecuteReader();
               int id = -1;
               while (dataReader.Read())
               {
                   id = dataReader.GetInt32(0);
               }
               command.Dispose();
               dataReader.Close();
               
               rds.Close();
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
            Message message = new Message();
            message.json = JsonSerializer.Serialize(id);
            Console.WriteLine(id);
            return message;
           }

           public async Task<Message> addUser(User user)
           {
               rds = new SqlConnection(connectionString);
               rds.Open();
               string statement = "insert into dbo.Ejer (Username,Password) values(@U_Name,@Password)";
               Console.WriteLine("command");
               command = new SqlCommand(statement, rds);
               command.Parameters.AddWithValue("@U_Name", user.Username);
               command.Parameters.AddWithValue("@Password", user.Password);
               Console.WriteLine("adapter");
               adapter = new SqlDataAdapter();
               Console.WriteLine("execute");
               adapter.UpdateCommand = command;
               adapter.UpdateCommand.ExecuteNonQuery();
               
               Console.WriteLine("close and dispose");
               command.Dispose();
               adapter.Dispose();
               
               statement = "select UserID from dbo.Ejer where Username = @U_Name AND Password = @PW ";
               command = new SqlCommand(statement, rds);
               command.Parameters.AddWithValue("@U_Name", user.Username);
               command.Parameters.AddWithValue("@PW", user.Password);
               dataReader = command.ExecuteReader();
               int id = -1;
               while (dataReader.Read())
               {
                   id = dataReader.GetInt32(0);
               }
               command.Dispose();
               dataReader.Close();
               rds.Close();
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
           Message message = new Message();
           message.json = JsonSerializer.Serialize(id);
           Console.WriteLine(id);
           return message;
           }
        }
    }
