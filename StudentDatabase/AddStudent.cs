using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace StudentDatabase
{
    public partial class AddStudent : Form
    {
        private MySqlConnection connection;
        private MySqlCommand command;
        private MySqlDataReader reader;
        protected ViewStudentData VSD;
        public AddStudent()
        {

            InitializeComponent();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            string Query = "INSERT INTO student.studentdetails(FirstName,LastName,Branch,RollNumber,Email,MobileNumber)";
             Query+= "VALUES(@First_Name,@Last_Name,@Branch,@Roll_Number,@Email,@Mobile_Number)";
           // Query += "VALUES('Sandeep','Kataria','ME','2014-073','skataria@gmail.com','9876598765')";
            connection = new MySqlConnection();
            connection.ConnectionString = @"datasource=127.0.0.1;port=3306;username=root;password=;";
            command = new MySqlCommand(Query, connection);
            
            command.Parameters.AddWithValue("@First_Name", First_Name.Text);
            command.Parameters.AddWithValue("@Last_Name", Last_Name.Text);
            command.Parameters.AddWithValue("@Branch", Branch.Text);
            command.Parameters.AddWithValue("@Roll_Number", Roll_Number.Text);
            command.Parameters.AddWithValue("@Email", Email.Text);
            command.Parameters.AddWithValue("@Mobile_Number", Mobile_Number.Text);
            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                MessageBox.Show("Student Added Successfully");
                //Reset textboxes
                First_Name.Text = string.Empty;
                Last_Name.Text = string.Empty;
                Branch.Text = string.Empty;
                Roll_Number.Text = string.Empty;
                Mobile_Number.Text = string.Empty;
                Email.Text = string.Empty;

                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("The Student could not be added because "+ex.Message);
            }
        }
    }
}
