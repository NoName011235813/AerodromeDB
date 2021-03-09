/*
 *	Класс: MaintTypesTable
 *	Класс предназначенный для взаимодействия с таблицей
 *	типов технического обслуживания
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *  Задание:
 *		Описывает работу с таблицей типов технического обслуживания в 
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
 *		MaintTypesTable - конструктор класса;
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

	class MaintTypesTable: IDbTable {

		public MySqlConnection Connection { get; set; }
		public MySqlDataAdapter Adapter { get; set; } 
		public DataSet Data { get; set; }

		public QueryColumns Columns { get; set; }	// Определение столбцов

		string tableName = "maint_types";
		public string TableName { get { return tableName; } }

		/*
		 *	Конструктор класса
		 *	
		 *	Формальный парметр:
		 *		Conn - ссылка на соединение в рамках которого
		 *		будет выполняться работа с БД.
		 */
		public MaintTypesTable(MySqlConnection Conn) {

			Columns = new QueryColumns();

													// Имена столбцов в таблице
			Columns.Add("id", "Ид", MySqlDbType.Int32, true,  0);
			Columns.Add("name", "Название", MySqlDbType.VarChar, false,  20);
			Columns.Add("period", "Периодичность_мес", MySqlDbType.Int32, false,  0);
						
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
		 *		tempAdapter - адаптер, используемый для проверки уникальности.
		 */
		public bool Save() { 

													// Внесённые изменения
			DataSet temp = Data.GetChanges(DataRowState.Added | DataRowState.Modified);
			
			if (temp != null) {
													// Определение и создание 
													// объектов, необходимых для
													// проверки уникальности
				MySqlDataAdapter tempAdapter = new MySqlDataAdapter(
					"select id from maint_types where name = @pname",
					Connection
				);

				tempAdapter.SelectCommand.Parameters.Add("pname", MySqlDbType.VarChar);
				
				foreach (DataRow Row in temp.Tables[0].Rows) {

													// Проверка на пустые поля
					if (
						Row["Название"] == DBNull.Value ||
						Row["Периодичность_мес"] == DBNull.Value
					) {
						MessageBox.Show("Вы оставили пустые поля, сохранение отменено");
						return false;
					}

													// Проверка значений
					if (
						!(
							CheckUtil.CheckStringLength(Row, "Название", 15) &&
							CheckUtil.CheckIntAndLength(Row, "Периодичность_мес", 1, 600)
						)
					)
						return false;
					
													// Проверка на уникальность
													// столбца "Название"
					if (!CheckUtil.CheckUniqueValue(tempAdapter, "pname", Data, Row, "Название")) {
						Connection.Close();
						MessageBox.Show("Название должно быть уникальным");
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

													// Выполнение запроса 
													// выборки
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
