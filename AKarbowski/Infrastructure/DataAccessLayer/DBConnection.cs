using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace AKarbowski.DataAccessLayer
{
    public class DBConnection : IDisposable
    {
        private MySqlTransaction _transaction;
        private MySqlDataAdapter _adapter;
        private MySqlCommand _command;

        private MySqlConnection _connection;
        private string _server;
        private string _database;
        private string _uid;
        private string _password;

        public DBConnection()
        {
            Initialize();
        }

        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void RollBack()
        {
            _transaction.Rollback();
        }

        public void AddParameter(string name, MySqlDbType dbType, object value, ParameterDirection direction = ParameterDirection.Input)
        {
            var model = new MySqlParameter(name, dbType)
            {
                Value = value,
                Direction = direction
            };
            _command.Parameters.Add(model);
        }

        public void AddParameters(IEnumerable<MySqlParameter> parameters)
        {
            var parametersArray = parameters.ToArray();
            _command.Parameters.AddRange(parametersArray);
        }

        public void PrepearProcedureCall(string procedureName)
        {
            var query = procedureName;
            _command = _connection.CreateCommand();
            _command.CommandText = query;
            _command.CommandType = CommandType.StoredProcedure;
        }

        public DataSet ExecuteCommandFill()
        {
            var result = new DataSet();
            try
            {
                _adapter.SelectCommand = _command;
                _adapter.Fill(result);
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
            }

            return result;
        }

        public int ExecuteCommandInsert()
        {
            var result = 0;
            try
            {
                _command.ExecuteNonQuery();
                result = Convert.ToInt32(_command.Parameters["@result"].Value);
            }
            catch (Exception ex)
            {

                _transaction.Rollback();
            }
            return result;
        }


        //Initialize values
        private void Initialize()
        {
            _server = "localhost";
            _database = "akarbowski";
            _uid = "app";
            _password = "zaq1@WSX";
            string connectionString;
            connectionString = "SERVER=" + _server + ";" + "DATABASE=" +
            _database + ";" + "UID=" + _uid + ";" + "PASSWORD=" + _password + ";";

            _connection = new MySqlConnection(connectionString);
            _adapter = new MySqlDataAdapter();
            OpenConnection();
            _transaction = _connection.BeginTransaction();
        }

        private string BuildParameters(MySqlParameterCollection parameters)
        {
            var result = new StringBuilder("(");

            for (int i = 0; i < parameters.Count; i++)
            {
                result.Append(parameters[i].ParameterName + ",");
            }
            result.Remove(result.Length - 1, 1);
            result.Append(");");
            return result.ToString();
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:

                        break;

                    case 1045:

                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {

                _connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {

                return false;
            }
        }

        public void Dispose()
        {
            CloseConnection();
            _command.Dispose();
            _connection.Dispose();
        }
    }
}