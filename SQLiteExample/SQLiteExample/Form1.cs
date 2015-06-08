using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace SQLiteExample
{
    public partial class Form1 : Form
    {
        private String connectionString;
        private SQLiteConnection connection;

        private String SQLInsert = "INSERT INTO User(Name, Surname) VALUES(?, ?)";
        private String SQLUpdate = "UPDATE User SET Name = ?, Surname = ? where UserId = ?";
        private String SQLSelect = "SELECT * FROM User";
        private String SQLDelete = "DELETE FROM User WHERE UserId = ?";

        public Form1()
        {
            InitializeComponent();
			SQLiteConnection.CreateFile("MyDatabase.sqlite");
            connection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
			connection.Open();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();

            Application.Exit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtId.Text))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = SQLInsert;

                command.Parameters.AddWithValue("Name", txtName.Text);
                command.Parameters.AddWithValue("Surname", txtSurname.Text);

                command.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = SQLUpdate;

                command.Parameters.AddWithValue("Name", txtName.Text);
                command.Parameters.AddWithValue("Surname", txtSurname.Text);
                command.Parameters.AddWithValue("UserId", int.Parse(txtId.Text));

                command.ExecuteNonQuery();
                connection.Close();

            }

            clean();
            search();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            search();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtId.Text))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = SQLDelete;

                command.Parameters.AddWithValue("UserId", int.Parse(txtId.Text));

                command.ExecuteNonQuery();
                connection.Close();

                clean();
                search();
            }
        }

        private void clean()
        {
            txtId.Text = String.Empty;
            txtName.Text = String.Empty;
            txtSurname.Text = String.Empty;
        }

        private void search()
        {
            // Eliminamos el handler del evento RowEnter para evitar que se dispare al
            // realizar la búsqueda
            dataGrid.RowEnter -= dataGrid_RowEnter;

            // Abrimos la conexión
            if (connection.State != ConnectionState.Open)
                connection.Open();

            // Creamos un SQLiteCommand y le asignamos la cadena de consulta
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = SQLSelect;

            // Creamos un nuevo DataTable y un DataAdapter a partir de la SELECT.
            // A continuación, rellenamos la tabla con el DataAdapter
            DataTable dt = new DataTable();
            SQLiteDataAdapter da = new SQLiteDataAdapter(command);
            da.Fill(dt);

            // Asignamos el DataTable al DataGrid y cerramos la conexión
            dataGrid.DataSource = dt;
            connection.Close();

            // Restauramos el handler del evento
            dataGrid.RowEnter += dataGrid_RowEnter;
        }

        private void dataGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Recuperamos ID, nombre y apellido de la fila
            int id = int.Parse(dataGrid.Rows[e.RowIndex].Cells[0].Value.ToString());
            String name = (String)dataGrid.Rows[e.RowIndex].Cells[1].Value;
            String surname = (String)dataGrid.Rows[e.RowIndex].Cells[2].Value;

            // Asignamos los valores a las cajas de texto
            txtId.Text = id.ToString();
            txtName.Text = name;
            txtSurname.Text = surname;
        }
    }
}
