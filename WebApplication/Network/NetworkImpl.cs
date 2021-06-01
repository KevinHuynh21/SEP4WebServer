using System;
using System.Collections;
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
                       house.temperatureThreshold.Add(dataReader.GetDouble(7));
                       house.temperatureThreshold.Add(dataReader.GetDouble(8));
                       house.co2Threshold.Add(dataReader.GetDouble(9));
                       house.co2Threshold.Add(dataReader.GetDouble(10));
                       house.humidityThreshold.Add(dataReader.GetDouble(11));
                       house.humidityThreshold.Add(dataReader.GetDouble(12));
                       house.WindowIsOpen = dataReader.GetBoolean(13);
                       house.waterFrequency = dataReader.GetInt32(15);
                       house.waterVolume = dataReader.GetDouble(16);
                       house.waterTimeOfDay = dataReader.GetString(17);
                       house.lastWaterDate = dataReader.GetString(18);
                       house.lastMeasurement = dataReader.GetString(19);
                       
                       GH.Add(house);
                   }
                  command.Dispose();
                  dataReader.Close();
                  for (int i = 0; i < GH.Count; i++)
                  {
                      sql = "select * from dbo.Plante where DrivhusID = @DrivhusID";
                      command = new SqlCommand(sql, rds);
                      command.Parameters.AddWithValue("DrivhusID", GH[i].greenHouseID);
                      dataReader = command.ExecuteReader();
                      Plant plant;
                      while (dataReader.Read())
                      {
                          plant = new Plant();
                          plant.plantID = dataReader.GetInt32(0);
                          plant.greenHouseID = dataReader.GetInt32(1);
                          plant.Name = dataReader.GetString(2);
                          plant.Url = dataReader.GetString(3);
                          GH[i].Plants.Add(plant);
                      }

                      command.Dispose();
                      dataReader.Close();
                  }
         
                  
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
               string sql;
               

               sql = "select * from dbo.drivhus where UserID = @User_ID and DrivhusID = @DrivhusID";
               command = new SqlCommand(sql, rds);
               command.Parameters.AddWithValue("User_ID", userId);
               command.Parameters.AddWithValue("DrivhusID", greenHouseID);
               dataReader = command.ExecuteReader();
               
             
               Greenhouse house = null;
               if(dataReader.Read())
               {
                   house = new Greenhouse();
                   SensorData sensTemperatur = new SensorData("Temperature", dataReader.GetDouble(5));
                   SensorData sensCO2 = new SensorData("CO2", dataReader.GetDouble(4));
                   SensorData sensFugtighed = new SensorData("Humidity", dataReader.GetDouble(6));
                   house.sensorData.Add(sensTemperatur);
                   house.sensorData.Add(sensCO2);
                   house.sensorData.Add(sensFugtighed);
                   house.userID = userId;
                   house.Name = dataReader.GetString(1);
                   house.greenHouseID = dataReader.GetInt32(0);
                   house.temperatureThreshold.Add(dataReader.GetDouble(7));
                   house.temperatureThreshold.Add(dataReader.GetDouble(8));
                   house.co2Threshold.Add(dataReader.GetDouble(9));
                   house.co2Threshold.Add(dataReader.GetDouble(10));
                   house.humidityThreshold.Add(dataReader.GetDouble(11));
                   house.humidityThreshold.Add(dataReader.GetDouble(12));
                   house.WindowIsOpen = dataReader.GetBoolean(13);
                   house.waterFrequency = dataReader.GetInt32(15);
                   house.waterVolume = dataReader.GetDouble(16);
                   house.waterTimeOfDay = dataReader.GetString(17);
                   house.lastWaterDate = dataReader.GetString(18);
                   house.lastMeasurement = dataReader.GetString(19);
                   Console.WriteLine(house.ToString());
               }

               command.Dispose();
               dataReader.Close();
               
               sql = "select * from dbo.Plante where DrivhusID = @DrivhusID";
               command = new SqlCommand(sql, rds);
               command.Parameters.AddWithValue("DrivhusID", house.greenHouseID);
               dataReader = command.ExecuteReader();
               Plant plant;
               while (dataReader.Read())
               {
                   plant = new Plant();
                   plant.plantID = dataReader.GetInt32(0);
                   plant.greenHouseID = dataReader.GetInt32(1);
                   plant.Name = dataReader.GetString(2);
                   plant.Url = dataReader.GetString(3);
                   house.Plants.Add(plant);
               }
               
               command.Dispose();
               dataReader.Close();
               rds.Close();
               string message = JsonSerializer.Serialize(house);
               return message;
           }

           public string getAverageData(int userId, int greenHouseID,DateTime timeFrom,DateTime timeTo)
           {
               SqlConnection rds;
               rds = new SqlConnection(connectionString);
               rds.Open();
               Console.WriteLine("Connection Open");
               string sql;
               
                    
               sql = "SELECT D_ID,Date FROM edwh.DimDate where not (Date > @RangeTill OR Date < @RangeFrom)";
               command = new SqlCommand(sql, rds);
               command.Parameters.AddWithValue("@RangeTill", timeTo);
               command.Parameters.AddWithValue("@RangeFrom", timeFrom);
                
               dataReader = command.ExecuteReader();
               List<string> d_IDList = new List<string>();
               List<DateTime> dateTimes = new List<DateTime>();
               while (dataReader.Read())
               {
                   d_IDList.Add(dataReader.GetString(0));
                   dateTimes.Add(dataReader.GetDateTime(1));
               }
               command.Dispose();
               dataReader.Close();
               List<ApiCurrentDataPackage> acdpList = new List<ApiCurrentDataPackage>();
               Console.WriteLine(d_IDList.Count);
               for (int i = 0; i < d_IDList.Count; i++)
               {

                   sql = "select distinct U_ID,DH_ID,Temperatur,CO2,Fugtighed from edwh.FactManagement where U_ID = @U_ID  and DH_ID = @DH_ID and D_ID = @D_ID";
                   command = new SqlCommand(sql, rds);
                   command.Parameters.AddWithValue("@U_ID", userId);
                   command.Parameters.AddWithValue("@DH_ID", greenHouseID);
                   command.Parameters.AddWithValue("@D_ID", d_IDList[i]);
                   dataReader = command.ExecuteReader();

                   ApiCurrentDataPackage apiCurrentDataPackage;
                   
                   while (dataReader.Read())
                   {
                       List<DataContainer> DataContainers = new List<DataContainer>();
                       DataContainer co2Container = new DataContainer(dataReader.GetDouble(3), DataType.CO2);
                       DataContainer TemperatureContainer = new DataContainer(dataReader.GetDouble(2), DataType.TEMPERATURE);
                       DataContainer humidityContainer = new DataContainer(dataReader.GetDouble(4), DataType.HUMIDITY);
                       DataContainers.Add(co2Container);
                       DataContainers.Add(TemperatureContainer);
                       DataContainers.Add(humidityContainer);
                       apiCurrentDataPackage = new ApiCurrentDataPackage(DataContainers, dateTimes[i]);
                       acdpList.Add(apiCurrentDataPackage);
                   }
                    Console.WriteLine(acdpList.Count);
                   command.Dispose();
                   dataReader.Close();
                   
               }
               rds.Close();
               for (int i = 0; i < acdpList.Count; i++)
               {
                   Console.WriteLine(acdpList[i].data[0].data);
               }
             String current = JsonSerializer.Serialize(acdpList);
             return current;
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
           }

           public async Task openWindow(int userId, int greenHouseID,int openOrClose)
           {
               rds = new SqlConnection(connectionString);
               Console.WriteLine("open");
               rds.Open();
               string statement = "update dbo.Drivhus set WindowIsOpen=@WindowCommand where DrivhusID=@GH_ID";
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

           }

           public async Task<Message> addGreenHouse(Greenhouse greenhouse)
           {
               rds = new SqlConnection(connectionString);
               Console.WriteLine("open");
               rds.Open();
               string statement = "insert into dbo.Drivhus (DrivhusID,Navn,UserID,CO2,Temperatur,Fugtighed,MinThresholdTemp,MaxThresholdTemp,MinThresholdCO2,MaxThresholdCO2,MinThresholdMoist,MaxThresholdMoist,WindowIsOpen,WaterNow,WaterFrequency,WaterVolume,WaterTimeOfDay,LastWaterDate,LastMeasurement) values(@GH_ID,@Name,@U_ID,@CO2,@Temp,@Hum,@minTemp,@maxTemp,@minCO2,@maxCO2,@minMoist,@maxMoist,@WinIOP,@WaterNow,@waterFreq,@waterVolume,@waterTimeOfDay,@lastWaterDate,@lastMeasurement)";
               command = new SqlCommand(statement, rds);
               Console.WriteLine(greenhouse.Name);
               command.Parameters.AddWithValue("@GH_ID", greenhouse.greenHouseID);
               command.Parameters.AddWithValue("@Name", greenhouse.Name);
               command.Parameters.AddWithValue("@U_ID", greenhouse.userID);
               command.Parameters.AddWithValue("@CO2", 0);
               command.Parameters.AddWithValue("@Temp", 0);
               command.Parameters.AddWithValue("@Hum", 0);
               command.Parameters.AddWithValue("@minTemp", double.Parse(greenhouse.temperatureThreshold[0].ToString()));
               command.Parameters.AddWithValue("@maxTemp", double.Parse(greenhouse.temperatureThreshold[1].ToString()));
               command.Parameters.AddWithValue("@minCO2", double.Parse(greenhouse.co2Threshold[0].ToString()));
               command.Parameters.AddWithValue("@maxCO2", double.Parse(greenhouse.co2Threshold[1].ToString()));
               command.Parameters.AddWithValue("@minMoist", double.Parse(greenhouse.humidityThreshold[0].ToString()));
               command.Parameters.AddWithValue("@maxMoist", double.Parse(greenhouse.humidityThreshold[1].ToString()));
               command.Parameters.AddWithValue("@WinIOP", 0);
               command.Parameters.AddWithValue("@WaterNow", 0);
               command.Parameters.AddWithValue("@waterFreq", greenhouse.waterFrequency);
               command.Parameters.AddWithValue("@waterVolume", greenhouse.waterVolume);
               command.Parameters.AddWithValue("@waterTimeOfDay", greenhouse.waterTimeOfDay);
               command.Parameters.AddWithValue("@lastWaterDate", greenhouse.lastWaterDate);
               command.Parameters.AddWithValue("@lastMeasurement", greenhouse.lastMeasurement);
               adapter = new SqlDataAdapter();
               adapter.UpdateCommand = command;
               adapter.UpdateCommand.ExecuteNonQuery();
               Console.WriteLine("close and dispose");
               adapter.Dispose();
               command.Dispose();
               for (int i = 0; i < greenhouse.Plants.Count; i++)
               {
                   string sql = "insert into dbo.Plante (DrivhusID,Navn,PicURL) values(@GH_ID,@Name,@URL";
                   command = new SqlCommand(sql, rds);
                   command.Parameters.AddWithValue("@GH_ID", greenhouse.greenHouseID);
                   command.Parameters.AddWithValue("@Name", greenhouse.Plants[i].Name);
                   command.Parameters.AddWithValue("@URL", greenhouse.Plants[i].Url);
                   adapter = new SqlDataAdapter();
                   adapter.UpdateCommand = command;
                   adapter.UpdateCommand.ExecuteNonQuery();
                   adapter.Dispose();
                   command.Dispose();
               }
               for (int i = 0; i < greenhouse.sharedWith.Count; i++)
               {
                   statement = "INSERT INTO dbo.SharedWith (UserID,DrivhusID) VALUES (@UserID,@DrivhusID)";
                   command = new SqlCommand(statement, rds);
                   command.Parameters.AddWithValue("@UserID", greenhouse.sharedWith[i]);
                   command.Parameters.AddWithValue("@DrivhusID", greenhouse.greenHouseID);
                   adapter = new SqlDataAdapter();
                   Console.WriteLine("execute");
                   adapter.UpdateCommand = command;
                   adapter.UpdateCommand.ExecuteNonQuery();
                   Console.WriteLine("close and dispose");
                   adapter.Dispose();
                   command.Dispose();
               }
               
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
           message.json = "{\"Value\":" + greenhouse.greenHouseID + "}"; ;
           return message;
           }

           public async Task<Message> addPlant(Plant plant)
           {
               rds = new SqlConnection(connectionString);
               rds.Open();
               string statement = "insert into dbo.Plante (DrivhusID,Navn,PicURL) values(@GH_ID,@Name,@URL)";
               Console.WriteLine("command");
               command = new SqlCommand(statement, rds);
               command.Parameters.AddWithValue("@GH_ID", plant.greenHouseID);
               command.Parameters.AddWithValue("@Name", plant.Name);
               command.Parameters.AddWithValue("@URL", plant.Url);
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
            message.json = "{\"Value\":" + id + "}"; ;
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
               Message message = new Message();
           message.json = "{\"Value\":" + id + "}"; ;
           Console.WriteLine(id);
           return message;
           }

           public string getFriendsGreenhouses(int userId)
           {
               SqlConnection rds;
                   rds = new SqlConnection(connectionString);
                   rds.Open();
                   Console.WriteLine("Connection Open");
                   string sql;
               
                   sql = "select DrivhusID from dbo.SharedWith where UserID = @User_ID";
                   command = new SqlCommand(sql, rds);
                   command.Parameters.AddWithValue("User_ID", userId);
                   dataReader = command.ExecuteReader();
                   
                   List<Greenhouse> listOfGreenhouses = new List<Greenhouse>();
                   List<int> listOfgreenhouseID = new List<int>();
                   int tal;
                   while(dataReader.Read())
                   {
                       tal = dataReader.GetInt32(0);
                       listOfgreenhouseID.Add(tal);
                   }
                   command.Dispose();
                   dataReader.Close();
                   rds.Close();
                   listOfGreenhouses = getGreenhousesForGetFriendsGreenhouses(listOfgreenhouseID);
                   string message = JsonSerializer.Serialize(listOfGreenhouses);
                   return message;
               }

           public List<Greenhouse> getGreenhousesForGetFriendsGreenhouses(List<int> list)
           {
               SqlConnection rds;
               rds = new SqlConnection(connectionString);
               rds.Open();
               Console.WriteLine("Connection Open");
               string sql;
               List<Greenhouse> listOfGreenhouses = new List<Greenhouse>();
               for (int i = 0; i < list.Count; i++)
               {
                   sql = "select * from dbo.Drivhus where DrivhusID = @Drivhus_ID";
                   command = new SqlCommand(sql, rds);
                   command.Parameters.AddWithValue("Drivhus_ID", list[i]);
                   dataReader = command.ExecuteReader();
                   Greenhouse house = null;
                   if(dataReader.Read())
                   {
                       house = new Greenhouse();
                       SensorData sensTemperatur = new SensorData("Temperature", dataReader.GetDouble(5));
                       SensorData sensCO2 = new SensorData("CO2", dataReader.GetDouble(4));
                       SensorData sensFugtighed = new SensorData("Humidity", dataReader.GetDouble(6));
                       house.sensorData.Add(sensTemperatur);
                       house.sensorData.Add(sensCO2);
                       house.sensorData.Add(sensFugtighed);
                       house.userID = dataReader.GetInt32(2);
                       house.Name = dataReader.GetString(1);
                       house.greenHouseID = dataReader.GetInt32(0);
                       house.temperatureThreshold.Add(dataReader.GetDouble(7));
                       house.temperatureThreshold.Add(dataReader.GetDouble(8));
                       house.co2Threshold.Add(dataReader.GetDouble(9));
                       house.co2Threshold.Add(dataReader.GetDouble(10));
                       house.humidityThreshold.Add(dataReader.GetDouble(11));
                       house.humidityThreshold.Add(dataReader.GetDouble(12));
                       house.WindowIsOpen = dataReader.GetBoolean(13);
                       house.waterFrequency = dataReader.GetInt32(15);
                       house.waterVolume = dataReader.GetDouble(16);
                       house.waterTimeOfDay = dataReader.GetString(17);
                       house.lastWaterDate = dataReader.GetString(18);
                       house.lastMeasurement = dataReader.GetString(19);
                   }
                   listOfGreenhouses.Add(house);
                   dataReader.Close();
               }
               command.Dispose();
               
               for (int i = 0; i < listOfGreenhouses.Count; i++)
               {
                   sql = "select * from dbo.Plante where DrivhusID = @DrivhusID";
                   command = new SqlCommand(sql, rds);
                   command.Parameters.AddWithValue("DrivhusID", listOfGreenhouses[i].greenHouseID);
                   dataReader = command.ExecuteReader();
                   Plant plant;
                   while (dataReader.Read())
                   {
                       plant = new Plant();
                       plant.plantID = dataReader.GetInt32(0);
                       plant.greenHouseID = dataReader.GetInt32(1);
                       plant.Name = dataReader.GetString(2);
                       plant.Url = dataReader.GetString(3);
                       listOfGreenhouses[i].Plants.Add(plant);
                   }

                   command.Dispose();
                   dataReader.Close();
               }
               rds.Close();
               return listOfGreenhouses;
           }
           
        }
    }
