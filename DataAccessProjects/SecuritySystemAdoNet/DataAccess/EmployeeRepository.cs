﻿
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
  public  class EmployeeRepository:DataConnection
    {
        private IEnumerable<EmployeeModel> ReadEmployeeWithDataTable(string predicate, params SqlParameter[] parameters)
        {
            DataTable table = new DataTable(); 
            Sqlconn.Open();
            var cmd = new SqlCommand("select * from Employee e " + predicate, Sqlconn);
            foreach (var item in parameters)
            {
                cmd.Parameters.Add(item);
            }
            SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
            Adapter.Fill(table);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                yield return new EmployeeModel
                {
                    EmployeeId = table.Rows[i].Field<int>("EmployeeId"),
                    FirstName = table.Rows[i].Field<string>("FirstName"),
                    LastName = table.Rows[i].Field<string>("LastName")
                };
            }
            Sqlconn.Close();
        }
        private IEnumerable<EmployeeModel> ReadEmployee(string predicate,params SqlParameter[] parameters  )
        {
            Sqlconn.Open();
            var cmd = new SqlCommand("select * from Employee e " + predicate, Sqlconn);
            foreach (var item in parameters)
            {
                cmd.Parameters.Add(item);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                yield return new EmployeeModel
                {
                    EmployeeId = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2)
                };
            }
            Sqlconn.Close();
        }
      
        public IEnumerable<EmployeeModel> ReadEmployeeByPk(int Pk)
        {
            return ReadEmployee(
                "where e.EmployeeId=@Pk",
                new SqlParameter ("@Pk",Pk)
                );
        }
        public IEnumerable<EmployeeModel> ReadEmployeeByName(string name)
        {
            return ReadEmployee("where  e.FirstName like '%'+@Name+ '%' or e.LastName like '%'+ @Name +'%' or @Name='' ",
                new SqlParameter("@Name", name)
                );          
        }
        public IEnumerable<EmployeeModel> ReadEmployeeByNameUsingDataTable(string name)
        {
            return ReadEmployeeWithDataTable("where  e.FirstName like '%'+@Name+ '%' or e.LastName like '%'+ @Name +'%' or @Name='' ",
                new SqlParameter("@Name", name)
                );
        }
        public IEnumerable<EmployeeModel> ReadEmployeeByPkUsingDataTable(int Pk)
        {
            return ReadEmployeeWithDataTable(
                "where e.EmployeeId=@Pk",
                new SqlParameter("@Pk", Pk)
                );
        }
        public void CreateEmployee(EmployeeModel model)
        {
            Sqlconn.Open();
//            SqlCommand cmd2 = new SqlCommand(@"if exists(select * from Employee e where e.EmployeeId=@Pk)
//begin
//update Employee
//set FirstName = @FName,
//LastName = @LName where EmployeeId = @Pk
//end
//else
//                begin
//                insert  Employee(FirstName, LastName)values(@FName, @LName)
//select scope_identity()
//end");
            SqlCommand cmd = new SqlCommand("insert  Employee(FirstName,LastName)values(@FName,@LName)",Sqlconn);
            cmd.Parameters.AddWithValue("@Fname", model.FirstName);
            cmd.Parameters.AddWithValue("@LName", model.LastName);
            // cmd.Parameters.AddWithValue("@Pk", model.EmployeeId);
            cmd.ExecuteNonQuery();
            Sqlconn.Close();
        }
        public void DeleteEmployee(int Id)
        {
            Sqlconn.Open();
            SqlCommand cmd = new SqlCommand("delete  Employee  Where EmployeeId=@Pk",Sqlconn);
            cmd.Parameters.AddWithValue("@Pk", Id);
            cmd.ExecuteNonQuery();
            Sqlconn.Close();
        }
     
    }
}
