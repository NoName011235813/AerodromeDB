/*
 *	Класс: PlanesTable
 *	Класс предназначенный для взаимодействия с таблицей самолётов
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *  Задание:
 *		Описывает работу с таблицей самолётов в базе данных.
 *		
 *  Реализует интерфейс IDbTable.
 *		
 *	Переменные, описанные в данном классе:
 *		Connection - соединение с базой данных;
 *		Adapter - взаимодействие с таблицей в базе данных;
 *		Data - источник данных, куда будет записываться информация
 *		из базы данных;
 *		Columns - описание столбцов таблицы;
 *		TableName - название таблицы в базе данных.
 *		
 *	Подпрограммы, описанные в данном классе:
 *		PlanesTable - конструктор класса;
 *		Save - функция сохранения изменений в таблице, возвращает
 *		true, если сохранение прошло успешно, false в противоположном случае;
 *		Cancel - процедура отмены несохраненных изменений;
 *		AddRow - процедура добавления строки к таблице;
 *		Fill - процедура отправки запроса на выборку данных.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace BaseConnection {

	class PlanesTable : IDbTable {

		public MySqlConnection Connection { get; set; }
		public MySqlDataAdapter Adapter { get; set; } 
		public DataSet Data { get; set; }

													
		public QueryColumns Columns { get; set; }	// Определение столбцов

		string tableName = "plane";

		public string TableName { get { return tableName; } }

		/*
		 *	Конструктор класса
		 *	
		 *	Формальный парметр:
		 *		Conn - ссылка на соединение в рамках которого
		 *		будет выполняться работа с БД.
		 */
		public PlanesTable(MySqlConnection Conn) {

			Columns = new QueryColumns();

													// Имена столбцов в таблице
			Columns.Add("id", "Ид", MySqlDbType.Int32, true, 0);
			
			Columns.Add("(select pl.manufacturer from plane_model pl where pl.id = p.model_id)", "Производитель", MySqlDbType.VarChar, false, 70);
			Columns.Add("(select pl.name from plane_model pl where pl.id = p.model_id)", "Модель", MySqlDbType.VarChar, false, 100);
			
			Columns.Add("registration_number", "Регистрационный_номер", MySqlDbType.VarChar, false, 10);
			
			Columns.Add("parking_place_id", "Стоянка", MySqlDbType.VarChar, false, 15);

						
			Connection = Conn;

													// Составление запроса на
													// выборку
			Adapter = new MySqlDataAdapter(QueryUtils.CreateSelectQ(Columns, tableName + " p"));
			Adapter.SelectCommand.Connection = Connection;
													
			Data = new DataSet();
													// Заполнение кэша
			Fill();

													// Составление DML запросов
			Adapter.InsertCommand = Connection.CreateCommand();
			Adapter.InsertCommand.CommandText = 
				"insert into " + tableName + 
				"(registration_number, parking_place_id, model_id)" +
				"values(@pregistration_number, @pparking_place_id, " + 
				"(select id from plane_model where manufacturer = @pmanufacturer and name = @pname))";


			Adapter.InsertCommand.Parameters.Add("pregistration_number", MySqlDbType.VarChar, 10, "Регистрационный_номер");
			Adapter.InsertCommand.Parameters.Add("pparking_place_id", MySqlDbType.VarChar, 15, "Стоянка");
			Adapter.InsertCommand.Parameters.Add("pmanufacturer", MySqlDbType.VarChar, 70, "Производитель");
			Adapter.InsertCommand.Parameters.Add("pname", MySqlDbType.VarChar, 100, "Модель");


			Adapter.UpdateCommand = Connection.CreateCommand();
			Adapter.UpdateCommand.CommandText =
				"update " + tableName + 
				" set registration_number = @pregistration_number, parking_place_id = @pparking_place_id, " +
				" model_id = (select id from plane_model where manufacturer = @pmanufacturer and name = @pname)" +
				" where id = @pid";

			Adapter.UpdateCommand.Parameters.Add("pid", MySqlDbType.Int32, 0, "Ид");
			Adapter.UpdateCommand.Parameters.Add("pregistration_number", MySqlDbType.VarChar, 10, "Регистрационный_номер");
			Adapter.UpdateCommand.Parameters.Add("pparking_place_id", MySqlDbType.VarChar, 15, "Стоянка");
			Adapter.UpdateCommand.Parameters.Add("pmanufacturer", MySqlDbType.VarChar, 70, "Производитель");
			Adapter.UpdateCommand.Parameters.Add("pname", MySqlDbType.VarChar, 100, "Модель");


			Adapter.DeleteCommand = QueryUtils.CreateDeleteQ(Columns, tableName);
			Adapter.DeleteCommand.Connection = Connection;
			
		}
		
		/*
		 *	Функция сохранения внесённых в таблицу изменений
		 *	
		 *	Локальные переменные:
		 *		temp - временный источник данных, что хранит строки, что были
		 *		добавлены или изменены;
		 *		Row - временная переменная, используемая в цикле foreach,
		 *		хранит строки из temp;
		 *		fkAdapter - адаптер, используемый для проверки
		 *		внешних ключей;
		 *		uniqueAdapter - адаптер, используемый для проверки
		 *		уникальности столбцов;
		 *		fkSet - источник данных, используемый для проверки
		 *		внешних ключей.
		 */
		public bool Save() { 

													// Внесённые изменения
			DataSet temp = Data.GetChanges(DataRowState.Added | DataRowState.Modified);
			
			if (temp != null) {
													// Определение и создание 
													// объектов, необходимых для
													// проверки внешних ключей
				MySqlDataAdapter fkAdapter = new MySqlDataAdapter(
					"select id from plane_model where manufacturer = @pmanufacturer and name = @pname",
					Connection
				);

				fkAdapter.SelectCommand.Parameters.Add("pmanufacturer", MySqlDbType.VarChar);
				fkAdapter.SelectCommand.Parameters.Add("pname", MySqlDbType.VarChar);

													// Определение и создание 
													// объектов, необходимых для
													// проверки уникальности
				MySqlDataAdapter uniqueAdapter = new MySqlDataAdapter();
				uniqueAdapter.SelectCommand = Connection.CreateCommand();

				uniqueAdapter.SelectCommand.Parameters.Add("pparking_place_id", MySqlDbType.VarChar);
				uniqueAdapter.SelectCommand.Parameters.Add("pregistration_number", MySqlDbType.VarChar);

				DataTable fkSet = new DataTable();

				foreach (DataRow Row in temp.Tables[0].Rows) {

													// Проверка на пустые поля
					if (
						Row["Регистрационный_номер"] == DBNull.Value || 
						Row["Производитель"] == DBNull.Value || 
						Row["Модель"] == DBNull.Value ||
						Row["Стоянка"] == DBNull.Value
					) {
						MessageBox.Show("Вы оставили пустые поля, сохранение отменено");
						return false;
					}

													// Проверка значений
					if (
						!(
							CheckUtil.CheckStringLength(Row, "Регистрационный_номер", 10) &&
							CheckUtil.CheckStringLength(Row, "Производитель", 70) &&
							CheckUtil.CheckStringLength(Row, "Модель", 100) &&
							CheckUtil.CheckStringLength(Row, "Стоянка", 15)
						)
					)
						return false;

					
													// Проверка на уникальность
													// столбца "Стоянка"
					uniqueAdapter.SelectCommand.CommandText = "select id from plane where parking_place_id = @pparking_place_id";

					if (!CheckUtil.CheckUniqueValue(uniqueAdapter, "pparking_place_id", Data, Row, "Стоянка")) {
						Connection.Close();
						MessageBox.Show("Стоянка должна быть уникальной");
						return false;
					}
					
					uniqueAdapter.SelectCommand.CommandText = "select id from plane where registration_number = @pregistration_number ";
					
													// Проверка на уникальность
													// столбца "Регистрационный номер"
					if (!CheckUtil.CheckUniqueValue(uniqueAdapter, "pregistration_number", Data, Row, "Регистрационный_номер")) {
						Connection.Close();
						MessageBox.Show("Регистрационный номер должен быть уникальным");
						return false;
					}

													// Проверка внешнего ключа
					fkAdapter.SelectCommand.Parameters["pmanufacturer"].Value = Row["Производитель"];
					fkAdapter.SelectCommand.Parameters["pname"].Value = Row["Модель"];

					fkSet.Rows.Clear();
					fkSet.Columns.Clear();


					if (fkAdapter.Fill(fkSet) == 0) {
						Connection.Close();
						MessageBox.Show("Модель самолёта не найдена");
						return false;
					}

				}

				Connection.Close();
					
			}

													// Отправка изменений
			try {
				Adapter.Update(Data.Tables[0]);
				return true;
			} catch {
				MessageBox.Show("Произошла непредвиденная ошибка при сохранении");
				return false;
			}
		}

													// Отмена изменений
		public void Cancel() { 
			Data.Tables[0].RejectChanges();
		}

													// Добавление строки
		public void AddRow() { 
			try {
				Data.Tables[0].Rows.Add(Data.Tables[0].NewRow());
			} catch {
				MessageBox.Show("Произошла ошибка при добавлении строки в таблицу");
			}
		}

													// Выполнение запроса выборки
		public void Fill() {
			Data.Clear();
			try {
				Adapter.Fill(Data);
				Connection.Close();
			} catch {
				Connection.Close();
				MessageBox.Show("Произошла ошибка при заполнении таблицы");
				return;
			}

													// Настройки столбцов
			Data.Tables[0].Columns["Ид"].ReadOnly = true;
		}

	}

}
