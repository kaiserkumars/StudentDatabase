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

    


    public partial class ViewStudentData : Form
    {

        public class Student
        {
            public string fname { get; set; }
            public string lname { get; set; }
            public string branch { get; set; }
            public string roll { get; set; }
            public string email { get; set; }
            public string mobile { get; set; }

        }


        AddStudent studentAdd = new AddStudent();
        private MySqlConnection connection;
        private MySqlCommand command;
        private MySqlDataReader reader;
        int Index = 0;

        List<Student> FormData { get; set; }
        Int32 record_counter;
        public ViewStudentData()
        {
            
            InitializeComponent();
            next_button.Visible = false; //it will be visible if no of records>1 checked in recordCount()
            back_button.Visible = false; //when showing first record we cannot go backward therefore kept it hidden
            apply_button.Visible = false;
            recordCount(); //checking the number of records         
            DataRetrieve();  //calling the data retrieval method                      
                
           
        }

        public void DataRetrieve()  //to retrieve data to list
        {
            
           
            
            List<Student> dbData = new List<Student>();
            /*** Here we have used List to store all the data of database at once, 
                 so that we don’t have to call the database again and again. ***/
            string Query = "SELECT * FROM studentdetails WHERE 1";
            //connection = new MySqlConnection();
            //connection.ConnectionString = @"datasource=localhost;port=3306;username=root;password=;database=student;";
            // command = new MySqlCommand(Query, connection);
            //connection.Open();
            makeConnection(Query);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Student newStudent = new Student();
                newStudent.fname = reader["FirstName"].ToString();
                newStudent.lname = reader["LastName"].ToString();
                newStudent.branch = reader["Branch"].ToString();
                newStudent.roll = reader["RollNumber"].ToString();
                newStudent.email = reader["Email"].ToString();
                newStudent.mobile = reader["MobileNumber"].ToString();
                dbData.Add(newStudent);

                /**
                Here "FirstName", "LastName"..... are columns names of table studentdetails **/

            }

            connection.Close();
            this.FormData = dbData;
            BindData(Index);
        }

        private void BindData(int Index)                //To bind the form Labels to the data from List
        {
            firstname.Text = FormData[Index].fname;
            lastname.Text = FormData[Index].lname;
            branch.Text = FormData[Index].branch;
            rollnumber.Text = FormData[Index].roll;
            email.Text = FormData[Index].email;
            mobnumber.Text = FormData[Index].mobile;
        }

       


        private void recordCount()   //function to keep count of number of records
        {
            string Query = "SELECT COUNT(*) FROM studentdetails";
            try
            {
                //connection = new MySqlConnection();
                //connection.ConnectionString = @"datasource=localhost;port=3306;username=root;password=;database=student;";
                makeConnection(Query);
                record_counter = Convert.ToInt32(command.ExecuteScalar()); //to count number of records
                if (record_counter > 1)
                    next_button.Visible = true;
                record_count.Text = record_counter.ToString();
                connection.Close();
                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void makeConnection(string Query) //function to make connection to database
        {
            try
            {
                connection = new MySqlConnection();
                connection.ConnectionString = @"datasource=localhost;port=3306;username=root;password=;database=student;";
                command = new MySqlCommand(Query, connection);
                connection.Open();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }


        private void writeTrue()            //makes textbox fields writable
        {
            apply_button.Visible = true;
            firstname.ReadOnly = false;
            lastname.ReadOnly = false;
            branch.ReadOnly = false;
            rollnumber.ReadOnly = false;
            email.ReadOnly = false;
            mobnumber.ReadOnly = false;
        }

        private void writeFalse()         //makes textbox fields only readable
        {
            firstname.ReadOnly = true;
            lastname.ReadOnly = true;
            branch.ReadOnly = true;
            rollnumber.ReadOnly = true;
            email.ReadOnly = true;
            mobnumber.ReadOnly = true;
        }


        //BUTTON SECTION STARTS//  //BUTTON SECTION STARTS//  //BUTTON SECTION STARTS//  //BUTTON SECTION STARTS// 
        //BUTTON SECTION STARTS// //BUTTON SECTION STARTS// //BUTTON SECTION STARTS// //BUTTON SECTION STARTS//


        private void button2_Click(object sender, EventArgs e)  //ADD 
        {
            studentAdd.Show();
          
        }

        private void button3_Click(object sender, EventArgs e)  //DELETE 
        {
            try
            {
                connection = new MySqlConnection();
                connection.ConnectionString = @"datasource=localhost;port=3306;username=root;password=;database=student;";
                string Query = "DELETE FROM studentdetails WHERE RollNumber='"+this.rollnumber.Text+"'; ";
                command = new MySqlCommand(Query, connection);
                connection.Open();
                reader = command.ExecuteReader();    //reader is important
                MessageBox.Show("Record Deleted!");
                recordCount();
                DataRetrieve();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /** Below is the code for Next Button.
         * Its calls BindData() with incremented value of Index,
         * whenever Next button is clicked.
        **/
        private void button5_Click(object sender, EventArgs e)  //NEXT
        {
            back_button.Visible = true; //after first click on next button previous button is set visible
            if(Index<FormData.Count-1)
            {
                
                Index++;
              //  MessageBox.Show(Index.ToString() + " " + FormData.Count.ToString());
                BindData(Index);
            }

            if(Index==FormData.Count-1)
            {
                next_button.Visible = false; //next button hidden after no more records available
            }
        }

        private void button4_Click(object sender, EventArgs e)  //PREVIOUS
        {
            if(Index>0)
            {
                next_button.Visible = true;
                Index--;
                BindData(Index);
            }

            if(Index==0)
            {
                
                back_button.Visible = false;  //previous button hidden when Index=0
            }
        }

        private void button1_Click(object sender, EventArgs e)   //UPDATE
        {
            writeTrue();

        }

        private void refresh_button_Click(object sender, EventArgs e)  //REFRESH
        {
            DataRetrieve();
            recordCount();
        }

        private void apply_button_Click(object sender, EventArgs e) //APPLY
        {
            /**
             * FIX requied here, make updation of values just like insertion to prevent SQL injection. **/
            string Query = "UPDATE studentdetails SET FirstName='" + firstname.Text + "',LastName='"
                + lastname.Text + "',Branch='" + branch.Text + "',RollNumber='" + rollnumber.Text + 
                "',Email='" + email.Text + "',MobileNumber='" + mobnumber.Text + "'WHERE RollNumber='" + this.rollnumber.Text+"' ;";
            try
            {
                makeConnection(Query);
                reader = command.ExecuteReader();
                MessageBox.Show("Updation Successfull!");
                DataRetrieve();
                connection.Close();
               
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
