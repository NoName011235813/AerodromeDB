/*
 *	Класс: UsersTable
 *	Класс предназначенный для взаимодействия с таблицей
 *	пользователей
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *  Задание:
 *		Описывает работу с таблицей пользователей в 
 *		базе данных.
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
 *		UsersTable - конструктор класса;
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

	class UsersTable : IDbTable {

		public MySqlConnection Connection { get; set; }
		public MySqlDataAdapter Adapter { get; set; } 
		public DataSet Data { get; set; }
				
		public QueryColumns Columns { get; set; }	// Определение столбцов

		string tableName = "users";
		public string TableName { get { return tableName; } }

		/*
		 *	Конструктор класса
		 *	
		 *	Формальный парметр:
		 *		Conn - ссылка на соединение в рамках которого
		 *		будет выполняться работа с БД.
		 */
		public UsersTable(MySqlConnection Conn) {

			Columns = new QueryColumns();

													// Имена столбцов в таблице
			Columns.Add("id", "Ид", MySqlDbType.Int32, true, 0);
			Columns.Add("status_id", "Ид_статуса", MySqlDbType.Int32, false, 0);
			Columns.Add("ulogin", "Логин", MySqlDbType.VarChar, false, 30);
			Columns.Add("usalt", "Соль", MySqlDbType.VarChar, false, 6);
			Columns.Add("upassword", "Пароль", MySqlDbType.VarChar, false, 32);
						
			Connection = Conn;

													// Составление запроса на
													// выборку
			Adapter = new MySqlDataAdapter(QueryUtils.CreateSelectQ(Columns, tableName));
			Adapter.SelectCommand.Connection = Connection;

			Data = new DataSet();
													// Заполнение кэша
			Fill();
					
													// Составление DML запросов
			Adapter.InsertCommand = QueryUtils.CreateInsertQ(Columns, tableName);
			Adapter.InsertCommand.Connection = Connection;
									
			Adapter.UpdateCommand = QueryUtils.CreateUpdateQ(Columns, tableName);
			Adapter.UpdateCommand.Connection = Connection;
				
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
		 *		tempAdapter - адаптер, используемый для проверки уникальности;
		 *		fkAdapter - адаптер, используемый для проверки
		 *		внешних ключей;
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
					"select id from statuses where id = @pstatus_id",
					Connection
				);

				fkAdapter.SelectCommand.Parameters.Add("pstatus_id", MySqlDbType.Int32);

													// Определение и создание 
													// объектов, необходимых для
													// проверки уникальности
				MySqlDataAdapter tempAdapter = new MySqlDataAdapter(
					"select id from users where ulogin = @plogin",
					Connection
				);
				tempAdapter.SelectCommand.Parameters.Add("plogin", MySqlDbType.VarChar);
				
				DataTable fkSet = new DataTable();
				
				foreach (DataRow Row in temp.Tables[0].Rows) {

													// Проверка на пустые поля
					if (
						Row["Ид_статуса"] == DBNull.Value ||
						Row["Логин"] == DBNull.Value ||
						Row["Соль"] == DBNull.Value ||
						Row["Пароль"] == DBNull.Value
					) {
						MessageBox.Show("Вы оставили пустые поля, сохранение отменено");
						return false;
					}

													// Проверка значений
					if (
						!(
							CheckUtil.CheckStringLength(Row, "Логин", 30) &&
							CheckUtil.CheckStringLength(Row, "Соль", 6) &&
							CheckUtil.CheckStringLength(Row, "Пароль", 32) &&
							CheckUtil.CheckIntAndLength(Row, "Ид_статуса", 1, 3)
						)
					)
						return false;

													// Проверка на уникальность
					if (!CheckUtil.CheckUniqueValue(tempAdapter, "plogin", Data, Row, "Логин")) {
						Connection.Close();
						MessageBox.Show("Логин должен быть уникальным");
						return false;
					}

													// Проверка внешних ключей
					fkAdapter.SelectCommand.Parameters["pstatus_id"].Value = Row["Ид_статуса"];

					fkSet.Rows.Clear();
					fkSet.Columns.Clear();

					if (fkAdapter.Fill(fkSet) == 0) {
						Connection.Close();
						MessageBox.Show("Статус не найден");
						return false;
					}
										
				}

				Connection.Close();

			}
													// Проверка внешних ключей
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

													// Выполнение запроса 
													// выборки
		public void Fill() {
			Data.Clear();
			try {
				Adapter.Fill(Data);
			} catch {
				MessageBox.Show("Произошла ошибка при заполнении таблицы");
			}
			Connection.Close();

													// Настройки столбцов
			Data.Tables[0].Columns["Ид"].ReadOnly = true;
			// Data.Tables[0].Columns["Соль"].ReadOnly = true;
			// Data.Tables[0].Columns["Пароль"].ReadOnly = true;
		}

	}

}
