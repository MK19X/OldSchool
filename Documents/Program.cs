using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;

namespace OldSchoolCcode
{
    class Program
    {
        static void Main(string[] args)
        {
            const string OSPsql = "Server=localhost;Port=5433;User Id=postgres;Password=2020;Database=OldSchoolPGDataBase";

            OSConsole ostc = new OSConsole();
            OSDataBaseOperations odbo = new OSDataBaseOperations();

        Menu:
            Console.Clear();
            Console.WriteLine("Postgress 14 DataBase emulator:");
            Console.WriteLine("Choose data base to work with :");
            Console.WriteLine("1. Old School");
            Console.WriteLine("2. exit");
            int menuA = ostc.UserNumInput(1, 2);

            string csql = "err";

            switch (menuA)
            {
                case 1:
                    csql = OSPsql;
                    goto ChooseAction;
                case 2:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Error, try again");
                    goto Menu;
            }
        ChooseAction:
            Console.Clear();
            Console.WriteLine("Choose action :");
            Console.WriteLine("1. Show Table Values");
            Console.WriteLine("2. Insert Random Users (50)");
            Console.WriteLine("3. Insert Random Games (10)");
            Console.WriteLine("4. Clear Table");
            Console.WriteLine("5. Create Account");
            Console.WriteLine("6. Delete Account");
            Console.WriteLine("7. back");
            
            int menuB = ostc.UserNumInput(1, 7);
            
            switch (menuB)
            {
                case 1:
                    Console.Clear();
                    odbo.ShowTableExecution(csql);
                    Console.WriteLine("Operation clear");
                    Console.WriteLine("Type any key :");
                    Console.ReadKey();
                    goto ChooseAction;
                case 2:
                    Console.Clear();
                    odbo.RandomUsersInsert(csql, 50);
                    Console.WriteLine("Operation clear");
                    Console.WriteLine("Type any key :");
                    Console.ReadKey();
                    goto ChooseAction;
                case 3:
                    Console.Clear();
                    odbo.RandomGameInsert(csql, 10);
                    Console.WriteLine("Operation clear");
                    Console.WriteLine("Type any key :");
                    Console.ReadKey();
                    goto ChooseAction;
                case 4:
                    Console.Clear();
                    odbo.FuncClearTest(csql);
                    Console.WriteLine("Operation clear");
                    Console.WriteLine("Type any key :");
                    Console.ReadKey();
                    goto ChooseAction;
                case 5:
                    Console.Clear();
                    odbo.CreateAccount(csql);
                    Console.WriteLine("Operation clear");
                    Console.WriteLine("Type any key :");
                    Console.ReadKey();
                    goto ChooseAction;
                case 6:
                    Console.Clear();
                    odbo.DeleteByIDWP(csql);
                    Console.WriteLine("Operation clear");
                    Console.WriteLine("Type any key :");
                    Console.ReadKey();
                    goto ChooseAction;
                case 7:
                    Console.Clear();
                    goto Menu;
                default:
                    Console.Clear();
                    Console.WriteLine("Error, try again");
                    Console.WriteLine("Type any key :");
                    Console.ReadKey();
                    goto ChooseAction;
            }
        }
    }
    class OSDataBaseOperations
    {
        public void ShowTableExecution(string connectionString)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                OSConsole c = new OSConsole();
            ShowTableExecutionEx:
                try
                {
                    string tableName;

                    Console.WriteLine("Enter a table name to show :");
                    tableName = Console.ReadLine();
                    Console.WriteLine($"You entered '{tableName}'");

                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        Console.WriteLine("| Count of columns |");
                        int k = c.UserNumInput(2, 5);
                        string[] entercolumn = new string[k];

                        for (int i = 0; i < k; i++)
                        {
                            Console.Write($"Enter column number {i + 1} to show :");
                            entercolumn[i] = Console.ReadLine().ToString();
                            Console.WriteLine($"You entered '{entercolumn[i]}'");
                        }
                        ShowTableExecutionExSort:
                        Console.WriteLine("Sorted by :");
                        string sortby = Console.ReadLine();
                        if (entercolumn.Contains(sortby))
                        {
                            cmd.CommandText = $"SELECT * FROM {tableName} ORDER BY {sortby}";
                            NpgsqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                for (int i = 0; i < k; i++)
                                {
                                    Console.Write(dr[entercolumn[i]].ToString() + "|");
                                }
                                Console.WriteLine("\n");
                            }
                            cmd.Parameters.Clear();
                            dr.Close();
                        }
                        else {
                            Console.WriteLine("Error ! You typed column to sort by, which is not in selected clolumns!");
                            Console.WriteLine("Choose next options :");
                            Console.WriteLine("1. Enter another column");
                            Console.WriteLine("2. Start again");
                            Console.WriteLine("3. exit");
                            int choose = c.UserNumInput(1, 2);
                            switch (choose)
                            {
                                case 1:
                                    Console.Clear();
                                    goto ShowTableExecutionExSort;
                                case 2:
                                    Console.Clear();
                                    goto ShowTableExecutionEx;
                                case 3:
                                    Console.Clear();
                                    return;
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Wrong input!");
                    Console.WriteLine("Choose next options :");
                    Console.WriteLine("1. Try Again");
                    Console.WriteLine("2. Cancel");
                    int choose = c.UserNumInput(1, 2);
                    switch (choose)
                    {
                        case 1:
                            Console.Clear();
                            goto ShowTableExecutionEx;
                        case 2:
                            Console.Clear();
                            return;
                    }
                }
            }
        }
        public void RandomUsersInsert(string connectionString, int n)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string table = "OSUser";
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        cmd.CommandText = $"SELECT MAX(id) FROM {table}";
                        cmd.ExecuteNonQuery();
                        int newId = (int)cmd.ExecuteScalar();
                        cmd.Parameters.Clear();

                        for (int i = 0; i < n; i++)
                        {
                            newId += 1;

                            string nameInsrt = "NewName";
                            string si = newId.ToString();
                            nameInsrt += si;

                            var rand = new Random();

                            cmd.CommandText = $"INSERT INTO {table} VALUES ({newId}, '{nameInsrt}','Pass12345','SysTest', {rand.Next(10, 1000)})";
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            CreateRating(connectionString, table, newId);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Error! Some information was wrong");
                    return;
                }
            }
        }
        public void RandomGameInsert(string connectionString, int n)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        string table = "OSGame";
                        cmd.CommandText = $"SELECT MAX(id) FROM {table}";
                        cmd.ExecuteNonQuery();
                        int newId = (int)cmd.ExecuteScalar();
                        cmd.Parameters.Clear();

                        for (int i = 0; i < n; i++)
                        {
                            newId += 1;

                            string nameInsrt = "NewGame";
                            string si = newId.ToString();
                            nameInsrt += si;

                            var rand = new Random();

                            cmd.CommandText = $"INSERT INTO {table} VALUES ({newId}, '{nameInsrt}','SysTest')";
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            CreateRating(connectionString, table, newId);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Error! Some information was wrong");
                    return;
                }
            }
        }
        public void CreateRating(string connectionString, string table, int someid)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        int vc = 0;
                        if (table == "OSUser")
                        {
                            cmd.CommandText = $"SELECT COUNT(*) FROM OSGame";
                            cmd.ExecuteNonQuery();
                            vc = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Parameters.Clear();
                        }
                        else if (table == "OSGame")
                        {
                            cmd.CommandText = $"SELECT COUNT(*) FROM OSUser";
                            cmd.ExecuteNonQuery();
                            vc = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Parameters.Clear();
                        }
                        int[] idarray = new int[vc];

                        if (table == "OSUser")
                        {
                            for (int i = 0; i < vc; i++)
                            {
                                cmd.CommandText = $"SELECT id FROM( SELECT id, ROW_NUMBER() OVER(ORDER BY id) AS RowNum FROM OSGame) AS rowsel WHERE rowsel.RowNum = {i + 1}";
                                idarray[i] = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Parameters.Clear();

                                cmd.CommandText = $"INSERT INTO OSRating VALUES ({idarray[i]}, {someid}, 0)";
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                        }
                        else if (table == "OSGame")
                        {
                            for (int i = 0; i < vc; i++)
                            {
                                cmd.CommandText = $"SELECT id FROM( SELECT id, ROW_NUMBER() OVER(ORDER BY id) AS RowNum FROM OSUser) AS rowsel WHERE rowsel.RowNum = {i + 1}";
                                idarray[i] = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Parameters.Clear();

                                cmd.CommandText = $"INSERT INTO OSRating VALUES ({someid}, {idarray[i]}, 0)";
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                        }
                        else {
                            Console.WriteLine("Unexpected error!");
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Error in CRFU! Some information was wrong");
                    return;
                }
            }
        }
        public void FuncClearTest(string connectionString)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string table = "OSUser";
                Start:
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = $"SELECT count(*) FROM {table} where stype = 'SysTest'";
                    int idcount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Parameters.Clear();

                    if (idcount == 0)
                    {
                        Console.WriteLine("There were 0 values");
                        return;
                    }
                    else
                    {
                        int[] idarray = new int[idcount];

                        for (int i = 0; i < idcount; i++)
                        {
                            cmd.CommandText = $"SELECT id FROM( SELECT id, ROW_NUMBER() OVER(ORDER BY id) AS RowNum FROM {table} where stype = 'SysTest') AS rowsel WHERE rowsel.RowNum = {i + 1}";
                            idarray[i] = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Parameters.Clear();
                            if (table == "OSUser")
                            {
                                cmd.CommandText = $"DELETE FROM OSRating WHERE user_id = {idarray[i]}";
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            else if (table == "OSGame")
                            {
                                cmd.CommandText = $"DELETE FROM OSRating WHERE game_id = {idarray[i]}";
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            else {
                                Console.WriteLine("Err");
                                return;
                            }
                        }
                        cmd.CommandText = $"DELETE FROM {table} where stype = 'SysTest'";
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }
                if (table == "OSUser")
                {
                    table = "OSGame";
                    goto Start;
                }
                else {
                    Console.WriteLine("Clear complete!");
                    return;
                }
            }
        }
        public void CreateAccount(string connectionString)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        OSConsole c = new OSConsole();
                        cmd.Connection = conn;
                        string table = "OSUser";
                        int i = 0;
                    CreateAccountlogin:
                        Console.WriteLine("Enter new login : ");
                        string login = Console.ReadLine();
                        try
                        {
                            FuncCheckLogin(table, login, connectionString);
                        }
                        catch
                        {
                            Console.WriteLine("Wrong Login!");
                            Console.WriteLine("1. Try Again");
                            Console.WriteLine("2. Cancel");
                            int choose = c.UserNumInput(1, 2);
                            switch (choose) { 
                                case 1 :
                                    Console.Clear();
                                    goto CreateAccountlogin;
                                case 2 :
                                    Console.Clear();
                                    return;            
                            }
                        }
                    CreateAccountpass:
                        Console.WriteLine("Enter password : ");
                        string pass = Console.ReadLine();
                        try
                        {
                            FuncCheckPass(table, login, pass, connectionString);
                        }
                        catch
                        {
                            Console.WriteLine("Wrong Password!");
                            Console.WriteLine("1. Try Again");
                            Console.WriteLine("2. Cancel");
                            int choose = c.UserNumInput(1, 2);
                            switch (choose)
                            {
                                case 1:
                                    Console.Clear();
                                    goto CreateAccountpass;
                                case 2:
                                    Console.Clear();
                                    return;
                            }
                        }
                        Console.WriteLine("Enter password again: ");
                        string passRepeat = Console.ReadLine();

                        if (pass == passRepeat)
                        {
                            try
                            {
                                int newId = FuncCreateID(table, connectionString);
                                
                                cmd.CommandText = $"INSERT INTO {table} VALUES ({newId}, '{login}','{pass}','Sys', 0)";
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();

                                CreateRating(connectionString, table, newId);
                            }
                            catch
                            {
                                Console.WriteLine("Unexpected error!");
                                if (i == 2)
                                {
                                    cmd.Parameters.Clear();
                                    return;
                                }
                                else
                                {
                                    i++;
                                    cmd.Parameters.Clear();
                                    Console.WriteLine($"Only {3 - i} tries left");
                                    goto CreateAccountlogin;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("!!! Error: your password was incorect");
                            goto CreateAccountpass;
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Something went wrong");
                    return;
                }
            }
        }
        public void FuncCheckLogin(string table, string login, string connectionString)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    int newId = FuncCreateID(table, connectionString);

                    cmd.CommandText = $"INSERT INTO {table} VALUES ({newId}, '{login}','TestPass123','SysTest', 0)";
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    FuncDeleteByID(table, newId, connectionString);
                }
            }
        }
        public void FuncCheckPass(string table, string login, string pass, string connectionString)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    int newId = FuncCreateID(table, connectionString);

                    cmd.CommandText = $"INSERT INTO {table} VALUES ({newId}, '{login}','{pass}','SysTest', 0)";
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    FuncDeleteByID(table, newId, connectionString);
                }
            }
        }
        public int FuncCreateID(string table, string connectionString) {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = $"SELECT MAX(id) FROM {table}";
                    int CreateNewIdValue = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Parameters.Clear();
                    CreateNewIdValue += 1;
                    return CreateNewIdValue;
                }
            }
        }
        public void FuncDeleteByID(string table, int i, string connectionString) {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    
                    cmd.CommandText = $"DELETE FROM {table} WHERE id={i};";
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                
            }
        }
        public void DeleteByIDWP(string connectionString) {
            OSConsole c = new OSConsole();
        DeleteByWPA:
            Console.WriteLine("Enter a table to work with :");
            string table = Console.ReadLine();
            Console.WriteLine($"You entered '{table}'");

            Console.WriteLine("Enter ID value");
            int max = FuncCreateID(table, connectionString);
            int idfordel = c.UserNumInput(0, max);
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        if (table == "OSUser")
                        {
                            cmd.CommandText = $"DELETE FROM OSRating WHERE user_id = {idfordel}";
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        else if (table == "OSGame")
                        {
                            cmd.CommandText = $"DELETE FROM OSRating WHERE game_id = {idfordel}";
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Err");
                            return;
                        }
                    }
                }
                FuncDeleteByID(table, idfordel, connectionString);
            }
            catch
            {
                Console.WriteLine("Wrong input!");
                Console.WriteLine("1. Try Again");
                Console.WriteLine("2. Cancel");
                int choose = c.UserNumInput(1, 2);
                switch (choose)
                {
                    case 1:
                        Console.Clear();
                        goto DeleteByWPA;
                    case 2:
                        Console.Clear();
                        return;
                }
            }
        }
    }
    class OSConsole
    {
        public int UserNumInput(int min, int max)
        {
            int num;
            string ui;
        UserNumInputActA:
            Console.WriteLine("Enter number :");
            ui = Console.ReadLine();
            try
            {
                num = Convert.ToInt32(ui);
                if (num < min)
                {
                    Console.WriteLine($"'Number should be bigger than {min}");
                    goto UserNumInputActA;
                }
                else if (num > max)
                {
                    Console.WriteLine($"'Number should be smaller than {max}");
                    goto UserNumInputActA;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine($"'{ui}' - is not a number");
                goto UserNumInputActA;
            }
            Console.WriteLine($"You entered '{num}'");
            return num;
        }
    }
}