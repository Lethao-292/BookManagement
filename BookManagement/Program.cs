using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement
{
    class Program
    {
        public static void ShowCommand()
        {
            Console.WriteLine("****** Book Management ******");
            Console.WriteLine("1. Add a book");
            Console.WriteLine("2. Find a book");
            Console.WriteLine("3. Show book list");
            Console.WriteLine("4. Remove a book");
            Console.WriteLine("5. Exit");
            Console.Write("Choose a command number(1-5): ");
        }

        public static int InputInteger(String Name)
        {
            int value = 0;
            do
            {
                Console.Write("Enter {0}: ", Name);
                try
                {
                    value = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("{0} must be integer. Please input again.", Name);
                    value = 0;
                }
            } while (value <= 0);
            return value;
        }

        public static String InputString(String Name)
        {
            String str = "";
            do
            {
                Console.Write("Enter {0}: ", Name);
                str = Console.ReadLine();
                if (str.Length <= 0)
                {
                    Console.WriteLine("{0} can't be blank. Please input again", Name);
                }
                else if (str.Length > 100)
                {
                    Console.WriteLine("{0} can't be over 100 character. Please input again", Name);
                }
            } while (str.Length <= 0 || str.Length > 100);
            return str;
        }
        
        public static void AddBook(SqlConnection cn)
        {
            #region Gathering Information
            Console.WriteLine("Add Book Method ");
            int BookID = InputInteger("BookID");
            String BookName = InputString("BookName");
            int Price = InputInteger("Price");
            int Quantity = InputInteger("Quantity");
            String Author = InputString("Author");
            String Publisher = InputString("Publisher");
            #endregion
            #region Format and Execute SqlCommand
            String sqlStr = string.Format("INSERT INTO Books VALUES (@ID, @Name, @Price, @Quantity, @Author, @Publisher)");
            SqlCommand cmd = new SqlCommand(sqlStr, cn);
            cmd.Parameters.AddWithValue("@ID", BookID);
            cmd.Parameters.AddWithValue("@Name", BookName);
            cmd.Parameters.AddWithValue("@Price", Price);
            cmd.Parameters.AddWithValue("@Quantity", Quantity);
            cmd.Parameters.AddWithValue("@Author", Author);
            cmd.Parameters.AddWithValue("@Publisher", Publisher);
            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Add Successfully.");
            }
            catch
            {
                Console.WriteLine("Your Book have existed");
            }
            #endregion
        }

        public static void FindBook(SqlConnection cn)
        {
            #region Gather Information
            int BookID = 0;
            BookID = InputInteger("BookID to find");
            #endregion
            #region Format and Exectute SqlCommand
            String sqlStr = string.Format("SELECT * FROM Books WHERE BookID = {0}", BookID);
            SqlCommand cmd = new SqlCommand(sqlStr, cn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Console.WriteLine("-------Your book information--------");
                for(int i = 0; i < dr.FieldCount; i++)
                {
                    Console.WriteLine("{0} : {1}", dr.GetName(i).Trim(), dr.GetValue(i).ToString().Trim());
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Your BookID is not exist.\n");
            }
            #endregion
        }

        public static void ShowBooks(SqlConnection cn)
        {
            String strSql = "SELECT * FROM Books";
            SqlCommand cmd = new SqlCommand(strSql, cn);

            SqlDataReader dr = cmd.ExecuteReader();
            int count = 0;
            while (dr.Read())
            {
                count++;
                Console.WriteLine("-------------Book {0}------------",count);
                for(int i = 0; i < dr.FieldCount; i++)
                {
                    Console.WriteLine("{0} : {1}", dr.GetName(i).Trim(), dr.GetValue(i).ToString().Trim());
                }
                Console.WriteLine();
            }
            dr.Close();
        }

        public static void RemoveBook(SqlConnection cn)
        {
            #region Gather Information
            int BookID = 0;
            BookID = InputInteger("BookID to remove");
            #endregion
            #region Format and Exectute SqlCommand
            String sqlStr = string.Format("DELETE FROM Books WHERE BookID = {0}", BookID);
            SqlCommand cmd = new SqlCommand(sqlStr, cn);
            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Delete Successfully.");
            }
            catch
            {
                Console.WriteLine("Your BookID does not exist");
            }
            #endregion
        }
        static void Main(string[] args)
        {
            int Command;
            bool UserDone = false;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = "uid=sa;pwd=123456;Initial Catalog=Book;"
                                + @"Data Source=localhost\SQLEXPRESS;Connect Timeout=30";
            cn.Open();
            do
            {
                #region Receive Command                
                do
                {
                    try
                    {
                        ShowCommand();
                        Command = int.Parse(Console.ReadLine());
                        Console.WriteLine();
                        if (Command < 1 || Command > 5)
                        {
                            Console.WriteLine("Please input from 1 to 5.");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Input must be number. Please choose again.");
                        Command = 0;
                    }
                } while (Command < 1 || Command > 5);
                #endregion
                #region Execute Command
                switch (Command)
                {
                    case 1:
                        AddBook(cn);
                        break;
                    case 2:
                        FindBook(cn);
                        break;
                    case 3:
                        ShowBooks(cn);
                        break;
                    case 4:
                        RemoveBook(cn);
                        break;
                    case 5:
                        Console.WriteLine("Good bye. See you later.");
                        UserDone = true;
                        break;
                }
                #endregion
            } while (!UserDone);
            cn.Close();
            Console.ReadLine();
        }
    }
}
