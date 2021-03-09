/*
 *	Форма: fPlanesStatus
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *	Задание: 
 *		Выводит состояние самолётов относительно проведения
 *		периодических технических обслуживаний.
 * 
 *		
 *	Переменные, используемые в данной форме:
 *		Connection - соединение с базой данных;
 *		BSource - посредник между источником данных и сеткой;
 *		Result - источник данных, где будет храниться информация из
 *		базы данных.
 * 
 *	Подпрограммы, используемые в данной форме:
 *		fPlanesStatus - конструктор класса;
 *			
 */

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

namespace BaseConnection {

	public partial class fPlanesStatus : Form {

		MySqlConnection Connection;					// Соединение
		BindingSource BSource = new BindingSource();// Связующее звено
		DataTable Result;							// Таблица

		/*
		 *	Конструктор класса
		 *	
		 *	Формальный параметр:
		 *		Conn - ссылка на соединение с БД.
		 *	
		 *	Локальные переменные:
		 *		Adapter - объект для работы с запросами;
		 *		Planes - таблица с данными о самолётах;
		 *		MaintTypes - таблица с данными о типах ТО;
		 *		Col - временная переменная для создания столбца
		 *		с регистрационными номерами самолётов;
		 *		Row - временная переменная, используемая в цикле 
		 *		foreach, что содержит строку таблицы;
		 *		temp - временная переменная для создания столбцов
		 *		типов ТО;
		 *		result - переменная, что хранит разность между датами
		 *		возвращаемая сервером;
		 *		i - счётчик;
		 *		NewCellValue - переменная для хранения будущего содержимого
		 *		ячейки;
		 *		Plane - временная переменная, используемая в цикле
		 *		foreach, что содержит строку таблицы самолётов;
		 *		MType - временная переменная, используемая в цикле
		 *		foreach, что содержит строку таблицы типов ТО;
		 *		Column - временная переменная, используемая в цикле
		 *		foreach, что содержит столбец сетки вывода.
		 */
		public fPlanesStatus(MySqlConnection Conn) {
			
			InitializeComponent();
													
			Connection = Conn;						// Получение соединения

			Result = new DataTable();

													// Информация о самолётах
			MySqlDataAdapter Adapter = new MySqlDataAdapter(
				"select id, registration_number from plane", 
				Connection
			);
			DataSet Planes = new DataSet();

			Adapter.Fill(Planes);
			Connection.Close();

													// Информация о типах ТО
			Adapter.SelectCommand.CommandText = "select * from maint_types";
			DataSet MaintTypes = new DataSet();

			Adapter.Fill(MaintTypes);
			Connection.Close();



													// Определение столбцов	
													// новой таблицы
			DataColumn Col = new DataColumn();		// Создание столбца самолётов
			Col.Caption = "Самолёты";
			Col.ColumnName = "Planes";

			Result.Columns.Add(Col);

													// Создание столбцов с
													// типами ТО
			foreach (DataRow Row in MaintTypes.Tables[0].Rows) {

				DataColumn temp = new DataColumn();
				temp.ColumnName = Row["name"].ToString();

				Result.Columns.Add(temp);

			}

													// Запрос на разницу 
													// между датами в месяцах
			Adapter.SelectCommand.CommandText = 
				"select timestampdiff(month, max(wh.work_date), curdate()) " + 
				"from works_history wh, plane p " +
				"where plane_id = " + 
					"(select id from plane where registration_number = @preg_num)" + 
				"and workload_id in " + 
					"(select id from workload where maint_type_id = @ptype_id)";

			Adapter.SelectCommand.Parameters.Add("preg_num", MySqlDbType.VarChar);
			Adapter.SelectCommand.Parameters.Add("ptype_id", MySqlDbType.Int32);


			string result;

			int i;
			string NewCellValue;

			Connection.Open();
													// Вычисление значений 
													// ячеек
			foreach (DataRow Plane in Planes.Tables[0].Rows) {
													
				Adapter.SelectCommand.Parameters["preg_num"].Value = Plane["registration_number"];

				Result.Rows.Add(Result.NewRow());	// Добавление новой строки 
													// к таблице
													
													// Запись рег. номера самолёта
				Result.Rows[Result.Rows.Count - 1]["Planes"] = Plane["registration_number"];
				
				i = 1;
													// Для каждого столбца
													// (типа ТО)
				foreach (DataRow MType in MaintTypes.Tables[0].Rows) {

					Adapter.SelectCommand.Parameters["ptype_id"].Value = MType["id"];

					// NewCellValue = "";
													// Получение значения 
													// разницы между датами
					result = Adapter.SelectCommand.ExecuteScalar().ToString();
					
													// Определение будущего
													// содержимого ячейки
					if (result == "")
						NewCellValue = "Необходимо, не проведены ранее";
				
					else if (int.Parse(result) >= int.Parse(MType["period"].ToString()))
						NewCellValue = "Необходимо, разница: " + result.ToString() + " мес.";

					else
						NewCellValue = "Проведена, разница: " + result.ToString() + " мес.";


													// Запись содержимого в
													// таблицу
					Result.Rows[Result.Rows.Count - 1][i] = NewCellValue;

					i++;
					
				}
				
			}

			Connection.Close();
													// Передача таблицы
													// для отображения в сетку
			BSource.DataSource = Result;
			StatusGrid.DataSource = BSource;

													// Переназначение названий 
													// столбцов сетки
			foreach (DataGridViewColumn Column in StatusGrid.Columns)
				Column.HeaderText = Result.Columns[Column.HeaderText].Caption;

		}

	}

}
